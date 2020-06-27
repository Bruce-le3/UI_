namespace Justech
{
    partial class Serial_Port_Test
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
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.cmb_PortName = new System.Windows.Forms.ComboBox();
            this.cmb_BaudRate = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmb_Parity = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmb_DataBits = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cmb_StopBits = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmb_StopBits);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.cmb_DataBits);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.cmb_Parity);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.cmb_BaudRate);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cmb_PortName);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(529, 451);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "串口测试";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(223, 279);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(197, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "(For Example :COM1,115200,N,8,1)\r\n";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1, 327);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "Notes:";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(48, 314);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(363, 31);
            this.textBox2.TabIndex = 7;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox3);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(3, 346);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(523, 102);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "MSG";
            // 
            // textBox3
            // 
            this.textBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox3.Location = new System.Drawing.Point(3, 17);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(517, 82);
            this.textBox3.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(417, 320);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Send";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(57, 276);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(160, 21);
            this.textBox1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 279);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Setting:";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "PortName:";
            // 
            // cmb_PortName
            // 
            this.cmb_PortName.FormattingEnabled = true;
            this.cmb_PortName.Location = new System.Drawing.Point(90, 58);
            this.cmb_PortName.Name = "cmb_PortName";
            this.cmb_PortName.Size = new System.Drawing.Size(121, 20);
            this.cmb_PortName.TabIndex = 11;
            // 
            // cmb_BaudRate
            // 
            this.cmb_BaudRate.FormattingEnabled = true;
            this.cmb_BaudRate.Location = new System.Drawing.Point(90, 101);
            this.cmb_BaudRate.Name = "cmb_BaudRate";
            this.cmb_BaudRate.Size = new System.Drawing.Size(121, 20);
            this.cmb_BaudRate.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 104);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 12;
            this.label5.Text = "BaudRate:";
            // 
            // cmb_Parity
            // 
            this.cmb_Parity.FormattingEnabled = true;
            this.cmb_Parity.Location = new System.Drawing.Point(90, 147);
            this.cmb_Parity.Name = "cmb_Parity";
            this.cmb_Parity.Size = new System.Drawing.Size(121, 20);
            this.cmb_Parity.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(25, 150);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 12);
            this.label6.TabIndex = 14;
            this.label6.Text = "Parity:";
            // 
            // cmb_DataBits
            // 
            this.cmb_DataBits.FormattingEnabled = true;
            this.cmb_DataBits.Location = new System.Drawing.Point(90, 191);
            this.cmb_DataBits.Name = "cmb_DataBits";
            this.cmb_DataBits.Size = new System.Drawing.Size(121, 20);
            this.cmb_DataBits.TabIndex = 17;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(25, 194);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 12);
            this.label7.TabIndex = 16;
            this.label7.Text = "DataBits:";
            // 
            // cmb_StopBits
            // 
            this.cmb_StopBits.FormattingEnabled = true;
            this.cmb_StopBits.Location = new System.Drawing.Point(90, 233);
            this.cmb_StopBits.Name = "cmb_StopBits";
            this.cmb_StopBits.Size = new System.Drawing.Size(121, 20);
            this.cmb_StopBits.TabIndex = 19;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(25, 236);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 12);
            this.label8.TabIndex = 18;
            this.label8.Text = "StopBits:";
            // 
            // Serial_Port_Test
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(565, 480);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.Name = "Serial_Port_Test";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Serial_Port_Test";
            this.Load += new System.EventHandler(this.Serial_Port_Test_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ComboBox cmb_BaudRate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmb_PortName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmb_Parity;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmb_DataBits;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmb_StopBits;
        private System.Windows.Forms.Label label8;
    }
}