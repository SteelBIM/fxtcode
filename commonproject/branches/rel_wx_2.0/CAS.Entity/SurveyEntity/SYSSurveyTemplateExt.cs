using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
namespace CAS.Entity.SurveyEntity
{
    /// <summary>
    /// 查勘模板扩展类 kevin 2013-4-4
    /// </summary>
    public class SYSSurveyTemplateExt:SYSBusinessTableSetup
    {
        public string surveytypecodename { get; set; }

        public string createtruename { get; set; }
    }
}
