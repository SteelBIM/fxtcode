using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Sys_QueryYPTemplet
    {
        private int _id;
        /// <summary>
        /// 预估单模板表
        /// </summary>
        //[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
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
        private string _tname;
        /// <summary>
        /// 模板名称
        /// </summary>
        public string tname
        {
            get { return _tname; }
            set { _tname = value; }
        }
        private int _ismore = 0;
        /// <summary>
        /// 是否多套
        /// </summary>
        public int ismore
        {
            get { return _ismore; }
            set { _ismore = value; }
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
        private int _companyid = 0;
        /// <summary>
        /// 客户ID
        /// </summary>
        public int companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        private int _qtype = 1016001;
        /// <summary>
        /// 业务类型
        /// </summary>
        public int qtype
        {
            get { return _qtype; }
            set { _qtype = value; }
        }
        private int _valid = 0;
        public int valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        private string _path;
        /// <summary>
        /// 模板路径
        /// </summary>
        public string path
        {
            get { return _path; }
            set { _path = value; }
        }
        private string _columns;
        /// <summary>
        /// 委估对象列，各列之间用“,”隔开
        /// </summary>
        public string columns
        {
            get { return _columns; }
            set { _columns = value; }
        }

    }
}
