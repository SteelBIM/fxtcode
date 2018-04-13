using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Account.Constract.Models
{
    public class com_actioncolumn
    {
        public int columnid
        {
            get;
            set;
        }

        public int parentid
        {
            get;
            set;
        }

        public string columnname
        {
            get;
            set;
        }
        public string actionurl
        {
            get;
            set;
        }
        /// <summary>
        /// 序列号
        /// </summary>
        public int sequence
        {
            get;
            set;
        }

        public string columnico
        {
            get;
            set;
        }
    }
}
