using CAS.Common.MVC4;
using FxtCollateralManager.Common;
using FxtCollateralManager.Common.FxtAPI;
using FxtCommonLibrary.LibraryUtils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FxtCollateralManager.Web.Controllers
{
    public class CollateralStressTestController : BaseController
    {

        public ActionResult Index()
        {
            return View();
        }

        //价格走势分析
        [HttpPost]
        public ActionResult PriceTrend(int cid, int type, bool down, string wh)
        {
            JObject _jobj = new JObject();
            _jobj.Add("pId", 0);
            _jobj.Add("cId", cid);
            _jobj.Add("aId", 0);
            _jobj.Add("type", type);
            _jobj.Add("ptwhere", wh);
            _jobj.Add("itemarrid", CookieHelper.Get("itemarrid"));
            using (FxtAPIClient client = new FxtAPIClient())
            {
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                    "C", _GetCollateralPriceTrend, Utils.Serialize(_jobj));
                if (down)
                {
                    List<JObject> listPT = Utils.Deserialize<List<JObject>>(Utils.GetJObjectValue(result, "data"));
                    string[] array = getDownFile(string.Format("{0}.xls", DateTime.Now.ToString("yyyyMMddffffff")));
                    string[] arrays = { "市场均价", "押品均价", "市场均价涨跌幅", "押品均价涨跌幅" };
                    string field = string.Empty, fMark = string.Empty;
                    List<JObject> list = new List<JObject>();
                    for (int i = 0; i < arrays.Length; i++)
                    {
                        JObject obj = new JObject();
                        obj.Add("价格类型", arrays[i]);
                        if (i.Equals(0))
                        {
                            field = "marketavg";
                            fMark = "";
                        }
                        else if (i.Equals(1))
                        {
                            field = "collavg";
                            fMark = "";
                        }
                        else if (i.Equals(2))
                        {
                            field = "marketpricechange";
                            fMark = "%";
                        }
                        else if (i.Equals(3))
                        {
                            field = "collpricechange";
                            fMark = "%";
                        }
                        foreach (var item in listPT)
                        {
                            obj.Add(Convert.ToDateTime(item["title"].ToString()).ToString("yyyy年MM月"),
                                string.Format("{0}{1}", item[field].ToString(), fMark));
                        }
                        list.Add(obj);
                    }
                    ExcelHelper excel = new ExcelHelper(array[1]);
                    bool flag = excel.WorkbookWrite(array[0], list);
                    if (flag)
                        return Json(new { path = Utils.GetDateTime("yyyy-MM-dd"), name = array[1] });
                    else
                        return Json("");
                }
                else
                {
                    return Json(result);
                }
            }
        }

        //压力测试
        [HttpPost]
        public ActionResult StressTest(string wuyetype, string jianzhutype, string niandaitype,
            string daikuantype, string mianjitype, string nianlingtype,
            string start, string end, string twhere, int type)
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
            _jobj.Add("start", start);
            _jobj.Add("end", end);
            _jobj.Add("twhere", twhere);
            _jobj.Add("cityarrid", CookieHelper.Get("cityarrid"));
            _jobj.Add("itemarrid", CookieHelper.Get("itemarrid"));
            using (FxtAPIClient client = new FxtAPIClient())
            {
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                    "C", _GetStressTest, Utils.Serialize(_jobj));
                if (type.Equals(0))
                {
                    return Json(result);
                }
                else//导出
                {
                    string[] array = getDownFile(string.Format("{0}.xls", DateTime.Now.ToString("yyyyMMddffffff")));
                    ExcelHelper excel = new ExcelHelper(array[1]);
                    bool flag = excel.WorkbookStressTest(array[0],
                        Utils.Deserialize<List<JObject>>(Utils.GetJObjectValue(result, "data")));
                    if (flag)
                        return Json(new { path = Utils.GetDateTime("yyyy-MM-dd"), name = array[1] });
                    else
                        return Json("");
                }
            }
        }

        //风险预警
        [HttpPost]
        public ActionResult RiskWarning(int typedown, int type = 0)
        {
            JObject _jobj = new JObject();
            _jobj.Add("pId", 0);
            _jobj.Add("cId", CookieHelper.Get("mapcityarrid"));
            _jobj.Add("aId", 0);
            _jobj.Add("type", type);
            _jobj.Add("itemarrid", CookieHelper.Get("itemarrid"));
            using (FxtAPIClient client = new FxtAPIClient())
            {
                if (typedown.Equals(0))
                {                    
                    result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                        "C", _GetRiskWarning, Utils.Serialize(_jobj));
                    return Json(result);
                }
                else
                {
                    result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                        "C", _GetRikWarningToDanger, Utils.Serialize(_jobj));

                    string[] array = getDownFile(string.Format("{0}.xls", DateTime.Now.ToString("yyyyMMddffffff")));
                    ExcelHelper excel = new ExcelHelper(array[1]);
                    bool flag = excel.WorkbookStressTestToRikWarning(array[0],
                        Utils.Deserialize<List<JObject>>(Utils.GetJObjectValue(result, "data")));
                    if (flag)
                        return Json(new { path = Utils.GetDateTime("yyyy-MM-dd"), name = array[1] });
                    else
                        return Json("");
                }
            }
        }
    }
}
