using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Privi_Config
    {
        private int _configid;
        /// <summary>
        /// 权限组ID
        /// </summary>
        //[SQLField("configid", EnumDBFieldUsage.PrimaryKey, true)]
        public int configid
        {
            get { return _configid; }
            set { _configid = value; }
        }
        private int _privicode;
        /// <summary>
        /// 权限CODE
        /// </summary>
        public int privicode
        {
            get { return _privicode; }
            set { _privicode = value; }
        }
        private int _functioncode;
        /// <summary>
        /// 功能CODE
        /// </summary>
        public int functioncode
        {
            get { return _functioncode; }
            set { _functioncode = value; }
        }
        private int _syscode = 1003003;
        /// <summary>
        /// 系统类型
        /// </summary>
        public int syscode
        {
            get { return _syscode; }
            set { _syscode = value; }
        }

    }
}
