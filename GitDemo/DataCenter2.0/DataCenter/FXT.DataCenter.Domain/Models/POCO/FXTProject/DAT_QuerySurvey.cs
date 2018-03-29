using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class DAT_QuerySurvey
    {
        private int _id;
        
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _fk_qid;
        public int fk_qid
        {
            get { return _fk_qid; }
            set { _fk_qid = value; }
        }
        private string _userid;
        public string userid
        {
            get { return _userid; }
            set { _userid = value; }
        }
        private DateTime _surveydate = DateTime.Now;
        public DateTime surveydate
        {
            get { return _surveydate; }
            set { _surveydate = value; }
        }
        private string _remark;
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        private int _cityid;
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }

    }
}
