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

		// variables used for hte project 
		String[] lines;
		bool terminating = false;
		bool listening = false;
		bool question = false;
		int question_num;
		uint max_num_of_clients = 2;
		uint num_of_players = 0;

		Socket server_socket = new Socket(AddressFamily.InterNetwork , SocketType.Stream , ProtocolType.Tcp );

		List<Socket> client_sockets = new List<Socket>();
		List<String> client_names = new List<String>();
		
		Semaphore first_sem = new Semaphore(0, 3); //  for step one 3 is known 
		Semaphore sem = new Semaphore(0, 2); // 2 is the number of players 

		Dictionary<String, float> player_scores = new Dictionary<String, float>();
		Dictionary<String, int> ans = new Dictionary<String, int>();
		Dictionary<Socket, String> socket_to_name = new Dictionary<Socket, String>();
		List<Thread> current_threads = new List<Thread>();
		
		public Form1()
		{
			Control.CheckForIllegalCrossThreadCalls = false;
			this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
			InitializeComponent();
			Thread game_loop_thread = new Thread(game_loop);
			game_loop_thread.Start();
		}
		// listen for the incoming connections
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
					// block untill new connection 
					this_threads_name = Encoding.Default.GetString(buffer);
					this_threads_name = this_threads_name.Substring(0, this_threads_name.IndexOf("\0"));
					//check if a client with the same name is connected
					if(client_names.Count() >= 2)
					{
						Byte[] players_statu = Encoding.Default.GetBytes("There are already two players in the game!");
						try
						{
							newClient.Send(players_statu);
						}
						catch
						{
							logs.AppendText("There is a problem! Check the connection...\n");
							// if client is disconnected, do stuff here 
						}
						client_sockets.Remove(newClient);
						newClient.Close();
						

						logs.AppendText("A client with the name: " + this_threads_name + " tried to join while the game was running.\n");
					}
					//if a client with the same name tries to connect
					else if (client_names.Contains(this_threads_name))
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
					//client successfully connects
					else
					{
						Byte[] name_statu = Encoding.Default.GetBytes("this name is not taken !");
						try
						{
							newClient.Send(name_statu);
							client_names.Add(this_threads_name);
							socket_to_name[newClient] = this_threads_name;
							logs.AppendText("Client with name " + this_threads_name + " is connected!\n");
							num_of_players++;
							//start thread 
							Thread receiveThread = new Thread(() => Receive(newClient, this_threads_name)); // updated
							first_sem.Release();// #UP							
							receiveThread.Start();
							current_threads.Add(receiveThread);
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
			// syncronized with hte game loop by the use of semaphores 
			while (connected && !terminating)
			{
				try
				{
					Byte[] buffer = new Byte[64];
					thisClient.Receive(buffer);
					// block untill recive 
					string incomingMessage = Encoding.Default.GetString(buffer);
					incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));
					logs.AppendText("Client " + name + " : " + incomingMessage + "\n");

					int clients_ans;
					if (Int32.TryParse(incomingMessage, out clients_ans))
					{
						ans[name] = clients_ans;
					}
					else
					{
						logs.AppendText("something went very wrong number should have been send from client \n");
					}
					
				}
				catch
				{
					connected = false;
					// One user terminated, other user should win the game
					if (!terminating)
					{
						logs.AppendText("A client " + name + " has disconnected\n");
						num_of_players--;
					}
					ans[name] = -1;
					try
					{
						thisClient.Close();
					}catch
					{

					}
					//client_sockets.Remove(thisClient);
					//client_names.Remove(name);
					//broadcast("User");		

				}
				finally
				{
					Thread.Sleep(250);
					sem.Release(); // #UP
					//sem.Release(); // #UP
				}
			}
		}

		private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			Environment.Exit(0);
			foreach (Thread t in current_threads)
			{
				t.Abort();// kill all ch 
			}

		}
		private void richTextBox1_TextChanged(object sender, EventArgs e)
		{

		}
		private void port_box_TextChanged(object sender, EventArgs e)
		{

		}

		private void launch_button_Click(object sender, EventArgs e)
		{
			//sets the launch parameters 
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
			//read the file 
			String[] lines = File.ReadAllLines(textFile);
			return lines;
		}
		// broadcast 
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
			//sleep for msg to arive 
			Thread.Sleep(700);
		}

		// game loop
		private void game_loop()
		{
			first_sem.WaitOne();
			first_sem.WaitOne();
			first_sem.WaitOne();

			logs.AppendText("got past the lock the game shall start now !\n");
			int q = 0;

			foreach(var pl in client_names)
			{
				player_scores[pl] = 0.0f; // st the score to 0
			}

			//broadcasting questions to the users
			bool some_one_disconnected = false;
			while (question_num > 0)
			{
				
				String this_question = lines[(2 * q) % (lines.Length )];
				//debug_logs.AppendText("lines.Length :" + lines.Length  + "(2 * q) % (lines.Length ) =  " + (2 * q) % (lines.Length ) );
				this_question = "QUEST" + this_question + "\n";
				broadcast(this_question);
				// Get the user answers
				// Wait for all the player threads
				sem.WaitOne();
				sem.WaitOne();
				
				some_one_disconnected = false;
				String disconnected_players_name = "";
				foreach (var cl in client_sockets)
				{
					debug_logs.AppendText("carsh : "+ client_sockets.Count());
					if (!cl.Connected)
					{
						some_one_disconnected = true;
						disconnected_players_name = socket_to_name[cl];
					}
				}
				if (some_one_disconnected)
				{
					broadcast("DCPLY" + disconnected_players_name + " has disconnected you win the game !");
					player_scores[disconnected_players_name] = 0.0f;
					break;
				}

				Dictionary<string, int> name_dif = new Dictionary<string, int>();

				//checking clients' answers and saving the real answer-client answer differences
				foreach (string client_name in client_names)
				{
					int answer;
					if (Int32.TryParse(lines[(2 * q) % (lines.Length) + 1], out answer))
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
				String tmp_round_winner;
				if (keys.Count() == 1)
				{
					tmp_round_winner = keys.First() + " is the winner of the round ";
				}
				else
				{
					tmp_round_winner = "This round it is a tie.";

				}
				// Concatenate the player name with their answer and broadcast it to the each client
				String answare_info = "Answers: real : "+ lines[(2 * q) % (lines.Length) + 1 ] + "\n";
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
				score_info += "\n" + tmp_round_winner + "\n";
				broadcast("SCORE" + score_info);

				question_num--;
				q++;
			}
			
			var sortedDict2 = from entry in player_scores orderby entry.Value descending select entry;
			float max_scores = sortedDict2.First().Value;
			var winner_keys = sortedDict2.Where(x => x.Value == max_scores).Select(x => x.Key);

			String end_of_game = "The game has ended !" + "\n "; 

			if (winner_keys.Count() > 1)
			{
				if (!some_one_disconnected) {
					end_of_game += "It is a tie!\n";
					end_of_game += "";
				}
				
				foreach(String winner in winner_keys)
				{
					end_of_game += winner + " with a score of " + sortedDict2.First().Value + "\n";
				}
				broadcast("TIEGO" + end_of_game + "\n");
			}
			else
			{
				end_of_game += "The winner is " + sortedDict2.First().Key + " with a score of " + sortedDict2.First().Value + "\n";
				broadcast("GMOVR" + end_of_game + "\n");
			}

			Thread.Sleep(1000);
			clean_after_game();
		}

		private void clean_after_game()
		{
			num_of_players = 0;
			foreach (Socket cl in client_sockets)
			{
				cl.Close();
			}
			
			foreach (Thread t in current_threads)
			{
				t.Join();
			}
			
			terminating = false;
			//listening = false;
			question = false;
			
			max_num_of_clients = 2;
			num_of_players = 0;

			client_sockets = new List<Socket>();
			client_names = new List<String>();

			first_sem = new Semaphore(0, 3); //  for step one 3 is known 
			sem = new Semaphore(0, 2); // 2 is the number of players 
			Set_question_number.Enabled = true;
			
			player_scores = new Dictionary<String, float>();
			ans = new Dictionary<String, int>();
			socket_to_name = new Dictionary<Socket, String>();

			Thread game_loop_thread = new Thread(game_loop);
			game_loop_thread.Start();
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
