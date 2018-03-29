namespace FxtCollateralTimer
{
    partial class Fxt
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.textMessage = new System.Windows.Forms.TextBox();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.dgvTack = new System.Windows.Forms.DataGridView();
            this.labNetWork = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTack)).BeginInit();
            this.SuspendLayout();
            // 
            // textMessage
            // 
            this.textMessage.Location = new System.Drawing.Point(1049, 38);
            this.textMessage.Multiline = true;
            this.textMessage.Name = "textMessage";
            this.textMessage.Size = new System.Drawing.Size(123, 208);
            this.textMessage.TabIndex = 1;
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "notifyIcon";
            this.notifyIcon1.Visible = true;
            // 
            // dgvTack
            // 
            this.dgvTack.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTack.Location = new System.Drawing.Point(4, 41);
            this.dgvTack.Name = "dgvTack";
            this.dgvTack.RowTemplate.Height = 23;
            this.dgvTack.Size = new System.Drawing.Size(1039, 204);
            this.dgvTack.TabIndex = 2;
            // 
            // labNetWork
            // 
            this.labNetWork.AutoSize = true;
            this.labNetWork.Location = new System.Drawing.Point(145, 19);
            this.labNetWork.Name = "labNetWork";
            this.labNetWork.Size = new System.Drawing.Size(0, 12);
            this.labNetWork.TabIndex = 3;
            // 
            // Fxt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 258);
            this.Controls.Add(this.labNetWork);
            this.Controls.Add(this.dgvTack);
            this.Controls.Add(this.textMessage);
            this.Name = "Fxt";
            this.Text = "房讯通定时任务器";
            ((System.ComponentModel.ISupportInitialize)(this.dgvTack)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textMessage;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.DataGridView dgvTack;
        private System.Windows.Forms.Label labNetWork;
    }
}

