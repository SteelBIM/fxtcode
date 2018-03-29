using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace FxtDataAcquisition.Web.Models
{
    public class WebMenuClass
    {
        /// <summary>
        /// 菜单索引,用于排序
        /// </summary>
        public int Index
        {
            get;
            set;
        }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 菜单页面唯一建(一般为页面链接)
        /// </summary>
        public string PageKey
        {
            get;
            set;
        }
        /// <summary>
        /// 页面链接
        /// </summary>
        public string Url
        {
            get;
            set;
        }
        public string Icon
        {
            get;
            set;
        }
        public List<WebMenuClass> ChildrenMenus
        {
            get;
            set;
        }
    }
}