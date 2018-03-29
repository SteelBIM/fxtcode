using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class DAT_ASS_JiangCheng
    {
        private int _jcid;
        //[SQLField("jcid", EnumDBFieldUsage.PrimaryKey, true)]
        public int jcid
        {
            get { return _jcid; }
            set { _jcid = value; }
        }
        private byte _jcobjecttype;
        /// <summary>
        /// 奖惩对象类型（机构/个人）
        /// </summary>
        public byte jcobjecttype
        {
            get { return _jcobjecttype; }
            set { _jcobjecttype = value; }
        }
        private string _jcobjectname;
        /// <summary>
        /// 奖惩对象名称
        /// </summary>
        public string jcobjectname
        {
            get { return _jcobjectname; }
            set { _jcobjectname = value; }
        }
        private string _jcuserid;
        /// <summary>
        /// 奖惩个人ID
        /// </summary>
        public string jcuserid
        {
            get { return _jcuserid; }
            set { _jcuserid = value; }
        }
        private int? _jccompanyid;
        /// <summary>
        /// 奖惩机构ID
        /// </summary>
        public int? jccompanyid
        {
            get { return _jccompanyid; }
            set { _jccompanyid = value; }
        }
        private byte _jctype;
        /// <summary>
        /// 1为奖，2为惩，3为良好记录，4为不良记录
        /// </summary>
        public byte jctype
        {
            get { return _jctype; }
            set { _jctype = value; }
        }
        private string _jccontent;
        /// <summary>
        /// 奖惩内容
        /// </summary>
        public string jccontent
        {
            get { return _jccontent; }
            set { _jccontent = value; }
        }
        private DateTime _jcdate;
        /// <summary>
        /// 奖惩日期
        /// </summary>
        public DateTime jcdate
        {
            get { return _jcdate; }
            set { _jcdate = value; }
        }
        private string _jcowner;
        /// <summary>
        /// 奖惩颁布方
        /// </summary>
        public string jcowner
        {
            get { return _jcowner; }
            set { _jcowner = value; }
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
        /// 创建人
        /// </summary>
        public string createuserid
        {
            get { return _createuserid; }
            set { _createuserid = value; }
        }
        private int _companyid;
        /// <summary>
        /// 协会ID
        /// </summary>
        public int companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }

    }
}
