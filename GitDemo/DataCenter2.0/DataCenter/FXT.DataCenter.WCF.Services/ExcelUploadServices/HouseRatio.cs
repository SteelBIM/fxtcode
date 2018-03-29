using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Infrastructure.Common.Common;
using FXT.DataCenter.Infrastructure.Common.Dictionary;
using FXT.DataCenter.Infrastructure.Common.NPOI;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.WCF.Services
{
    public partial class ExcelUpload
    {
        public void HouseRatioExcelUpload(int cityid, int fxtcompanyid, string filePath, string userid, string taskName)
        {
            var taskId = 0;

            try
            {
                //在任务列表创建一条记录 iscomplete:0,代表否；1，代表是
                var task = new DAT_ImportTask()
                {
                    TaskName = taskName,
                    ImportType = SYS_Code_Dict.批量导入类型.住宅房号系数差,
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
                var data1 = excelHelper.ExcelToDataTable("楼层差修正汇总", true);//1033003
                var data2 = excelHelper.ExcelToDataTable("朝向修正系数", true);//1033001
                var data3 = excelHelper.ExcelToDataTable("景观修正系数", true);//1033002
                var data4 = excelHelper.ExcelToDataTable("通风采光修正系数", true);//1033006
                var data5 = excelHelper.ExcelToDataTable("装修修正系数", true);//1033004
                var data6 = excelHelper.ExcelToDataTable("面积段修正系数", true);//1033005

                var dataCount = data1.Rows.Count + data2.Rows.Count + data3.Rows.Count + data4.Rows.Count + data5.Rows.Count;
                var integer = Math.Floor(Convert.ToDouble(dataCount / 50));

                //楼层差修正汇总
                List<Sys_FloorPrice> listTrue1;
                DataTable dtError1;
                DataFilter(taskId, integer, 1002001, 1033003, cityid, fxtcompanyid, data1, out listTrue1, out dtError1);
                //朝向修正系数
                List<sys_CodePrice> listTrue2;
                DataTable dtError2;
                DataFilter(taskId, integer, 2004, 1002001, 1033001, cityid, fxtcompanyid, data2, out listTrue2, out dtError2, null);
                //景观修正系数
                List<sys_CodePrice> listTrue3;
                DataTable dtError3;
                DataFilter(taskId, integer, 2006, 1002001, 1033002, cityid, fxtcompanyid, data3, out listTrue3, out dtError3, null);
                //通风采光修正系数
                List<sys_CodePrice> listTrue4;
                DataTable dtError4;
                DataFilter(taskId, integer, 1216, 1002001, 1033006, cityid, fxtcompanyid, data4, out listTrue4, out dtError4, null);
                //装修修正系数
                List<sys_CodePrice> listTrue5;
                DataTable dtError5;
                DataFilter(taskId, integer, 6026, 1002001, 1033004, cityid, fxtcompanyid, data5, out listTrue5, out dtError5, null);
                //面积段修正系数
                List<sys_CodePrice> listTrue6;
                DataTable dtError6;
                DataFilter(taskId, integer, 2003, 1002001, 1033005, cityid, fxtcompanyid, data6, out listTrue6, out dtError6, 8003);

                //错误数据写入Excel
                int errorCount = dtError1.Rows.Count + dtError2.Rows.Count + dtError3.Rows.Count + dtError4.Rows.Count + dtError5.Rows.Count + dtError6.Rows.Count;
                var fileNamePath = string.Empty;
                if (errorCount > 0)
                {
                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "房号系数差错误数据.xlsx";
                    var folder = MapPath("NeedHandledFiles/FailureData/" + fxtcompanyid.ToString().Trim());
                    fileNamePath = Path.Combine(folder, fileName);
                    Dictionary<string, DataTable> sheets = new Dictionary<string, DataTable>();
                    if (dtError1.Rows.Count > 0)
                    {
                        sheets.Add("楼层差修正汇总", dtError1);
                    }
                    if (dtError2.Rows.Count > 0)
                    {
                        sheets.Add("朝向修正系数", dtError2);
                    }
                    if (dtError3.Rows.Count > 0)
                    {
                        sheets.Add("景观修正系数", dtError3);
                    }
                    if (dtError4.Rows.Count > 0)
                    {
                        sheets.Add("通风采光修正系数", dtError4);
                    }
                    if (dtError5.Rows.Count > 0)
                    {
                        sheets.Add("装修修正系数", dtError5);
                    }
                    if (dtError6.Rows.Count > 0)
                    {
                        sheets.Add("面积段修正系数", dtError6);
                    }
                    excelHelper.CreateExcel(sheets, fileNamePath, folder);
                }

                //计算进度
                var failureNum = 0;
                var listNum = listTrue1.Count + listTrue2.Count + listTrue3.Count + listTrue4.Count + listTrue5.Count + listTrue6.Count;
                var index4True = 0;//用于统计进度

                foreach (var item in listTrue1)
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

                    var obj = _floorPrice.FindAll(cityid, fxtcompanyid).FirstOrDefault(m => m.CityId == item.CityId & m.fxtcompanyid == item.fxtcompanyid & m.StartTotalFloor == item.StartTotalFloor & m.EndTotalFloor == item.EndTotalFloor
                        & m.CurrFloor == item.CurrFloor & m.IsLift == item.IsLift);
                    if (obj != null)
                    {
                        var insertResult = _floorPrice.UpdateFloorPriceInImport(item);
                        if (insertResult <= 0) failureNum = failureNum + 1;
                    }
                    else//新增该信息
                    {
                        var insertResult = _floorPrice.AddFloorPrice(item);
                        if (insertResult <= 0) failureNum = failureNum + 1;
                    }
                }

                foreach (var item in listTrue2.Concat(listTrue3).Concat(listTrue4).Concat(listTrue5).Concat(listTrue6))
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

                    var obj = _codePrice.FindAll(cityid, fxtcompanyid).FirstOrDefault(m => m.cityid == cityid & m.fxtcompanyid == fxtcompanyid & m.codename == item.codename & m.typecode == item.typecode & m.code == item.code & m.subcode == item.subcode);
                    if (obj != null)//存在该名称则更新该信息
                    {
                        var modifyResult = _codePrice.UpdateCodePrice(obj.id, item.price.ToString());
                        if (modifyResult <= 0) failureNum = failureNum + 1;
                    }
                    else//新增该信息
                    {
                        var insertResult = _codePrice.AddCodePrice(item);
                        if (insertResult <= 0) failureNum = failureNum + 1;
                    }
                }

                //更新任务状态
                var indexPath = fileNamePath.IndexOf("NeedHandledFiles");
                var relativePath = string.Empty;
                if (indexPath >= 0)
                {
                    relativePath = fileNamePath.Substring(indexPath);
                    relativePath = relativePath.Replace(@"\", @"/");
                }
                _importTask.UpdateTask(taskId, listNum - failureNum, errorCount, 0, relativePath, 1);
            }
            catch (Exception ex)
            {
                _importTask.UpdateTask(taskId, 0, 0, 0, "", -1);
                LogHelper.WriteLog("HouseRatioExcelUpload", "", userid, cityid, fxtcompanyid, ex);
            }
        }

        private void DataFilter(int taskId, double integer, int codeId, int purposecode, int typecode, int cityId, int fxtCompanyId, DataTable dt, out List<sys_CodePrice> listTrue, out DataTable dtError, int? subcodeId = null)
        {
            listTrue = new List<sys_CodePrice>();
            dtError = new DataTable();

            foreach (DataColumn column in dt.Columns)
                dtError.Columns.Add(column.Caption);

            for (var i = 0; i < dt.Rows.Count; i++)
            {
                var dh = new sys_CodePrice();
                var dr = dtError.NewRow();
                var isError = false;

                dh.cityid = cityId;
                dh.fxtcompanyid = fxtCompanyId;
                dh.purposecode = purposecode;
                dh.typecode = typecode;

                if (dt.Columns.Contains("朝向"))
                {
                    var codename = dt.Rows[i]["朝向"].ToString().Trim();
                    int code = GetCodeByName(codename, codeId);
                    dh.codename = codename;
                    dh.code = code;
                    dr["朝向"] = codename;
                    if (string.IsNullOrEmpty(codename) || code < 1)
                    {
                        isError = true;
                        dr["朝向"] = (string.IsNullOrEmpty(codename) ? "必填" : codename) + "#error";
                    }
                }

                if (dt.Columns.Contains("景观"))
                {
                    var codename = dt.Rows[i]["景观"].ToString().Trim();
                    int code = GetCodeByName(codename, codeId);
                    dh.codename = codename;
                    dh.code = code;
                    dr["景观"] = codename;
                    if (string.IsNullOrEmpty(codename) || code < 1)
                    {
                        isError = true;
                        dr["景观"] = (string.IsNullOrEmpty(codename) ? "必填" : codename) + "#error";
                    }
                }

                if (dt.Columns.Contains("通风采光"))
                {
                    var codename = dt.Rows[i]["通风采光"].ToString().Trim();
                    int code = GetCodeByName(codename, codeId);
                    dh.codename = codename;
                    dh.code = code;
                    dr["通风采光"] = codename;
                    if (string.IsNullOrEmpty(codename) || code < 1)
                    {
                        isError = true;
                        dr["通风采光"] = (string.IsNullOrEmpty(codename) ? "必填" : codename) + "#error";
                    }
                }

                if (dt.Columns.Contains("装修"))
                {
                    var codename = dt.Rows[i]["装修"].ToString().Trim();
                    int code = GetCodeByName(codename, codeId);
                    dh.codename = codename;
                    dh.code = code;
                    dr["装修"] = codename;
                    if (string.IsNullOrEmpty(codename) || code < 1)
                    {
                        isError = true;
                        dr["装修"] = (string.IsNullOrEmpty(codename) ? "必填" : codename) + "#error";
                    }
                }

                if (dt.Columns.Contains("建筑类型"))
                {
                    var codename = dt.Rows[i]["建筑类型"].ToString().Trim();
                    int code = GetCodeByName(codename, codeId);
                    dh.codename = codename;
                    dh.code = code;
                    dr["建筑类型"] = codename;
                    if (string.IsNullOrEmpty(codename) || code < 1)
                    {
                        isError = true;
                        dr["建筑类型"] = (string.IsNullOrEmpty(codename) ? "必填" : codename) + "#error";
                    }
                }

                if (dt.Columns.Contains("面积段"))
                {
                    var subcodename = dt.Rows[i]["面积段"].ToString().Trim();
                    int subcode = GetCodeByName(subcodename, (subcodeId != null ? (int)subcodeId : 0));
                    dh.subcode = subcode;
                    dr["面积段"] = subcodename;
                    if (string.IsNullOrEmpty(subcodename) || subcode < 1)
                    {
                        isError = true;
                        dr["面积段"] = (string.IsNullOrEmpty(subcodename) ? "必填" : subcodename) + "#error";
                    }
                }

                var price = dt.Rows[i]["修正系数_百分比"].ToString().Trim();
                decimal pr = decimal.Zero;
                if (string.IsNullOrEmpty(price))
                {
                    continue;
                }
                else
                {
                    if (decimal.TryParse(price, out pr))
                    {
                        dh.price = pr;
                        dr["修正系数_百分比"] = price;
                    }
                    else
                    {
                        isError = true;
                        dr["修正系数_百分比"] = (string.IsNullOrEmpty(price) ? "必填" : price) + "#error";
                    }
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
                    dtError.Rows.Add(dr);
                }
                else
                {
                    listTrue.Add(dh);
                }
            }
        }

        private void DataFilter(int taskId, double integer, int purposecode, int typecode, int cityId, int fxtCompanyId, DataTable dt, out List<Sys_FloorPrice> listTrue, out DataTable dtError)
        {
            listTrue = new List<Sys_FloorPrice>();
            dtError = new DataTable();

            var codePrices = _codePrice.FindAll(cityId, fxtCompanyId);

            foreach (DataColumn column in dt.Columns)
                dtError.Columns.Add(column.Caption);

            for (var i = 0; i < dt.Rows.Count; i++)
            {
                var dh = new Sys_FloorPrice();
                var dr = dtError.NewRow();
                var isError = false;

                dh.CityId = cityId;
                dh.fxtcompanyid = fxtCompanyId;

                var startTotalFloor = dt.Rows[i]["总楼层开始"].ToString().Trim();
                int int1 = 0;
                if (int.TryParse(startTotalFloor, out int1))
                {
                    dr["总楼层开始"] = startTotalFloor;
                    dh.StartTotalFloor = int.Parse(startTotalFloor);
                }
                else
                {
                    isError = true;
                    dr["总楼层开始"] = (string.IsNullOrEmpty(startTotalFloor) ? "必填" : startTotalFloor) + "#error";
                }

                var endTotalFloor = dt.Rows[i]["总楼层结束"].ToString().Trim();
                int int2 = 0;
                if (int.TryParse(endTotalFloor, out int2))
                {
                    dr["总楼层结束"] = endTotalFloor;
                    dh.EndTotalFloor = int.Parse(endTotalFloor);
                }
                else
                {
                    isError = true;
                    dr["总楼层结束"] = (string.IsNullOrEmpty(endTotalFloor) ? "必填" : endTotalFloor) + "#error";
                }

                var currFloor = dt.Rows[i]["所在楼层"].ToString().Trim();
                int int3 = 0;
                if (int.TryParse(currFloor, out int3))
                {
                    dr["所在楼层"] = currFloor;
                    dh.CurrFloor = int.Parse(currFloor);
                }
                else
                {
                    isError = true;
                    dr["所在楼层"] = (string.IsNullOrEmpty(currFloor) ? "必填" : currFloor) + "#error";
                }

                var floorDifference = dt.Rows[i]["楼层差_百分比"].ToString().Trim();
                decimal decimal1 = decimal.Zero;
                if (decimal.TryParse(floorDifference, out decimal1))
                {
                    dr["楼层差_百分比"] = floorDifference;
                    dh.FloorDifference = decimal1;
                }
                else
                {
                    isError = true;
                    dr["楼层差_百分比"] = (string.IsNullOrEmpty(floorDifference) ? "必填" : floorDifference) + "#error";
                }


                var isLiftText = dt.Rows[i]["是否带电梯"].ToString().Trim();
                var isLift = YesOrNo(isLiftText);
                isLift = isLift == -1 && (isLiftText.Trim() == "0") ? 0 : isLift;    // 0  否
                isLift = isLift == -1 && (isLiftText.Trim() == "1") ? 1 : isLift;    //1   是
                dh.IsLift = isLift;
                dr["是否带电梯"] = isLiftText;
                if (!string.IsNullOrEmpty(isLiftText) && isLift == -1)
                {
                    isError = true;
                    dr["是否带电梯"] = (string.IsNullOrEmpty(isLiftText) ? "必填" : isLiftText) + "#error";
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
                    dtError.Rows.Add(dr);
                }
                else
                {
                    listTrue.Add(dh);
                }
            }
        }
    }
}
