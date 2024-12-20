namespace MiddlwareTester
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
            this.btnLocateApp = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.textPostApp = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnPostApp = new System.Windows.Forms.Button();
            this.btnPutApp = new System.Windows.Forms.Button();
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textPut2App = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textPut1App = new System.Windows.Forms.TextBox();
            this.btnDeleteApp = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.textDeleteApp = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLocateApp
            // 
            this.btnLocateApp.Location = new System.Drawing.Point(76, 44);
            this.btnLocateApp.Name = "btnLocateApp";
            this.btnLocateApp.Size = new System.Drawing.Size(100, 37);
            this.btnLocateApp.TabIndex = 0;
            this.btnLocateApp.Text = "Locate/Get";
            this.btnLocateApp.UseVisualStyleBackColor = true;
            this.btnLocateApp.Click += new System.EventHandler(this.button1_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(3, 498);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(349, 127);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            // 
            // textPostApp
            // 
            this.textPostApp.Location = new System.Drawing.Point(132, 180);
            this.textPostApp.Name = "textPostApp";
            this.textPostApp.Size = new System.Drawing.Size(108, 22);
            this.textPostApp.TabIndex = 2;
            this.textPostApp.Text = "AppPostTest";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "application",
            "container",
            "record",
            "notification"});
            this.comboBox1.Location = new System.Drawing.Point(105, 87);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(135, 24);
            this.comboBox1.TabIndex = 4;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 90);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "Somiod-Locate";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 183);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 16);
            this.label2.TabIndex = 6;
            this.label2.Text = "Application Name";
            // 
            // btnPostApp
            // 
            this.btnPostApp.Location = new System.Drawing.Point(76, 125);
            this.btnPostApp.Name = "btnPostApp";
            this.btnPostApp.Size = new System.Drawing.Size(100, 36);
            this.btnPostApp.TabIndex = 7;
            this.btnPostApp.Text = "POST";
            this.btnPostApp.UseVisualStyleBackColor = true;
            this.btnPostApp.Click += new System.EventHandler(this.btnPostApp_Click);
            // 
            // btnPutApp
            // 
            this.btnPutApp.Location = new System.Drawing.Point(76, 233);
            this.btnPutApp.Name = "btnPutApp";
            this.btnPutApp.Size = new System.Drawing.Size(100, 36);
            this.btnPutApp.TabIndex = 8;
            this.btnPutApp.Text = "PUT";
            this.btnPutApp.UseVisualStyleBackColor = true;
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.SynchronizingObject = this;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(61, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 16);
            this.label3.TabIndex = 9;
            this.label3.Text = "CRUD Aplications";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(229, 16);
            this.label4.TabIndex = 10;
            this.label4.Text = "@\"https://localhost:44354/api/somiod";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(116, 464);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 16);
            this.label5.TabIndex = 11;
            this.label5.Text = "Response";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 327);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(144, 16);
            this.label6.TabIndex = 13;
            this.label6.Text = "New Application Name";
            // 
            // textPut2App
            // 
            this.textPut2App.Location = new System.Drawing.Point(174, 327);
            this.textPut2App.Name = "textPut2App";
            this.textPut2App.Size = new System.Drawing.Size(108, 22);
            this.textPut2App.TabIndex = 12;
            this.textPut2App.Text = "AppPutTest";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 287);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(114, 16);
            this.label7.TabIndex = 15;
            this.label7.Text = "Application Name";
            // 
            // textPut1App
            // 
            this.textPut1App.Location = new System.Drawing.Point(174, 287);
            this.textPut1App.Name = "textPut1App";
            this.textPut1App.Size = new System.Drawing.Size(108, 22);
            this.textPut1App.TabIndex = 14;
            this.textPut1App.Text = "AppPostTest";
            // 
            // btnDeleteApp
            // 
            this.btnDeleteApp.Location = new System.Drawing.Point(76, 366);
            this.btnDeleteApp.Name = "btnDeleteApp";
            this.btnDeleteApp.Size = new System.Drawing.Size(100, 36);
            this.btnDeleteApp.TabIndex = 18;
            this.btnDeleteApp.Text = "DELETE";
            this.btnDeleteApp.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 424);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(114, 16);
            this.label8.TabIndex = 17;
            this.label8.Text = "Application Name";
            // 
            // textDeleteApp
            // 
            this.textDeleteApp.Location = new System.Drawing.Point(132, 421);
            this.textDeleteApp.Name = "textDeleteApp";
            this.textDeleteApp.Size = new System.Drawing.Size(108, 22);
            this.textDeleteApp.TabIndex = 16;
            this.textDeleteApp.Text = "AppPostPut";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1255, 637);
            this.Controls.Add(this.btnDeleteApp);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textDeleteApp);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textPut1App);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textPut2App);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnPutApp);
            this.Controls.Add(this.btnPostApp);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.textPostApp);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.btnLocateApp);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLocateApp;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TextBox textPostApp;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnPostApp;
        private System.Windows.Forms.Button btnPutApp;
        private System.IO.FileSystemWatcher fileSystemWatcher1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnDeleteApp;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textDeleteApp;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textPut1App;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textPut2App;
        private System.Windows.Forms.Label label5;
    }
}

