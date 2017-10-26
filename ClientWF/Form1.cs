using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientWF
{
    public partial class Form1 : Form
    {
        private TcpClient client;
        private BinaryReader reader;
        private BinaryWriter writer;

        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnClosed(EventArgs e)
        {
            client.Close();
            base.OnClosed(e);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                client = new TcpClient("192.168.0.101", 9050);
                NetworkStream stream = client.GetStream();
                reader = new BinaryReader(stream);
                writer = new BinaryWriter(stream);
                Thread thread = new Thread(GetMessages); 
                thread.Start();
            }
            catch (Exception exception)
            {
                listBox1.Items.Add($"Connection failed ({exception.Message})");
            }
        }

        private void GetMessages()
        {
            try
            {
                while (true)
                {
                    string message = reader.ReadString();
                    if (message.Length != 0)
                    {
                        listBox1.BeginInvoke(new Action(delegate
                        {
                            listBox1.Items.Add(message);
                        }));
                    }
                }
            }
            catch (Exception e)
            {
                listBox1.BeginInvoke(new Action(() => { listBox1.Items.Add($"Connection lost ({e.Message})"); }));
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (writer != null)
                {
                    writer.Write(textBox1.Text);
                    listBox1.Items.Add("You: " + textBox1.Text);
                }
            }
            catch (Exception exception)
            {
                listBox1.Items.Add($"Connection lost ({exception.Message})");
            }
        }
    }
}
