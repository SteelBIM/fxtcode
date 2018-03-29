using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Privi_Group_Right
    {
        private int _groupid;
        /// <summary>
        /// 用户组权限关系表
        /// </summary>
        public int groupid
        {
            get { return _groupid; }
            set { _groupid = value; }
        }
        private int _rightid;
        public int rightid
        {
            get { return _rightid; }
            set { _rightid = value; }
        }

    }
}
