namespace DataConv.WinForm
{
    partial class MainWinForm
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
            this.lblStartTime = new System.Windows.Forms.Label();
            this.dtpStartTime = new System.Windows.Forms.DateTimePicker();
            this.lblEndTime = new System.Windows.Forms.Label();
            this.dtpEndTime = new System.Windows.Forms.DateTimePicker();
            this.lblBatchCommit = new System.Windows.Forms.Label();
            this.cmbBatchCommit = new System.Windows.Forms.ComboBox();
            this.lblProvince = new System.Windows.Forms.Label();
            this.cmbProvince = new System.Windows.Forms.ComboBox();
            this.lblCity = new System.Windows.Forms.Label();
            this.cmbCity = new System.Windows.Forms.ComboBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnShutdown = new System.Windows.Forms.Button();
            this.lblNotice = new System.Windows.Forms.Label();
            this.lblNoticeInfo = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // lblStartTime
            // 
            this.lblStartTime.AutoSize = true;
            this.lblStartTime.Location = new System.Drawing.Point(13, 25);
            this.lblStartTime.Name = "lblStartTime";
            this.lblStartTime.Size = new System.Drawing.Size(59, 12);
            this.lblStartTime.TabIndex = 0;
            this.lblStartTime.Text = "开始时间:";
            // 
            // dtpStartTime
            // 
            this.dtpStartTime.Location = new System.Drawing.Point(78, 19);
            this.dtpStartTime.Name = "dtpStartTime";
            this.dtpStartTime.Size = new System.Drawing.Size(115, 21);
            this.dtpStartTime.TabIndex = 1;
            // 
            // lblEndTime
            // 
            this.lblEndTime.AutoSize = true;
            this.lblEndTime.Location = new System.Drawing.Point(217, 24);
            this.lblEndTime.Name = "lblEndTime";
            this.lblEndTime.Size = new System.Drawing.Size(59, 12);
            this.lblEndTime.TabIndex = 2;
            this.lblEndTime.Text = "结束时间:";
            // 
            // dtpEndTime
            // 
            this.dtpEndTime.Location = new System.Drawing.Point(283, 18);
            this.dtpEndTime.Name = "dtpEndTime";
            this.dtpEndTime.Size = new System.Drawing.Size(115, 21);
            this.dtpEndTime.TabIndex = 3;
            // 
            // lblBatchCommit
            // 
            this.lblBatchCommit.AutoSize = true;
            this.lblBatchCommit.Location = new System.Drawing.Point(426, 24);
            this.lblBatchCommit.Name = "lblBatchCommit";
            this.lblBatchCommit.Size = new System.Drawing.Size(59, 12);
            this.lblBatchCommit.TabIndex = 4;
            this.lblBatchCommit.Text = "批量入库:";
            // 
            // cmbBatchCommit
            // 
            this.cmbBatchCommit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBatchCommit.FormattingEnabled = true;
            this.cmbBatchCommit.Items.AddRange(new object[] {
            "10",
            "20",
            "30"});
            this.cmbBatchCommit.Location = new System.Drawing.Point(492, 18);
            this.cmbBatchCommit.Name = "cmbBatchCommit";
            this.cmbBatchCommit.Size = new System.Drawing.Size(58, 20);
            this.cmbBatchCommit.TabIndex = 5;
            // 
            // lblProvince
            // 
            this.lblProvince.AutoSize = true;
            this.lblProvince.Location = new System.Drawing.Point(37, 58);
            this.lblProvince.Name = "lblProvince";
            this.lblProvince.Size = new System.Drawing.Size(35, 12);
            this.lblProvince.TabIndex = 6;
            this.lblProvince.Text = "省份:";
            // 
            // cmbProvince
            // 
            this.cmbProvince.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProvince.FormattingEnabled = true;
            this.cmbProvince.Location = new System.Drawing.Point(78, 55);
            this.cmbProvince.Name = "cmbProvince";
            this.cmbProvince.Size = new System.Drawing.Size(115, 20);
            this.cmbProvince.TabIndex = 7;
            // 
            // lblCity
            // 
            this.lblCity.AutoSize = true;
            this.lblCity.Location = new System.Drawing.Point(241, 58);
            this.lblCity.Name = "lblCity";
            this.lblCity.Size = new System.Drawing.Size(35, 12);
            this.lblCity.TabIndex = 8;
            this.lblCity.Text = "城市:";
            // 
            // cmbCity
            // 
            this.cmbCity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCity.FormattingEnabled = true;
            this.cmbCity.Location = new System.Drawing.Point(283, 55);
            this.cmbCity.Name = "cmbCity";
            this.cmbCity.Size = new System.Drawing.Size(115, 20);
            this.cmbCity.TabIndex = 9;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(428, 52);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 10;
            this.btnStart.Text = "开始";
            this.btnStart.UseVisualStyleBackColor = true;
            // 
            // btnShutdown
            // 
            this.btnShutdown.Location = new System.Drawing.Point(521, 52);
            this.btnShutdown.Name = "btnShutdown";
            this.btnShutdown.Size = new System.Drawing.Size(75, 23);
            this.btnShutdown.TabIndex = 11;
            this.btnShutdown.Text = "结束";
            this.btnShutdown.UseVisualStyleBackColor = true;
            // 
            // lblNotice
            // 
            this.lblNotice.AutoSize = true;
            this.lblNotice.Location = new System.Drawing.Point(37, 95);
            this.lblNotice.Name = "lblNotice";
            this.lblNotice.Size = new System.Drawing.Size(35, 12);
            this.lblNotice.TabIndex = 12;
            this.lblNotice.Text = "提示:";
            // 
            // lblNoticeInfo
            // 
            this.lblNoticeInfo.AutoSize = true;
            this.lblNoticeInfo.Location = new System.Drawing.Point(76, 95);
            this.lblNoticeInfo.Name = "lblNoticeInfo";
            this.lblNoticeInfo.Size = new System.Drawing.Size(0, 12);
            this.lblNoticeInfo.TabIndex = 13;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(15, 130);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(581, 338);
            this.richTextBox1.TabIndex = 14;
            this.richTextBox1.Text = "";
            // 
            // MainWinForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(608, 480);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.lblNoticeInfo);
            this.Controls.Add(this.lblNotice);
            this.Controls.Add(this.btnShutdown);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.cmbCity);
            this.Controls.Add(this.lblCity);
            this.Controls.Add(this.cmbProvince);
            this.Controls.Add(this.lblProvince);
            this.Controls.Add(this.cmbBatchCommit);
            this.Controls.Add(this.lblBatchCommit);
            this.Controls.Add(this.dtpEndTime);
            this.Controls.Add(this.lblEndTime);
            this.Controls.Add(this.dtpStartTime);
            this.Controls.Add(this.lblStartTime);
            this.Name = "MainWinForm";
            this.Text = "MainWinForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblStartTime;
        private System.Windows.Forms.DateTimePicker dtpStartTime;
        private System.Windows.Forms.Label lblEndTime;
        private System.Windows.Forms.DateTimePicker dtpEndTime;
        private System.Windows.Forms.Label lblBatchCommit;
        private System.Windows.Forms.ComboBox cmbBatchCommit;
        private System.Windows.Forms.Label lblProvince;
        private System.Windows.Forms.ComboBox cmbProvince;
        private System.Windows.Forms.Label lblCity;
        private System.Windows.Forms.ComboBox cmbCity;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnShutdown;
        private System.Windows.Forms.Label lblNotice;
        private System.Windows.Forms.Label lblNoticeInfo;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}