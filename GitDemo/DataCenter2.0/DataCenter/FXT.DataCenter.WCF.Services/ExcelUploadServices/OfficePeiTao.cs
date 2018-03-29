using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Infrastructure.Common.Common;
using FXT.DataCenter.Infrastructure.Common.Dictionary;
using FXT.DataCenter.Infrastructure.Common.NPOI;


namespace FXT.DataCenter.WCF.Services
{
    public partial class ExcelUpload
    {
        public void OfficePeiTaoUpload(int cityid, int fxtcompanyid, string filePath, string userid, string taskName)
        {
            var taskId = 0;

            try
            {
                //在任务列表创建一条记录
                var task = new DAT_ImportTask()
                {
                    TaskName = taskName,
                    ImportType = SYS_Code_Dict.批量导入类型.办公商务配套,
                    CityID = cityid,
                    FXTCompanyId = fxtcompanyid,
                    CreateDate = DateTime.Now,
                    Creator = userid,
                    IsComplete = 0,
                    SucceedNumber = 0,
                    DataErrNumber = 0,
                    NameErrNumber = 0,
                    FilePath = ""
                };
                taskId = _importTask.AddTask(task);

                var excelHelper = new ExcelHandle(filePath);
                var data = excelHelper.ExcelToDataTable("Sheet1", true);

                List<DatOfficePeiTao> listTrue;//正确数据
                DataTable dtError;//格式错误数据
                DataFilter(cityid, fxtcompanyid, out listTrue, out dtError, data);

                //错误数据写入Excel
                var fileNamePath = string.Empty;
                if (dtError.Rows.Count > 0)
                {
                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "办公商务配套错误数据.xlsx";
                    var folder = MapPath("NeedHandledFiles/FailureData/" + fxtcompanyid);
                    fileNamePath = Path.Combine(folder, fileName);
                    excelHelper.CreateExcel(dtError, fileNamePath, folder);

                }

                //正确数据写入表中
                foreach (var item in listTrue)
                {
                    //var areaid = GetAreaId(cityid, item.AreaName);
                    var tenantId = GetCompanyIdByName_office(cityid, item.TenantName);
                    if (tenantId <= 0)
                    {
                        var company = new DAT_Company();
                        company.ChineseName = item.TenantName;
                        company.CreateDate = DateTime.Now;
                        company.CityId = cityid;
                        company.FxtCompanyId = fxtcompanyid;
                        _company.AddCompany(company);
                        tenantId = GetCompanyIdByName_office(cityid, item.TenantName);
                    }
                    item.TenantID = tenantId;

                    var PeiTaoId = GetPeiTaoIdByName_office(item.PeiTaoName, item.ProjectId, cityid, fxtcompanyid);
                    if (PeiTaoId <= 0)
                    {
                        item.Creators = userid;
                        _officePeiTao.AddOfficePeiTao(item);
                    }
                    else
                    {
                        item.SaveUser = userid;
                        item.PeiTaoID = PeiTaoId;
                        _officePeiTao.UpdateOfficePeiTao(item, fxtcompanyid);
                    }
                }

                //更新任务状态
                //var tmpRootDir = AppDomain.CurrentDomain.BaseDirectory;//获取程序根目录
                //var relativePath = fileNamePath.Replace(tmpRootDir, ""); //转换成相对路径
                var indexPath = fileNamePath.IndexOf("NeedHandledFiles");
                var relativePath = string.Empty;
                if (indexPath >= 0)
                {
                    relativePath = fileNamePath.Substring(indexPath);
                    relativePath = relativePath.Replace(@"\", @"/");
                }
                _importTask.UpdateTask(taskId, listTrue.Count, dtError.Rows.Count, 0, relativePath, 1);
            }

            catch (Exception ex)
            {
                _importTask.UpdateTask(taskId, 0, 0, 0, "", -1);
                LogHelper.WriteLog("OfficePeiTaoExcelUploadTask", "", userid, cityid, fxtcompanyid, ex);
            }
        }

        private void DataFilter(int cityId, int fxtCompanyId, out List<DatOfficePeiTao> listTrue, out DataTable dtError, DataTable dt)
        {
            listTrue = new List<DatOfficePeiTao>();
            dtError = new DataTable();
            foreach (DataColumn column in dt.Columns)
                dtError.Columns.Add(column.Caption);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var isSkip = false;
                var datOfficePeiTao = new DatOfficePeiTao();
                var dr = dtError.NewRow();

                datOfficePeiTao.CityId = cityId;
                datOfficePeiTao.FxtCompanyId = fxtCompanyId;
                datOfficePeiTao.CreateDate = DateTime.Now;
                datOfficePeiTao.SaveDate = DateTime.Now;

                var areaName = dt.Rows[i]["*行政区"].ToString().Trim();
                var areaId = GetAreaId(cityId, areaName);
                datOfficePeiTao.AreaName = areaName;
                datOfficePeiTao.AreaIdId = areaId;
                dr["*行政区"] = areaName;
                if (string.IsNullOrEmpty(areaName) || areaId == -1)
                {
                    isSkip = true;
                    dr["*行政区"] = areaName + "#error";
                }

                var projectName = dt.Rows[i]["*办公楼盘"].ToString().Trim();
                datOfficePeiTao.ProjectId = GetProjectIdByName_office(projectName, areaId, cityId, fxtCompanyId).FirstOrDefault();
                datOfficePeiTao.ProjectName = projectName;
                dr["*办公楼盘"] = projectName;
                if (string.IsNullOrEmpty(projectName) || datOfficePeiTao.ProjectId <= 0)
                {
                    isSkip = true;
                    dr["*办公楼盘"] = projectName + "#error";
                }

                var PeiTaoName = dt.Rows[i]["*配套名称"].ToString().Trim();
                datOfficePeiTao.PeiTaoName = PeiTaoName;
                dr["*配套名称"] = PeiTaoName;
                if (string.IsNullOrEmpty(PeiTaoName) || (!string.IsNullOrEmpty(PeiTaoName) && PeiTaoName.Length > 100))
                {
                    isSkip = true;
                    dr["*配套名称"] = PeiTaoName + "#error";
                }

                var PeiTaoCodeName = dt.Rows[i]["*配套类型"].ToString().Trim();
                var PeiTaoCode = GetCodeByName(PeiTaoCodeName, SYS_Code_Dict._办公商务配套);
                datOfficePeiTao.PeiTaoCode = PeiTaoCode;
                datOfficePeiTao.PeiTaoCodeName = PeiTaoCodeName;
                dr["*配套类型"] = PeiTaoCodeName;
                if (!string.IsNullOrEmpty(PeiTaoCodeName) && PeiTaoCode <= 0)
                {
                    isSkip = true;
                    dr["*配套类型"] = PeiTaoCodeName + "#error";
                }

                var Floor = dt.Rows[i]["*楼层"].ToString().Trim();
                datOfficePeiTao.Floor = Floor;
                dr["*楼层"] = Floor;
                if (string.IsNullOrEmpty(Floor) || (!string.IsNullOrEmpty(Floor) && Floor.Length > 20))
                {
                    isSkip = true;
                    dr["*楼层"] = Floor + "#error";
                }

                var Location = dt.Rows[i]["部位"].ToString().Trim();
                datOfficePeiTao.Location = Location;
                dr["部位"] = Location;
                if (!string.IsNullOrEmpty(Location) && Location.Length > 100)
                {
                    isSkip = true;
                    dr["部位"] = Location + "#error";
                }

                var buildingArea = dt.Rows[i]["面积_平方米"].ToString().Trim();
                var buildingAreaValue = (decimal?)TryParseHelper.StrToDecimal(buildingArea);
                datOfficePeiTao.BuildingArea = buildingAreaValue;
                dr["面积_平方米"] = buildingArea;
                if (!string.IsNullOrEmpty(buildingArea) && buildingAreaValue == null)
                {
                    isSkip = true;
                    dr["面积_平方米"] = buildingArea + "#error";
                }

                var tenantName = dt.Rows[i]["*商家名称"].ToString().Trim();
                datOfficePeiTao.TenantName = tenantName;
                dr["*商家名称"] = tenantName;
                if (string.IsNullOrEmpty(tenantName) || tenantName.Length > 200)
                {
                    isSkip = true;
                    dr["*商家名称"] = tenantName + "#error";
                }

                var remarks = dt.Rows[i]["备注"].ToString().Trim();
                datOfficePeiTao.Remarks = remarks;
                dr["备注"] = remarks;
                if (remarks.Length > 200)
                {
                    isSkip = true;
                    dr["备注"] = remarks + "#error";
                }

                if (isSkip)//该行数据中，至少有一列数据格式错误
                {
                    dtError.Rows.Add(dr);
                }
                else
                {
                    listTrue.Add(datOfficePeiTao);
                }

            }

        }
    }
}
