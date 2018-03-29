using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.SYS_Config")]
    public class SYSConfig : BaseTO
    {
        private int _configid;
        [SQLField("configid", EnumDBFieldUsage.PrimaryKey, true)]
        public int configid
        {
            get { return _configid; }
            set { _configid = value; }
        }
        private bool _queryapproval = false;
        /// <summary>
        /// 询价是否需要审批
        /// </summary>
        public bool queryapproval
        {
            get { return _queryapproval; }
            set { _queryapproval = value; }
        }
        private bool _queryassign = false;
        /// <summary>
        /// 询价是否需要分配
        /// </summary>
        public bool queryassign
        {
            get { return _queryassign; }
            set { _queryassign = value; }
        }

        private string _systemname;
        /// <summary>
        /// 系统名称
        /// </summary>
        public string systemname
        {
            get { return _systemname; }
            set { _systemname = value; }
        }
        private string _systemnamecolor = "#000";
        /// <summary>
        /// 系统名称颜色
        /// </summary>
        public string systemnamecolor
        {
            get { return _systemnamecolor; }
            set { _systemnamecolor = value; }
        }
        private string _topbannerbackcolor = "#eee";
        /// <summary>
        /// 顶部背景色
        /// </summary>
        public string topbannerbackcolor
        {
            get { return _topbannerbackcolor; }
            set { _topbannerbackcolor = value; }
        }
        private string _topbannerbackimage = "";
        /// <summary>
        /// 顶部背景图片
        /// </summary>
        public string topbannerbackimage
        {
            get { return _topbannerbackimage; }
            set { _topbannerbackimage = value; }
        }
        private string _menubackcolor = "#566FAD";
        /// <summary>
        /// 菜单背景色
        /// </summary>
        public string menubackcolor
        {
            get { return _menubackcolor; }
            set { _menubackcolor = value; }
        }
        private string _menucolor = "#FFF";
        /// <summary>
        /// 菜单颜色
        /// </summary>
        public string menucolor
        {
            get { return _menucolor; }
            set { _menucolor = value; }
        }
        private string _buttonbackcolor = "#566FAD";
        /// <summary>
        /// 按钮背景色
        /// </summary>
        public string buttonbackcolor
        {
            get { return _buttonbackcolor; }
            set { _buttonbackcolor = value; }
        }
        private string _buttoncolor = "#FFF";
        /// <summary>
        /// 按钮颜色
        /// </summary>
        public string buttoncolor
        {
            get { return _buttoncolor; }
            set { _buttoncolor = value; }
        }
        private string _danweiname;
        /// <summary>
        /// 单位名称
        /// </summary>
        public string danweiname
        {
            get { return _danweiname; }
            set { _danweiname = value; }
        }
        private string _logourl;
        /// <summary>
        /// 单位logo路径
        /// </summary>
        public string logourl
        {
            get { return _logourl; }
            set { _logourl = value; }
        }
        private bool _confirmreassign = false;
        /// <summary>
        /// 转交是否需要确认
        /// </summary>
        public bool confirmreassign
        {
            get { return _confirmreassign; }
            set { _confirmreassign = value; }
        }
        /// <summary>
        /// 一键下载地图宽
        /// </summary>
        private decimal _mapheight = 13M;
        public decimal mapheight
        {
            get { return _mapheight; }
            set { _mapheight = value; }
        }
        /// <summary>
        /// 一键下载地图高
        /// </summary>
        private decimal _mapwidth = 15M;
        public decimal mapwidth
        {
            get { return _mapwidth; }
            set { _mapwidth = value; }
        }
        private string _marklogourl;
        /// <summary>
        /// 公司Logo水印
        /// </summary>
        public string marklogourl
        {
            get { return _marklogourl; }
            set { _marklogourl = value; }
        }

        private bool _yptoreportassign = false;
        /// <summary>
        /// 预评转报告是否需要分配
        /// </summary>
        public bool yptoreportassign
        {
            get { return _yptoreportassign; }
            set { _yptoreportassign = value; }
        }

        private string _messagecontent;
        /// <summary>
        /// 短信内容
        /// </summary>
        public string messagecontent
        {
            get { return _messagecontent; }
            set { _messagecontent = value; }
        }
        private string _messagetitle;
        /// <summary>
        /// 短信模板开头内容
        /// </summary>
        public string messagetitle
        {
            get { return _messagetitle; }
            set { _messagetitle = value; }
        }
        private bool _isneedassignentrust = false;
        /// <summary>
        /// 是否需要分配业务
        /// </summary>
        public bool isneedassignentrust
        {
            get { return _isneedassignentrust; }
            set { _isneedassignentrust = value; }
        }

        private bool _samereportno = false;
        /// <summary>
        /// 预评报告是否使用相同编号
        /// </summary>
        public bool samereportno
        {
            get { return _samereportno; }
            set { _samereportno = value; }
        }

        private bool _candownload = false;
        /// <summary>
        /// 能否下载
        /// </summary>
        public bool candownload
        {
            get { return _candownload; }
            set { _candownload = value; }
        }

        private bool _objfullname = false;
        /// <summary>
        /// 楼层是否拼入委估对象全称
        /// </summary>
        public bool objfullname
        {
            get { return _objfullname; }
            set { _objfullname = value; }
        }
        private bool _approval = false;
        /// <summary>
        /// 审批时填写审批价格信息
        /// </summary>
        public bool approval
        {
            get { return _approval; }
            set { _approval = value; }
        }
        private bool _repeatlogin = false;
        /// <summary>
        /// 是否允许重复登录 0为不允许、1为允许
        /// </summary>
        public bool repeatlogin
        {
            get { return _repeatlogin; }
            set { _repeatlogin = value; }
        }

        private string _linkcolor;
        public string linkcolor
        {
            get { return _linkcolor; }
            set { _linkcolor = value; }
        }
        private bool _ypcharge=false;
        /// <summary>
        /// 预评是否收费 true：是，false：否
        /// </summary>
        public bool ypcharge
        {
            get { return _ypcharge; }
            set { _ypcharge = value; }
        }
        private bool _isquerystamp = false;
        /// <summary>
        /// cas询价单是否需要盖章
        /// </summary>
        public bool isquerystamp
        {
            get { return _isquerystamp; }
            set { _isquerystamp = value; }
        }
    }
}