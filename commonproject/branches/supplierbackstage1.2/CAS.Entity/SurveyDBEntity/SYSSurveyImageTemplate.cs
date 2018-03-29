using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.SurveyDBEntity
{
    /// <summary>
    /// 查勘库照片模版表 caoq 2013-11-11
    /// </summary>
    [Serializable]
    [TableAttribute("dbo.SYS_SurveyImageTemplate")]
    public class SYSSurveyImageTemplate : BaseTO
    {
        private int _id;
        /// <summary>
        /// 查勘照片排版模板
        /// </summary>
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
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
        private int _fxtcompanyid;
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private int? _subcompanyid;
        public int? subcompanyid
        {
            get { return _subcompanyid; }
            set { _subcompanyid = value; }
        }
        private int _surveytype = 6040001;
        /// <summary>
        /// 查勘类型1031
        /// </summary>
        public int surveytype
        {
            get { return _surveytype; }
            set { _surveytype = value; }
        }
        private string _name;
        /// <summary>
        /// 模板名称
        /// </summary>
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }
        private int? _pagecount;
        /// <summary>
        /// 模板页数
        /// </summary>
        public int? pagecount
        {
            get { return _pagecount; }
            set { _pagecount = value; }
        }
        private string _pagecontent;
        /// <summary>
        /// 每页模板排版内容，包括照片位置等
        /// </summary>
        public string pagecontent
        {
            get { return _pagecontent; }
            set { _pagecontent = value; }
        }
        private DateTime _createtime = DateTime.Now;
        public DateTime createtime
        {
            get { return _createtime; }
            set { _createtime = value; }
        }
        private string _createuserid;
        public string createuserid
        {
            get { return _createuserid; }
            set { _createuserid = value; }
        }
        private bool _valid = true;
        public bool valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        /// <summary>
        /// 创建人名字
        /// </summary>
        private string _createusername;
        public string createusername {
            get { return _createusername; }
            set { _createusername = value; }
        }
    }
}