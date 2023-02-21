
namespace MR.Client
{
    public class Song
    {
        public int id { get; private set; }
        public string title { get; private set; }
        public DateTime length { get; private set; }
        public int? num { get; private set; }

        public Song(int _id, string _title, DateTime _length)
        {
            id = _id;
            title = _title;
            length = _length;
        }

        public Song(int _id, string _title, DateTime _length, int _num)
        {
            id = _id;
            title = _title;
            length = _length;
            num = _num;
        }

    }
}
