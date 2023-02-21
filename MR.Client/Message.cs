namespace MR.Client
{
    internal class Message //от диспетчера клиенту отправляется ip и port сервера и uuid комнаты
    {
        public string ip;
        public string port;
        public string UUID;
    }
}
