namespace Client
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.textBox_name = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.portValue = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.IPValue = new System.Windows.Forms.TextBox();
			this.logs = new System.Windows.Forms.RichTextBox();
			this.connectButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.button_disconnect = new System.Windows.Forms.Button();
			this.textBox_answer = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.button_send_answer = new System.Windows.Forms.Button();
			this.logs_debug = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// textBox_name
			// 
			this.textBox_name.Location = new System.Drawing.Point(110, 121);
			this.textBox_name.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.textBox_name.Name = "textBox_name";
			this.textBox_name.Size = new System.Drawing.Size(96, 20);
			this.textBox_name.TabIndex = 15;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(73, 121);
			this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(35, 13);
			this.label3.TabIndex = 14;
			this.label3.Text = "Name";
			// 
			// portValue
			// 
			this.portValue.Location = new System.Drawing.Point(110, 84);
			this.portValue.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.portValue.Name = "portValue";
			this.portValue.Size = new System.Drawing.Size(96, 20);
			this.portValue.TabIndex = 13;
			this.portValue.Text = "1515";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(82, 86);
			this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(26, 13);
			this.label2.TabIndex = 12;
			this.label2.Text = "Port";
			// 
			// IPValue
			// 
			this.IPValue.Location = new System.Drawing.Point(110, 50);
			this.IPValue.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.IPValue.Name = "IPValue";
			this.IPValue.Size = new System.Drawing.Size(96, 20);
			this.IPValue.TabIndex = 11;
			this.IPValue.Text = "172.22.40.190";
			this.IPValue.TextChanged += new System.EventHandler(this.IPValue_TextChanged);
			// 
			// logs
			// 
			this.logs.Location = new System.Drawing.Point(264, 50);
			this.logs.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.logs.Name = "logs";
			this.logs.Size = new System.Drawing.Size(265, 266);
			this.logs.TabIndex = 10;
			this.logs.Text = "";
			// 
			// connectButton
			// 
			this.connectButton.Location = new System.Drawing.Point(110, 160);
			this.connectButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.connectButton.Name = "connectButton";
			this.connectButton.Size = new System.Drawing.Size(94, 19);
			this.connectButton.TabIndex = 9;
			this.connectButton.Text = "Connect";
			this.connectButton.UseVisualStyleBackColor = true;
			this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(82, 55);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(17, 13);
			this.label1.TabIndex = 8;
			this.label1.Text = "IP";
			// 
			// button_disconnect
			// 
			this.button_disconnect.Enabled = false;
			this.button_disconnect.Location = new System.Drawing.Point(434, 331);
			this.button_disconnect.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.button_disconnect.Name = "button_disconnect";
			this.button_disconnect.Size = new System.Drawing.Size(94, 19);
			this.button_disconnect.TabIndex = 16;
			this.button_disconnect.Text = "Disconnect";
			this.button_disconnect.UseVisualStyleBackColor = true;
			this.button_disconnect.Click += new System.EventHandler(this.button1_Click);
			// 
			// textBox_answer
			// 
			this.textBox_answer.Location = new System.Drawing.Point(110, 249);
			this.textBox_answer.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.textBox_answer.Name = "textBox_answer";
			this.textBox_answer.Size = new System.Drawing.Size(142, 20);
			this.textBox_answer.TabIndex = 17;
			this.textBox_answer.TextChanged += new System.EventHandler(this.textBox_answer_TextChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(67, 249);
			this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(42, 13);
			this.label4.TabIndex = 18;
			this.label4.Text = "Answer";
			// 
			// button_send_answer
			// 
			this.button_send_answer.Enabled = false;
			this.button_send_answer.Location = new System.Drawing.Point(194, 280);
			this.button_send_answer.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.button_send_answer.Name = "button_send_answer";
			this.button_send_answer.Size = new System.Drawing.Size(58, 19);
			this.button_send_answer.TabIndex = 19;
			this.button_send_answer.Text = "Send";
			this.button_send_answer.UseVisualStyleBackColor = true;
			this.button_send_answer.Click += new System.EventHandler(this.button_send_answer_Click);
			// 
			// logs_debug
			// 
			this.logs_debug.Location = new System.Drawing.Point(587, 50);
			this.logs_debug.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.logs_debug.Name = "logs_debug";
			this.logs_debug.Size = new System.Drawing.Size(265, 266);
			this.logs_debug.TabIndex = 20;
			this.logs_debug.Text = "";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(916, 366);
			this.Controls.Add(this.logs_debug);
			this.Controls.Add(this.button_send_answer);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.textBox_answer);
			this.Controls.Add(this.button_disconnect);
			this.Controls.Add(this.textBox_name);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.portValue);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.IPValue);
			this.Controls.Add(this.logs);
			this.Controls.Add(this.connectButton);
			this.Controls.Add(this.label1);
			this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox textBox_name;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox portValue;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox IPValue;
		private System.Windows.Forms.RichTextBox logs;
		private System.Windows.Forms.Button connectButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button_disconnect;
		private System.Windows.Forms.TextBox textBox_answer;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button button_send_answer;
		private System.Windows.Forms.RichTextBox logs_debug;
	}
}

