using SuperSimpleTcp;
using System.Linq.Expressions;
using System.Text;

namespace TCPclient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SimpleTcpClient client;

        private void btnConnect_Click(object sender, EventArgs e)
        {
            //txtMessage.Text= "127.0.0.1:9000";
            /*
            if (!string.IsNullOrEmpty(txtMessage.Text))
            {
                client.Send(txtMessage.Text);
                txtInfo.Text += $"Me: {txtMessage.Text}{Environment.NewLine}";
                txtMessage.Text = string.Empty;
            }
            */
            try
            {
                client.Connect();
                btnSend.Enabled = true;
                btnConnect.Enabled = false;
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {


            if (!string.IsNullOrEmpty(txtMessage.Text))
            {
                client.Send(txtMessage.Text);
                txtInfo.Text += $"Me: {txtMessage.Text}{Environment.NewLine}";
                txtMessage.Text = string.Empty;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            client = new(txtIP.Text);
            client.Events.Connected += Event_Connected;
            client.Events.DataReceived += Event_DataReceived;
            client.Events.Disconnected += Event_Disconnected;
            btnSend.Enabled = false;

        }

        private void Event_Disconnected(object? sender, ConnectionEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtInfo.Text += $"Server disconnected.{Environment.NewLine}";
            });
        }

        private void Event_DataReceived(object? sender, DataReceivedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtInfo.Text += $"Server: {Encoding.UTF8.GetString(e.Data)}{Environment.NewLine}";
            });
        }

        private void Event_Connected(object? sender, ConnectionEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtInfo.Text += $"Server connected.{Environment.NewLine}";
            });
            
        }
    }
}