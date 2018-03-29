using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Privi_ExpertUser
    {
        private int _id;
        /// <summary>
        /// 专家库
        /// </summary>
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _cityid;
        /// <summary>
        /// 城市ID
        /// </summary>
        //[SQLField("cityid", EnumDBFieldUsage.PrimaryKey)]
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private int _fxtcompanyid;
        /// <summary>
        /// 专家所在评估机构ID
        /// </summary>
        //[SQLField("fxtcompanyid", EnumDBFieldUsage.PrimaryKey)]
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private int? _departmentid;
        /// <summary>
        /// 专家所在部门ID
        /// </summary>
        public int? departmentid
        {
            get { return _departmentid; }
            set { _departmentid = value; }
        }
        private string _userid;
        /// <summary>
        /// 专家账号ID
        /// </summary>
        //[SQLField("userid", EnumDBFieldUsage.PrimaryKey)]
        public string userid
        {
            get { return _userid; }
            set { _userid = value; }
        }
        private int _experttype = 1019001;
        /// <summary>
        /// 专业类型:房地产、土地、资产
        /// </summary>
        //[SQLField("experttype", EnumDBFieldUsage.PrimaryKey)]
        public int experttype
        {
            get { return _experttype; }
            set { _experttype = value; }
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

    }
}
