using CAS.Common.MVC4;
using FxtCollateralManager.Common;
using FxtCollateralManager.Common.FxtAPI;
using FxtCommonLibrary.LibraryUtils;
using FxtNHibernate.DTODomain.FxtLoanDTO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/***
 * 作者:李晓东
 * 时间:2014.04.08
 * 摘要:押品复估
 *      2014.05.05  修改人:李晓东
 *                  修整：RiskAnalysis中的不足
 * ***/
namespace FxtCollateralManager.Web.Controllers
{
    public class CollateralReassessmentController : BaseController
    {
        //
        // GET: /CollateralReassessment/

        public ActionResult Index()
        {
            return View();
        }
        //押品复估管理
        [HttpPost]
        public ActionResult GetReassessment(int pid, int cid, int aid, int pageSize, int pageIndex)
        {
            JObject _obj = new JObject();
            _obj.Add("pId", pid);
            _obj.Add("cId", cid);
            _obj.Add("aId", aid);
            _obj.Add("pageSize", pageSize);
            _obj.Add("pageIndex", pageIndex);
            _obj.Add("cityarrid", CookieHelper.Get("cityarrid"));
            _obj.Add("itemarrid", CookieHelper.Get("itemarrid"));
            using (FxtAPIClient client = new FxtAPIClient())
            {
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                    "C", _DATAGetCollateralsByReassessment, Utils.Serialize(_obj));
                return Json(result);
            }
        }

