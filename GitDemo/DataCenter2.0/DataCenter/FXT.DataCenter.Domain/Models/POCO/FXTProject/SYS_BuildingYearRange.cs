using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class SYS_BuildingYearRange
    {
        private int _fk_code = 0;
        /// <summary>
        /// FROM dbo.SYS_Code WHERE ID = 8004
        /// </summary>
        //[SQLField("fk_code", EnumDBFieldUsage.PrimaryKey)]
        public int fk_code
        {
            get { return _fk_code; }
            set { _fk_code = value; }
        }
        private int _yearfrom = 0;
        public int yearfrom
        {
            get { return _yearfrom; }
            set { _yearfrom = value; }
        }
        private int _yearto = 0;
        public int yearto
        {
            get { return _yearto; }
            set { _yearto = value; }
        }

    }
}
