using Microsoft.AspNetCore.SignalR;
using MR.Server.Data.Entities;
using MR.Server.Information;
using System;
using System.Runtime.InteropServices;

namespace MR.Server
{
    public class RoomHub : Hub
    {

        public async Task Authentication(string uuid)
        {
            ClientsInfo.AddNewClient(uuid, Context.ConnectionId);
            Console.WriteLine("INFO: Подключился новый клиент");
            Console.WriteLine($"INFO: Статистика сервера: комнаты - {ClientsInfo.clients.Count}, клиенты - {ClientsInfo.GetClientsCount()}");
            await Clients.Caller.SendAsync("ReceiveAuth", true);
        }

        public async Task GetAllSongs()
        {
            var songs = DBPlaylistWork.GetAllSongs();
            await Clients.Caller.SendAsync("ReceiveAllSongs", songs);
        }

        public async Task AddNewSong(string uuid, int sid)
        {
            DBPlaylistWork.AddSongToPlaylist(uuid, sid);
            var songs = DBPlaylistWork.GetAllPlaylistSong(uuid);
            var clientsId = ClientsInfo.GetClientsExceptUuid(uuid);
            await Clients.AllExcept(clientsId).SendAsync("ReceivePlaylistSongs", songs);
        }

        public async Task DeleteSong(string uuid, int sid, int num)
        {
            var song = ClientsInfo.currentSongs[uuid];
            if (song.Item1 != -1 && song.Item3 > num)
                ClientsInfo.currentSongs[uuid] = new Tuple<int, bool, int>(song.Item1, song.Item2, song.Item3 - 1);

            DBPlaylistWork.DeleteSongFromPlaylist(uuid, sid, num);
            var songs = DBPlaylistWork.GetAllPlaylistSong(uuid);
            var clientsId = ClientsInfo.GetClientsExceptUuid(uuid);
            await Clients.AllExcept(clientsId).SendAsync("ReceivePlaylistSongs", songs);
        }

        public async Task GetPlaylistSong(string uuid)
        {
            var songs = DBPlaylistWork.GetAllPlaylistSong(uuid);
            await Clients.Caller.SendAsync("ReceivePlaylistSongs", songs);
        }
        public async Task RoomExit(string uuid)
        {
            bool isLast = ClientsInfo.DeleteClient(uuid, Context.ConnectionId);
            if (isLast)
                DBPlaylistWork.CloseRoom(uuid);
            Console.WriteLine("INFO: Отключился клиент");
            Console.WriteLine($"INFO: Статистика сервера: комнаты - {ClientsInfo.clients.Count}, клиенты - {ClientsInfo.GetClientsCount()}");
            await Clients.Caller.SendAsync("ClientRoomExit");
        }

        public async Task RoomClose(string uuid)
        {
            DBPlaylistWork.CloseRoom(uuid);
            var clientsId = ClientsInfo.GetClientsExceptUuid(uuid);
            clientsId.Add(Context.ConnectionId);
            ClientsInfo.DeleteRoom(uuid);
            Console.WriteLine("INFO: Закрыли комнату");
            Console.WriteLine($"INFO: Статистика сервера: комнаты - {ClientsInfo.clients.Count}, клиенты - {ClientsInfo.GetClientsCount()}");
            await Clients.AllExcept(clientsId).SendAsync("ReceiveForceClose");
        }

        public async Task GetCurrentPlaylistState(string uuid)
        {
            var song = ClientsInfo.currentSongs[uuid];
            string title = null;
            if (song.Item1 != -1)
                title = DBPlaylistWork.GetSongTitle(song.Item1);
            await Clients.Caller.SendAsync("ReceiveCurrentState", title, song.Item2, song.Item3);
        }

        public async Task ChangeCurrentPlaylistState(string uuid)
        {
            var song = ClientsInfo.currentSongs[uuid];
            if (song.Item1 == -1)
            {
                Playlist curSong = DBPlaylistWork.FindNextSong(uuid, 0);
                ClientsInfo.currentSongs[uuid] = new Tuple<int, bool, int>(curSong.SongId, true, 1);
            }
            else
            {
                ClientsInfo.currentSongs[uuid] = new Tuple<int, bool, int>(song.Item1, !song.Item2, song.Item3);
            }
            var res = ClientsInfo.currentSongs[uuid];
            string title = DBPlaylistWork.GetSongTitle(res.Item1);
            var clientsId = ClientsInfo.GetClientsExceptUuid(uuid);
            await Clients.AllExcept(clientsId).SendAsync("ReceiveCurrentState", title, res.Item2, res.Item3);

        }

        //true - следующая песня
        //false - предыдущая песня
        public async Task GetPrevOrNextSong(bool flag, string uuid, string title, bool state, int num)
        {
            var curSong = ClientsInfo.currentSongs[uuid];
            int id = DBPlaylistWork.GetSongId(title);

            if (curSong.Item1 != id || curSong.Item2 != state || curSong.Item3 != num)
                ClientsInfo.currentSongs[uuid] = new Tuple<int, bool, int>(id, state, num);

            var song = ClientsInfo.currentSongs[uuid];

            if (flag)
            {
                Playlist nextSong = DBPlaylistWork.FindNextSong(uuid, song.Item3);
                ClientsInfo.currentSongs[uuid] = new Tuple<int, bool, int>(nextSong.SongId, song.Item2, nextSong.Number);
            }
            else
            {
                Playlist prevSong = DBPlaylistWork.FindPrevSong(uuid, song.Item3);
                ClientsInfo.currentSongs[uuid] = new Tuple<int, bool, int>(prevSong.SongId, song.Item2, prevSong.Number);
            }

            var res = ClientsInfo.currentSongs[uuid];
            string title1 = DBPlaylistWork.GetSongTitle(res.Item1);
            var clientsId = ClientsInfo.GetClientsExceptUuid(uuid);
            await Clients.AllExcept(clientsId).SendAsync("ReceiveCurrentState", title1, res.Item2, res.Item3);

        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {

            var uuid = ClientsInfo.GetUuidByClient(Context.ConnectionId);
            RoomExit(uuid);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
