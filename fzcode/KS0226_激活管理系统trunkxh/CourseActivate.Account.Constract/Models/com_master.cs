using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Framework;


namespace CourseActivate.Account.Constract.Models
{
    public class com_master
    {
        public int masterid { get; set; }
        public string mastername { get; set; }
        public string password { get; set; }
        public string truename { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public int issend { get; set; }
        public string qq { get; set; }
        public int groupid { get; set; }
        /// <summary>
        /// 渠道
        /// </summary>
        public int channel { get; set; }
        /// <summary>
        ///    0: 正常  1 已锁定 2 已拉黑   3  已删除
        /// </summary>
        public int state { get; set; }
        public int createid { get; set; }
        public string createtime { get; set; }
        public string updatetime { get; set; }

        /// <summary>
        /// 用户类型 0 超级管理员 1普通管理员
        /// </summary>
        public int mastertype { get; set; }

        /// <summary>
        /// 数据查看查看权限0
        /// </summary>
        public int dataauthority { get; set; }
        //0全部
        //1本人
        //2本人+下级部门(含本部门)+下级代理商
        //3本人+下级部门(不含本部门)+下级代理商
        //本人+下级代理商
        public string remark { get; set; }
    }
}
