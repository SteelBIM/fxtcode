using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseActivate.Web.API.Models
{
    public class AppVersionModel
    {
        public Guid? AppID { get; set; }
        public string Version { get; set; }
    }
}