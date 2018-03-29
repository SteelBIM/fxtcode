using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.CASEntity
{
    /// <summary>
    /// CAS系统基础信息 （hody 20140425）
    /// </summary>
    public class SystemInfo: BaseTO
    {
        /// <summary>
        /// 公司名称
        /// </summary>
        public string companyname { get; set; }
        /// <summary>
        /// 账号后缀
        /// </summary>
        public string companycode { get; set; }
        /// <summary>
        /// 公司名简称
        /// </summary>
        public string smssendname { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        public int companyid { get; set; }
        /// <summary>
        /// 公司logo大图路径
        /// </summary>
        public string logopath { get; set; }
        /// <summary>
        /// 公司小图路径
        /// </summary>
        public string smalllogopath { get; set; }
        /// <summary>
        /// 系统名称
        /// </summary>
        public string titlename { get; set; }
        /// <summary>
        /// 公司联系方式
        /// </summary>
        public string telephone { get; set; }
        /// <summary>
        /// 登录页照片
        /// </summary>
        public string bg_pic { get; set; }
        /// <summary>
        /// 首页
        /// </summary>
        public string homepage { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        public string currentversion { get; set; }
        /// <summary>
        /// 二维码
        /// </summary>
        public string twodimensionalcode { get; set; }
    }
}
