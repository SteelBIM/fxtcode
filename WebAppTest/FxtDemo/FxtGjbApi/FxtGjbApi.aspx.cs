using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Net;
using System.IO;
using System.Text;
using CAS.Common;
using Newtonsoft.Json;

namespace FxtDemo.FxtGjbApi
{
    public partial class FxtGjbApi : System.Web.UI.Page
    {
        public string jsArgs;
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 注释
            //try
            //{
            //    string txt = "{\"id\":\"3\",\"password\":\"ilsdf\"}";
            //    string config = ConfigurationManager.AppSettings["FxtGjbApi"];



            //    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(config);
            //    request.ContentType = "application/json";
            //    request.Method = "POST";

            //    MemoryStream memory = new MemoryStream();


            //    //ASCIIEncoding encoding = new ASCIIEncoding();
            //    byte[] postdata = Encoding.GetEncoding("UTF-8").GetBytes(txt);
            //    request.ContentLength = postdata.Length;
            //    Stream newStream = request.GetRequestStream();
            //    newStream.Write(postdata, 0, postdata.Length);
            //    newStream.Close();

            //    HttpWebResponse myResponse = (HttpWebResponse)request.GetResponse();
            //    StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            //    string content = reader.ReadToEnd();//得到结果
            //    jsArgs = content;
            //}
            //catch (Exception ex)
            //{
            //    acceptTb.Text = ex.Message;
            //}
            #endregion

        }


        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                #region 本地
                //string appid = "1003100";//要调用的接口序列号
                //string apppwd = "737345965";//接口密码
                //string appkey = "1986222340";//加密接口安全属性的key
                //string functionname = "download";//"splist";//"pev";//方法名
                //string time = DateTime.Now.ToString("yyyyMMddHHmmss");//时间
                //string signname = "60765FEC-9156-409D-923E-A12EB53A1D1F";//商户标示号
                //string config = "http://192.168.0.103:8912/gjb/active/download";
                #endregion
                #region 09
                string appid = "1003100";//要调用的接口序列号
                string apppwd = "737345965";//接口密码
                string appkey = "1986222340";//加密接口安全属性的key
                string functionname = "download";//方法名
                string time = DateTime.Now.ToString("yyyyMMddHHmmss");//时间
                string signname = "AAE0E2C7-A416-49EB-8C8A-E504E520D9C6";//商户标示号
                string config = "http://wcfgjbapi.fxtchina.com/gjb/active/download";

                #endregion

                string[] pwdArray = { appid, apppwd, signname, time, functionname };
                string code = EncryptHelper.GetMd5(pwdArray, appkey);

                var par = new
                {
                    sinfo = new { appid = appid, apppwd = apppwd, signname = signname, time = time, code = code, functionname = functionname }.ToJson(),
                    info = new
                    {
                        appinfo = new { splatype = "win", stype = "gjb", version = "1.0", vcode = "1", systypecode = "1003301", channel = "360" },
                        uinfo = new { username = "", token = "" },
                        funinfo = new
                        {
                            entrustid = 20140411015,
                            guid = "E7CD72F6-84DC-40F0-872B-5ECFF1D692A9"
                        }
                    }.ToJson()
                };


                string txt = par.ToJson();



                //FileStream fs = new FileStream("C:\\Users\\tanl\\Desktop\\Test.doc", FileMode.Create);
                if (ddlRoute.SelectedItem.Text == "用户中心start")
                {
                    txt = ReplaceStr(txt);
                }
                txt = txt.Replace("\r\n", "");

                //byte[] buffer = APIPostBack(config, txt, "application/json");
                //fs.Write(buffer, 0, buffer.Length);
                //fs.Flush();
                //fs.Close();


                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(config);
                request.ContentType = "application/json";
                request.Method = "POST";
                byte[] postdata = Encoding.GetEncoding("UTF-8").GetBytes(txt);
                request.ContentLength = postdata.Length;
                Stream newStream = request.GetRequestStream();
                newStream.Write(postdata, 0, postdata.Length);
                newStream.Close();
                HttpWebResponse myResponse = (HttpWebResponse)request.GetResponse();
                Stream stream = myResponse.GetResponseStream();
                string str = HttpUtility.UrlDecode(myResponse.Headers["Content-Disposition"]);

