using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class LNK_P_Company
    {

        /// <summary>
        /// ProjectId
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// CompanyId
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// CompanyType
        /// </summary>
        public int CompanyType { get; set; }
        /// <summary>
        /// CityId
        /// </summary>
        public int CityId { get; set; }

    }
}
