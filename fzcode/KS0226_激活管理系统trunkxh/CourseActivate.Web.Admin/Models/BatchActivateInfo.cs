using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseActivate.Web.Admin.Models
{
    public class BatchActivateInfo
    {
        public int activateuseid { get; set; }
        public int activateid { get; set; }
        public string activatecode { get; set; }
        public string userid { get; set; }
        public string username { get; set; }
        public DateTime createtime { get; set; }
        public int bookid { get; set; }
        public int batchid { get; set; }
        public string batchcode { get; set; }
        public int activatetypeid { get; set; }
        public string activatetypename { get; set; }
        public int publishid { get; set; }
        public int usenum { get; set; }
        public string BookName { get; set; }
        public int TotalNum { get; set; }
        public string publishname { get; set; }
    }
}