using MR.Server.Data.Entities;

namespace MR.Server.Information
{
    public static class ClientsInfo
    {
        //Tuple<int, bool, int> - идентификатор песни, включена/выключена, позиция в плейлисте
        public static Dictionary<string, Tuple<int, bool, int>> currentSongs = new Dictionary<string, Tuple<int, bool, int>>();

        //uuid комнаты - список клиентов connectionId 
        public static Dictionary<string, List<string>> clients = new Dictionary<string, List<string>>();

        public static string GetUuidByClient(string id)
        {
            foreach(var c in clients)
            {
                if (c.Value.Contains(id))
                    return c.Key;
            }
            return null;
        }
        public static int GetClientsCount()
        {
            int count = 0;
            foreach (var c in clients)
            {
                count += c.Value.Count;
            }
            return count;
        }
        public static void AddNewClient(string uuid, string id)
        {
            if (!clients.ContainsKey(uuid))
            {
                clients[uuid] = new List<string>();
                currentSongs[uuid] = new Tuple<int, bool, int>(-1, false, -1);
                Console.WriteLine($"UUID: {uuid}");
            }

            clients[uuid].Add(id);
        }

        public static bool DeleteClient(string uuid, string id)
        {
            clients[uuid].Remove(id);

            if (clients[uuid].Count == 0) //если последний клиент, то комнату надо закрыть
            {
                clients.Remove(uuid);
                currentSongs.Remove(uuid);
                return true;
            }

            return false;
        }

        public static void DeleteRoom(string uuid)
        {
            clients.Remove(uuid);
            currentSongs.Remove(uuid);
        }

        public static List<string> GetClientsExceptUuid(string uuid)
        {
            List<string> clientsId = new List<string>();
            foreach (var r in clients)
            {
                if (r.Key != uuid)
                    clientsId.AddRange(r.Value);
            }
            return clientsId;
        }
    }
    public class ServerInfo
    {
        public string Ip { get; init; }
        public string GrpcPort { get; init; }
        public string SocketPort { get; init; }

        public ServerInfo(string ip, string grpcPort, string socketPort)
        {
            Ip = ip;
            GrpcPort = grpcPort;
            SocketPort = socketPort;
        }
    }
}