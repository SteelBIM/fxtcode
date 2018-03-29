using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FxtSpider.Manager.Common;
using System.Net;
using System.IO;
using System.Text;
using System.Net.Http;
using FxtSpider.FxtApi.ApiManager;
using Newtonsoft.Json.Linq;
using FxtSpider.Common;
using FxtSpider.FxtApi.Model;
using FxtSpider.Bll;


/***
 * 作者:  李晓东
 * 时间:  2013.12.4
 * 摘要:  创建 HomeController 默认页类
 * **/
namespace FxtSpider.Manager.Web.Controllers
{

    public class HomeController : BaseController
    {
        //
        // GET: /Home/
        //[FxtSpider.Manager.Common.AuthorizeAttribute(NowRequestType = FxtSpider.Manager.Common.AuthorizeAttribute.RequestType.AJAX)]
        //[AllowAnonymous]
        public ActionResult Index()
        {
            //CompanyManager.Insert("测试企sdf业dds");
            //HttpGetTest();
            //ProjectAvgPriceApi.GetCross(new int[]{45291}, 157, null,new string[]{ "2014-04"});
            //int result=ProjectAvgPriceApi.GetCrossProjectByCodeType(45772, 6, 1002001, "2012-05");
            //HttpUploadFile3();
            //HttpUploadFile();
            // GetHtml();
            //ProjectAvgPriceApi.GetCross(47838, 157, 1002001, "2014-03");
            //string str = TempSysCodeManager.test();
            return View();
        }
        public ActionResult Login()
        {
            return null;
        }
        public ActionResult test()
        {
            return View();
        }
        public ActionResult Index2()
        {
            return View();
        }

        //string GetHtml()
        //{
        //    string url = "http://localhost:6300/API/FxtMobileAPI.svc/Entrance/E/test";// "http://localhost:6300/API/FxtMobileAPI.svc/all/sdf";




        //    HttpClient client = new HttpClient();
        //    //client.Headers.Add(   添加头部
        //    var postData = new List<KeyValuePair<string, string>>();
        //    postData.Add(new KeyValuePair<string, string>("date", "abscs"));
        //    postData.Add(new KeyValuePair<string, string>("code", "abscs"));
        //    postData.Add(new KeyValuePair<string, string>("value", "{\"code\":1109000}"));
        //    //HttpResponseMessage hrm = client.PostAsync(url, new FormUrlEncodedContent(postData)).Result;
        //    HttpResponseMessage hrm = client.PostAsJsonAsync(url, new { date = WcfCheck.GetWcfCheckValidDate(), code = WcfCheck.GetWcfCheckValidCode(), parameter = "{\"code\":1109000}" }).Result;            
        //    string str = hrm.Content.ReadAsStringAsync().Result;
        //    client.Dispose();
        //    //return "";
        //    string url2 = "http://localhost:6300/API/FxtMobileAPI.svc/GetFile";
        //    string paraUrlCoded = "a=a";
        //    byte[] payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
        //    string resultHtml = "";
        //    HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(url2);

        //    request.Method = "POST";
        //    request.Timeout = 10000;
        //    request.KeepAlive = false;
        //    request.AllowAutoRedirect = true;
        //    //request.ContentType = "application/json";
        //    request.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";
        //    //request.Accept = "application/json";
        //    request.ContentLength = payload.Length;
        //    Stream writer = request.GetRequestStream();
        //    //将请求参数写入流
        //    writer.Write(payload, 0, payload.Length);
        //    // 关闭请求流
        //    writer.Close();


        //    System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
        //    System.IO.Stream stream = response.GetResponseStream();
        //    System.IO.StreamReader read = new System.IO.StreamReader(stream);
        //    resultHtml = read.ReadToEnd();
        //    response.Close();

