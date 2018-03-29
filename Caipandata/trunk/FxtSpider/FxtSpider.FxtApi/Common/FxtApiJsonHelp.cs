using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using FxtSpider.DAL.LinqToSql;
using FxtSpider.FxtApi.Model;
using FxtSpider.Common;

namespace FxtSpider.FxtApi.Common
{
    /// <summary>
    /// 用于转换fxtApi传过来的json格式转换(用于非数据源类型或不规则实体)
    /// </summary>
    public static class FxtApiJsonHelp
    {
        public static string FxtApi_GetJson(this VIEW_案例信息_城市表_网站表 view)
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
            sb.Append("\"楼盘名\":\"").Append(projectName).Append("\",");
            sb.Append("\"案例时间\":\"").Append(view.案例时间 == null ? "" : view.案例时间.ToString()).Append("\",");
            sb.Append("\"行政区\":\"").Append(areaName).Append("\",");
            sb.Append("\"片区\":\"").Append(subAreaName).Append("\",");
            sb.Append("\"楼栋\":\"").Append(view.楼栋 == null ? "" : view.楼栋).Append("\",");
            sb.Append("\"房号\":\"").Append(view.房号 == null ? "" : view.房号.ToString()).Append("\",");
            sb.Append("\"用途\":\"").Append(view.SysData用途 == null ? "" : view.SysData用途.ToString()).Append("\",");
            sb.Append("\"面积\":\"").Append(view.面积 == null ? "" : Convert.ToString(view.面积)).Append("\",");
            sb.Append("\"单价\":\"").Append(view.单价 == null ? "" : Convert.ToString(view.单价)).Append("\",");
            sb.Append("\"案例类型\":\"").Append(view.SysData案例类型 == null ? "" : view.SysData案例类型).Append("\",");
            sb.Append("\"结构\":\"").Append(view.SysData结构 == null ? "" : view.SysData结构).Append("\",");
            sb.Append("\"建筑类型\":\"").Append(view.SysData建筑类型 == null ? "" : view.SysData建筑类型).Append("\",");
            sb.Append("\"总价\":\"").Append(view.总价 == null ? "" : Convert.ToString(view.总价)).Append("\",");
            sb.Append("\"所在楼层\":\"").Append(view.所在楼层 == null ? "" : Convert.ToString(view.所在楼层)).Append("\",");
            sb.Append("\"总楼层\":\"").Append(view.总楼层 == null ? "" : Convert.ToString(view.总楼层)).Append("\",");
            sb.Append("\"户型\":\"").Append(view.SysData户型 == null ? "" : view.SysData户型.Replace("房", "室")).Append("\",");
            sb.Append("\"朝向\":\"").Append(view.SysData朝向 == null ? "" : view.SysData朝向).Append("\",");
            sb.Append("\"装修\":\"").Append(view.SysData装修 == null ? "" : view.SysData装修).Append("\",");
            sb.Append("\"建筑年代\":\"").Append(view.建筑年代 == null ? "" : view.建筑年代).Append("\",");
            sb.Append("\"信息\":\"").Append(view.信息 == null ? "" : view.信息).Append("\",");
            sb.Append("\"电话\":\"").Append(view.电话 == null ? "" : view.电话).Append("\",");
            sb.Append("\"URL\":\"").Append(view.URL == null ? "" : view.URL).Append("\",");
            sb.Append("\"币种\":\"").Append(view.SysData币种 == null ? "" : view.SysData币种).Append("\",");
            sb.Append("\"地址\":\"").Append(view.地址 == null ? "" : view.地址).Append("\",");
            sb.Append("\"创建时间\":\"").Append(view.创建时间 == null ? "" : view.创建时间.ToString()).Append("\",");
            sb.Append("\"建筑形式\":\"").Append(view.建筑形式 == null ? "" : view.建筑形式).Append("\",");
            sb.Append("\"花园面积\":\"").Append(view.花园面积 == null ? "" : Convert.ToString(view.花园面积)).Append("\",");
            sb.Append("\"厅结构\":\"").Append(view.厅结构 == null ? "" : view.厅结构).Append("\",");
            sb.Append("\"车位数量\":\"").Append(view.车位数量 == null ? "" : Convert.ToString(view.车位数量)).Append("\",");
            sb.Append("\"配套设施\":\"").Append(view.配套设施 == null ? "" : view.配套设施).Append("\",");
            sb.Append("\"地下室面积\":\"").Append(view.地下室面积 == null ? "" : Convert.ToString(view.地下室面积)).Append("\",");
            sb.Append("\"城市\":\"").Append(view.城市 == null ? "" : view.城市).Append("\",");
            sb.Append("\"网站\":\"").Append(view.网站 == null ? "" : view.网站).Append("\"");
            sb.Append("}");
            return sb.ToString();
        }
        /// <summary>
        /// CompanyStatus List转换为Json
        /// </summary>
        /// <param name="companyStatusList"></param>
        /// <returns></returns>
        public static string FxtApi_GetJson(this List<VIEW_案例信息_城市表_网站表> viewList)
        {
            if (viewList == null || viewList.Count < 1)
            {
                return null;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (VIEW_案例信息_城市表_网站表 view in viewList)
            {
                string jsonStr = FxtApi_GetJson(view);
                if (jsonStr != null && jsonStr != "")
                {
                    sb.Append(jsonStr).Append(",");
                }
            }
            return sb.ToString().TrimEnd(',') + "]";

        }
        public static string FxtApi_GetJson(this SpiderExportResult obj)
        {
            if (obj == null)
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"ID\":\"").Append(obj.ID.ToString()).Append("\",");
            sb.Append("\"Remark\":\"").Append(obj.Remark == null ? "" : obj.Remark).Append("\",");
            sb.Append("\"Success\":\"").Append(obj.Success == null ? "" : obj.Success).Append("\"");
            sb.Append("}");
            return sb.ToString();
        }
        public static string FxtApi_GetJson(this FxtApi_SYSArea obj)
        {
            obj = JsonHelp.EncodeField<FxtApi_SYSArea>(obj);
            string jsonStr = JsonHelp.ToJSONjss(obj);
            return jsonStr;
        }
        public static string FxtApi_GetJson(this List<FxtApi_SYSArea> viewList)
        {
            if (viewList == null || viewList.Count < 1)
            {
                return null;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (FxtApi_SYSArea view in viewList)
            {
                string jsonStr = FxtApi_GetJson(view);
                if (jsonStr != null && jsonStr != "")
                {
                    sb.Append(jsonStr).Append(",");
                }
            }
            return sb.ToString().TrimEnd(',') + "]";
        }
        public static string FxtApi_GetJson(this FxtApi_SYSCity obj)
        {
            obj = JsonHelp.EncodeField<FxtApi_SYSCity>(obj);
            string jsonStr = JsonHelp.ToJSONjss(obj);
            return jsonStr;
        }
        public static string FxtApi_GetJson(this List<FxtApi_SYSCity> viewList)
        {
            if (viewList == null || viewList.Count < 1)
            {
                return null;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (FxtApi_SYSCity view in viewList)
            {
                string jsonStr = FxtApi_GetJson(view);
                if (jsonStr != null && jsonStr != "")
                {
                    sb.Append(jsonStr).Append(",");
                }
            }
            return sb.ToString().TrimEnd(',') + "]";
        }

    }
}
