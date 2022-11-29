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
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.portValue = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.IPValue = new System.Windows.Forms.TextBox();
			this.logs = new System.Windows.Forms.RichTextBox();
			this.connectButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(110, 121);
			this.textBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(96, 20);
			this.textBox1.TabIndex = 15;
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
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(600, 366);
			this.Controls.Add(this.textBox1);
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

		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox portValue;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox IPValue;
		private System.Windows.Forms.RichTextBox logs;
		private System.Windows.Forms.Button connectButton;
		private System.Windows.Forms.Label label1;
	}
}

