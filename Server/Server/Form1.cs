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

namespace Server
{
	public partial class Form1 : Form
	{
		Socket server_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		List<Socket> client_sockets = new List<Socket>();
		bool terminating = false;
		bool listening = false;


		public Form1()
		{
			Control.CheckForIllegalCrossThreadCalls = false;
			this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
			InitializeComponent();
		}

		private void Accept()
		{
			while (listening)
			{
				try
				{
					Socket newClient = server_socket.Accept();
					client_sockets.Add(newClient);
					logs.AppendText("A client is connected.\n");

					Thread receiveThread = new Thread(() => Receive(newClient)); // updated
					receiveThread.Start();
				}
				catch
				{
					if (terminating)
					{
						listening = false;
					}
					else
					{
						logs.AppendText("The socket stopped working.\n");
					}

				}
			}
		}



		private void Receive(Socket thisClient) // updated
		{
			bool connected = true;

			while (connected && !terminating)
			{
				try
				{
					Byte[] buffer = new Byte[64];
					thisClient.Receive(buffer);

					string incomingMessage = Encoding.Default.GetString(buffer);
					incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));
					logs.AppendText("Client: " + incomingMessage + "\n");
				}
				catch
				{
					if (!terminating)
					{
						logs.AppendText("A client has disconnected\n");
					}
					thisClient.Close();
					client_sockets.Remove(thisClient);
					connected = false;
				}
			}
		}


		private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			Environment.Exit(0);
		}

		private void port_box_TextChanged(object sender, EventArgs e)
		{

		}

		private void launch_button_Click(object sender, EventArgs e)
		{

			int serverPort;

			if (Int32.TryParse(port_box.Text, out serverPort))
			{
				IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, serverPort);
				server_socket.Bind(endPoint);
				server_socket.Listen(5);

				listening = true;
				launch_button.Enabled = false;				

				Thread acceptThread = new Thread(Accept);
				acceptThread.Start();

				logs.AppendText("Started listening on port: " + serverPort + "\n");

			}
			else
			{
				logs.AppendText("Please check port number \n");
			}
		}

		private void richTextBox1_TextChanged(object sender, EventArgs e)
		{

		}
	}
}
