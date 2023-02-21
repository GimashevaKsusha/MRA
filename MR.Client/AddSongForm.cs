using Microsoft.AspNetCore.SignalR.Client;
using System.Data.Common;

namespace MR.Client
{
    public partial class AddSongForm : Form
    {
        private List<Song> songs;
        public int selectedSongId { get; private set; }
        public AddSongForm(List<Song> _songs)
        {
            InitializeComponent();
            songs = _songs;
            SetSongsToComboBox();
        }

        private void SetSongsToComboBox()
        {
            foreach(var s in songs)
            {
                cbSongs.Items.Add(s.title);
            }
            cbSongs.SelectedIndex = 0;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var item = cbSongs.SelectedItem.ToString();
            var song = songs.FirstOrDefault(s => s.title == item);
            selectedSongId = song.id;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
