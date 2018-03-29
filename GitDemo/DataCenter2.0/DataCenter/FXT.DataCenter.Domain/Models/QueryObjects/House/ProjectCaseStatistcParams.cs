using System;
using System.Collections.Generic;

namespace FXT.DataCenter.Domain.Models.QueryObjects.House
{
    public class ProjectCaseStatistcParams : BaseParams
    {
        public string Type { get; set; }

        //工作量统计条件
        public string CaseUpLoadtimeFrom { get; set; }
        public string CaseUpLoadtimeTo { get; set; }

        //案例统计条件
        public string CaseDateFrom { get; set; }
        public string CaseDateTo { get; set; }
        public int CaseTypeCode { get; set; }
        public string Condition { get; set; }
        public int? Amount { get; set; }

        //楼盘均价统计条件
        public DateTime? TimeFrom { get; set; }
        public DateTime? TimeTo { get; set; }
        public string Groupcycle { get; set; }
        public string Grouparea { get; set; }
        public List<int> Buildingtypecode { get; set; }
        public List<int> Purposecode { get; set; }
        public List<int> Buildingdatecode { get; set; }
        public string Sampleproject { get; set; }
        public int Cityid { get; set; }
        public int Fxtcompanyid { get; set; }

        //楼盘可估统计
        public string ProjectEValueTimeFrom { get; set; }
        public string ProjectEValueTimeTo { get; set; }
        public List<int> peareaname { get; set; }
        public string ProjectEValueProjectName { get; set; }
        public string ProjectUEReason { get; set; }

        //楼栋可估统计
        public string BuildingEValueTimeFrom { get; set; }
        public string BuildingEValueTimeTo { get; set; }
        public List<int> bpeareaname { get; set; }
        public string BuildingEValueProjectName { get; set; }
        public string BuildingUEReason { get; set; }
    }
}
