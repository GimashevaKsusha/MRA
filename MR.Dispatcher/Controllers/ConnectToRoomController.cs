using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using MR.Dispatcher.Information;
using Rpc.Core;
using System.Net;

namespace MR.Dispatcher.Controllers
{
    [Route("connect")]
    [ApiController]
    public class ConnectToRoomController : ControllerBase
    {
        private ServerInfoList servers;
        public ConnectToRoomController(ServerInfoList _servers) 
        { 
            servers= _servers;
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] AuthRequestConnect authRequest)
        {
            Console.WriteLine("INFO: Пришел запрос от клиента: " + authRequest.Hi + " to " + authRequest.UUID);

            //проверка, есть ли такая комната
            var access = CheckRoomUUID(authRequest.UUID);

            if (access == 0)
            {
                return Unauthorized(new { ip = "1", port = "1", UUID = "" });
            }

            if (access == 1)
            {
                return Ok(new { ip = "1", port = "1", UUID = "" });
            }

            //поиск сервера, на котором работает комната

            ServerInfo bestServer = null;

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
                            request: new InfoMessage { Info = authRequest.UUID },
                            deadline: DateTime.UtcNow.AddSeconds(1)
                            );

                        if (info.IsConnected)
                        {
                            bestServer = s;
                            break;
                        }

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

            if (bestServer == null && access == 2)
            {
                DBRoomWork.CloseRoom(authRequest.UUID);
                return Ok(new { ip = "1", port = "1", UUID = "" });
            }

            //обработать ситуацию отсутствия серверов (нет ответа от серверов)
            if (bestServer == null)
                return Unauthorized(new { ip = "", port = "", UUID = $"{authRequest.UUID}" });

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

