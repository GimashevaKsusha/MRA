using MR.Dispatcher.Information;
using MR.Dispatcher.Controllers;
using Microsoft.VisualBasic;

namespace MR.Dispatcher
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddGrpc();

            builder.Services.AddMvc().AddControllersAsServices();

            builder.Services.AddSingleton(ServerInfoSerialize.ReadFromFile());

            builder.Services.AddSingleton<ConnectToRoomController, ConnectToRoomController>();
            builder.Services.AddSingleton<CreateNewRoomController, CreateNewRoomController>();
            builder.Services.AddSingleton<ReconnectServerController, ReconnectServerController>();

            var app = builder.Build();

            app.MapGrpcService<GRPCConnection>().RequireHost("*:51000");

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Run();
        }
    }
}