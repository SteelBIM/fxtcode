using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_WXAloneSet")]
    public class DatWXAloneSet : BaseTO
    {
        private int _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _companyid;
        /// <summary>
        /// 公司id
        /// </summary>
        public int companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        private string _introduction;
        /// <summary>
        /// 公司介绍简介
        /// </summary>
        public string introduction
        {
            get { return _introduction; }
            set { _introduction = value; }
        }
        private string _businessscope;
        /// <summary>
        /// 业务范围简介
        /// </summary>
        public string businessscope
        {
            get { return _businessscope; }
            set { _businessscope = value; }
        }
        private string _classiccase;
        /// <summary>
        /// 经典案例简介
        /// </summary>
        public string classiccase
        {
            get { return _classiccase; }
            set { _classiccase = value; }
        }
        private string _imgintruduction;
        /// <summary>
        /// 公司介绍图片
        /// </summary>
        public string imgintruduction
        {
            get { return _imgintruduction; }
            set { _imgintruduction = value; }
        }
        private string _imgbusinessscope;
        /// <summary>
        /// 业务范围图片
        /// </summary>
        public string imgbusinessscope
        {
            get { return _imgbusinessscope; }
            set { _imgbusinessscope = value; }
        }
        private string _imgclassiccase;
        /// <summary>
        /// 经典案例图片
        /// </summary>
        public string imgclassiccase
        {
            get { return _imgclassiccase; }
            set { _imgclassiccase = value; }
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
        private string _businessdetails;
        /// <summary>
        /// 业务范围详情
        /// </summary>
        public string businessdetails
        {
            get { return _businessdetails; }
            set { _businessdetails = value; }
        }
        private string _classiccasedetails;
        /// <summary>
        /// 经典案例详情
        /// </summary>
        public string classiccasedetails
        {
            get { return _classiccasedetails; }
            set { _classiccasedetails = value; }
        }
        private string _contacts;
        public string contacts
        {
            get { return _contacts; }
            set { _contacts = value; }
        }
        private string _managerwxopenid;
        public string managerwxopenid
        {
            get { return _managerwxopenid; }
            set { _managerwxopenid = value; }
        }
        private string _appid;
        public string appid
        {
            get { return _appid; }
            set { _appid = value; }
        }
        private string _appsecret;
        public string appsecret
        {
            get { return _appsecret; }
            set { _appsecret = value; }
        }
        private string _managertelephone;
        public string managertelephone
        {
            get { return _managertelephone; }
            set { _managertelephone = value; }
        }
        private string _managername;
        public string managername
        {
            get { return _managername; }
            set { _managername = value; }
        }

    }
}