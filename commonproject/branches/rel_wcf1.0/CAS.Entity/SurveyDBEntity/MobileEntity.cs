using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.Entity.SurveyDBEntity
{
    /// <summary>
    /// 云查勘登录手机版本相关
    /// </summary>
    public class MobileEntity
    {
        /// <summary>
        /// 移动端平台类型 (ios，android)
        /// </summary>
        public string splatype { get; set; }
        /// <summary>
        /// 移动端平台版本 (如ios6,ios7,android4.0)
        /// </summary>
        public string platver { get; set; }
        /// <summary>
        /// 移动端平台软件类型 (如云查勘，零售信贷估价管理平台)
        /// </summary>
        public string stype { get; set; }
        /// <summary>
        /// 软件版本号
        /// </summary>
        private int vcode;
        /// <summary>
        /// 软件版本号 version  
        /// </summary>
        public decimal version { get; set; }
         /// <summary>
        /// 软件渠道 (360,baidu,91等)
        /// </summary>
        public int channel { get; set; }        
        /// <summary>
        /// 软件版本  如银行版（1003022），企业版（1003008），个人版（1003007）
        /// </summary>
        public int systypecode { get; set; }
    }

    /// <summary>
    /// 云查勘登录用户实体
    /// </summary>
    public class MobileUinfo
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public String username { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public String password { get; set; }
    }
}