        //    return resultHtml;
        //}
        /// <summary> 
        /// 上传图片文件 
        /// </summary> 
        /// <param name="url">提交的地址</param> 
        /// <param name="poststr">发送的文本串   比如：user=eking&pass=123456  </param> 
        /// <param name="fileformname">文本域的名称  比如：name="file"，那么fileformname=file  </param> 
        /// <param name="filepath">上传的文件路径  比如： c:\12.jpg </param> 
        /// <param name="refre">头部的跳转地址</param> 
        /// <returns></returns> 
        public string HttpUploadFile()
        {
            string url, filepath;
            url = "http://localhost:50887/API/FxtMobileAPI.svc/UploadFile?aa=f%sfsf";
            filepath = "E:\\案例.rar";// "E:\\1_ycxjex2006.jpg";//1_ycxjex2006.jpg 案例上传格式-说明 - 副本.xls
            // 这个可以是改变的，也可以是下面这个固定的字符串 
            string boundary = "—————————7d930d1a850658";

            // 创建request对象 
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
            webrequest.ContentType = "application/octet-stream";//application/octet-stream
            webrequest.Method = "POST";
            //构造尾部数据 
            byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n–" + boundary + "–\r\n");
            FileStream fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            Stream requestStream = webrequest.GetRequestStream();
            // 输入文件流数据 
            byte[] buffer = new Byte[checked((uint)Math.Min(4096, (int)fileStream.Length))];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                requestStream.Write(buffer, 0, bytesRead);
            }
            // 输入尾部数据 
            requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
            WebResponse responce = webrequest.GetResponse();
            Stream s = responce.GetResponseStream();
            StreamReader sr = new StreamReader(s);

