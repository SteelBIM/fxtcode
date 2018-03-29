using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.SYS_DanWeiInfo")]
    public class SYSDanWeiInfo : BaseTO
    {
        private int _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _companycode;
        public string companycode
        {
            get { return _companycode; }
            set { _companycode = value; }
        }
        private string _tel;
        /// <summary>
        /// 电话
        /// </summary>
        public string tel
        {
            get { return _tel; }
            set { _tel = value; }
        }
        private string _fax;
        /// <summary>
        /// 传真
        /// </summary>
        public string fax
        {
            get { return _fax; }
            set { _fax = value; }
        }
        private string _youbian;
        /// <summary>
        /// 邮编
        /// </summary>
        public string youbian
        {
            get { return _youbian; }
            set { _youbian = value; }
        }
        private string _address;
        /// <summary>
        /// 地址
        /// </summary>
        public string address
        {
            get { return _address; }
            set { _address = value; }
        }
        private string _weburl;
        /// <summary>
        /// 网站
        /// </summary>
        public string weburl
        {
            get { return _weburl; }
            set { _weburl = value; }
        }
        private string _email;
        /// <summary>
        /// 邮箱
        /// </summary>
        public string email
        {
            get { return _email; }
            set { _email = value; }
        }
        private string _kaihuhang;
        /// <summary>
        /// 开户行
        /// </summary>
        public string kaihuhang
        {
            get { return _kaihuhang; }
            set { _kaihuhang = value; }
        }
        private string _zhanghao;
        /// <summary>
        /// 账号
        /// </summary>
        public string zhanghao
        {
            get { return _zhanghao; }
            set { _zhanghao = value; }
        }        
        private int? _yptoreportalertday;
        /// <summary>
        /// 预评报告转报告预警天数
        /// </summary>
        public int? yptoreportalertday
        {
            get { return _yptoreportalertday; }
            set { _yptoreportalertday = value; }
        }
        private int? _reportendalertday;
        /// <summary>
        /// 距离报告出具时间预警天数
        /// </summary>
        public int? reportendalertday
        {
            get { return _reportendalertday; }
            set { _reportendalertday = value; }
        }
        private string _reportno;
        /// <summary>
        /// 报告编号格式
        /// </summary>
        public string reportno
        {
            get { return _reportno; }
            set { _reportno = value; }
        }
        private string _ypno;
        /// <summary>
        /// 预评编号格式
        /// </summary>
        public string ypno
        {
            get { return _ypno; }
            set { _ypno = value; }
        }

        private int? _appraiserexpireddays;
        public int? appraiserexpireddays
        {
            get { return _appraiserexpireddays; }
            set { _appraiserexpireddays = value; }
        }
        private int? _reportexpireddays;
        public int? reportexpireddays
        {
            get { return _reportexpireddays; }
            set { _reportexpireddays = value; }
        }
        private string _introduction;
        /// <summary>
        /// 公司介绍
        /// </summary>
        public string introduction
        {
            get { return _introduction; }
            set { _introduction = value; }
        }

         private string _imgintruduction;
        /// <summary>
        /// 公司介绍简介附图
        /// </summary>
        public string imgintruduction
        {
            get { return _imgintruduction; }
            set { _imgintruduction = value; }
        }


         private string _introdetails;
        /// <summary>
        /// 公司介绍详情
        /// </summary>
        public string introdetails
        {
            get { return _introdetails; }
            set { _introdetails = value; }
        }

        private string _imgbusinessscope;
        /// <summary>
        /// 业务范围简介附图
        /// </summary>
        public string imgbusinessscope
        {
            get { return _imgbusinessscope; }
            set { _imgbusinessscope = value; }
        }
        private string _businessdetails;
        /// <summary>
        /// 业务范围详细信息
        /// </summary>
        public string businessdetails
        {
            get { return _businessdetails; }
            set { _businessdetails = value; }
        }

        private string _businessscope;
        public string businessscope
        {
            get { return _businessscope; }
            set { _businessscope = value; }
        }
        private string _classiccase;
        public string classiccase
        {
            get { return _classiccase; }
            set { _classiccase = value; }
        }

        private string _imgclassiccase;
        /// <summary>
        /// 经典案例简介附图
        /// </summary>
        public string imgclassiccase
        {
            get { return _imgclassiccase; }
            set { _imgclassiccase = value; }
        }
        private string _classiccasedetails;
        /// <summary>
        /// 经典案例详细信息
        /// </summary>
        public string classiccasedetails
        {
            get { return _classiccasedetails; }
            set { _classiccasedetails = value; }
        }

        private string _appid;
        /// <summary>
        /// 微信接口凭证
        /// </summary>
        public string appid
        {
            get { return _appid; }
            set { _appid = value; }
        }
        private string _appsecret;
        /// <summary>
        /// 微信secret值
        /// </summary>
        public string appsecret
        {
            get { return _appsecret; }
            set { _appsecret = value; }
        }

    }
}