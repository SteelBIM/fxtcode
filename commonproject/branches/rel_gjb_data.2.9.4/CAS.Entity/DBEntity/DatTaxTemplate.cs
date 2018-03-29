using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_TaxTemplate")]
    public class DatTaxTemplate : BaseTO
    {
        private int _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _name;
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }
        private int _cityid;
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private int _purpose;
        public int purpose
        {
            get { return _purpose; }
            set { _purpose = value; }
        }
        private int _ownertypecode;
        public int ownertypecode
        {
            get { return _ownertypecode; }
            set { _ownertypecode = value; }
        }
        private int _expiryfiveyear;
        public int expiryfiveyear
        {
            get { return _expiryfiveyear; }
            set { _expiryfiveyear = value; }
        }
        private int _onlylivingroom;
        public int onlylivingroom
        {
            get { return _onlylivingroom; }
            set { _onlylivingroom = value; }
        }
        private int _firstbuye;
        public int firstbuye
        {
            get { return _firstbuye; }
            set { _firstbuye = value; }
        }
        private int _areasegment;
        public int areasegment
        {
            get { return _areasegment; }
            set { _areasegment = value; }
        }
        private string _yingyeshui;
        public string yingyeshui
        {
            get { return _yingyeshui; }
            set { _yingyeshui = value; }
        }
        private string _chengjianshui;
        public string chengjianshui
        {
            get { return _chengjianshui; }
            set { _chengjianshui = value; }
        }
        private string _jiaoyufujiafei;
        public string jiaoyufujiafei
        {
            get { return _jiaoyufujiafei; }
            set { _jiaoyufujiafei = value; }
        }
        private string _yinhuashui;
        public string yinhuashui
        {
            get { return _yinhuashui; }
            set { _yinhuashui = value; }
        }
        private string _qishui;
        public string qishui
        {
            get { return _qishui; }
            set { _qishui = value; }
        }
        private string _jiaoyishouxufei;
        public string jiaoyishouxufei
        {
            get { return _jiaoyishouxufei; }
            set { _jiaoyishouxufei = value; }
        }
        private string _tudizengzhishui;
        public string tudizengzhishui
        {
            get { return _tudizengzhishui; }
            set { _tudizengzhishui = value; }
        }
        private string _chuzhifei;
        public string chuzhifei
        {
            get { return _chuzhifei; }
            set { _chuzhifei = value; }
        }
        private string _suodeshui;
        public string suodeshui
        {
            get { return _suodeshui; }
            set { _suodeshui = value; }
        }
        private string _n;
        /// <summary>
        /// 扣除项目金额
        /// </summary>
        public string n
        {
            get { return _n; }
            set { _n = value; }
        }
        private string _o;
        /// <summary>
        /// 增值额
        /// </summary>
        public string o
        {
            get { return _o; }
            set { _o = value; }
        }
        private string _p;
        /// <summary>
        /// 改良投资
        /// </summary>
        public string p
        {
            get { return _p; }
            set { _p = value; }
        }
        private string _q;
        /// <summary>
        /// 应补地价
        /// </summary>
        public string q
        {
            get { return _q; }
            set { _q = value; }
        }
        private string _r;
        /// <summary>
        /// 案件受理费
        /// </summary>
        public string r
        {
            get { return _r; }
            set { _r = value; }
        }
        private string _s;
        /// <summary>
        /// 拍卖费
        /// </summary>
        public string s
        {
            get { return _s; }
            set { _s = value; }
        }
        private string _t;
        /// <summary>
        /// 诉讼费
        /// </summary>
        public string t
        {
            get { return _t; }
            set { _t = value; }
        }
        private string _u;
        /// <summary>
        /// 申请执行费
        /// </summary>
        public string u
        {
            get { return _u; }
            set { _u = value; }
        }
        private string _v;
        /// <summary>
        /// 地方教育费附加
        /// </summary>
        public string v
        {
            get { return _v; }
            set { _v = value; }
        }
        private string _shuifeizonge;
        public string shuifeizonge
        {
            get { return _shuifeizonge; }
            set { _shuifeizonge = value; }
        }
        private string _biztypes;
        public string biztypes
        {
            get { return _biztypes; }
            set { _biztypes = value; }
        }
        private string _biztexttypes;
        public string biztexttypes
        {
            get { return _biztexttypes; }
            set { _biztexttypes = value; }
        }
        private string _branchcompanyids;
        public string branchcompanyids
        {
            get { return _branchcompanyids; }
            set { _branchcompanyids = value; }
        }
        private string _branchnames;
        public string branchnames
        {
            get { return _branchnames; }
            set { _branchnames = value; }
        }

        private DateTime _createdon;
        public DateTime createdon
        {
            get { return _createdon; }
            set { _createdon = value; }
        }
        private bool _valid;
        public bool valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
    }

}
