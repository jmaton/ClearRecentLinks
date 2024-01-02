namespace ClearRecentLinks
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
            this.btnRunNow = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtRunInterval = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnAddFilename = new System.Windows.Forms.Button();
            this.btnAddDirectory = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.labelStatus = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRunNow
            // 
            this.btnRunNow.Location = new System.Drawing.Point(603, 18);
            this.btnRunNow.Name = "btnRunNow";
            this.btnRunNow.Size = new System.Drawing.Size(129, 60);
            this.btnRunNow.TabIndex = 0;
            this.btnRunNow.Text = "Run Now";
            this.btnRunNow.UseVisualStyleBackColor = true;
            this.btnRunNow.Click += new System.EventHandler(this.btnRunNow_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtRunInterval);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(315, 67);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // txtRunInterval
            // 
            this.txtRunInterval.Location = new System.Drawing.Point(183, 23);
            this.txtRunInterval.Name = "txtRunInterval";
            this.txtRunInterval.Size = new System.Drawing.Size(100, 26);
            this.txtRunInterval.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(157, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Run every n minutes:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(253, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Clear links containing any of these:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnAddFilename);
            this.groupBox2.Controls.Add(this.btnAddDirectory);
            this.groupBox2.Controls.Add(this.listBox1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(12, 86);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(776, 410);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            // 
            // btnAddFilename
            // 
            this.btnAddFilename.Location = new System.Drawing.Point(332, 363);
            this.btnAddFilename.Name = "btnAddFilename";
            this.btnAddFilename.Size = new System.Drawing.Size(272, 38);
            this.btnAddFilename.TabIndex = 5;
            this.btnAddFilename.Text = "Add partial filename...";
            this.btnAddFilename.UseVisualStyleBackColor = true;
            this.btnAddFilename.Click += new System.EventHandler(this.btnAddFilename_Click);
            // 
            // btnAddDirectory
            // 
            this.btnAddDirectory.Location = new System.Drawing.Point(11, 363);
            this.btnAddDirectory.Name = "btnAddDirectory";
            this.btnAddDirectory.Size = new System.Drawing.Size(272, 38);
            this.btnAddDirectory.TabIndex = 4;
            this.btnAddDirectory.Text = "Add a Directory...";
            this.btnAddDirectory.UseVisualStyleBackColor = true;
            this.btnAddDirectory.Click += new System.EventHandler(this.btnAddDirectory_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 20;
            this.listBox1.Location = new System.Drawing.Point(11, 46);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(759, 304);
            this.listBox1.TabIndex = 3;
            this.listBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.listBox1_KeyUp);
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(12, 520);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(65, 20);
            this.labelStatus.TabIndex = 4;
            this.labelStatus.Text = "status...";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 626);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnRunNow);
            this.Name = "Form1";
            this.Text = "Clear Win10 Recent/JumpList entries    v1.1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnRunNow;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox txtRunInterval;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button btnAddFilename;
		private System.Windows.Forms.Button btnAddDirectory;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.Label labelStatus;
	}
}

