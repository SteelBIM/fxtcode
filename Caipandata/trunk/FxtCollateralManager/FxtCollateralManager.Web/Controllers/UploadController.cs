using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FxtCollateralManager.Common;
using FxtCommonLibrary.LibraryUtils;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Model = FxtNHibernate.FxtLoanDomain.Entities;
using FxtCollateralManager.Common.FxtAPI;
using FxtNHibernate.FxtDataUserDomain.Entities;
using FxtNHibernate.FxtLoanDomain.Entities;
using FxtNHibernate.DATProjectDomain.Entities;
using FxtNHibernate.DTODomain.FxtLoanDTO;
using CAS.Common.MVC4;

/**
 * 作者:李晓东
 * 摘要:新建 2013.12.27
 *      修改人:李晓东  2014.01.20 
 *             1.fileUpload修改 2.GetFiles获得文件列表 3.fileDelete删除文件
 *      修改人:李晓东  2014.03.07
 *             修改:标准化"进行中" 导出能全部导出
 *      修改人:贺黎亮  2014.03.26
 *             添加:MapExcelAllDown方法 用于 押品监测"地图查询统计" 全部导出
 *      修改人:贺黎亮  2014.04.01
 *             添加:CFCountAllDown方法 押品分类统计统计导出" 全部导出
 *             添加:CollDetialsAllDown方法  押品明细 全部导出
  *      修改人:贺黎亮  2014.04.01
 *             修改 押品明细导出-物业类型会导出为0的问题
 *             添加 //押品资产价值动态监测-押品分类统计统计导出方法 AssetsCFCountAllDown
 *      2014.05.07 修改人:李晓东
                   修改:CollDetialsAllDown 押品监测明细查询
 *        2014.06.11 修改人:贺黎亮
                   修改:fileUpload方法 添加proid参数,修改UpLoads,GetFiles方法
 * **/
namespace FxtCollateralManager.Web.Controllers
{
    public class UploadController : BaseController
    {
        //
        // GET: /Upload/

        public ActionResult Index()
        {
            return View();
        }

