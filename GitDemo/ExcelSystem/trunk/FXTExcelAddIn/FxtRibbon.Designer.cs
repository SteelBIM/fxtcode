namespace FXTExcelAddIn
{
    partial class FxtRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public FxtRibbon()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

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
            this.tab1 = this.Factory.CreateRibbonTab();
            this.groupCase = this.Factory.CreateRibbonGroup();
            this.btnSelCity = this.Factory.CreateRibbonButton();
            this.toggleButtonCase = this.Factory.CreateRibbonToggleButton();
            this.toggleButtonProjects = this.Factory.CreateRibbonToggleButton();
            this.toggleButtonAddress = this.Factory.CreateRibbonToggleButton();
            this.group5 = this.Factory.CreateRibbonGroup();
            this.galleryMath = this.Factory.CreateRibbonGallery();
            this.btnMath_1 = this.Factory.CreateRibbonButton();
            this.group1 = this.Factory.CreateRibbonGroup();
            this.galleryStat = this.Factory.CreateRibbonGallery();
            this.btn_Stat_Line = this.Factory.CreateRibbonButton();
            this.btn_Stat_Bar1 = this.Factory.CreateRibbonButton();
            this.btn_Stat_Pie = this.Factory.CreateRibbonButton();
            this.btn_Stat_Bar = this.Factory.CreateRibbonButton();
            this.btn_Stat_Scatter = this.Factory.CreateRibbonButton();
            this.btn_Stat_Pie1 = this.Factory.CreateRibbonButton();
            this.btn_Stat_Bar2 = this.Factory.CreateRibbonButton();
            this.galleryDataMap = this.Factory.CreateRibbonGallery();
            this.btn_DataMap_Hot = this.Factory.CreateRibbonButton();
            this.group2 = this.Factory.CreateRibbonGroup();
            this.splitButtonMap = this.Factory.CreateRibbonSplitButton();
            this.splitButtonHouseWeb = this.Factory.CreateRibbonSplitButton();
            this.btnDataCenter = this.Factory.CreateRibbonButton();
            this.group4 = this.Factory.CreateRibbonGroup();
            this.galleryHelp = this.Factory.CreateRibbonGallery();
            this.btn_help_1 = this.Factory.CreateRibbonButton();
            this.btn_help_about = this.Factory.CreateRibbonButton();
            this.btn_DataMap_Selected = this.Factory.CreateRibbonButton();
            this.tab1.SuspendLayout();
            this.groupCase.SuspendLayout();
            this.group5.SuspendLayout();
            this.group1.SuspendLayout();
            this.group2.SuspendLayout();
            this.group4.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.Groups.Add(this.groupCase);
            this.tab1.Groups.Add(this.group5);
            this.tab1.Groups.Add(this.group1);
            this.tab1.Groups.Add(this.group2);
            this.tab1.Groups.Add(this.group4);
            this.tab1.Label = "房讯通";
            this.tab1.Name = "tab1";
            // 
            // groupCase
            // 
            this.groupCase.Items.Add(this.btnSelCity);
            this.groupCase.Items.Add(this.toggleButtonCase);
            this.groupCase.Items.Add(this.toggleButtonProjects);
            this.groupCase.Items.Add(this.toggleButtonAddress);
            this.groupCase.Label = "数据处理";
            this.groupCase.Name = "groupCase";
            // 
            // btnSelCity
            // 
            this.btnSelCity.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnSelCity.Image = global::FXTExcelAddIn.Properties.Resources._9;
            this.btnSelCity.Label = "选择城市";
            this.btnSelCity.Name = "btnSelCity";
            this.btnSelCity.ShowImage = true;
            this.btnSelCity.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnSelCity_Click);
            // 
            // toggleButtonCase
            // 
            this.toggleButtonCase.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.toggleButtonCase.Image = global::FXTExcelAddIn.Properties.Resources._0;
            this.toggleButtonCase.Label = "案例处理";
            this.toggleButtonCase.Name = "toggleButtonCase";
            this.toggleButtonCase.ShowImage = true;
            this.toggleButtonCase.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.toggleButtonCase_Click);
            // 
            // toggleButtonProjects
            // 
            this.toggleButtonProjects.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.toggleButtonProjects.Image = global::FXTExcelAddIn.Properties.Resources._3;
            this.toggleButtonProjects.Label = "基础数据";
            this.toggleButtonProjects.Name = "toggleButtonProjects";
            this.toggleButtonProjects.ShowImage = true;
            // 
            // toggleButtonAddress
            // 
            this.toggleButtonAddress.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.toggleButtonAddress.Image = global::FXTExcelAddIn.Properties.Resources._7;
            this.toggleButtonAddress.Label = "地址拆分";
            this.toggleButtonAddress.Name = "toggleButtonAddress";
            this.toggleButtonAddress.ShowImage = true;
            // 
            // group5
            // 
            this.group5.Items.Add(this.galleryMath);
            this.group5.Name = "group5";
            // 
            // galleryMath
            // 
            this.galleryMath.Buttons.Add(this.btnMath_1);
            this.galleryMath.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.galleryMath.Image = global::FXTExcelAddIn.Properties.Resources._6;
            this.galleryMath.Label = "常用公式";
            this.galleryMath.Name = "galleryMath";
            this.galleryMath.ShowImage = true;
            this.galleryMath.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.gallery1_Click);
            // 
            // btnMath_1
            // 
            this.btnMath_1.Label = "=sum()";
            this.btnMath_1.Name = "btnMath_1";
            // 
            // group1
            // 
            this.group1.Items.Add(this.galleryStat);
            this.group1.Items.Add(this.galleryDataMap);
            this.group1.Label = "可视化";
            this.group1.Name = "group1";
            // 
            // galleryStat
            // 
            this.galleryStat.Buttons.Add(this.btn_Stat_Line);
            this.galleryStat.Buttons.Add(this.btn_Stat_Bar1);
            this.galleryStat.Buttons.Add(this.btn_Stat_Pie);
            this.galleryStat.Buttons.Add(this.btn_Stat_Bar);
            this.galleryStat.Buttons.Add(this.btn_Stat_Scatter);
            this.galleryStat.Buttons.Add(this.btn_Stat_Pie1);
            this.galleryStat.Buttons.Add(this.btn_Stat_Bar2);
            this.galleryStat.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.galleryStat.Image = global::FXTExcelAddIn.Properties.Resources._8;
            this.galleryStat.Label = "统计图表";
            this.galleryStat.Name = "galleryStat";
            this.galleryStat.ShowImage = true;
            this.galleryStat.ButtonClick += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.galleryStat_ButtonClick);
            this.galleryStat.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.galleryStat_Click);
            // 
            // btn_Stat_Line
            // 
            this.btn_Stat_Line.Image = global::FXTExcelAddIn.Properties.Resources._12;
            this.btn_Stat_Line.Label = "折线图";
            this.btn_Stat_Line.Name = "btn_Stat_Line";
            this.btn_Stat_Line.ShowImage = true;
            // 
            // btn_Stat_Bar1
            // 
            this.btn_Stat_Bar1.Image = global::FXTExcelAddIn.Properties.Resources._13;
            this.btn_Stat_Bar1.Label = "柱状图";
            this.btn_Stat_Bar1.Name = "btn_Stat_Bar1";
            this.btn_Stat_Bar1.ShowImage = true;
            // 
            // btn_Stat_Pie
            // 
            this.btn_Stat_Pie.Image = global::FXTExcelAddIn.Properties.Resources._15;
            this.btn_Stat_Pie.Label = "饼图";
            this.btn_Stat_Pie.Name = "btn_Stat_Pie";
            this.btn_Stat_Pie.ShowImage = true;
            // 
            // btn_Stat_Bar
            // 
            this.btn_Stat_Bar.Image = global::FXTExcelAddIn.Properties.Resources._14;
            this.btn_Stat_Bar.Label = "条形图";
            this.btn_Stat_Bar.Name = "btn_Stat_Bar";
            this.btn_Stat_Bar.ShowImage = true;
            // 
            // btn_Stat_Scatter
            // 
            this.btn_Stat_Scatter.Enabled = false;
            this.btn_Stat_Scatter.Image = global::FXTExcelAddIn.Properties.Resources._19;
            this.btn_Stat_Scatter.Label = "散点图";
            this.btn_Stat_Scatter.Name = "btn_Stat_Scatter";
            this.btn_Stat_Scatter.ShowImage = true;
            // 
            // btn_Stat_Pie1
            // 
            this.btn_Stat_Pie1.Image = global::FXTExcelAddIn.Properties.Resources._16;
            this.btn_Stat_Pie1.Label = "环形图";
            this.btn_Stat_Pie1.Name = "btn_Stat_Pie1";
            this.btn_Stat_Pie1.ShowImage = true;
            // 
            // btn_Stat_Bar2
            // 
            this.btn_Stat_Bar2.Image = global::FXTExcelAddIn.Properties.Resources._18;
            this.btn_Stat_Bar2.Label = "瀑布图";
            this.btn_Stat_Bar2.Name = "btn_Stat_Bar2";
            this.btn_Stat_Bar2.ShowImage = true;
            // 
            // galleryDataMap
            // 
            this.galleryDataMap.Buttons.Add(this.btn_DataMap_Hot);
            this.galleryDataMap.Buttons.Add(this.btn_DataMap_Selected);
            this.galleryDataMap.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.galleryDataMap.Image = global::FXTExcelAddIn.Properties.Resources._11;
            this.galleryDataMap.Label = "数据地图";
            this.galleryDataMap.Name = "galleryDataMap";
            this.galleryDataMap.ShowImage = true;
            this.galleryDataMap.ButtonClick += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.galleryDataMap_ButtonClick);
            // 
            // btn_DataMap_Hot
            // 
            this.btn_DataMap_Hot.Image = global::FXTExcelAddIn.Properties.Resources._17;
            this.btn_DataMap_Hot.Label = "热力图";
            this.btn_DataMap_Hot.Name = "btn_DataMap_Hot";
            this.btn_DataMap_Hot.ShowImage = true;
            // 
            // group2
            // 
            this.group2.Items.Add(this.splitButtonMap);
            this.group2.Items.Add(this.splitButtonHouseWeb);
            this.group2.Items.Add(this.btnDataCenter);
            this.group2.Label = "网络";
            this.group2.Name = "group2";
            // 
            // splitButtonMap
            // 
            this.splitButtonMap.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.splitButtonMap.Image = global::FXTExcelAddIn.Properties.Resources._4;
            this.splitButtonMap.Label = "地图";
            this.splitButtonMap.Name = "splitButtonMap";
            this.splitButtonMap.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.splitButtonMap_Click);
            // 
            // splitButtonHouseWeb
            // 
            this.splitButtonHouseWeb.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.splitButtonHouseWeb.Image = global::FXTExcelAddIn.Properties.Resources._5;
            this.splitButtonHouseWeb.Label = "房地产网站";
            this.splitButtonHouseWeb.Name = "splitButtonHouseWeb";
            // 
            // btnDataCenter
            // 
            this.btnDataCenter.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnDataCenter.Image = global::FXTExcelAddIn.Properties.Resources._2;
            this.btnDataCenter.Label = "数据中心";
            this.btnDataCenter.Name = "btnDataCenter";
            this.btnDataCenter.ShowImage = true;
            this.btnDataCenter.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.button5_Click);
            // 
            // group4
            // 
            this.group4.Items.Add(this.galleryHelp);
            this.group4.Name = "group4";
            // 
            // galleryHelp
            // 
            this.galleryHelp.Buttons.Add(this.btn_help_1);
            this.galleryHelp.Buttons.Add(this.btn_help_about);
            this.galleryHelp.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.galleryHelp.Image = global::FXTExcelAddIn.Properties.Resources._1;
            this.galleryHelp.Label = "帮助";
            this.galleryHelp.Name = "galleryHelp";
            this.galleryHelp.ShowImage = true;
            this.galleryHelp.ButtonClick += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.gallery1_ButtonClick);
            this.galleryHelp.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.gallery1_Click_1);
            // 
            // btn_help_1
            // 
            this.btn_help_1.Label = "Excel学习论坛";
            this.btn_help_1.Name = "btn_help_1";
            // 
            // btn_help_about
            // 
            this.btn_help_about.Label = "关于";
            this.btn_help_about.Name = "btn_help_about";
            // 
            // btn_DataMap_Selected
            // 
            this.btn_DataMap_Selected.Label = "省市数据图";
            this.btn_DataMap_Selected.Name = "btn_DataMap_Selected";
            // 
            // FxtRibbon
            // 
            this.Name = "FxtRibbon";
            this.RibbonType = "Microsoft.Excel.Workbook";
            this.Tabs.Add(this.tab1);
            this.Close += new System.EventHandler(this.FxtRibbon_Close);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.FxtRibbon_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.groupCase.ResumeLayout(false);
            this.groupCase.PerformLayout();
            this.group5.ResumeLayout(false);
            this.group5.PerformLayout();
            this.group1.ResumeLayout(false);
            this.group1.PerformLayout();
            this.group2.ResumeLayout(false);
            this.group2.PerformLayout();
            this.group4.ResumeLayout(false);
            this.group4.PerformLayout();

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonGroup groupCase;
        internal Microsoft.Office.Tools.Ribbon.RibbonToggleButton toggleButtonCase;
        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group2;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group4;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnDataCenter;
        internal Microsoft.Office.Tools.Ribbon.RibbonSplitButton splitButtonMap;
        internal Microsoft.Office.Tools.Ribbon.RibbonSplitButton splitButtonHouseWeb;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group5;
        internal Microsoft.Office.Tools.Ribbon.RibbonGallery galleryMath;
        private Microsoft.Office.Tools.Ribbon.RibbonButton btnMath_1;
        internal Microsoft.Office.Tools.Ribbon.RibbonToggleButton toggleButtonProjects;
        internal Microsoft.Office.Tools.Ribbon.RibbonToggleButton toggleButtonAddress;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnSelCity;
        internal Microsoft.Office.Tools.Ribbon.RibbonGallery galleryHelp;
        private Microsoft.Office.Tools.Ribbon.RibbonButton btn_help_1;
        private Microsoft.Office.Tools.Ribbon.RibbonButton btn_help_about;
        internal Microsoft.Office.Tools.Ribbon.RibbonGallery galleryDataMap;
        private Microsoft.Office.Tools.Ribbon.RibbonButton btn_DataMap_Hot;
        internal Microsoft.Office.Tools.Ribbon.RibbonGallery galleryStat;
        private Microsoft.Office.Tools.Ribbon.RibbonButton btn_Stat_Bar;
        private Microsoft.Office.Tools.Ribbon.RibbonButton btn_Stat_Line;
        private Microsoft.Office.Tools.Ribbon.RibbonButton btn_Stat_Bar1;
        private Microsoft.Office.Tools.Ribbon.RibbonButton btn_Stat_Pie;
        private Microsoft.Office.Tools.Ribbon.RibbonButton btn_Stat_Scatter;
        private Microsoft.Office.Tools.Ribbon.RibbonButton btn_Stat_Pie1;
        private Microsoft.Office.Tools.Ribbon.RibbonButton btn_Stat_Bar2;
        private Microsoft.Office.Tools.Ribbon.RibbonButton btn_DataMap_Selected;
    }

    partial class ThisRibbonCollection
    {
        internal FxtRibbon FxtRibbon
        {
            get { return this.GetRibbon<FxtRibbon>(); }
        }
    }
}
