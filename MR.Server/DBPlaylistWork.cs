using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;
using MR.Server.Data;
using MR.Server.Data.Entities;
using System;
using System.Diagnostics.Eventing.Reader;
using System.Security.Cryptography;

namespace MR.Server
{
    public class DBPlaylistWork
    {
        public static string GetSongTitle(int id)
        {
            try
            {
                using (var db = new TrrpMraContext())
                {
                    return db.Songs.First(x => x.Id == id).Title;
                }
            }
            catch
            {
                return null;
            }
        }

        public static int GetSongId(string title)
        {
            try
            {
                using (var db = new TrrpMraContext())
                {
                    return db.Songs.First(x => x.Title == title).Id;
                }
            }
            catch
            {
                return -1;
            }
        }

        //поиск комнаты в БД
        //возвращает комнату (id, uuid, is_active)
        public static Room FindRoom(string uuid)
        {
            try
            {
                using (var db = new TrrpMraContext())
                {
                    return db.Rooms.FirstOrDefault(r => r.Uuid.ToString() == uuid);
                }
            }
            catch
            {
                return null;
            }
        }

        //получить список всех песен
        public static List<string> GetAllSongs()
        {
            List<string> songsList = new List<string>();
            try
            {
                using (var db = new TrrpMraContext())
                {
                    var songs = db.Songs.ToList();
                    foreach(var s in songs)
                    {
                        songsList.Add($"{s.Id}|{s.Title}|{s.Length}");
                    }
                }
            }
            catch
            {
                return null;
            }
            return songsList;
        }

        //получить весь плейлист комнаты
        public static List<string> GetAllPlaylistSong(string  uuid)
        {
            int? rid = FindRoom(uuid).Id;
            if (rid == null) return null;

            List<string> songsList = new List<string>();
            try
            {
                using (var db = new TrrpMraContext())
                {
                    var psongs = db.Playlists.Where(p => p.RoomId == rid).OrderBy(p => p.Number).ToList();
                    foreach (var p in psongs)
                    {
                        var s = db.Songs.First(x => x.Id == p.SongId);
                        songsList.Add($"{s.Id}|{s.Title}|{s.Length.ToString("mm:ss")}|{p.Number}");
                    }
                }
            }
            catch
            {
                return null;
            }
            return songsList;
        }

        //добавить конкретную песню в конкретный плейлист (комнату)
        public static bool AddSongToPlaylist(string uuid, int sid)
        {
            int? rid = FindRoom(uuid).Id;
            if (rid == null) return false;
            try
            {
                using (var db = new TrrpMraContext())
                {
                    int num = 0;
                    if (db.Playlists.Where(p => p.RoomId == rid).Count() != 0)
                    {
                        num = db.Playlists.Where(p => p.RoomId == rid).Max(p => p.Number);
                    }
                    var note = new Playlist()
                    {
                        RoomId = (int)rid,
                        SongId = sid,
                        Number = num + 1
                    };
                    db.Playlists.Add(note);
                    db.SaveChanges();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        //удалить конкретную песню из конкретного плейлиста (комнаты)
        public static bool DeleteSongFromPlaylist(string uuid, int sid, int num)
        {
            int? rid = FindRoom(uuid).Id;
            if (rid == null) return false;
            try
            {
                using (var db = new TrrpMraContext())
                {
                    var song = db.Playlists.FirstOrDefault(p => p.RoomId == rid && p.SongId == sid && p.Number == num);
                    if (song == null) return false;
                    db.Playlists.Remove(song);
                    db.SaveChanges();
                    var list = db.Playlists.Where(p => p.RoomId == rid && p.Number > num).OrderBy(p => p.Number).ToList();
                    foreach (var p in list)
                    {
                        db.Playlists.Attach(p);
                        p.Number = p.Number - 1;
                        db.SaveChanges();
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        //найти следующую песню в плейлисте
        //для изменения текущей песни 
        //для переключения на следующую песню по запросу клиента
        public static Playlist FindNextSong(string uuid, int num)
        {
            int? rid = FindRoom(uuid).Id;
            if (rid == null) return null;
            try
            {
                using (var db = new TrrpMraContext())
                {
                    var p = db.Playlists.FirstOrDefault(p => p.RoomId == rid && p.Number == num + 1);
                    if (p == null)
                        return db.Playlists.FirstOrDefault(p => p.RoomId == rid && p.Number == 1);
                    else
                        return p;
                }
            }
            catch
            {
                return null;
            }
        }

        //найти предыдущую песню в плейлисте
        //для переключения на предыдущую песню по запросу клиента 
        public static Playlist FindPrevSong(string uuid, int num)
        {
            int? rid = FindRoom(uuid).Id;
            if (rid == null) return null;
            try
            {
                using (var db = new TrrpMraContext())
                {
                    var p = db.Playlists.FirstOrDefault(p => p.RoomId == rid && p.Number == num - 1);
                    if (p == null)
                    {
                        int maxNum = db.Playlists.Where(p => p.RoomId == rid).Max(p => p.Number);
                        return db.Playlists.FirstOrDefault(p => p.RoomId == rid && p.Number == maxNum);
                    }
                    else
                        return p;
                }
            }
            catch
            {
                return null;
            }
        }

        //закрыть комнату
        public static bool CloseRoom(string uuid)
        {
            var room = FindRoom(uuid);
            if (room == null) return false;
            try
            {
                using (var db = new TrrpMraContext())
                {
                    db.Rooms.Attach(room);
                    room.IsActive = false;
                    db.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

