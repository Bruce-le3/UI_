namespace Justech
{
    partial class ResultSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResultSetting));
            this.cobID = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtUCL = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtLCL = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnWrite = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cobID
            // 
            this.cobID.FormattingEnabled = true;
            this.cobID.Location = new System.Drawing.Point(155, 81);
            this.cobID.Name = "cobID";
            this.cobID.Size = new System.Drawing.Size(121, 20);
            this.cobID.TabIndex = 0;
            this.cobID.SelectedIndexChanged += new System.EventHandler(this.cobID_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(120, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "ID:";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("宋体", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(125, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(162, 39);
            this.label2.TabIndex = 2;
            this.label2.Text = "结果设置";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(106, 128);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "Name:";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(155, 125);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(121, 21);
            this.txtName.TabIndex = 4;
            // 
            // txtUCL
            // 
            this.txtUCL.Location = new System.Drawing.Point(155, 176);
            this.txtUCL.Name = "txtUCL";
            this.txtUCL.Size = new System.Drawing.Size(121, 21);
            this.txtUCL.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(113, 179);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "UCL:";
            // 
            // txtLCL
            // 
            this.txtLCL.Location = new System.Drawing.Point(155, 223);
            this.txtLCL.Name = "txtLCL";
            this.txtLCL.Size = new System.Drawing.Size(121, 21);
            this.txtLCL.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(113, 226);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 12);
            this.label5.TabIndex = 7;
            this.label5.Text = "LCL:";
            // 
            // btnWrite
            // 
            this.btnWrite.Location = new System.Drawing.Point(165, 267);
            this.btnWrite.Name = "btnWrite";
            this.btnWrite.Size = new System.Drawing.Size(99, 34);
            this.btnWrite.TabIndex = 9;
            this.btnWrite.Text = "写入";
            this.btnWrite.UseVisualStyleBackColor = true;
            this.btnWrite.Click += new System.EventHandler(this.btnWrite_Click);
            // 
            // ResultSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightBlue;
            this.ClientSize = new System.Drawing.Size(422, 365);
            this.Controls.Add(this.btnWrite);
            this.Controls.Add(this.txtLCL);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtUCL);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cobID);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ResultSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ResultSetting";
            this.Load += new System.EventHandler(this.ResultSetting_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cobID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtUCL;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtLCL;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnWrite;
    }
}