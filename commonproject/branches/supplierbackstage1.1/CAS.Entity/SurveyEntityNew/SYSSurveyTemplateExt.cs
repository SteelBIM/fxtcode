using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.SurveyDBEntity;

namespace CAS.Entity.SurveyEntityNew
{
    /// <summary>
    /// 查勘库查勘模板扩展类
    /// </summary>
    public class SYSSurveyTemplateExt : SYSBusinessTableSetup
    {
        public string surveytypecodename { get; set; }

        public string createtruename { get; set; }
    }
}
