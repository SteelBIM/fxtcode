using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;

namespace SqlTCP
{
	/// <summary>
	/// Form1 的摘要说明。
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TreeView TList;
		private string TableName="",ShowTableName="";
        string applicationPath = System.AppDomain.CurrentDomain.BaseDirectory;
        List<string> ShowTableNames = new List<string>();

		private int Rows=0,Cols=0;
		private bool IsWait;//是否等待中
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItem5;
        private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem menuItem8;
		private System.Windows.Forms.MenuItem menuItem9;
        private System.Windows.Forms.MenuItem menuItem10;
		private System.Windows.Forms.MenuItem menuItem12;
        private System.Windows.Forms.MenuItem menuItem13;
		private System.Windows.Forms.MenuItem menuItem15;
        private System.Windows.Forms.MenuItem menuItem16;
        private Button button1;
        private TabControl tabControl1;
        private TabPage tabPage7;
        private DataGrid DList;
        private TabPage tabPage1;
        private RichTextBox txtCode;
        private TabPage tabPage5;
        private RichTextBox txtDataRequest;
        private TabPage tabPage3;
        private RichTextBox txtWeb;
        private TabPage tabPage8;
        private RichTextBox txtSQL;
        private TabPage tabPage9;
        private RichTextBox txtJSON;
        private TabPage tabPage10;
        private TabPage tabPage11;
        private RichTextBox txtDA;
        private RichTextBox txtLogic;
        private TabPage tabPage12;
        private RichTextBox txtFormField;
        private Button button3;
        private Button button2;
        private Button button4;
        private TabPage tabPage13;
        private TabPage tabPage2;
        private RichTextBox txtDataRequestDelete;
        private RichTextBox txtShowPage;
        private IContainer components;

