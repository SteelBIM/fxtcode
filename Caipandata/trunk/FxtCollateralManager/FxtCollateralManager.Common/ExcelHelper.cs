#region 引用命名空间
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.IO;
using System.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Xml;
using System.Text.RegularExpressions;
using FxtCollateralManager.Common.FxtAPI;
using FxtNHibernate.DATProjectDomain.Entities;
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using FxtNHibernate.DTODomain.FxtLoanDTO;
using FxtCommonLibrary.LibraryExcel;
using FxtCommonLibrary.LibraryUtils;
using FxtNHibernate.FxtLoanDomain.Entities;
using System.Reflection;
#endregion

#region  操作记录
/** 
 * 作者:李晓东
 * 摘要:2014.01.21 新建类并创建ExcelColumns方法
 *      2014.01.22 修改人:李晓东
 *                 新增ExcelStandardization
 *      2014.01.26 修改人:李晓东
 *                 完善xml中拆分细节
 *      2014.02.07 修改人:李晓东
 *                 修改CustomXmlValue 加了判断
 *      2014.02.11 修改人:李晓东
 *                 新增WindowThreadProcess 类,主要作用是关闭指定进程
 *      2014.02.12 修改人:李晓东
 *                 新增 PCADetect OneLtTwoOrTwoLtOne PCADetectVal 处理押品名称和押品地址是否合并
 *      2014.02.13 修改人:李晓东
 *                 修改:xml
 *      2014.02.17 修改人:李晓东
 *                 修改:ExcelColumns
 *                 删除:ExcelHelper中的部分无需要方法
 *                 新增:Excels做到公用,WorkbookWrite写Excel方法
 *      2014.02.18 修改人:李晓东
 *                 新增:ExcelResolve 对Excel解析
 *                 修改:ReadExcel 被调用时更方便的使用
 *      2014.02.19 修改人:李晓东
 *                 修改:ExcelResolve导入时开启多个Excel进程的问题
 *                 整理ExcelHepler类,把关键的相关操作组件类提取到公共用法中,移到FxtCommonLibrary类库中LibraryExcel目录下
 *      2014.02.24 修改人:李晓东
 *                 修改:1.Excel读取方式,解决Excel无法在进程中退出的问题
 *                      2.把每个获得区域、楼盘、楼栋、房号分开提出来,让代码更易读取
 *                      3.ExcelResolve()导入中加了再次匹配库中信息   zxc
 *      2014.02.26 修改人:李晓东
 *                 新增:GetExcelCount获得文件内容总数
 *      2014.06.11 新增以Task开头的方法,方便在任务中执行
 *      
 *      2014-06-20 修改人:贺黎亮
 *                 新增ExcelToXml导出Excel方法，直接输出字符，快速导出。
 * **/
#endregion
namespace FxtCollateralManager.Common
{
    public class ExcelHelper
    {
        string excelFile = string.Empty,
            //工作薄名称
            worsheetName = string.Empty;
        /// <summary>
        /// 总记录数
        /// </summary>
        int count = 0;
        object result = null;

        #region 类声明
        public ExcelHelper()
        {
        }
        public ExcelHelper(string file)
        {
            excelFile = file;
        }
        #endregion

        #region Excel 读写后相关数据封装、操作
        /// <summary>
        /// 获得Excel列头
        /// </summary>
        /// <returns></returns>
        public string ExcelColumns()
        {
            Dictionary<int, string> list = new Dictionary<int, string>();
            try
            {
                object[,] arryItem = ReadExcel(1, 1);
                if (arryItem != null)
                {
                    int i = 0;
                    while (i < arryItem.Length)
                    {
                        list.Add(i, arryItem[0, i].ToString());
                        i++;
                    }
                }
            }
            catch (Exception exe)
            {
                return exe.Message;
            }
            return Utils.Serialize(list);
        }

        /// <summary>
        /// 获得Excel中的头部文件
        /// </summary>
        /// <param name="etmPath">匹对实体XML文件路径</param>
        /// <returns></returns>
        public Dictionary<int, string> TaskExcelColumns(string etmPath)
        {
            Dictionary<int, string> list = new Dictionary<int, string>();
            try
            {
                object[,] arryItem = ReadExcel(1, 1);
                if (arryItem != null)
                {
                    int i = 0, j = 0;

                    XmlDocument document = new XmlDocument();
                    document.Load(etmPath);
                    XmlNodeList xmlnodeList = document.ChildNodes.Item(1).ChildNodes;
                    while (i < arryItem.Length)
                    {
                        j = 0;
                        while (j < xmlnodeList.Count)
                        {
                            XmlNode xmlNode = xmlnodeList.Item(j);
                            if (xmlNode.InnerText.Trim().Equals(arryItem[0, i].ToString().Trim()))
                                list.Add(i, xmlNode.Attributes["Value"].Value);
                            j++;
                        }
                        i++;
                    }
                }
            }
            catch
            {

            }
            return list;
        }

