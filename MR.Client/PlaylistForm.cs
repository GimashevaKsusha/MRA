using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.VisualBasic.ApplicationServices;
using System.Data.Common;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MR.Client
{
    public partial class PlaylistForm : Form
    {
        private string uuid;
        private string serverIp;
        private string serverPort;
        private HubConnection hubConnection;
        private List<Song> songs;
        private bool forceClosed;
        private bool disconnect;

        public PlaylistForm(int type, string UUID, string ip, string port)
        {
            InitializeComponent();
            songs = new List<Song>();
            uuid = UUID;
            закрытьКомнатуToolStripMenuItem.Visible = type == 1;
            выйтиToolStripMenuItem.Visible = type == 2;
            Text = uuid;
            serverIp = ip;
            serverPort = port;
            forceClosed = false;
            disconnect = false;
        }

        private async void PlaylistForm_Load(object sender, EventArgs e)
        {
            var isConnected = await ConnectServer();
            if (!isConnected)
            {
                await ReconnectServer();
                ReceiveAllSongs();
                GetPlaylistSong();
                GetCurrentState();
                return;
            }
            ReceiveAllSongs();
            GetPlaylistSong();
            GetCurrentState();
        }

        private async Task ReconnectServer()
        {
            //запрос к диспетчеру, чтобы найти новый сервер
            (int code, string ip, string port, string UUID) response;

            try
            {
                response = await AuthClient.ReconnectToServer("reconnect", uuid);
                await Task.Delay(TimeSpan.FromMilliseconds(3000));
                switch (response.code)
                {
                    //не удалось найти комнату
                    case 401 when string.IsNullOrEmpty(response.UUID):
                        MessageBox.Show("Подключение с серверов было прервано, не удалось найти комнату", "Ошибка!", MessageBoxButtons.OK);
                        disconnect = true;
                        break;
                    //нет доступных серверов
                    case 401 when string.IsNullOrEmpty(response.port):
                        MessageBox.Show("Подключение с серверов было прервано, нет доступных серверов", "Ошибка!", MessageBoxButtons.OK);
                        disconnect = true;
                        break;
                    //комната не активная
                    case 200 when string.IsNullOrEmpty(response.UUID):
                        MessageBox.Show("Подключение с серверов было прервано, к данной комнате невозможно подключиться, так как она не активна", "Ошибка!", MessageBoxButtons.OK);
                        disconnect = true;
                        break;
                    //успешное подключение
                    case 200:
                        serverIp = response.ip;
                        serverPort = response.port;
                        break;
                    default:
                        MessageBox.Show("Подключение с серверов было прервано, что-то пошло не так...", "Ошибка!");
                        disconnect = true;
                        break;
                }
            }
            catch
            {
                //не смогли подключиться к диспетчеру
                MessageBox.Show("Подключение с серверов было прервано, невозможно подключиться к диспетчеру", "Ошибка!", MessageBoxButtons.OK);
                disconnect = true;
            }

            if (disconnect)
            {
                Close();
                return;
            }

            var isConnected = await ConnectServer();
            if (!isConnected)
            {
                MessageBox.Show("Подключение с серверов было прервано, не удалось подключиться к серверу", "Ошибка!");
                Close();
                return;
            }
            Enabled = true;
            Text = uuid;
        }

        private async Task<bool> ConnectServer()
        {
            hubConnection = new HubConnectionBuilder().WithUrl($"http://{serverIp}:{serverPort}/roomsHub").Build();

            ReceiveFromServer();

            var token = new CancellationTokenSource();
            token.CancelAfter(TimeSpan.FromSeconds(30));

            try
            {
                await hubConnection.StartAsync(token.Token);
                await hubConnection.InvokeAsync("Authentication", uuid);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void ReceiveFromServer()
        {
            hubConnection.On<List<string>>("ReceiveAllSongs", (_songs) =>
            {
                Invoke(new Action(() => SetSongs(_songs)));
            });

            hubConnection.On<List<string>>("ReceivePlaylistSongs", (psongs) =>
            {
                Invoke(new Action(() => SetPlaylist(psongs)));
            });

            hubConnection.On("ReceiveForceClose", () =>
            {
                Invoke(new Action(() => ForceClose()));
            });

            hubConnection.On<string, bool, int>("ReceiveCurrentState", (curSong, curState, pos) =>
            {
                Invoke(new Action(() => SetCurrentState(curSong, curState, pos)));
            });

            hubConnection.Closed += async (error) =>
            {
                if (error is null)
                {
                    return;
                }

                Text = "Соединение с сервером прервано, попытка переподключения...";
                Enabled = false;

                await ReconnectServer();

                Enabled = true;
                Text = uuid;
            };
        }

        private void SetCurrentState(string curSong, bool curState, int pos)
        {
            if (curSong == null)
                return;

            foreach (ListViewItem item in lvPlaylist.Items)
                item.BackColor = Color.White;

            tbCurSong.Text = curSong;
            lvPlaylist.Items[pos - 1].BackColor = Color.Thistle;
            btnPlay.Text = curState ? "Pause" : "Play";

        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (lvPlaylist.Items.Count == 0 || tbCurSong.Text == "")
                return;

            GetPrevOrNextSong(false);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (lvPlaylist.Items.Count == 0 || tbCurSong.Text == "")
                return;

            GetPrevOrNextSong(true);
        }

        private async void GetPrevOrNextSong(bool flag)
        {
            var title = tbCurSong.Text;
            var state = btnPlay.Text == "Play";
            int num = -1;
            if (title != "")
            {
                foreach (ListViewItem item in lvPlaylist.Items)
                {
                    if (item.BackColor == Color.Thistle)
                    {
                        num = lvPlaylist.Items.IndexOf(item);
                        break;
                    }
                }
            }

            try
            {
                await hubConnection.InvokeAsync("GetPrevOrNextSong", flag, uuid, title, !state, num + 1);
            }
            catch
            {
                //переподключение
                return;
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (lvPlaylist.Items.Count == 0)
                return;

            ChangeCurrentState();
        }

        private async void ChangeCurrentState()
        {
            try
            {
                await hubConnection.InvokeAsync("ChangeCurrentPlaylistState", uuid);
            }
            catch
            {
                //переподключение
                return;
            }
        }

        private async void GetCurrentState()
        {
            try
            {
                await hubConnection.InvokeAsync("GetCurrentPlaylistState", uuid);
            }
            catch
            {
                //переподключение
                return;
            }
        }

        private async void GetPlaylistSong()
        {
            try
            {
                await hubConnection.InvokeAsync("GetPlaylistSong", uuid);
            }
            catch
            {
                //переподключение
                return;
            }

        }

        private async void ReceiveAllSongs()
        {
            try
            {
                await hubConnection.InvokeAsync("GetAllSongs");
            }
            catch
            {
                //переподключение
                return;
            }
        }
        
        private void ForceClose()
        {
            forceClosed = true;
            Close();
        }

        private void SetSongs(List<string> _songs)
        {
            songs.Clear();
            foreach (var s in _songs)
            {
                var arr = s.Split("|");
                songs.Add(new Song(int.Parse(arr[0]), arr[1], DateTime.Parse(arr[2])));
            }
        }

        private void SetPlaylist(List<string> _songs)
        {
            lvPlaylist.Items.Clear();
            foreach (var s in _songs)
            {
                var arr = s.Split("|");
                var item = new ListViewItem($"{arr[3]}. {arr[1]}   |   {arr[2]}");
                item.Tag = new Tuple<int, int>(int.Parse(arr[0]), int.Parse(arr[3]));
                lvPlaylist.Items.Add(item);
            }
            GetCurrentState();
        }

        private void выйтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            forceClosed = false;
            Close();
        }

        private void закрытьКомнатуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            forceClosed = false;
            Close();
        }

        private void добавитьПеснюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (songs.Count == 0)
            {
                MessageBox.Show("Произошла ошибка! Повторите попытку еще раз!", "Ошибка!");
                return;
            }
            AddSongForm aform = new AddSongForm(songs);
            var result = aform.ShowDialog();
            if (result == DialogResult.OK)
            {
                int songId = aform.selectedSongId;
                AddSongToPlaylist(songId);
            }
        }

        private async void AddSongToPlaylist(int id)
        {
            try
            {
                await hubConnection.InvokeAsync("AddNewSong", uuid, id);
            }
            catch
            {
                //переподключение
                return;
            }
        }

        private void lvPlaylist_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (lvPlaylist.SelectedItems.Count != 1 || lvPlaylist.Items[lvPlaylist.SelectedItems[0].Index].BackColor == Color.Thistle)
                удалитьПеснюToolStripMenuItem.Enabled = false;
            else
                удалитьПеснюToolStripMenuItem.Enabled = true;
        }

        private void PlaylistForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (disconnect) return;
            if (!forceClosed)
            {
                if (закрытьКомнатуToolStripMenuItem.Visible) //при закрытии формы комната закрывается у всех (создавший комнату, кнопки "Выйти" нет)
                {
                    RoomClose();
                }
                else //при закрытии формы комната остается активной (подключившийся к комнате, кнопки "Закрыть" нет)
                {
                    RoomExit();
                }
            }
            else
            {
                MessageBox.Show("Пользователь, создавший комнату, закрыл сессию", "Предупреждение!");
            }
        }

        private async void RoomExit()
        {
            try
            {
                await hubConnection.InvokeAsync("RoomExit", uuid);
            }
            catch
            {
                //переподключение
                return;
            }
        }

        private async void RoomClose()
        {
            try
            {
                await hubConnection.InvokeAsync("RoomClose", uuid);
            }
            catch
            {
                //переподключение
                return;
            }
        }

        private void скопироватьIDКомнатыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(uuid);
        }

        private void удалитьПеснюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tuple<int, int> tag = (Tuple<int, int>)lvPlaylist.SelectedItems[0].Tag;
            DeleteSongFromPlaylist(tag.Item1, tag.Item2);   
        }

        private async void DeleteSongFromPlaylist(int id, int num)
        {
            try
            {
                await hubConnection.InvokeAsync("DeleteSong", uuid, id, num);
            }
            catch
            {
                //переподключение
                return;
            }
        }
    }
}
