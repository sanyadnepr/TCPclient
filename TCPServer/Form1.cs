using SuperSimpleTcp;
using System.Text;

namespace TCPServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SimpleTcpServer _server; 

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                if(_server is {})
                    _server.Stop();
                _server = new SimpleTcpServer(txtIP.Text);
                _server.Events.ClientConnected += Events_ClientConnect;
                _server.Events.ClientDisconnected += Events_ClientDisconnected;
                _server.Events.DataReceived += Events_DataReceived;
                _server.Start();
                txtInfo.Text += $"Starting {txtIP.Text}...{Environment.NewLine}";
                btnStart.Enabled = false;
                btnSend.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Message:{ex.Message}\nStackTrace:{ex.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnSend.Enabled = false;
            

        }

        private void Events_DataReceived(object? sender, DataReceivedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtInfo.Text += $"{e.IpPort}: {Encoding.UTF8.GetString(e.Data)}{Environment.NewLine}";
            });
        }

        private void Events_ClientDisconnected(object? sender, ConnectionEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtInfo.Text += $"{e.IpPort} disconnected.{Environment.NewLine}";
            LstClientIP.Items.Remove(e.IpPort);
            });
        }

        private void Events_ClientConnect(object? sender, ConnectionEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtInfo.Text += $"{e.IpPort} connected.{Environment.NewLine}";
                LstClientIP.Items.Add(e.IpPort);
            });

        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if(_server.IsListening)
            {
                if(!string.IsNullOrEmpty(txtMessage.Text) && LstClientIP.SelectedItem !=null)
                {
                    _server.Send(LstClientIP.SelectedItem.ToString(), txtMessage.Text);
                    txtInfo.Text += $"Server: {txtMessage.Text}{Environment.NewLine}";
                    txtMessage.Text = string.Empty;

                }
            }
        }
    }
}