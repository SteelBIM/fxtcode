using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.DAL.LinqToSql;
using FxtSpider.Common;

namespace FxtSpider.Manager.Common
{
    public static class WebJsonHelp
    {

        public static string Web_GetJson(this VIEW_案例信息_城市表_网站表 view)
        {
            if (view == null)
            {
                return null;
            }
            string projectName = "";
            string areaName = "";
            string subAreaName = "";
            if (view.ProjectName == null)
            {
                if (!string.IsNullOrEmpty(view.楼盘名))
                {
                    projectName = view.楼盘名;
                }
            }
            else
            {
                projectName = view.ProjectName;
            }
            if (view.AreaName == null)
            {
                if (!string.IsNullOrEmpty(view.行政区))
                {
                    areaName = view.行政区;
                }
            }
            else
            {
                areaName = view.AreaName;
            }
            if (view.SubAreaName == null)
            {
                if (!string.IsNullOrEmpty(view.片区))
                {
                    subAreaName = view.片区;
                }
            }
            else
            {
                subAreaName = view.SubAreaName;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"ID\":\"").Append(view.ID.ToString()).Append("\",");
            sb.Append("\"projectName\":\"").Append(JsonHelp.EncodeField(projectName)).Append("\",");
            sb.Append("\"caseDate\":\"").Append(JsonHelp.EncodeField(view.案例时间 == null ? "" : view.案例时间.ToString())).Append("\",");
            sb.Append("\"areaName\":\"").Append(JsonHelp.EncodeField(areaName)).Append("\",");
            sb.Append("\"areaName2\":\"").Append(JsonHelp.EncodeField(subAreaName)).Append("\",");
            sb.Append("\"building\":\"").Append(JsonHelp.EncodeField(view.楼栋 == null ? "" : view.楼栋)).Append("\",");
            sb.Append("\"houseNumber\":\"").Append(JsonHelp.EncodeField(view.房号 == null ? "" : view.房号.ToString())).Append("\",");
            sb.Append("\"purpose\":\"").Append(JsonHelp.EncodeField(view.SysData用途 == null ? "" : view.SysData用途)).Append("\",");
            sb.Append("\"are\":\"").Append(JsonHelp.EncodeField(view.面积 == null ? "" : Convert.ToString(view.面积))).Append("\",");
            sb.Append("\"unitPrice\":\"").Append(JsonHelp.EncodeField(view.单价 == null ? "" : Convert.ToString(view.单价))).Append("\",");
            sb.Append("\"caseType\":\"").Append(JsonHelp.EncodeField(view.SysData案例类型 == null ? "" : view.SysData案例类型)).Append("\",");
            sb.Append("\"structure\":\"").Append(JsonHelp.EncodeField(view.SysData结构 == null ? "" : view.SysData结构)).Append("\",");
            sb.Append("\"buildingType\":\"").Append(JsonHelp.EncodeField(view.SysData建筑类型 == null ? "" : view.SysData建筑类型)).Append("\",");
            sb.Append("\"totalPrice\":\"").Append(JsonHelp.EncodeField(view.总价 == null ? "" : Convert.ToString(view.总价))).Append("\",");
            sb.Append("\"floorNumber\":\"").Append(JsonHelp.EncodeField(view.所在楼层 == null ? "" : Convert.ToString(view.所在楼层))).Append("\",");
            sb.Append("\"totalFloor\":\"").Append(JsonHelp.EncodeField(view.总楼层 == null ? "" : Convert.ToString(view.总楼层))).Append("\",");
            sb.Append("\"houseType\":\"").Append(JsonHelp.EncodeField(view.SysData户型 == null ? "" : view.SysData户型)).Append("\",");
            sb.Append("\"front\":\"").Append(JsonHelp.EncodeField(view.SysData朝向 == null ? "" : view.SysData朝向)).Append("\",");
            sb.Append("\"fitment\":\"").Append(JsonHelp.EncodeField(view.SysData装修 == null ? "" : view.SysData装修)).Append("\",");
            sb.Append("\"buildingDate\":\"").Append(JsonHelp.EncodeField(view.建筑年代 == null ? "" : view.建筑年代)).Append("\",");
            sb.Append("\"title\":\"").Append(JsonHelp.EncodeField(view.信息 == null ? "" : view.信息)).Append("\",");
            sb.Append("\"phone\":\"").Append(JsonHelp.EncodeField(view.电话 == null ? "" : view.电话)).Append("\",");
            sb.Append("\"url\":\"").Append(JsonHelp.EncodeField(view.URL == null ? "" : view.URL)).Append("\",");
            sb.Append("\"moneyUnit\":\"").Append(JsonHelp.EncodeField(view.SysData币种 == null ? "" : view.SysData币种)).Append("\",");
            sb.Append("\"address\":\"").Append(JsonHelp.EncodeField(view.地址 == null ? "" : view.地址)).Append("\",");
            sb.Append("\"createDate\":\"").Append(JsonHelp.EncodeField(view.创建时间 == null ? "" : view.创建时间.ToString())).Append("\",");
            sb.Append("\"xingshi\":\"").Append(JsonHelp.EncodeField(view.建筑形式 == null ? "" : view.建筑形式)).Append("\",");
            sb.Append("\"huayuan\":\"").Append(JsonHelp.EncodeField(view.花园面积 == null ? "" : Convert.ToString(view.花园面积))).Append("\",");
            sb.Append("\"tingjiegou\":\"").Append(JsonHelp.EncodeField(view.厅结构 == null ? "" : view.厅结构)).Append("\",");
            sb.Append("\"chewei\":\"").Append(JsonHelp.EncodeField(view.车位数量 == null ? "" : Convert.ToString(view.车位数量))).Append("\",");
            sb.Append("\"peitao\":\"").Append(JsonHelp.EncodeField(view.配套设施 == null ? "" : view.配套设施)).Append("\",");
            sb.Append("\"dixiashi\":\"").Append(JsonHelp.EncodeField(view.地下室面积 == null ? "" : Convert.ToString(view.地下室面积))).Append("\",");
            sb.Append("\"city\":\"").Append(JsonHelp.EncodeField(view.城市 == null ? "" : view.城市)).Append("\",");
            sb.Append("\"web\":\"").Append(JsonHelp.EncodeField(view.网站 == null ? "" : view.网站)).Append("\",");
            sb.Append("\"count\":\"").Append(JsonHelp.EncodeField(view.动态排序字段 == null ? "" : view.动态排序字段.ToString())).Append("\",");
            sb.Append("\"fxtId\":\"").Append(view.fxtId == null ? "" : view.fxtId.ToString()).Append("\"");
            sb.Append("}");
            return sb.ToString();
        }


