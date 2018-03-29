using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;

namespace CAS.Entity
{
    /// <summary>
    /// 登录用户实体 kevin
    /// </summary>
    public class LoginInfoEntity : SYSUser
    {
        //ip
        public string sourceip { get; set; }
        //机构ID
        public int fxtcompanyid { get; set; }
        //权限
        public string rights { get; set; }
        //公司后缀
        public string companycode { get; set; }
        //授权公司名称，来自用户中心
        public string companyname { get; set; }
        //是否管理员
        public bool isadmin { get; set; }
        /// <summary>
        /// 部门所在城市
        /// </summary>
        public int cityid { get; set; }

        /// <summary>
        /// 估价师证到期提醒
        /// </summary>
        public int appraiserexpireddays { get; set; }

        /// <summary>
        /// 报告到期提醒
        /// </summary>
        public int reportexpireddays { get; set; }

        /// <summary>
        /// 是否允许查看其他公司业务（caoq 2013-10-17 用于云查勘3.0）
        /// </summary>
        public bool allowsview { get; set; }

        public string signname { get; set; }
        public List<Apps> apps { get; set; }
        public int[] products { get; set; }
        /// <summary>
        /// 城市名字
        /// </summary>
        public string cityname { get; set; }
        /// <summary>
        /// 单位名字
        /// </summary>
        public string danweiname { get; set; }
    }
}
