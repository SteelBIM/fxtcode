using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class SYS_SurveyImageTemplate
    {
        private int _id;
        /// <summary>
        /// 查勘照片排版模板
        /// </summary>
        //[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
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
        private int _surveytype = 6000001;
        /// <summary>
        /// 查勘类型
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
        private string _creator;
        public string creator
        {
            get { return _creator; }
            set { _creator = value; }
        }

    }
}
