﻿using System;
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
		String[] lines;
		bool terminating = false;
		bool listening = false;
		bool question = false;
		int question_num;
		uint max_num_of_clients = 2;
		uint num_of_players = 0;

		Socket server_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		List<Socket> client_sockets = new List<Socket>();
		List<String> client_names = new List<String>();
		
		Semaphore first_sem = new Semaphore(0, 3); //  for step one 3 is known 
		Semaphore sem = new Semaphore(0, 2); // 2 is the number of players 

		Dictionary<String, float> player_scores = new Dictionary<String, float>();
		Dictionary<String, int> ans = new Dictionary<String, int>();

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
		public void broadcast (string msg)
		{
			debug_logs.AppendText("broadcasting: " + msg + "\n");
			foreach (Socket cl in client_sockets)
			{				
				Byte[] question__ = Encoding.Default.GetBytes(msg);
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
			
			Thread.Sleep(400);
		}

		private void game_loop()
		{
			first_sem.WaitOne();
			first_sem.WaitOne();
			first_sem.WaitOne();

			logs.AppendText("got past the lock the game shall start now !\n");
			int q = 0;
			foreach(var pl in client_names)
			{
				player_scores[pl] = 0.0f;
			}
			//boradcasting questions to the users
			while (question_num > 0)
			{
				String this_question = lines[(2 * q) % (lines.Length / 2)];
				this_question = "QUEST" + this_question + "\n";
				broadcast(this_question);
				// Get the user answers
				// Wait for all the player threads
				sem.WaitOne();
				sem.WaitOne();

				Dictionary<string, int> name_dif = new Dictionary<string, int>();

				//checking clients' answers and saving the real answer-client answer differences
				foreach (string client_name in client_names)
				{
					int answer;
					if (Int32.TryParse(lines[(2 * q) % (lines.Length / 2) + 1], out answer))
					{
						int client_answer = ans[client_name];
						int difference = Math.Abs(answer - client_answer);
						name_dif[client_name] = difference;


					}
				}

				// Sort the difference dictionary to pull out the max score.
				var sortedDict = from entry in name_dif orderby entry.Value ascending select entry;
				int max_score = sortedDict.First().Value;
				
				// magic !
				var keys = sortedDict.Where(x => x.Value == max_score).Select(x => x.Key);
				
				// Update the player scores
				foreach(String Pl in keys)
				{
					player_scores[Pl] += 1.0f/keys.Count();
				}
				
				// Concatenate the player name with their answer and broadcast it to the each client
				String answare_info = "Answers: real : "+ lines[(2 * q) % (lines.Length / 2) + 1 ] + "\n";
				foreach(var pl in client_names)
				{
					answare_info += pl + " : " + ans[pl] + " ";
				}
				answare_info += "\n";

				broadcast("ANSWE" + answare_info);
				
				// Concatenate the player with their associated score and broadcast it to the each client

				String score_info = "Current scores: ";
				foreach (var pl in client_names)
				{
					score_info += pl + " : " + player_scores[pl] + " ";
				}
				score_info += "\n";
				broadcast("SCORE" + score_info);

				question_num--;
				q++;
			}
			
			var sortedDict2 = from entry in player_scores orderby entry.Value descending select entry;
			String end_of_game = "The game has ended !" + "\n " + "The winner is " + sortedDict2.First().Key + " with a score of " + sortedDict2.First().Value + "\n";

			broadcast("GMOVR" + end_of_game + "\n");
			
			Thread.Sleep(1000);
			foreach (Socket cl in client_sockets)
			{
				cl.Close();
			}

		}

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

		private void debug_logs_TextChanged(object sender, EventArgs e)
		{

		}
	}
}