        public object TaskExcelStandardization(object[,] arryItem, Dictionary<int, string> listHeader, int Index)
        {
            DataCollateral model = new DataCollateral();
            try
            {
                foreach (var item in listHeader)
                {
                    string readerVale = arryItem[Index, item.Key].ToString();
                    if (readerVale.Equals("0")) readerVale = string.Empty;
                    PropertyInfo property = model.GetType().GetProperty(item.Value);

                    if (!property.PropertyType.IsGenericType)
                    {
                        //非泛型
                        property.SetValue(model, Convert.ChangeType(readerVale, property.PropertyType), null);
                    }
                    else
                    {
                        //泛型Nullable<>
                        Type genericTypeDefinition = property.PropertyType.GetGenericTypeDefinition();
                        if (genericTypeDefinition == typeof(Nullable<>))
                        {
                            property.SetValue(model, Convert.ChangeType(readerVale, Nullable.GetUnderlyingType(property.PropertyType)), null);
                        }
                    }

                }
                //押品类型
                if (!Utils.IsNullOrEmpty(model.PurposeName))
                {
                    List<SYSCode> listCode = null;
                    using (FxtAPIClient client = new FxtAPIClient())
                    {
                        JObject obj = new JObject();
                        obj.Add("id", 8007);
                        result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                        "D", "GetSYSCodeByID", Utils.Serialize(obj));
                        listCode = Utils.Deserialize<List<SYSCode>>(result.ToString());
                    }
                    if (listCode != null)
                    {
                        var code = listCode.Where(item => item.CodeName.Equals(PurposeCodeNameReplace(model.PurposeName))).FirstOrDefault();
                        model.PurposeCode = code != null ? code.Code : 0;
                    }
                }
                //押品面积
                if (model.BuildingArea > 0)
                {
                    List<SYSCode> listCode = null;
                    using (FxtAPIClient client = new FxtAPIClient())
                    {
                        JObject obj = new JObject();
                        obj.Add("id", 8003);
                        result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                        "D", "GetSYSCodeByID", Utils.Serialize(obj));
                        listCode = Utils.Deserialize<List<SYSCode>>(result.ToString());
                    }
                    if (listCode != null)
                    {
                        var code = listCode.Where(item => item.CodeName.Equals(BuildingArea(model.BuildingArea))).FirstOrDefault();
                        model.BuildingAreaCode = code != null ? code.Code : 0;
                    }
                }
                TaskXml(model);
                return model;
            }
            catch (Exception exe)
            {
                return string.Format("押品编号:\"{0}\",押品名称:\"{1}\",押品地址:\"{2}\",因{3},无法拆分",
                    model.Number, model.Name, model.Address, exe.Message);
            }

        }

        /// <summary>
        /// 押品类型相关字符替换
        /// </summary>
        /// <param name="name">押品类型原名称</param>
        /// <returns></returns>
        private string PurposeCodeNameReplace(string name)
        {
            name = name.Replace("商铺", "商业")
                .Replace("居住", "住宅");
            return name;
        }

        /// <summary>
        /// 押品面积类型
        /// </summary>
        /// <param name="area">押品面积</param>
        /// <returns></returns>
        private string BuildingArea(decimal area)
        {
            string result = string.Empty;
            if (area < 30) {
                result = "小于30";
            }
            else if (area >= 30 && area <60)
            {
                result = "30~60";
            }
            else if (area >= 60 && area < 90)
            {
                result = "60~90";
            }
            else if (area >= 90 && area < 120)
            {
                result = "90~120";
            }
            else if (area >= 120 && area < 144)
            {
                result = "120~144";
            }
            else if (area >= 144 && area < 180)
            {
                result = "144~180";
            }
            else if (area >= 180 && area < 250)
            {
                result = "180~250";
            }
            else if (area >= 250 && area <= 350)
            {
                result = "250~350";
            }
            else if (area >350)
            {
                result = "大于350";
            }
            return result;
        }

        /// <summary>
        /// 拆分标准化
        /// </summary>
        /// <param name="list">列集合</param>
        /// <param name="pageSize">一页记录大小</param>
        /// <param name="pageIndex">当前页</param>
        /// <returns></returns>
        public object ExcelStandardization(Dictionary<int, string> list, int pageSize, int pageIndex, bool IsList = false)
        {
            //List<string[]> listArray = new List<string[]>();
            //XmlDocument document = new XmlDocument();
            //string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SplitReadColumn.xml");
            //document.Load(path);
            //XmlNodeList xmlnodeList = document.ChildNodes.Item(1).ChildNodes;
            //for (int i = 0; i < xmlnodeList.Count; i++)
            //{
            //    XmlNode xmlNode = xmlnodeList.Item(i);
            //    listArray.Add(new string[] { 
            //        xmlNode.Attributes["cindex"].Value,
            //        xmlNode.Attributes["cname"].Value,
            //        xmlNode.Attributes["ctype"].Value,
            //        xmlNode.InnerText
            //    });
            //}

            List<DataCollaterals> listDataCollateral = new List<DataCollaterals>();

            object[,] arryItem = null;
            //拆分全部
            if (pageSize.Equals(0) && pageIndex.Equals(0))
            {
                arryItem = ReadExcel(pageIndex, pageSize, false);
            }
            else//拆分部分
            {
                arryItem = ReadExcel(pageIndex, pageSize, true);
                count = count - 2;
            }
            int rowCount = arryItem.GetLength(0);//,
            //colCount = arryItem.GetLength(1);
            if (arryItem != null)
            {
                //遍历数据
                for (int i = 0; i < rowCount; i++)
                {
                    DataCollaterals model = new DataCollaterals();
                    string splitValue = string.Empty;
                    foreach (var item in list)
                    {
                        string readerVale = arryItem[i, item.Key].ToString();
                        if (readerVale.Equals("0")) readerVale = string.Empty;
                        switch (item.Value)
                        {
                            case "押品编号":
                                model.Number = readerVale;
                                break;
                            case "分行":
                                model.Branch = readerVale;
                                break;
                            case "押品类型":
                                model.PurposeName = readerVale;
                                break;
                            case "押品名称":
                                model.Name = readerVale;
                                break;
                            case "面积":
                                model.BuildingArea = !Utils.IsNullOrEmpty(readerVale) ? decimal.Parse(readerVale) : 0;
                                break;
                            case "押品地址":
                                model.Address = readerVale;
                                break;
                        }
                    }
                    //foreach (var item in listArray)
                    //{
                    //    object readerVale = arryItem[i, int.Parse(item[0].ToString())];
                    //    if (item[2].ToString().Equals("decimal"))
                    //    {
                    //        readerVale = decimal.Parse(readerVale.ToString());
                    //    }
                    //    else if (item[2].ToString().Equals("datetime"))
                    //    {
                    //        readerVale = DateTime.Parse(readerVale.ToString());
                    //    }
                    //    model.GetType().GetProperty(item[1].ToString()).SetValue(model, readerVale, null);
                    //}
                    DataCollateral getModel = null;
                    using (FxtAPIClient client = new FxtAPIClient())
                    {
                        JObject obj = new JObject();
                        obj.Add("dataCollateral", Utils.Serialize(model));
                        result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                            "C", "GetDataCollateralByMoreWhere", Utils.Serialize(obj));

                        getModel = Utils.Deserialize<DataCollateral>(Utils.GetJObjectValue(result, "data"));

                        if (getModel != null)
                        {
                            model.Id = getModel.Id;
                            model.ProvinceId = getModel.ProvinceId;
                            model.CityId = getModel.CityId;
                            model.AreaId = getModel.AreaId;
                            model.SubAreaId = getModel.SubAreaId;
                            model.BuildingId = getModel.BuildingId;
                            model.RoomId = getModel.RoomId;

                            model.Road = getModel.Road;
                            model.RoadNumber = getModel.RoadNumber;

                            List<DATProject> _datproject = GetDatProject("", model.CityId, 1,
                                    string.Format("{0}{1}", model.Road, model.RoadNumber));
                            var datProject = new DATProject();
                            //根据地址去查找是否能够匹配上临时库和正式库的楼盘信息
                            if (_datproject != null && _datproject.Count() == 1)
                            {
                                datProject = _datproject.FirstOrDefault();
                                model.ProjectName = datProject.ProjectName;
                                model.ProjectId = datProject.ProjectId;
                                //楼盘匹配
                                if (!model.ProjectId.Equals(0))
                                {
                                    model.ProjectNameMatch = true;
                                }
                            }
                            else
                            {
                                model.ProjectName = getModel.ProjectName;
                                //楼盘匹配
                                if (!getModel.ProjectId.Equals(0))
                                {
                                    model.ProjectId = getModel.ProjectId;
                                    model.ProjectNameMatch = true;
                                }
                            }
                            model.ProjectAddress = getModel.ProjectAddress;
                            model.Installment = getModel.Installment;
                            model.ProjectType = getModel.ProjectType;

                            model.BuildingNumber = getModel.BuildingNumber;
                            //model.BuildingNumberMatch = true;
                            //楼栋匹配
                            if (getModel.BuildingId != null && !getModel.BuildingId.Value.Equals(0))
                            {
                                model.BuildingId = getModel.BuildingId;
                                model.BuildingNumberMatch = true;
                            }
                            else
                            {
                                //匹配楼栋 楼栋编号加楼盘编号加城市ID
                                DATBuilding datBuilding = GetDATBuilding(datProject.ProjectId,
                                    datProject.CityID, model.BuildingNumber);
                                if (datBuilding != null && !datBuilding.BuildingId.Equals(0))
                                {
                                    model.BuildingId = datBuilding.BuildingId;
                                    model.BuildingNumberMatch = true;
                                }
                            }
                            model.FloorNumber = getModel.FloorNumber;
                            model.RoomNumber = getModel.RoomNumber;
                            //房号匹配
                            if (getModel.RoomId != null && !getModel.RoomId.Value.Equals(0))
                            {
                                model.RoomNumberMatch = true;
                            }
                            else
                            {
                                string RoomVal = model.RoomNumber, FloorVal = model.FloorNumber;
                                if (RoomVal != null)
                                    RoomVal = RoomVal.Replace("房号", "")
                                        .Replace("号房", "")
                                        .Replace("室", "");
                                if (FloorVal != null)
                                    FloorVal = FloorVal.Replace("层", "")
                                        .Replace("楼", "");
                                //匹配房号
                                if (model.BuildingId != null)
                                {
                                    DATHouse datHouse = GetDATHouse(datProject.CityID, model.BuildingId.Value, RoomVal);
                                    if (datHouse != null
                                        && !Utils.IsNullOrEmpty(datHouse.HouseName)
                                        && datHouse.HouseName.Equals(RoomVal))
                                    {
                                        model.RoomNumber = datHouse.HouseName;
                                        model.RoomNumberMatch = true;
                                    }
                                    if (datHouse != null && datHouse.FloorNo.Equals(FloorVal))
                                    {
                                        model.FloorNumber = datHouse.FloorNo.ToString();
                                        model.FloorNumberMatch = true;
                                    }
                                }
                            }
                            if (model.ProjectNameMatch && model.BuildingNumberMatch && model.RoomNumberMatch)
                                model.MatchStatus = 0;
                            //省份
                            SYSProvince sysp = GetSYSProvince(null, 0, model.ProvinceId);
                            if (sysp != null)
                                model.ProvinceName = sysp.ProvinceName;
                            //城市
                            SYSCity sysc = GetSysCity(0, model.CityId);
                            if (sysc != null)
                                model.CityName = sysc.CityName;
                            //行政区GetAreaByAreaId
                            SYSArea sysa = GetSysArea(model.AreaId);
                            if (sysa != null)
                                model.AreaName = sysa.AreaName;
                        }
                        else
                        {
                            xml(model);
                        }
                    }
                    listDataCollateral.Add(model);
                }
            }
            if (pageSize.Equals(0) && pageIndex.Equals(0) || IsList)
            {
                return listDataCollateral;
            }
            else
            {
                return new
                {
                    data = listDataCollateral,
                    count = count
                };
            }
        }

        /// <summary>
        /// Excel解析
        /// </summary>
        /// <returns></returns>
        public object ExcelResolve()
        {
            string pageIndex = string.Empty;
            List<DataCollaterals> list = null;
            object[,] arryItem = ReadExcel(0, 0);
            if (arryItem != null)
            {
                int rowCount = arryItem.GetLength(0),
                colCount = arryItem.GetLength(1);
                if (rowCount > 0)
                {
                    pageIndex = worsheetName;
                    list = new List<DataCollaterals>();
                    try
                    {
                        for (int i = 0; i < rowCount; i++)
                        {
                            DataCollaterals model = new DataCollaterals();
                            model.Number = Utils.ObjectIsNull(arryItem[i, 0]);
                            using (FxtAPIClient client = new FxtAPIClient())
                            {
                                JObject _job = new JObject();
                                _job.Add("collNumber", model.Number);
                                var dataColl = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                                    "C", "GetDataCollateralByNumber", Utils.Serialize(_job));
                                if (Utils.GetJObjectValue(dataColl, "type").Equals("1"))
                                {
                                    DataCollateral modelColl = Utils.Deserialize<DataCollateral>(Utils.GetJObjectValue(dataColl, "data"));
                                    model.Branch = modelColl.Branch;
                                    model.PurposeCode = modelColl.PurposeCode;
                                    model.Name = modelColl.Name;
                                    model.BuildingArea = modelColl.BuildingArea;
                                    model.Address = modelColl.Address;
                                    model.BuildingId = modelColl.BuildingId;
                                    model.ProjectId = modelColl.ProjectId;
                                    model.RoomId = modelColl.RoomId;
                                    model.Id = modelColl.Id;
                                }
                                else
                                {
                                    model.Branch = Utils.ObjectIsNull(arryItem[i, 1]);
                                    model.PurposeName = Utils.ObjectIsNull(arryItem[i, 2]);
                                    model.Name = Utils.ObjectIsNull(arryItem[i, 3]);

                                    string strBuildingArea = Utils.ObjectIsNull(arryItem[i, 4]);
                                    model.BuildingArea =
                                        Utils.IsNullOrEmpty(strBuildingArea) ?
                                        0 :
                                        Convert.ToDecimal(strBuildingArea);

                                    model.Address = Utils.ObjectIsNull(arryItem[i, 5]);
                                }
                            }


                            model.ProvinceName = Utils.ObjectIsNull(arryItem[i, 6]);
                            model.CityName = Utils.ObjectIsNull(arryItem[i, 7]);
                            model.AreaName = Utils.ObjectIsNull(arryItem[i, 8]);

                            //省份
                            if (!Utils.IsNullOrEmpty(model.ProvinceName))
                            {
                                SYSProvince sysProvince = GetSYSProvince(model.ProvinceName);
                                model.ProvinceId = sysProvince.ProvinceId;
                            }
                            //城市
                            if (!model.ProvinceId.Equals(0))
                            {
                                var sysCity = GetSysCity(model.ProvinceId, null);

                                var city = sysCity.Count == 1 ? sysCity.FirstOrDefault() : sysCity
                                    .Where(sc => sc.CityName.Equals(model.CityName))
                                    .FirstOrDefault();

                                model.CityId = city != null ? city.CityId : 0;
                            }
                            //行政区
                            if (!model.CityId.Equals(0))
                            {
                                var sysArea = GetSysArea(null, model.CityId);

                                var area = sysArea.Count == 1 ? sysArea.FirstOrDefault() : sysArea
                                    .Where(sa => sa.AreaName.Equals(model.AreaName))
                                    .FirstOrDefault();
                                model.AreaId = area != null ? area.AreaId : 0;
                            }


                            model.Road = Utils.ObjectIsNull(arryItem[i, 11]);
                            model.RoadNumber = Utils.ObjectIsNull(arryItem[i, 12]);
                            DATProject datProject = null;
                            List<DATProject> listProject = null;
                            if (model.Id == 0)//不是从库里读取出来的
                            {
                                //导入地址匹配
                                listProject = GetDatProject("", model.CityId, 1,
                                                string.Format("{0}{1}", model.Road, model.RoadNumber));
                                if (listProject.Count() == 1)
                                {
                                    datProject = listProject.FirstOrDefault();
                                    model.ProjectName = Utils.DecodeField(datProject.ProjectName);
                                    model.ProjectAddress = datProject.Address;
                                }
                            }
                            model.ProjectName = Utils.ObjectIsNull(arryItem[i, 9]);
                            if (model.Id == 0)//不是从库里读取出来的
                            {
                                //导入楼盘匹配
                                if (datProject == null)
                                {
                                    listProject = GetDatProject(model.ProjectName, model.CityId);
                                    if (list.Count() == 1)
                                        datProject = listProject.FirstOrDefault();
                                }
                            }
                            model.Installment = Utils.ObjectIsNull(arryItem[i, 10]);
                            if (model.Id == 0)//不是从库里读取出来的
                            {
                                //导入分期匹配
                                if (datProject == null)
                                {
                                    listProject = GetDatProject(string.Format("{0}{1}", model.ProjectName, model.Installment),
                                        model.CityId);
                                    if (listProject.Count() == 1)
                                        datProject = listProject.FirstOrDefault();
                                }
                                if (datProject != null)
                                {
                                    model.ProjectName = Utils.DecodeField(datProject.ProjectName);
                                    model.ProjectId = datProject.ProjectId;//有则,表示关联上
                                    model.ProjectNameMatch = true;
                                }
                            }
                            else
                            {
                                //楼盘匹配
                                if (!model.ProjectId.Equals(0))
                                {
                                    model.ProjectNameMatch = true;
                                }
                            }
                            model.BuildingNumber = Utils.ObjectIsNull(arryItem[i, 13]);
                            if (model.Id == 0)//不是从库里读取出来的
                            {
                                //导入楼栋匹配
                                if (datProject != null)
                                {
                                    DATBuilding datBuilding = GetDATBuilding(datProject.ProjectId,
                                        datProject.CityID, model.BuildingNumber);
                                    if (datBuilding != null)
                                    {
                                        model.BuildingNumber = Utils.DecodeField(datBuilding.BuildingName);
                                        model.BuildingId = datBuilding.BuildingId;
                                        model.BuildingNumberMatch = true;
                                    }
                                }
                            }
                            else
                            {
                                //楼栋匹配
                                if (model.BuildingId != null && !model.BuildingId.Value.Equals(0))
                                {
                                    model.BuildingNumberMatch = true;
                                }
                            }
                            model.FloorNumber = Utils.ObjectIsNull(arryItem[i, 14]);
                            model.RoomNumber = Utils.ObjectIsNull(arryItem[i, 15]);
                            if (model.Id == 0)//不是从库里读取出来的
                            {
                                //导入房号匹配
                                string RoomVal = model.RoomNumber, FloorVal = model.FloorNumber;
                                RoomVal = RoomVal.Replace("房号", "")
                                    .Replace("号房", "")
                                    .Replace("室", "");
                                FloorVal = FloorVal.Replace("层", "")
                                    .Replace("楼", "");
                                if (model.BuildingId != null)
                                {
                                    DATHouse datHouse = GetDATHouse(datProject.CityID, model.BuildingId.Value, RoomVal);
                                    if (datHouse.HouseName.Equals(RoomVal))
                                    {
                                        model.RoomNumber = Utils.DecodeField(datHouse.HouseName);
                                        model.RoomNumberMatch = true;
                                    }
                                    if (datHouse.FloorNo.Equals(FloorVal))
                                    {
                                        model.FloorNumber = datHouse.FloorNo.ToString();
                                        model.FloorNumberMatch = true;
                                    }
                                    model.RoomId = datHouse.HouseId;
                                }
                            }
                            else
                            {
                                //房号匹配
                                if (model.RoomId != null && !model.RoomId.Value.Equals(0))
                                {
                                    model.RoomNumberMatch = true;
                                }
                            }
                            list.Add(model);
                        }

                    }
                    catch (Exception exe)
                    {
                        Utils.DeleteFile(excelFile);
                    }
                }
            }

            return new
            {
                data = list,
                pageIndex = pageIndex,
                count = count,
                column = ExcelColumns()
            };
        }

        /// <summary>
        /// 获得Excel文件总数
        /// </summary>
        /// <returns></returns>
        public int GetExcelCount()
        {
            ReadExcel(1, 1);
            return count;
        }
        #endregion

        #region 押品拆分

        void TaskXml(DataCollateral model)
        {
            string splitValue = string.Empty;
            XmlDocument document = new XmlDocument();
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CollateralSplit.xml");
            document.Load(path);
            XmlNode node = document.ChildNodes.Item(1);//Content
            XmlNodeList list = node.ChildNodes;
            for (int i = 0; i < list.Count; i++)
            {
                XmlNode nodeContent = list.Item(i);
                if (i == 0)
                {
                    //过滤省市区
                    splitValue = TaskPCA(nodeContent, model);
                }
                else//非省市县
                {
                    XmlNodeList xnl = nodeContent.ChildNodes;
                    string strValue = string.Empty,
                           strValueAfter = string.Empty,
                           attrValue = nodeContent.Attributes["N1"].Value,
                           attrValueChild = string.Empty;
                    for (int j = 0; j < xnl.Count; j++)
                    {
                        XmlNode xnlNode = xnl.Item(j);
                        Regex regex = new Regex(xnlNode.InnerText);
                        strValue = regex.Match(splitValue).Value;

                        if (nodeContent.Name.Equals("Address"))
                        {
                            string lou = string.Empty;
                            if (!Utils.IsNullOrEmpty(strValue))
                            {
                                lou = splitValue.Replace(strValue, "");
                                if (!Utils.IsNullOrEmpty(lou))//是否有"号楼"
                                    lou = lou.Substring(0, 1);
                            }
                            //拆分后信息中不能包含指定关键字
                            if (strValue.Contains("栋")
                                || strValue.Contains("幢")
                                || strValue.Contains("号楼")
                                || strValue.Contains("座")
                                || strValue.Contains("幢楼")
                                || strValue.Contains("单元")
                                || strValue.Contains("层")
                                || strValue.Contains("花园")
                                || (lou != null && lou.Equals("楼")))
                            {
                                strValue = string.Empty;
                                strValueAfter = splitValue;
                                continue;
                            }
                        }
                        else if (nodeContent.Name.Equals("Building"))
                        {
                            if (strValue.Contains("层"))
                            {
                                strValue = string.Empty;
                                strValueAfter = splitValue;
                                continue;
                            }
                        }
                        if (!Utils.IsNullOrEmpty(strValue))
                        {
                            strValueAfter = splitValue.Replace(strValue, "");
                            if (xnlNode.Attributes["N1"] != null)
                                attrValueChild = xnlNode.Attributes["N1"].Value;
                            strValue = CharacterReplace(strValue);
                            break;
                        }
                        else if (j == xnl.Count - 1 && Utils.IsNullOrEmpty(strValue))
                        {
                            strValueAfter = splitValue;
                        }
                    }
                    DATProject datProject = null;
                    switch (nodeContent.Name)
                    {
                        case "Address"://路号                            
                            model.Road = strValue;
                            string[] val = CustomXmlValue(attrValueChild, strValue);
                            if (val.Length > 1)
                            {
                                model.Road = val[0];
                                model.RoadNumber = val[1];
                                //匹配楼盘 根据地址
                                List<DATProject> _datproject = GetDatProject("", model.CityId, 1,
                                    string.Format("{0}{1}", model.Road, model.RoadNumber));
                                if (_datproject != null && _datproject.Count() == 1)
                                {
                                    datProject = _datproject.FirstOrDefault();
                                    model.ProjectName = datProject.ProjectName;
                                    model.ProjectAddress = datProject.Address;
                                }
                            }
                            else
                            {
                                model.Road = val[0];
                            }
                            break;
                        case "Project"://楼盘名称
                            if (Utils.IsNullOrEmpty(model.ProjectName))
                            {
                                model.ProjectName = strValue;
                                if (!model.CityId.Equals(0) && !Utils.IsNullOrEmpty(strValue))
                                {
                                    //匹配楼盘 根据楼盘
                                    if (datProject == null)
                                    {
                                        List<DATProject> _datproject = GetDatProject(strValue, model.CityId);
                                        if (_datproject != null && _datproject.Count() == 1)
                                            datProject = _datproject.FirstOrDefault();
                                    }
                                    if (datProject != null)
                                    {
                                        model.ProjectName = datProject.ProjectName;
                                        model.ProjectId = datProject.ProjectId;//有则,表示关联上
                                    }
                                }
                            }
                            break;
                        case "PInstallments"://分期
                            model.Installment = strValue;
                            if (!Utils.IsNullOrEmpty(model.ProjectName) && !Utils.IsNullOrEmpty(model.Installment))
                            {
                                //匹配楼盘 根据分期
                                if (datProject == null)
                                {
                                    List<DATProject> _datproject = GetDatProject(
                                        string.Format("{0}{1}", model.ProjectName, model.Installment),
                                        model.CityId);
                                    if (_datproject != null && _datproject.Count() == 1)
                                        datProject = _datproject.FirstOrDefault();
                                }
                                if (datProject != null)
                                {
                                    model.ProjectName = datProject.ProjectName;
                                    model.ProjectId = datProject.ProjectId;//有则,表示关联上
                                }
                            }
                            break;
                        case "Building"://楼栋
                            model.BuildingNumber = strValue;
                            if (datProject != null && !Utils.IsNullOrEmpty(strValue))
                            {
                                int buildingNum = 0;
                                char ch = strValue.Last<char>();
                                if (!char.IsNumber(ch))
                                    buildingNum = Convert.ToInt32(strValue.Remove(strValue.Length - 1));
                                else
                                    buildingNum = Convert.ToInt32(strValue);
                                if (datProject.BuildingNum != null
                                    && datProject.BuildingNum.Value.Equals(buildingNum))
                                {
                                    //匹配楼栋
                                    DATBuilding datBuilding = GetDATBuilding(datProject.ProjectId,
                                        datProject.CityID, model.BuildingNumber);
                                    if (datBuilding != null && !datBuilding.BuildingId.Equals(0))
                                        model.BuildingNumber = datBuilding.BuildingName;
                                }
                            }
                            break;
                        case "Floor"://楼层
                            //如果楼层是空的。就到楼栋里检查是否有楼层
                            if (Utils.IsNullOrEmpty(strValue))
                            {
                                XmlNode nodeFloor = list.Item(5);//得到楼层表达式
                                Regex regex = new Regex(nodeFloor.Attributes["Building"].Value);
                                strValue = CustomXmlValue(nodeFloor.ChildNodes, model.BuildingNumber);
                                strValue = regex.Match(strValue).Value;
                                if (!string.IsNullOrEmpty(strValue))
                                    model.BuildingNumber =
                                        model.BuildingNumber.Replace(strValue, "");
                            }
                            model.FloorNumber = strValue;
                            break;
                        case "HouseNo"://房号
                            if (model.FloorNumber.Contains("跃"))
                            {
                                XmlNode nodeFloor = list.Item(6);//得到房号表达式
                                Regex regex = new Regex(nodeFloor.Attributes["Floor"].Value);
                                strValue = regex.Match(model.FloorNumber).Value;
                                if (!string.IsNullOrEmpty(strValue))
                                {
                                    model.FloorNumber =
                                        model.FloorNumber.Replace(strValue, "");
                                    //是否全中文
                                    Regex regxs = new Regex("^[\u4E00-\u9FA5]|[\uFE30-\uFFA0]$");
                                    if (regxs.IsMatch(model.FloorNumber))
                                        model.FloorNumber = string.Empty;
                                }
                            }
                            if (Utils.IsNullOrEmpty(strValue))//拆到房号,且又是最后一个,把最后一部分当作房号
                                model.RoomNumber = CharacterReplace(strValueAfter);
                            else
                                model.RoomNumber = strValue;
                            //检查房号、楼层是否能够匹配上
                            if (datProject != null && !Utils.IsNullOrEmpty(model.RoomNumber))
                            {
                                string RoomVal = model.RoomNumber, FloorVal = model.FloorNumber;
                                RoomVal = RoomVal.Replace("房号", "")
                                    .Replace("号房", "")
                                    .Replace("室", "");
                                FloorVal = FloorVal.Replace("层", "")
                                    .Replace("楼", "");
                                //匹配房号
                                DATHouse datHouse = GetDATHouse(datProject.CityID, model.BuildingId.Value, RoomVal);
                                if (datHouse != null)
                                {
                                    if (!Utils.IsNullOrEmpty(datHouse.HouseName) && datHouse.HouseName.Equals(RoomVal))
                                    {
                                        model.RoomNumber = datHouse.HouseName;
                                    }
                                    if (!datHouse.FloorNo.Equals(0) && datHouse.FloorNo.Equals(FloorVal))
                                    {
                                        model.FloorNumber = datHouse.FloorNo.ToString();
                                    }
                                }
                            }
                            break;
                    }
                    splitValue = strValueAfter;
                }
            }
        }

        string TaskPCA(XmlNode pnode, DataCollateral model)
        {
            XmlNodeList list = pnode.ChildNodes;
            string splitVal = TaskPCADetect(model),
                strZXS = splitVal.Substring(0, 2),
                strValue = string.Empty,
                strValueAfter = string.Empty;
            int empty = 0;
            string[] arrayZXS = { "北京", "重庆", "天津", "上海" };
            for (int i = 0; i < list.Count; i++)
            {
                if (Array.IndexOf(arrayZXS, strZXS) >= 0)//如果是直辖市
                {
                    strValue = strZXS;
                    strZXS = string.Empty;
                    strValueAfter = splitVal.Replace(strValue, "");
                    if (strValueAfter.Substring(0, 1).Equals("市"))
                        strValueAfter = strValueAfter.Replace("市", "");
                }
                else//非直辖市,如带什么省
                {
                    XmlNode node = list.Item(i);
                    Regex regex1 = new Regex(node.InnerText);
                    strValue = regex1.Match(splitVal).Value;
                    if (!string.IsNullOrEmpty(strValue))
                    {
                        if (node.Name.Equals("Area") && strValue.Contains("小区"))
                        {
                            strValueAfter = splitVal;
                        }
                        else
                            strValueAfter = splitVal.Replace(strValue, "");
                    }
                    else
                    {
                        /*如果存在县级市*/
                        if (node.Name.Equals("Area"))
                        {
                            strValue = new Regex(list.Item(1).InnerText).Match(splitVal).Value;
                            if (!string.IsNullOrEmpty(strValue))
                                strValueAfter = splitVal.Replace(strValue, "");
                        }
                        strValueAfter = splitVal;
                    }
                }
                if (i == 0)
                {
                    empty = 0;
                    if (string.IsNullOrEmpty(strValue))
                        strValue = Participle.ReadWordbase(strValueAfter, 1);
                    SYSProvince sysProvince = GetSYSProvince(strValue);
                    if (sysProvince != null)
                    {
                        empty = sysProvince.ProvinceId;
                    }
                    model.ProvinceId = empty;
                    if (!empty.Equals(0))
                    {
                        splitVal = strValueAfter.Replace(strValue, "");
                    }
                }
                else if (i == 1)
                {
                    if (string.IsNullOrEmpty(strValue))
                        strValue = Participle.ReadWordbase(strValueAfter, 1);
                    empty = 0;
                    List<SYSCity> sysCity = null;
                    SYSCity city = null;
                    if (model.ProvinceId > 0)
                    {
                        sysCity = GetSysCity(model.ProvinceId, null);

                        city = sysCity != null && sysCity.Count == 1 ? sysCity.FirstOrDefault() : sysCity
                            .Where(sc => sc.CityName.Equals(strValue) ||
                                sc.CityName.Equals(string.Format("{0}市", strValue)))
                            .FirstOrDefault();
                        if (city != null)
                            empty = city.CityId;
                    }
                    else
                    {
                        sysCity = GetSysCity(0, strValue);

                        if (sysCity != null && sysCity.Count.Equals(1))
                        {
                            city = sysCity.FirstOrDefault();
                            empty = city.CityId;
                        }
                    }

                    model.CityId = empty;

                    if (!empty.Equals(0))
                    {
                        if (model.ProvinceId.Equals(0))
                        {
                            /*联动提取到省份*/
                            SYSProvince sysProvince = GetSYSProvince(null, model.CityId);

                            model.ProvinceId = sysProvince.ProvinceId;
                        }
                        if (!strValue.Contains("区"))/*城市不能有区*/
                            splitVal = strValueAfter.Replace(strValue, "");
                    }
                }
                else if (i == 2)
                {
                    if (string.IsNullOrEmpty(strValue))
                        strValue = Participle.ReadWordbase(strValueAfter, 1);
                    List<SYSArea> sysArea = null;
                    SYSArea area = null;
                    empty = 0;
                    if (model.CityId > 0)
                    {
                        sysArea = GetSysArea(null, model.CityId);

                        area = sysArea != null && sysArea.Count == 1 ? sysArea.FirstOrDefault() : sysArea
                            .Where(sa => sa.AreaName.Equals(strValue))
                            .FirstOrDefault();
                        if (area != null)
                            empty = area.AreaId;
                    }
                    else
                    {
                        sysArea = GetSysArea(strValue);
                        if (sysArea != null && sysArea.Count.Equals(1))
                        {
                            area = sysArea.FirstOrDefault();
                            empty = area.AreaId;
                        }
                    }
                    model.AreaId = empty;
                    if (!empty.Equals(0))
                    {
                        /*联动提取到城市及省份*/
                        if (model.CityId.Equals(0))
                        {
                            SYSCity sysCity = GetSysCity(model.AreaId, 0);
                            model.CityId = sysCity.CityId;
                        }
                        if (!model.CityId.Equals(0) && model.ProvinceId.Equals(0))
                        {
                            SYSProvince sysProvince = GetSYSProvince(null, model.CityId);
                            model.ProvinceId = sysProvince.ProvinceId;
                        }

                        if (!strValue.Contains("小区"))
                            splitVal = strValueAfter.Replace(strValue, "");
                    }
                }
            }
            return splitVal;
        }

        //表达式匹配
        void xml(DataCollaterals model)
        {
            string splitValue = string.Empty;
            XmlDocument document = new XmlDocument();
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CollateralSplit.xml");
            document.Load(path);
            XmlNode node = document.ChildNodes.Item(1);//Content
            XmlNodeList list = node.ChildNodes;
            for (int i = 0; i < list.Count; i++)
            {
                XmlNode nodeContent = list.Item(i);
                if (i == 0)
                {
                    //过滤省市区
                    splitValue = PCA(nodeContent, model);
                }
                else//非省市县
                {
                    XmlNodeList xnl = nodeContent.ChildNodes;
                    string strValue = string.Empty,
                           strValueAfter = string.Empty,
                           attrValue = nodeContent.Attributes["N1"].Value,
                           attrValueChild = string.Empty;
                    for (int j = 0; j < xnl.Count; j++)
                    {
                        XmlNode xnlNode = xnl.Item(j);
                        Regex regex = new Regex(xnlNode.InnerText);
                        strValue = regex.Match(splitValue).Value;

                        if (nodeContent.Name.Equals("Address"))
                        {
                            string lou = string.Empty;
                            if (!Utils.IsNullOrEmpty(strValue))
                            {
                                lou = splitValue.Replace(strValue, "");
                                if (!Utils.IsNullOrEmpty(lou))//是否有"号楼"
                                    lou = lou.Substring(0, 1);
                            }
                            //拆分后信息中不能包含指定关键字
                            if (strValue.Contains("栋")
                                || strValue.Contains("幢")
                                || strValue.Contains("号楼")
                                || strValue.Contains("座")
                                || strValue.Contains("幢楼")
                                || strValue.Contains("单元")
                                || strValue.Contains("层")
                                || strValue.Contains("花园")
                                || (lou != null && lou.Equals("楼")))
                            {
                                strValue = string.Empty;
                                strValueAfter = splitValue;
                                continue;
                            }
                        }
                        else if (nodeContent.Name.Equals("Building"))
                        {
                            if (strValue.Contains("层"))
                            {
                                strValue = string.Empty;
                                strValueAfter = splitValue;
                                continue;
                            }
                        }
                        if (!Utils.IsNullOrEmpty(strValue))
                        {
                            strValueAfter = splitValue.Replace(strValue, "");
                            if (xnlNode.Attributes["N1"] != null)
                                attrValueChild = xnlNode.Attributes["N1"].Value;
                            strValue = CharacterReplace(strValue);
                            break;
                        }
                        else if (j == xnl.Count - 1 && Utils.IsNullOrEmpty(strValue))
                        {
                            strValueAfter = splitValue;
                        }
                    }
                    DATProject datProject = null;
                    switch (nodeContent.Name)
                    {
                        case "Address"://路号                            
                            model.Road = strValue;
                            string[] val = CustomXmlValue(attrValueChild, strValue);
                            if (val.Length > 1)
                            {
                                model.Road = val[0];
                                model.RoadNumber = val[1];
                                //匹配楼盘 根据地址
                                List<DATProject> _datproject = GetDatProject("", model.CityId, 1,
                                    string.Format("{0}{1}", model.Road, model.RoadNumber));
                                if (_datproject != null && _datproject.Count() == 1)
                                {
                                    datProject = _datproject.FirstOrDefault();
                                    model.ProjectName = datProject.ProjectName;
                                    model.ProjectAddress = datProject.Address;
                                }
                            }
                            else
                            {
                                model.Road = val[0];
                            }
                            break;
                        case "Project"://楼盘名称
                            if (Utils.IsNullOrEmpty(model.ProjectName))
                            {
                                model.ProjectName = strValue;
                                if (!model.CityId.Equals(0) && !Utils.IsNullOrEmpty(strValue))
                                {
                                    //匹配楼盘 根据楼盘
                                    if (datProject == null)
                                    {
                                        List<DATProject> _datproject = GetDatProject(strValue, model.CityId);
                                        if (_datproject != null && _datproject.Count() == 1)
                                            datProject = _datproject.FirstOrDefault();
                                    }
                                    if (datProject != null)
                                    {
                                        model.ProjectName = datProject.ProjectName;
                                        model.ProjectId = datProject.ProjectId;//有则,表示关联上
                                        model.ProjectNameMatch = true;
                                    }
                                }
                            }
                            break;
                        case "PInstallments"://分期
                            model.Installment = strValue;
                            if (!Utils.IsNullOrEmpty(model.ProjectName) && !Utils.IsNullOrEmpty(model.Installment))
                            {
                                //匹配楼盘 根据分期
                                if (datProject == null)
                                {
                                    List<DATProject> _datproject = GetDatProject(
                                        string.Format("{0}{1}", model.ProjectName, model.Installment),
                                        model.CityId);
                                    if (_datproject != null && _datproject.Count() == 1)
                                        datProject = _datproject.FirstOrDefault();
                                }
                                if (datProject != null)
                                {
                                    model.ProjectName = datProject.ProjectName;
                                    model.ProjectId = datProject.ProjectId;//有则,表示关联上
                                    model.ProjectNameMatch = true;
                                }
                            }
                            break;
                        case "Building"://楼栋
                            model.BuildingNumber = strValue;
                            if (datProject != null && !Utils.IsNullOrEmpty(strValue))
                            {
                                int buildingNum = 0;
                                char ch = strValue.Last<char>();
                                if (!char.IsNumber(ch))
                                    buildingNum = Convert.ToInt32(strValue.Remove(strValue.Length - 1));
                                else
                                    buildingNum = Convert.ToInt32(strValue);
                                if (datProject.BuildingNum != null
                                    && datProject.BuildingNum.Value.Equals(buildingNum))
                                {
                                    model.BuildingNumberMatch = true;
                                    //匹配楼栋
                                    DATBuilding datBuilding = GetDATBuilding(datProject.ProjectId,
                                        datProject.CityID, model.BuildingNumber);
                                    if (datBuilding != null && !datBuilding.BuildingId.Equals(0))
                                        model.BuildingNumber = datBuilding.BuildingName;
                                }
                            }
                            break;
                        case "Floor"://楼层
                            //如果楼层是空的。就到楼栋里检查是否有楼层
                            if (Utils.IsNullOrEmpty(strValue))
                            {
                                XmlNode nodeFloor = list.Item(5);//得到楼层表达式
                                Regex regex = new Regex(nodeFloor.Attributes["Building"].Value);
                                strValue = CustomXmlValue(nodeFloor.ChildNodes, model.BuildingNumber);
                                strValue = regex.Match(strValue).Value;
                                if (!string.IsNullOrEmpty(strValue))
                                    model.BuildingNumber =
                                        model.BuildingNumber.Replace(strValue, "");
                            }
                            model.FloorNumber = strValue;
                            break;
                        case "HouseNo"://房号
                            if (model.FloorNumber.Contains("跃"))
                            {
                                XmlNode nodeFloor = list.Item(6);//得到房号表达式
                                Regex regex = new Regex(nodeFloor.Attributes["Floor"].Value);
                                strValue = regex.Match(model.FloorNumber).Value;
                                if (!string.IsNullOrEmpty(strValue))
                                {
                                    model.FloorNumber =
                                        model.FloorNumber.Replace(strValue, "");
                                    //是否全中文
                                    Regex regxs = new Regex("^[\u4E00-\u9FA5]|[\uFE30-\uFFA0]$");
                                    if (regxs.IsMatch(model.FloorNumber))
                                        model.FloorNumber = string.Empty;
                                }
                            }
                            if (Utils.IsNullOrEmpty(strValue))//拆到房号,且又是最后一个,把最后一部分当作房号
                                model.RoomNumber = CharacterReplace(strValueAfter);
                            else
                                model.RoomNumber = strValue;
                            //检查房号、楼层是否能够匹配上
                            if (datProject != null && !Utils.IsNullOrEmpty(model.RoomNumber))
                            {
                                string RoomVal = model.RoomNumber, FloorVal = model.FloorNumber;
                                RoomVal = RoomVal.Replace("房号", "")
                                    .Replace("号房", "")
                                    .Replace("室", "");
                                FloorVal = FloorVal.Replace("层", "")
                                    .Replace("楼", "");
                                //匹配房号
                                DATHouse datHouse = GetDATHouse(datProject.CityID, model.BuildingId.Value, RoomVal);
                                if (datHouse != null)
                                {
                                    if (!Utils.IsNullOrEmpty(datHouse.HouseName) && datHouse.HouseName.Equals(RoomVal))
                                    {
                                        model.RoomNumber = datHouse.HouseName;
                                        model.RoomNumberMatch = true;
                                    }
                                    if (!datHouse.FloorNo.Equals(0) && datHouse.FloorNo.Equals(FloorVal))
                                    {
                                        model.FloorNumber = datHouse.FloorNo.ToString();
                                        model.FloorNumberMatch = true;
                                    }
                                }
                            }
                            break;
                    }
                    splitValue = strValueAfter;
                }
            }
        }

        //省市区        
        string PCA(XmlNode pnode, DataCollaterals model)
        {
            XmlNodeList list = pnode.ChildNodes;
            string splitVal = PCADetect(model),
                strZXS = splitVal.Substring(0, 2),
                strValue = string.Empty,
                strValueAfter = string.Empty;
            int empty = 0;
            string[] arrayZXS = { "北京", "重庆", "天津", "上海" };
            for (int i = 0; i < list.Count; i++)
            {
                if (Array.IndexOf(arrayZXS, strZXS) >= 0)//如果是直辖市
                {
                    strValue = strZXS;
                    strZXS = string.Empty;
                    strValueAfter = splitVal.Replace(strValue, "");
                    if (strValueAfter.Substring(0, 1).Equals("市"))
                        strValueAfter = strValueAfter.Replace("市", "");
                }
                else//非直辖市,如带什么省
                {
                    XmlNode node = list.Item(i);
                    Regex regex1 = new Regex(node.InnerText);
                    strValue = regex1.Match(splitVal).Value;
                    if (!string.IsNullOrEmpty(strValue))
                    {
                        if (node.Name.Equals("Area") && strValue.Contains("小区"))
                        {
                            strValueAfter = splitVal;
                        }
                        else
                            strValueAfter = splitVal.Replace(strValue, "");
                    }
                    else
                    {
                        /*如果存在县级市*/
                        if (node.Name.Equals("Area"))
                        {
                            strValue = new Regex(list.Item(1).InnerText).Match(splitVal).Value;
                            if (!string.IsNullOrEmpty(strValue))
                                strValueAfter = splitVal.Replace(strValue, "");
                        }
                        strValueAfter = splitVal;
                    }
                }
                if (i == 0)
                {
                    empty = 0;
                    if (string.IsNullOrEmpty(strValue))
                        strValue = Participle.ReadWordbase(strValueAfter, 1);
                    SYSProvince sysProvince = GetSYSProvince(strValue);
                    if (sysProvince != null)
                    {
                        empty = sysProvince.ProvinceId;
                        model.ProvinceName = sysProvince.ProvinceName;
                    }
                    model.ProvinceId = empty;
                    if (!empty.Equals(0))
                    {
                        splitVal = strValueAfter.Replace(strValue, "");
                    }
                }
                else if (i == 1)
                {
                    if (string.IsNullOrEmpty(strValue))
                        strValue = Participle.ReadWordbase(strValueAfter, 1);
                    empty = 0;
                    List<SYSCity> sysCity = null;
                    SYSCity city = null;
                    if (model.ProvinceId > 0)
                    {
                        sysCity = GetSysCity(model.ProvinceId, null);

                        city = sysCity != null && sysCity.Count == 1 ? sysCity.FirstOrDefault() : sysCity
                            .Where(sc => sc.CityName.Equals(strValue) ||
                                sc.CityName.Equals(string.Format("{0}市", strValue)))
                            .FirstOrDefault();
                        if (city != null)
                            empty = city.CityId;
                    }
                    else
                    {
                        sysCity = GetSysCity(0, strValue);

                        if (sysCity != null && sysCity.Count.Equals(1))
                        {
                            city = sysCity.FirstOrDefault();
                            empty = city.CityId;
                        }
                    }

                    model.CityId = empty;
                    model.CityName = city != null ? city.CityName : "";

                    if (!empty.Equals(0))
                    {
                        if (model.ProvinceId.Equals(0))
                        {
                            /*联动提取到省份*/
                            SYSProvince sysProvince = GetSYSProvince(null, model.CityId);

                            model.ProvinceId = sysProvince.ProvinceId;
                            model.ProvinceName = sysProvince.ProvinceName;
                        }
                        if (!strValue.Contains("区"))/*城市不能有区*/
                            splitVal = strValueAfter.Replace(strValue, "");
                    }
                }
                else if (i == 2)
                {
                    if (string.IsNullOrEmpty(strValue))
                        strValue = Participle.ReadWordbase(strValueAfter, 1);
                    List<SYSArea> sysArea = null;
                    SYSArea area = null;
                    empty = 0;
                    if (model.CityId > 0)
                    {
                        sysArea = GetSysArea(null, model.CityId);

                        area = sysArea != null && sysArea.Count == 1 ? sysArea.FirstOrDefault() : sysArea
                            .Where(sa => sa.AreaName.Equals(strValue))
                            .FirstOrDefault();
                        if (area != null)
                            empty = area.AreaId;
                    }
                    else
                    {
                        sysArea = GetSysArea(strValue);
                        if (sysArea != null && sysArea.Count.Equals(1))
                        {
                            area = sysArea.FirstOrDefault();
                            empty = area.AreaId;
                        }
                    }
                    model.AreaId = empty;
                    model.AreaName = area != null ? area.AreaName : "";
                    if (!empty.Equals(0))
                    {
                        /*联动提取到城市及省份*/
                        if (model.CityId.Equals(0))
                        {
                            SYSCity sysCity = GetSysCity(model.AreaId, 0);
                            model.CityId = sysCity.CityId;
                            model.CityName = sysCity.CityName;
                        }
                        if (!model.CityId.Equals(0) && model.ProvinceId.Equals(0))
                        {
                            SYSProvince sysProvince = GetSYSProvince(null, model.CityId);
                            model.ProvinceId = sysProvince.ProvinceId;
                            model.ProvinceName = sysProvince.ProvinceName;
                        }

                        if (!strValue.Contains("小区"))
                            splitVal = strValueAfter.Replace(strValue, "");
                    }
                }
            }
            return splitVal;
        }

        /// <summary>
        /// 自定义匹配某阶段表达式值
        /// </summary>
        /// <param name="list">集合</param>
        /// <param name="splitValue">查找字符串</param>
        /// <returns></returns>
        string CustomXmlValue(XmlNodeList list, string splitValue)
        {
            string strValue = string.Empty;
            for (int j = 0; j < list.Count; j++)
            {
                XmlNode xnlNode = list.Item(j);
                Regex regex = new Regex(xnlNode.InnerText);
                strValue = regex.Match(splitValue).Value;
                if (!string.IsNullOrEmpty(strValue)) break;
            }
            return strValue;
        }
        string[] CustomXmlValue(string valRegex, string splitValue)
        {
            string[] strValue = null;
            if (!string.IsNullOrEmpty(valRegex))//以 *路*？号、*街*？号、*道*？号、*街道*？号
            {
                Regex regex = new Regex(valRegex);
                string val = regex.Match(splitValue).Value;
                strValue = new string[] { val, splitValue.Replace(val, "") };
            }
            else
            {
                strValue = new string[] { splitValue };
            }
            return strValue;
        }
        /// <summary>
        /// 替换无需字符
        /// </summary>
        /// <param name="val">值</param>
        /// <returns></returns>
        string CharacterReplace(string val)
        {
            val = val.Replace("(", "");
            val = val.Replace(")", "");
            val = val.Replace("（", "");
            val = val.Replace("）", "");
            val = val.Replace("\"/", "");
            return val;
        }
        #endregion

        #region 处理押品名称和押品地址是否合并
        string TaskPCADetect(DataCollateral model)
        {
            int[] name = PCADetectVal(model.Name);
            int[] address = PCADetectVal(model.Address);

            string splitVal = string.Empty;
            bool nameIndexOf = name[0] >= 1 || name[1] >= 1 || name[2] >= 1,
                addressIndexOf = address[0] >= 1 || address[1] >= 1 || address[2] >= 1;

            if (nameIndexOf && addressIndexOf)//两边都有相关的关键字时
            {
                splitVal = TaskOneLtTwoOrTwoLtOne(model);
            }
            else if (nameIndexOf || addressIndexOf)//当某一边有关键字时
            {
                //当"押品地址"和"押品名称"中有空或者0的时候清空
                if (model.Address.Equals("0") || string.IsNullOrEmpty(model.Address)) model.Address = string.Empty;
                if (model.Name.Equals("0") || string.IsNullOrEmpty(model.Name)) model.Name = string.Empty;

                if (nameIndexOf)
                {
                    int i = RepeatCount(model.Address, model.Name);
                    if (i == 0)
                        splitVal = string.Format("{0}{1}", model.Name, model.Address);
                    else
                        splitVal = model.Name;
                }
                else if (addressIndexOf)
                {
                    int i = RepeatCount(model.Name, model.Address);
                    if (i == 0)
                        splitVal = string.Format("{0}{1}", model.Address, model.Name);
                    else
                        splitVal = model.Address;

                }
            }
            else//当没有一边有关键字的时候
            {
                splitVal = TaskOneLtTwoOrTwoLtOne(model);
            }
            return splitVal;
        }
        //是否合并
        string PCADetect(DataCollaterals model)
        {
            int[] name = PCADetectVal(model.Name);
            int[] address = PCADetectVal(model.Address);

            string splitVal = string.Empty;
            bool nameIndexOf = name[0] >= 1 || name[1] >= 1 || name[2] >= 1,
                addressIndexOf = address[0] >= 1 || address[1] >= 1 || address[2] >= 1;

            if (nameIndexOf && addressIndexOf)//两边都有相关的关键字时
            {
                splitVal = OneLtTwoOrTwoLtOne(model);
            }
            else if (nameIndexOf || addressIndexOf)//当某一边有关键字时
            {
                //当"押品地址"和"押品名称"中有空或者0的时候清空
                if (model.Address.Equals("0") || string.IsNullOrEmpty(model.Address)) model.Address = string.Empty;
                if (model.Name.Equals("0") || string.IsNullOrEmpty(model.Name)) model.Name = string.Empty;

                if (nameIndexOf)
                {
                    int i = RepeatCount(model.Address, model.Name);
                    if (i == 0)
                        splitVal = string.Format("{0}{1}", model.Name, model.Address);
                    else
                        splitVal = model.Name;
                }
                else if (addressIndexOf)
                {
                    int i = RepeatCount(model.Name, model.Address);
                    if (i == 0)
                        splitVal = string.Format("{0}{1}", model.Address, model.Name);
                    else
                        splitVal = model.Address;

                }
            }
            else//当没有一边有关键字的时候
            {
                splitVal = OneLtTwoOrTwoLtOne(model);
            }
            return splitVal;
        }

        string TaskOneLtTwoOrTwoLtOne(DataCollateral model)
        {
            string splitVal = string.Empty;
            int lenName = model.Name != null ? model.Name.Length : 0,
                lenAddress = model.Address.Length;
            if (lenName > lenAddress)
                splitVal = model.Name;
            else
            {
                if (string.IsNullOrEmpty(model.Address))
                    splitVal = model.Name;
                else
                    splitVal = model.Address;
            }
            return splitVal;
        }

        //比较哪个大,就取哪个值
        string OneLtTwoOrTwoLtOne(DataCollaterals model)
        {
            string splitVal = string.Empty;
            int lenName = model.Name.Length,
                lenAddress = model.Address.Length;
            if (lenName > lenAddress)
                splitVal = model.Name;
            else
            {
                if (string.IsNullOrEmpty(model.Address))
                    splitVal = model.Name;
                else
                    splitVal = model.Address;
            }
            return splitVal;
        }
        //查找关键字是否存在
        int[] PCADetectVal(string strVal)
        {
            string Name = strVal,
                   NP = string.Empty,
                   NC = string.Empty,
                   NA = string.Empty;
            int[] val = { 0, 0, 0 };
            if (!string.IsNullOrEmpty(Name))
            {
                NP = Participle.ReadWordbase(Name, 1);
                if (!string.IsNullOrEmpty(NP))//name省
                {
                    SYSProvince sysProvince = GetSYSProvince(NP);
                    if (sysProvince != null)
                        val[0] = sysProvince.ProvinceId;
                }
                if (!string.IsNullOrEmpty(NP))//name市
                {
                    Name = val[0] == 0 ? Name : Name.Replace(NP, "");
                    NC = Participle.ReadWordbase(Name, 1);
                    List<SYSCity> sysCity = GetSysCity(0, NC);
                    var vcity = sysCity.Where(sc =>
                        sc.Alias == NC ||
                        sc.CityName == (NC.IndexOf("市") >= 0 ? NC : string.Format("{0}市", NC)))
                        .FirstOrDefault();
                    if (vcity != null)
                        val[1] = vcity.CityId;
                }
                if (!string.IsNullOrEmpty(NC))//name区
                {
                    Name = val[1] == 0 ? Name : Name.Replace(NC, "");
                    NA = Participle.ReadWordbase(Name, 1);
                    List<SYSArea> sysArea = GetSysArea(NA);
                    var varea = sysArea.Where(sc =>
                        sc.AreaName == (NA.IndexOf("区") >= 0 ? NA : string.Format("{0}区", NA)) ||
                        sc.AreaName == (NA.IndexOf("县") >= 0 ? NA : string.Format("{0}县", NA)) ||
                        sc.AreaName == (NA.IndexOf("市") >= 0 ? NA : string.Format("{0}市", NA)))
                        .FirstOrDefault();
                    if (varea != null)
                        val[2] = varea.AreaId;
                }
            }
            return val;
        }
        //出现次数
        int RepeatCount(string val, string repeatVal)
        {
            string[] ch = Participle.ReadWordbase(val);
            int i = 1, rei = 0;
            string strLast = string.Empty;
            //last = string.Empty;
            while (i <= ch.Length)
            {
                if (repeatVal.Contains(ch[i - 1]))
                {
                    rei = i;
                    break;
                }
                //else
                //{
                //    strLast += ch[i - 1];
                //}
                i++;
            }
            //if (!rei.Equals(0))
            //    last = strLast;
            return rei;
        }
        #endregion

        #region 数据处理
        int SubstringCount(string str, string substring)
        {
            if (str.Contains(substring))
            {
                string strReplaced = str.Replace(substring, "");
                return (str.Length - strReplaced.Length) / substring.Length;
            }

            return 0;
        }
        /// <summary>
        /// 得到楼盘
        /// </summary>
        /// <param name="pname">楼盘名称</param>
        /// <param name="cityid">城市ID</param>
        /// <param name="type">类型:0(楼盘、分期)1(地址)</param>
        /// <returns></returns>
        List<DATProject> GetDatProject(string pname, int cityid, int type = 0, string paddress = null)
        {
            List<DATProject> _datproject = null;
            using (FxtAPIClient client = new FxtAPIClient())
            {
                if (type == 0)
                {
                    //GetProjectJoinProjectMatchByProjectNameCityId
                    JObject _obj = new JObject();
                    _obj.Add("projectName", pname);
                    _obj.Add("cityId", cityid);
                    result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                      "D", "GetProjectJoinProjectMatchADOByProjectNameCityId", Utils.Serialize(_obj));
                    //Utils.GetJObjectValue(result, "data")
                    _datproject = Utils.Deserialize<List<DATProject>>(result.ToString());
                }
                else if (type == 1)
                {
                    //GetProjectJoinPMatchByPNameOrPAddressCityId
                    JObject _obj = new JObject();
                    _obj.Add("pName", "");
                    _obj.Add("pAddress", paddress);
                    _obj.Add("cId", cityid);
                    result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                      "D", "GetProjectJoinPMatchADOByPNameOrPAddressCityId", Utils.Serialize(_obj));
                    //Utils.GetJObjectValue(result, "data")
                    _datproject = Utils.Deserialize<List<DATProject>>(result.ToString());
                }
            }
            return _datproject;
        }

        /// <summary>
        /// 获得楼栋(楼宇)
        /// </summary>
        /// <param name="pid">楼盘ID</param>
        /// <param name="cid">城市ID</param>
        /// <param name="bname">楼栋名称</param>
        /// <returns></returns>
        DATBuilding GetDATBuilding(int pid, int cid, string bname)
        {
            DATBuilding datBuilding = null;
            using (FxtAPIClient client = new FxtAPIClient())
            {
                //GetBuildingByProject_City_Build
                JObject _obj = new JObject();
                _obj.Add("pId", pid);
                _obj.Add("cId", cid);
                _obj.Add("bName", bname);
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                  "D", "GetBuildingByProject_City_Build", Utils.Serialize(_obj));
                //Utils.GetJObjectValue(result, "data")
                datBuilding = Utils.Deserialize<DATBuilding>(result.ToString());
            }
            return datBuilding;
        }

        /// <summary>
        /// 得到房号
        /// </summary>
        /// <param name="cid">城市ID</param>
        /// <param name="bid">楼栋ID</param>
        /// <param name="hname">房号名称</param>
        /// <returns></returns>
        DATHouse GetDATHouse(int cid, int bid, string hname)
        {
            DATHouse datCase = null;
            using (FxtAPIClient client = new FxtAPIClient())
            {
                //GetHouseByHouse_City_Building
                JObject _obj = new JObject();
                _obj.Add("houseName", hname);
                _obj.Add("cId", cid);
                _obj.Add("bId", bid);
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                  "D", "GetHouseByHouse_City_Building", Utils.Serialize(_obj));
                //Utils.GetJObjectValue(result, "data")
                datCase = Utils.Deserialize<DATHouse>(result.ToString());
            }
            return datCase;
        }
        /// <summary>
        /// 根据省份名称或者城市ID或者省份ID 得到省份
        /// </summary>
        /// <param name="pname">省份名称</param>
        /// <param name="cid">城市ID</param>
        /// <param name="pid">省份ID</param>
        /// <returns></returns>
        SYSProvince GetSYSProvince(string pname, int cid = 0, int pid = 0)
        {
            SYSProvince sysProvince = null;
            using (FxtAPIClient client = new FxtAPIClient())
            {
                if (cid.Equals(0) && !Utils.IsNullOrEmpty(pname))
                {
                    //GetProvinceByName
                    JObject _obj = new JObject();
                    _obj.Add("provinceName", pname);
                    result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                      "D", "GetProvinceByName", Utils.Serialize(_obj));
                    //Utils.GetJObjectValue(result, "data")
                    sysProvince = Utils.Deserialize<SYSProvince>(result.ToString());
                }
                else if (!cid.Equals(0) && Utils.IsNullOrEmpty(pname))
                {
                    //GetProvinceByCityId
                    JObject _obj = new JObject();
                    _obj.Add("cityId", cid);
                    result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                      "D", "GetProvinceByCityId", Utils.Serialize(_obj));
                    //Utils.GetJObjectValue(result, "data")
                    sysProvince = Utils.Deserialize<SYSProvince>(result.ToString());
                }
                else if (Utils.IsNullOrEmpty(pname) && cid.Equals(0) && !pid.Equals(0))
                {
                    //GetProvinceById
                    JObject _obj = new JObject();
                    _obj.Add("provinceId", pid);
                    result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                      "D", "GetProvinceADOById", Utils.Serialize(_obj));
                    //Utils.GetJObjectValue(result, "data")
                    sysProvince = Utils.Deserialize<SYSProvince>(result.ToString());
                }
            }
            return sysProvince;
        }

        /// <summary>
        /// 根据省份或者城市名称得到城市
        /// </summary>
        /// <param name="pid">省份ID</param>
        /// <param name="cname">城市名称</param>
        /// <returns>城市集合</returns>
        List<SYSCity> GetSysCity(int pid, string cname = null)
        {
            List<SYSCity> list = null;
            using (FxtAPIClient client = new FxtAPIClient())
            {
                if (!pid.Equals(0))
                {
                    JObject _obj = new JObject();
                    _obj.Add("provinceId", pid);
                    result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                        "D", "GetCity", Utils.Serialize(_obj));

                    list = Utils.Deserialize<List<SYSCity>>(Utils.GetJObjectValue(result, "data"));
                }
                else if (pid.Equals(0) && !Utils.IsNullOrEmpty(cname))
                {
                    //GetCityListByCityName
                    JObject _obj = new JObject();
                    _obj.Add("cityName", cname);
                    result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                      "D", "GetCityListByCityName", Utils.Serialize(_obj));
                    //Utils.GetJObjectValue(result, "data")
                    list = Utils.Deserialize<List<SYSCity>>(result.ToString());
                }

            }
            return list;
        }

        /// <summary>
        /// 根据行政区ID、城市ID 得到单个城市对象
        /// </summary>
        /// <param name="aid">行政区ID</param>
        /// <param name="cid">城市ID</param>
        /// <returns></returns>
        SYSCity GetSysCity(int aid, int cid = 0)
        {
            SYSCity sysCity = null;
            using (FxtAPIClient client = new FxtAPIClient())
            {
                if (!aid.Equals(0) && cid.Equals(0))
                {
                    //GetCityByAreaId
                    JObject _obj = new JObject();
                    _obj.Add("areaId", aid);
                    result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                      "D", "GetCityByAreaId", Utils.Serialize(_obj));
                    //Utils.GetJObjectValue(result, "data")
                    sysCity = Utils.Deserialize<SYSCity>(result.ToString());
                }
                else if (aid.Equals(0) && !cid.Equals(0))
                {
                    //GetCityByCityId
                    JObject _obj = new JObject();
                    _obj.Add("cityId", cid);
                    result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                      "D", "GetCityByCityId", Utils.Serialize(_obj));
                    //Utils.GetJObjectValue(result, "data")
                    sysCity = Utils.Deserialize<SYSCity>(result.ToString());
                }
            }
            return sysCity;
        }

        /// <summary>
        /// 根据行政区名称或者城市ID 得到行政区
        /// </summary>
        /// <param name="aname">行政区名称</param>
        /// <param name="cid">城市ID</param>
        /// <returns></returns>
        List<SYSArea> GetSysArea(string aname, int cid = 0)
        {
            List<SYSArea> list = null;
            using (FxtAPIClient client = new FxtAPIClient())
            {
                if (!cid.Equals(0) && Utils.IsNullOrEmpty(aname))
                {
                    //GetArea
                    JObject _obj = new JObject();
                    _obj.Add("cityId", cid);
                    result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                        "D", "GetArea", Utils.Serialize(_obj));

                    list = Utils.Deserialize<List<SYSArea>>(Utils.GetJObjectValue(result, "data"));
                }
                else if (cid.Equals(0) && !Utils.IsNullOrEmpty(aname))
                {
                    //GetAreaListByAraeName
                    JObject _obj = new JObject();
                    _obj.Add("areaName", aname);
                    result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                      "D", "GetAreaListByAraeName", Utils.Serialize(_obj));
                    //Utils.GetJObjectValue(result, "data")
                    list = Utils.Deserialize<List<SYSArea>>(result.ToString());
                }
            }
            return list;
        }

        /// <summary>
        /// 根据行政区ID,获取行政区
        /// </summary>
        /// <param name="aid">行政区ID</param>
        /// <returns></returns>
        SYSArea GetSysArea(int aid)
        {
            SYSArea sysArea = null;
            using (FxtAPIClient client = new FxtAPIClient())
            {
                if (!aid.Equals(0))
                {
                    //GetAreaByAreaId
                    JObject _obj = new JObject();
                    _obj.Add("areaId", aid);
                    result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                      "D", "GetAreaByAreaId", Utils.Serialize(_obj));
                    //Utils.GetJObjectValue(result, "data")
                    sysArea = Utils.Deserialize<SYSArea>(result.ToString());
                }
            }
            return sysArea;
        }
        #endregion

        #region Excel组件 读取、写入
        /// <summary>
        /// 组件形式读取Excel
        /// </summary>
        /// <param name="pageIndex">开始索引页</param>
        /// <param name="pageSize">一页总记录</param>
        /// <param name="count">总记录数</param>
        /// <param name="worsheetName">工作簿名称</param>
        /// <param name="isStartPlusOne">是否要设置第一页</param>
        /// <returns></returns>
        public object[,] ReadExcel(int pageIndex, int pageSize, bool isStartPlusOne = false)
        {
            object[,] arryItem = null;
            try
            {
                using (var lExcel = new LibraryExcel())
                {
                    lExcel.Open(excelFile);
                    worsheetName = lExcel.WorksheetName;
                    lExcel.GeRowColumn();
                    count = lExcel.RowCount;
                    int startp = 0, endp = 0;
                    if (isStartPlusOne)//需要分页
                    {
                        endp = pageIndex * pageSize;
                        if (endp > lExcel.RowCount)
                            endp = lExcel.RowCount;

                        startp += 1;
                        if (!endp.Equals(lExcel.RowCount))
                        {
                            endp += 1;
                        }
                        startp = endp - pageSize;
                    }
                    else
                    {
                        if (pageIndex.Equals(0) && pageSize.Equals(0))
                        {
                            startp = 1;
                            endp = lExcel.RowCount;
                            pageSize = endp - 1;
                        }
                        else
                        {
                            startp = 0;
                            endp = 1;
                        }
                    }
                    Array arraySource = Array.CreateInstance(typeof(string), pageSize, lExcel.ColCount);
                    int k = 0;
                    for (int i = startp; i < endp; i++)
                    {
                        for (int j = 0; j < lExcel.ColCount; j++)
                        {
                            using (var rng = Disposable.Create(lExcel.GetCell(i, j)))
                            {
                                arraySource.SetValue(Utils.ObjectIsNull(rng.Value.Value2), k, j);
                            }
                        }
                        k++;
                    }
                    arryItem = (object[,])arraySource;
                }
            }
            catch (Exception exe)
            {

            }
            finally
            {
                GC.Collect();
            }
            return arryItem;
        }

        public object[,] TaskReadExcel()
        {
            object[,] arryItem = null;
            try
            {
                using (var lExcel = new LibraryExcel())
                {
                    lExcel.Open(excelFile);
                    lExcel.GeRowColumn();
                    int RowCount = lExcel.RowCount, ColCount = lExcel.ColCount;
                    Array arraySource = Array.CreateInstance(typeof(string), RowCount - 1, ColCount);
                    int i = 0, j = 0;
                    while (i < RowCount - 1)
                    {
                        j = 0;
                        while (j < ColCount)
                        {
                            using (var rng = Disposable.Create(lExcel.GetCell(i + 1, j)))
                            {
                                arraySource.SetValue(Utils.ObjectIsNull(rng.Value.Value2), i, j);
                            }
                            j++;
                        }
                        i++;
                    }
                    arryItem = (object[,])arraySource;
                }
            }
            finally
            {
                GC.Collect();
            }
            return arryItem;
        }



        /// <summary>
        /// 组件形式读取Excel 押品复估导入
        /// </summary>
        /// <param name="pageIndex">开始索引页</param>
        /// <param name="pageSize">一页总记录</param>
        /// <param name="count">总记录数</param>
        /// <param name="worsheetName">工作簿名称</param>
        /// <param name="isStartPlusOne">是否要设置第一页</param>
        /// <returns></returns>
        public object[,] ReadComplexExcel(int pageIndex, int pageSize, bool isStartPlusOne = false)
        {
            object[,] arryItem = null;
            try
            {
                using (var lExcel = new LibraryExcel())
                {
                    lExcel.Open(excelFile);
                    worsheetName = lExcel.WorksheetName;
                    lExcel.GeRowColumn();
                    count = lExcel.RowCount;
                    int startp = 0, endp = 0;
                    if (isStartPlusOne)//需要分页
                    {
                        endp = pageIndex * pageSize;
                        if (endp > lExcel.RowCount)
                            endp = lExcel.RowCount;

                        startp += 1;
                        if (!endp.Equals(lExcel.RowCount))
                        {
                            endp += 1;
                        }
                        startp = endp - pageSize;
                    }
                    else
                    {
                        if (pageIndex.Equals(0) && pageSize.Equals(0))
                        {
                            startp = 1;
                            endp = lExcel.RowCount;
                            pageSize = endp - 1;
                        }
                        else
                        {
                            startp = 0;
                            endp = 1;
                        }
                    }
                    Array arraySource = Array.CreateInstance(typeof(string), pageSize, 4);
                    int[] coladd = new int[] { 0, 13, 10, 9 };
                    for (int i = startp; i < endp; i++)
                    {
                        for (int j = 0; j < coladd.Length; j++)
                        {
                            using (var rng = Disposable.Create(lExcel.GetCell(i, coladd[j])))
                            {
                                arraySource.SetValue(Utils.ObjectIsNull(rng.Value.Value2), i - 1, j);
                            }
                        }
                    }
                    arryItem = (object[,])arraySource;
                }
            }
            catch (Exception exe)
            {

            }
            finally
            {
                GC.Collect();
            }
            return arryItem;
        }


        /// <summary>
        /// 写Excel
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <param name="filename">文件名称</param>
        /// <param name="jobject">对象</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="firestobj">列头</param>
        /// <returns></returns>
        public bool WorkbookWrite(string filepath, List<JObject> jobject, int pageIndex = 0, JObject firestobj = null)
        {
            if (jobject == null || jobject.Count == 0)
                return false;

            filepath = FilePathCreateOrDelete(filepath);

            var firstItem = (firestobj == null) ? jobject.FirstOrDefault() : firestobj;
            int i = 1, cols = 0;
            try
            {
                using (var excel = new LibraryExcel())
                {
                    excel.AddWorkbook();
                    if (pageIndex > 0)
                        excel.WorksheetName = pageIndex.ToString();
                    //设置头
                    foreach (var ch in firstItem)
                    {
                        using (var range = Disposable.Create(excel.GetRange(0, cols, 0, cols)))
                        {
                            using (var font = Disposable.Create(range.Value.Font))
                            {
                                font.Value.Bold = true;
                            }
                            range.Value.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                            range.Value.Value2 = ch.Key.ToString();
                        }
                        cols++;
                    }
                    //遍历数据
                    foreach (var item in jobject)
                    {
                        cols = 0;

                        foreach (var ch in item)
                        {
                            using (var range = Disposable.Create(excel.GetRange(i, cols, i, cols)))
                            {
                                range.Value.NumberFormatLocal = "@";
                                range.Value.Value2 = ch.Value.ToString();
                            }
                            cols++;
                        }
                        i++;
                    }
                    excel.Save(filepath);
                }
                return true;
            }
            catch (Exception exe)
            {
                return false;
            }
        }

        /// <summary>
        /// 押品风险分析Excel生成
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <param name="listData">数据源</param>
        /// <returns>True Or False</returns>
        public bool WorkbookStepAstrideColumnWrite(string filepath, List<JObject> listData)
        {
            #region 复估风险分析列头
            List<JObject> list = new List<JObject>();
            JObject jobect = new JObject();
            jobect.Add("name", "风险状况");
            jobect.Add("row1", 0);
            jobect.Add("col1", 0);
            jobect.Add("row2", 1);
            jobect.Add("col2", 0);
            jobect.Add("isMC", true);
            list.Add(jobect);
            jobect = new JObject();
            jobect.Add("name", "抵押率区间");
            jobect.Add("row1", 0);
            jobect.Add("col1", 1);
            jobect.Add("row2", 1);
            jobect.Add("col2", 1);
            jobect.Add("isMC", true);
            list.Add(jobect);
            jobect = new JObject();
            jobect.Add("name", "业务量");
            jobect.Add("row1", 0);
            jobect.Add("col1", 2);
            jobect.Add("row2", 0);
            jobect.Add("col2", 3);
            jobect.Add("isMC", true);
            list.Add(jobect);
            jobect = new JObject();
            jobect.Add("name", "笔数");
            jobect.Add("row1", 1);
            jobect.Add("col1", 2);
            jobect.Add("row2", 1);
            jobect.Add("col2", 2);
            jobect.Add("isMC", false);
            list.Add(jobect);
            jobect = new JObject();
            jobect.Add("name", "占比");
            jobect.Add("row1", 1);
            jobect.Add("col1", 3);
            jobect.Add("row2", 1);
            jobect.Add("col2", 3);
            jobect.Add("isMC", false);
            list.Add(jobect);

            jobect = new JObject();
            jobect.Add("name", "贷款金额");
            jobect.Add("row1", 0);
            jobect.Add("col1", 4);
            jobect.Add("row2", 0);
            jobect.Add("col2", 5);
            jobect.Add("isMC", true);
            list.Add(jobect);
            jobect = new JObject();
            jobect.Add("name", "金额");
            jobect.Add("row1", 1);
            jobect.Add("col1", 4);
            jobect.Add("row2", 1);
            jobect.Add("col2", 4);
            jobect.Add("isMC", false);
            list.Add(jobect);
            jobect = new JObject();
            jobect.Add("name", "占比");
            jobect.Add("row1", 1);
            jobect.Add("col1", 5);
            jobect.Add("row2", 1);
            jobect.Add("col2", 5);
            jobect.Add("isMC", false);
            list.Add(jobect);

            jobect = new JObject();
            jobect.Add("name", "贷款余额");
            jobect.Add("row1", 0);
            jobect.Add("col1", 6);
            jobect.Add("row2", 0);
            jobect.Add("col2", 7);
            jobect.Add("isMC", true);
            list.Add(jobect);
            jobect = new JObject();
            jobect.Add("name", "金额");
            jobect.Add("row1", 1);
            jobect.Add("col1", 6);
            jobect.Add("row2", 1);
            jobect.Add("col2", 6);
            jobect.Add("isMC", false);
            list.Add(jobect);
            jobect = new JObject();
            jobect.Add("name", "占比");
            jobect.Add("row1", 1);
            jobect.Add("col1", 7);
            jobect.Add("row2", 1);
            jobect.Add("col2", 7);
            jobect.Add("isMC", false);
            list.Add(jobect);

            jobect = new JObject();
            jobect.Add("name", "原估价值");
            jobect.Add("row1", 0);
            jobect.Add("col1", 8);
            jobect.Add("row2", 0);
            jobect.Add("col2", 9);
            jobect.Add("isMC", true);
            list.Add(jobect);
            jobect = new JObject();
            jobect.Add("name", "金额");
            jobect.Add("row1", 1);
            jobect.Add("col1", 8);
            jobect.Add("row2", 1);
            jobect.Add("col2", 8);
            jobect.Add("isMC", false);
            list.Add(jobect);
            jobect = new JObject();
            jobect.Add("name", "占比");
            jobect.Add("row1", 1);
            jobect.Add("col1", 9);
            jobect.Add("row2", 1);
            jobect.Add("col2", 9);
            jobect.Add("isMC", false);
            list.Add(jobect);

            jobect = new JObject();
            jobect.Add("name", "现估价值");
            jobect.Add("row1", 0);
            jobect.Add("col1", 10);
            jobect.Add("row2", 0);
            jobect.Add("col2", 11);
            jobect.Add("isMC", true);
            list.Add(jobect);
            jobect = new JObject();
            jobect.Add("name", "金额");
            jobect.Add("row1", 1);
            jobect.Add("col1", 10);
            jobect.Add("row2", 1);
            jobect.Add("col2", 10);
            jobect.Add("isMC", false);
            list.Add(jobect);
            jobect = new JObject();
            jobect.Add("name", "占比");
            jobect.Add("row1", 1);
            jobect.Add("col1", 11);
            jobect.Add("row2", 1);
            jobect.Add("col2", 11);
            jobect.Add("isMC", false);
            list.Add(jobect);
            #endregion
            filepath = FilePathCreateOrDelete(filepath);
            try
            {
                using (var excel = new LibraryExcel())
                {
                    excel.AddWorkbook();
                    foreach (var item in list)
                    {
                        using (var range = Disposable.Create(excel.GetRange(int.Parse(item["row1"].ToString())
                            , int.Parse(item["col1"].ToString()), int.Parse(item["row2"].ToString()),
                            int.Parse(item["col2"].ToString()))))
                        {
                            range.Value.MergeCells = bool.Parse(item["isMC"].ToString());
                            range.Value.Value2 = item["name"].ToString();
                        }
                    }
                    int row = 2, lastCol = 1;
                    foreach (var item in listData)
                    {
                        using (var range = Disposable.Create(excel.GetRange(row, 0, row + 2, 0)))
                        {
                            range.Value.MergeCells = true;
                            range.Value.Value2 = item["title"].ToString();
                        }
                        JObject j1 = (JObject)item["v1"];
                        foreach (var i1 in j1)
                        {
                            using (var range = Disposable.Create(excel.GetRange(row, lastCol, row, lastCol)))
                            {
                                range.Value.Value2 = i1.Value.ToString();
                            }
                            lastCol++;
                            if (lastCol == j1.Count + 1)
                            {
                                lastCol = 1;
                                row += 1;
                            }
                        }
                        j1 = (JObject)item["v2"];
                        foreach (var i1 in j1)
                        {
                            using (var range = Disposable.Create(excel.GetRange(row, lastCol, row, lastCol)))
                            {
                                range.Value.Value2 = i1.Value.ToString();
                            }
                            lastCol++;
                            if (lastCol == j1.Count + 1)
                            {
                                lastCol = 1;
                                row += 1;
                            }
                        }
                        j1 = (JObject)item["v3"];
                        foreach (var i1 in j1)
                        {
                            using (var range = Disposable.Create(excel.GetRange(row, lastCol, row, lastCol)))
                            {
                                range.Value.Value2 = i1.Value.ToString();
                            }
                            lastCol++;
                            if (lastCol == j1.Count + 1)
                            {
                                lastCol = 1;
                                row += 1;
                            }
                        }
                    }

                    excel.Save(filepath);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 风险预警导出
        /// </summary>
        /// <param name="filepath">路径</param>
        /// <param name="listData">数据集</param>
        /// <returns></returns>
        public bool WorkbookStressTestToRikWarning(string filepath, List<JObject> listData)
        {
            #region 列标题
            List<JObject> listTitle = new List<JObject>();
            JObject title = new JObject();
            title.Add("name", "风险状况");
            title.Add("row1", 0);
            title.Add("col1", 0);
            title.Add("row2", 0);
            title.Add("col2", 0);
            listTitle.Add(title);
            title = new JObject();
            title.Add("name", "抵押率区间");
            title.Add("row1", 0);
            title.Add("col1", 1);
            title.Add("row2", 0);
            title.Add("col2", 1);
            listTitle.Add(title);
            title = new JObject();
            title.Add("name", "业务量");
            title.Add("row1", 0);
            title.Add("col1", 2);
            title.Add("row2", 0);
            title.Add("col2", 2);
            listTitle.Add(title);
            title = new JObject();
            title.Add("name", "原贷款额");
            title.Add("row1", 0);
            title.Add("col1", 3);
            title.Add("row2", 0);
            title.Add("col2", 3);
            listTitle.Add(title);
            title = new JObject();
            title.Add("name", "贷款余额");
            title.Add("row1", 0);
            title.Add("col1", 4);
            title.Add("row2", 0);
            title.Add("col2", 4);
            listTitle.Add(title);
            title = new JObject();
            title.Add("name", "原估价值");
            title.Add("row1", 0);
            title.Add("col1", 5);
            title.Add("row2", 0);
            title.Add("col2", 5);
            listTitle.Add(title);
            title = new JObject();
            title.Add("name", "现估价值");
            title.Add("row1", 0);
            title.Add("col1", 6);
            title.Add("row2", 0);
            title.Add("col2", 6);
            listTitle.Add(title);
            title = new JObject();
            title.Add("name", "原平均抵押率");
            title.Add("row1", 0);
            title.Add("col1", 7);
            title.Add("row2", 0);
            title.Add("col2", 7);
            listTitle.Add(title);
            title = new JObject();
            title.Add("name", "现平均抵押率");
            title.Add("row1", 0);
            title.Add("col1", 8);
            title.Add("row2", 0);
            title.Add("col2", 8);
            listTitle.Add(title);
            #endregion
            filepath = FilePathCreateOrDelete(filepath);
            try
            {
                using (var excel = new LibraryExcel())
                {
                    excel.AddWorkbook();
                    foreach (var item in listTitle)
                    {
                        using (var range = Disposable.Create(excel.GetRange(int.Parse(item["row1"].ToString())
                            , int.Parse(item["col1"].ToString()), int.Parse(item["row2"].ToString()),
                            int.Parse(item["col2"].ToString()))))
                        {
                            range.Value.Value2 = item["name"].ToString();
                        }
                    }

                    int row = 1, lastCol = 1;
                    foreach (var item in listData)
                    {
                        using (var range = Disposable.Create(excel.GetRange(row, 0, row + 2, 0)))
                        {
                            range.Value.MergeCells = true;
                            range.Value.Value2 = item["title"].ToString();
                        }
                        JObject j1 = (JObject)item["v1"];
                        foreach (var i1 in j1)
                        {
                            using (var range = Disposable.Create(excel.GetRange(row, lastCol, row, lastCol)))
                            {
                                range.Value.Value2 = i1.Value.ToString();
                            }
                            lastCol++;
                            if (lastCol == j1.Count + 1)
                            {
                                lastCol = 1;
                                row += 1;
                            }
                        }
                        j1 = (JObject)item["v2"];
                        foreach (var i1 in j1)
                        {
                            using (var range = Disposable.Create(excel.GetRange(row, lastCol, row, lastCol)))
                            {
                                range.Value.Value2 = i1.Value.ToString();
                            }
                            lastCol++;
                            if (lastCol == j1.Count + 1)
                            {
                                lastCol = 1;
                                row += 1;
                            }
                        }
                        j1 = (JObject)item["v3"];
                        foreach (var i1 in j1)
                        {
                            using (var range = Disposable.Create(excel.GetRange(row, lastCol, row, lastCol)))
                            {
                                range.Value.Value2 = i1.Value.ToString();
                            }
                            lastCol++;
                            if (lastCol == j1.Count + 1)
                            {
                                lastCol = 1;
                                row += 1;
                            }
                        }
                    }
                    excel.Save(filepath);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 压力测试导出
        /// </summary>
        /// <param name="filepath">路径</param>
        /// <param name="listData">数据集</param>
        /// <returns></returns>
        public bool WorkbookStressTest(string filepath, List<JObject> listData)
        {
            #region 列标题
            List<JObject> listTitle = new List<JObject>();
            JObject title = new JObject();
            title.Add("name", "风险状况");
            title.Add("row1", 0);
            title.Add("col1", 0);
            title.Add("row2", 0);
            title.Add("col2", 0);
            listTitle.Add(title);
            title = new JObject();
            title.Add("name", "抵押率区间");
            title.Add("row1", 0);
            title.Add("col1", 1);
            title.Add("row2", 0);
            title.Add("col2", 1);
            listTitle.Add(title);
            title = new JObject();
            title.Add("name", "业务量");
            title.Add("row1", 0);
            title.Add("col1", 2);
            title.Add("row2", 0);
            title.Add("col2", 2);
            listTitle.Add(title);
            title = new JObject();
            title.Add("name", "原贷款额");
            title.Add("row1", 0);
            title.Add("col1", 3);
            title.Add("row2", 0);
            title.Add("col2", 3);
            listTitle.Add(title);
            title = new JObject();
            title.Add("name", "贷款余额");
            title.Add("row1", 0);
            title.Add("col1", 4);
            title.Add("row2", 0);
            title.Add("col2", 4);
            listTitle.Add(title);
            title = new JObject();
            title.Add("name", "原估价值");
            title.Add("row1", 0);
            title.Add("col1", 5);
            title.Add("row2", 0);
            title.Add("col2", 5);
            listTitle.Add(title);
            title = new JObject();
            title.Add("name", "现估价值");
            title.Add("row1", 0);
            title.Add("col1", 6);
            title.Add("row2", 0);
            title.Add("col2", 6);
            listTitle.Add(title);
            title = new JObject();
            title.Add("name", "原平均抵押率");
            title.Add("row1", 0);
            title.Add("col1", 7);
            title.Add("row2", 0);
            title.Add("col2", 7);
            listTitle.Add(title);
            title = new JObject();
            title.Add("name", "现平均抵押率");
            title.Add("row1", 0);
            title.Add("col1", 8);
            title.Add("row2", 0);
            title.Add("col2", 8);
            listTitle.Add(title);
            #endregion
            filepath = FilePathCreateOrDelete(filepath);
            try
            {
                using (var excel = new LibraryExcel())
                {
                    excel.AddWorkbook();
                    foreach (var item in listTitle)
                    {
                        using (var range = Disposable.Create(excel.GetRange(int.Parse(item["row1"].ToString())
                            , int.Parse(item["col1"].ToString()), int.Parse(item["row2"].ToString()),
                            int.Parse(item["col2"].ToString()))))
                        {
                            range.Value.Value2 = item["name"].ToString();
                        }
                    }

                    int row = 1, lastCol = 1;
                    foreach (var item in listData)
                    {
                        using (var range = Disposable.Create(excel.GetRange(row, 0, row + 1, 0)))
                        {
                            range.Value.MergeCells = true;
                            range.Value.Value2 = item["title"].ToString();
                        }
                    }
                    excel.Save(filepath);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 删除或创建文件及文件夹并获得新文件路径
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <returns></returns>
        string FilePathCreateOrDelete(string filepath)
        {
            string fileDate = Utils.GetDateTime("yyyy-MM-dd"),
                   newFile = string.Empty,
                   yesterDay = Utils.GetDateTime("yyyy-MM-dd", -1),
                   dirYesterDay = Utils.ServerMapPath(Path.Combine(filepath, yesterDay));

            //得到昨天目录下的所有文件进行删除,保证磁盘空间
            if (Directory.Exists(dirYesterDay))
            {
                string[] files = Utils.GetFils(dirYesterDay);
                foreach (var file in files)
                {
                    Utils.DeleteFile(Path.Combine(dirYesterDay, file));
                }
                //删除指定目录
                Utils.DeleteDir(dirYesterDay);
            }
            filepath = Utils.ServerMapPath(Path.Combine(filepath, fileDate));
            newFile = Path.Combine(filepath, excelFile);
            Utils.CreateDirectory(filepath);
            return newFile;
        }

        #endregion




        /// <summary>
        /// 导出Xml的Excel数据
        /// </summary>
        /// <param name="OrderFinaci">数据对象</param>
        /// <param name="sheetName">表名</param>
        /// <param name="lst">列</param>
        /// <returns></returns>
        public static StringBuilder ExcelToXml(List<JObject> OrderFinaci, string sheetName, Dictionary<string, int> lst)
        {
            StringBuilder xls = new StringBuilder();
            xls.Append("<?xml version='1.0'?><?mso-application progid='Excel.Sheet'?>");
            xls.Append(@"<Workbook xmlns='urn:schemas-microsoft-com:office:spreadsheet'
                      xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:x='urn:schemas-microsoft-com:office:excel'            xmlns:ss='urn:schemas-microsoft-com:office:spreadsheet' xmlns:html='http://www.w3.org/TR/REC-html40'>");
            xls.Append(@"<DocumentProperties xmlns='urn:schemas-microsoft-com:office:office'>");
            xls.Append(@"<Author>Fxt</Author><LastAuthor>Fxt</LastAuthor>
                        <Created>" + DateTime.Now.ToString() + "</Created><Company>dadasou</Company><Version>" + DateTime.Now.Year + "</Version>");
            xls.Append("</DocumentProperties>");
            xls.Append(@"<Styles><Style ss:ID='Default' ss:Name='Normal'><Alignment ss:Vertical='Center'/>
                            <Borders/><Font ss:FontName='宋体' x:CharSet='134' ss:Size='12'/><Interior/><NumberFormat/><Protection/></Style>");
            //定义样式    
            xls.Append(@"<Style ss:ID='Header'><Font ss:FontName='宋体' x:CharSet='134' ss:Size='14' ss:Color='#000' ss:Bold='1' /></Style>");
            xls.Append(@"<Style ss:ID='border'><NumberFormat ss:Format='@'/></Style>");
            xls.Append(@"<Style ss:ID='total'><NumberFormat ss:Format='@'/><Font ss:FontName='宋体' x:CharSet='134' ss:Size='16' ss:Color='#000' ss:Bold='1' /></Style>");
            xls.Append("</Styles>");
            xls.Append("<Worksheet ss:Name='" + sheetName + "'>");
            xls.Append("<Table x:FullColumns='1' x:FullRows='1'>");
            foreach (KeyValuePair<string, int> kvp in lst)
            {
                xls.Append("<Column ss:AutoFitWidth='0' ss:Width='" + kvp.Value + "'/>");
            }
            //输出标题
            xls.Append("\r\n<Row ss:AutoFitHeight='1' ss:Height='25'>");
            foreach (KeyValuePair<string, int> kvp in lst)
            {
                xls.Append("<Cell ss:StyleID='Header'><Data ss:Type='String'>" + kvp.Key + "</Data></Cell>");
            }
            xls.Append("\r\n</Row>");
            //输出记录
            if (OrderFinaci != null)
            {
                foreach (var objitem in OrderFinaci)
                {
                    xls.Append("<Row ss:Height='22'>");
                    foreach (var tr in objitem)
                    {
                        xls.Append("<Cell ss:StyleID='border'><Data ss:Type='String'>" + tr.Value + "</Data></Cell>");
                    }
                    xls.Append("</Row>");
                }
            }
            xls.Append("<Row ss:Colspan='12' ss:Height='50'>");
            xls.Append("</Row>");
            xls.Append("</Table>");
            xls.Append("</Worksheet>");
            xls.Append("</Workbook>");
            return xls;
        }

    }
}
