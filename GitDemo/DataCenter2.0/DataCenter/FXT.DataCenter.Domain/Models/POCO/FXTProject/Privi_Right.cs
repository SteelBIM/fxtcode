using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Privi_Right
    {
        private int _rightid;
        /// <summary>
        /// 用户权限
        /// </summary>
        //[SQLField("rightid", EnumDBFieldUsage.PrimaryKey, true)]
        public int rightid
        {
            get { return _rightid; }
            set { _rightid = value; }
        }
        private string _rightname;
        /// <summary>
        /// 权限名称
        /// </summary>
        public string rightname
        {
            get { return _rightname; }
            set { _rightname = value; }
        }
        private int _parentid;
        /// <summary>
        /// 父权限
        /// </summary>
        public int parentid
        {
            get { return _parentid; }
            set { _parentid = value; }
        }
        private int _systypecode = 0;
        public int systypecode
        {
            get { return _systypecode; }
            set { _systypecode = value; }
        }
        private int _rightcode;
        /// <summary>
        /// 用户权限代码
        /// </summary>
        public int rightcode
        {
            get { return _rightcode; }
            set { _rightcode = value; }
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

    }
}