		public Form1()
		{
			InitializeComponent();
		}
		
		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows 窗体设计器生成的代码
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panel1 = new System.Windows.Forms.Panel();
            this.TList = new System.Windows.Forms.TreeView();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.DList = new System.Windows.Forms.DataGrid();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.txtCode = new System.Windows.Forms.RichTextBox();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.txtDataRequest = new System.Windows.Forms.RichTextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txtDataRequestDelete = new System.Windows.Forms.RichTextBox();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.txtSQL = new System.Windows.Forms.RichTextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.txtWeb = new System.Windows.Forms.RichTextBox();
            this.tabPage12 = new System.Windows.Forms.TabPage();
            this.txtFormField = new System.Windows.Forms.RichTextBox();
            this.tabPage13 = new System.Windows.Forms.TabPage();
            this.tabPage9 = new System.Windows.Forms.TabPage();
            this.txtJSON = new System.Windows.Forms.RichTextBox();
            this.tabPage10 = new System.Windows.Forms.TabPage();
            this.txtDA = new System.Windows.Forms.RichTextBox();
            this.tabPage11 = new System.Windows.Forms.TabPage();
            this.txtLogic = new System.Windows.Forms.RichTextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem15 = new System.Windows.Forms.MenuItem();
            this.menuItem16 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this.menuItem9 = new System.Windows.Forms.MenuItem();
            this.menuItem10 = new System.Windows.Forms.MenuItem();
            this.menuItem12 = new System.Windows.Forms.MenuItem();
            this.menuItem13 = new System.Windows.Forms.MenuItem();
            this.txtShowPage = new System.Windows.Forms.RichTextBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DList)).BeginInit();
            this.tabPage1.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage8.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage12.SuspendLayout();
            this.tabPage13.SuspendLayout();
            this.tabPage9.SuspendLayout();
            this.tabPage10.SuspendLayout();
            this.tabPage11.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.TList);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5, 5, 0, 5);
            this.panel1.Size = new System.Drawing.Size(226, 533);
            this.panel1.TabIndex = 2;
            // 
            // TList
            // 
            this.TList.BackColor = System.Drawing.SystemColors.Window;
            this.TList.CheckBoxes = true;
            this.TList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TList.Location = new System.Drawing.Point(5, 5);
            this.TList.Name = "TList";
            this.TList.Size = new System.Drawing.Size(221, 523);
            this.TList.TabIndex = 2;
            this.TList.BeforeCheck += new System.Windows.Forms.TreeViewCancelEventHandler(this.TList_BeforeCheck);
            this.TList.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.TList_AfterCheck);
            this.TList.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.TList_BeforeSelect);
            this.TList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TList_AfterSelect);
            this.TList.DoubleClick += new System.EventHandler(this.TList_DoubleClick);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(226, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(5, 533);
            this.splitter1.TabIndex = 13;
            this.splitter1.TabStop = false;
            this.splitter1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitter1_SplitterMoved);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tabControl1);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(231, 0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(0, 5, 5, 5);
            this.panel2.Size = new System.Drawing.Size(601, 533);
            this.panel2.TabIndex = 15;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage7);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage8);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage12);
            this.tabControl1.Controls.Add(this.tabPage13);
            this.tabControl1.Controls.Add(this.tabPage9);
            this.tabControl1.Controls.Add(this.tabPage10);
            this.tabControl1.Controls.Add(this.tabPage11);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 5);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(596, 472);
            this.tabControl1.TabIndex = 21;
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.DList);
            this.tabPage7.Location = new System.Drawing.Point(4, 21);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Size = new System.Drawing.Size(588, 447);
            this.tabPage7.TabIndex = 6;
            this.tabPage7.Text = "表结构";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // DList
            // 
            this.DList.AllowNavigation = false;
            this.DList.AllowSorting = false;
            this.DList.BackgroundColor = System.Drawing.SystemColors.InactiveCaption;
            this.DList.DataMember = "";
            this.DList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DList.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.DList.Location = new System.Drawing.Point(0, 0);
            this.DList.Name = "DList";
            this.DList.Size = new System.Drawing.Size(588, 447);
            this.DList.TabIndex = 21;
            this.DList.Navigate += new System.Windows.Forms.NavigateEventHandler(this.DList_Navigate);
            this.DList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DList_MouseDown);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.txtCode);
            this.tabPage1.Location = new System.Drawing.Point(4, 21);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(588, 447);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "实体类";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // txtCode
            // 
            this.txtCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCode.Location = new System.Drawing.Point(3, 3);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(582, 441);
            this.txtCode.TabIndex = 1;
            this.txtCode.Text = "";
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.txtDataRequest);
            this.tabPage5.Location = new System.Drawing.Point(4, 21);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(588, 447);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "新增编辑API";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // txtDataRequest
            // 
            this.txtDataRequest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDataRequest.Location = new System.Drawing.Point(0, 0);
            this.txtDataRequest.Name = "txtDataRequest";
            this.txtDataRequest.Size = new System.Drawing.Size(588, 447);
            this.txtDataRequest.TabIndex = 4;
            this.txtDataRequest.Text = "";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.txtDataRequestDelete);
            this.tabPage2.Location = new System.Drawing.Point(4, 21);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(588, 447);
            this.tabPage2.TabIndex = 13;
            this.tabPage2.Text = "删除API";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // txtDataRequestDelete
            // 
            this.txtDataRequestDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDataRequestDelete.Location = new System.Drawing.Point(0, 0);
            this.txtDataRequestDelete.Name = "txtDataRequestDelete";
            this.txtDataRequestDelete.Size = new System.Drawing.Size(588, 447);
            this.txtDataRequestDelete.TabIndex = 0;
            this.txtDataRequestDelete.Text = "";
            // 
            // tabPage8
            // 
            this.tabPage8.Controls.Add(this.txtSQL);
            this.tabPage8.Location = new System.Drawing.Point(4, 21);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Size = new System.Drawing.Size(588, 447);
            this.tabPage8.TabIndex = 7;
            this.tabPage8.Text = "SQL语句";
            this.tabPage8.UseVisualStyleBackColor = true;
            // 
            // txtSQL
            // 
            this.txtSQL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSQL.Location = new System.Drawing.Point(0, 0);
            this.txtSQL.Name = "txtSQL";
            this.txtSQL.Size = new System.Drawing.Size(588, 447);
            this.txtSQL.TabIndex = 3;
            this.txtSQL.Text = "";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.txtWeb);
            this.tabPage3.Location = new System.Drawing.Point(4, 21);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(588, 447);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "列表页面";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // txtWeb
            // 
            this.txtWeb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtWeb.Location = new System.Drawing.Point(0, 0);
            this.txtWeb.Name = "txtWeb";
            this.txtWeb.Size = new System.Drawing.Size(588, 447);
            this.txtWeb.TabIndex = 2;
            this.txtWeb.Text = "";
            // 
            // tabPage12
            // 
            this.tabPage12.Controls.Add(this.txtFormField);
            this.tabPage12.Location = new System.Drawing.Point(4, 21);
            this.tabPage12.Name = "tabPage12";
            this.tabPage12.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage12.Size = new System.Drawing.Size(588, 447);
            this.tabPage12.TabIndex = 11;
            this.tabPage12.Text = "编辑页面";
            this.tabPage12.UseVisualStyleBackColor = true;
            // 
            // txtFormField
            // 
            this.txtFormField.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFormField.Location = new System.Drawing.Point(3, 3);
            this.txtFormField.Name = "txtFormField";
            this.txtFormField.Size = new System.Drawing.Size(582, 441);
            this.txtFormField.TabIndex = 5;
            this.txtFormField.Text = "";
            // 
            // tabPage13
            // 
            this.tabPage13.Controls.Add(this.txtShowPage);
            this.tabPage13.Location = new System.Drawing.Point(4, 21);
            this.tabPage13.Name = "tabPage13";
            this.tabPage13.Size = new System.Drawing.Size(588, 447);
            this.tabPage13.TabIndex = 12;
            this.tabPage13.Text = "显示页面";
            this.tabPage13.UseVisualStyleBackColor = true;
            // 
            // tabPage9
            // 
            this.tabPage9.Controls.Add(this.txtJSON);
            this.tabPage9.Location = new System.Drawing.Point(4, 21);
            this.tabPage9.Name = "tabPage9";
            this.tabPage9.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage9.Size = new System.Drawing.Size(588, 447);
            this.tabPage9.TabIndex = 8;
            this.tabPage9.Text = "JSON";
            this.tabPage9.UseVisualStyleBackColor = true;
            // 
            // txtJSON
            // 
            this.txtJSON.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtJSON.Location = new System.Drawing.Point(3, 3);
            this.txtJSON.Name = "txtJSON";
            this.txtJSON.Size = new System.Drawing.Size(582, 441);
            this.txtJSON.TabIndex = 3;
            this.txtJSON.Text = "";
            // 
            // tabPage10
            // 
            this.tabPage10.Controls.Add(this.txtDA);
            this.tabPage10.Location = new System.Drawing.Point(4, 21);
            this.tabPage10.Name = "tabPage10";
            this.tabPage10.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage10.Size = new System.Drawing.Size(588, 447);
            this.tabPage10.TabIndex = 9;
            this.tabPage10.Text = "DA";
            this.tabPage10.UseVisualStyleBackColor = true;
            // 
            // txtDA
            // 
            this.txtDA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDA.Location = new System.Drawing.Point(3, 3);
            this.txtDA.Name = "txtDA";
            this.txtDA.Size = new System.Drawing.Size(582, 441);
            this.txtDA.TabIndex = 4;
            this.txtDA.Text = "";
            // 
            // tabPage11
            // 
            this.tabPage11.Controls.Add(this.txtLogic);
            this.tabPage11.Location = new System.Drawing.Point(4, 21);
            this.tabPage11.Name = "tabPage11";
            this.tabPage11.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage11.Size = new System.Drawing.Size(588, 447);
            this.tabPage11.TabIndex = 10;
            this.tabPage11.Text = "Logic";
            this.tabPage11.UseVisualStyleBackColor = true;
            // 
            // txtLogic
            // 
            this.txtLogic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLogic.Location = new System.Drawing.Point(3, 3);
            this.txtLogic.Name = "txtLogic";
            this.txtLogic.Size = new System.Drawing.Size(582, 441);
            this.txtLogic.TabIndex = 4;
            this.txtLogic.Text = "";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.button4);
            this.panel4.Controls.Add(this.button3);
            this.panel4.Controls.Add(this.button2);
            this.panel4.Controls.Add(this.button1);
            this.panel4.Controls.Add(this.lblStatus);
            this.panel4.Controls.Add(this.progressBar1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 477);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(596, 51);
            this.panel4.TabIndex = 18;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(251, 3);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 13;
            this.button4.Text = "批量生成";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(170, 4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 12;
            this.button3.Text = "全不选";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(88, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 11;
            this.button2.Text = "全选";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "生成代码";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatus.Location = new System.Drawing.Point(0, -4);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(596, 39);
            this.lblStatus.TabIndex = 9;
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar1.Location = new System.Drawing.Point(0, 35);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(596, 16);
            this.progressBar1.TabIndex = 8;
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem4,
            this.menuItem8});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem2,
            this.menuItem3,
            this.menuItem15,
            this.menuItem16});
            this.menuItem1.Text = "(&F)文件";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 0;
            this.menuItem2.Shortcut = System.Windows.Forms.Shortcut.F5;
            this.menuItem2.Text = "(&G)获取Table列表";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 1;
            this.menuItem3.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
            this.menuItem3.Text = "(&S)保存字段说明";
            this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
            // 
            // menuItem15
            // 
            this.menuItem15.Index = 2;
            this.menuItem15.Text = "-";
            // 
            // menuItem16
            // 
            this.menuItem16.Index = 3;
            this.menuItem16.Text = "(&X)退出";
            this.menuItem16.Click += new System.EventHandler(this.menuItem16_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 1;
            this.menuItem4.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem5,
            this.menuItem6});
            this.menuItem4.Text = "(&D)文档";
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 0;
            this.menuItem5.Text = "生成当前表";
            this.menuItem5.Click += new System.EventHandler(this.menuItem5_Click);
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 1;
            this.menuItem6.Text = "生成多个表";
            this.menuItem6.Click += new System.EventHandler(this.menuItem6_Click);
            // 
            // menuItem8
            // 
            this.menuItem8.Index = 2;
            this.menuItem8.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem9,
            this.menuItem10,
            this.menuItem12,
            this.menuItem13});
            this.menuItem8.Text = "(&P)字段说明";
            // 
            // menuItem9
            // 
            this.menuItem9.Index = 0;
            this.menuItem9.Text = "备份当前表";
            this.menuItem9.Click += new System.EventHandler(this.menuItem9_Click);
            // 
            // menuItem10
            // 
            this.menuItem10.Index = 1;
            this.menuItem10.Text = "备份多个表";
            this.menuItem10.Click += new System.EventHandler(this.menuItem10_Click);
            // 
            // menuItem12
            // 
            this.menuItem12.Index = 2;
            this.menuItem12.Text = "-";
            // 
            // menuItem13
            // 
            this.menuItem13.Index = 3;
            this.menuItem13.Text = "(&R)还原";
            this.menuItem13.Click += new System.EventHandler(this.menuItem13_Click);
            // 
            // txtShowPage
            // 
            this.txtShowPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtShowPage.Location = new System.Drawing.Point(0, 0);
            this.txtShowPage.Name = "txtShowPage";
            this.txtShowPage.Size = new System.Drawing.Size(588, 447);
            this.txtShowPage.TabIndex = 0;
            this.txtShowPage.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(832, 533);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu1;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SQL Server 2005 辅助工具";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DList)).EndInit();
            this.tabPage1.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage8.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage12.ResumeLayout(false);
            this.tabPage13.ResumeLayout(false);
            this.tabPage9.ResumeLayout(false);
            this.tabPage10.ResumeLayout(false);
            this.tabPage11.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
            BindDataBaseList();
		}

        #region 绑定数据库列表
        private void BindDataBaseList()
        {
            try
            {
                DataSet ds = TableColumns.Exec_P_DataBaseList();
                TList.Nodes.Clear();
                TreeNode root = new TreeNode("数据库");
                TList.Nodes.Add(root);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    root.Nodes.Add(ds.Tables[0].Rows[i][0].ToString(),ds.Tables[0].Rows[i][0].ToString());
                }
                TList.ExpandAll();
            }
            catch (Exception ex)
            {
                ShowError(ex.ToString());
            }
        }
        #endregion

		#region 绑定数据库表
		private void BindTableList(TreeNode node)
		{
			try
			{
				DataSet ds=TableColumns.Exec_P_TableList();
				node.Nodes.Clear();
				for(int i=0;i<ds.Tables[0].Rows.Count;i++)
				{
					node.Nodes.Add(ds.Tables[0].Rows[i][0].ToString());
				}
				node.ExpandAll();
			}
			catch(Exception ex){
			ShowError(ex.ToString());
			}
		}
		#endregion

		#region 绑定数据库字段说明到datagrid
		private void BindTableColumns(string table)
		{
			try
			{
				DList.Enabled=true;
				DataSet ds=TableColumns.Exec_P_TableColumnProperty(table);
				DataTable db=ds.Tables[0];
				DList.DataSource=db;
				//禁止新增
				CurrencyManager cm=(CurrencyManager)this.BindingContext[DList.DataSource];
				((DataView)cm.List).AllowNew=false;
				
				Rows=db.Rows.Count;
				Cols=db.Columns.Count;
				//自定义样式
				DataGridTableStyle TableStyle = new DataGridTableStyle();
				TableStyle.MappingName=ds.Tables[0].TableName;
				
				TableStyle.AllowSorting=false;
				TableStyle.GridLineColor=Color.Gray;

                DataGridColumnStyle chk = new DataGridBoolColumn();
                chk.HeaderText = "选择";
                ((DataGridBoolColumn)chk).FalseValue = false;
                ((DataGridBoolColumn)chk).TrueValue = true;
                ((DataGridBoolColumn)chk).AllowNull = false;
                chk.Width = 30;
                chk.MappingName = ds.Tables[0].Columns[0].ColumnName;
                chk.ReadOnly = true;
                TableStyle.GridColumnStyles.Add(chk);
                for (int i = 1; i < Cols; i++)
				{
					DataGridColumnStyle cs;
                    if (ds.Tables[0].Columns[i].ColumnName=="字段说明")
					{
						cs=new SQLColumn(Color.White);
						cs.Width=200;
						cs.ReadOnly=false;
					}
					else
					{
						cs=new SQLColumn(Color.FromName("menu"));
						cs.ReadOnly=true;						
					}
					
					cs.MappingName=ds.Tables[0].Columns[i].ColumnName;
					cs.HeaderText=ds.Tables[0].Columns[i].ColumnName;
					
					TableStyle.GridColumnStyles.Add(cs);
				}
								
				TableName=table;
				DList.CaptionText="表："+ShowTableName;
				DList.TableStyles.Clear();
				DList.TableStyles.Add(TableStyle);
				
			}
			catch(Exception ex) {throw ex;}
		}
		#endregion

		#region 对话框
		private void ShowWarning(string msg)
		{
			MessageBox.Show(msg,"警告",MessageBoxButtons.OK,MessageBoxIcon.Warning);
		}

		private void ShowSuccess(string msg)
		{
			MessageBox.Show(msg,"提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
		}

		private void ShowError(string msg)
		{
			MessageBox.Show(msg,"错误",MessageBoxButtons.OK,MessageBoxIcon.Error);
		}

		private string ShowQuestion(string msg)
		{
			return MessageBox.Show(msg,"询问",MessageBoxButtons.YesNo,MessageBoxIcon.Question).ToString().ToUpper();
		}
		#endregion

		#region 点击树，获取数据库字段说明
		private void TList_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			if(IsWait) return;
			try
			{
				if(e.Node.Parent==null) return;
				setEnabled(false);
                if (e.Node.Level == 1)
                {
                    pubFunc.DefaultDB = e.Node.Text;
                    BindTableList(e.Node);
                }
                else
                {
                    ShowTableName = e.Node.Text;
                    BindTableColumns(GetTableName(ShowTableName));
                }
			}
			catch(Exception ex)
			{
				ShowError(ex.ToString());
			}
			finally
			{
				setEnabled(true);
			}
		}
		#endregion

		#region 更新数据库字段说明
		private void menuItem3_Click(object sender, System.EventArgs e)
		{
			if(TableName=="")
			{
				ShowWarning("请选择要保存的表！");
				return;
			}
			try
			{
				setEnabled(false);
				for(int i=0;i<Rows;i++)
				{
					TableColumns.UpdateColumn(TableName,DList[i,2].ToString(),DList[i,9].ToString().Trim());
				}
				ShowSuccess("保存成功 !");
				
			}
			catch(Exception ex)
			{
				ShowError(ex.ToString());
			}
			finally
			{
				setEnabled(true);
			}
		}
		#endregion

		#region 生成一个表文档到EXCEL
		private void menuItem5_Click(object sender, System.EventArgs e)
		{
			if(TableName=="")
			{
				ShowWarning("请选择要生成文档的表！");
				return;
			}
			try
			{
				string doc=pubFunc.DocPath+TableName+".xls";
				if(File.Exists(doc))
				{
					if(ShowQuestion("文档已经存在，是否覆盖？")=="YES")
						File.Delete(doc);
					else
						return;
				}
				setEnabled(false);
				Export exp=new Export();
				if(exp.ExportExcel(TableName,ShowTableName))
				{
					exp.SaveAs(doc);
				}
				if(ShowQuestion("生成["+TableName+"]成功,是否查看文档?")=="YES")
					exp.ShowExcel();
				else
					exp.Dispose();
			}
			catch(Exception ex)
			{
				ShowError(ex.ToString());
			}
			finally
			{
				setEnabled(true);
			}
		}

		#endregion
		
		#region 生成所有表文档到EXCEL
		private void menuItem7_Click(object sender, System.EventArgs e)
		{
			if(ShowQuestion("确定要生成全部表结构文档吗？")=="YES")
			{
				try
				{
					string doc=pubFunc.DocPath+"All.xls";
					if(File.Exists(doc))
					{
						if(ShowQuestion("文档已经存在，是否覆盖？")=="YES")
							File.Delete(doc);
						else
							return;
					}
					
					setEnabled(false);
					Export exp=new Export();	
					exp.ShtIndex=3;
					int cnt=TList.Nodes[0].Nodes.Count;
					string table;
					setProgress(cnt);
					for(int i=0;i<cnt;i++)
					{
						IAsyncResult ar;
						//table=TList.Nodes[0].Nodes[i].Text;
						table=GetTableName(TList.Nodes[0].Nodes[i].Text);
						DgExport md=new DgExport(exp.ExportExcel);
						ar=md.BeginInvoke(table,TList.Nodes[0].Nodes[i].Text,null,null);
						lblStatus.Text="正在生成文档："+table;
						//异步执行，显示进度条
						while(ar.IsCompleted==false)
						{
							Application.DoEvents();
						}
						md.EndInvoke(ar);
						progressBar1.Value++;
					}
					exp.Init();
					exp.SaveAs(doc);
					if(ShowQuestion("生成文档成功,是否查看文档?")=="YES")
						exp.ShowExcel();
					else
						exp.Dispose();
				}
				catch(Exception ex)
				{
					ShowError(ex.ToString());
					
				}
				finally
				{
					lblStatus.Text="";
					progressBar1.Value=0;
					setEnabled(true);
				}
			}
		}
		#endregion
		
		#region 生成选择的表文档到EXCEL
		private void menuItem6_Click(object sender, System.EventArgs e)
		{
			if(ShowQuestion("确定要生成选择的表结构文档吗？")=="YES")
			{
				try
				{
					ArrayList ary=new ArrayList();
					ArrayList ary1=new ArrayList();
                    TreeNode[] nodes = TList.Nodes.Find(pubFunc.DefaultDB,true);
                    if (nodes.Length <= 0)
                    {
                        ShowWarning("没有选择数据库!");
                        return;
                    }
                    TreeNode node = nodes[0];
					for(int i=0;i<node.Nodes.Count;i++)
					{
						if(node.Nodes[i].Checked==false) continue;
						ary1.Add(node.Nodes[i].Text);
						ary.Add(GetTableName(node.Nodes[i].Text));
					}
					if(ary.Count<=0)
					{
						ShowWarning("没有选择任何表!");
						return;
					}
					string rnd=System.DateTime.Now.ToString("yyyyMMddHHmmss");
					string doc=pubFunc.DocPath+rnd+".xls";
					if(File.Exists(doc))
					{
						if(ShowQuestion("文档已经存在，是否覆盖？")=="YES")
							File.Delete(doc);
						else
							return;
					}
				
					setEnabled(false);
					Export exp=new Export();	
					exp.ShtIndex=3;
					int cnt=ary.Count;
					string table;
					setProgress(cnt+1);
					for(int i=0;i<cnt;i++)
					{
						IAsyncResult ar;
						table=ary[i].ToString();
						DgExport md=new DgExport(exp.ExportExcel);
						ar=md.BeginInvoke(table,ary1[i].ToString(),null,null);
						lblStatus.Text="正在生成文档："+table;
						//异步执行，显示进度条
						while(ar.IsCompleted==false)
						{
							Application.DoEvents();
						}
						md.EndInvoke(ar);
						progressBar1.Value++;
					}
					exp.Init();
					exp.SaveAs(doc);
					if(ShowQuestion("生成文档成功,是否查看文档?")=="YES")
						exp.ShowExcel();
					else
						exp.Dispose();
				}
				catch(Exception ex)
				{
					ShowError(ex.ToString());
				}
				finally
				{
					lblStatus.Text="";
					progressBar1.Value=0;
					setEnabled(true);
				}
		
			}
		}

		#endregion

		#region 备份字段说明
		private void menuItem9_Click(object sender, System.EventArgs e)
		{
			if(TableName=="")
			{
				ShowWarning("请选择要备份的表！");
				return;
			}
			try
			{
				string doc=pubFunc.BackupPath+TableName+".xls";
				if(File.Exists(doc))
				{
					if(ShowQuestion("文档已经存在，是否覆盖？")=="YES")
						File.Delete(doc);
					else
						return;
				}
				setEnabled(false);
				Export exp=new Export();
				exp.ExportBackup(TableName);
				exp.SaveAs(doc);
				
				if(ShowQuestion("备份数据成功,是否查看?")=="YES")
					exp.ShowExcel();
				else
					exp.Dispose();
			}
			catch(Exception ex)
			{
				ShowError(ex.ToString());
			}
			finally
			{
				setEnabled(true);
			}
		}
		#endregion

		#region 备份选择的表字段说明
		private void menuItem10_Click(object sender, System.EventArgs e)
		{
			if(ShowQuestion("确定要备份选择的表字段说明吗？")=="YES")
			{
				try
				{
					ArrayList ary=new ArrayList();
                    TreeNode[] nodes = TList.Nodes.Find(pubFunc.DefaultDB, true);
                    if (nodes.Length <= 0)
                    {
                        ShowWarning("没有选择数据库!");
                        return;
                    }
                    TreeNode node = nodes[0];
                    for (int i = 0; i < node.Nodes.Count; i++)
                    {
                        if (node.Nodes[i].Checked == false) continue;
                        ary.Add(GetTableName(node.Nodes[i].Text));
                    }
					if(ary.Count<=0)
					{
						ShowWarning("没有选择任何表!");
						return;
					}
					string rnd=System.DateTime.Now.ToString("yyyyMMddHHmmss");
					string doc=pubFunc.BackupPath+rnd+".xls";
					if(File.Exists(doc))
					{
						if(ShowQuestion("文档已经存在，是否覆盖？")=="YES")
							File.Delete(doc);
						else
							return;
					}
				
					setEnabled(false);
					Export exp=new Export();	
					int cnt=ary.Count;
					string table;
					setProgress(cnt+1);
					for(int i=0;i<cnt;i++)
					{
						IAsyncResult ar;
						table=ary[i].ToString();
						DgBackup md=new DgBackup(exp.ExportBackup);
						ar=md.BeginInvoke(table,null,null);
						lblStatus.Text="正在备份："+table;
						//异步执行，显示进度条
						while(ar.IsCompleted==false)
						{
							Application.DoEvents();
						}
						md.EndInvoke(ar);
						progressBar1.Value++;
					}
					exp.Init();
					exp.SaveAs(doc);
					if(ShowQuestion("备份数据成功,是否查看文档?")=="YES")
						exp.ShowExcel();
					else
						exp.Dispose();
				}
				catch(Exception ex)
				{
					ShowError(ex.ToString());
				}
				finally
				{
					lblStatus.Text="";
					progressBar1.Value=0;
					setEnabled(true);
				}
		
			}
		}
		#endregion

		#region 全部备份字段说明
		private void menuItem11_Click(object sender, System.EventArgs e)
		{
			if(MessageBox.Show("确定要全部备份字段说明吗？","全部备份",MessageBoxButtons.YesNo,MessageBoxIcon.Question).ToString()=="Yes")
			{
				try
				{
					string doc=pubFunc.BackupPath+"All.xls";
					if(File.Exists(doc))
					{
						if(ShowQuestion("文档已经存在，是否覆盖？")=="YES")
							File.Delete(doc);
						else
							return;
					}
					
					setEnabled(false);
					Export exp=new Export();	
					int cnt=TList.Nodes[0].Nodes.Count;
					string table;
					setProgress(cnt);
					for(int i=0;i<cnt;i++)
					{
						IAsyncResult ar;
						//table=TList.Nodes[0].Nodes[i].Text;
						table=GetTableName(TList.Nodes[0].Nodes[i].Text);
						DgBackup md=new DgBackup(exp.ExportBackup);
						ar=md.BeginInvoke(table,null,null);
						lblStatus.Text="正在备份："+table;
						//异步执行，显示进度条
						while(ar.IsCompleted==false)
						{
							//System.Threading.Thread.Sleep(1);
							//Cursor.Current=Cursors.WaitCursor;
							Application.DoEvents();
						}
						md.EndInvoke(ar);
						progressBar1.Value++;
					}
					exp.SaveAs(doc);
					if(ShowQuestion("备份数据成功,是否查看?")=="YES")
						exp.ShowExcel();
					else
						exp.Dispose();
					
				}
				catch(Exception ex)
				{
					ShowError(ex.ToString());
				}
				finally
				{
					lblStatus.Text="";
					progressBar1.Value=0;
					setEnabled(true);
				}
			}
		}
		#endregion

		#region 导入数据还原字段说明
		private void menuItem13_Click(object sender, System.EventArgs e)
		{
			try
			{
				if(!Directory.Exists(pubFunc.BackupPath))
				{
					ShowError("找不到备份路径!");
					return;
				}
				openFileDialog1.Filter="备份文件|*.xls";
				openFileDialog1.InitialDirectory=pubFunc.BackupPath;
				if(openFileDialog1.ShowDialog().ToString().ToUpper()=="CANCEL") return;
				
				if(openFileDialog1.FileName!="")
				{
					if(ShowQuestion("确定要还原吗？")=="NO") return;
					
					Import imp=new Import(openFileDialog1.FileName);
					DataSet ds=imp.GetImportData();
					if(ds.Tables[0].Rows.Count>0)
					{
						int cnt=ds.Tables[0].Rows.Count-1;
						setEnabled(false);
						setProgress(cnt+1);
						string table="";
						for(int i=0;i<ds.Tables[0].Rows.Count;i++)
						{
							if(ds.Tables[0].Rows[i][0].ToString()=="") break;
							if(table!= ds.Tables[0].Rows[i][0].ToString())
								table=ds.Tables[0].Rows[i][0].ToString();
							IAsyncResult ar;
							DgImport md=new DgImport(imp.ImportBackup);
							ar=md.BeginInvoke(table,ds.Tables[0].Rows[i][1].ToString(),ds.Tables[0].Rows[i][2].ToString(),null,null);
							lblStatus.Text="正在还原："+table;
							//异步执行，显示进度条
							while(ar.IsCompleted==false)
							{
								Application.DoEvents();
							}
							md.EndInvoke(ar);
							progressBar1.Value++;
						}
					}
					
					ShowSuccess("还原成功！");
					//刷新
					if(TableName!="")
					{
						BindTableColumns(TableName);
					}
					
					
				}
			}
			catch(Exception ex)
			{
				ShowError(ex.ToString());
			}
			finally
			{
				lblStatus.Text="";
				progressBar1.Value=0;
				setEnabled(true);
			}
			
		}
		#endregion


		//异步委托
		private delegate bool DgExport(string _Table,string _ShowName);
		private delegate bool DgBackup(string _Table);
		private delegate bool DgImport(string _Table,string _Column,string _Value);

		private void setProgress(int max)
		{
			progressBar1.Maximum=max;
			progressBar1.Minimum=0;
			progressBar1.Step=1;
			progressBar1.Value=0;
		}

		//等待中
		private void setEnabled(bool Status)
		{
			DList.Enabled=Status;
			foreach(MenuItem menu in mainMenu1.MenuItems)
			{
				menu.Enabled=Status;
			}
			IsWait=!Status;
			
			if(IsWait) 
				Cursor.Current=Cursors.WaitCursor;
			else
				Cursor.Current=Cursors.Default;
		}

		private void splitter1_SplitterMoved(object sender, System.Windows.Forms.SplitterEventArgs e)
		{
			panel1.Width=splitter1.Left;
		}

		private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if(IsWait)
			{
				ShowError("正在处理数据,不能退出!");
				e.Cancel=true;
			}
		}


		private void TList_AfterCheck(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			if(IsWait) return;
			if(e.Node.Nodes.Count>0)
			{
				for(int i=0;i<e.Node.Nodes.Count;i++)
				{
					e.Node.Nodes[i].Checked=e.Node.Checked;
				}
			}
            if (e.Node.Parent == null) return;
            if (e.Node.Level != 1)
            {
                if (e.Node.Checked == true)
                {
                    ShowTableName = e.Node.Text;
                    ShowTableNames.Add(ShowTableName);
                }
                else {
                    ShowTableNames.Remove(e.Node.Text);
                }
            }
		}

		private void TList_DoubleClick(object sender, System.EventArgs e)
		{
			
		}

		private void TList_BeforeCheck(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
		{
			if(IsWait)	e.Cancel=true;
		}

		private void TList_BeforeSelect(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
		{
			if(IsWait)	e.Cancel=true;
		}


		private void menuItem2_Click(object sender, System.EventArgs e)
		{
			BindDataBaseList();
		}

		private void menuItem16_Click(object sender, System.EventArgs e)
		{
			if(IsWait)
			{
				ShowError("正在处理数据,不能退出!");
			}
			else
			{
				System.Windows.Forms.Application.Exit();
			}
		}
        		
		private string GetTableName(string _str)
		{
			//return System.Text.RegularExpressions.Regex.Match(_str,@"\[([\S]*?)\]",System.Text.RegularExpressions.RegexOptions.IgnoreCase).Groups[1].Value;
			return _str;
		}

        private DataTable dt;
        private DataRow[] rows;
        private string clearedTableName;
        string primaryKey;
        string primaryKeyType;

        private void button1_Click(object sender, EventArgs e)
        {
            if (DList.DataSource == null)
            {
                return;
            }
            pubFunc.CurrentTable = ((DataTable)DList.DataSource);
            pubFunc.CurrentTable.TableName = ShowTableName;
            dt = pubFunc.CurrentTable;
            clearedTableName = dt.TableName.Replace("_", "");
            rows = dt.Select("chk=1");
            DataRow[] primaryKeyRows = dt.Select("主键<>''");
            if (null != primaryKeyRows && primaryKeyRows.Length > 0)
            {
                primaryKey = rows[0]["字段名"].ToString().ToLower() ?? "";
                primaryKeyType = pubFunc.GetFieldType(rows[0]["类型"].ToString());
            }

            BindEntity();
            //新增编辑api
            BindDataRequestAddEdit();
            //删除api
            BindDataRequestDelete();

            BindSQL();
            //列表页面
            BindListPage();

            BindJSON();

            BindDA();

            BindLogic();
            //编辑页面
            BindDetailsPage();
            //显示页面
            BindShowPage();

            tabControl1.SelectedIndex = 1;
        }
        
        /// <summary>
        /// 实体
        /// </summary>
        private void BindEntity()
        {
            txtCode.Clear();
            txtCode.AppendText("using System;" + pubFunc.Str_Enter);
            txtCode.AppendText("using CAS.Entity.BaseDAModels;" + pubFunc.Str_Enter);
            txtCode.AppendText(pubFunc.Str_Enter);
            txtCode.AppendText("namespace CAS.Entity.DBEntity" + pubFunc.Str_Enter);
            txtCode.AppendText("{" + pubFunc.Str_Enter);
            txtCode.AppendText(pubFunc.Str_Tab + "[Serializable]" + pubFunc.Str_Enter);
            txtCode.AppendText(pubFunc.Str_Tab + "[TableAttribute(\"dbo." + dt.TableName + "\")]" + pubFunc.Str_Enter);
            txtCode.AppendText(pubFunc.Str_Tab + "public class " + clearedTableName + " : BaseTO" + pubFunc.Str_Enter);
            txtCode.AppendText(pubFunc.Str_Tab + "{" + pubFunc.Str_Enter);
            for (int i = 0; i < rows.Length; i++)
            {
                bool nullable = rows[i]["允许空"].ToString() != "";
                string fieldname = rows[i]["字段名"].ToString().ToLower();
                string type = pubFunc.GetFieldType(rows[i]["类型"].ToString());
                string value = rows[i]["默认值"].ToString();
                value = value == "" ? "" : " = " + pubFunc.GetFieldValue(type, value);
                type = (value == "" && nullable) ? (type == "string" ? type : type + "?") : type;
                txtCode.AppendText(pubFunc.Str_Tab2 + "private " + type + " _" + fieldname + value + ";" + pubFunc.Str_Enter);
                if (rows[i]["字段说明"].ToString() != "")
                {
                    txtCode.AppendText(pubFunc.Str_Tab2 + "/// <summary>" + pubFunc.Str_Enter);
                    txtCode.AppendText(pubFunc.Str_Tab2 + "/// " + rows[i]["字段说明"].ToString() + pubFunc.Str_Enter);
                    txtCode.AppendText(pubFunc.Str_Tab2 + "/// </summary>" + pubFunc.Str_Enter);
                }
                string sqlFieldAtt = string.Empty;
                if (rows[i]["主键"].ToString() != "")
                {
                    sqlFieldAtt = "[SQLField(\"" + fieldname + "\", EnumDBFieldUsage.PrimaryKey";
                    if (rows[i]["标识"].ToString() != "")
                    {
                        sqlFieldAtt += ", true"; 
                    }
                    sqlFieldAtt += ")]";
                    txtCode.AppendText(pubFunc.Str_Tab2 + sqlFieldAtt + pubFunc.Str_Enter);
                }
                txtCode.AppendText(pubFunc.Str_Tab2 + "public " + type + " " + fieldname + pubFunc.Str_Enter);
                txtCode.AppendText(pubFunc.Str_Tab2 + "{" + pubFunc.Str_Enter);
                txtCode.AppendText(pubFunc.Str_Tab3 + "get{ return _" + fieldname + ";}" + pubFunc.Str_Enter);
                txtCode.AppendText(pubFunc.Str_Tab3 + "set{ _" + fieldname + "=value;}" + pubFunc.Str_Enter);
                txtCode.AppendText(pubFunc.Str_Tab2 + "}" + pubFunc.Str_Enter);
            }
            txtCode.AppendText(pubFunc.Str_Tab + "}" + pubFunc.Str_Enter);
            txtCode.AppendText("}");

            string path = applicationPath + "Entity";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = path + "\\" + dt.TableName.Replace("_","") + ".cs";
            File.WriteAllText(filepath, txtCode.Text);
        }

        private string GetData(string type, string fieldname, string index)
        {
            string rtn = "";
            if (type == "string")
                rtn = "dt.Rows[" + index.ToString() + "][\"" + fieldname + "\"].ToString()";
            else if (type == "int")
                rtn = "Convert.ToInt32(dt.Rows[" + index.ToString() + "][\"" + fieldname + "\"].ToString())";
            else if (type == "long")
                rtn = "Convert.ToInt64(dt.Rows[" + index.ToString() + "][\"" + fieldname + "\"].ToString())";
            else if (type == "double")
                rtn = "Convert.ToDouble(dt.Rows[" + index.ToString() + "][\"" + fieldname + "\"].ToString())";
            else if (type == "decimal")
                rtn = "Convert.ToDecimal(dt.Rows[" + index.ToString() + "][\"" + fieldname + "\"].ToString())";
            else if (type == "bool")
                rtn = "Convert.ToBoolean(dt.Rows[" + index.ToString() + "][\"" + fieldname + "\"].ToString())";
            else if (type == "DateTime")
                rtn = "Convert.ToDateTime(dt.Rows[" + index.ToString() + "][\"" + fieldname + "\"].ToString())";
            return rtn;
        }

        /// <summary>
        /// 一般处理程序
        /// </summary>
        private void BindDataRequestAddEdit()
        {
            txtDataRequest.Clear();
            txtDataRequest.AppendText("/// <summary>" + pubFunc.Str_Enter);
            txtDataRequest.AppendText("/// 新增编辑" + pubFunc.Str_Enter);
            txtDataRequest.AppendText("/// <summary>" + pubFunc.Str_Enter);
            txtDataRequest.AppendText("public class Edit" + clearedTableName + ":ApiBase" + pubFunc.Str_Enter);
            txtDataRequest.AppendText("{" + pubFunc.Str_Enter + pubFunc.Str_Tab);
            txtDataRequest.AppendText("public override void ProcessRequest(HttpContext context)" + pubFunc.Str_Enter);
            txtDataRequest.AppendText(pubFunc.Str_Tab + "{" + pubFunc.Str_Enter);
            txtDataRequest.AppendText(pubFunc.Str_Tab2 + "if (!CheckLogin()) return;"+pubFunc.Str_Enter);
            txtDataRequest.AppendText(pubFunc.Str_Tab2 + "if (!CheckMustRequest(new string[] { })) return;" + pubFunc.Str_Enter);
            txtDataRequest.AppendText(pubFunc.Str_Tab2 + "string result=\"\";" + pubFunc.Str_Enter);
            txtDataRequest.AppendText(pubFunc.Str_Tab2 + "string type=GetRequest(\"type\");" + pubFunc.Str_Enter);
            txtDataRequest.AppendText(pubFunc.Str_Tab2 + "try" + pubFunc.Str_Enter);
            txtDataRequest.AppendText(pubFunc.Str_Tab2 + "{" + pubFunc.Str_Enter);
            txtDataRequest.AppendText(pubFunc.Str_Tab3 + "switch(type)" + pubFunc.Str_Enter);
            txtDataRequest.AppendText(pubFunc.Str_Tab3 + "{" + pubFunc.Str_Enter);
            //新增
            txtDataRequest.AppendText(pubFunc.Str_Tab4 + "case \"add\":" + pubFunc.Str_Enter);
            txtDataRequest.AppendText(pubFunc.Str_Tab5 + clearedTableName + " model = InitModel<" + clearedTableName + ">(context.Request);" + pubFunc.Str_Enter);
            txtDataRequest.AppendText(pubFunc.Str_Tab5 + primaryKeyType + " newId = " + clearedTableName + "BL.Add(model);" + pubFunc.Str_Enter);
            txtDataRequest.AppendText(pubFunc.Str_Tab5 + "if(newId > 0)" + pubFunc.Str_Enter);
            txtDataRequest.AppendText(pubFunc.Str_Tab6 + "result=GetJson(newId,1,\"新增成功\",null);" + pubFunc.Str_Enter);
            txtDataRequest.AppendText(pubFunc.Str_Tab5 + "else" + pubFunc.Str_Enter);
            txtDataRequest.AppendText(pubFunc.Str_Tab6 + "result=GetJson(0,\"新增失败\");" + pubFunc.Str_Enter);
            txtDataRequest.AppendText(pubFunc.Str_Tab5 + "break;" + pubFunc.Str_Enter);
            //修改
            txtDataRequest.AppendText(pubFunc.Str_Tab4 + "case \"edit\":" + pubFunc.Str_Enter);
            txtDataRequest.AppendText(pubFunc.Str_Tab5 + primaryKeyType + " " + primaryKey.ToLower() + " = StringHelper.TryGet" + primaryKeyType.Substring(0, 1).ToUpper() + primaryKeyType.Substring(1) +"(GetRequest(\""+primaryKey.ToLower()+"\"));" + pubFunc.Str_Enter);
            txtDataRequest.AppendText(pubFunc.Str_Tab5 + clearedTableName + " model1 = " + clearedTableName + "BL.Get" + clearedTableName + "ByPK(" + primaryKey.ToLower() + ");" + pubFunc.Str_Enter);
            txtDataRequest.AppendText(pubFunc.Str_Tab5 + "model1 = InitModel<" + clearedTableName + ">(context.Request,model1);" + pubFunc.Str_Enter);
            txtDataRequest.AppendText(pubFunc.Str_Tab5 + "int updateFlag = " + clearedTableName + "BL.Update(model1);" + pubFunc.Str_Enter);
            txtDataRequest.AppendText(pubFunc.Str_Tab5 + "if(updateFlag > 0)" + pubFunc.Str_Enter);
            txtDataRequest.AppendText(pubFunc.Str_Tab6 + "result=GetJson(1,\"修改成功\");" + pubFunc.Str_Enter);
            txtDataRequest.AppendText(pubFunc.Str_Tab5 + "else" + pubFunc.Str_Enter);
            txtDataRequest.AppendText(pubFunc.Str_Tab6 + "result=GetJson(0,\"修改失败\");" + pubFunc.Str_Enter);
            txtDataRequest.AppendText(pubFunc.Str_Tab5 + "break;" + pubFunc.Str_Enter);

            txtDataRequest.AppendText(pubFunc.Str_Tab3 + "}" + pubFunc.Str_Enter);
            txtDataRequest.AppendText(pubFunc.Str_Tab2 + "}" + pubFunc.Str_Enter);
            txtDataRequest.AppendText(pubFunc.Str_Tab2 + "catch (Exception ex)" + pubFunc.Str_Enter);
            txtDataRequest.AppendText(pubFunc.Str_Tab2 + "{" + pubFunc.Str_Enter);
            txtDataRequest.AppendText(pubFunc.Str_Tab3 + "LogError(ex);" + pubFunc.Str_Enter);
            txtDataRequest.AppendText(pubFunc.Str_Tab3 + "result = GetJson(ex);" + pubFunc.Str_Enter);
            txtDataRequest.AppendText(pubFunc.Str_Tab2 + "}" + pubFunc.Str_Enter);
            txtDataRequest.AppendText(pubFunc.Str_Tab2 + "context.Response.Write(result);" + pubFunc.Str_Enter);
            txtDataRequest.AppendText(pubFunc.Str_Tab2 + "context.Response.End();" + pubFunc.Str_Enter);
            txtDataRequest.AppendText(pubFunc.Str_Tab + "}" + pubFunc.Str_Enter);
            txtDataRequest.AppendText("}");
        }

        /// <summary>
        /// 一般处理程序
        /// </summary>
        private void BindDataRequestDelete()
        {
            txtDataRequestDelete.Clear();
            txtDataRequestDelete.AppendText("/// <summary>" + pubFunc.Str_Enter);
            txtDataRequestDelete.AppendText("/// 批量删除" + pubFunc.Str_Enter);
            txtDataRequestDelete.AppendText("/// <summary>" + pubFunc.Str_Enter);
            txtDataRequestDelete.AppendText("public class Delete" + clearedTableName + ":ApiBase" + pubFunc.Str_Enter);
            txtDataRequestDelete.AppendText("{" + pubFunc.Str_Enter + pubFunc.Str_Tab);
            txtDataRequestDelete.AppendText("public override void ProcessRequest(HttpContext context)" + pubFunc.Str_Enter);
            txtDataRequestDelete.AppendText(pubFunc.Str_Tab + "{" + pubFunc.Str_Enter);
            txtDataRequestDelete.AppendText(pubFunc.Str_Tab2 + "if (!CheckLogin()) return;" + pubFunc.Str_Enter);
            txtDataRequestDelete.AppendText(pubFunc.Str_Tab2 + "if (!CheckMustRequest(new string[] { \""+primaryKey.ToLower()+"\" })) return;" + pubFunc.Str_Enter);
            txtDataRequestDelete.AppendText(pubFunc.Str_Tab2 + "string result=\"\";" + pubFunc.Str_Enter);
            txtDataRequestDelete.AppendText(pubFunc.Str_Tab2 + primaryKeyType + "[] idarray = GetRequest(\"" + primaryKey.ToLower() + "\").Split(new string[] { \",\" }, StringSplitOptions.RemoveEmptyEntries).Select(StringHelper.TryGet" + primaryKeyType.Substring(0, 1).ToUpper() + primaryKeyType.Substring(1) + ").ToArray();" + pubFunc.Str_Enter);            
            txtDataRequestDelete.AppendText(pubFunc.Str_Tab2 + "try" + pubFunc.Str_Enter);
            txtDataRequestDelete.AppendText(pubFunc.Str_Tab2 + "{" + pubFunc.Str_Enter);
            txtDataRequestDelete.AppendText(pubFunc.Str_Tab3 + "if (idarray != null && idarray.Length > 0) " + pubFunc.Str_Enter);
            txtDataRequestDelete.AppendText(pubFunc.Str_Tab3 + "{" + pubFunc.Str_Enter);
            txtDataRequestDelete.AppendText(pubFunc.Str_Tab4 + clearedTableName + " model = new " + clearedTableName + "();" + pubFunc.Str_Enter);
            txtDataRequestDelete.AppendText(pubFunc.Str_Tab4 + "model.isvalid = false;" + pubFunc.Str_Enter);
            txtDataRequestDelete.AppendText(pubFunc.Str_Tab4 + "model.SetAvailableFields(new string[] { \"isvalid\" });" + pubFunc.Str_Enter);
            txtDataRequestDelete.AppendText(pubFunc.Str_Tab4 + "int flag = " + clearedTableName + "BL.UpdateMul(model, idarray);" + pubFunc.Str_Enter);
            txtDataRequestDelete.AppendText(pubFunc.Str_Tab4 + "if(flag > 0)" + pubFunc.Str_Enter);
            txtDataRequestDelete.AppendText(pubFunc.Str_Tab5 + "result=GetJson(1,\"删除成功\");" + pubFunc.Str_Enter);
            txtDataRequestDelete.AppendText(pubFunc.Str_Tab4 + "else" + pubFunc.Str_Enter);
            txtDataRequestDelete.AppendText(pubFunc.Str_Tab5 + "result=GetJson(0,\"删除失败\");" + pubFunc.Str_Enter);
            
            txtDataRequestDelete.AppendText(pubFunc.Str_Tab3 + "}" + pubFunc.Str_Enter);
            txtDataRequestDelete.AppendText(pubFunc.Str_Tab2 + "}" + pubFunc.Str_Enter);
            txtDataRequestDelete.AppendText(pubFunc.Str_Tab2 + "catch (Exception ex)" + pubFunc.Str_Enter);
            txtDataRequestDelete.AppendText(pubFunc.Str_Tab2 + "{" + pubFunc.Str_Enter);
            txtDataRequestDelete.AppendText(pubFunc.Str_Tab3 + "LogError(ex);" + pubFunc.Str_Enter);
            txtDataRequestDelete.AppendText(pubFunc.Str_Tab3 + "result = GetJson(ex);" + pubFunc.Str_Enter);
            txtDataRequestDelete.AppendText(pubFunc.Str_Tab2 + "}" + pubFunc.Str_Enter);
            txtDataRequestDelete.AppendText(pubFunc.Str_Tab2 + "context.Response.Write(result);" + pubFunc.Str_Enter);
            txtDataRequestDelete.AppendText(pubFunc.Str_Tab2 + "context.Response.End();" + pubFunc.Str_Enter);
            txtDataRequestDelete.AppendText(pubFunc.Str_Tab + "}" + pubFunc.Str_Enter);
            txtDataRequestDelete.AppendText("}");
        }

        private void BindDataRequestAddEdit2()
        {
            txtDataRequest.Clear();
            string entityFields = string.Empty;
            string requestFields = string.Empty;
            string assignment = string.Empty;
            string checking = string.Empty;
            for (int i = 0; i < rows.Length; i++)
            {
                string type = pubFunc.GetFieldType(rows[i]["类型"].ToString());
                string fieldname = rows[i]["字段名"].ToString().ToLower();
                string value = rows[i]["默认值"].ToString();
                bool enableEmpty = !string.IsNullOrEmpty(rows[i]["允许空"].ToString());
                entityFields += string.Format("{0} {1};" + pubFunc.Str_Enter, new object[] { type, fieldname });
                string convertStatement = string.Empty;
                string checkingSatatement = string.Empty;
                switch (type)
                {
                    case "bool":
                        value = string.IsNullOrEmpty(value) ? "0" : value;
                        convertStatement = "model.{0} = StringHelper.TryGetBool(GetRequest(\"{0}\"));";
                        break;
                    case "int"://int型在先在DB中大多数情况下作为PK或code，因此判定<=0为无效
                        value = string.IsNullOrEmpty(value) ? "0" : value;
                        convertStatement = "model.{0} = StringHelper.TryGetInt(GetRequest(\"{0}\"));";
                        if (!enableEmpty)
                        {
                            checkingSatatement = "if (model.{0} <= 0)" + pubFunc.Str_Enter
                                + "{{" + pubFunc.Str_Enter                                
                                + pubFunc.Str_Tab + "errList.Add(\"{0}\", \"{0}无效\");" + pubFunc.Str_Enter
                                + "}}" + pubFunc.Str_Enter;
                        }
                        break;
                    case "long":
                        value = string.IsNullOrEmpty(value) ? "0" : value;
                        convertStatement = "model.{0} = StringHelper.TryGetLong(GetRequest(\"{0}\"));";
                        break;
                    case "byte":
                        value = string.IsNullOrEmpty(value) ? "0" : value;
                        convertStatement = "model.{0} = StringHelper.TryGetByte(GetRequest(\"{0}\"));";
                        break;
                    case "DateTime":
                        value = string.IsNullOrEmpty(value) ? default(DateTime).ToString() : value;
                        convertStatement = "model.{0} = StringHelper.TryGetDateTime(GetRequest(\"{0}\"));";
                        break;
                    case "double":
                        value = string.IsNullOrEmpty(value) ? default(double).ToString() : value;
                        convertStatement = "model.{0} = StringHelper.TryGetDouble(GetRequest(\"{0}\"));";
                        break;
                    case "decimal":
                        value = string.IsNullOrEmpty(value) ? default(decimal).ToString() : value;
                        convertStatement = "model.{0} = StringHelper.TryGetDecimal(GetRequest(\"{0}\"));";
                        break;
                    case "string":
                        value = string.IsNullOrEmpty(value) ? "" : value;
                        requestFields += string.Format("string r_{0} = string.Empty;" + pubFunc.Str_Enter, new object[] { fieldname });
                        convertStatement = "r_{0} = GetRequest(\"{0}\");" + pubFunc.Str_Enter                        
                            + pubFunc.Str_Tab + "model.{0} = r_{0};";
                        if (!enableEmpty)
                        {
                            checkingSatatement = "if (string.IsNullOrEmpty(model.{0}))" + pubFunc.Str_Enter
                                + "{{" + pubFunc.Str_Enter
                                + pubFunc.Str_Tab + "errList.Add(\"{0}\", \"{0}不能为空\");" + pubFunc.Str_Enter
                                + "}}" + pubFunc.Str_Enter;
                        }
                        break;
                    default:
                        value = string.IsNullOrEmpty(value) ? "" : value;
                        convertStatement = "model.{0} = GetRequest(\"{0}\");";
                        break;
                }

                assignment += string.Format("if (requestKeys.Contains<string>(\"{0}\"))"
                    + pubFunc.Str_Enter + "{{"
                    + pubFunc.Str_Enter + pubFunc.Str_Tab + convertStatement
                    + pubFunc.Str_Enter + "}}" + pubFunc.Str_Enter, new object[] { fieldname });
                checking += string.Format(checkingSatatement, new object[] { fieldname });
            }
            txtDataRequest.AppendText("#region 处理数据" + pubFunc.Str_Enter);
            txtDataRequest.AppendText(requestFields + pubFunc.Str_Enter);
            txtDataRequest.AppendText("HttpRequest request = context.Request;" + pubFunc.Str_Enter);
            txtDataRequest.AppendText("string[] requestKeys = request.Params.AllKeys;" + pubFunc.Str_Enter);
            //txtDataRequest.AppendText("for (int i = 0; i < requestKeys.Length; i++)" + pubFunc.Str_Enter);
            //txtDataRequest.AppendText("{" + pubFunc.Str_Enter);
            //txtDataRequest.AppendText(pubFunc.Str_Tab + "requestKeys[i] = requestKeys[i].ToLower();" + pubFunc.Str_Enter);
            //txtDataRequest.AppendText("}" + pubFunc.Str_Enter);
            txtDataRequest.AppendText(pubFunc.Str_Enter);
            txtDataRequest.AppendText(clearedTableName + " model = new " + clearedTableName + "();" + pubFunc.Str_Enter);
            txtDataRequest.AppendText(assignment + pubFunc.Str_Enter);
            txtDataRequest.AppendText("Dictionary<string, string> errList = new Dictionary<string, string>();" + pubFunc.Str_Enter);            
            txtDataRequest.AppendText(checking + pubFunc.Str_Enter);
            txtDataRequest.AppendText("#endregion 处理数据");
        }

       

        /// <summary>
        /// SQL语句 
        /// </summary>
        private void BindSQL()
        {
            txtSQL.Clear();
            
            string sqlselect = "";
            string sql = "";
            string sql1 = "";
            string sql2 = "";
            string key = "";
            for (int i = 0; i < rows.Length; i++)
            {
                string fieldname = rows[i]["字段名"].ToString().ToLower();
                sqlselect += fieldname + ",";
                //主键除外
                if (rows[i]["主键"].ToString() != "")
                {
                    key = fieldname;
                    continue;
                }
                string type = pubFunc.GetFieldType(rows[i]["类型"].ToString());
                
                sql += fieldname + ",";
                sql1 += "@" + fieldname + ",";
                sql2 += fieldname + " = @" + fieldname + ",";
            }
            sqlselect = sqlselect.Substring(0, sqlselect.Length - 1) + " ";
            sql = sql.Substring(0, sql.Length - 1);
            sql1 = sql1.Substring(0, sql1.Length - 1);
            sql2 = sql2.Substring(0, sql2.Length - 1);
            txtSQL.AppendText("--查询" + pubFunc.Str_Enter);
            txtSQL.AppendText("select " + sqlselect + pubFunc.Str_Enter + "from @table_" + clearedTableName.ToLower() + " with(nolock) " + pubFunc.Str_Enter);
            txtSQL.AppendText(pubFunc.Str_Enter);

            txtSQL.AppendText("--新增" + pubFunc.Str_Enter);
            txtSQL.AppendText("insert into @table_" + clearedTableName.ToLower() + " (" + sql + ") " + pubFunc.Str_Enter);
            txtSQL.AppendText("values(" + sql1 + ")" + pubFunc.Str_Enter);
            txtSQL.AppendText(pubFunc.Str_Enter);

            txtSQL.AppendText("--修改" + pubFunc.Str_Enter);
            txtSQL.AppendText("update @table_" + clearedTableName.ToLower() + " set " + sql2 + pubFunc.Str_Enter);
            txtSQL.AppendText("where " + key + " = @" + key + pubFunc.Str_Enter);
            txtSQL.AppendText(pubFunc.Str_Enter);

            txtSQL.AppendText("--删除" + pubFunc.Str_Enter);
            txtSQL.AppendText("delete from @table_" + clearedTableName.ToLower() + pubFunc.Str_Enter);
            txtSQL.AppendText("where " + key + " = @" + key + pubFunc.Str_Enter);
            txtSQL.AppendText(pubFunc.Str_Enter);
        }

        /// <summary>
        /// 列表页面
        /// </summary>
        private void BindListPage()
        {
            txtWeb.Clear();
            txtWeb.AppendText("<asp:Content ID=\"Content1\" ContentPlaceHolderID=\"ContentMeta\" runat=\"server\"></asp:Content>" + pubFunc.Str_Enter);
            txtWeb.AppendText("<asp:Content ID=\"Content2\" ContentPlaceHolderID=\"ContentScripts\" runat=\"server\">" + pubFunc.Str_Enter);
            //脚本
            txtWeb.AppendText("<script language=\"javascript\" type=\"text/javascript\">"+ pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab + "$(function () {" + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab2 + "//绑定列表" + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab2 + "$(\"#grid\").casgrid({" + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab3 + "urlType: \"api\"," + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab3 + "url: \""+clearedTableName.ToLower()+"list\"," + pubFunc.Str_Enter);
            //列
            txtWeb.AppendText(pubFunc.Str_Tab3 + "colModel: [" + pubFunc.Str_Enter);
            for (int i = 0; i < rows.Length; i++)
            {
                string fieldname = rows[i]["字段名"].ToString().ToLower();
                string fieldtype = pubFunc.GetFieldType(rows[i]["类型"].ToString());
                string format = "";
                string align = "left";
                if (fieldtype == "DateTime") {
                    align = "center";
                    format = ",format: { type: \"date\", text: \"yyyy-MM-dd hh:mm:ss\"}";
                }
                else if (fieldtype == "double" ||
                   fieldtype == "long" ||
                   fieldtype == "int" ||
                   fieldtype == "decimal" ) {
                       align = "right";                       
                }
                else if (fieldtype == "bool")
                {
                    align = "center";
                    format = ",process: function (t, p, r) { return t?\"是\":\"否\"; }";
                }
                txtWeb.AppendText(pubFunc.Str_Tab4+"{ display: \""+rows[i]["字段说明"].ToString()+"\", name: \""+fieldname+"\", width: \"100\", sortable: true, align: \""+align+"\" "+format+" }" + (i==rows.Length-1?"":",") + pubFunc.Str_Enter);
            }
            txtWeb.AppendText(pubFunc.Str_Tab3 + "]," + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab3 + "key: \""+primaryKey.ToLower()+"\"," + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab3 + "extParam: { type: \"list\"}," + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab3 + "sortname: \""+primaryKey.ToLower()+"\"," + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab3 + "sortorder: \"desc\"," + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab3 + "usepager: true," + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab3 + "useRp: true," + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab3 + "rp: 20," + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab3 + "showcheckbox: true," + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab3 + "rowdblclick: details" + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab2 + "});" + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab2 + "//查询" + pubFunc.Str_Enter);            
            txtWeb.AppendText(pubFunc.Str_Tab2 + "$(\"#btnSearch\").click(search);" + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab2 + "//新增" + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab2 + "$(\"#btnAdd\").click(function(){details({" + primaryKey.ToLower() + ":0});});" + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab2 + "//修改" + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab2 + "$(\"#btnEdit\").click(editdetails);" + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab2 + "//删除" + pubFunc.Str_Enter);            
            txtWeb.AppendText(pubFunc.Str_Tab2 + "$(\"#btnDelete\").click(function () {" + pubFunc.Str_Enter);            
            txtWeb.AppendText(pubFunc.Str_Tab3 + "$(\"#grid\").flexDeleteRow({ api: \"delete" + clearedTableName.ToLower() + "\",data:{type:\"delete\"} , key:\""+primaryKey.ToLower()+"\" });"+pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab2 + "});" + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab + "});" + pubFunc.Str_Enter);
            //查询
            txtWeb.AppendText(pubFunc.Str_Tab + "//查询" + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab + "function search(){" + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab2 + "$(\"#grid\").flexSearch({" + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab3 + "type: \"list\"," + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab3 + "key: $(\"#txtKey\").val()" + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab2 + "});" + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab + "}" + pubFunc.Str_Enter);
            //新增修改
            txtWeb.AppendText(pubFunc.Str_Tab + "//新增修改"+pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab + "function details(o){" + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab2 + "var title = o."+primaryKey.ToLower()+"<=0 ? \"新增\" : \"修改\";" + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab2 + "var url = \""+clearedTableName+"Modify.aspx?type=\" + (o."+primaryKey.ToLower()+"<=0 ? \"add\" : \"edit\") + \"&"+primaryKey.ToLower()+"=\" + o."+primaryKey.ToLower()+";" + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab2 + "var min=false;" + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab2 + "var max=false;" + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab2 + "var initmax=false;" + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab2 + "var width=600;" + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab2 + "var height=400;" + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab2 + "CAS.Dialog({ title: title, initmax: initmax, max: max, min: min, content: \"url:\" + CAS.RootUrl + url, width: width, height: height });" + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab + "}" + pubFunc.Str_Enter);
            //修改按钮
            txtWeb.AppendText(pubFunc.Str_Tab + "//修改" + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab + "function editdetails(){" + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab2 + "var sels = $(\"#grid\").flexSelectRow(this);"+pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab2 + "if (!sels) return;" + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab2 + "details(sels[0]);" + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab + "}" + pubFunc.Str_Enter);
            txtWeb.AppendText("</script>" + pubFunc.Str_Enter);
            txtWeb.AppendText("</asp:Content>" + pubFunc.Str_Enter);
            txtWeb.AppendText("<asp:Content ID=\"Content3\" ContentPlaceHolderID=\"ContentBody\" runat=\"server\">" + pubFunc.Str_Enter);
            //内容
            txtWeb.AppendText("<div class=\"layout\">"+pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab + "<div class=\"layout_top\">" + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab2 + "<div class=\"layout_toolbar\">" + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab3 + "<input class=\"btn\" type=\"button\" id=\"btnAdd\" value=\"新增\"/>" + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab3 + "<input class=\"btn\" type=\"button\" id=\"btnEdit\" value=\"修改\"/>" + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab3 + "<input class=\"btn\" type=\"button\" id=\"btnDelete\" value=\"删除\"/>" + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab2 + "</div>" + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab2 + "<div class=\"layout_searchbar\">" + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab3 + "<input type=\"text\" id=\"txtKey\" class=\"w100\" /> <input type=\"button\" class=\"btn\" id=\"btnSearch\" value=\"查询\" />" + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab2 + "</div>" + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab + "</div>" + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab + "<div class=\"layout_center\">" + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab2 + "<table id=\"grid\"></table>" + pubFunc.Str_Enter);
            txtWeb.AppendText(pubFunc.Str_Tab + "</div>" + pubFunc.Str_Enter);
            txtWeb.AppendText("</div>" + pubFunc.Str_Enter);
            txtWeb.AppendText("</asp:Content>");
            //txtWeb.AppendText("<!--表单开始-->" + pubFunc.Str_Enter);
            //txtWeb.AppendText("<table id=\"tbl_" + clearedTableName.ToLower() + "\" cellspacing=\"0\" cellpadding=\"0\">" + pubFunc.Str_Enter);
            //int index = 0;
            //for (int i = 0; i < rows.Length; i++)
            //{
            //    string type = pubFunc.GetFieldType(rows[i]["类型"].ToString());
            //    string fieldname = rows[i]["字段名"].ToString().ToLower();
            //    string desc = rows[i]["字段说明"].ToString().ToLower();
            //    string len = rows[i]["长度"].ToString();
            //    string css = "";
            //    if (type != "string")
            //    {
            //        len = "0";                    
            //    }
            //    if (type == "int" || type == "long" || type == "double" || type == "decimal")
            //    {
            //        css = "number";
            //    }
            //    else if (type == "DateTime")
            //        css = "date";
            //    if(index%3==0)
            //        txtWeb.AppendText("<tr>");                
            //    txtWeb.AppendText("<td class=\"tr\">" + (desc.Length>0?desc:fieldname) + "</td><td>");
            //    txtWeb.AppendText("<input type=\"text\" id=\"field_" + fieldname + "\"" + (len == "0" ? "" : " maxlength=\"" + len + "\"") + (css == "" ? "" : " class=\"" + css + "\"") + "/></td>");
            //    if(index %3==2)
            //        txtWeb.AppendText("</tr>"+pubFunc.Str_Enter);
            //    index++;
            //}
            //if (index % 3 == 2)
            //    txtWeb.AppendText("</tr>" + pubFunc.Str_Enter);
            //txtWeb.AppendText("</table>"+pubFunc.Str_Enter);
            //txtWeb.AppendText("<!--表单结束-->"+pubFunc.Str_Enter);

            //txtWeb.AppendText("<!--数据处理开始-->" + pubFunc.Str_Enter);
            //txtWeb.AppendText("<script type=\"text/javascript\">" + pubFunc.Str_Enter);
            //txtWeb.AppendText("$(function(){" + pubFunc.Str_Enter);
            //txtWeb.AppendText(pubFunc.Str_Tab + "var data=<%=JsonData%>;" + pubFunc.Str_Enter);
            //for (int i = 0; i < rows.Length; i++)
            //{
            //    string fieldname = rows[i]["字段名"].ToString().ToLower();
            //    string val = "data." + fieldname;
            //    string type = pubFunc.GetFieldType(rows[i]["类型"].ToString());
            //    if (type == "DateTime")
            //    {
            //        val = "CAS.JsonToDate(" + val + ",\"yyyy-MM-dd\")";
            //    }
            //    else if (type == "int" || type == "long" || type == "double" || type == "decimal")
            //    {
            //        val = "CAS.Commafy(" + val + ")";
            //    }
            //    txtWeb.AppendText(pubFunc.Str_Tab + "$(\"#field_" + fieldname + "\").val("+val+");" + pubFunc.Str_Enter);
            //}            
            //txtWeb.AppendText("});" + pubFunc.Str_Enter);

            //txtWeb.AppendText("function SubmitData(){" + pubFunc.Str_Enter);
            //txtWeb.AppendText(pubFunc.Str_Tab + "var data={" + pubFunc.Str_Enter);
            //for (int i = 0; i < rows.Length; i++)
            //{
            //    string fieldname = rows[i]["字段名"].ToString().ToLower();
            //    txtWeb.AppendText(pubFunc.Str_Tab2 + fieldname + ":$(\"#field_" + fieldname + "\").val()" + (i==rows.Length-1?"":",") + pubFunc.Str_Enter);
            //}
            //txtWeb.AppendText(pubFunc.Str_Tab + "};" + pubFunc.Str_Enter);
            //txtWeb.AppendText(pubFunc.Str_Tab + "CAS.API({ type: \"post\", api: \"\", data: data , callback: function (data) { } });" + pubFunc.Str_Enter);
            //txtWeb.AppendText("}" + pubFunc.Str_Enter);
            //txtWeb.AppendText("</script>" + pubFunc.Str_Enter);
            //txtWeb.AppendText("<!--数据处理结束-->" + pubFunc.Str_Enter);
        }


        /// <summary>
        /// 界面字段
        /// </summary>
        private void BindDetailsPage()
        {
            txtFormField.Clear();
            txtFormField.AppendText("<asp:Content ID=\"Content1\" ContentPlaceHolderID=\"ContentMeta\" runat=\"server\"></asp:Content>" + pubFunc.Str_Enter);
            txtFormField.AppendText("<asp:Content ID=\"Content2\" ContentPlaceHolderID=\"ContentScripts\" runat=\"server\">" + pubFunc.Str_Enter);
            //脚本
            txtFormField.AppendText("<script language=\"javascript\" type=\"text/javascript\">" + pubFunc.Str_Enter);
            txtFormField.AppendText(pubFunc.Str_Tab + "$(function () {" + pubFunc.Str_Enter);
            txtFormField.AppendText(pubFunc.Str_Tab2 + "$(\"table.table\").castable();" + pubFunc.Str_Enter);
            txtFormField.AppendText(pubFunc.Str_Tab2 + "//表单提交" + pubFunc.Str_Enter);
            txtFormField.AppendText(pubFunc.Str_Tab2 + "$(\"#table\").casform({" + pubFunc.Str_Enter);
            txtFormField.AppendText(pubFunc.Str_Tab3 + "ctl: $(\"#btnSave\")," + pubFunc.Str_Enter);
            txtFormField.AppendText(pubFunc.Str_Tab3 + "api: \"edit" + clearedTableName.ToLower() + "\"," + pubFunc.Str_Enter);
            txtFormField.AppendText(pubFunc.Str_Tab3 + "getdata: getdata," + pubFunc.Str_Enter);
            txtFormField.AppendText(pubFunc.Str_Tab3 + "check:check," + pubFunc.Str_Enter);
            txtFormField.AppendText(pubFunc.Str_Tab3 + "callback: function(data){" + pubFunc.Str_Enter);
            txtFormField.AppendText(pubFunc.Str_Tab4 + "if(data.returntype==1){" + pubFunc.Str_Enter);
            txtFormField.AppendText(pubFunc.Str_Tab5 + "//成功" + pubFunc.Str_Enter);
            txtFormField.AppendText(pubFunc.Str_Tab4 + "}else{" + pubFunc.Str_Enter);
            txtFormField.AppendText(pubFunc.Str_Tab5 + "CAS.Alert(data.returntext);" + pubFunc.Str_Enter);
            txtFormField.AppendText(pubFunc.Str_Tab4 + "}" + pubFunc.Str_Enter);
            txtFormField.AppendText(pubFunc.Str_Tab3 + "}" + pubFunc.Str_Enter);
            txtFormField.AppendText(pubFunc.Str_Tab2 + "});" + pubFunc.Str_Enter);
            txtFormField.AppendText(pubFunc.Str_Tab + "});" + pubFunc.Str_Enter);
            //取数据
            txtFormField.AppendText(pubFunc.Str_Tab + "//取数据" + pubFunc.Str_Enter);
            txtFormField.AppendText(pubFunc.Str_Tab + "function getdata(){" + pubFunc.Str_Enter);
            txtFormField.AppendText(pubFunc.Str_Tab2 + "//非系统字段的数据" + pubFunc.Str_Enter);
            txtFormField.AppendText(pubFunc.Str_Tab2 + "var data={};" + pubFunc.Str_Enter);
            txtFormField.AppendText(pubFunc.Str_Tab2 + "return data;" + pubFunc.Str_Enter);
            txtFormField.AppendText(pubFunc.Str_Tab + "}" + pubFunc.Str_Enter);
            //检查数据
            txtFormField.AppendText(pubFunc.Str_Tab + "//检查数据" + pubFunc.Str_Enter);
            txtFormField.AppendText(pubFunc.Str_Tab + "function check(){" + pubFunc.Str_Enter);
            txtFormField.AppendText(pubFunc.Str_Tab2 + "//处理" + pubFunc.Str_Enter);
            txtFormField.AppendText(pubFunc.Str_Tab2 + "return true;" + pubFunc.Str_Enter);
            txtFormField.AppendText(pubFunc.Str_Tab + "};" + pubFunc.Str_Enter);
            
            txtFormField.AppendText("</script>" + pubFunc.Str_Enter);
            txtFormField.AppendText("</asp:Content>" + pubFunc.Str_Enter);
            txtFormField.AppendText("<asp:Content ID=\"Content3\" ContentPlaceHolderID=\"ContentBody\" runat=\"server\">" + pubFunc.Str_Enter);
            //内容
            txtFormField.AppendText("<div class=\"layout\">" + pubFunc.Str_Enter);
            txtFormField.AppendText(pubFunc.Str_Tab + "<div class=\"layout_top\">" + pubFunc.Str_Enter);
            txtFormField.AppendText(pubFunc.Str_Tab2 + "<div class=\"layout_toolbar\">" + pubFunc.Str_Enter);
            txtFormField.AppendText(pubFunc.Str_Tab3 + "<input class=\"btn\" type=\"button\" id=\"btnSave\" value=\"保存\"/>" + pubFunc.Str_Enter);
            txtFormField.AppendText(pubFunc.Str_Tab2 + "</div>" + pubFunc.Str_Enter);
            txtFormField.AppendText(pubFunc.Str_Tab + "</div>" + pubFunc.Str_Enter);
            txtFormField.AppendText(pubFunc.Str_Tab + "<div class=\"layout_center\" id=\"table\">" + pubFunc.Str_Enter);
            txtFormField.AppendText(pubFunc.Str_Tab2 + "<table rel=\"\" class=\"table\">" + pubFunc.Str_Enter);
            //字段
            for (int i = 0; i < rows.Length; i++)
            {
                string type = pubFunc.GetFieldType(rows[i]["类型"].ToString());
                string fieldname = rows[i]["字段名"].ToString().ToLower();
                string desc = rows[i]["字段说明"].ToString().ToLower();
                string len = rows[i]["长度"].ToString();
                bool required = (rows[i]["允许空"].ToString() == "");
                string css = "";
                if (type != "string")
                {
                    len = "0";
                }
                if (type == "int" || type == "long" || type == "double" || type == "decimal")
                {
                    css = "number";
                }
                else if (type == "DateTime")
                    css = "date";
                string dot = "";
                if (type == "double" || type == "decimal")
                {
                    dot = " dot=\"2\"";
                    css += " afy";
                }
                if (required) css += " iptrequired";
                if (i % 2 == 0) txtFormField.AppendText(pubFunc.Str_Tab3 + "<tr>"+pubFunc.Str_Enter);
                txtFormField.AppendText(pubFunc.Str_Tab4 + "<td class=\"tr\">" + (desc.Length > 0 ? desc : fieldname) + "</td><td>" + pubFunc.Str_Enter);
                txtFormField.AppendText(pubFunc.Str_Tab5 + "<input type=\"text\" id=\"field_" + fieldname + "\"" + (len == "0" ? "" : " maxlength=\"" + len + "\"") + (css == "" ? "" : " class=\"" + css + "\"") + dot + "/>" + pubFunc.Str_Enter);
                txtFormField.AppendText(pubFunc.Str_Tab4 + "</td>" + pubFunc.Str_Enter);
                if (i % 2 == 1) txtFormField.AppendText(pubFunc.Str_Tab3 + "</tr>"+pubFunc.Str_Enter);
            }            
            txtFormField.AppendText(pubFunc.Str_Tab2 + "</table>" + pubFunc.Str_Enter);
            txtFormField.AppendText(pubFunc.Str_Tab + "</div>" + pubFunc.Str_Enter);
            txtFormField.AppendText("</div>" + pubFunc.Str_Enter);
            txtFormField.AppendText("</asp:Content>");
            //for (int i = 0; i < rows.Length; i++)
            //{
            //    string type = pubFunc.GetFieldType(rows[i]["类型"].ToString());
            //    string fieldname = rows[i]["字段名"].ToString().ToLower();
            //    string desc = rows[i]["字段说明"].ToString().ToLower();
            //    string len = rows[i]["长度"].ToString();
            //    bool required = ( rows[i]["允许空"].ToString()=="" );
            //    string css = "";
            //    if (type != "string")
            //    {
            //        len = "0";
            //    }
            //    if (type == "int" || type == "long" || type == "double" || type == "decimal")
            //    {
            //        css = "number";
            //    }
            //    else if (type == "DateTime")
            //        css = "date";
            //    string dot = "";
            //    if (type == "double" || type == "decimal")
            //    {
            //        dot = " dot=\"2\"";
            //        css += " afy";
            //    }
            //    if (required) css += " iptrequired";

            //    txtFormField.AppendText("<td class=\"tr\">" + (desc.Length > 0 ? desc : fieldname) + "</td><td>"+ pubFunc.Str_Enter);
            //    txtFormField.AppendText("<input type=\"text\" id=\"field_" + fieldname + "\"" + (len == "0" ? "" : " maxlength=\"" + len + "\"") + (css == "" ? "" : " class=\"" + css + "\"") + dot + "/>" + pubFunc.Str_Enter );
            //    txtFormField.AppendText("</td>" + pubFunc.Str_Enter);
                
            //}
            //for (int i = 0; i < rows.Length; i++)
            //{
            //    string fieldname = rows[i]["字段名"].ToString().ToLower();
            //    txtFormField.AppendText("<input type=\"hidden\" id=\"field_" + fieldname + "\"/>" + pubFunc.Str_Enter);
            //}
                        
        }

        /// <summary>
        /// 显示界面
        /// </summary>
        private void BindShowPage()
        {
            txtShowPage.Clear();
            txtShowPage.AppendText("<asp:Content ID=\"Content1\" ContentPlaceHolderID=\"ContentMeta\" runat=\"server\"></asp:Content>" + pubFunc.Str_Enter);
            txtShowPage.AppendText("<asp:Content ID=\"Content2\" ContentPlaceHolderID=\"ContentScripts\" runat=\"server\">" + pubFunc.Str_Enter);
            //脚本
            txtShowPage.AppendText("<script language=\"javascript\" type=\"text/javascript\">" + pubFunc.Str_Enter);
            txtShowPage.AppendText(pubFunc.Str_Tab + "$(function () {" + pubFunc.Str_Enter);
            txtShowPage.AppendText(pubFunc.Str_Tab2 + "$(\"table.table\").castable();" + pubFunc.Str_Enter);
            txtShowPage.AppendText(pubFunc.Str_Tab + "});" + pubFunc.Str_Enter);            
            txtShowPage.AppendText("</script>" + pubFunc.Str_Enter);
            txtShowPage.AppendText("</asp:Content>" + pubFunc.Str_Enter);
            txtShowPage.AppendText("<asp:Content ID=\"Content3\" ContentPlaceHolderID=\"ContentBody\" runat=\"server\">" + pubFunc.Str_Enter);
            //内容
            txtShowPage.AppendText("<div class=\"layout\">" + pubFunc.Str_Enter);
            txtShowPage.AppendText(pubFunc.Str_Tab + "<div class=\"layout_center\">" + pubFunc.Str_Enter);
            txtShowPage.AppendText(pubFunc.Str_Tab2 + "<table rel=\"详情\" class=\"table\">" + pubFunc.Str_Enter);
            //字段
            for (int i = 0; i < rows.Length; i++)
            {
                string fieldname = rows[i]["字段名"].ToString().ToLower();
                string desc = rows[i]["字段说明"].ToString().ToLower();
                
                if (i % 2 == 0) txtShowPage.AppendText(pubFunc.Str_Tab3 + "<tr>" + pubFunc.Str_Enter);
                txtShowPage.AppendText(pubFunc.Str_Tab4 + "<td class=\"tr\">" + (desc.Length > 0 ? desc : fieldname) + "</td><td>" + pubFunc.Str_Enter);
                txtShowPage.AppendText(pubFunc.Str_Tab5 + "<%=model."+fieldname+"%>" + pubFunc.Str_Enter);
                txtShowPage.AppendText(pubFunc.Str_Tab4 + "</td>" + pubFunc.Str_Enter);
                if (i % 2 == 1) txtShowPage.AppendText(pubFunc.Str_Tab3 + "</tr>" + pubFunc.Str_Enter);
            }
            txtShowPage.AppendText(pubFunc.Str_Tab2 + "</table>" + pubFunc.Str_Enter);
            txtShowPage.AppendText(pubFunc.Str_Tab + "</div>" + pubFunc.Str_Enter);
            txtShowPage.AppendText("</div>" + pubFunc.Str_Enter);
            txtShowPage.AppendText("</asp:Content>");
            //for (int i = 0; i < rows.Length; i++)
        }

        /// <summary>
        /// 实体JSON格式
        /// </summary>
        private void BindJSON()
        {
            txtJSON.Clear();
            txtJSON.AppendText("{" + pubFunc.Str_Enter);
            for (int i = 0; i < rows.Length; i++)
            {
                string type = pubFunc.GetFieldType(rows[i]["类型"].ToString());
                string fieldname = rows[i]["字段名"].ToString().ToLower();
                string fieldValue = string.Empty;
                switch (type)
                {
                    case "bool":
                        fieldValue = "true";
                        break;
                    case "int":                        
                        fieldValue = "0";
                        break;
                    case "long":
                        fieldValue = "0.0";
                        break;
                    case "DateTime":
                        fieldValue = "\"" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "\"";
                        break;
                    case "double":
                        fieldValue = "0.0";
                        break;
                    case "decimal":
                        fieldValue = "0.0";
                        break;
                    default:
                        fieldValue = "\"\"";
                        break;
                }
                string fieldJSON = string.Format(pubFunc.Str_Tab + "\"{0}\":{1}," + pubFunc.Str_Enter, new object[] { fieldname, fieldValue });
                txtJSON.AppendText(fieldJSON);
            }
            txtJSON.AppendText("}" + pubFunc.Str_Enter);
        }

        /// <summary>
        /// 绑定数据访问代码
        /// </summary>
        private void BindDA()
        {
            string insertMethodName = "InsertFromEntity";
            if (primaryKeyType == "long")
            {
                insertMethodName = "InsertFromEntityAndReturnLongId";
            }
            string daFormat = string.Empty;
            daFormat += "using System;" + pubFunc.Str_Enter;
            daFormat += "using System.Collections.Generic;" + pubFunc.Str_Enter;
            daFormat += "using System.Linq;" + pubFunc.Str_Enter;
            daFormat += "using System.Text;" + pubFunc.Str_Enter;
            daFormat += "using System.Data;" + pubFunc.Str_Enter;
            daFormat += "using System.Data.SqlClient;" + pubFunc.Str_Enter;
            daFormat += "using CAS.Common;" + pubFunc.Str_Enter;
            daFormat += "using CAS.Entity.DBEntity;" + pubFunc.Str_Enter;
            daFormat += "using CAS.DataAccess.BaseDAModels;" + pubFunc.Str_Enter;
            daFormat += pubFunc.Str_Enter;
            daFormat += "namespace CAS.DataAccess" + pubFunc.Str_Enter;
            daFormat += "{{" + pubFunc.Str_Enter;
            daFormat += pubFunc.Str_Tab + "public class {0}DA : Base " + pubFunc.Str_Enter;
            daFormat += pubFunc.Str_Tab + "{{" + pubFunc.Str_Enter;
            daFormat += pubFunc.Str_Tab2 + "public static {1} Add({0} model)" + pubFunc.Str_Enter;
            daFormat += pubFunc.Str_Tab2 + "{{" + pubFunc.Str_Enter;
            daFormat += pubFunc.Str_Tab3 + "return {2}<{0}>(model);" + pubFunc.Str_Enter;
            daFormat += pubFunc.Str_Tab2 + "}}" + pubFunc.Str_Enter;
            daFormat += pubFunc.Str_Tab2 + "public static int Update({0} model)" + pubFunc.Str_Enter;
            daFormat += pubFunc.Str_Tab2 + "{{" + pubFunc.Str_Enter;
            daFormat += pubFunc.Str_Tab3 + "return UpdateFromEntity<{0}>(model);" + pubFunc.Str_Enter;
            daFormat += pubFunc.Str_Tab2 + "}}" + pubFunc.Str_Enter;
            //批量更新
            daFormat += pubFunc.Str_Tab2 + "//批量更新" + pubFunc.Str_Enter;
            daFormat += pubFunc.Str_Tab2 + "public static int UpdateMul({0} model,"+primaryKeyType.ToLower()+"[] ids)" + pubFunc.Str_Enter;
            daFormat += pubFunc.Str_Tab2 + "{{" + pubFunc.Str_Enter;
            daFormat += pubFunc.Str_Tab3 + "return UpdateFromIds<{0}>(model,ids);" + pubFunc.Str_Enter;
            daFormat += pubFunc.Str_Tab2 + "}}" + pubFunc.Str_Enter;

            daFormat += pubFunc.Str_Tab2 + "public static int Delete({1} id)" + pubFunc.Str_Enter;
            daFormat += pubFunc.Str_Tab2 + "{{" + pubFunc.Str_Enter;
            daFormat += pubFunc.Str_Tab3 + "return DeleteByPrimaryKey<{0}>(id);" + pubFunc.Str_Enter;
            daFormat += pubFunc.Str_Tab2 + "}}" + pubFunc.Str_Enter;
            daFormat += pubFunc.Str_Tab2 + "public static {0} Get{0}ByPK({1} id)" + pubFunc.Str_Enter;
            daFormat += pubFunc.Str_Tab2 + "{{" + pubFunc.Str_Enter;
            daFormat += pubFunc.Str_Tab3 + "return ExecuteToEntityByPrimaryKey<{0}>(id);" + pubFunc.Str_Enter;
            daFormat += pubFunc.Str_Tab2 + "}}" + pubFunc.Str_Enter;
            daFormat += pubFunc.Str_Tab2 + "public static List<{0}> Get{0}List(SearchBase search, string key)" + pubFunc.Str_Enter;
            daFormat += pubFunc.Str_Tab2 + "{{" + pubFunc.Str_Enter;
            daFormat += pubFunc.Str_Tab3 + "List<SqlParameter> parameters = new List<SqlParameter>();" + pubFunc.Str_Enter;
            daFormat += pubFunc.Str_Tab3 + "if (!string.IsNullOrEmpty(key))" + pubFunc.Str_Enter;
            daFormat += pubFunc.Str_Tab3 + "{{" + pubFunc.Str_Enter;
            daFormat += pubFunc.Str_Tab3 + pubFunc.Str_Tab + "search.Where += \" and <search field> like @key escape '$'\";" + pubFunc.Str_Enter;
            daFormat += pubFunc.Str_Tab3 + pubFunc.Str_Tab + "parameters.Add(SqlHelper.GetSqlParameter(\"@key\", \"%\" + SQLFilterHelper.EscapeLikeString(key, \"$\") + \"%\", SqlDbType.NVarChar, <search field length>));" + pubFunc.Str_Enter;
            daFormat += pubFunc.Str_Tab3 + "}}" + pubFunc.Str_Enter;
            daFormat += pubFunc.Str_Tab3 + "string sql = SQL.{0}.{0}List;" + pubFunc.Str_Enter;
            daFormat += pubFunc.Str_Tab3 + "sql = HandleSQL(search, sql);" + pubFunc.Str_Enter;
            daFormat += pubFunc.Str_Tab3 + "return ExecuteToEntityList<{0}>(sql, System.Data.CommandType.Text, parameters);" + pubFunc.Str_Enter;            
            daFormat += pubFunc.Str_Tab2 + "}}" + pubFunc.Str_Enter;
            daFormat += pubFunc.Str_Tab + "}}" + pubFunc.Str_Enter;
            daFormat += "}}" + pubFunc.Str_Enter;
            txtDA.Text = string.Format(daFormat, new string[] { clearedTableName, primaryKeyType, insertMethodName });

            string path = applicationPath + "DataAccess";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = path + "\\" + dt.TableName.Replace("_", "") + ".cs";
            File.WriteAllText(filepath, txtDA.Text);
        }

        /// <summary>
        /// 绑定逻辑层代码
        /// </summary>
        private void BindLogic()
        {
            string logicFormat = string.Empty;
            logicFormat += "using System;" + pubFunc.Str_Enter;
            logicFormat += "using System.Collections.Generic;" + pubFunc.Str_Enter;
            logicFormat += "using System.Linq;" + pubFunc.Str_Enter;
            logicFormat += "using System.Text;" + pubFunc.Str_Enter;
            logicFormat += "using CAS.Entity.DBEntity;" + pubFunc.Str_Enter;
            logicFormat += "using CAS.DataAccess;" + pubFunc.Str_Enter;
            logicFormat += pubFunc.Str_Enter;
            logicFormat += "namespace CAS.Logic" + pubFunc.Str_Enter;
            logicFormat += "{{" + pubFunc.Str_Enter;
            logicFormat += pubFunc.Str_Tab + "public class {0}BL" + pubFunc.Str_Enter;
            logicFormat += pubFunc.Str_Tab + "{{" + pubFunc.Str_Enter;
            logicFormat += pubFunc.Str_Tab2 + "public static {1} Add({0} model)" + pubFunc.Str_Enter;
            logicFormat += pubFunc.Str_Tab2 + "{{" + pubFunc.Str_Enter;
            logicFormat += pubFunc.Str_Tab3 + "return {0}DA.Add(model);" + pubFunc.Str_Enter;
            logicFormat += pubFunc.Str_Tab2 + "}}" + pubFunc.Str_Enter;
            logicFormat += pubFunc.Str_Tab2 + "public static int Update({0} model)" + pubFunc.Str_Enter;
            logicFormat += pubFunc.Str_Tab2 + "{{" + pubFunc.Str_Enter;
            logicFormat += pubFunc.Str_Tab3 + "return {0}DA.Update(model);" + pubFunc.Str_Enter;
            logicFormat += pubFunc.Str_Tab2 + "}}" + pubFunc.Str_Enter;
            //批量更新
            logicFormat += pubFunc.Str_Tab2 + "//批量更新" + pubFunc.Str_Enter;
            logicFormat += pubFunc.Str_Tab2 + "public static int UpdateMul({0} model," + primaryKeyType.ToLower() + "[] ids)" + pubFunc.Str_Enter;
            logicFormat += pubFunc.Str_Tab2 + "{{" + pubFunc.Str_Enter;
            logicFormat += pubFunc.Str_Tab3 + "return {0}DA.UpdateMul(model,ids);" + pubFunc.Str_Enter;
            logicFormat += pubFunc.Str_Tab2 + "}}" + pubFunc.Str_Enter;

            logicFormat += pubFunc.Str_Tab2 + "public static int Delete({1} id)" + pubFunc.Str_Enter;
            logicFormat += pubFunc.Str_Tab2 + "{{" + pubFunc.Str_Enter;
            logicFormat += pubFunc.Str_Tab3 + "return {0}DA.Delete(id);" + pubFunc.Str_Enter;
            logicFormat += pubFunc.Str_Tab2 + "}}" + pubFunc.Str_Enter;            
            logicFormat += pubFunc.Str_Tab2 + "public static int DeleteOnLogical({1} id)" + pubFunc.Str_Enter;
            logicFormat += pubFunc.Str_Tab2 + "{{" + pubFunc.Str_Enter;
            logicFormat += pubFunc.Str_Tab3 + "{0} model = new {0}();" + pubFunc.Str_Enter;
            logicFormat += pubFunc.Str_Tab3 + "model.{1} = id;" + pubFunc.Str_Enter;
            logicFormat += pubFunc.Str_Tab3 + "model.valid = 0;" + pubFunc.Str_Enter;
            logicFormat += pubFunc.Str_Tab3 + "model.SetAvailableFields(new string[] {{ \"valid\" }});" + pubFunc.Str_Enter;
            logicFormat += pubFunc.Str_Tab3 + "return {0}DA.Update(model);" + pubFunc.Str_Enter;
            logicFormat += pubFunc.Str_Tab2 + "}}" + pubFunc.Str_Enter;
            logicFormat += pubFunc.Str_Tab2 + "public static {0} Get{0}ByPK({1} id)" + pubFunc.Str_Enter;
            logicFormat += pubFunc.Str_Tab2 + "{{" + pubFunc.Str_Enter;
            logicFormat += pubFunc.Str_Tab3 + "return {0}DA.Get{0}ByPK(id); " + pubFunc.Str_Enter;
            logicFormat += pubFunc.Str_Tab2 + "}}" + pubFunc.Str_Enter;
            logicFormat += pubFunc.Str_Tab2 + "public static List<{0}> Get{0}List(SearchBase search, string key)" + pubFunc.Str_Enter;
            logicFormat += pubFunc.Str_Tab2 + "{{" + pubFunc.Str_Enter;
            logicFormat += pubFunc.Str_Tab3 + "return {0}DA.Get{0}List(search, key); " + pubFunc.Str_Enter;
            logicFormat += pubFunc.Str_Tab2 + "}}" + pubFunc.Str_Enter;
            logicFormat += pubFunc.Str_Tab + "}}" + pubFunc.Str_Enter;
            logicFormat += "}}";
            txtLogic.Text = string.Format(logicFormat, new object[]{clearedTableName, primaryKeyType});

            string path = applicationPath + "Logic";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = path + "\\" + dt.TableName.Replace("_", "") + ".cs";
            File.WriteAllText(filepath, txtLogic.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataTable dt = ((DataTable)DList.DataSource);
            for (int i = 0; i < dt.Rows.Count; i++)
			{
                dt.Rows[i][0] = 1;
			}            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DataTable dt = ((DataTable)DList.DataSource);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i][0] = 0;
            }    
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (ShowTableNames != null && ShowTableNames.Count > 0)
            {
                foreach (string tablename in ShowTableNames)
                {
                    BindTableColumns(GetTableName(tablename));
                    if (DList.DataSource == null)
                    {
                        return;
                    }
                    pubFunc.CurrentTable = ((DataTable)DList.DataSource);
                    pubFunc.CurrentTable.TableName = tablename;
                    dt = pubFunc.CurrentTable;
                    clearedTableName = dt.TableName.Replace("_", "");
                    rows = dt.Select("chk=1");
                    DataRow[] primaryKeyRows = dt.Select("主键<>''");
                    if (null != primaryKeyRows && primaryKeyRows.Length > 0)
                    {
                        primaryKey = rows[0]["字段名"].ToString().ToLower() ?? "";
                        primaryKeyType = pubFunc.GetFieldType(rows[0]["类型"].ToString());
                    }

                    BindEntity();
                    BindLogic();
                    BindDA();
                }
            }
        }

        private void DList_Navigate(object sender, NavigateEventArgs ne)
        {

        }

        private void DList_MouseDown(object sender, MouseEventArgs e)
        {
            DataGrid.HitTestInfo hti = DList.HitTest(new Point(e.X, e.Y));
            if (hti.Column == 0 )
            {
                ((DataTable)DList.DataSource).Rows[hti.Row][0] = ((DataTable)DList.DataSource).Rows[hti.Row][0].ToString() == "True" ? 0 : 1;
            }
        }
	}

	#region 重载DataGridTextBoxColumn类，设置背景色
	public class SQLColumn:System.Windows.Forms.DataGridTextBoxColumn
	{

		private Color bgColor;
		public SQLColumn(Color _bgColor) 
		{
			bgColor=_bgColor;
		}

		protected override void Paint(Graphics g, Rectangle bounds, CurrencyManager source, int rowNum, Brush backBrush, Brush foreBrush, bool alignToRight)
		{
			if(bgColor.ToString()!="")
				backBrush=new SolidBrush(bgColor);
			else
				backBrush=new SolidBrush(TextBox.BackColor);
			foreBrush=new SolidBrush(Color.Black);
			base.Paint (g, bounds, source, rowNum, backBrush, foreBrush, alignToRight);
		}
		
	}
	#endregion


}