        //押品进行中导出
        [HttpPost]
        public ActionResult ExcelDown(string data, int pageSize, int pageIndex, int type, string column, int fileId)
        {
            string[] array = getDownFile();
            bool flag = false;


            string[] dataSplit = data.Split('&');
            JObject jcolumn = JObject.Parse(column);
            string filename = string.Empty;
            Dictionary<int, string> list = new Dictionary<int, string>();
            foreach (var ds in dataSplit)
            {
                if (Utils.IsNullOrEmpty(ds))
                    continue;
                var curDS = ds.Split('=');
                if (!curDS[0].ToString().Equals("type") && !curDS[0].ToString().Equals("filename"))
                {
                    string strName = string.Empty;
                    if (curDS[0].ToString().Equals("Number"))
                    {
                        strName = "押品编号";
                    }
                    else if (curDS[0].ToString().Equals("Branch"))
                    {
                        strName = "分行";
                    }
                    else if (curDS[0].ToString().Equals("PurposeCode"))
                    {
                        strName = "押品类型";
                    }
                    else if (curDS[0].ToString().Equals("Name"))
                    {
                        strName = "押品名称";
                    }
                    else if (curDS[0].ToString().Equals("BuildingArea"))
                    {
                        strName = "面积";
                    }
                    else if (curDS[0].ToString().Equals("Address"))
                    {
                        strName = "押品地址";
                    }
                    list.Add(Convert.ToInt32(curDS[1]), strName);
                }
                else if (curDS[0].ToString().Equals("filename"))
                {
                    filename = Utils.ServerMapPath(Path.Combine(GetUploadUrl(), curDS[1]));
                }
            }
            //拆分出信息
            ExcelHelper excelR = new ExcelHelper(filename);
            List<DataCollaterals> rlist = null;
            if (type.Equals(0))//导出当前页
            {
                rlist = excelR.ExcelStandardization(list, pageSize, pageIndex, true) as List<DataCollaterals>;
            }
            else//导出全部
            {
                rlist = excelR.ExcelStandardization(list, 0, 0) as List<DataCollaterals>;
            }
            //导出拆分信息
            ExcelHelper excelW = new ExcelHelper(array[1]);
            List<JObject> newList = new List<JObject>();
            List<JObject> newSaveList = new List<JObject>();
            foreach (var item in rlist)
            {
                var parray = item.GetType().GetProperties();
                JObject _newJobject = new JObject();
                JObject _newSaveJobject = new JObject();
                foreach (var p in parray)
                {
                    foreach (var _jobj in jcolumn)
                    {
                        if (_jobj.Key.Equals(p.Name))
                        {
                            string strVal = Utils.ObjectIsNull(p.GetValue(item, null));
                            _newJobject.Add(_jobj.Value.ToString(), strVal);
                            _newSaveJobject.Add(_jobj.Key, strVal);

                            if (p.Name.ToLower().Equals("provinceid") &&
                           _jobj.Key.ToLower().Equals("provinceid") &&
                           _newJobject[_jobj.Value.ToString()] != null)
                            {
                                _newJobject[_jobj.Value.ToString()] =
                                    Utils.ObjectIsNull(item.GetType().GetProperty("ProvinceName").GetValue(item, null));
                            }
                            else if (p.Name.ToLower().Equals("cityid") &&
                                _jobj.Key.ToLower().Equals("cityid") &&
                                _newJobject[_jobj.Value.ToString()] != null)
                            {
                                _newJobject[_jobj.Value.ToString()] =
                                    Utils.ObjectIsNull(item.GetType().GetProperty("CityName").GetValue(item, null));
                            }
                            else if (p.Name.ToLower().Equals("areaid") &&
                                _jobj.Key.ToLower().Equals("areaid") &&
                                _newJobject[_jobj.Value.ToString()] != null)
                            {
                                _newJobject[_jobj.Value.ToString()] =
                                    Utils.ObjectIsNull(item.GetType().GetProperty("AreaName").GetValue(item, null));
                            }
                        }
                    }
                }
                newSaveList.Add(_newSaveJobject);
                newList.Add(_newJobject);
            }
            using (FxtAPIClient client = new FxtAPIClient())
            {
                foreach (var itemSave in newSaveList)
                {
                    DataCollateral model = new DataCollateral() { Status = 2 };
                    Type t = model.GetType();

                    foreach (var _itemSave in itemSave)
                    {
                        if (t.GetProperty(_itemSave.Key).PropertyType == typeof(String))
                            t.GetProperty(_itemSave.Key).SetValue(model, _itemSave.Value.Value<string>(), null);
                        else if (t.GetProperty(_itemSave.Key).PropertyType == typeof(decimal))
                            t.GetProperty(_itemSave.Key).SetValue(model, _itemSave.Value.Value<decimal>(), null);
                        else if (t.GetProperty(_itemSave.Key).PropertyType == typeof(int))
                            t.GetProperty(_itemSave.Key).SetValue(model, _itemSave.Value.Value<int>(), null);
                    }

                    JObject _obj = new JObject();
                    _obj.Add("dataCollateral", Utils.Serialize(model));
                    _obj.Add("uploadFileId", fileId);
                    client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                        "C", _DATACollateralAdd, Utils.Serialize(_obj));
                }
            }
            if (type.Equals(0))//导出当前页
            {
                flag = excelW.WorkbookWrite(array[0], newList, pageIndex);
            }
            else
            {
                flag = excelW.WorkbookWrite(array[0], newList, 0);
            }
            if (flag)
                return Json(new
                {
                    path = Utils.GetDateTime("yyyy-MM-dd"),
                    name = array[1]
                });
            else
                return Json("");
        }

