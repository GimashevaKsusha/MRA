using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.VisualBasic;
using Newtonsoft.Json;

namespace MR.Dispatcher.Information
{
    public static class ServerInfoSerialize
    {
        public static ServerInfoList DeserializeServerInfo(string filePath)
        {
            try
            {
                string json = File.ReadAllText(filePath);
                var serverObj = JsonConvert.DeserializeObject<ServerInfoList>(json);
                return serverObj;
            }
            catch
            {
                return null;
            }
        }

        public static bool SerializeServerInfo(ServerInfoList obj, string filePath)
        {
            string json = JsonConvert.SerializeObject(obj);
            try
            {
                File.WriteAllText(filePath, json);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static ServerInfoList ReadFromFile()
        {
            string filename = "serversInfo.json";
            string filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
            var serverJson = DeserializeServerInfo(filepath);

            var result = new ServerInfoList();

            if (serverJson == null)
                return result;

            foreach(var server in serverJson.serverInfos)
            {
                result.serverInfos.Add(server);
            }

            return result;
        }
    }

    public class ServerInfoList
    {
        public List<ServerInfo> serverInfos { get; } = new();

        public void AddServer(string ip, string grpcPort, string socketPort)
        {
            string filename = "serversInfo.json";
            string filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
            ServerInfo elem = new ServerInfo
            {
                Ip = ip,
                GrpcPort = grpcPort,
                SocketPort = socketPort
            };
            serverInfos.Add(elem);
            ServerInfoSerialize.SerializeServerInfo(this, filepath);
        }

        public void DeleteServer(ServerInfo server)
        {
            string filename = "serversInfo.json";
            string filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
            serverInfos.Remove(server);
            ServerInfoSerialize.SerializeServerInfo(this, filepath);
        }
    }
    public class ServerInfo
    {
        public string Ip { get; init; } = null!;
        public string GrpcPort { get; init; } = null!;
        public string SocketPort { get; init; } = null!;
    }
}