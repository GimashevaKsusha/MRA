using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using MR.Dispatcher.Information;
using Rpc.Core;

namespace MR.Dispatcher.Controllers
{
    [Route("create")]
    [ApiController]
    public class CreateNewRoomController : ControllerBase
    {
        private ServerInfoList servers;
        public CreateNewRoomController(ServerInfoList _servers)
        {
            servers = _servers;
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] AuthRequestCreate authRequest)
        {
            Console.WriteLine("INFO: Пришел запрос от клиента: " + authRequest.Hi);

            //запрос ко всем серверам на информацию о загруженности
            //сервера возвращают количество обслуживаемых клиент

            PriorityQueue<ServerInfo, int> counts = new PriorityQueue<ServerInfo, int>();

            //сервера на удаление
            List<ServerInfo> serverToRemove = new List<ServerInfo>();

            foreach (var s in servers.serverInfos.ToList())
            {
                using (var channel = GrpcChannel.ForAddress($"http://{s.Ip}:{s.GrpcPort}"))
                {
                    var client = new ServiceConnection.ServiceConnectionClient(channel);
                    try
                    {
                        var info = await client.GetServerInfoAsync(
                            request: new InfoMessage { Info = "Hi" },
                            deadline: DateTime.UtcNow.AddSeconds(1)
                            );

                        counts.Enqueue(s, info.ClientsNumber);

                        await channel.ShutdownAsync().WaitAsync(TimeSpan.FromSeconds(20.0));
                    }
                    catch (RpcException ex)
                    {
                        Console.WriteLine(ex.Message);
                        serverToRemove.Add(s);
                    }
                }
            }

            foreach (var s in serverToRemove)
            {
                try
                {
                    servers.DeleteServer(s);
                }
                catch
                {
                    Console.WriteLine("INFO: Не удалось удалить сервер из списка");
                }
            }

            //обработать ситуацию отсутствия серверов (нет ответа от серверов)
            if (counts.Count == 0)
                return Unauthorized(new { ip = "", port = "", UUID = "1" });

            //выбрать наименее загруженный сервер
            var bestServer = counts.Dequeue();

            //создать комнату в бд
            string uuid = CreateNewRoomInDB();

            if (uuid == null) //не удалилось создать комнату в БД
            {
                return Unauthorized(new { ip = $"{bestServer.Ip}", port = $"{bestServer.SocketPort}", UUID = "" });
            }

            Console.WriteLine($"INFO: Подключаем клиента к серверу: ip - {bestServer.Ip}, port - {bestServer.SocketPort}");
            //вернуть клиенту ip, port, uuid
            return Ok(new { ip = $"{bestServer.Ip}", port = $"{bestServer.SocketPort}", UUID = $"{uuid}" });
        }

        private string CreateNewRoomInDB()
        {
            //создать комнату
            var room = DBRoomWork.CreateRoom();
            if (room != null) 
                return room.Uuid.ToString();
            else 
                return null;
        }
    }
}