            // 返回数据流(源码) 
            string str= sr.ReadToEnd();
            return "";
        }
        public void HttpUploadFile2()
        {
            string fileName = "E:\\1_ycxjex2006.jpg";//E:\\案例.rar
            string url = "http://localhost:50887/API/FxtMobileAPI.svc/UpLoadFileSeries";
            long cruuent = HttpGetFile(fileName);//获取服务器已经上传的大小
            FileStream fStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);//将文件加载到文件流            
            BinaryReader bReader = new BinaryReader(fStream);//将文件了加载成二进制数据
            long length = fStream.Length;//当前文件的总大小
            fileName = fileName.Substring(fileName.LastIndexOf('\\') + 1);//获取文件名称
            //如果服务器已经有过此文件
            if (cruuent > 0)
            {
                //将文件流的读取位置移到服务器已上传的大小的位置
                fStream.Seek(cruuent, SeekOrigin.Current);
            }
            //创建一个用于存储要上传文件内容的字节对象
            byte[] data = new byte[length - cruuent];
            //将流中读取指定字节数加载到到字节对象data
            bReader.Read(data, 0, Convert.ToInt32(length - cruuent));
            url = url + "?filename=" + fileName + "&npos=" + cruuent;
            WebClient webClientObj = new System.Net.WebClient();
            webClientObj.Headers.Add("Content-Type", "application/octet-stream");
            byte[] result= webClientObj.UploadData(url, "POST", data);
            string ll = Encoding.Default.GetString(result);
            //"E:\\案例.rar"
        }
        public void HttpUploadFile3()
        {
            string fileName = "E:\\test2.xls";
            string url = "http://192.168.0.7:6400/API/FxtMobileAPI.svc/UpLoadFileSeries";
            long cruuent = HttpGetFile(fileName);//获取服务器已经上传的大小
            FileStream fStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);//将文件加载到文件流            
            BinaryReader bReader = new BinaryReader(fStream);//将文件了加载成二进制数据
            long length = fStream.Length;//当前文件的总大小
            fileName = fileName.Substring(fileName.LastIndexOf('\\') + 1);//获取文件名称
            //如果服务器已经有过此文件
            if (cruuent > 0)
            {
                //将文件流的读取位置移到服务器已上传的大小的位置
                fStream.Seek(cruuent, SeekOrigin.Current);
            }
            int upCount = 100;//每次上传多少
            byte[] data;
            for (; cruuent <=length; cruuent = cruuent + upCount)
            {
                if (cruuent + upCount > length)
                {
                    //创建一个用于存储要上传文件内容的字节对象
                    data = new byte[length - cruuent];
                    //将流中读取指定字节数加载到到字节对象data
                    bReader.Read(data, 0, Convert.ToInt32(length - cruuent));
                    
                }
                else
                {
                    data = new byte[upCount];
                    bReader.Read(data, 0, upCount);
                }
                uploadbytes(data, fileName, cruuent);
            }

        }
        public long HttpGetFile(string filename)
        {
            filename = filename.Substring(filename.LastIndexOf('\\') + 1);
            string url = "http://192.168.0.7:6400/API/FxtMobileAPI.svc/GetFileSeries";
            HttpClient client = new HttpClient();
            HttpResponseMessage hrm = client.PostAsJsonAsync(url, new { filename = filename }).Result;
            string str = hrm.Content.ReadAsStringAsync().Result;
            JObject jobj = JObject.Parse(str);
            long cruuent = Convert.ToInt64(jobj.Value<string>("data"));
            return cruuent;
        }
        public void HttpGetTest()
        {
            //return;
            List<FxtApi_DATProjectAvgPrice> listtest = ProjectAvgPriceApi.GetCrossByCompanyId(45606, 6, 1002001, "2014-4",25);
            //ProjectApi.testcross();
            return;
            //FxtApi_SYSCode aa = new FxtApi_SYSCode { Code = 1, CodeName = "aaa", CodeType = "sdf", ID = 22, Remark = "fdf", SubCode = 1 };
            //FxtApi_SYSCode bb =new FxtApi_SYSCode ();
            //string bbb = aa.Remark; ;
           // bbb = "哈哈";
            //bb = aa;
            //bb.Remark = "哈哈";
            //string aaa = aa.Remark;
            

            #region 示例
            var obj = new
            {
                x = "(decimal?,null,物业精度)",
                y = "(decimal?,null,物业纬度)",
                allotflowx = "(decimal?,not null,查勘员现场经度(插入表Dat_AllotFlow))",
                allotflowy = "(decimal?,not null,查勘员现场纬度(插入表Dat_AllotFlow))",
                projectname = "(string,not null,楼盘名称)",
                cityid = "(int,not null,城市ID(选择))",
                areaid = "(int,not null,行政区ID(选择))",
                address = "(string,not null,物业地址)",
                enddate = "(DateTime?,not null,竣工时间)",
                east = "(string,null,四至朝向-东)",
                west = "(string,null,四至朝向-西)",
                south = "(string,null,四至朝向-南)",
                north = "(string,null,四至朝向-北)",
                buildingarea = "(decimal?,not null,建筑面积)",
                landarea = "(decimal?,not null,占地面积)",
                cubagerate = "(decimal?,not null,容积率)",
                greenrate = "(decimal?,not null,绿化率)",
                manager_company = "(string,null,物业管理公司(插入表LNK_P_Company)",
                managerprice = "(nvarchar,not null,物业管理费)",
                developers = "(string,null,开发商(插入表LNK_P_Company)",
                parkingnumber = "(int,not null,车位数)",
                totalnum = "(int,not null,总户数or总套数)",
                saledate = "(DateTime?,not null,开盘时间)",
                buildingdate = "(DateTime?,not null,开工时间)",
                statedate = "(DateTime,not null,采集时间)",
                buildingnum = "(int,not null,总栋数)",
                detail = "(string,null,楼盘备注)",
                allotflowremark = "(string,null,任务备注(插入表Dat_AllotFlow))",
                fxtprojectid = "(int,null,正式库的楼盘ID)",
                photocount = "(int,not null,照片个数)",
                appendage = new[]{  new
                 {
                     appendagecode = "(int,not null,配套类型(学校;医院..)(选择)",
                     p_aname = "(string,not null,配套名字)",
                     classcode = "(int,not null,配套等级)(选择)"
                 }},
                buildingList = new[]{ new { 
                                                        buildingid=0,
                                                        buildingname = "(string,not null,楼栋名称)", 
                                                        doorplate = "(string,null,门牌号)", 
                                                        othername = "(string,null,楼栋别称)", 
                                                        structurecode = "(int?,not null,建筑结构(选择))", 
                                                        locationcode = "(int?,not null,位置(选择))", 
                                                        averageprice = "(decimal?,not null,楼栋均价)", 
                                                        builddate = "(DateTime?,not null,楼栋竣工时间(建筑时间))", 
                                                        iselevator = "(int,not null,是否带电梯)", 
                                                        elevatorrate = "(string,null,梯户数(梯户比))", 
                                                        pricedetail = "(string,null,价格说明)", 
                                                        remark = "(string,null,备注)", 
                                                        fxtbuildingid="(int,null,正式库的楼栋ID)",
                                                        sightcode="(int,null,景观(选择))",
                                                        totalfloor="(int,not null,总层数)",
                                                        houseList =new[]{ new { 
                                                                          houseid="(int,null,ID)", 
                                                                          unitno = "(string,null,单元名称)", 
                                                                          floorno="(int,not null,所在层)",
                                                                          housename = "(string,not null,房号)", 
                                                                          frontcode = "(string,not null,朝向(选择))", 
                                                                          buildarea = "(decimal?,not null,面积)", 
                                                                          housetypecode = "(int?,not null,户型(选择))", 
                                                                          remark = "(string,null,备注)" ,
                                                                          fxthouseid="(int,null,正式库的房号ID)",
                                                                          sightcode="(int,null,景观)",
                                                                         } }
                                                        }
                                   }
            };
            #endregion

            var obj2 = new
            {
                x = 212.1212,
                y = 212.121,
                allotflowx = "212.1212",
                allotflowy = "212.121",
                projectname = "测试提交楼盘",
                cityid = 1,
                areaid =1,
                address = "测试提交楼盘物业地址)",
                enddate = "2004-1-1",
                east = "山",
                west = "河",
                south = "湖",
                north = "海",
                buildingarea = 10000,
                landarea = 20000,
                cubagerate = 0.2,
                greenrate = 0.5,
                manager_company = "测试提交楼盘物业管理公司",
                managerprice = "122",
                developers = "测试提交楼盘",
                parkingnumber = "1",
                totalnum =10,
                saledate = "2012-3-3",
                buildingdate = "2000-1-1",
                buildingnum="2",
                statedate = DateTime.Now.ToString(),
                detail = "楼盘备注",
                allotflowremark = "(string,null,任务备注(插入表Dat_AllotFlow))",
                photocount= "(int,not null,照片个数)",
                fxtprojectid = "",
                appendage = new List<object>{  new
                 {
                     appendagecode =2008007,
                     p_aname = "东城区人民医院修改",
                     classcode = 1012003
                 }, new
                 {
                     appendagecode =2008005,
                     p_aname = "修改广东发展银行、中国建设银行、东莞银行、东莞市农村商业银行",
                     classcode = 1012003
                 }, new
                 {
                     appendagecode =2008006,
                     p_aname = "修改东莞市岭南学校、莞城英文实验学校",
                     classcode = 1012003
                 }, new
                 {
                     appendagecode =2008002,
                     p_aname = "新增超市",
                     classcode = 1012003
                 }},
                buildinglist =new List<object>{ new { 
                                                        buildingid=0,
                                                        buildingname = "1-楼栋", 
                                                        doorplate = "(string,null,门牌号)", 
                                                        othername = "(string,null,楼栋别称)", 
                                                        structurecode = 2010003, 
                                                        locationcode = 2011001, //(int?,not null,位置(选择))
                                                        averageprice = 12121, 
                                                        builddate = "2012-1-1", 
                                                        iselevator = 1, 
                                                        elevatorrate = "(string,null,梯户数(梯户比))", 
                                                        pricedetail = "(string,null,价格说明)", 
                                                        remark = "(string,null,备注)", 
                                                        fxtbuildingid="",
                                                        sightcode="2006007",
                                                        totalfloor=22,
                                                        houselist =new List<object>{ new { 
                                                                          houseid=0,
                                                                          unitno = "单元名称1",
                                                                          floorno=1,
                                                                          housename = "测试房号1", 
                                                                          frontcode = 2004004, 
                                                                          buildarea = "145.2", 
                                                                          housetypecode =4001007, 
                                                                          remark = "(string,null,备注)" ,
                                                                          fxthouseid="",
                                                                          sightcode=2006002
                                                                         },new { 
                                                                          houseid=0,
                                                                          unitno = "单元名称2", 
                                                                          floorno=1,
                                                                          housename = "测试房号2", 
                                                                          frontcode = 2004004, 
                                                                          buildarea = "145.2", 
                                                                          housetypecode =4001007, 
                                                                          remark = "(string,null,备注)" ,
                                                                          fxthouseid="",
                                                                          sightcode=2006002
                                                                         } }
                                                        },
                                                        new { 
                                                        buildingid=5,
                                                        buildingname = "1-楼栋", 
                                                        doorplate = "(string,null,门牌号)", 
                                                        othername = "(string,null,楼栋别称)", 
                                                        structurecode = 2010003, 
                                                        locationcode = 2011001, //(int?,not null,位置(选择))
                                                        averageprice = 12121, 
                                                        builddate = "2012-1-1", 
                                                        iselevator = 1, 
                                                        elevatorrate = "(string,null,梯户数(梯户比))", 
                                                        pricedetail = "(string,null,价格说明)", 
                                                        remark = "(string,null,备注)", 
                                                        fxtbuildingid="",
                                                        sightcode="2006007",
                                                        totalfloor=22,
                                                        houselist =new List<object>{ new { 
                                                                          houseid=0,
                                                                          unitno = "单元名称3", 
                                                                          floorno=1,
                                                                          housename = "测试房号3", 
                                                                          frontcode = 2004004, 
                                                                          buildarea = "145.2", 
                                                                          housetypecode =4001007, 
                                                                          remark = "(string,null,备注)" ,
                                                                          fxthouseid="",
                                                                          sightcode=2006002
                                                                         },new { 
                                                                          houseid=58,
                                                                          unitno = "单元名称4", 
                                                                          floorno=1,
                                                                          housename = "测试房号4", 
                                                                          frontcode = 2004004, 
                                                                          buildarea = "145.2", 
                                                                          housetypecode =4001007, 
                                                                          remark = "(string,null,备注)" ,
                                                                          fxthouseid="",
                                                                          sightcode=2006002
                                                                         },new { 
                                                                          houseid=59,
                                                                          unitno = "单元名称5", 
                                                                          floorno=1,
                                                                          housename = "1-1201", 
                                                                          frontcode = 2004004, 
                                                                          buildarea = "145.2", 
                                                                          housetypecode =4001007, 
                                                                          remark = "(string,null,备注)" ,
                                                                          fxthouseid="",
                                                                          sightcode=2006002
                                                                         }  }
                                                        }
                                   }
            };
            //http://localhost:50887/mobileapi/runflats
            string url = "http://192.168.0.128:6400/mobileapi/runflats";
            HttpClient client = new HttpClient();
            //client.Headers.Add(   添加头部
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            var para = new
            {
                sinfo = JsonHelp.ToJSONjss(new { functionname = "getallotsurveyingproject", appid = "1003106", apppwd = "1300558927", signname = "4106DEF5-A760-4CD7-A6B2-8250420FCB18", time = date, code = "aabbccddeeffgg" }),
                info = JsonHelp.ToJSONjss(new
                {
                    uinfo = new { username = "admin@fxt", token = "" },
                    appinfo = new
                    {
                        splatype = "android",
                        stype = "yck",
                        version = "4.26",
                        vcode = "1",
                        systypecode = "1003034",
                        channel = "360"
                    },
                    funinfo = new { username = "admin@fxt", cityid = 1 }// new { userid = "3", cityid = 1, allotid = 1, data =obj2.ToJSONjss()}
                })
            };
   
            HttpResponseMessage hrm = client.PostAsJsonAsync(url, para).Result;
            string str = hrm.Content.ReadAsStringAsync().Result;

        }
        void uploadbytes(byte[] data, string fileName, long cruuent)
        {

            string url = "http://192.168.0.7:6400/API/FxtMobileAPI.svc/UpLoadFileSeries";
            url = url + "?filename=" + fileName + "&npos=" + cruuent;
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
            webrequest.ContentType = "application/octet-stream";
            webrequest.Method = "POST";
            Stream requestStream = webrequest.GetRequestStream();
            byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n–—————————7d930d1a850658–\r\n");
            requestStream.Write(data, 0, data.Length);
            // 输入尾部数据 
            requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
            WebResponse responce = webrequest.GetResponse();
            Stream s = responce.GetResponseStream();
            StreamReader sr = new StreamReader(s);

            // 返回数据流(源码) 
            string str = sr.ReadToEnd();
        }


    }
}
