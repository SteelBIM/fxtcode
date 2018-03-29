using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Dat_SysMessage
    {
        private int _id;
        //[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _cityid = -1;
        /// <summary>
        /// 城市ID
        /// </summary>
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private int _fxtcompanyid = -1;
        /// <summary>
        /// 评估机构ID
        /// </summary>
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private int _companyid = -1;
        /// <summary>
        /// 客户公司ID
        /// </summary>
        public int companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        private int _departmentid = -1;
        /// <summary>
        /// 部门ID
        /// </summary>
        public int departmentid
        {
            get { return _departmentid; }
            set { _departmentid = value; }
        }
        private string _touserid = "";
        /// <summary>
        /// 接收人
        /// </summary>
        public string touserid
        {
            get { return _touserid; }
            set { _touserid = value; }
        }
        private int _togroupid = -1;
        /// <summary>
        /// 接收组
        /// </summary>
        public int togroupid
        {
            get { return _togroupid; }
            set { _togroupid = value; }
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
        private string _message;
        /// <summary>
        /// 内容
        /// </summary>
        public string message
        {
            get { return _message; }
            set { _message = value; }
        }
        private string _remark;
        /// <summary>
        /// 备注
        /// </summary>
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        private int _systypecode = -1;
        /// <summary>
        /// 系统类型
        /// </summary>
        public int systypecode
        {
            get { return _systypecode; }
            set { _systypecode = value; }
        }
        private int _messagetype = 7004003;
        /// <summary>
        /// 消息类型
        /// </summary>
        public int messagetype
        {
            get { return _messagetype; }
            set { _messagetype = value; }
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
        private DateTime _overdate = DateTime.Now;
        /// <summary>
        /// 有效期
        /// </summary>
        public DateTime overdate
        {
            get { return _overdate; }
            set { _overdate = value; }
        }
        private int _valid = 1;
        /// <summary>
        /// 是否有效
        /// </summary>
        public int valid
        {
            get { return _valid; }
            set { _valid = value; }
        }

    }
}
