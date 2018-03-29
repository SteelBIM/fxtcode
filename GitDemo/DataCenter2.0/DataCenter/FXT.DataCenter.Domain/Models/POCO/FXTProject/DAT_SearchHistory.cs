using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class DAT_SearchHistory
    {
        private long _id;
        //[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public long id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _cityid;
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private string _userid;
        public string userid
        {
            get { return _userid; }
            set { _userid = value; }
        }
        private int _projectid;
        public int projectid
        {
            get { return _projectid; }
            set { _projectid = value; }
        }
        private string _projectname;
        public string projectname
        {
            get { return _projectname; }
            set { _projectname = value; }
        }
        private DateTime _querydate = DateTime.Now;
        /// <summary>
        /// 询价时间
        /// </summary>
        public DateTime querydate
        {
            get { return _querydate; }
            set { _querydate = value; }
        }
        private decimal? _unitprice;
        public decimal? unitprice
        {
            get { return _unitprice; }
            set { _unitprice = value; }
        }
        private int _type = 1003001;
        /// <summary>
        /// 数据类型
        /// </summary>
        public int type
        {
            get { return _type; }
            set { _type = value; }
        }
        private DateTime? _casestartdate;
        /// <summary>
        /// 案例开始时间
        /// </summary>
        public DateTime? casestartdate
        {
            get { return _casestartdate; }
            set { _casestartdate = value; }
        }
        private DateTime? _caseenddate;
        /// <summary>
        /// 案例结束时间
        /// </summary>
        public DateTime? caseenddate
        {
            get { return _caseenddate; }
            set { _caseenddate = value; }
        }
        private int? _casenumber;
        /// <summary>
        /// 案例数
        /// </summary>
        public int? casenumber
        {
            get { return _casenumber; }
            set { _casenumber = value; }
        }
        private int? _casemaxprice;
        /// <summary>
        /// 案例最大值
        /// </summary>
        public int? casemaxprice
        {
            get { return _casemaxprice; }
            set { _casemaxprice = value; }
        }
        private int? _caseminprice;
        /// <summary>
        /// 案例最小值
        /// </summary>
        public int? caseminprice
        {
            get { return _caseminprice; }
            set { _caseminprice = value; }
        }
        private decimal? _eprice;
        /// <summary>
        /// 项目均价
        /// </summary>
        public decimal? eprice
        {
            get { return _eprice; }
            set { _eprice = value; }
        }
        private int _caseavgprice = 0;
        /// <summary>
        /// 案例均价
        /// </summary>
        public int caseavgprice
        {
            get { return _caseavgprice; }
            set { _caseavgprice = value; }
        }
        private string _ipaddress;
        public string ipaddress
        {
            get { return _ipaddress; }
            set { _ipaddress = value; }
        }
        private int? _companyid;
        /// <summary>
        /// 客户单位ID
        /// </summary>
        public int? companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        private int _fxtcompanyid;
        /// <summary>
        /// 评估机构ID
        /// </summary>
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }

    }
}
