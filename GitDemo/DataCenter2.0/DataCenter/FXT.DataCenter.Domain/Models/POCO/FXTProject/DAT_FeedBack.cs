using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
     public class DAT_FeedBack
    {
        private int _id;
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _userid;
        public string userid
        {
            get { return _userid; }
            set { _userid = value; }
        }
        private int _cityid;
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private int? _typecode;
        /// <summary>
        /// 反馈意见类型
        /// </summary>
        public int? typecode
        {
            get { return _typecode; }
            set { _typecode = value; }
        }
        private int? _feedcode;
        /// <summary>
        /// 反馈原因
        /// </summary>
        public int? feedcode
        {
            get { return _feedcode; }
            set { _feedcode = value; }
        }
        private string _detail;
        /// <summary>
        /// 详细说明
        /// </summary>
        public string detail
        {
            get { return _detail; }
            set { _detail = value; }
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
        private int _isread = 0;
        /// <summary>
        /// 是否已查看
        /// </summary>
        public int isread
        {
            get { return _isread; }
            set { _isread = value; }
        }
        private int _isvalid = 0;
        /// <summary>
        /// 是否处理
        /// </summary>
        public int isvalid
        {
            get { return _isvalid; }
            set { _isvalid = value; }
        }
        private int _ishui = 0;
        /// <summary>
        /// 是否回复
        /// </summary>
        public int ishui
        {
            get { return _ishui; }
            set { _ishui = value; }
        }
        private string _replyinfo;
        public string replyinfo
        {
            get { return _replyinfo; }
            set { _replyinfo = value; }
        }

    }
}
