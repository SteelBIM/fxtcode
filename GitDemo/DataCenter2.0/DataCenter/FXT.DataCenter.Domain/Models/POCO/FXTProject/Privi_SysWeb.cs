using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Privi_SysWeb
    {
        private string _weburl;
        /// <summary>
        /// 网址
        /// </summary>
        //[SQLField("weburl", EnumDBFieldUsage.PrimaryKey)]
        public string weburl
        {
            get { return _weburl; }
            set { _weburl = value; }
        }
        private int _valid = 1;
        public int valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        private int _fk_systypecode = 1003001;
        /// <summary>
        /// 系统类型
        /// </summary>
        public int fk_systypecode
        {
            get { return _fk_systypecode; }
            set { _fk_systypecode = value; }
        }
        private string _sysdefaultpage;
        /// <summary>
        /// 默认页
        /// </summary>
        public string sysdefaultpage
        {
            get { return _sysdefaultpage; }
            set { _sysdefaultpage = value; }
        }
        private string _sysloginpage;
        /// <summary>
        /// 登录页
        /// </summary>
        public string sysloginpage
        {
            get { return _sysloginpage; }
            set { _sysloginpage = value; }
        }
        private string _syscsspath;
        /// <summary>
        /// CSS文件路径
        /// </summary>
        public string syscsspath
        {
            get { return _syscsspath; }
            set { _syscsspath = value; }
        }
        private string _sysjspath;
        /// <summary>
        /// JS文件路径
        /// </summary>
        public string sysjspath
        {
            get { return _sysjspath; }
            set { _sysjspath = value; }
        }
        private string _sysimagepath;
        /// <summary>
        /// 系统图片文件路径
        /// </summary>
        public string sysimagepath
        {
            get { return _sysimagepath; }
            set { _sysimagepath = value; }
        }
        private string _sysphotopath;
        /// <summary>
        /// 业务照片文件路径
        /// </summary>
        public string sysphotopath
        {
            get { return _sysphotopath; }
            set { _sysphotopath = value; }
        }
        private string _skinpath;
        /// <summary>
        /// 皮肤
        /// </summary>
        public string skinpath
        {
            get { return _skinpath; }
            set { _skinpath = value; }
        }
        private string _ftpuser;
        public string ftpuser
        {
            get { return _ftpuser; }
            set { _ftpuser = value; }
        }
        private string _ftppass;
        public string ftppass
        {
            get { return _ftppass; }
            set { _ftppass = value; }
        }
        private string _ftpip;
        public string ftpip
        {
            get { return _ftpip; }
            set { _ftpip = value; }
        }
        private string _picpath;
        /// <summary>
        /// 业务上传照片路径
        /// </summary>
        public string picpath
        {
            get { return _picpath; }
            set { _picpath = value; }
        }
        private string _websysname;
        public string websysname
        {
            get { return _websysname; }
            set { _websysname = value; }
        }
        private string _innersysname;
        public string innersysname
        {
            get { return _innersysname; }
            set { _innersysname = value; }
        }
        private string _soasysname;
        public string soasysname
        {
            get { return _soasysname; }
            set { _soasysname = value; }
        }
        private string _surveysysname;
        public string surveysysname
        {
            get { return _surveysysname; }
            set { _surveysysname = value; }
        }
        private string _staticweburl;
        /// <summary>
        /// 静态文件URL
        /// </summary>
        public string staticweburl
        {
            get { return _staticweburl; }
            set { _staticweburl = value; }
        }
        private string _apiurl;
        public string apiurl
        {
            get { return _apiurl; }
            set { _apiurl = value; }
        }
        private string _loginurl;
        /// <summary>
        /// 登录URL
        /// </summary>
        public string loginurl
        {
            get { return _loginurl; }
            set { _loginurl = value; }
        }
        private string _themename;
        /// <summary>
        /// 主题名称
        /// </summary>
        public string themename
        {
            get { return _themename; }
            set { _themename = value; }
        }
        private string _stylename;
        /// <summary>
        /// 样式名称
        /// </summary>
        public string stylename
        {
            get { return _stylename; }
            set { _stylename = value; }
        }
        private string _msgserverpath;
        /// <summary>
        /// 消息服务路径
        /// </summary>
        public string msgserverpath
        {
            get { return _msgserverpath; }
            set { _msgserverpath = value; }
        }
        private string _surveycenterurl;
        public string surveycenterurl
        {
            get { return _surveycenterurl; }
            set { _surveycenterurl = value; }
        }

    }
}
