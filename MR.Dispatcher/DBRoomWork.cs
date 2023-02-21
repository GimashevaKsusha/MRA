using MR.Dispatcher.Data;
using MR.Dispatcher.Data.Entities;

namespace MR.Dispatcher
{
    public class DBRoomWork
    {

        //создание комнаты в БД
        //вовзращает новый комнату (id, uuid, is_active)
        public static Room CreateRoom()
        {
            var room = new Room()
            {
                Uuid = Guid.NewGuid(),
                IsActive = true
            };
            try
            {
                using (var db = new TrrpMraContext())
                {
                    db.Rooms.Add(room);
                    db.SaveChanges();
                }
                return room;
            }
            catch
            {
                return null;
            }
        }

        //поиск комнаты в БД
        //вовзращает комнату (id, uuid, is_active) или null 
        public static Room FindRoom(string id) 
        {
            try
            {
                using (var db = new TrrpMraContext())
                {
                    return db.Rooms.FirstOrDefault(r => r.Uuid.ToString() == id);
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
