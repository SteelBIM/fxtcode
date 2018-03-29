namespace FXTExcelAddIn
{
    partial class UC_CitySelect
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lblStatus = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.btnGetCity = new System.Windows.Forms.Button();
            this.cbProvince = new System.Windows.Forms.ComboBox();
            this.btnGetProvine = new System.Windows.Forms.Button();
            this.cbCity = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblStatus
            // 
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.Location = new System.Drawing.Point(0, 129);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(283, 250);
            this.lblStatus.TabIndex = 7;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.btnGetCity);
            this.panel1.Controls.Add(this.cbProvince);
            this.panel1.Controls.Add(this.btnGetProvine);
            this.panel1.Controls.Add(this.cbCity);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(283, 129);
            this.panel1.TabIndex = 8;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(44, 84);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(121, 23);
            this.button1.TabIndex = 38;
            this.button1.Text = "确定";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnGetCity
            // 
            this.btnGetCity.Location = new System.Drawing.Point(172, 48);
            this.btnGetCity.Name = "btnGetCity";
            this.btnGetCity.Size = new System.Drawing.Size(40, 23);
            this.btnGetCity.TabIndex = 37;
            this.btnGetCity.Text = "重试";
            this.btnGetCity.UseVisualStyleBackColor = true;
            this.btnGetCity.Click += new System.EventHandler(this.btnGetCity_Click);
            // 
            // cbProvince
            // 
            this.cbProvince.FormattingEnabled = true;
            this.cbProvince.Location = new System.Drawing.Point(44, 18);
            this.cbProvince.Name = "cbProvince";
            this.cbProvince.Size = new System.Drawing.Size(121, 20);
            this.cbProvince.TabIndex = 32;
            this.cbProvince.SelectedIndexChanged += new System.EventHandler(this.cbProvince_SelectedIndexChanged);
            // 
            // btnGetProvine
            // 
            this.btnGetProvine.Location = new System.Drawing.Point(172, 16);
            this.btnGetProvine.Name = "btnGetProvine";
            this.btnGetProvine.Size = new System.Drawing.Size(40, 23);
            this.btnGetProvine.TabIndex = 36;
            this.btnGetProvine.Text = "重试";
            this.btnGetProvine.UseVisualStyleBackColor = true;
            this.btnGetProvine.Click += new System.EventHandler(this.btnGetProvine_Click);
            // 
            // cbCity
            // 
            this.cbCity.FormattingEnabled = true;
            this.cbCity.Location = new System.Drawing.Point(44, 50);
            this.cbCity.Name = "cbCity";
            this.cbCity.Size = new System.Drawing.Size(121, 20);
            this.cbCity.TabIndex = 33;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 34;
            this.label1.Text = "省";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 35;
            this.label2.Text = "市";
            // 
            // UC_CitySelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.panel1);
            this.Name = "UC_CitySelect";
            this.Size = new System.Drawing.Size(283, 379);
            this.Load += new System.EventHandler(this.UC_CitySelect_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnGetCity;
        private System.Windows.Forms.ComboBox cbProvince;
        private System.Windows.Forms.Button btnGetProvine;
        private System.Windows.Forms.ComboBox cbCity;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}
