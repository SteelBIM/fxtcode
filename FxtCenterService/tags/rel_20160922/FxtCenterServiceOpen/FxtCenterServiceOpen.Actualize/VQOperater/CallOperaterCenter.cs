using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Data;
using Newtonsoft.Json.Linq;


using CAS.Common;
using CAS.Entity;
using FXT.VQ.VQCenter;
using FXT.VQ.VQCenter.VQCenterService;
using FxtCenterServiceOpen.Actualize.Common;
using FxtCenterServiceOpen.Actualize.UserCenter;
using FxtCenterServiceOpen.Actualize.DataCenter;

/**************************************************
 * 创建时间：2015-12-03
 * 创建人：wb
 * 说明：目前接口只发送 自动估价数据 至运营中心
 * 1.通过signname取CompanyID...(用户中心)
 * 2.通过盘栋房ID取名称 (数据中心)
 * 
 * ###用户自动估价请求信息###
 * {
        "sinfo": {
            "functionname": "ghafvq",
            "appid": "调试的时候告知",
            "apppwd": "调试的时候告知",
            "signname": "调试的时候告知",
            "time": "时间戳",
            "code": "加密方法见Sinfo,code参数说明"
        },
        "info": {
            "uinfo": {
                "username": "",
                "token": ""
            },
            "appinfo": {
                "splatype": "win",
                "stype": "vq",
                "version": "1.0",
                "vcode": "1",
                "systypecode": "1003036",
                "channel": "",
                "IP": "接口是跟IP绑定的，固定IP才能调用接口"
            },
            "funinfo": {
                "cityid": "城市ID",
                "houseid": " 房号ID",
                "totalfloor": "总楼层",
                "floornumber": "所在层",
                "projectid": "楼盘ID",
                "buildingid": "楼栋ID",
                "frontcode": "朝向",
                "buildingarea": "面积",
                "unitprice": "楼盘均价",
                "plprice": "底层基准房价",
                "pmprice": "多层基准房价",
                "psprice": "小高层基准房价",
                "phprice": " 高层基准房价"
            }
        }
    }
 * 
 * 
 * *###自动估价传入的信息点##
    cityid 必填 [ "int" ] // 城市ID
    projectid 必填 [ "string" ] // 楼盘ID
    buildingid 必填 [ "string" ] // 楼栋ID
    houseid 必填 [ "string" ] // 房号ID
    totalfloor 必填 [ "int" ] // 总楼层
    floornumber 必填 [ "int" ] // 所在层
    frontcode 必填 [ "int" ] // 朝向
    buildingarea 必填 [ "float" ] // 面积
    unitprice 必填 [ "int" ] // 楼盘基准房价
    plprice 必填 [ "int" ] // 底层基准房价
    pmprice 必填 [ "int" ] // 多层基准房价
    psprice 必填 [ "int" ] // 小高层基准房价
    phprice 必填 [ "int" ] // 高层基准房价

 * * ###自动估价输出的信息点###
 *  unitprice = 0;
    totalprice = 0;//总价
    estimable = 0;//是否可估
 * ************************************************/


namespace FxtCenterServiceOpen.Actualize.VQOperater
{
    public class CallOperaterCenter
    {

        private delegate bool AddHandler(string sinfo, string info, string result);

        /// <summary>
        /// 异步 发送自动估价记录至运营中心
        /// </summary>
        /// <param name="sinfo">客户安全参数</param>
        /// <param name="info">传入的请求信息</param>
        /// <param name="result">返回的结果</param>
        public static void QuerySendToCenterAsync(string sinfo, string info, string result)
        {
            AddHandler handler = new AddHandler(SendToVQOperater);
            IAsyncResult res = handler.BeginInvoke(sinfo, info, result, null, "AsycState:OK");
        }

