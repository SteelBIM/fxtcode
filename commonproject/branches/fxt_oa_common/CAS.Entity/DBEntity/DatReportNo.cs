using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity
{
    [Serializable]
    [TableAttribute("dbo.Dat_ReportNo")]
    public class DatReportNo : BaseTO
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
        /// 公司ID
        /// </summary>
        public int companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        private string _dateno;
        /// <summary>
        /// 年月值 比如 201308 或者20130828
        /// </summary>
        public string dateno
        {
            get { return _dateno; }
            set { _dateno = value; }
        }
        private string _randomno;
        /// <summary>
        /// 随即自增号  从1开始  000001 几位数不一定
        /// </summary>
        public string randomno
        {
            get { return _randomno; }
            set { _randomno = value; }
        }
        private int _reporttype;
        /// <summary>
        /// 2018005预评 2018006报告
        /// </summary>
        public int reporttype
        {
            get { return _reporttype; }
            set { _reporttype = value; }
        }
        private string _reportno;
        /// <summary>
        /// 最后生成的报告编号
        /// </summary>
        public string reportno
        {
            get { return _reportno; }
            set { _reportno = value; }
        }

        private string _dynamicstring;
        /// <summary>
        /// 动态部分字符串
        /// </summary>
        public string dynamicstring
        {
            get { return _dynamicstring; }
            set { _dynamicstring = value; }
        }

        private DateTime _createdate;
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private int _createuserid;
        public int createuserid
        {
            get { return _createuserid; }
            set { _createuserid = value; }
        }

        private int _valid=1;
        public int valid
        {
            get { return _valid; }
            set { _valid = value; }
        }

        private string _yeardate;
        /// <summary>
        /// 年份
        /// </summary>
        public string yeardate
        {
            get { return _yeardate; }
            set { _yeardate = value; }
        }

        private string _monthdate;
        /// <summary>
        /// 月份
        /// </summary>
        public string monthdate
        {
            get { return _monthdate; }
            set { _monthdate = value; }
        }
        private string _daydate;
        /// <summary>
        /// 日
        /// </summary>
        public string daydate
        {
            get { return _daydate; }
            set { _daydate = value; }
        }
    }
}