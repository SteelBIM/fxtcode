using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CourseActivate.Web.Admin.Models
{
    public class ComMasterInfo
    {
        
        public string truename { get; set; }

        [RegularExpression(@"^[1]+[3,5,7,8]+\d{9}", ErrorMessage = "手机号不正确")]
        public string mobile { get; set; }


        [EmailAddress(ErrorMessage = "邮箱不正确")]
        public string email { get; set; }

        [RegularExpression(@"^\d{6,15}", ErrorMessage = "QQ号不正确")]
        public string qq { get; set; }


        public int masterid { get; set; }
        public int parentid { get; set; }
        public string parentname { get; set; }
        public string mastername { get; set; }
        public string password { get; set; }
        public int deptid { get; set; }
        public int groupid { get; set; }
        public int channel { get; set; }

        public int mastertype { get; set; }
        public int state { get; set; }
        public string agentname { get; set; }
        public DateTime agent_startdate { get; set; }
        public DateTime agent_enddate { get; set; }

        [RegularExpression(@"^0\d{2,3}-?\d{7,8}", ErrorMessage = "电话不正确")]
        public string agent_tel { get; set; }

        [RegularExpression(@"^\d{6}", ErrorMessage = "邮编不正确")]
        public string agent_postal { get; set; }

        [RegularExpression(@"^.{0,200}", ErrorMessage = "地址长度0-200个字符")]
        public string agent_addr { get; set; }

        [RegularExpression(@"^.{0,50}", ErrorMessage = "传真不正确")]
        public string agent_fax { get; set; }

        [RegularExpression(@"^.{0,200}", ErrorMessage = "备注长度0-200个字符")]
        public string remark { get; set; }


        public int agent_level { get; set; }
        public string createid { get; set; }
        public string createtime { get; set; }


    }
}