        //获得押品的复估
        [HttpPost]
        public ActionResult GetReassessmentMonthNumber(int month, int number)
        {
            JObject _obj = new JObject();
            _obj.Add("id", number);
            _obj.Add("nMonths", month);

            using (FxtAPIClient client = new FxtAPIClient())
            {
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                    "C", _DATAGetReassessment, Utils.Serialize(_obj));
                return Json(result);
            }
        }

        //押品复估任务
        [HttpPost]
        public ActionResult ReassTask(int uploadfileid, int bankproid, int bankid, int tasktype = 0)
        {
            JObject _obj = new JObject();
            _obj.Add("Title", (tasktype == 0) ? "押品拆分" : "押品复估");
            _obj.Add("Status", 0);
            _obj.Add("Count", 0);
            _obj.Add("BankId", bankid);
            _obj.Add("OpeaterId", 1);
            _obj.Add("CreateDateTime", DateTime.Now.ToString());
            if (tasktype == 1)
            {
                _obj.Add("Url1", GetAppSettingValue("ReassUrl1"));
                _obj.Add("Url2", GetAppSettingValue("ReassUrl2"));
            }
            _obj.Add("Url3", GetAppSettingValue("ReassUrl3"));
            _obj.Add("UploadFileId", uploadfileid);
            _obj.Add("BankProjectId", bankproid);
            _obj.Add("UserId", Public.LoginInfo.Id);
            _obj.Add("TaskType", tasktype);
            JObject sendobj = new JObject();
            sendobj.Add("task", Utils.Serialize(_obj));

            using (FxtAPIClient client = new FxtAPIClient())
            {
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                    "C", _DATAAddTask, Utils.Serialize(sendobj));
                return Json(result);
            }
        }
        //人工干预复估
        [HttpPost]
        public ActionResult ChangeReass(int id, int price)
        {
            JObject _obj = new JObject();
            _obj.Add("id", id);
            _obj.Add("price", price);
            _obj.Add("oper", 0);

            using (FxtAPIClient client = new FxtAPIClient())
            {
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                    "C", _DATAUpdateReassessment, Utils.Serialize(_obj));
                return Json(result);
            }
        }

        //复估风险分析查询及导出
        [HttpPost]
        public ActionResult RiskAnalysis(int type)
        {
            JObject _jobj = new JObject();
            _jobj.Add("pId", 0);
            _jobj.Add("cId", 0);
            _jobj.Add("aId", 0);
            _jobj.Add("cityarrid", CookieHelper.Get("cityarrid"));
            _jobj.Add("itemarrid", CookieHelper.Get("itemarrid"));
            using (FxtAPIClient client = new FxtAPIClient())
            {
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                    "C", _DATAGetRiskAnalysis, Utils.Serialize(_jobj));
                if (type.Equals(0))
                {
                    return Json(result);
                }
                else
                {
                    string[] array = getDownFile(string.Format("{0}.xls", DateTime.Now.ToString("yyyyMMddffffff")));
                    ExcelHelper excel = new ExcelHelper(array[1]);
                    bool flag = excel.WorkbookStepAstrideColumnWrite(array[0], Utils
                        .Deserialize<List<JObject>>(Utils.GetJObjectValue(result, "data")));
                    if (flag)
                    {
                        return Json(new { path = Utils.GetDateTime("yyyy-MM-dd"), name = array[1] });
                    }
                    else
                    {
                        return Json("");
                    }
                }
            }
        }

        //押品明细查询
        [HttpPost]
        public ActionResult GetReassessmentDetails(string wuyetype, string jianzhutype, string niandaitype,
            string daikuantype, string mianjitype, string nianlingtype,
            int projectid, int companyid, int type, int pageIndex, int pageSize)
        {
            JObject _jobj = new JObject();
            _jobj.Add("pId", 0);
            _jobj.Add("cId", 0);
            _jobj.Add("aId", 0);
            _jobj.Add("houseType", wuyetype);
            _jobj.Add("buildingType", jianzhutype);
            _jobj.Add("buildingDate", niandaitype);
            _jobj.Add("loanAmount", daikuantype);
            _jobj.Add("buildingArea", mianjitype);
            _jobj.Add("age", nianlingtype);
            _jobj.Add("projectid", projectid);
            _jobj.Add("companyid", companyid);
            _jobj.Add("pageIndex", 0);
            _jobj.Add("pageSize", 0);
            _jobj.Add("cityarrid", CookieHelper.Get("cityarrid"));
            _jobj.Add("itemarrid", CookieHelper.Get("itemarrid"));
            using (FxtAPIClient client = new FxtAPIClient())
            {
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                    "C", _DATAReassessmentDetails, Utils.Serialize(_jobj));
                if (type.Equals(0))
                {
                    return Json(result);
                }
                else
                {
                    List<ReassessmentCollateral> list = Utils
                        .Deserialize<List<ReassessmentCollateral>>(Utils.GetJObjectValue(result, "data"));
                    List<JObject> listObj = new List<JObject>();
                    foreach (var rc in list)
                    {
                        _jobj = new JObject();
                        _jobj.Add("行政区", "");
                        _jobj.Add("物业类型", rc.PurposeCodeName);
                        _jobj.Add("面积段", rc.BuildingAreaSection);
                        _jobj.Add("建筑年代", rc.JianZhuYearSection);
                        _jobj.Add("放贷日期", rc.LoanDate != null && rc.LoanDate.Value <= DateTime.Now ?
                            string.Format("{0}年", rc.LoanDate.Value.ToString("yyyy")) :"");
                        _jobj.Add("贷款额度", rc.LoanAmountSection);
                        _jobj.Add("贷款人年龄段", rc.AgeSection);

                        _jobj.Add("押品编号", rc.Number);
                        _jobj.Add("分行", rc.Branch);
                        _jobj.Add("押品类型", rc.PurposeName);
                        _jobj.Add("押品名称", rc.Name);
                        _jobj.Add("面积", rc.BuildingArea);
                        _jobj.Add("楼盘名称", rc.ProjectName);
                        _jobj.Add("*市", rc.CityName);
                        _jobj.Add("*县/区", rc.AreaName);
                        _jobj.Add("楼栋名", rc.BuildingNumber);
                        _jobj.Add("房号", rc.RoomNumber);
                        _jobj.Add("押品物业类型", rc.PurposeCodeName);
                        _jobj.Add("原贷款额", rc.LoanAmount);
                        _jobj.Add("贷款余额", rc.LoanAmount);
                        _jobj.Add("原评估值", rc.OldRate);
                        _jobj.Add("复评估值", rc.Price);
                        _jobj.Add("复评估方法", rc.CalculationMode);
                        _jobj.Add("涨跌幅", string.Format("{0}%", rc.PriceChange));
                        _jobj.Add("原抵押率", rc.OldMortgageRates);
                        _jobj.Add("现抵押率", rc.ArrivedLoanRates);

                        string strStatus = "安全";
                        if (rc.ArrivedLoanRates > (decimal)0.6 && rc.ArrivedLoanRates <= (decimal)0.8)
                        {
                            strStatus = "正常";
                        }
                        else if (rc.ArrivedLoanRates > (decimal)0.8 && rc.ArrivedLoanRates <= (decimal)1)
                        {
                            strStatus = "风险";
                        }
                        else if (rc.ArrivedLoanRates > (decimal)1)
                        {
                            strStatus = "危险";
                        }
                        _jobj.Add("风险状况", strStatus);
                        listObj.Add(_jobj);
                    }

                    string[] array = getDownFile(string.Format("{0}.xls", DateTime.Now.ToString("yyyyMMddffffff")));
                    ExcelHelper excel = new ExcelHelper(array[1]);
                    bool flag = excel.WorkbookWrite(array[0], listObj);
                    if (flag)
                    {
                        return Json(new { path = Utils.GetDateTime("yyyy-MM-dd"), name = array[1] });
                    }
                    else
                    {
                        return Json("");
                    }
                }
            }
        }
    }
}
