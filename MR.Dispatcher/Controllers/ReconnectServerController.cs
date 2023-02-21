using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using MR.Dispatcher.Information;
using Rpc.Core;

namespace MR.Dispatcher.Controllers
{
    [Route("reconnect")]
    [ApiController]
    public class ReconnectServerController : ControllerBase
    {
        private ServerInfoList servers;
        public ReconnectServerController(ServerInfoList _servers)
        {
            servers = _servers;
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] AuthRequestConnect authRequest)
        {
            Console.WriteLine("INFO: Пришел запрос от клиента: " + authRequest.Hi + " to " + authRequest.UUID);

            //проверить, закрыта ли комната
            var access = CheckRoomUUID(authRequest.UUID);

            if (access == 0)
            {
                return Unauthorized(new { ip = "1", port = "1", UUID = "" });
            }

            if (access == 1)
            {
                return Ok(new { ip = "1", port = "1", UUID = "" });
            }

            ServerInfo bestServer = null;

            //поиск сервера, на котором может работать комната после переподключения

            //либо поиск наименее загруженного сервера
            //либо поиск сервера, на котором уже работает комната

            PriorityQueue<ServerInfo, int> counts = new PriorityQueue<ServerInfo, int>();

            //сервера на удаление
            List<ServerInfo> serverToRemove = new List<ServerInfo>();

            Console.WriteLine("INFO: Сейчас серверов " + servers.serverInfos.Count);
            foreach (var s in servers.serverInfos.ToList())
            {
                using (var channel = GrpcChannel.ForAddress($"http://{s.Ip}:{s.GrpcPort}"))
                {
                    var client = new ServiceConnection.ServiceConnectionClient(channel);
                    try
                    {
                        var info = await client.GetServerInfoAsync(
                            request: new InfoMessage { Info = authRequest.UUID },
                            deadline: DateTime.UtcNow.AddSeconds(1)
                            );

                        if (info.IsConnected)
                        {
                            bestServer = s;
                            break;
                        }

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

            foreach(var s in serverToRemove)
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
            if (bestServer == null && counts.Count == 0)
                return Unauthorized(new { ip = "", port = "", UUID = "1" });

            //выбрать наименее загруженный сервер
            if (bestServer == null)
                bestServer = counts.Dequeue();

            Console.WriteLine($"INFO: Подключаем клиента к серверу: ip - {bestServer.Ip}, port - {bestServer.SocketPort}");

            //вернуть клиенту ip, port, uuid
            return Ok(new { ip = $"{bestServer.Ip}", port = $"{bestServer.SocketPort}", UUID = $"{authRequest.UUID}" });
        }

        private int CheckRoomUUID(string UUID)
        {
            //проверить наличие комнаты
            var room = DBRoomWork.FindRoom(UUID);
            if (room == null) //не удалось подключиться к БД или комната не найдена
                return 0;

            if (!room.IsActive) //комната не активна
                return 1;

            return 2; //успешное подключение
        }
    }
}