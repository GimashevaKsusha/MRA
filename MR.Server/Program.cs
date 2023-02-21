using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting.Server;
using Grpc.Net.Client;
using Rpc.Core;
using MR.Server.Information;

namespace MR.Server
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddGrpc();

            builder.Services.AddSignalR((obj) => obj.EnableDetailedErrors = true);

            builder.Services.AddMvc().AddControllersAsServices();

            var app = builder.Build();

            app.MapGrpcService<GRPCConnection>();

            app.UseRouting();

            var roomPath = builder.Configuration["RoomPath"];
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<RoomHub>(roomPath);
            });

            app.Start();

            string dispatcher = builder.Configuration["Dispatcher"];

            IServer server = app.Services.GetService<IServer>();
            var features = server.Features.Get<IServerAddressesFeature>();
            List<string> ports = new List<string>();
            foreach(var f in features.Addresses)
            {
                ports.Add(GetPort(f));
            }

            if (ports.Count != 2)
            {
                await app.StopAsync();
                Console.WriteLine("Что-то пошло не так...");
                Console.ReadKey();
            }

            if (!await ConnectToDispatcher(dispatcher, ports[0], ports[1]))
            {
                await app.StopAsync();
                Console.WriteLine("Что-то пошло не так...");
                Console.ReadKey();
            }

            Console.WriteLine("INFO: Подключился к диспетчеру");

            await app.WaitForShutdownAsync();
        }

        public static async Task<bool> ConnectToDispatcher(string dispatcher, string _GrpcPort, string _SocketPort)
        {
            using (var channel = GrpcChannel.ForAddress(dispatcher))
            {
                var client = new ServiceConnection.ServiceConnectionClient(channel);

                try
                {
                    var ping = await client.PingServerAsync(
                        new PingMessage
                        {
                            GrpcPort = _GrpcPort,
                            SockerPort = _SocketPort
                        }
                    );

                    await channel.ShutdownAsync().WaitAsync(TimeSpan.FromSeconds(20.0));
                    return true;
                }
                catch
                {
                    Console.WriteLine("Не удалось подключиться к диспетчеру");
                }

                return false;
            }
        }

        private static string GetPort(string url)
        {
            var elements = url.Split(":").ToList();
            if (elements.Count != 3)
                return "";

            return elements[2];
        }
    }
}