using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.Common.MVC4
{
    public class EntityCommon
    {
           
    }

    /// <summary>
    /// 菜单
    /// </summary>
    public class Menus
    {
        public string ID;
        public string Class;
        public Link[] Links;
    }

    /// <summary>
    /// 链接
    /// </summary>
    public class Link
    {
        private string href;
        /// <summary>
        /// 链接指向
        /// </summary>
        public string Href
        {
            get { return href; }
            set { href = value; }
        }

        private string target=null;
        /// <summary>
        /// 链接打开的目标
        /// </summary>
        public string Target
        {
            get { return target; }
            set { target = value; }
        }

        private string id;
        /// <summary>
        /// 链接ID
        /// </summary>
        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        private int index;
        /// <summary>
        /// 链接显示顺序
        /// </summary>
        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        private string name;
        /// <summary>
        /// 链接名
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string caption;
        /// <summary>
        /// 链接显示
        /// </summary>
        public string Caption
        {
            get { return caption; }
            set { caption = value; }
        }

        private string title=null;
        /// <summary>
        /// 链接提示
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private string _class = null;
        /// <summary>
        /// 链接样式
        /// </summary>
        public string Class
        {
            get { return _class; }
            set { _class = value; }
        }

        private bool isSelected=false;
        /// <summary>
        /// 是否为选中状态
        /// </summary>
        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; }
        }
    }
}
