using System.Text;
using Newtonsoft.Json;
using System.Configuration;

namespace MR.Client
{
    internal class AuthClient
    {
        private static HttpClient _httpClient;
        private static string _dispatcherAddress;

        static AuthClient()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
            _dispatcherAddress = ConfigurationSettings.AppSettings.Get("Dispatcher");
        }

        public static async Task<(int, string, string, string)> CreateNewRoom(string hi) //отправить запрос на создание новой комнаты
        {
            var json = $"{{\"Hi\": \"{hi}\"}}";
            StringContent postContent = new(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync(_dispatcherAddress + "/create", postContent);

                //ip и port сервера
                var ip = JsonConvert.DeserializeObject<Message>(await response.Content.ReadAsStringAsync()).ip;
                var port = JsonConvert.DeserializeObject<Message>(await response.Content.ReadAsStringAsync()).port;
                var UUID = JsonConvert.DeserializeObject<Message>(await response.Content.ReadAsStringAsync()).UUID;

                var statusCode = (int)response.StatusCode;
                return (statusCode, ip, port, UUID);
            }
            catch
            {
                throw;
            }
        }

        public static async Task<(int, string, string, string)> ConnectToRoom(string hi, string UUID) //отправить запрос на подключении к комнате
        {
            var json = $"{{\"Hi\": \"{hi}\", \"UUID\": \"{UUID}\"}}";
            StringContent postContent = new(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync(_dispatcherAddress + "/connect", postContent);

                //ip и port сервера
                var ip = JsonConvert.DeserializeObject<Message>(await response.Content.ReadAsStringAsync()).ip;
                var port = JsonConvert.DeserializeObject<Message>(await response.Content.ReadAsStringAsync()).port;
                var UUID1 = JsonConvert.DeserializeObject<Message>(await response.Content.ReadAsStringAsync()).UUID;

                var statusCode = (int)response.StatusCode;
                return (statusCode, ip, port, UUID1);
            }
            catch
            {
                throw;
            }
        }

        public static async Task<(int, string, string, string)> ReconnectToServer(string hi, string UUID) //отправить запрос на переподключение
        {
            var json = $"{{\"Hi\": \"{hi}\", \"UUID\": \"{UUID}\"}}";
            StringContent postContent = new(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync(_dispatcherAddress + "/reconnect", postContent);

                //ip и port сервера
                var ip = JsonConvert.DeserializeObject<Message>(await response.Content.ReadAsStringAsync()).ip;
                var port = JsonConvert.DeserializeObject<Message>(await response.Content.ReadAsStringAsync()).port;
                var UUID1 = JsonConvert.DeserializeObject<Message>(await response.Content.ReadAsStringAsync()).UUID;

                var statusCode = (int)response.StatusCode;
                return (statusCode, ip, port, UUID1);
            }
            catch
            {
                throw;
            }
        }
    }
}