        //押品已标准化导出
        [HttpPost]
        public ActionResult ExcelAllDown()
        {
            string[] array = getDownFile(string.Format("{0}.xls", DateTime.Now.ToString("yyyyMMddffffff")));
            JObject jobject = new JObject();
            jobject.Add("cityarrid", CookieHelper.Get("cityarrid"));
            jobject.Add("itemarrid", CookieHelper.Get("itemarrid"));
            List<JObject> _listData = new List<JObject>();
            using (FxtAPIClient client = new FxtAPIClient())
            {
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey), 
                    "C", _DATAGetAllDataCollateral, Utils.Serialize(jobject));
                string data = string.Empty;
                List<CollateralMonitorDetails> listCMDCollateral = 
                    Utils.Deserialize<List<CollateralMonitorDetails>>(Utils.GetJObjectValue(result, "data"));
                foreach (var item in listCMDCollateral) {
                    jobject = new JObject(); 
                    jobject.Add("项目名称", item.BankProjectName);
                    jobject.Add("押品编号", item.Number);
                    jobject.Add("分行", item.Branch);
                    jobject.Add("押品类型", item.PurposeName);
                    jobject.Add("押品名称", item.Name);
                    jobject.Add("面积", item.BuildingArea);
                    jobject.Add("押品地址", item.Address);
                    jobject.Add("省份", item.ProvinceName);
                    jobject.Add("城市", item.CityName);
                    jobject.Add("行政区", item.AreaName);
                    jobject.Add("路", item.Road);
                    jobject.Add("号", item.RoadNumber);
                    jobject.Add("楼盘名", item.ProjectName);
                    jobject.Add("分期", item.Installment);
                    jobject.Add("楼栋", item.BuildingNumber);
                    jobject.Add("楼层", item.FloorNumber);
                    jobject.Add("房号", item.RoomNumber);
                    _listData.Add(jobject);
                }

