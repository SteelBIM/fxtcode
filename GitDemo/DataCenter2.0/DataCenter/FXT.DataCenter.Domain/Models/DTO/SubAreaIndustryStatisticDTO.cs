using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models.DTO
{
    public class SubAreaIndustryStatisticDTO
    {
        public string AreaName { get; set; }
        public string SubAreaName { get; set; }
        public int ProjectCount { get; set; }
        public int BuildingCount { get; set; }
    }
}
