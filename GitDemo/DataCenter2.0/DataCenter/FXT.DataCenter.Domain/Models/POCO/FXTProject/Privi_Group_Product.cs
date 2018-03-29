using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Privi_Group_Product
    {
        private int _groupid = 0;
        /// <summary>
        /// 用户组ID
        /// </summary>
        public int groupid
        {
            get { return _groupid; }
            set { _groupid = value; }
        }
        private int _systypecode = 1003011;
        /// <summary>
        /// 产品类型
        /// </summary>
        public int systypecode
        {
            get { return _systypecode; }
            set { _systypecode = value; }
        }

    }
}
