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
        public void LandPriceExcelUpload(int cityid, int fxtcompanyid, string filePath, string userid, string taskName)
        {
            var taskId = -1;
            try
            {
                //在任务列表创建一条记录
                var task = new DAT_ImportTask()
                {
                    TaskName = taskName,
                    ImportType = SYS_Code_Dict.批量导入类型.土地信息,
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
                List<DAT_Land_BasePrice> listTrue;//正确数据
                DataTable dtError;//格式错误数据
                DataFilter(cityid, fxtcompanyid, out listTrue, out dtError, data);
                //错误数据写入excel
                string fileNamePath = string.Empty;
                if (dtError.Rows.Count > 0)
                {
                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "土地基准地价格式错误数据.xlsx";
                    var folder = MapPath("NeedHandledFiles/FailureData/" + fxtcompanyid.ToString().Trim());
                    fileNamePath = Path.Combine(folder, fileName);
                    excelHelper.CreateExcel(dtError, fileNamePath, folder);

                }
                //正确数据添加到数据表
                listTrue.ForEach(m => _datLandPrice.AddLand_BasePrice(m));

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
                LogHelper.WriteLog("LandPriceExcelUpload", "", userid, cityid, fxtcompanyid, ex);
            }
        }

        private void DataFilter(int cityId, int fxtCompanyId, out List<DAT_Land_BasePrice> listTrue, out DataTable dtError, DataTable dt)
        {
            listTrue = new List<DAT_Land_BasePrice>();
            dtError = new DataTable();

            foreach (DataColumn column in dt.Columns)
                dtError.Columns.Add(column.Caption);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                bool IsSkip = false;
                var land = new DAT_Land_BasePrice();
                var dr = dtError.NewRow();
                land.fxtcompanyid = fxtCompanyId;
                land.cityid = cityId;

                var areaName = dt.Rows[i]["行政区"].ToString().Trim();
                var areaId = GetAreaId(cityId, areaName);
                land.areaid = areaId > 0 ? areaId : -1;
                land.AreaName = areaName;
                dr["行政区"] = areaName;
                if (!string.IsNullOrEmpty(areaName) && areaId == -1)
                {
                    IsSkip = true;
                    dr["行政区"] = areaName + "#error";
                }

                var subAreaName = dt.Rows[i]["片区"].ToString().Trim();
                var subAreaId = SubAreaIdByName(subAreaName, areaId);
                land.subareaid = subAreaId;
                land.SubAreaName = subAreaName;
                dr["片区"] = subAreaName;
                if (!string.IsNullOrEmpty(subAreaName) && subAreaId == -1)
                {
                    IsSkip = true;
                    dr["片区"] = subAreaName + "#error";
                }

                var purposeName = dt.Rows[i]["*土地用途"].ToString().Trim();
                var purposeCode = GetCodeByName(purposeName, SYS_Code_Dict._土地用途);
                land.purposecode = purposeCode;
                dr["*土地用途"] = purposeName;
                if (string.IsNullOrEmpty(purposeName) || purposeCode == -1)
                {
                    IsSkip = true;
                    dr["*土地用途"] = purposeName + "#error";
                }

                var landClassName = dt.Rows[i]["*土地等级"].ToString().Trim();
                var landClassCode = GetCodeByName(landClassName, SYS_Code_Dict._土地等级);
                land.landclass = landClassCode;
                dr["*土地等级"] = landClassName;
                if (string.IsNullOrEmpty(landClassName) || landClassCode == -1)
                {
                    IsSkip = true;
                    dr["*土地等级"] = landClassName + "#error";
                }

                var landunitprice_avg = dt.Rows[i]["土地平均单价_元/平方米"].ToString().Trim();
                land.landunitprice_avg = (decimal?)TryParseHelper.StrToDecimal(landunitprice_avg);
                dr["土地平均单价_元/平方米"] = landunitprice_avg;
                if (!string.IsNullOrEmpty(landunitprice_avg) && TryParseHelper.StrToDecimal(landunitprice_avg) == null)
                {
                    IsSkip = true;
                    dr["土地平均单价_元/平方米"] = landunitprice_avg + "#error";
                }

                var landunitprice_min = dt.Rows[i]["土地最低单价_元/平方米"].ToString().Trim();
                land.landunitprice_min = (decimal?)TryParseHelper.StrToDecimal(landunitprice_min);
                dr["土地最低单价_元/平方米"] = landunitprice_min;
                if (!string.IsNullOrEmpty(landunitprice_min) && TryParseHelper.StrToDecimal(landunitprice_min) == null)
                {
                    IsSkip = true;
                    dr["土地最低单价_元/平方米"] = landunitprice_min + "#error";
                }

                var landunitprice_max = dt.Rows[i]["土地最高单价_元/平方米"].ToString().Trim();
                land.landunitprice_max = (decimal?)TryParseHelper.StrToDecimal(landunitprice_max);
                dr["土地最高单价_元/平方米"] = landunitprice_max;
                if (!string.IsNullOrEmpty(landunitprice_max) && TryParseHelper.StrToDecimal(landunitprice_max) == null)
                {
                    IsSkip = true;
                    dr["土地最高单价_元/平方米"] = landunitprice_max + "#error";
                }

                var buildingunitprice_avg = dt.Rows[i]["建面平均地价_元/平方米"].ToString().Trim();
                land.buildingunitprice_avg = (decimal?)TryParseHelper.StrToDecimal(buildingunitprice_avg);
                dr["建面平均地价_元/平方米"] = buildingunitprice_avg;
                if (!string.IsNullOrEmpty(buildingunitprice_avg) && TryParseHelper.StrToDecimal(buildingunitprice_avg) == null)
                {
                    IsSkip = true;
                    dr["建面平均地价_元/平方米"] = buildingunitprice_avg + "#error";
                }


                var buildingunitprice_min = dt.Rows[i]["建面最低地价_元/平方米"].ToString().Trim();
                land.buildingunitprice_min = (decimal?)TryParseHelper.StrToDecimal(buildingunitprice_min);
                dr["建面最低地价_元/平方米"] = buildingunitprice_min;
                if (!string.IsNullOrEmpty(buildingunitprice_min) && TryParseHelper.StrToDecimal(buildingunitprice_min) == null)
                {
                    IsSkip = true;
                    dr["建面最低地价_元/平方米"] = buildingunitprice_min + "#error";
                }

                var buildingunitprice_max = dt.Rows[i]["建面最高地价_元/平方米"].ToString().Trim();
                land.buildingunitprice_max = (decimal?)TryParseHelper.StrToDecimal(buildingunitprice_max);
                dr["建面最高地价_元/平方米"] = buildingunitprice_max;
                if (!string.IsNullOrEmpty(buildingunitprice_max) && TryParseHelper.StrToDecimal(buildingunitprice_max) == null)
                {
                    IsSkip = true;
                    dr["建面最高地价_元/平方米"] = buildingunitprice_max + "#error";
                }

                var pricedate = dt.Rows[i]["*地价公布日期"].ToString().Trim();
                land.pricedate = (DateTime?)TryParseHelper.StrToDateTime(pricedate);
                dr["*地价公布日期"] = pricedate;
                if (string.IsNullOrEmpty(pricedate) || TryParseHelper.StrToDateTime(pricedate) == null)
                {
                    IsSkip = true;
                    dr["*地价公布日期"] = pricedate + "#error";
                }

                var documentNo = dt.Rows[i]["发布公文号"].ToString().Trim();
                land.DocumentNo = documentNo;
                dr["发布公文号"] = documentNo;
                if (!string.IsNullOrEmpty(documentNo) && documentNo.Length > 128)
                {
                    IsSkip = true;
                    dr["发布公文号"] = documentNo + "#error";
                }
                
                var remarks = dt.Rows[i]["备注"].ToString().Trim();
                land.Remarks = remarks;
                dr["备注"] = remarks;
                if (!string.IsNullOrEmpty(remarks) && remarks.Length > 500)
                {
                    IsSkip = true;
                    dr["备注"] = remarks + "#error";
                }

                if (IsSkip)//该行数据中，至少有一列数据格式错误
                {
                    dtError.Rows.Add(dr);
                }
                else
                {
                    listTrue.Add(land);
                }

            }

        }
    }
}
