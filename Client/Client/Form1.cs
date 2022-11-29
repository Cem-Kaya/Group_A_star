using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
	public partial class Form1 : Form
	{
		private bool m_Connected = false;
		private Socket m_ClientSocket;
		public Form1()
		{
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            InitializeComponent();
		}

        private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void connectButton_Click(object sender, EventArgs e)
		{
			m_ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			string IpAddress = IPValue.Text;
			int portNum;
			if(Int32.TryParse(portValue.Text, out portNum))
			{
				try
				{
					m_ClientSocket.Connect(IpAddress, portNum);

					connectButton.Enabled = false;
					m_Connected = true;

					Thread recieveThread = new Thread(Recieve);
					recieveThread.Start();
				}
				catch
				{
					logs.AppendText("Failed to connect to the server!\n");
				}
			}
		}

		private void Recieve()
		{
			while(m_Connected)
			{
				try
				{
                    Byte[] buffer = new Byte[64];
                    m_ClientSocket.Receive(buffer);

                    string incomingMessage = Encoding.Default.GetString(buffer);
                    incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));

                    logs.AppendText("Server: " + incomingMessage + "\n");
                }
				catch
				{
					m_ClientSocket.Close();
					m_Connected = false;
				}
			}
		}
	}
}