                ExcelHelper excel = new ExcelHelper(array[1]);
                bool flag = excel.WorkbookWrite(array[0], _listData);
                if (flag)
                    return Json(new { path = Utils.GetDateTime("yyyy-MM-dd"), name = array[1] });
                else
                    return Json("");
            }
        }


        //地图查询统计导出
        [HttpPost]
        public ActionResult MapExcelAllDown(int cid,
            string wuyetype, string jianzhutype, string niandaitype,
            string daikuantype, string mianjitype, string nianlingtype,
            string mapselecttype, string type)
        {
            string[] array = getDownFile(string.Format("{0}.xls", DateTime.Now.ToString("yyyyMMddffffff")));
            List<JObject> _listData = new List<JObject>();
            List<JObject> _explistData = new List<JObject>();
            using (FxtAPIClient client = new FxtAPIClient())
            {
                string data = string.Empty;
                JObject obj = new JObject();
                obj.Add("pId", 0);
                obj.Add("cId", cid);
                obj.Add("aId", 0);
                obj.Add("houseType", wuyetype);
                obj.Add("buildingType", jianzhutype);
                obj.Add("buildingDate", niandaitype);
                obj.Add("loanAmount", daikuantype);
                obj.Add("buildingArea", mianjitype);
                obj.Add("age", nianlingtype);
                obj.Add("type", type);
                obj.Add("itemarrid", CookieHelper.Get("itemarrid"));
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                    "C", _CollateralCountByPCA, Utils.Serialize(obj));
                _listData = Utils.Deserialize<List<JObject>>(Utils.GetJObjectValue(result, "data"));
                Dictionary<string, string> mapselecttypeDic = new Dictionary<string, string>();
                mapselecttypeDic.Add("贷款总额", "CollTotal");
                mapselecttypeDic.Add("贷款余额", "CollOver");
                mapselecttypeDic.Add("押品数量", "CollNumberCount");
                mapselecttypeDic.Add("押品面积", "CollateralArea");
                mapselecttypeDic.Add("原估价值", "OriginalValue");
                mapselecttypeDic.Add("现估价值", "AssessedValue");
                mapselecttypeDic.Add("担保金额", "AmountValue");
                mapselecttypeDic.Add("原抵押率", "OriginalRate");
                mapselecttypeDic.Add("现抵押率", "MortgageRate");
                string[] selecttype = mapselecttype.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToArray();
                if (_listData != null && _listData.Count > 0)
                {
                    foreach (JObject objitem in _listData)
                    {
                        objitem["Area"] = JObject.Parse(objitem["Area"].ToString())["AreaName"];
                        obj = new JObject();
                        obj["行政区"] = objitem["Area"];
                        foreach (string seitem in selecttype)
                        {
                            if (mapselecttypeDic.ContainsKey(seitem))
                            {
                                obj[seitem] = objitem[mapselecttypeDic[seitem]];
                            }
                        }
                        _explistData.Add(obj);
                    }
                    obj = new JObject();
                    obj["行政区"] = "合计";
                    foreach (string selitem in selecttype)
                    {
                        if (mapselecttypeDic.ContainsKey(selitem))
                        {
                            obj[selitem] = _explistData.Sum(o => o[selitem] != null ? decimal.Parse(o[selitem].ToString()) : 0);
                        }
                    }
                    _explistData.Add(obj);
                }
                else
                {
                    obj = new JObject();
                    obj["行政区"] = "";
                    foreach (string seitem in selecttype)
                    {
                        if (mapselecttypeDic.ContainsKey(seitem))
                        {
                            obj[seitem] = "";
                        }
                    }
                    _explistData.Add(obj);
                }
                data = Utils.Serialize(_explistData);
                ExcelHelper excel = new ExcelHelper(array[1]);
                bool flag = excel.WorkbookWrite(array[0], Utils.Deserialize<List<JObject>>(data));
                if (flag)
                    return Json(new { path = Utils.GetDateTime("yyyy-MM-dd"), name = array[1] });
                else
                    return Json("");
            }
        }

        [HttpPost]
        //押品分类统计统计导出
        public ActionResult CFCountAllDown(string requirement, string start, string end, string requirename)
        {
            string[] array = getDownFile(string.Format("{0}.xls", DateTime.Now.ToString("yyyyMMddffffff")));
            List<JObject> _listData = new List<JObject>();
            List<JObject> _explistData = new List<JObject>();
            using (FxtAPIClient client = new FxtAPIClient())
            {
                string data = string.Empty;
                JObject obj = new JObject();
                obj.Add("pId", 0);
                obj.Add("cId", 0);
                obj.Add("aId", 0);
                obj.Add("requirement", requirement);
                obj.Add("start", start);
                obj.Add("end", end);
                obj.Add("type", 0);
                obj.Add("cityarrid", CookieHelper.Get("cityarrid"));
                obj.Add("itemarrid", CookieHelper.Get("itemarrid"));
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                    "C", _GetCollateralClassification, Utils.Serialize(obj));
                _listData = Utils.Deserialize<List<JObject>>(Utils.GetJObjectValue(result, "data"));
                Dictionary<string, string> selecttypeDic = new Dictionary<string, string>();
                selecttypeDic.Add(requirename, "name");
                selecttypeDic.Add("押品数量", "count");
                selecttypeDic.Add("押品面积", "buildingarea");
                selecttypeDic.Add("原估价值", "oldrate");
                selecttypeDic.Add("担保金额", "guaranteeprice");
                selecttypeDic.Add("原平均抵押率", "oldavergerates");
                if (_listData != null && _listData.Count > 0)
                {
                    foreach (JObject objitem in _listData)
                    {
                        obj = new JObject();
                        foreach (string seitem in selecttypeDic.Keys)
                        {
                            obj[seitem] = "";
                            if (selecttypeDic.ContainsKey(seitem))
                            {
                                obj[seitem] = objitem[selecttypeDic[seitem]];
                            }
                        }
                        _explistData.Add(obj);
                    }
                }
                data = Utils.Serialize(_explistData);
                ExcelHelper excel = new ExcelHelper(array[1]);
                bool flag = excel.WorkbookWrite(array[0], Utils.Deserialize<List<JObject>>(data));
                if (flag)
                    return Json(new { path = Utils.GetDateTime("yyyy-MM-dd"), name = array[1] });
                else
                    return Json("");
            }
        }



        [HttpPost]
        //押品资产价值动态监测-押品分类统计导出
        public ActionResult AssetsCFCountAllDown(string requirement, string start, string end, string requirename)
        {
            string[] array = getDownFile(string.Format("{0}.xls", DateTime.Now.ToString("yyyyMMddffffff")));
            List<JObject> _listData = new List<JObject>();
            List<JObject> _explistData = new List<JObject>();
            using (FxtAPIClient client = new FxtAPIClient())
            {
                string data = string.Empty;
                JObject obj = new JObject();
                obj.Add("pId", 0);
                obj.Add("cId", 0);
                obj.Add("aId", 0);
                obj.Add("requirement", requirement);
                obj.Add("start", start);
                obj.Add("end", end);
                obj.Add("type", 1);
                obj.Add("cityarrid", CookieHelper.Get("cityarrid"));
                obj.Add("itemarrid", CookieHelper.Get("itemarrid"));
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                    "C", _GetCollateralClassification, Utils.Serialize(obj));
                _listData = Utils.Deserialize<List<JObject>>(Utils.GetJObjectValue(result, "data"));
                Dictionary<string, string> selecttypeDic = new Dictionary<string, string>();
                selecttypeDic.Add(requirename, "name");
                selecttypeDic.Add("押品数量", "count");
                selecttypeDic.Add("押品面积", "buildingarea");
                selecttypeDic.Add("原估价值", "oldrate");
                selecttypeDic.Add("现估价值", "rate");
                selecttypeDic.Add("担保金额", "guaranteeprice");
                selecttypeDic.Add("原平均抵押率", "oldavergerates");
                selecttypeDic.Add("现平均抵押率", "avergerates");
                if (_listData != null && _listData.Count > 0)
                {
                    foreach (JObject objitem in _listData)
                    {
                        obj = new JObject();
                        foreach (string seitem in selecttypeDic.Keys)
                        {
                            obj[seitem] = "";
                            if (selecttypeDic.ContainsKey(seitem))
                            {
                                obj[seitem] = objitem[selecttypeDic[seitem]];
                            }
                        }
                        _explistData.Add(obj);
                    }
                }
                data = Utils.Serialize(_explistData);
                ExcelHelper excel = new ExcelHelper(array[1]);
                bool flag = excel.WorkbookWrite(array[0], Utils.Deserialize<List<JObject>>(data));
                if (flag)
                    return Json(new { path = Utils.GetDateTime("yyyy-MM-dd"), name = array[1] });
                else
                    return Json("");
            }
        }

        [HttpPost]
        //明细查询
        public ActionResult CollDetialsAllDown(string wuyetype, string jianzhutype, string niandaitype,
            string daikuantype, string mianjitype, string nianlingtype,
            int projectid, int companyid, string start, string end)
        {
            string[] array = getDownFile(string.Format("{0}.xls", DateTime.Now.ToString("yyyyMMddffffff")));
            List<JObject> _explistData = new List<JObject>();
            using (FxtAPIClient client = new FxtAPIClient())
            {
                string data = string.Empty;
                JObject obj = new JObject();
                obj.Add("pId", 0);
                obj.Add("cId", 0);
                obj.Add("aId", 0);
                obj.Add("houseType", wuyetype);
                obj.Add("buildingType", jianzhutype);
                obj.Add("buildingDate", niandaitype);
                obj.Add("loanAmount", daikuantype);
                obj.Add("buildingArea", mianjitype);
                obj.Add("age", nianlingtype);
                obj.Add("projectid", projectid);
                obj.Add("companyid", companyid);
                obj.Add("start", start);
                obj.Add("end", end);
                obj.Add("pageIndex", 0);
                obj.Add("pageSize", 0);
                obj.Add("cityarrid", CookieHelper.Get("cityarrid"));
                obj.Add("itemarrid", CookieHelper.Get("itemarrid"));
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                    "C", _GetDetials, Utils.Serialize(obj));
                List<CollateralMonitorDetails> listCMD = Utils
                    .Deserialize<List<CollateralMonitorDetails>>(Utils.GetJObjectValue(result, "data"));
                foreach (var item in listCMD)
                {
                    obj = new JObject();
                    obj.Add("项目名称", item.BankProjectName);
                    obj.Add("押品编号", item.Number);
                    obj.Add("管理机构", item.Agency);
                    obj.Add("押品类型", item.PurposeName);
                    obj.Add("押品名称", item.Name);
                    obj.Add("面积", item.BuildingArea);
                    obj.Add("楼盘名称", item.ProjectName);
                    obj.Add("*市", item.CityName);
                    obj.Add("*县/区", item.AreaName);
                    obj.Add("楼栋名", item.BuildingNumber);
                    obj.Add("房号", item.RoomNumber);
                    obj.Add("贷款金额", item.LoanAmount);
                    obj.Add("贷款余额", item.LoanBalance);
                    obj.Add("物业类型", item.PurposeCodeName);
                    _explistData.Add(obj);
                }
                ExcelHelper excel = new ExcelHelper(array[1]);
                bool flag = excel.WorkbookWrite(array[0], _explistData);
                if (flag)
                    return Json(new { path = Utils.GetDateTime("yyyy-MM-dd"), name = array[1] });
                else
                    return Json("");
            }


        }



        //导入
        [HttpPost]
        public ActionResult ExcelUp(string filename)
        {
            string filepath = Utils.ServerMapPath(Path.Combine(GetUploadUrl(), filename));
            ExcelHelper excel = new ExcelHelper(filepath);
            object objResolve = excel.ExcelResolve();
            Utils.DeleteFile(filepath);
            return Json(objResolve);
        }

        //上传文件
        [HttpPost]
        public ActionResult fileUpload(HttpPostedFileBase file, string type="", int proid=0,int bankid=0)
        {
            dynamic dy = saveFile(file);
            if (dy.flag)//上传成功
            {
                type = type.ToLower();
                if (type.Equals("list"))//要更新列表的
                {
                    return Json(fileServerSave(dy.filepath, dy.filename, file, proid,bankid));
                }
                else if (type.Equals("up"))//上传解析Excel
                {
                    return Json(dy);
                }
            }
            return null;
        }


        //保存文件到物理磁盘
        protected dynamic saveFile(HttpPostedFileBase file)
        {
            try
            {
                string DirectoryName = Utils.ServerMapPath(GetUploadUrl()),
                       NewFileName = FileNewName(Path.GetExtension(file.FileName)),
                       FileName = Path.Combine(DirectoryName, NewFileName);
                if (!Directory.Exists(DirectoryName))
                {
                    Directory.CreateDirectory(DirectoryName);
                }

                file.SaveAs(FileName);
                return new
                {
                    flag = true,
                    filepath = DirectoryName,
                    filename = NewFileName
                };
            }
            catch (Exception exe)
            {
                return new
                {
                    flag = false,
                    message = exe.Message
                };
            }
        }


        //获得标准化文件列
        [HttpPost]
        public ActionResult fileColumns(string filename)
        {
            string filePath = Utils.ServerMapPath(Path.Combine(GetUploadUrl(), filename));
            ExcelHelper excel = new ExcelHelper(filePath);
            return Json(excel.ExcelColumns());
        }

        //执行标准化
        [HttpPost]
        public ActionResult fileExcute(int Number, int Branch, int PurposeCode,
            int Name, int BuildingArea, int Address, string filename,
            int pageSize, int pageIndex, int type)
        {
            string filePath = Utils.ServerMapPath(Path.Combine(GetUploadUrl(), filename));
            Dictionary<int, string> list = new Dictionary<int, string>();
            list.Add(Number, "押品编号");
            list.Add(Branch, "分行");
            list.Add(PurposeCode, "押品类型");
            list.Add(Name, "押品名称");
            list.Add(BuildingArea, "面积");
            list.Add(Address, "押品地址");
            ExcelHelper excel = new ExcelHelper(filePath);
            object rlist = null;
            if (type.Equals(0))//执行当前页
            {
                rlist = excel.ExcelStandardization(list, pageSize, pageIndex);
            }
            else//执行全部
            {
                rlist = excel.ExcelStandardization(list, 0, 0);
            }
            return Json(rlist);
        }

       
        //获得属于某个文件的押品总数
        [HttpPost]
        public ActionResult GetFileCollateralCount(int fileId)
        {
            using (FxtAPIClient client = new FxtAPIClient())
            {
                JObject _jobject = new JObject();
                _jobject.Add("fileId", fileId);
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                    "C", "GetCountCollateralByFileId", Serialize(_jobject));
                return Json(result);
            }
        }
       

        #region 文件

        //保存文件到服务器
        object fileServerSave(string path, string filename, HttpPostedFileBase file
            , int proid,int bankselid)
        {
            using (FxtAPIClient client = new FxtAPIClient())
            {
                string filetype = Path.GetExtension(file.FileName);
                SysUploadFile uploadFile = new SysUploadFile()
                {
                    UserId = "ddd",
                    FilePath = filename,
                    FileType = filetype,
                    FileSize = file.ContentLength,
                    Name = file.FileName.Replace(filetype, ""),
                    Count = new ExcelHelper(Path.Combine(path, filename)).GetExcelCount(),
                    BankProid = proid,
                    BankId = bankselid
                };
                JObject obj = new JObject();
                obj.Add("model", Serialize(uploadFile));
                obj.Add("otype", "C");
                result=client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                  "C", _Uploads, Utils.Serialize(obj)).ToString();
            }
            return new
            {
                type = result,
                message = file.FileName
            };
        }

        //获得文件列表
        [HttpPost]
        public ActionResult GetFiles(int pageSize, int pageIndex
            , int bankid = 0, int proid = 0, string key = "")
        {
            using (FxtAPIClient client = new FxtAPIClient())
            {
                JObject obj = new JObject();
                obj.Add("uploadfile", "");
                obj.Add("pageSize", pageSize);
                obj.Add("pageIndex", pageIndex);
                obj.Add("bankid", bankid);
                obj.Add("proid", proid);
                obj.Add("key", key);
                obj.Add("customerid", Public.LoginInfo.CustomerId);
                obj.Add("customertype", Public.LoginInfo.CustomerType);
                return Json(client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                   "C", _GetUploads, Utils.Serialize(obj)).ToString());
            }
        }

        //删除文件
        [HttpPost]
        public ActionResult fileDelete(string filename, int id)
        {
            string FileName = Utils.ServerMapPath(Path.Combine(GetUploadUrl(), filename));
            using (FxtAPIClient client = new FxtAPIClient())
            {
                JObject _jobject = new JObject();
                _jobject.Add("id", id);
                JObject obj = new JObject();
                obj.Add("model", Serialize(_jobject));
                obj.Add("otype", "D");
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                "C", _Uploads, Utils.Serialize(obj)).ToString().ToLower();
                if (result.ToString()=="true")
                    Utils.DeleteFile(FileName);
            }
            return Json(ResultServerJson(null, (result.ToString()=="true")));
        }
        #endregion
    }
}
