namespace Justech
{
    partial class FrmClient
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
            this.btnClear = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.txtReceive = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtSend = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cobLocalIP = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtRemoteIP = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupStatus = new System.Windows.Forms.GroupBox();
            this.lblStatusClient2 = new System.Windows.Forms.Label();
            this.lblStatusClient3 = new System.Windows.Forms.Label();
            this.lblStatusClient4 = new System.Windows.Forms.Label();
            this.lblStatusClient5 = new System.Windows.Forms.Label();
            this.lblStatusClient6 = new System.Windows.Forms.Label();
            this.lblStatusClient7 = new System.Windows.Forms.Label();
            this.lblStatusClient8 = new System.Windows.Forms.Label();
            this.lblStatusClient1 = new System.Windows.Forms.Label();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.Location = new System.Drawing.Point(521, 35);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(95, 39);
            this.btnClear.TabIndex = 24;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.txtReceive);
            this.groupBox5.Controls.Add(this.btnClear);
            this.groupBox5.Location = new System.Drawing.Point(69, 329);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(642, 97);
            this.groupBox5.TabIndex = 23;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Receive Message";
            // 
            // txtReceive
            // 
            this.txtReceive.Location = new System.Drawing.Point(6, 20);
            this.txtReceive.Multiline = true;
            this.txtReceive.Name = "txtReceive";
            this.txtReceive.Size = new System.Drawing.Size(502, 62);
            this.txtReceive.TabIndex = 1;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnSend);
            this.groupBox4.Controls.Add(this.txtSend);
            this.groupBox4.Location = new System.Drawing.Point(69, 219);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(642, 102);
            this.groupBox4.TabIndex = 22;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Send Message";
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(521, 42);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(95, 39);
            this.btnSend.TabIndex = 5;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtSend
            // 
            this.txtSend.Location = new System.Drawing.Point(6, 20);
            this.txtSend.Multiline = true;
            this.txtSend.Name = "txtSend";
            this.txtSend.Size = new System.Drawing.Size(502, 68);
            this.txtSend.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cobLocalIP);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnDisconnect);
            this.groupBox1.Controls.Add(this.btnConnect);
            this.groupBox1.Controls.Add(this.txtRemoteIP);
            this.groupBox1.Controls.Add(this.txtPort);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(69, 52);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(642, 104);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Client Setting";
            // 
            // cobLocalIP
            // 
            this.cobLocalIP.FormattingEnabled = true;
            this.cobLocalIP.Location = new System.Drawing.Point(102, 24);
            this.cobLocalIP.Name = "cobLocalIP";
            this.cobLocalIP.Size = new System.Drawing.Size(121, 20);
            this.cobLocalIP.TabIndex = 14;
            this.cobLocalIP.SelectedIndexChanged += new System.EventHandler(this.cobLocalIP_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 13;
            this.label2.Text = "Local IP:";
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(432, 19);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(105, 29);
            this.btnDisconnect.TabIndex = 12;
            this.btnDisconnect.Text = "Disconnect ";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(304, 19);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(105, 29);
            this.btnConnect.TabIndex = 11;
            this.btnConnect.Text = "Conect ";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtRemoteIP
            // 
            this.txtRemoteIP.Location = new System.Drawing.Point(102, 67);
            this.txtRemoteIP.Name = "txtRemoteIP";
            this.txtRemoteIP.Size = new System.Drawing.Size(121, 21);
            this.txtRemoteIP.TabIndex = 10;
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(304, 67);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(86, 21);
            this.txtPort.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(264, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "Port:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "Remote IP:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(348, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 29);
            this.label1.TabIndex = 20;
            this.label1.Text = "客户端";
            // 
            // groupStatus
            // 
            this.groupStatus.Controls.Add(this.lblStatusClient2);
            this.groupStatus.Controls.Add(this.lblStatusClient3);
            this.groupStatus.Controls.Add(this.lblStatusClient4);
            this.groupStatus.Controls.Add(this.lblStatusClient5);
            this.groupStatus.Controls.Add(this.lblStatusClient6);
            this.groupStatus.Controls.Add(this.lblStatusClient7);
            this.groupStatus.Controls.Add(this.lblStatusClient8);
            this.groupStatus.Controls.Add(this.lblStatusClient1);
            this.groupStatus.Location = new System.Drawing.Point(69, 162);
            this.groupStatus.Name = "groupStatus";
            this.groupStatus.Size = new System.Drawing.Size(642, 45);
            this.groupStatus.TabIndex = 24;
            this.groupStatus.TabStop = false;
            this.groupStatus.Text = "Status";
            // 
            // lblStatusClient2
            // 
            this.lblStatusClient2.BackColor = System.Drawing.Color.Red;
            this.lblStatusClient2.Location = new System.Drawing.Point(42, 17);
            this.lblStatusClient2.Name = "lblStatusClient2";
            this.lblStatusClient2.Size = new System.Drawing.Size(20, 22);
            this.lblStatusClient2.TabIndex = 45;
            this.lblStatusClient2.Tag = "1";
            // 
            // lblStatusClient3
            // 
            this.lblStatusClient3.BackColor = System.Drawing.Color.Red;
            this.lblStatusClient3.Location = new System.Drawing.Point(69, 17);
            this.lblStatusClient3.Name = "lblStatusClient3";
            this.lblStatusClient3.Size = new System.Drawing.Size(20, 22);
            this.lblStatusClient3.TabIndex = 44;
            this.lblStatusClient3.Tag = "2";
            // 
            // lblStatusClient4
            // 
            this.lblStatusClient4.BackColor = System.Drawing.Color.Red;
            this.lblStatusClient4.Location = new System.Drawing.Point(96, 17);
            this.lblStatusClient4.Name = "lblStatusClient4";
            this.lblStatusClient4.Size = new System.Drawing.Size(20, 22);
            this.lblStatusClient4.TabIndex = 43;
            this.lblStatusClient4.Tag = "3";
            // 
            // lblStatusClient5
            // 
            this.lblStatusClient5.BackColor = System.Drawing.Color.Red;
            this.lblStatusClient5.Location = new System.Drawing.Point(123, 17);
            this.lblStatusClient5.Name = "lblStatusClient5";
            this.lblStatusClient5.Size = new System.Drawing.Size(20, 22);
            this.lblStatusClient5.TabIndex = 42;
            this.lblStatusClient5.Tag = "4";
            // 
            // lblStatusClient6
            // 
            this.lblStatusClient6.BackColor = System.Drawing.Color.Red;
            this.lblStatusClient6.Location = new System.Drawing.Point(150, 17);
            this.lblStatusClient6.Name = "lblStatusClient6";
            this.lblStatusClient6.Size = new System.Drawing.Size(20, 22);
            this.lblStatusClient6.TabIndex = 41;
            this.lblStatusClient6.Tag = "5";
            // 
            // lblStatusClient7
            // 
            this.lblStatusClient7.BackColor = System.Drawing.Color.Red;
            this.lblStatusClient7.Location = new System.Drawing.Point(177, 17);
            this.lblStatusClient7.Name = "lblStatusClient7";
            this.lblStatusClient7.Size = new System.Drawing.Size(20, 22);
            this.lblStatusClient7.TabIndex = 40;
            this.lblStatusClient7.Tag = "6";
            // 
            // lblStatusClient8
            // 
            this.lblStatusClient8.BackColor = System.Drawing.Color.Red;
            this.lblStatusClient8.Location = new System.Drawing.Point(204, 17);
            this.lblStatusClient8.Name = "lblStatusClient8";
            this.lblStatusClient8.Size = new System.Drawing.Size(20, 22);
            this.lblStatusClient8.TabIndex = 39;
            this.lblStatusClient8.Tag = "7";
            // 
            // lblStatusClient1
            // 
            this.lblStatusClient1.BackColor = System.Drawing.Color.Red;
            this.lblStatusClient1.Location = new System.Drawing.Point(15, 17);
            this.lblStatusClient1.Name = "lblStatusClient1";
            this.lblStatusClient1.Size = new System.Drawing.Size(20, 22);
            this.lblStatusClient1.TabIndex = 38;
            this.lblStatusClient1.Tag = "0";
            // 
            // FrmClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightCyan;
            this.ClientSize = new System.Drawing.Size(798, 446);
            this.Controls.Add(this.groupStatus);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.Name = "FrmClient";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmClient_FormClosing);
            this.Load += new System.EventHandler(this.FrmClient_Load);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupStatus.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.GroupBox groupBox5;
        public System.Windows.Forms.TextBox txtReceive;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnSend;
        public System.Windows.Forms.TextBox txtSend;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnConnect;
        public System.Windows.Forms.TextBox txtRemoteIP;
        public System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cobLocalIP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupStatus;
        private System.Windows.Forms.Label lblStatusClient2;
        private System.Windows.Forms.Label lblStatusClient3;
        private System.Windows.Forms.Label lblStatusClient4;
        private System.Windows.Forms.Label lblStatusClient5;
        private System.Windows.Forms.Label lblStatusClient6;
        private System.Windows.Forms.Label lblStatusClient7;
        private System.Windows.Forms.Label lblStatusClient8;
        private System.Windows.Forms.Label lblStatusClient1;
    }
}