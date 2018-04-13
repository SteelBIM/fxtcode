using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using CBSS.Core.Utility;

namespace CBSS.Web.Admin
{
    public class WebControl
    {
        /// <summary>
        /// 根据枚举转换成SelectList
        /// </summary>
        /// <param name="enumType">枚举</param>
        /// <returns></returns>
        public static List<SelectListItem> GetSelectList(Type enumType)
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            foreach (object e in Enum.GetValues(enumType))
            {
                selectList.Add(new SelectListItem() { Text = EnumHelper.GetDescription(e), Value = ((int)e).ToString() });
            }
            if (selectList!=null&&selectList.Count()>0)
            {
                return selectList.OrderBy(a => Convert.ToInt32(a.Value)).ToList();
            }
            return selectList;
        }
        /// <summary>
        /// 根据枚举转换成SelectList并且设置默认选中项
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="ObjDefaultValue">默认选中项</param>
        /// <returns></returns>
        public static List<SelectListItem> GetSelectList(Type enumType, object ObjDefaultValue)
        {
            int defaultValue = Int32.Parse(ObjDefaultValue.ToString());
            List<SelectListItem> selectList = new List<SelectListItem>();
            foreach (object e in Enum.GetValues(enumType))
            {
                try
                {
                    if ((int)e == defaultValue)
                    {
                        selectList.Add(new SelectListItem() { Text = EnumHelper.GetDescription(e), Value = ((int)e).ToString(), Selected = true });
                    }
                    else
                    {
                        selectList.Add(new SelectListItem() { Text = EnumHelper.GetDescription(e), Value = ((int)e).ToString() });
                    }
                }
                catch (Exception ex)
                {
                    string exs = ex.Message;
                }
            }
            return selectList;
        }
    }
}