        /// <summary>
        /// 发送数据至运营中心
        /// </summary>
        /// <param name="sinfo"></param>
        /// <param name="info"></param>
        /// <param name="result"></param>
        /// <param name="noLog"></param>
        /// <returns></returns>
        private static bool SendToVQOperater(string sinfo, string info, string result, bool noLog = false)
        {
            try
            {
                string funtionname;
                JObject objSinfo;
                try
                {
                    objSinfo = JObject.Parse(sinfo);
                    funtionname = objSinfo.Value<string>("functionname");
                }
                catch(Exception ex)
                {
                    LogHelper.Error(ex);
                    return false;
                }
                //LogHelper.Info("sinfo:" + sinfo);
                //LogHelper.Info("info:" + info);
                //LogHelper.Info("result:" + result);
                //LogHelper.Info("funtionname:" + funtionname);

                #region 自动估价接口  ###说明：目前只记录自动估价 后续需要记录其他接口时 创建分支
                if (funtionname == "ghafvq") //自动估价
                {
                    JsonReturnData jrd = new JsonReturnData();
                    string outJson = "";

                    #region 用户请求信息
                    JObject objinfo = JObject.Parse(info);
                    JObject funinfo = objinfo["funinfo"] as JObject;
                    JObject appinfo = objinfo["appinfo"] as JObject;
                    JObject uinfo = objinfo["uinfo"] as JObject;

                    JObject queryRes = JObject.Parse(result);
                    if (queryRes.Value<int>("returntype") != 1)//数据中心未正常返回
                        return false;

                    if (funinfo.Property("cityid") == null || funinfo.Property("cityid").Value.ToString() == "")
                        return false;

                    if (funinfo.Property("projectid") == null || funinfo.Property("projectid").Value.ToString() == "")
                        return false;

                    #endregion


                    #region 估价返回信息
                    decimal unitprice = 0;
                    decimal totalprice = 0;//总价
                    //decimal estimable = 0;//是否可估

                    unitprice = JObject.Parse(queryRes["data"].ToString()).Value<decimal>("unitprice");
                    totalprice = JObject.Parse(queryRes["data"].ToString()).Value<decimal>("totalprice");
                    #endregion

                    #region 获取调用公司信息
                    int companyid = 0;
                    string companyCode = "";
                    string companyName = "";
                    string signname = objSinfo.Value<string>("signname");
                    //获取公司信息
                    CompanyModel company = CallUserCenter.GetCompanyModelBySignName(signname, out jrd, out outJson);
                    if (company != null)
                    {
                        companyid = company.CompanyID;
                        companyCode = company.CompanyCode;
                        companyName = company.CompanyName;
                    }
                    else
                    {
                        LogHelper.Info(" SignName为获取到:" + signname + " 用户中心提示:" + jrd.returntext);

                        throw new Exception(@"通过客户传入的SignName从用户中心取客户的ID：未取到数据！
                                            因客户传入SignName有误或调用用户中心失败！
                                            (客户Signname:" + signname + " 用户中心提示:" + jrd.returntext + ")");
                    }
                    #endregion

                    #region 组合运营中心所要数据
                    //运营中心接口实现
                    PropertyMain main = new PropertyMain();
                    //主要信息
                    main.caseid = companyCode;
                    main.serialid = "0";
                    main.companyid = companyid;
                    main.entrustsource = 5003;

                    main.entrusttype = 3002;//自动估价
                    //物业信息
                    List<Property> property = new List<Property>();
                    Property pro = new Property();

                    pro.propertype = 1031001;
                    pro.bankpropertyid = companyCode;
                    pro.bankserialid = "0";

                    pro.cityid = funinfo.Value<int>("cityid");
                    //客户传的ID有加密
                    string projectid = funinfo.Value<string>("projectid");
                    string buildingid = funinfo.Value<string>("buildingid");
                    string houseid = funinfo.Value<string>("houseid");

                    int projectidint = 0;
                    int buildingidint = 0;
                    int houseidint = 0;
                    if (projectid != null && projectid != "" && projectid != "0")
                        projectidint = EncryptHelper.ProjectIdDecrypt(projectid);
                    if (buildingid != null && buildingid != "" && buildingid != "0")
                        buildingidint = EncryptHelper.ProjectIdDecrypt(buildingid);
                    if (houseid != null && houseid != "" && houseid != "0")
                        houseidint = EncryptHelper.ProjectIdDecrypt(houseid);

                    pro.projectid = projectidint;
                    pro.buildingid = buildingidint;
                    pro.floornumber = funinfo.Value<string>("floornumber");
                    pro.houseid = houseidint;

                    if (funinfo.Property("buildingarea") == null || funinfo.Property("buildingarea").Value.ToString() == "")
                        pro.buildingarea = 0;
                    else
                        pro.buildingarea = funinfo.Value<decimal>("buildingarea");
                    //自动估价类型
                    pro.querypricetype = 0; //0房号 1人工 2楼层估价 3无询价业务
                    pro.autopricedate = DateTime.Now;
                    pro.autounprice = unitprice;
                    pro.autototalprice = totalprice;

                    if (funinfo.Property("totalfloor") == null || funinfo.Property("totalfloor").Value.ToString() == "")
                        pro.totalfloor = 0;
                    else
                        pro.totalfloor = funinfo.Value<int>("totalfloor");
                    //以下接口未定义
                    //pro.frontcode;
                    //pro.unitprice;
                    //pro.plprice;
                    //pro.pmprice;
                    //pro.psprice;
                    //pro.phprice;

                    #region 获取盘栋房 名称

                    //可能为楼层差估价 传入为String 非楼层差时从数据中心取名称
                    if (Convert.ToInt32(pro.projectid) > 0)
                    {
                        //楼盘名称
                        TempProject projectTem = CallDataCenter.GetProjectInfo(pro.cityid, Convert.ToInt32(pro.projectid), out jrd, out outJson);
                        if (projectTem != null)
                            pro.projectname = projectTem.projectname;
                    }

                    if (Convert.ToInt32(pro.buildingid) > 0)
                    {
                        //楼栋名称
                        TempProject buildingTem = CallDataCenter.GetBuildingInfo(pro.cityid, Convert.ToInt32(pro.buildingid), out jrd, out outJson);
                        if (buildingTem != null)
                            pro.buildingname = buildingTem.buildingname;
                    }

                    if (Convert.ToInt32(pro.houseid) > 0)
                    {
                        //房号名称
                        TempProject houseTem = CallDataCenter.GetHouseInfo(pro.cityid, Convert.ToInt32(pro.houseid), out jrd, out outJson);

                        if (houseTem != null)
                            pro.housename = houseTem.housename;
                    }
                    #endregion

                    #endregion

                    property.Add(pro);
                    main.property = property.ToArray();

                    int centerRes = SendToCenter.Send(main);

                    //LogHelper.Info(" centerRes:" + centerRes);
                    //接口返回值:
                    //必填属性为空= -2,
                    //系统错误 =-1,
                    //参数对象为空 = 0,
                    //请求成功=1
                    if (centerRes == 1)
                    {
                        //成功
                    }
                    else
                    {
                        throw new Exception(@"运营中心调用失败，返回状态：" + centerRes);
                    }
                }

                #endregion

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);

                #region 记录未发送至运营中心的数据
                if (!noLog)
                {
                    string file = AppDomain.CurrentDomain.BaseDirectory + "\\upload\\VQCenterData\\VQCenterData" + DateTime.Now.ToString("yyyy-MM-dd") + ".table";

                    System.Data.DataTable dt = new System.Data.DataTable("datatable");

                    lock (padlock)  //防止冲突
                    {
                        if (File.Exists(file))
                        {
                            dt.ReadXml(file);
                        }
                        else
                        {
                            dt.Columns.Add("sinfo");
                            dt.Columns.Add("info");
                            dt.Columns.Add("result");
                            dt.Columns.Add("state");//1已发送 0未发送
                            dt.Columns.Add("error");
                        }

                        System.Data.DataRow dr = dt.NewRow();
                        dr["sinfo"] = sinfo;
                        dr["info"] = info;
                        dr["result"] = result;
                        dr["state"] = 0;
                        dr["error"] = "/n Error Message:" + ex.Message.ToString() +
                                    "/n Stack Trace:" + ex.StackTrace.ToString() +
                                    "/n Source:" + ex.Source.ToString();

                        dt.Rows.Add(dr);

                        //一天一个文件
                        dt.WriteXml(file, XmlWriteMode.WriteSchema);
                    }
                }
                #endregion

                return false;
            }
        }

