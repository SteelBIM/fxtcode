using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Sys_CompanyShowFile
    {
        private int _id;
        /// <summary>
        /// 业务附件显示设置
        /// </summary>
        //[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _systypecode = 1003001;
        /// <summary>
        /// 系统类型
        /// </summary>
        public int systypecode
        {
            get { return _systypecode; }
            set { _systypecode = value; }
        }
        private int _companytypecode;
        /// <summary>
        /// 公司类型
        /// </summary>
        public int companytypecode
        {
            get { return _companytypecode; }
            set { _companytypecode = value; }
        }
        private int _filetypecode;
        /// <summary>
        /// 文件类型
        /// </summary>
        public int filetypecode
        {
            get { return _filetypecode; }
            set { _filetypecode = value; }
        }
        private int _isshow = 1;
        /// <summary>
        /// 附件是否显示
        /// </summary>
        public int isshow
        {
            get { return _isshow; }
            set { _isshow = value; }
        }

    }
}
