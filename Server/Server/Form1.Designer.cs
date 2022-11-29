namespace Server
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
			this.label1 = new System.Windows.Forms.Label();
			this.port_box = new System.Windows.Forms.TextBox();
			this.logs = new System.Windows.Forms.RichTextBox();
			this.launch_button = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(62, 74);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(28, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Post";
			// 
			// port_box
			// 
			this.port_box.Cursor = System.Windows.Forms.Cursors.AppStarting;
			this.port_box.Location = new System.Drawing.Point(103, 74);
			this.port_box.Name = "port_box";
			this.port_box.Size = new System.Drawing.Size(100, 20);
			this.port_box.TabIndex = 1;
			this.port_box.TextChanged += new System.EventHandler(this.port_box_TextChanged);
			// 
			// logs
			// 
			this.logs.Location = new System.Drawing.Point(65, 121);
			this.logs.Name = "logs";
			this.logs.Size = new System.Drawing.Size(374, 193);
			this.logs.TabIndex = 2;
			this.logs.Text = "";
			this.logs.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
			// 
			// launch_button
			// 
			this.launch_button.Location = new System.Drawing.Point(364, 74);
			this.launch_button.Name = "launch_button";
			this.launch_button.Size = new System.Drawing.Size(75, 23);
			this.launch_button.TabIndex = 3;
			this.launch_button.Text = "launch";
			this.launch_button.UseVisualStyleBackColor = true;
			this.launch_button.Click += new System.EventHandler(this.launch_button_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(600, 366);
			this.Controls.Add(this.launch_button);
			this.Controls.Add(this.logs);
			this.Controls.Add(this.port_box);
			this.Controls.Add(this.label1);
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox port_box;
		private System.Windows.Forms.RichTextBox logs;
		private System.Windows.Forms.Button launch_button;
	}
}