        /// <summary>
        /// 发送数据至运营中心 单个
        /// </summary>
        /// <param name="sinfo"></param>
        /// <param name="info"></param>
        /// <param name="result"></param>
        private static bool SendToVQOperater(string sinfo, string info, string result)
        {
            return SendToVQOperater(sinfo, info, result, false);
        }

        //同步锁
        private static readonly object padlock = new object();

        /// <summary>
        /// 重新发送失败的数据 至 运营中心
        /// </summary>
        /// <param name="file">文件名</param>
        /// <param name="sendCount"></param>
        /// <param name="success"></param>
        public static void NotSendDataToCenter(string file, out int sendCount, out int success)
        {
            if (!File.Exists(file))
            {
                sendCount = -1;
                success = -1;
                return;
            }

            int send = 0;//发送数
            int sendok = 0;//成功数
            System.Data.DataTable dt = new System.Data.DataTable();

            lock (padlock)  //防止冲突
            {
                if (File.Exists(file))
                {
                    dt.ReadXml(file);
                }

                foreach (DataRow dr in dt.Rows)
                {
                    if (Convert.ToInt32(dr["state"]) == 0)
                    {
                        bool res = SendToVQOperater(dr["sinfo"].ToString(), dr["info"].ToString(), dr["result"].ToString(), noLog: true);
                        dr["state"] = res ? 1 : 0;

                        send++;
                        if (res)
                            sendok++;
                    }

                    //一次最多发送1000个
                    if (sendok == 1000)
                        break;
                }

                dt.WriteXml(file, XmlWriteMode.WriteSchema);
            }
            sendCount = send;
            success = sendok;
        }
    }
}
