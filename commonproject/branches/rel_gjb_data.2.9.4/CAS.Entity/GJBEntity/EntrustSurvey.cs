using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    /// <summary>
    /// 业务查勘对象
    /// yinpc
    /// </summary>
    public class EntrustSurvey:BaseTO
    {
        private long _entrustid;
        private int _entrustsurveystate;
        private string _surveyuserid;
        private string _surveyuser;
        private DateTime? _surveybegindate;
        private DateTime? _surveyenddate;

        public int entrustsurveystate
        {
            set { _entrustsurveystate = value; }
            get { return _entrustsurveystate; }
        }
        public string surveyuserid
        {
            set { _surveyuserid = value; }
            get { return _surveyuserid; }
        }
        public string surveyuser
        {
            set { _surveyuser = value; }
            get { return _surveyuser; }
        }
        public DateTime? surveybegindate
        {
            set { _surveybegindate = value; }
            get { return _surveybegindate; }
        }
        public DateTime? surveyenddate
        {
            set { _surveyenddate = value; }
            get { return _surveyenddate; }
        }
        public long entrustid
        {
            get { return _entrustid; }
            set { _entrustid = value; }
        }
    }
}
