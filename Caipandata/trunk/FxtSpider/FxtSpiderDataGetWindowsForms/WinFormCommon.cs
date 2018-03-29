using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.Common;

namespace FxtSpiderDataGetWindowsForms
{
    public static class WinFormCommon
    {
        /// <summary>
        /// 优先级别从高到低：
        /// </summary>
        private static readonly string[] RemarkDid_chongqing = null;
        static WinFormCommon()
        {
            RemarkDid_chongqing = new string[] { "江景", "湖景", "江", "湖", "山", "景" };
        }
        /// <summary>
        /// 根据城市的字典提取关键字
        /// </summary>
        /// <param name="cityName"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public static string GetCaseRemark(string cityName, string remark)
        {
            if (string.IsNullOrEmpty(remark.TrimBlank()))
            {
                return remark;
            }
            StringBuilder sb = new StringBuilder("");
            if (cityName.Contains("重庆"))
            {
                foreach (string str in RemarkDid_chongqing)
                {
                    if (remark.Contains(str))
                    {
                        sb.Append(str).Append(",");
                        remark=remark.Replace(str, "");
                    }
                }
                remark = sb.ToString().TrimEnd(',');
            }
            return remark;
        }
    }
}
