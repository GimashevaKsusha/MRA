namespace MR.Client
{
    public partial class AuthForm : Form
    {
        private string serverIp;
        private string serverPort;
        public AuthForm()
        {
            InitializeComponent();
        }

        private async void btnCreate_Click(object sender, EventArgs e)
        {
            (int code, string ip, string port, string UUID) response;

            try
            {
                response = await AuthClient.CreateNewRoom("create");
                switch (response.code)
                {
                    //не удалось создать новую комнату
                    case 401 when string.IsNullOrEmpty(response.UUID):
                        MessageBox.Show("Не удалось создать новую комнату", "Ошибка!", MessageBoxButtons.OK);
                        break;
                    //нет доступных серверов
                    case 401 when string.IsNullOrEmpty(response.port):
                        MessageBox.Show("Нет доступных серверов", "Ошибка!", MessageBoxButtons.OK);
                        break;
                    //успешное подключение
                    case 200:
                        serverIp = response.ip;
                        serverPort = response.port;
                        OpenRoom(1, response.UUID);
                        break;
                    default:
                        MessageBox.Show("Что-то пошло не так...", "Ошибка!");
                        break;
                }
            }
            catch
            {
                //не смогли подключиться к диспетчеру
                MessageBox.Show("Невозможно подключиться к диспетчеру", "Ошибка!", MessageBoxButtons.OK);
            }
        }

        private void btnConnect1_Click(object sender, EventArgs e)
        {
            gbRoomId.Visible = true;
            btnConnect1.Enabled = false;
        }

        private async void btnConnect2_Click(object sender, EventArgs e)
        {
            (int code, string ip, string port, string UUID) response;

            try
            {
                response = await AuthClient.ConnectToRoom("connect", tbRoomId.Text);
                switch (response.code)
                {
                    //не удалось найти комнату
                    case 401 when string.IsNullOrEmpty(response.UUID):
                        MessageBox.Show("Не удалось найти комнату", "Ошибка!", MessageBoxButtons.OK);
                        tbRoomId.Text = "";
                        break;
                    //нет доступных серверов
                    case 401 when string.IsNullOrEmpty(response.port):
                        MessageBox.Show("Нет доступных серверов", "Ошибка!", MessageBoxButtons.OK);
                        tbRoomId.Text = "";
                        break;
                    //комната не активная
                    case 200 when string.IsNullOrEmpty(response.UUID):
                        MessageBox.Show("К данной комнате невозможно подключиться, так как она не активна", "Ошибка!", MessageBoxButtons.OK);
                        tbRoomId.Text = "";
                        break;
                    //успешное подключение
                    case 200:
                        serverIp = response.ip;
                        serverPort = response.port;
                        OpenRoom(2, response.UUID);
                        break;
                    default:
                        MessageBox.Show("Что-то пошло не так...", "Ошибка!");
                        break;
                }
            }
            catch
            {
                //не смогли подключиться к диспетчеру
                MessageBox.Show("Невозможно подключиться к диспетчеру", "Ошибка!", MessageBoxButtons.OK);
                tbRoomId.Text = "";
            }
        }

        private void OpenRoom(int type, string UUID)
        {
            Hide();
            PlaylistForm pform = new PlaylistForm(type, UUID, serverIp, serverPort);
            pform.ShowDialog();
            tbRoomId.Text = "";
            gbRoomId.Visible = false;
            btnConnect1.Enabled = true;
            serverIp = null;
            serverPort = null;
            Show();
        }

        private void tbRoomId_TextChanged(object sender, EventArgs e)
        {
            btnConnect2.Enabled = tbRoomId.Text.Length > 0;
        }
    }
}