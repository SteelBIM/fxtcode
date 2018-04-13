using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Framework;


namespace KSWF.WFM.Constract.Models
{
    public class com_master
    {
        public int masterid { get; set; }
        public int parentid { get; set; }
        public string parentname { get; set; }
        
        public string mastername { get; set; }
        public string password { get; set; }
        public string truename { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string qq { get; set; }
        public int deptid { get; set; }
        public int groupid { get; set; }
        /// <summary>
        /// 渠道
        /// </summary>
        public int channel { get; set; }
        /// <summary>
        ///    0: 正常  1 黑名单  
        /// </summary>
        public int state { get; set; }
        /// <summary>
        /// 代理商或者本公司ID
        /// </summary>
        public string agentid { get; set; }

        public string pagentid { get; set; }
        /// <summary>
        /// 代理商名称
        /// </summary>
        public string agentname { get; set; }
        public DateTime agent_startdate { get; set; }
        public DateTime agent_enddate { get; set; }
        public string agent_tel { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>
        public string agent_postal { get; set; }
        public string agent_fax { get; set; }
        public string agent_addr { get; set; }
        public string agent_remark { get; set; }
        public int createid { get; set; }
        public DateTime createtime { get; set; }
        /// <summary>
        /// 用户类型 0 员工 1 代理商
        /// 
        /// </summary>
        public int mastertype { get; set; }

        /// <summary>
        /// 数据查看查看权限0
        /// 0全部
        ///1本人
        ///2本人+下级部门(含本部门)+下级代理商
        ///3本人+下级部门(不含本部门)+下级代理商
        ///4本人+下级代理商
        /// </summary>
        public int dataauthority { get; set; }
       
    }
}
