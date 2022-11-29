using System;
using System.IO;
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
		List<String> client_names = new List<String>();
		bool terminating = false;
		bool listening = false;
		uint max_num_of_clients = 2;
		uint num_of_players = 0;
        Dictionary<Socket,uint> player_scores = new Dictionary<Socket, uint>();




        public Form1()
		{
			Control.CheckForIllegalCrossThreadCalls = false;
			this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
			InitializeComponent();
			Thread game_loop_thread = new Thread(game_loop);
			game_loop_thread.Start();
		}

		private void Accept()
		{
			while (listening)
			{
				try
				{
					Socket newClient = server_socket.Accept();
					client_sockets.Add(newClient);

					Byte[] buffer = new Byte[64];
					string this_threads_name;
					newClient.Receive(buffer);

					this_threads_name = Encoding.Default.GetString(buffer);
					this_threads_name = this_threads_name.Substring(0, this_threads_name.IndexOf("\0"));
					//check if a client with the same name is connected
					if (client_names.Contains(this_threads_name))
					{
						Byte[] name_statu = Encoding.Default.GetBytes("this name is taken!");
						try
						{
							newClient.Send(name_statu);
						}
						catch
						{
							logs.AppendText("There is a problem! Check the connection...\n");
							// if client is disconnected, do stuff here 
						}
						
						newClient.Close();
						logs.AppendText("Existing clients name: " + this_threads_name + " there was already someone with this name, so the new client was disconnected.\n");
					}
					else
					{
						Byte[] name_statu = Encoding.Default.GetBytes("this name is not taken !");
						try
						{
							newClient.Send(name_statu);
							client_names.Add(this_threads_name);
							logs.AppendText("Client with name " + this_threads_name + " is connected!\n");
							num_of_players++;
							
							Thread receiveThread = new Thread(() => Receive(newClient, this_threads_name)); // updated
							first_sem.Release();// #UP
							receiveThread.Start();
						}
						catch
						{
							logs.AppendText("There is a problem! Check the connection...\n");
							// if client is disconnected, do stuff here 

						}
					}
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

		private void Receive(Socket thisClient , String name ) // updated
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
					logs.AppendText("Client "+ name + " : " + incomingMessage + "\n");

					int clients_ans;
					if (Int32.TryParse(incomingMessage, out clients_ans))
					{
						ans[name] = clients_ans;
					}
					else
					{
						logs.AppendText(" something went very wrong number should have been send from client \n");
					}
					sem.Release();// #UP
				}
				catch
				{
					if (!terminating)
					{
						logs.AppendText("A client has disconnected\n");
						num_of_players--;

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
		private void richTextBox1_TextChanged(object sender, EventArgs e)
		{

		}
		private void port_box_TextChanged(object sender, EventArgs e)
		{

		}

		private void launch_button_Click(object sender, EventArgs e)
		{
			int serverPort;
			
			if (Int32.TryParse(port_box.Text, out serverPort)  )
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

		private String[] readFile(String textFile)
		{
			String[] lines = File.ReadAllLines(textFile);
			return lines;
		}

		Semaphore first_sem = new Semaphore(0, 3);

		Dictionary<String, int> ans = new Dictionary<String, int>();
		Semaphore  sem = new Semaphore(0, 2);
		private void game_loop()
		{
			first_sem.WaitOne();
			first_sem.WaitOne();
			first_sem.WaitOne();

			logs.AppendText("got past the lock the game shall start now !\n");
			int q = 0;
			while (question_num > 0){
				foreach(Socket cl in client_sockets)
				{
					String this_question = lines[(2 * q) % ( lines.Length /2 ) ];
					Byte[] question__ = Encoding.Default.GetBytes("QUEST"+this_question);
					try
					{
						cl.Send(question__);
					}
					catch
					{
						logs.AppendText("Could not send question \n");
						// if client is disconnected, do stuff here 
					}
				}
				// Get the user answers
				// Wait for all the player threads
				sem.WaitOne();
				sem.WaitOne();

                
				foreach(string client_name in client_names)
				{
					int answer;
					if(Int32.TryParse(lines[(2 * q) % (lines.Length / 2) + 1], out answer))
					{
						int client_answer = ans[client_name];
						int difference = Math.Abs(answer - client_answer); 					
						
					}

				}

				question_num--;
				q++;
			}
		}

		bool question = false;
		int question_num;
		String[] lines; 

		private void Set_question_number_Click(object sender, EventArgs e)
		{			
			if (Int32.TryParse(number_of_questions.Text, out question_num))
			{
				//reads all lines and stores all lines in string array
				lines = readFile("questions.txt");
				question = true;
				Set_question_number.Enabled = false;
				logs.AppendText("number of questions: " + question_num + "\n");
				first_sem.Release();//#UP

			}
			else
			{
				logs.AppendText("Please check question number \n");
			}			
		}
	}
}