        public static string Web_GetJson(this VIEW_案例信息_城市表_网站表2 view)
        {
            if (view == null)
            {
                return null;
            }
            string projectName = "";
            string areaName = "";
            string subAreaName = "";
            if (view.ProjectName == null)
            {
                if (!string.IsNullOrEmpty(view.楼盘名))
                {
                    projectName = view.楼盘名;
                }
            }
            else
            {
                projectName = view.ProjectName;
            }
            if (view.AreaName == null)
            {
                if (!string.IsNullOrEmpty(view.行政区))
                {
                    areaName = view.行政区;
                }
            }
            else
            {
                areaName = view.AreaName;
            }
            if (view.SubAreaName == null)
            {
                if (!string.IsNullOrEmpty(view.片区))
                {
                    subAreaName = view.片区;
                }
            }
            else
            {
                subAreaName = view.SubAreaName;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"ID\":\"").Append(view.ID.ToString()).Append("\",");
            sb.Append("\"projectName\":\"").Append(JsonHelp.EncodeField(projectName)).Append("\",");
            sb.Append("\"caseDate\":\"").Append(JsonHelp.EncodeField(view.案例时间 == null ? "" : view.案例时间.ToString())).Append("\",");
            sb.Append("\"areaName\":\"").Append(JsonHelp.EncodeField(areaName)).Append("\",");
            sb.Append("\"areaName2\":\"").Append(JsonHelp.EncodeField(subAreaName)).Append("\",");
            sb.Append("\"building\":\"").Append(JsonHelp.EncodeField(view.楼栋 == null ? "" : view.楼栋)).Append("\",");
            sb.Append("\"houseNumber\":\"").Append(JsonHelp.EncodeField(view.房号 == null ? "" : view.房号.ToString())).Append("\",");
            sb.Append("\"purpose\":\"").Append(JsonHelp.EncodeField(view.SysData用途 == null ? "" : view.SysData用途)).Append("\",");
            sb.Append("\"are\":\"").Append(JsonHelp.EncodeField(view.面积 == null ? "" : Convert.ToString(view.面积))).Append("\",");
            sb.Append("\"unitPrice\":\"").Append(JsonHelp.EncodeField(view.单价 == null ? "" : Convert.ToString(view.单价))).Append("\",");
            sb.Append("\"caseType\":\"").Append(JsonHelp.EncodeField(view.SysData案例类型 == null ? "" : view.SysData案例类型)).Append("\",");
            sb.Append("\"structure\":\"").Append(JsonHelp.EncodeField(view.SysData结构 == null ? "" : view.SysData结构)).Append("\",");
            sb.Append("\"buildingType\":\"").Append(JsonHelp.EncodeField(view.SysData建筑类型 == null ? "" : view.SysData建筑类型)).Append("\",");
            sb.Append("\"totalPrice\":\"").Append(JsonHelp.EncodeField(view.总价 == null ? "" : Convert.ToString(view.总价))).Append("\",");
            sb.Append("\"floorNumber\":\"").Append(JsonHelp.EncodeField(view.所在楼层 == null ? "" : Convert.ToString(view.所在楼层))).Append("\",");
            sb.Append("\"totalFloor\":\"").Append(JsonHelp.EncodeField(view.总楼层 == null ? "" : Convert.ToString(view.总楼层))).Append("\",");
            sb.Append("\"houseType\":\"").Append(JsonHelp.EncodeField(view.SysData户型 == null ? "" : view.SysData户型)).Append("\",");
            sb.Append("\"front\":\"").Append(JsonHelp.EncodeField(view.SysData朝向 == null ? "" : view.SysData朝向)).Append("\",");
            sb.Append("\"fitment\":\"").Append(JsonHelp.EncodeField(view.SysData装修 == null ? "" : view.SysData装修)).Append("\",");
            sb.Append("\"buildingDate\":\"").Append(JsonHelp.EncodeField(view.建筑年代 == null ? "" : view.建筑年代)).Append("\",");
            sb.Append("\"title\":\"").Append(JsonHelp.EncodeField(view.信息 == null ? "" : view.信息)).Append("\",");
            sb.Append("\"phone\":\"").Append(JsonHelp.EncodeField(view.电话 == null ? "" : view.电话)).Append("\",");
            sb.Append("\"url\":\"").Append(JsonHelp.EncodeField(view.URL == null ? "" : view.URL)).Append("\",");
            sb.Append("\"moneyUnit\":\"").Append(JsonHelp.EncodeField(view.SysData币种 == null ? "" : view.SysData币种)).Append("\",");
            sb.Append("\"address\":\"").Append(JsonHelp.EncodeField(view.地址 == null ? "" : view.地址)).Append("\",");
            sb.Append("\"createDate\":\"").Append(JsonHelp.EncodeField(view.创建时间 == null ? "" : view.创建时间.ToString())).Append("\",");
            sb.Append("\"xingshi\":\"").Append(JsonHelp.EncodeField(view.建筑形式 == null ? "" : view.建筑形式)).Append("\",");
            sb.Append("\"huayuan\":\"").Append(JsonHelp.EncodeField(view.花园面积 == null ? "" : Convert.ToString(view.花园面积))).Append("\",");
            sb.Append("\"tingjiegou\":\"").Append(JsonHelp.EncodeField(view.厅结构 == null ? "" : view.厅结构)).Append("\",");
            sb.Append("\"chewei\":\"").Append(JsonHelp.EncodeField(view.车位数量 == null ? "" : Convert.ToString(view.车位数量))).Append("\",");
            sb.Append("\"peitao\":\"").Append(JsonHelp.EncodeField(view.配套设施 == null ? "" : view.配套设施)).Append("\",");
            sb.Append("\"dixiashi\":\"").Append(JsonHelp.EncodeField(view.地下室面积 == null ? "" : Convert.ToString(view.地下室面积))).Append("\",");
            sb.Append("\"city\":\"").Append(JsonHelp.EncodeField(view.城市 == null ? "" : view.城市)).Append("\",");
            sb.Append("\"cityId\":\"").Append(view.城市ID).Append("\",");
            sb.Append("\"web\":\"").Append(JsonHelp.EncodeField(view.网站 == null ? "" : view.网站)).Append("\",");
            sb.Append("\"count\":\"").Append(JsonHelp.EncodeField(view.NotImportCaseCount == null ? "" : view.NotImportCaseCount.ToString())).Append("\",");
            sb.Append("\"fxtId\":\"").Append(view.fxtId == null ? "" : view.fxtId.ToString()).Append("\"");
            sb.Append("}");
            return sb.ToString();
        }

