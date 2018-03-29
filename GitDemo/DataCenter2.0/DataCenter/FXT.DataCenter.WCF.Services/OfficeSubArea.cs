using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using FXT.DataCenter.Common.Common;
using FXT.DataCenter.Common.Dict;
using FXT.DataCenter.Entity.POCO.DataBase_FxtDataCenter;
using FXT.DataCenter.Entity.POCO.FxtDataOffice;
using FXT.DataCenter.Web.NPOI;

namespace FXT.DataCenter.Services
{
    public partial class ExcelUpload
    {
        public void OfficeSubAreaUpload(int cityid, int fxtcompanyid, string filePath, string userid, string taskName)
        {
            try
            {
                //在任务列表创建一条记录
                var task = new DAT_ImportTask()
                {
                    TaskName = taskName,
                    ImportType = SYS_Code_Dict.批量导入类型.办公商务中心,
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
                var taskid = _importTask.AddTask(task);

                var excelHelper = new ExcelHandle(filePath);
                var data = excelHelper.ExcelToDataTable("Sheet1", true);

                List<SYS_SubArea_Office> listTrue;//正确数据
                DataTable dtError;//格式错误数据
                DataFilter(cityid, fxtcompanyid, out listTrue, out dtError, data);

                //错误数据写入Excel
                var fileNamePath = string.Empty;
                if (dtError.Rows.Count > 0)
                {
                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "办公商务中心错误数据.xlsx";
                    var folder = MapPath("Excel/Office/FailureData/" + fxtcompanyid);
                    fileNamePath = Path.Combine(folder, fileName);
                    excelHelper.CreateExcel(dtError, fileNamePath, folder);

                }

                //正确数据写入表中
                foreach (var item in listTrue)
                {
                    var areaid = GetAreaId(cityid, item.AreaName);
                    var subAreaId = GetSubAreaIdByName_office(item.SubAreaName, areaid, fxtcompanyid);

                    if (subAreaId <= 0)
                    {
                        _officeSubArea.AddSubArea(item);
                    }
                    else
                    {
                        item.SubAreaId = subAreaId;
                        _officeSubArea.UpdateSubArea(item);
                    }
                }

                //更新任务状态
                var tmpRootDir = AppDomain.CurrentDomain.BaseDirectory;//获取程序根目录
                var relativePath = fileNamePath.Replace(tmpRootDir, ""); //转换成相对路径
                relativePath = relativePath.Replace(@"\", @"/");
                //imagesurl2 = imagesurl2.Replace(@"Aspx_Uc/", @"");
                _importTask.UpdateTask(taskid, listTrue.Count, dtError.Rows.Count, 0, relativePath, 1);
            }

            catch (Exception ex)
            {
                LogHelper.WriteLog("OfficeSubAreaExcelUploadTask", "", userid, cityid, fxtcompanyid, ex);
            }
        }

        private void DataFilter(int cityId, int fxtCompanyId, out List<SYS_SubArea_Office> listTrue, out DataTable dtError, DataTable dt)
        {
            listTrue = new List<SYS_SubArea_Office>();
            dtError = new DataTable();
            foreach (DataColumn column in dt.Columns)
                dtError.Columns.Add(column.Caption);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var isSkip = false;
                var datOfficeSubArea = new SYS_SubArea_Office();
                var dr = dtError.NewRow();

                datOfficeSubArea.FxtCompanyId = fxtCompanyId;
                datOfficeSubArea.CreateDate = DateTime.Now;
                datOfficeSubArea.SaveDate = DateTime.Now;

                var areaName = dt.Rows[i]["*行政区"].ToString().Trim();
                var areaId = GetAreaId(cityId, areaName);
                datOfficeSubArea.AreaId = areaId;
                datOfficeSubArea.AreaName = areaName;
                dr["*行政区"] = areaName;
                if (string.IsNullOrEmpty(areaName) || areaId == -1)
                {
                    isSkip = true;
                    dr["*行政区"] = areaName + "#error";
                }

                var subAreaName = dt.Rows[i]["*办公商务中心"].ToString().Trim();
                datOfficeSubArea.SubAreaName = subAreaName;
                dr["*办公商务中心"] = subAreaName;
                if (string.IsNullOrEmpty(subAreaName) || (!string.IsNullOrEmpty(subAreaName) && subAreaName.Length > 30))
                {
                    isSkip = true;
                    dr["*办公商务中心"] = subAreaName + "#error";
                }

                var TypeCodeName = dt.Rows[i]["商务中心等级"].ToString().Trim();
                var TypeCode = GetCodeByName(TypeCodeName, SYS_Code_Dict._办公区域等级);
                datOfficeSubArea.TypeCode = TypeCode;
                datOfficeSubArea.TypeCodeName = TypeCodeName;
                dr["商务中心等级"] = TypeCodeName;
                if (!string.IsNullOrEmpty(TypeCodeName) && TypeCode <= 0)
                {
                    isSkip = true;
                    dr["商务中心等级"] = TypeCodeName + "#error";
                }

                var AreaLine = dt.Rows[i]["环线"].ToString().Trim();
                datOfficeSubArea.AreaLine = AreaLine;
                dr["环线"] = AreaLine;
                if (!(string.IsNullOrEmpty(AreaLine)) && AreaLine.Length > 30)
                {
                    isSkip = true;
                    dr["环线"] = AreaLine + "#error";
                }

                var Details = dt.Rows[i]["描述"].ToString().Trim();
                datOfficeSubArea.Details = Details;
                dr["描述"] = Details;
                if (!(string.IsNullOrEmpty(Details)) && Details.Length > 600)
                {
                    isSkip = true;
                    dr["描述"] = Details + "#error";
                }
                
                var X = dt.Rows[i]["经度"].ToString().Trim();
                var XValue = (decimal?)TryParseHelper.StrToDecimal(X);
                datOfficeSubArea.X = XValue;
                dr["经度"] = X;
                if (!string.IsNullOrEmpty(X) && XValue == null)
                {
                    isSkip = true;
                    dr["经度"] = X + "#error";
                }

                var Y = dt.Rows[i]["纬度"].ToString().Trim();
                var YValue = (decimal?)TryParseHelper.StrToDecimal(Y);
                datOfficeSubArea.Y = YValue;
                dr["纬度"] = Y;
                if (!string.IsNullOrEmpty(Y) && YValue == null)
                {
                    isSkip = true;
                    dr["纬度"] = Y + "#error";
                }

                var XYScale = dt.Rows[i]["经纬度比例"].ToString().Trim();
                var XYScaleValue = (int?)TryParseHelper.StrToDecimal(XYScale);
                datOfficeSubArea.XYScale = XYScaleValue;
                dr["经纬度比例"] = XYScale;
                if (!string.IsNullOrEmpty(XYScale) && XYScaleValue == null)
                {
                    isSkip = true;
                    dr["经纬度比例"] = XYScale + "#error";
                }

                if (isSkip)//该行数据中，至少有一列数据格式错误
                {
                    dtError.Rows.Add(dr);
                }
                else
                {
                    listTrue.Add(datOfficeSubArea);
                }

            }

        }
    }
}
