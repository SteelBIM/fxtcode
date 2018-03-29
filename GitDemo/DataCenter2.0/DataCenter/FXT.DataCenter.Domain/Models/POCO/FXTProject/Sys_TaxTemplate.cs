using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Sys_TaxTemplate
    {
        private int _id;
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _cityid;
        //[SQLField("cityid", EnumDBFieldUsage.PrimaryKey)]
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private int _fxtcompanyid;
        //[SQLField("fxtcompanyid", EnumDBFieldUsage.PrimaryKey)]
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private string _templatename;
        /// <summary>
        /// 模板名称
        /// </summary>
        public string templatename
        {
            get { return _templatename; }
            set { _templatename = value; }
        }
        private int _purposecode;
        /// <summary>
        /// 用途
        /// </summary>
        //[SQLField("purposecode", EnumDBFieldUsage.PrimaryKey)]
        public int purposecode
        {
            get { return _purposecode; }
            set { _purposecode = value; }
        }
        private int _ownercode;
        /// <summary>
        /// 产权
        /// </summary>
       // [SQLField("ownercode", EnumDBFieldUsage.PrimaryKey)]
        public int ownercode
        {
            get { return _ownercode; }
            set { _ownercode = value; }
        }
        private int _overyearcode;
        /// <summary>
        /// 是否满五年
        /// </summary>
        //[SQLField("overyearcode", EnumDBFieldUsage.PrimaryKey)]
        public int overyearcode
        {
            get { return _overyearcode; }
            set { _overyearcode = value; }
        }
        private int _firstcode;
        /// <summary>
        /// 是否首次购房
        /// </summary>
        //[SQLField("firstcode", EnumDBFieldUsage.PrimaryKey)]
        public int firstcode
        {
            get { return _firstcode; }
            set { _firstcode = value; }
        }
        private int _onlycode;
        /// <summary>
        /// 是否家庭唯一生活用房
        /// </summary>
        //[SQLField("onlycode", EnumDBFieldUsage.PrimaryKey)]
        public int onlycode
        {
            get { return _onlycode; }
            set { _onlycode = value; }
        }
        private int _areacode;
        /// <summary>
        /// 面积分段（60,90,144）
        /// </summary>
        //[SQLField("areacode", EnumDBFieldUsage.PrimaryKey)]
        public int areacode
        {
            get { return _areacode; }
            set { _areacode = value; }
        }
        private DateTime _templatedate;
        /// <summary>
        /// 税费公式生效日期
        /// </summary>
       // [SQLField("templatedate", EnumDBFieldUsage.PrimaryKey)]
        public DateTime templatedate
        {
            get { return _templatedate; }
            set { _templatedate = value; }
        }
        private int _companyid = 0;
        /// <summary>
        /// 客户单位ID
        /// </summary>
       // [SQLField("companyid", EnumDBFieldUsage.PrimaryKey)]
        public int companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        private string _d;
        /// <summary>
        /// 营业税公式。A原购价，B评估总值，C建筑面积
        /// </summary>
        public string d
        {
            get { return _d; }
            set { _d = value; }
        }
        private string _e;
        /// <summary>
        /// 城建税公式
        /// </summary>
        public string e
        {
            get { return _e; }
            set { _e = value; }
        }
        private string _f;
        /// <summary>
        /// 教育附加税公式
        /// </summary>
        public string f
        {
            get { return _f; }
            set { _f = value; }
        }
        private string _g;
        /// <summary>
        /// 印花税公式
        /// </summary>
        public string g
        {
            get { return _g; }
            set { _g = value; }
        }
        private string _h;
        /// <summary>
        /// 契税公式
        /// </summary>
        public string h
        {
            get { return _h; }
            set { _h = value; }
        }
        private string _i;
        /// <summary>
        /// 处置费用公式
        /// </summary>
        public string i
        {
            get { return _i; }
            set { _i = value; }
        }
        private string _j;
        /// <summary>
        /// 交易手续费公式
        /// </summary>
        public string j
        {
            get { return _j; }
            set { _j = value; }
        }
        private string _k;
        /// <summary>
        /// 土地增值税公式
        /// </summary>
        public string k
        {
            get { return _k; }
            set { _k = value; }
        }
        private string _l;
        /// <summary>
        /// 所得税公式
        /// </summary>
        public string l
        {
            get { return _l; }
            set { _l = value; }
        }
        private string _totaltax;
        /// <summary>
        /// 税费总额公式
        /// </summary>
        public string totaltax
        {
            get { return _totaltax; }
            set { _totaltax = value; }
        }
        private DateTime _createtime = DateTime.Now;
        public DateTime createtime
        {
            get { return _createtime; }
            set { _createtime = value; }
        }
        private string _createuser;
        public string createuser
        {
            get { return _createuser; }
            set { _createuser = value; }
        }
        private DateTime _savetime = DateTime.Now;
        public DateTime savetime
        {
            get { return _savetime; }
            set { _savetime = value; }
        }
        private string _saveuser;
        public string saveuser
        {
            get { return _saveuser; }
            set { _saveuser = value; }
        }
        private int _valid = 1;
        public int valid
        {
            get { return _valid; }
            set { _valid = value; }
        }

    }
}