        /// <summary>
        /// VIEW_案例信息_城市表_网站表 List转换为Json
        /// </summary>
        /// <param name="viewList"></param>
        /// <returns></returns>
        public static string Web_GetJson(this List<VIEW_案例信息_城市表_网站表> viewList)
        {
            if (viewList == null || viewList.Count < 1)
            {
                return null;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (VIEW_案例信息_城市表_网站表 view in viewList)
            {
                string jsonStr = Web_GetJson(view);
                if (jsonStr != null && jsonStr != "")
                {
                    sb.Append(jsonStr).Append(",");
                }
            }
            return sb.ToString().TrimEnd(',') + "]";

        }
        /// <summary>
        /// VIEW_案例信息_城市表_网站表2 List转换为Json
        /// </summary>
        /// <param name="viewList"></param>
        /// <returns></returns>
        public static string Web_GetJson(this List<VIEW_案例信息_城市表_网站表2> viewList)
        {
            if (viewList == null || viewList.Count < 1)
            {
                return null;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (VIEW_案例信息_城市表_网站表2 view in viewList)
            {
                string jsonStr = Web_GetJson(view);
                if (jsonStr != null && jsonStr != "")
                {
                    sb.Append(jsonStr).Append(",");
                }
            }
            return sb.ToString().TrimEnd(',') + "]";

        }

        /// <summary>
        /// 输出json格式(ajax统一输出json格式)
        /// </summary>
        /// <param name="detail"></param>
        /// <param name="result">请求是否成功</param>
        /// <param name="errorType">请求失败时的错误类型(0:无,1(WebUserHelp.NotLogin):未登陆,2(WebUserHelp.NotRight):无权限,3(WebUserHelp.SysError):系统错误)</param>
        /// <param name="message">消息</param>
        /// <returns></returns>
        public static string MvcResponseJson(this string detail,int result=1,string errorType="", string message="")
        {
            StringBuilder sb = new StringBuilder("{\"result\":\"");
            sb.Append(result).Append("\",\"errorType\":\"");
            sb.Append(errorType).Append("\",\"message\":\"");
            sb.Append(message.EncodeField()).Append("\",\"detail\":");
            if (!string.IsNullOrEmpty(detail) && (detail[0].Equals('[') || detail[0].Equals('{')))
            {
                sb.Append(detail).Append("}");
            }
            else
            {
                sb.Append("\"").Append(detail).Append("\"}");
            }
            string resultStr = sb.ToString();
            return resultStr;
        }
    }
}
