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
        public void ProjectWeightRevisedExcelUpload(int cityid, int fxtcompanyid, string filePath, string userid, string taskName)
        {
            var taskId = 0;

            try
            {
                //在任务列表创建一条记录  iscomplete:0,代表否；1，代表是
                var task = new DAT_ImportTask
                {
                    //此处要加导入模块信息区分:住宅、商业
                    TaskName = taskName,
                    ImportType = SYS_Code_Dict.批量导入类型.VQ住宅基准房价,
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

                List<string> modifiedProperty;
                List<DatWeightProject> listTrue;//正确数据
                DataTable dtError;//格式错误数据
                DataFilter(taskId, integer, cityid, fxtcompanyid, data, out listTrue, out dtError, out modifiedProperty);

                //错误数据写入Excel
                var fileNamePath = string.Empty;
                if (dtError.Rows.Count > 0)
                {
                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "住宅基准房价错误数据.xlsx";
                    var folder = MapPath("NeedHandledFiles/FailureData/" + fxtcompanyid);
                    fileNamePath = Path.Combine(folder, fileName);
                    excelHelper.CreateExcel(dtError, fileNamePath, folder);
                }

                var failureNum = 0;
                var index4True = 0;//用于统计进度
                //正确数据添加到Project表中
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

                    var pwr = new ProjectWeightRevised();

                    var cp = FxtUserCenterService_GetFPInfo(Convert.ToInt32(ConfigurationHelper.FxtCompanyId), cityid, 1003036, userid, out _msg, _productTypeCode);

                    //VQ模块维护，使用房讯通角色共享数据。
                    var isExist = pwr.GetWeightProject(m.ProjectId, cityid, cp.ParentShowDataCompanyId, cp.ParentProductTypeCode);
                    if (isExist != null && isExist.Id > 0)//存在则更新
                    {
                        m.Id = isExist.Id;
                        m.UpdateDate = DateTime.Now;
                        m.UpdateUser = userid;
                        m.EvaluationCompanyId = fxtcompanyid;
                        var modifyResult = pwr.UpdateWeightProject(m);
                        if (modifyResult <= 0) failureNum = failureNum + 1;
                    }
                    else//不存在则新增
                    {
                        m.FxtCompanyId = Convert.ToInt32(ConfigurationHelper.FxtCompanyId);
                        m.EvaluationCompanyId = fxtcompanyid;
                        m.UpdateUser = userid;
                        m.UpdateDate = DateTime.Now;
                        m.LowLayerWeight = 1;
                        m.MultiLayerWeight = 1;
                        m.SmallHighLayerWeight = 1;
                        m.HighLayerWeight = 1;
                        var insertResult = pwr.AddWeightProject(m);
                        if (insertResult <= 0) failureNum = failureNum + 1;
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
                LogHelper.WriteLog("ProjectWeightRevisedExcelUpload", "", userid, cityid, fxtcompanyid, ex);
            }

        }

        private void DataFilter(int taskId, double integer, int cityId, int fxtCompanyId, DataTable dt, out List<DatWeightProject> listTrue, out DataTable dtError, out List<string> modifiedProperty)
        {

            modifiedProperty = new List<string>();
            listTrue = new List<DatWeightProject>();
            dtError = new DataTable();

            foreach (DataColumn column in dt.Columns)
                dtError.Columns.Add(column.Caption);

            for (var i = 0; i < dt.Rows.Count; i++)
            {
                var db = new DatWeightProject();
                var dr = dtError.NewRow();
                db.CityId = cityId;
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

                var avgPrice = dt.Rows[i]["楼盘均价"].ToString().Trim();
                var aprice = (int?)TryParseHelper.StrToInt32(avgPrice);
                db.ProjectAvgPrice = aprice;
                dr["楼盘均价"] = avgPrice;
                if (!string.IsNullOrEmpty(avgPrice) && aprice == null)
                {
                    isError = true;
                    dr["楼盘均价"] = avgPrice + "请填写正确值#error";
                }
                else if (aprice <= 100)
                {
                    isError = true;
                    dr["楼盘均价"] = avgPrice + "值必须大于100#error";
                }

                var lowavgPrice = dt.Rows[i]["低层建筑均价"].ToString().Trim();
                var lowaprice = (int?)TryParseHelper.StrToInt32(lowavgPrice);
                db.LowLayerPrice = lowaprice;
                dr["低层建筑均价"] = lowavgPrice;
                if (!string.IsNullOrEmpty(lowavgPrice) && lowaprice == null)
                {
                    isError = true;
                    dr["低层建筑均价"] = lowavgPrice + "请填写正确值#error";
                }
                else if (lowaprice <= 100)
                {
                    isError = true;
                    dr["低层建筑均价"] = lowavgPrice + "值必须大于100#error";
                }

                var mulavgPrice = dt.Rows[i]["多层建筑均价"].ToString().Trim();
                var mulaprice = (int?)TryParseHelper.StrToInt32(mulavgPrice);
                db.MultiLayerPrice = mulaprice;
                dr["多层建筑均价"] = mulavgPrice;
                if (!string.IsNullOrEmpty(mulavgPrice) && mulaprice == null)
                {
                    isError = true;
                    dr["多层建筑均价"] = mulavgPrice + "请填写正确值#error";
                }
                else if (mulaprice <= 100)
                {
                    isError = true;
                    dr["多层建筑均价"] = mulavgPrice + "值必须大于100#error";
                }

                var smallavgPrice = dt.Rows[i]["小高层建筑均价"].ToString().Trim();
                var smallaprice = (int?)TryParseHelper.StrToInt32(smallavgPrice);
                db.SmallHighLayerPrice = smallaprice;
                dr["小高层建筑均价"] = smallavgPrice;
                if (!string.IsNullOrEmpty(smallavgPrice) && smallaprice == null)
                {
                    isError = true;
                    dr["小高层建筑均价"] = smallavgPrice + "请填写正确值#error";
                }
                else if (smallaprice <= 100)
                {
                    isError = true;
                    dr["小高层建筑均价"] = smallavgPrice + "值必须大于100#error";
                }

                var highavgPrice = dt.Rows[i]["高层建筑均价"].ToString().Trim();
                var highaprice = (int?)TryParseHelper.StrToInt32(highavgPrice);
                db.HighLayerPrice = highaprice;
                dr["高层建筑均价"] = highavgPrice;
                if (!string.IsNullOrEmpty(highavgPrice) && highaprice == null)
                {
                    isError = true;
                    dr["高层建筑均价"] = highavgPrice + "请填写正确值#error";
                }
                else if (highaprice <= 100)
                {
                    isError = true;
                    dr["高层建筑均价"] = highavgPrice + "值必须大于100#error";
                }

                var sigleavgPrice = dt.Rows[i]["独幢别墅建筑均价"].ToString().Trim();
                var sigleaprice = (int?)TryParseHelper.StrToInt32(sigleavgPrice);
                db.SingleVillaPrice = sigleaprice;
                dr["独幢别墅建筑均价"] = sigleavgPrice;
                if (!string.IsNullOrEmpty(sigleavgPrice) && sigleaprice == null)
                {
                    isError = true;
                    dr["独幢别墅建筑均价"] = sigleavgPrice + "请填写正确值#error";
                }
                else if (sigleaprice <= 100)
                {
                    isError = true;
                    dr["独幢别墅建筑均价"] = sigleavgPrice + "值必须大于100#error";
                }

                var platoonavgPrice = dt.Rows[i]["联排别墅建筑均价"].ToString().Trim();
                var platoonaprice = (int?)TryParseHelper.StrToInt32(platoonavgPrice);
                db.PlatoonVillaPrice = platoonaprice;
                dr["联排别墅建筑均价"] = platoonavgPrice;
                if (!string.IsNullOrEmpty(platoonavgPrice) && platoonaprice == null)
                {
                    isError = true;
                    dr["联排别墅建筑均价"] = platoonavgPrice + "请填写正确值#error";
                }
                else if (platoonaprice <= 100)
                {
                    isError = true;
                    dr["联排别墅建筑均价"] = platoonavgPrice + "值必须大于100#error";
                }

                var superpositionVillaPrice = dt.Rows[i]["叠加别墅建筑均价"].ToString().Trim();
                var superpositionvp = (int?)TryParseHelper.StrToInt32(superpositionVillaPrice);
                db.SuperpositionVillaPrice = superpositionvp;
                dr["叠加别墅建筑均价"] = superpositionVillaPrice;
                if (!string.IsNullOrEmpty(superpositionVillaPrice) && superpositionvp == null)
                {
                    isError = true;
                    dr["叠加别墅建筑均价"] = superpositionVillaPrice + "请填写正确值#error";
                }
                else if (superpositionvp <= 100)
                {
                    isError = true;
                    dr["叠加别墅建筑均价"] = superpositionVillaPrice + "值必须大于100#error";
                }

                var duplexesVillaPrice = dt.Rows[i]["双拼别墅建筑均价"].ToString().Trim();
                var duplexexvprice = (int?)TryParseHelper.StrToInt32(duplexesVillaPrice);
                db.DuplexesVillaPrice = duplexexvprice;
                dr["双拼别墅建筑均价"] = duplexesVillaPrice;
                if (!string.IsNullOrEmpty(duplexesVillaPrice) && duplexexvprice == null)
                {
                    isError = true;
                    dr["双拼别墅建筑均价"] = duplexesVillaPrice + "请填写正确值#error";
                }
                else if (duplexexvprice <= 100)
                {
                    isError = true;
                    dr["双拼别墅建筑均价"] = duplexesVillaPrice + "值必须大于100#error";
                }

                var movebackavgPrice = dt.Rows[i]["回迁房建筑均价"].ToString().Trim();
                var movebackaprice = (int?)TryParseHelper.StrToInt32(movebackavgPrice);
                db.MoveBackHousePrice = movebackaprice;
                dr["回迁房建筑均价"] = movebackavgPrice;
                if (!string.IsNullOrEmpty(movebackavgPrice) && movebackaprice == null)
                {
                    isError = true;
                    dr["回迁房建筑均价"] = movebackavgPrice + "请填写正确值#error";
                }
                else if (movebackaprice <= 100)
                {
                    isError = true;
                    dr["回迁房建筑均价"] = movebackavgPrice + "值必须大于100#error";
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
                    dr["城市名称"] = dt.Rows[i]["城市名称"].ToString().Trim();
                    dr["行政区"] = dt.Rows[i]["行政区"].ToString().Trim();
                    dr["楼盘名称"] = dt.Rows[i]["楼盘名称"].ToString().Trim();
                    dr["楼栋建筑类型"] = dt.Rows[i]["楼栋建筑类型"].ToString().Trim();
                    dr["更新时间"] = dt.Rows[i]["更新时间"].ToString().Trim();
                    dr["是否过期"] = dt.Rows[i]["是否过期"].ToString().Trim();
                    dr["修改人"] = dt.Rows[i]["修改人"].ToString().Trim();
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
