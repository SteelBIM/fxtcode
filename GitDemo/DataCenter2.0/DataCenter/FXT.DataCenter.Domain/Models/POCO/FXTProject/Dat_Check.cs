using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Dat_Check
    {
        private long _id;
        /// <summary>
        /// 数据审核表
        /// </summary>
        public long id
        {
            get { return _id; }
            set { _id = value; }
        }
        private long _allotid;
        /// <summary>
        /// 任务ID
        /// </summary>
        public long allotid
        {
            get { return _allotid; }
            set { _allotid = value; }
        }
        private int _dattype = 1034001;
        /// <summary>
        /// 数据类型：楼栋、楼层、房号
        /// </summary>
        public int dattype
        {
            get { return _dattype; }
            set { _dattype = value; }
        }
        private long _datid;
        /// <summary>
        /// 数据ID
        /// </summary>
        public long datid
        {
            get { return _datid; }
            set { _datid = value; }
        }
        private string _otherid;
        /// <summary>
        /// 评估发展中心ID
        /// </summary>
        public string otherid
        {
            get { return _otherid; }
            set { _otherid = value; }
        }
        private string _checkuserid1;
        /// <summary>
        /// 一审人
        /// </summary>
        public string checkuserid1
        {
            get { return _checkuserid1; }
            set { _checkuserid1 = value; }
        }
        private int? _checkstate1;
        /// <summary>
        /// 一审结果,1:通过，0:不通过
        /// </summary>
        public int? checkstate1
        {
            get { return _checkstate1; }
            set { _checkstate1 = value; }
        }
        private string _checkremark1;
        /// <summary>
        /// 一审备注
        /// </summary>
        public string checkremark1
        {
            get { return _checkremark1; }
            set { _checkremark1 = value; }
        }
        private DateTime? _checkdate1;
        /// <summary>
        /// 一审时间
        /// </summary>
        public DateTime? checkdate1
        {
            get { return _checkdate1; }
            set { _checkdate1 = value; }
        }
        private string _checkuserid2;
        /// <summary>
        /// 二审人
        /// </summary>
        public string checkuserid2
        {
            get { return _checkuserid2; }
            set { _checkuserid2 = value; }
        }
        private int? _checkstate2;
        /// <summary>
        /// 二审结果,1:通过，0:不通过
        /// </summary>
        public int? checkstate2
        {
            get { return _checkstate2; }
            set { _checkstate2 = value; }
        }
        private string _checkremark2;
        /// <summary>
        /// 二审备注
        /// </summary>
        public string checkremark2
        {
            get { return _checkremark2; }
            set { _checkremark2 = value; }
        }
        private DateTime? _checkdate2;
        /// <summary>
        /// 二审时间
        /// </summary>
        public DateTime? checkdate2
        {
            get { return _checkdate2; }
            set { _checkdate2 = value; }
        }

    }
}
