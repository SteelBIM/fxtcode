using FxtDataAcquisition.BLL;
using FxtDataAcquisition.NHibernate.Entities;
using FxtDataAcquisition.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace FxtDataAcquisition.Web.Common
{
    public static class WebMenuHelp
    {
        public static readonly List<WebMenuClass> BaseMenu = new List<WebMenuClass>();
        static WebMenuHelp()
        {
            BaseMenu = GetBaseMenu();
        }
        static List<WebMenuClass> GetBaseMenu()
        {
            List<WebMenuClass> list = new List<WebMenuClass>();
            XmlDocument doc = new XmlDocument();
            doc.Load(HttpContext.Current.Server.MapPath("/Menu.xml"));
            XmlNodeList menuItems = doc.SelectSingleNode("menu").ChildNodes;
            foreach (XmlNode v in menuItems)
            {
                int index = Convert.ToInt32(v.SelectSingleNode("index").InnerText);
                string name = v.SelectSingleNode("name").InnerText;
                string url = v.SelectSingleNode("url").InnerText;
                string icon = v.SelectSingleNode("icon").InnerText;
                string pagekey = v.SelectSingleNode("pagekey").InnerText;
                WebMenuClass menuObj = new WebMenuClass { Index = index, Name = name, Icon = icon, Url = url, PageKey = pagekey };
                if (v.SelectSingleNode("childrenmenuitem") != null)
                {
                    List<WebMenuClass> _list = new List<WebMenuClass>();
                    foreach (XmlNode v2 in v.SelectSingleNode("childrenmenuitem").ChildNodes)
                    {
                        int index2 = Convert.ToInt32(v2.SelectSingleNode("index").InnerText);
                        string name2 = v2.SelectSingleNode("name").InnerText;
                        string url2 = v2.SelectSingleNode("url").InnerText;
                        string icon2 = v2.SelectSingleNode("icon").InnerText;
                        string pagekey2 = v2.SelectSingleNode("pagekey").InnerText;
                        WebMenuClass menuObj2 = new WebMenuClass { Index = index2, Name = name2, Icon = icon2, Url = url2, PageKey = pagekey2 };
                        _list.Add(menuObj2);
                    }
                    menuObj.ChildrenMenus = _list;
                }
                list.Add(menuObj);
            }
            return list;
        }

        /// <summary>
        /// 获取用户所拥有的菜单
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="userName"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public static List<WebMenuClass> GetNowUserMenu(int cityId, string userName, int companyId)
        {
            List<WebMenuClass> baseList = new List<WebMenuClass>();
            WebMenuClass[] lists = new WebMenuClass[BaseMenu.Count()];
            BaseMenu.CopyTo(lists);
            baseList = lists.ToList();
            //获取当前用户在数据库中拥有的菜单
            IList<SYSMenu> roleMenuList = SYSMenuManager.GetSYSMenuPageByUserNameAndCompanyIdAndCityId(userName, companyId, cityId);
            if (roleMenuList == null || roleMenuList.Count < 1)
            {
                return new List<WebMenuClass>();
            }
            List<string> urlList = roleMenuList.Select(_obj => _obj.URL.ToLower()).ToList();
            //从页面展示菜单中过滤出当前用户拥有的一级菜单
            List<WebMenuClass> userList = baseList.Where(obj =>
                  urlList.Contains(obj.PageKey.ToLower())
                  ||
                  (obj.ChildrenMenus != null && obj.ChildrenMenus.Where(obj2 => urlList.Contains(obj2.PageKey.ToLower())).Count() > 0)
                ).ToList();
            //从页面展示菜单中过滤出当前用户拥有的二级菜单
            foreach (WebMenuClass obj in userList)
            {
                if (obj.ChildrenMenus != null && obj.ChildrenMenus.Count > 0)
                {
                    List<WebMenuClass> _clist = obj.ChildrenMenus.Where(_obj => urlList.Contains(_obj.PageKey.ToLower())).ToList();
                    obj.ChildrenMenus = _clist;
                }
            }
            return userList;
        }

        public static List<WebMenuClass> GetNowUserMenu(IList<SYSMenu> roleMenuList)
        {
            List<WebMenuClass> baseList = new List<WebMenuClass>();
            WebMenuClass[] lists = new WebMenuClass[BaseMenu.Count()];
            BaseMenu.CopyTo(lists);
            baseList = lists.ToList();
            if (roleMenuList == null || roleMenuList.Count < 1)
            {
                return new List<WebMenuClass>();
            }
            List<string> urlList = roleMenuList.Select(_obj => _obj.URL.ToLower()).ToList();
            //从页面展示菜单中过滤出当前用户拥有的一级菜单
            List<WebMenuClass> userList = baseList.Where(obj =>
                  urlList.Contains(obj.PageKey.ToLower())
                  ||
                  (obj.ChildrenMenus != null 
                  && obj.ChildrenMenus.Where(obj2 => urlList.Contains(obj2.PageKey.ToLower())).Count() > 0)
                ).ToList();
            //从页面展示菜单中过滤出当前用户拥有的二级菜单
            foreach (WebMenuClass obj in userList)
            {
                if (obj.ChildrenMenus != null && obj.ChildrenMenus.Count > 0)
                {
                    List<WebMenuClass> _clist = obj.ChildrenMenus.Where(_obj => urlList.Contains(_obj.PageKey.ToLower())).ToList();
                    obj.ChildrenMenus = _clist;
                }
            }
            return userList;
        }
    }
}