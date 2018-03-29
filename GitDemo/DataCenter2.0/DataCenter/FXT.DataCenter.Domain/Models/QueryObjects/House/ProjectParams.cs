using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models.QueryObjects.House
{
    public class ProjectParams : BaseParams
    {
        public string Type { get; set; }

        //楼盘房号统计条件
        public List<int> bhareaname { get; set; }

        //楼盘图片统计条件
        public List<int> ppareaname { get; set; }
        public string ProjectName { get; set; }
    }
}
