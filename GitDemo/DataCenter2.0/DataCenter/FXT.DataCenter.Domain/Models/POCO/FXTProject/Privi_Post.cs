using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Privi_Post
    {
        private int _postid;
        //[SQLField("postid", EnumDBFieldUsage.PrimaryKey, true)]
        public int postid
        {
            get { return _postid; }
            set { _postid = value; }
        }
        private string _postname;
        /// <summary>
        /// 职位
        /// </summary>
        public string postname
        {
            get { return _postname; }
            set { _postname = value; }
        }
        private int _companyid;
        /// <summary>
        /// 公司Id
        /// </summary>
        public int companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }

    }
}
