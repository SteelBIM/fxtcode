using CAS.Entity.DBEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.Common
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

    public class DictionaryHelper 
    {
        public static List<SYSProvince> GetProvinceList()
        {
            List<SYSProvince> list = new List<SYSProvince>();
            list = new List<SYSProvince>() {new  SYSProvince { provinceid = 1, provincename = "北京直辖市", alias = "北京" },
            new  SYSProvince { provinceid = 2, provincename = "上海直辖市", alias = "上海" },
            new  SYSProvince { provinceid = 3, provincename = "天津直辖市", alias = "天津" },
            new  SYSProvince { provinceid = 4, provincename = "重庆直辖市", alias = "重庆" },
            new  SYSProvince { provinceid = 5, provincename = "广东省", alias = "广东" },
            new  SYSProvince { provinceid = 6, provincename = "黑龙江省", alias = "黑龙江" },
            new  SYSProvince { provinceid = 7, provincename = "吉林省", alias = "吉林" },
            new  SYSProvince { provinceid = 8, provincename = "辽宁省", alias = "辽宁" },
            new  SYSProvince { provinceid = 9, provincename = "河北省", alias = "河北" },
            new  SYSProvince { provinceid = 10, provincename = "河南省", alias = "河南" },
            new  SYSProvince { provinceid = 11, provincename = "湖北省", alias = "湖北" },
            new  SYSProvince { provinceid = 12, provincename = "湖南省", alias = "湖南" },
            new  SYSProvince { provinceid = 13, provincename = "浙江省", alias = "浙江" },
            new  SYSProvince { provinceid = 14, provincename = "江苏省", alias = "江苏" },
            new  SYSProvince { provinceid = 15, provincename = "安徽省", alias = "安徽" },
            new  SYSProvince { provinceid = 16, provincename = "贵州省", alias = "贵州" },
            new  SYSProvince { provinceid = 17, provincename = "福建省", alias = "福建" },
            new  SYSProvince { provinceid = 18, provincename = "四川省", alias = "四川" },
            new  SYSProvince { provinceid = 19, provincename = "山东省", alias = "山东" },
            new  SYSProvince { provinceid = 20, provincename = "山西省", alias = "山西" },
            new  SYSProvince { provinceid = 21, provincename = "新疆维吾尔自治区", alias = "新疆" },
            new  SYSProvince { provinceid = 22, provincename = "内蒙古自治区", alias = "内蒙古" },
            new  SYSProvince { provinceid = 23, provincename = "西藏自治区", alias = "西藏" },
            new  SYSProvince { provinceid = 24, provincename = "青海省", alias = "青海" },
            new  SYSProvince { provinceid = 25, provincename = "宁夏回族自治区", alias = "宁夏" },
            new  SYSProvince { provinceid = 26, provincename = "陕西省", alias = "陕西" },
            new  SYSProvince { provinceid = 27, provincename = "甘肃省", alias = "甘肃" },
            new  SYSProvince { provinceid = 28, provincename = "江西省", alias = "江西" },
            new  SYSProvince { provinceid = 29, provincename = "云南省", alias = "云南" },
            new  SYSProvince { provinceid = 30, provincename = "广西壮族自治区", alias = "广西" },
            new  SYSProvince { provinceid = 31, provincename = "海南省", alias = "海南" },
            new  SYSProvince { provinceid = 32, provincename = "香港特区", alias = "香港" },
            new  SYSProvince { provinceid = 33, provincename = "澳门特区", alias = "澳门" },
            new  SYSProvince { provinceid = 34, provincename = "台湾省", alias = "台湾" }};
            return list;
        }
    }
}
