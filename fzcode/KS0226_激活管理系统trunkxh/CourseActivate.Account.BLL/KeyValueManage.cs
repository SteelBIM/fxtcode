using CourseActivate.Framework.BLL;
using CourseActivate.Account.Constract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Account.BLL
{
    public class KeyValueManage
    {
        static Manage m = new Manage();

        /// <summary>
        /// 获取渠道来源配置
        /// </summary>
        /// <returns></returns>
        public static List<cfg_keyvalue> GetChannleData()
        {
            return m.SelectSearch<cfg_keyvalue>(i => i.UseType == "Channel");
        }

        /// <summary>
        /// 获取产品分类配置
        /// </summary>
        /// <returns></returns>
        public static List<cfg_keyvalue> GetCatogoryData()
        {
            return m.SelectSearch<cfg_keyvalue>(i => i.UseType == "Category");
        }

        public static List<cfg_keyvalue> GetPayTypeData()
        {
            return m.SelectSearch<cfg_keyvalue>(i => i.UseType == "PayType");
        }
        
        public static List<cfg_keyvalue> GetQudaoData()
        {
            return m.SelectSearch<cfg_keyvalue>(i => i.UseType == "Qudao");
        }
        /// <summary>
        /// 产品版本
        /// </summary>
        /// <returns></returns>
        public static List<cfg_keyvalue> GetVersionData()
        {
            return m.SelectSearch<cfg_keyvalue>(i => i.UseType == "Version");
        }
    }
}
