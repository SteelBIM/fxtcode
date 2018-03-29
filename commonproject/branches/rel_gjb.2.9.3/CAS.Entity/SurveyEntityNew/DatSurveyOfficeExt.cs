﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.SurveyDBEntity;

namespace CAS.Entity.SurveyEntityNew
{
    public class DatSurveyOfficeExt : DatSurveyOffice
    {
        public int? areaid { get; set; }
        public string areaname { get; set; }
        public string address { get; set; }
        public string names { get; set; }
        public DateTime? begintime { get; set; }
        public DateTime? surveycompletetime { get; set; }
        public string cityname { get; set; }
        public int? provinceid { get; set; }
        public string provincename { get; set; }
        public decimal? buildingarea { get; set; }
        public int? userYear { get; set; }
    }
}