using Grpc.Core;
using MR.Server.Information;
using Rpc.Core;


namespace MR.Server
{
    public class GRPCConnection : ServiceConnection.ServiceConnectionBase
    {
        public override Task<InfoReply> GetServerInfo(InfoMessage request, ServerCallContext context)
        {
            if (request.Info == "Hi")
            {
                var answer1 = new InfoReply
                {
                    ClientsNumber = ClientsInfo.GetClientsCount(),
                    IsConnected = true
                };
                Console.WriteLine($"INFO: Статистика сервера: комнаты - {ClientsInfo.clients.Count}, клиенты - {ClientsInfo.GetClientsCount()}");
                return Task.FromResult(answer1);
            }
            else
            {
                var answer2 = new InfoReply
                {
                    ClientsNumber = ClientsInfo.GetClientsCount(),
                    IsConnected = ClientsInfo.clients.ContainsKey(request.Info)
                };
                Console.WriteLine($"INFO: Комната {request.Info} работает на данном сервере?  " + ClientsInfo.clients.ContainsKey(request.Info).ToString());
                return Task.FromResult(answer2);
            }  
        }
    }
}

