using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class SYS_PropertyAreaRange
    {
        private int _fk_code = 0;
        /// <summary>
        /// FROM dbo.SYS_Code WHERE ID = 8003
        /// </summary>
        //[SQLField("fk_code", EnumDBFieldUsage.PrimaryKey)]
        public int fk_code
        {
            get { return _fk_code; }
            set { _fk_code = value; }
        }
        private int _areafrom = 0;
        public int areafrom
        {
            get { return _areafrom; }
            set { _areafrom = value; }
        }
        private int _areato = 0;
        public int areato
        {
            get { return _areato; }
            set { _areato = value; }
        }

    }
}
