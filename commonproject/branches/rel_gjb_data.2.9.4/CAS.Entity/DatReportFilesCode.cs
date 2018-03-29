using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity
{
    [Serializable]
    [TableAttribute("FxtSOA.dbo.Dat_ReportFiles_Code")]
    public class DatReportFilesCode : BaseTO
    {
        private int _id;
        /// <summary>
        /// 报告外部提取码
        /// </summary>
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
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
        private int _fxtcompanyid;
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private int _fk_rid;
        /// <summary>
        /// 报告ID
        /// </summary>
        public int fk_rid
        {
            get { return _fk_rid; }
            set { _fk_rid = value; }
        }
        private int _fk_rfileid;
        /// <summary>
        /// 报告文件ID
        /// </summary>
        public int fk_rfileid
        {
            get { return _fk_rfileid; }
            set { _fk_rfileid = value; }
        }
        private DateTime _createdate = DateTime.Now;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private string _createuserid;
        /// <summary>
        /// 创建用户
        /// </summary>
        public string createuserid
        {
            get { return _createuserid; }
            set { _createuserid = value; }
        }
        private DateTime? _overdate;
        /// <summary>
        /// 有效期
        /// </summary>
        public DateTime? overdate
        {
            get { return _overdate; }
            set { _overdate = value; }
        }
        private string _getcode;
        /// <summary>
        /// 提前码
        /// </summary>
        public string getcode
        {
            get { return _getcode; }
            set { _getcode = value; }
        }
        private string _weburl;
        /// <summary>
        /// 提前地址
        /// </summary>
        public string weburl
        {
            get { return _weburl; }
            set { _weburl = value; }
        }
        private int _isget = 0;
        /// <summary>
        /// 是否已经提取
        /// </summary>
        public int isget
        {
            get { return _isget; }
            set { _isget = value; }
        }
        private string _webip;
        /// <summary>
        /// 提取客户IP地址
        /// </summary>
        public string webip
        {
            get { return _webip; }
            set { _webip = value; }
        }
    }
}