                Response.BufferOutput = false;   // to prevent buffering 
                byte[] buffer = new byte[myResponse.ContentLength];
                int bytesRead = 0;

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=3bd1b5e91cee474abe36.doc");

                bytesRead = stream.Read(buffer, 0, buffer.Length);

                while (bytesRead > 0)
                {
                    // Verify that the client is connected.
                    if (Response.IsClientConnected)
                    {

                        Response.OutputStream.Write(buffer, 0, bytesRead);
                        // Flush the data to the HTML output.
                        Response.Flush();

                        buffer = new byte[myResponse.ContentLength];
                        bytesRead = stream.Read(buffer, 0, buffer.Length);
                    }
                    else
                    {
                        bytesRead = -1;
                    }
                }
                acceptTb.Text = str;
            }
            catch (Exception ex)
            {
                acceptTb.Text = ex.Message;
            }

        }

        protected void btnFile_Click(object sender, EventArgs e)
        {
            try
            {
                string url, filepath;


                #region 本地
                string appid = "1003100";//要调用的接口序列号
                string apppwd = "737345965";//接口密码
                string appkey = "1986222340";//加密接口安全属性的key
                string functionname = "upload";//"splist";//"pev";//方法名
                string time = DateTime.Now.ToString("yyyyMMddHHmmss");//时间
                string signname = "60765FEC-9156-409D-923E-A12EB53A1D1F";//商户标示号
                #endregion
                #region 09
                //string appid = "1003100";//要调用的接口序列号
                //string apppwd = "737345965";//接口密码
                //string appkey = "1986222340";//加密接口安全属性的key
                //string functionname = "upload";//方法名
                //string time = DateTime.Now.ToString("yyyyMMddHHmmss");//时间
                //string signname = "AAE0E2C7-A416-49EB-8C8A-E504E520D9C6";//商户标示号
                #endregion

                string[] pwdArray = { appid, apppwd, signname, time, functionname };
                string code = EncryptHelper.GetMd5(pwdArray, appkey);
                filepath = "C:\\Users\\tanl\\Desktop\\汇丰第三方业务接入估价宝OA解决方案.doc";//1_ycxjex2006.jpg 案例上传格式-说明 - 副本.xls
                //string sinfo = "1", info = "1";
                string sinfo = new { appid = appid, apppwd = apppwd, signname = signname, time = time, code = code, functionname = functionname }.ToJson(),
                       info = new
                       {
                           appinfo = new { splatype = "win", stype = "gjbapi", version = "1.0", vcode = "1", systypecode = "1003301", channel = "360" },
                           uinfo = new { username = "", token = "" },
                           funinfo = new
                           {
                               format = "doc",
                               name = "汇丰第三方业务接入估价宝OA解决方案.doc"
                           }
                       }.ToJson();


                string config = "http://192.168.0.103:8912/gjb/active/upload?sinfo={0}&info={1}";//本地
                //string config = "http://wcfgjbapi.fxtchina.com/gjb/active/upload?sinfo={0}&info={1}";//09

                //active/upload/{sinfo}/{info}
                url = string.Format(config, sinfo, info);//config; //

                // 这个可以是改变的，也可以是下面这个固定的字符串 
                string boundary = "—————————7d930d1a850658";

                // 创建request对象 
                HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
                //webrequest.ContentType = "multipart/form-data;"; 
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
                    requestStream.Write(buffer, 0, bytesRead);

                // 输入尾部数据 
                requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                WebResponse responce = webrequest.GetResponse();
                Stream s = responce.GetResponseStream();
                StreamReader sr = new StreamReader(s);
                // 返回数据流(源码) 
                string str = sr.ReadToEnd();
                acceptTb.Text = str;
            }
            catch (Exception ex)
            {
                acceptTb.Text = ex.Message;
            }

        }

        protected void btn09_Click(object sender, EventArgs e)
        {
            try
            {
                #region 本地
                string appid = "1003100";//要调用的接口序列号
                string apppwd = "737345965";//接口密码
                string appkey = "1986222340";//加密接口安全属性的key
                string functionname = "newentrust";//"provincelist";//方法名
                string time = DateTime.Now.ToString("yyyyMMddHHmmss");//时间
                string signname = "60765FEC-9156-409D-923E-A12EB53A1D1F";//商户标示号
                string config = "http://192.168.0.103:8912/gjb/active";
                #endregion
                #region 09
                //string appid = "1003100";//要调用的接口序列号
                //string apppwd = "737345965";//接口密码
                //string appkey = "1986222340";//加密接口安全属性的key
                //string functionname = "externalentruststate";//"newentrust";//方法名
                //string time = DateTime.Now.ToString("yyyyMMddHHmmss");//时间
                //string signname = "AAE0E2C7-A416-49EB-8C8A-E504E520D9C6";//商户标示号
                //string config = "http://wcfgjbapi.fxtchina.com/gjb/active";

                #endregion


                string[] pwdArray = { appid, apppwd, signname, time, functionname };
                string code = EncryptHelper.GetMd5(pwdArray, appkey);
                List<FileAccessoryEntity> file = new List<FileAccessoryEntity>();
                file.Add(new FileAccessoryEntity() { name = "汇丰第三方业务接入估价宝OA解决方案.doc", path = "upload/365/OA/2014/04/09/3bd1b5e91cee474abe36.doc", filesubtypecode = 5018001, flietypecode = 5019001 });
                file.Add(new FileAccessoryEntity() { name = "汇丰第三方业务接入估价宝OA解决方案.doc", path = "upload/365/OA/2014/04/09/3bd1b5e91cee474abe36.doc", filesubtypecode = 5018001, flietypecode = 5019001 });
                file.Add(new FileAccessoryEntity() { name = "汇丰第三方业务接入估价宝OA解决方案.doc", path = "upload/365/OA/2014/04/09/3bd1b5e91cee474abe36.doc", filesubtypecode = 5018001, flietypecode = 5019001 });
                file.Add(new FileAccessoryEntity() { name = "汇丰第三方业务接入估价宝OA解决方案.doc", path = "upload/365/OA/2014/04/09/3bd1b5e91cee474abe36.doc", filesubtypecode = 5018001, flietypecode = 5019001 });
                file.Add(new FileAccessoryEntity() { name = "汇丰第三方业务接入估价宝OA解决方案.doc", path = "upload/365/OA/2014/04/09/3bd1b5e91cee474abe36.doc", filesubtypecode = 5018001, flietypecode = 5019001 });
                file.Add(new FileAccessoryEntity() { name = "汇丰第三方业务接入估价宝OA解决方案.doc", path = "upload/365/OA/2014/04/09/3bd1b5e91cee474abe36.doc", filesubtypecode = 5018001, flietypecode = 5019001 });

                var par = new
                {
                    sinfo = new { appid = appid, apppwd = apppwd, signname = signname, time = time, code = code, functionname = functionname }.ToJson(),
                    info = new
                    {
                        appinfo = new { splatype = "win", stype = "gjb", version = "1.0", vcode = "1", systypecode = "1003301", channel = "360" },
                        uinfo = new { username = "", token = "" },
                        funinfo = new
                        {
                            #region newentrust
                            bankusername = "谭",
                            bankuserphone = "13341339638",
                            remarks = "易房保业务委托测试",
                            biztype = "20010402",
                            source = "易房保",
                            clientname = "个人",
                            clientcontact = "瞿",
                            clientphone = "13341339638",
                            name = "合作平台",
                            file = file.ToJson()
                            #endregion
                            #region externalentruststate
                            //entrustid = 20140411015 //
                            #endregion
                            #region querynoinside
                            //cityid = 6,
                            //projectname = "金海湾花园",
                            //buildingname = "1-3栋1栋",
                            //floornumber = "2",
                            //housename = "2A",
                            //querytype = "住宅",
                            //address = "福田沙嘴路和福荣路交汇处",
                            //totalfloor = "30",
                            //buildingarea = "87.13",
                            //housecount = "3",
                            //hallcount = "2",
                            //bathroomcount = "1",
                            //front = "南",
                            //sight = "公园",
                            //owner = "瞿",
                            //source = "易房保",
                            //registrationprice = "21000",
                            //ownertypecode = "20010502",
                            //clientcontactusername = "谭",
                            //clientphone = "13800138000",
                            //remark = "重庆汇丰" + txtArgs.Text,
                            //file = file.ToJson()
                            #endregion

                        }
                    }.ToJson()
                };


                string txt = par.ToJson();


                //"http://wcfgjbapi.fxtchina.com/gjb/active";// "http://localhost:37335/gjb/active";//

                if (ddlRoute.SelectedItem.Text == "用户中心start")
                {
                    txt = ReplaceStr(txt);
                }
                txt = txt.Replace("\r\n", "");


                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(config);
                request.ContentType = "application/json";
                request.Method = "POST";
                MemoryStream memory = new MemoryStream();
                byte[] postdata = Encoding.GetEncoding("UTF-8").GetBytes(txt);
                request.ContentLength = postdata.Length;
                Stream newStream = request.GetRequestStream();
                newStream.Write(postdata, 0, postdata.Length);
                newStream.Close();

                HttpWebResponse myResponse = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                string content = reader.ReadToEnd();//得到结果
                acceptTb.Text = content;


            }
            catch (Exception ex)
            {
                acceptTb.Text = ex.Message;
            }
        }

        protected void btnData_Click(object sender, EventArgs e)
        {

            try
            {
                string functionname = "projectdropdownlist";//"splist";//"pev";//方法名
                string appid = "1003104";//"1";//要调用的接口序列号
                string apppwd = "487522690";//接口密码   本地
                string appkey = "1480880117";//"gjbcqhf$%2014";//加密接口安全属性的key   本地
                string signname = "4106DEF5-A760-4CD7-A6B2-8250420FCB18";//"fxtcqhf";//商户标示号   本地
                string time = DateTime.Now.ToString("yyyyMMddHHmmss");//时间     "20160223125458";// 
                string[] pwdArray = { appid, apppwd, signname, time, functionname };
                string code = EncryptHelper.GetMd5(pwdArray, appkey);

                var par = new
                {
                    sinfo = new { appid = appid, apppwd = apppwd, signname = signname, time = time, code = code, functionname = functionname }.ToJson(),
                    info = new
                    {
                        appinfo = new { splatype = "win", stype = "gjb", version = "1.0", vcode = "1", systypecode = "1003036", channel = "360" },//本地
                        uinfo = new { username = "admin@fxt", token = "" },//本地
                        funinfo = new
                        {
                            cityid = 6,
                            key = "浪琴屿",
                        }
                    }.ToJson()
                };
                string txt = par.ToJson();//   txtArgs.Text;// 

                string config ="http://192.168.2.30:9997/dc/active";// 07本地环境

                if (ddlRoute.SelectedItem.Text == "用户中心start")
                {
                    txt = ReplaceStr(txt);
                }
                txt = txt.Replace("\r\n", "");

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(config);
                request.ContentType = "application/json";
                request.Method = "POST";
                MemoryStream memory = new MemoryStream();
                byte[] postdata = Encoding.GetEncoding("UTF-8").GetBytes(txt);
                request.ContentLength = postdata.Length;
                Stream newStream = request.GetRequestStream();
                newStream.Write(postdata, 0, postdata.Length);
                newStream.Close();

                HttpWebResponse myResponse = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                string content = reader.ReadToEnd();//得到结果
                acceptTb.Text = content;
            }
            catch (Exception ex)
            {
                acceptTb.Text = ex.Message;
            }
        }

        protected void ButtonLogin_Click(object sender, EventArgs e)
        {

            string time = DateTime.Now.ToString("yyyyMMddHHmmss");//时间
            string signname = "60765FEC-9156-409D-923E-A12EB53A1D1F";//"fxtcqhf";//商户标示号
            string[] pwdArray = { time, };
            string code = EncryptHelper.GetMd5(pwdArray, ConstCommon.WcfLoginMd5Key);

            var par = new
            {
                sinfo = new { time = time, code = code }.ToJson(),
                info = new
                {
                    appinfo = new { splatype = "ie", platVer = "7.01", stype = "cmb", version = "1.6", vcode = "16", systypecode = "1003021", channel = "360", sign = "" },
                    uinfo = new { username = "lzadmin@gxkz", password = "3ffe8cf42ed82836815ca5a26efc0ca0" },
                    funinfo = new
                    {

                    }
                }.ToJson()
            };
            string txt = par.ToJson();


            string config = "http://192.168.0.103:8726/uc/start";//ddlRoute.SelectedValue;//"http://192.168.0.103:8912/gjb/active";

            if (ddlRoute.SelectedItem.Text == "用户中心start")
            {
                txt = ReplaceStr(txt);
            }
            txt = txt.Replace("\r\n", "");

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(config);
            request.ContentType = "application/json";
            request.Method = "POST";
            MemoryStream memory = new MemoryStream();
            byte[] postdata = Encoding.GetEncoding("UTF-8").GetBytes(txt);
            request.ContentLength = postdata.Length;
            Stream newStream = request.GetRequestStream();
            newStream.Write(postdata, 0, postdata.Length);
            newStream.Close();

            HttpWebResponse myResponse = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            string content = reader.ReadToEnd();//得到结果
            acceptTb.Text = content;
        }

        protected void btnuc_Click(object sender, EventArgs e)
        {
            string appid = "1003105";//要调用的接口序列号
            #region 本地
            //string apppwd = "1003818754";//接口密码
            //string appkey = "1442318247";//加密接口安全属性的key
            //string functionname = "cpone";//"provincelist";//方法名
            //string time = DateTime.Now.ToString("yyyyMMddHHmmss");//时间
            //string signname = "60765FEC-9156-409D-923E-A12EB53A1D1F";//商户标示号
            //string config = "http://localhost:18726/uc/start"; //正式环境
            ////string config = "http://192.168.0.103:8726/uc/update"; //uc/active";            
            #endregion
            #region 正式
            //string apppwd = "56713531";//接口密码
            //string appkey = "1862108626";//加密接口安全属性的key
            //string functionname = "cptcityids";//"provincelist";//方法名
            ////string time = DateTime.Now.ToString("yyyyMMddHHmmss");//时间
            //string time = "20151204125019";//时间
            //string signname = "FA75CA5D-68C1-45C4-B0F4-85CCB7F687F7";//商户标示号
            ////string config = "https://api.fxtcn.com/wuc/uc/active"; //正式环境
            #endregion
            #region 09
            string apppwd = "2104101158";//接口密码
            string appkey = "219412082";//加密接口安全属性的key
            string functionname = "cptcityids";//"newentrust";//方法名
            string time = DateTime.Now.ToString("yyyyMMddHHmmss");//时间
            string signname = "AAE0E2C7-A416-49EB-8C8A-E504E520D9C6";//商户标示号
            string config = "http://api.fxtchina.com/usercenter/uc/active";

            #endregion
            string[] pwdArray = { appid, apppwd, signname, time, functionname };
            string code = EncryptHelper.GetMd5(pwdArray, appkey);

            var par = new
            {
                sinfo = new { appid = appid, apppwd = apppwd, signname = signname, time = time, code = code, functionname = functionname }.ToJson(),
                info = new
                {
                    appinfo = new { splatype = "android", platVer = "7", stype = "gjb", version = "1.0", vcode = "1", systypecode = "1003034", channel = "", sign = "" },
                    uinfo = new { username = "admin@gjb", token = "" },
                    funinfo = new
                    {
                        productcode = "1003034",
                        signname = "AAE0E2C7-A416-49EB-8C8A-E504E520D9C6",
                    }
                }.ToJson()
            };
            string txt = txtArgs.Text;//   par.ToJson(); // 
            if (ddlRoute.SelectedItem.Text == "用户中心start")
            {
                txt = ReplaceStr(txt);
            }
            txt = txt.Replace("\r\n", "");

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(config);
            request.ContentType = "application/json";
            request.Method = "POST";
            MemoryStream memory = new MemoryStream();
            byte[] postdata = Encoding.GetEncoding("UTF-8").GetBytes(txt);
            request.ContentLength = postdata.Length;
            Stream newStream = request.GetRequestStream();
            newStream.Write(postdata, 0, postdata.Length);
            newStream.Close();

            HttpWebResponse myResponse = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            string content = reader.ReadToEnd();//得到结果
            acceptTb.Text = content;
        }

        private string ReplaceStr(string txt)
        {
            string strdate = DateTime.Now.ToString("yyyyMMddHHmmss");
            string strcode = EncryptHelper.GetMd5(strdate, ConstCommon.WcfLoginMd5Key);
            txt = txt.Replace("@strtime", strdate);
            txt = txt.Replace("@strcode", strcode);

            return txt;
        }


        //private static string object.ToJson(this object obj)
        //{
        //    return  JsonConvert.SerializeObject(obj);
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
        public string HttpUploadFile(string url, string filepath)
        {

            // 这个可以是改变的，也可以是下面这个固定的字符串 
            string boundary = "—————————7d930d1a850658";

            // 创建request对象 
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
            //webrequest.ContentType = "multipart/form-data;"; 
            webrequest.ContentType = "image/jpeg";
            webrequest.Method = "POST";



            //构造尾部数据 
            byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n–" + boundary + "–\r\n");

            FileStream fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read);

            Stream requestStream = webrequest.GetRequestStream();


            // 输入文件流数据 
            byte[] buffer = new Byte[checked((uint)Math.Min(4096, (int)fileStream.Length))];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                requestStream.Write(buffer, 0, bytesRead);

            // 输入尾部数据 
            requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
            WebResponse responce = webrequest.GetResponse();
            Stream s = responce.GetResponseStream();
            StreamReader sr = new StreamReader(s);

            // 返回数据流(源码) 
            return sr.ReadToEnd();
        }

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
            //active/upload/{sinfo}/{info}
            string sinfo = "";
            string info = "";
            url = string.Format("http://192.168.0.103:8912/active/upload/", sinfo, info);
            filepath = "E:\\1_ycxjex2006.jpg";//1_ycxjex2006.jpg 案例上传格式-说明 - 副本.xls
            // 这个可以是改变的，也可以是下面这个固定的字符串 
            string boundary = "—————————7d930d1a850658";

            // 创建request对象 
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
            //webrequest.ContentType = "multipart/form-data;"; 
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
                requestStream.Write(buffer, 0, bytesRead);

            // 输入尾部数据 
            requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
            WebResponse responce = webrequest.GetResponse();
            Stream s = responce.GetResponseStream();
            StreamReader sr = new StreamReader(s);

            // 返回数据流(源码) 
            string str = sr.ReadToEnd();
            return "";
        }

        public static byte[] APIPostBack(string url, string posts, string contentType)
        {

            byte[] postData = Encoding.UTF8.GetBytes(posts);
            //找退出原因
            //LogHelper.Info(url + posts);
            WebClient client = new WebClient();

            client.Headers.Add("Content-Type", contentType);
            client.Headers.Add("ContentLength", postData.Length.ToString());
            //这里url要组装安全标记等参数
            byte[] responseData = null;

            try
            {
                responseData = client.UploadData(url, "POST", postData);

                //找退出原因
                //LogHelper.Info(result);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);

            }
            client.Dispose();
            return responseData;
        }
    }

    public static class ObjectExtension
    {
        public static string ToJson(this Object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }

    public class FileAccessoryEntity
    {
        public string name { get; set; }
        public string path { get; set; }
        public int flietypecode { get; set; }
        public int filesubtypecode { get; set; }
    }

}