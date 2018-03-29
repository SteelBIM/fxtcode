using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FXT.DataCenter.Domain.Models.DTO;
using Webdiyer.WebControls.Mvc;

namespace FXT.DataCenter.WebUI.Areas.House.Models
{
    public class ProjectStatics
    {
        public PagedList<Project_BHCount> BHCount { get; set; }

        public PagedList<Project_PPCount> PPCount { get; set; }
    }
}
