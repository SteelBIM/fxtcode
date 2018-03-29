using System.Collections.Generic;
using CAS.Entity.SurveyDBEntity;

namespace CAS.Entity.SurveyEntityNew
{
    /// <summary>
    /// 查勘库扩展类 caoq 2013-11-11
    /// </summary>
    public class CacheSurvey
    {
        //cache key
        public string Key { get; set; }
        //查勘信息
        public DatSurvey Survey { get; set; }
        public DatSurveyHouse SurveyHouse { get; set; }
        public DatSurveyOffice SurveyOffice { get; set; }
        public DatSurveyBusiness SurveyBusiness { get; set; }
        public DatSurveyFactory SurveyFactory { get; set; }
        public DatSurveyLand SurveyLand { get; set; }
        //上传文件
        public List<DatFiles> Files { get; set; }
        //更新flow
        public DatFollowUp Flow { get; set; }
        //日志
        public SYSSurveyUploadLog Log { get; set; }
        //excel
        public Dictionary<string, string> ExcelTable { get; set; }
        //业务员
        public string workeruser { get; set; }
        //查勘员
        public string surveyuser { get; set; }
    }
}
