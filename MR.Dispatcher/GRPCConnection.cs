using Grpc.Core;
using MR.Dispatcher.Information;
using Rpc.Core;
using System.Net.Sockets;
using System.Net;

namespace MR.Dispatcher
{
    public class GRPCConnection : ServiceConnection.ServiceConnectionBase
    {
        private ServerInfoList servers;

        public GRPCConnection(ServerInfoList _servers)
        {
            servers = _servers;
        }

        public override Task<PingReply> PingServer(PingMessage request, ServerCallContext context)
        {
            var address = context.Peer.Split(":").ToList()[1];

            if (address == "127.0.0.1" || address == "localhost")
            {
                string localAddress = GetLocalAddress();

                if (string.IsNullOrEmpty(localAddress))
                    return Task.FromResult(new PingReply { IsSuccess = false });

                address = localAddress;
            }

            servers.AddServer(address, request.GrpcPort, request.SockerPort);
            Console.WriteLine($"INFO: Подключился новый сервер: ip - {address}, grpc - {request.GrpcPort}, socket - {request.SockerPort}");

            return Task.FromResult(new PingReply { IsSuccess = true });
        }

        public static string GetLocalAddress()
        {
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint? endPoint = socket.LocalEndPoint as IPEndPoint;
                return endPoint != null ? endPoint.Address.ToString() : string.Empty;
            }
        }

    }
}