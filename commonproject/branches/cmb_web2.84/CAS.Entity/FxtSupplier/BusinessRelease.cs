using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.FxtSupplier
{
    [Serializable]
    [TableAttribute("dbo.BusinessRelease")]
    public class BusinessRelease : BaseTO
    {
        private int _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _acceptbusinessid;
        /// <summary>
        /// 业务受理ID
        /// </summary>
        public int acceptbusinessid
        {
            get { return _acceptbusinessid; }
            set { _acceptbusinessid = value; }
        }
        private int _companyid;
        /// <summary>
        /// 供应商ID
        /// </summary>
        public int companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        private int _cityid;
        /// <summary>
        /// 城市ID
        /// </summary>
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private string _title;
        /// <summary>
        /// 标题
        /// </summary>
        public string title
        {
            get { return _title; }
            set { _title = value; }
        }
        private string _releasecontent;
        public string releasecontent
        {
            get { return _releasecontent; }
            set { _releasecontent = value; }
        }
        private string _tips;
        public string tips
        {
            get { return _tips; }
            set { _tips = value; }
        }
        private string _author;
        public string author
        {
            get { return _author; }
            set { _author = value; }
        }
        private DateTime _receiveddate;
        /// <summary>
        /// 投稿时间
        /// </summary>
        public DateTime receiveddate
        {
            get { return _receiveddate; }
            set { _receiveddate = value; }
        }
        private int _releasetype;
        /// <summary>
        /// 发布内容的类型
        /// </summary>
        public int releasetype
        {
            get { return _releasetype; }
            set { _releasetype = value; }
        }
    }

}
