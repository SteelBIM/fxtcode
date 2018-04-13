namespace Kingsun.ExamPaper.ImportTool
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnChoosePath = new System.Windows.Forms.Button();
            this.txtFileUrl = new System.Windows.Forms.TextBox();
            this.btnLoadPath = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.listUnit = new System.Windows.Forms.ListBox();
            this.rtbMessage = new System.Windows.Forms.RichTextBox();
            this.btnChooseAll = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnChoosePath
            // 
            this.btnChoosePath.Location = new System.Drawing.Point(12, 12);
            this.btnChoosePath.Name = "btnChoosePath";
            this.btnChoosePath.Size = new System.Drawing.Size(110, 47);
            this.btnChoosePath.TabIndex = 0;
            this.btnChoosePath.Text = "选择路径";
            this.btnChoosePath.UseVisualStyleBackColor = true;
            this.btnChoosePath.Click += new System.EventHandler(this.btnChoosePath_Click);
            // 
            // txtFileUrl
            // 
            this.txtFileUrl.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtFileUrl.Location = new System.Drawing.Point(247, 19);
            this.txtFileUrl.Name = "txtFileUrl";
            this.txtFileUrl.Size = new System.Drawing.Size(715, 30);
            this.txtFileUrl.TabIndex = 1;
            // 
            // btnLoadPath
            // 
            this.btnLoadPath.Location = new System.Drawing.Point(128, 12);
            this.btnLoadPath.Name = "btnLoadPath";
            this.btnLoadPath.Size = new System.Drawing.Size(110, 47);
            this.btnLoadPath.TabIndex = 2;
            this.btnLoadPath.Text = "读取单元";
            this.btnLoadPath.UseVisualStyleBackColor = true;
            this.btnLoadPath.Click += new System.EventHandler(this.btnLoadPath_Click);
            // 
            // btnUp
            // 
            this.btnUp.Location = new System.Drawing.Point(12, 65);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(71, 41);
            this.btnUp.TabIndex = 3;
            this.btnUp.Text = "上移";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDown
            // 
            this.btnDown.Location = new System.Drawing.Point(89, 65);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(68, 41);
            this.btnDown.TabIndex = 4;
            this.btnDown.Text = "下移";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnImport
            // 
            this.btnImport.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnImport.Location = new System.Drawing.Point(852, 65);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(110, 41);
            this.btnImport.TabIndex = 6;
            this.btnImport.Text = "开始导入";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // listUnit
            // 
            this.listUnit.BackColor = System.Drawing.SystemColors.HighlightText;
            this.listUnit.FormattingEnabled = true;
            this.listUnit.ItemHeight = 12;
            this.listUnit.Location = new System.Drawing.Point(12, 112);
            this.listUnit.MultiColumn = true;
            this.listUnit.Name = "listUnit";
            this.listUnit.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listUnit.Size = new System.Drawing.Size(226, 472);
            this.listUnit.TabIndex = 7;
            // 
            // rtbMessage
            // 
            this.rtbMessage.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.rtbMessage.Location = new System.Drawing.Point(247, 112);
            this.rtbMessage.Name = "rtbMessage";
            this.rtbMessage.ReadOnly = true;
            this.rtbMessage.Size = new System.Drawing.Size(715, 472);
            this.rtbMessage.TabIndex = 8;
            this.rtbMessage.Text = "";
            // 
            // btnChooseAll
            // 
            this.btnChooseAll.Location = new System.Drawing.Point(163, 65);
            this.btnChooseAll.Name = "btnChooseAll";
            this.btnChooseAll.Size = new System.Drawing.Size(75, 41);
            this.btnChooseAll.TabIndex = 10;
            this.btnChooseAll.Text = "全选";
            this.btnChooseAll.UseVisualStyleBackColor = true;
            this.btnChooseAll.Click += new System.EventHandler(this.btnChooseAll_Click);
            // 
            // btnClear
            // 
            this.btnClear.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClear.Location = new System.Drawing.Point(247, 65);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(110, 41);
            this.btnClear.TabIndex = 11;
            this.btnClear.Text = "清空全部";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(974, 599);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnChooseAll);
            this.Controls.Add(this.rtbMessage);
            this.Controls.Add(this.listUnit);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.btnLoadPath);
            this.Controls.Add(this.txtFileUrl);
            this.Controls.Add(this.btnChoosePath);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "53天天练V1.0工具";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnChoosePath;
        private System.Windows.Forms.TextBox txtFileUrl;
        private System.Windows.Forms.Button btnLoadPath;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.ListBox listUnit;
        private System.Windows.Forms.RichTextBox rtbMessage;
        private System.Windows.Forms.Button btnChooseAll;
        private System.Windows.Forms.Button btnClear;
    }
}

