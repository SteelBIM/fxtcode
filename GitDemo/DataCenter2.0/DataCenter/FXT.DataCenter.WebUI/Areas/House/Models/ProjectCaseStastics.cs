using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FXT.DataCenter.Domain.Models.DTO;
using Webdiyer.WebControls.Mvc;

namespace FXT.DataCenter.WebUI.Areas.House.Models
{
    public class ProjectCaseStastics
    {
        public PagedList<ProjectCase_WorkLoad> WorkLoad { get; set; }

        public PagedList<ProjectCase_Statist> Statist { get; set; }

        public PagedList<ProjectCase_AvePrice> AvePrice { get; set; }

        public PagedList<ProjectCase_ProjectEValue> ProjectEValue { get; set; }

        public PagedList<ProjectCase_BuildingEValue> BuildingEValue { get; set; }
    }
}