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
                    //�� ������� ������� ����� �������
                    case 401 when string.IsNullOrEmpty(response.UUID):
                        MessageBox.Show("�� ������� ������� ����� �������", "������!", MessageBoxButtons.OK);
                        break;
                    //��� ��������� ��������
                    case 401 when string.IsNullOrEmpty(response.port):
                        MessageBox.Show("��� ��������� ��������", "������!", MessageBoxButtons.OK);
                        break;
                    //�������� �����������
                    case 200:
                        serverIp = response.ip;
                        serverPort = response.port;
                        OpenRoom(1, response.UUID);
                        break;
                    default:
                        MessageBox.Show("���-�� ����� �� ���...", "������!");
                        break;
                }
            }
            catch
            {
                //�� ������ ������������ � ����������
                MessageBox.Show("���������� ������������ � ����������", "������!", MessageBoxButtons.OK);
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
                    //�� ������� ����� �������
                    case 401 when string.IsNullOrEmpty(response.UUID):
                        MessageBox.Show("�� ������� ����� �������", "������!", MessageBoxButtons.OK);
                        tbRoomId.Text = "";
                        break;
                    //��� ��������� ��������
                    case 401 when string.IsNullOrEmpty(response.port):
                        MessageBox.Show("��� ��������� ��������", "������!", MessageBoxButtons.OK);
                        tbRoomId.Text = "";
                        break;
                    //������� �� ��������
                    case 200 when string.IsNullOrEmpty(response.UUID):
                        MessageBox.Show("� ������ ������� ���������� ������������, ��� ��� ��� �� �������", "������!", MessageBoxButtons.OK);
                        tbRoomId.Text = "";
                        break;
                    //�������� �����������
                    case 200:
                        serverIp = response.ip;
                        serverPort = response.port;
                        OpenRoom(2, response.UUID);
                        break;
                    default:
                        MessageBox.Show("���-�� ����� �� ���...", "������!");
                        break;
                }
            }
            catch
            {
                //�� ������ ������������ � ����������
                MessageBox.Show("���������� ������������ � ����������", "������!", MessageBoxButtons.OK);
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