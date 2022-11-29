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
		private Thread recieveThread;

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
			string client_name;
			int portNum;
			if(Int32.TryParse(portValue.Text, out portNum))
			{
				try
				{
					m_ClientSocket.Connect(IpAddress, portNum);
					client_name = textBox_name.Text;
					connectButton.Enabled = false;
					m_Connected = true;
					button_disconnect.Enabled = true;

					if(client_name != "" && client_name.Length <= 64) 
					{
						Byte[] buffer = Encoding.Default.GetBytes(client_name);
						m_ClientSocket.Send(buffer);
					}

					Byte[] buffer2 = new Byte[64];
					m_ClientSocket.Receive(buffer2);

					string incomingMessage = Encoding.Default.GetString(buffer2);
					incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));

					if (incomingMessage == "this name is taken!")
					{
						logs.AppendText("Name is already taken can't connect to the server with this name!\n");
						connectButton.Enabled = true;
						m_Connected = false;
						button_disconnect.Enabled = false;
					}
					else
					{
                        
						recieveThread = new Thread(Recieve);
						recieveThread.Start();
						logs.AppendText("Connected to the server!\n");
					}
					//logs.AppendText("Server: " + incomingMessage + "\n");
				
					//logs.AppendText("Name is: " + client_name+ "\n");
				}
				catch
				{
					logs.AppendText("Failed to connect to the server due to non name related issues!\n");
				}
			}
		}

		private void Recieve()
		{
			while(m_Connected)
			{
				try
				{
					Byte[] buffer = new Byte[1024];
					Byte[] buffer2 = new Byte[1024];
					m_ClientSocket.Receive(buffer);

					string incoming_message = Encoding.Default.GetString(buffer);
					string type = incoming_message.Substring(0, 5);
					//logs.AppendText("Server: " + incoming_message + "\n");
					
					if(type == "QUEST")
					{
						button_send_answer.Enabled = true;
						string question_line = Encoding.Default.GetString(buffer);
						question_line = question_line.Substring(5, question_line.IndexOf("\0"));
						logs.AppendText("Server Asks: " + question_line);	
					}
				}
				catch
				{
					m_ClientSocket.Close();
					m_Connected = false;
                    logs.AppendText("Should be disconnecting now \n");
                    button_disconnect.Enabled = false;
                    connectButton.Enabled = true;
                    
					
                }
			}
						
		}

		private void IPValue_TextChanged(object sender, EventArgs e)
		{

		}

		//button_disconnect
		private void button1_Click(object sender, EventArgs e)
		{
			m_Connected = false;
			button_disconnect.Enabled = false;
			connectButton.Enabled = true;
			
			m_ClientSocket.Close();
			recieveThread.Abort();

			logs.AppendText("Disconnecting from server...\n");
		}

		private void textBox_answer_TextChanged(object sender, EventArgs e)
		{

		}

		private void button_send_answer_Click(object sender, EventArgs e)
		{
			string answer = textBox_answer.Text;
	        Byte[] buffer = Encoding.Default.GetBytes(answer);
			m_ClientSocket.Send(buffer);
            button_send_answer.Enabled = false;
        }
	}
}
