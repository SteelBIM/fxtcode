using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseActivate.Web.Admin.Models
{
    /// <summary>
    /// 操作权限
    /// </summary>
    public class Jurisdiction
    {
        private bool _view = false;
        public bool View
        {
            get { return _view; }
            set { _view = value; }
        }
        private bool _add = false;
        public bool Add
        {
            get { return _add; }
            set { _add = value; }
        }
        private bool _edit = false;
        public bool Edit
        {
            get { return _edit; }
            set { _edit = value; }
        }
        private bool _del = false;
        public bool Del
        {
            get { return _del; }
            set { _del = value; }
        }
        private bool _export = false;
        /// <summary>
        /// 导出
        /// </summary>
        public bool Export
        {
            get { return _export; }
            set { _export = value; }
        }
        private bool _pullblack = false;
        /// <summary>
        /// 拉黑
        /// </summary>
        public bool Pullblack
        {
            get { return _pullblack; }
            set { _pullblack = value; }
        }
        private bool _locking = false;
        /// <summary>
        /// 锁定
        /// </summary>
        public bool Locking
        {
            get { return _locking; }
            set { _locking = value; }
        }
        private bool _move = false;
        /// <summary>
        /// 移动
        /// </summary>
        public bool Move
        {
            get { return _move; }
            set { _move = value; }
        }
        private bool _detailed = false;
        /// <summary>
        /// 详细
        /// </summary>
        public bool Detailed
        {
            get { return _detailed; }
            set { _detailed = value; }
        }
        private bool _blacklist = false;
        /// <summary>
        /// 查看黑名单
        /// </summary>
        public bool Blacklist
        {
            get { return _blacklist; }
            set { _blacklist = value; }
        }
        private bool _kont = false;
        /// <summary>
        /// 结算
        /// </summary>
        public bool Kont
        {
            get { return _kont; }
            set { _kont = value; }
        }
        private bool _revoked = false;
        /// <summary>
        /// 结算撤销
        /// </summary>
         public bool Revoked
        {
            get { return _revoked; }
            set { _revoked = value; }
        }
         private bool _employee = false;
        
         public bool Employee
         {
             get { return _employee; }
             set { _employee = value; }
         }
         private bool _dept = false;
        
         public bool Dept
         {
             get { return _dept; }
             set { _dept = value; }
         }
         public bool _agent = false;
       
         public bool Agent
         {
             get { return _agent; }
             set { _agent = value; }
         }

        
    }
}