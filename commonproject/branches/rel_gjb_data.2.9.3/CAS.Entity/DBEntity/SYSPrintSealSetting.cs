//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using CAS.Entity.BaseDAModels;

//namespace CAS.Entity.DBEntity
//{
//    [Serializable]
//    [Table("dbo.SYS_PrintRealSetting")]
//    public class SYSPrintRealSetting : BaseTO
//    {
//        private int _id;
//        public int id
//        {
//            get { return _id; }
//            set { _id = value; }
//        }
//        private string _branchid;
//        /// <summary>
//        /// 分支机构
//        /// </summary>
//        public string branchid
//        {
//            get { return _branchid; }
//            set { _branchid = value; }
//        }
//        private string _userid;
//        /// <summary>
//        /// 打印人或者盖章人
//        /// </summary>
//        public string userid
//        {
//            get { return _userid; }
//            set { _userid = value; }
//        }
//        private int? _createuserid;
//        /// <summary>
//        /// 创建人
//        /// </summary>
//        public int? createuserid
//        {
//            get { return _createuserid; }
//            set { _createuserid = value; }
//        }
//        private DateTime? _createdate;
//        /// <summary>
//        /// 创建时间
//        /// </summary>
//        public DateTime? createdate
//        {
//            get { return _createdate; }
//            set { _createdate = value; }
//        }
//        private bool? _isprint;
//        /// <summary>
//        /// 属于打印还是盖章
//        /// </summary>
//        public bool? isprint
//        {
//            get { return _isprint; }
//            set { _isprint = value; }
//        }
//        private bool? _valid;
//        public bool? valid
//        {
//            get { return _valid; }
//            set { _valid = value; }
//        }
//    }

//}
