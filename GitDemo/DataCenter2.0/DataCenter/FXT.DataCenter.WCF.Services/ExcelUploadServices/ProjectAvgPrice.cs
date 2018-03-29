using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Infrastructure.Common.Common;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using FXT.DataCenter.Infrastructure.Common.Dictionary;
using FXT.DataCenter.Infrastructure.Common.NPOI;
using FXT.DataCenter.Infrastructure.Data.ServicesImpl;

namespace FXT.DataCenter.WCF.Services
{
    public partial class ExcelUpload
    {
        public void ProjectAvgPriceExcelUpload(int cityid, int fxtcompanyid, string filePath, string userid, string taskName)
        {
            var taskId = 0;
            try
            {
                var task = new DAT_ImportTask
                {
                    TaskName = taskName,
                    ImportType = SYS_Code_Dict.批量导入类型.VQ住宅案例均价,
                    CityID = cityid,
                    FXTCompanyId = fxtcompanyid,
                    CreateDate = DateTime.Now,
                    Creator = userid,
                    IsComplete = 0,
                    SucceedNumber = 0,
                    DataErrNumber = 0,
                    NameErrNumber = 0,
                    FilePath = "",
                    Steps = 1
                };
                taskId = _importTask.AddTask(task);

                var excelHelper = new ExcelHandle(filePath);
                var data = excelHelper.ExcelToDataTable("Sheet1", true);

                var integer = Math.Floor(Convert.ToDouble(data.Rows.Count / 50));

                List<Dat_ProjectAvg> listTrue;//正确数据
                DataTable dtError;//格式错误数据
                DataFilter(taskId, integer, cityid, fxtcompanyid, data, out listTrue, out dtError);

                //错误数据写入Excel
                var fileNamePath = string.Empty;
                if (dtError.Rows.Count > 0)
                {
                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "住宅案例均价错误数据.xlsx";
                    var folder = MapPath("NeedHandledFiles/FailureData/" + fxtcompanyid);
                    fileNamePath = Path.Combine(folder, fileName);
                    excelHelper.CreateExcel(dtError, fileNamePath, folder);
                }

                var failureNum = 0;
                var index4True = 0;//用于统计进度
                listTrue.ForEach(m =>
                {
                    #region 更新进度
                    index4True++;
                    if (integer > 0)
                    {
                        if (index4True % integer == 0)
                        {
                            _importTask.TaskStepsIncreased(taskId);
                        }
                    }
                    #endregion

                    var cp = FxtUserCenterService_GetFPInfo(Convert.ToInt32(ConfigurationHelper.FxtCompanyId), cityid, 1003036, userid, out _msg, _productTypeCode);

                    var pap = new ProjectAvgPrice();

                    var proHistoryAvgBe = pap.GetProjectHistoryAvgPriceByProjectid((int)m.ProjectId, m.CityID, cp.ParentShowDataCompanyId, cp.ParentProductTypeCode, m.UseMonth.AddMonths(-1));
                    if (proHistoryAvgBe != null && proHistoryAvgBe.projectavgid > 0)
                    {
                        decimal p = m.ProjectAvgPrice == null ? 0 : (decimal)m.ProjectAvgPrice;
                        decimal pb = proHistoryAvgBe.ProjectAvgPrice == null ? 0 : (decimal)proHistoryAvgBe.ProjectAvgPrice;
                        m.ProjectGained = m.ProjectAvgPrice == null || proHistoryAvgBe.ProjectAvgPrice == null ? null : (decimal?)Math.Round((p - pb) / pb, 4);
                    }

                    //History表里先更新一次
                    var proHistoryAvg = pap.GetProjectHistoryAvgPriceByProjectid((int)m.ProjectId, m.CityID, cp.ParentShowDataCompanyId, cp.ParentProductTypeCode, m.UseMonth);
                    if (proHistoryAvg != null && proHistoryAvg.projectavgid > 0)
                    {
                        m.FxtCompanyId = Convert.ToInt32(ConfigurationHelper.FxtCompanyId);
                        m.UpdateDate = DateTime.Now;
                        m.UpdateUser = userid;
                        pap.UpdateProjectHistoryAvg(m);
                    }
                    else
                    {
                        m.FxtCompanyId = Convert.ToInt32(ConfigurationHelper.FxtCompanyId);
                        m.CreateDate = DateTime.Now;
                        m.UpdateDate = DateTime.Now;
                        m.UpdateUser = userid;
                        pap.AddProjectHistoryAvg(m);
                    }

                    //更新即时表
                    if (m.UseMonth == Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-01")))
                    {
                        var proAvg = pap.GetProjectAvgPriceByProjectid((int)m.ProjectId, m.CityID, cp.ParentShowDataCompanyId, cp.ParentProductTypeCode, m.UseMonth);
                        if (proAvg != null && proAvg.projectavgid > 0)
                        {
                            m.FxtCompanyId = Convert.ToInt32(ConfigurationHelper.FxtCompanyId);
                            m.UpdateDate = DateTime.Now;
                            m.UpdateUser = userid;
                            int r = pap.UpdateProjectAvg(m);
                        }
                        else
                        {
                            //VQ模块维护，使用房讯通角色共享数据。
                            m.FxtCompanyId = Convert.ToInt32(ConfigurationHelper.FxtCompanyId);
                            m.CreateDate = DateTime.Now;
                            m.UpdateDate = DateTime.Now;
                            m.UpdateUser = userid;
                            int r = pap.AddProjectAvg(m);
                        }
                    }

                    //修改均价后，更新后一个月的涨跌幅
                    var proHistoryAvgAf = pap.GetProjectHistoryAvgPriceByProjectid((int)m.ProjectId, m.CityID, cp.ParentShowDataCompanyId, cp.ParentProductTypeCode, m.UseMonth.AddMonths(1));
                    if (proHistoryAvgAf != null && proHistoryAvgAf.projectavgid > 0)
                    {
                        decimal p = m.ProjectAvgPrice == null ? 0 : (decimal)m.ProjectAvgPrice;
                        decimal pa = proHistoryAvgAf.ProjectAvgPrice == null ? 0 : (decimal)proHistoryAvgAf.ProjectAvgPrice;
                        proHistoryAvgAf.ProjectGained = m.ProjectAvgPrice == null || proHistoryAvgAf.ProjectAvgPrice == null ? null : (decimal?)Math.Round((pa - p) / p, 4);
                        proHistoryAvgAf.FxtCompanyId = Convert.ToInt32(ConfigurationHelper.FxtCompanyId);
                        proHistoryAvgAf.UpdateDate = DateTime.Now;
                        proHistoryAvgAf.UpdateUser = userid;
                        pap.UpdateProjectHistoryAvg(proHistoryAvgAf);
                    }
                    //更新即时表
                    if (m.UseMonth.AddMonths(1) == Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-01")))
                    {
                        var proAvgAf = pap.GetProjectAvgPriceByProjectid((int)m.ProjectId, m.CityID, cp.ParentShowDataCompanyId, cp.ParentProductTypeCode, m.UseMonth.AddMonths(1));
                        if (proAvgAf != null && proAvgAf.projectavgid > 0)
                        {
                            decimal p = m.ProjectAvgPrice == null ? 0 : (decimal)m.ProjectAvgPrice;
                            decimal pa = proAvgAf.ProjectAvgPrice == null ? 0 : (decimal)proAvgAf.ProjectAvgPrice;
                            proAvgAf.ProjectGained = m.ProjectAvgPrice == null || proAvgAf.ProjectAvgPrice == null ? null : (decimal?)Math.Round((pa - p) / p, 4);
                            proAvgAf.FxtCompanyId = Convert.ToInt32(ConfigurationHelper.FxtCompanyId);
                            proAvgAf.UpdateDate = DateTime.Now;
                            proAvgAf.UpdateUser = userid;
                            int r = pap.UpdateProjectAvg(proAvgAf);
                        }
                    }
                });
                var indexPath = fileNamePath.IndexOf("NeedHandledFiles");
                var relativePath = string.Empty;
                if (indexPath >= 0)
                {
                    relativePath = fileNamePath.Substring(indexPath);
                    relativePath = relativePath.Replace(@"\", @"/");
                }
                _importTask.UpdateTask(taskId, listTrue.Count - failureNum, dtError.Rows.Count, 0, relativePath, 1, 100);
            }

            catch (Exception ex)
            {
                _importTask.UpdateTask(taskId, 0, 0, 0, "", -1);
                LogHelper.WriteLog("ProjectAvgPriceRevisedExcelUpload", "", userid, cityid, fxtcompanyid, ex);
            }
        }

        private void DataFilter(int taskId, double integer, int cityId, int fxtCompanyId, DataTable dt, out List<Dat_ProjectAvg> listTrue, out DataTable dtError)
        {
            listTrue = new List<Dat_ProjectAvg>();
            dtError = new DataTable();

            foreach (DataColumn column in dt.Columns)
                dtError.Columns.Add(column.Caption);

            for (var i = 0; i < dt.Rows.Count; i++)
            {
                var db = new Dat_ProjectAvg();
                var dr = dtError.NewRow();
                db.CityID = cityId;
                var isError = false;

                var projectId = dt.Rows[i]["楼盘ID"].ToString().Trim();
                var projectid = TryParseHelper.StrToInt32(projectId, 0);
                db.ProjectId = projectid;
                dr["楼盘ID"] = projectId;
                if (projectid == 0)
                {
                    isError = true;
                    dr["楼盘ID"] = projectId + "请填写正确的楼盘ID#error";
                }

                var avgPrice = dt.Rows[i]["楼盘均价_元/平方米"].ToString().Trim();
                var aprice = (int?)TryParseHelper.StrToInt32(avgPrice);
                db.ProjectAvgPrice = aprice;
                dr["楼盘均价_元/平方米"] = avgPrice;
                if (aprice == null || aprice <= 0)
                {
                    isError = true;
                    dr["楼盘均价_元/平方米"] = avgPrice + "请填写正确值#error";
                }

                var useMonthStr = dt.Rows[i]["案例月份"].ToString().Trim();
                var useMonth = (DateTime?)TryParseHelper.StrToDateTime(useMonthStr);
                dr["案例月份"] = useMonthStr;
                if (useMonth == null || useMonth == DateTime.MinValue)
                {
                    isError = true;
                    dr["案例月份"] = useMonthStr + "请填写正确值#error";
                }
                else if (useMonth != (DateTime)TryParseHelper.StrToDateTime(((DateTime)useMonth).ToString("yyyy-MM-01")))
                {
                    isError = true;
                    dr["案例月份"] = useMonthStr + "请填写正确值#error";
                }
                else
                {
                    db.UseMonth = (DateTime)useMonth;
                }

                var isevalueText = dt.Rows[i]["是否确认价格"].ToString().Trim();
                var isevalue = (isevalueText == "是" || isevalueText == "1") ? 1 : 0;
                db.isevalue = isevalue;
                dr["是否确认价格"] = isevalueText;
                if (!string.IsNullOrEmpty(isevalueText) && isevalue == -1)
                {
                    isError = true;
                    dr["是否确认价格"] = isevalueText + "#error";
                }

                if (i > 0 && integer > 0)
                {
                    if (i % integer == 0)
                    {
                        _importTask.TaskStepsIncreased(taskId);
                    }
                }

                if (isError)
                {
                    dr["行政区"] = dt.Rows[i]["行政区"].ToString().Trim();
                    dr["楼盘名称"] = dt.Rows[i]["楼盘名称"].ToString().Trim();
                    dr["楼盘别名"] = dt.Rows[i]["楼盘别名"].ToString().Trim();
                    dr["涨跌幅_百分比"] = dt.Rows[i]["涨跌幅_百分比"].ToString().Trim();
                    dr["案例均价值参考"] = dt.Rows[i]["案例均价值参考"].ToString().Trim();
                    dr["创建时间"] = dt.Rows[i]["创建时间"].ToString().Trim();
                    dr["修改时间"] = dt.Rows[i]["修改时间"].ToString().Trim();
                    dr["修改人"] = dt.Rows[i]["修改人"].ToString().Trim();
                    dr["当前基准房价_元/平方米"] = dt.Rows[i]["当前基准房价_元/平方米"].ToString().Trim();
                    dtError.Rows.Add(dr);
                }
                else
                {
                    listTrue.Add(db);
                }
            }
        }

    }
}
