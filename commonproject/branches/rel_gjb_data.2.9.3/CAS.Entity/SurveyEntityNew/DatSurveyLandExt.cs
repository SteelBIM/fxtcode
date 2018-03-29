using System;
using CAS.Entity.SurveyDBEntity;

namespace CAS.Entity.SurveyEntityNew
{
    public class DatSurveyLandExt : DatSurveyLand
    {
        public int? areaid { get; set; }
        public string areaname{get;set;}
        public string address{get;set;}
        public string names{get;set;}
        public DateTime? surveybegintime{get;set;}
        public DateTime? completetime { get; set; }
        public string cityname{get;set;}
        public int? provinceid{get;set;}
        public string provincename{get;set;}
        public decimal? buildingarea{get;set;}
    }
}
