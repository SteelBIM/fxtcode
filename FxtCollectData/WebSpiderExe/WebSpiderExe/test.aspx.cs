using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebSpiderExe
{
    public partial class test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)WebRequest.Create(strURL);
            request.Headers.Add("Cookie", "ASP.NET_SessionId=zjxiwksxy2l3hgpv4gfqq4at");
            request.Method = "post";
            System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
            System.IO.StreamReader myreader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string content = myreader.ReadToEnd();
        }

        /// <summary>
        /// 获取功能参数安全验证加密code
        /// </summary>
        /// <param name="certifyArray">certifyArray：加密所使用的参数 {"appid", "signname", "apppwd", "time","appkey"}</param>
        /// <param name="type">type:{functionname}</param>
        /// <returns></returns>
        public static int ApiFunctionArgsVerify(string[] certifyArray, string code, string appkey)
        {
            string strdate = certifyArray[3];
            string certifyCode = GetApiFunctionArgsVerifyCode(certifyArray, appkey);
            if (certifyCode != code)
            {
                return -1;
            }
            else
            {
                return 1;//CheckTime(strdate);//用于验证时间差，暂时去掉的原因是配合云查勘IOS通过验证
            }
        }

        public static string GetApiFunctionArgsVerifyCode(string[] certifyArray, string appkey)
        {
            string[] curArray = certifyArray;
            Array.Sort(certifyArray);
            string certify = string.Join("", certifyArray);
            string certifyCode = GetMd5(certify, appkey);
            return certifyCode;
        }

        public static string GetMd5(string strmd5, string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                strmd5 += key;
            }
            byte[] md5Bytes = ASCIIEncoding.Default.GetBytes(strmd5);
            byte[] encodedBytes;
            MD5 md5;
            md5 = new MD5CryptoServiceProvider();
            //FileStream fs= new FileStream(filepath,FileMode.Open,FileAccess.Read);
            encodedBytes = md5.ComputeHash(md5Bytes);
            string nn = BitConverter.ToString(encodedBytes);
            nn = Regex.Replace(nn, "-", "");//因为转化完的都是34-2d这样的，所以替换掉- 
            nn = nn.ToLower();//根据需要转化成小写
            //fs.Close();
            return nn;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                string ids = TextBox1.Text;
                string[] idlist = Regex.Split(ids, "\r", RegexOptions.IgnoreCase);
                string newids = "";
                foreach (string id in idlist)
                {
                    string newid = "<br>";
                    foreach (char c in id)
                    {
                        switch (c)
                        {
                            case '0':
                                newid += "0";
                                break;
                            case '1':
                                newid += "2";
                                break;
                            case '2':
                                newid += "5";
                                break;
                            case '3':
                                newid += "8";
                                break;
                            case '4':
                                newid += "6";
                                break;
                            case '5':
                                newid += "1";
                                break;
                            case '6':
                                newid += "3";
                                break;
                            case '7':
                                newid += "4";
                                break;
                            case '8':
                                newid += "9";
                                break;
                            case '9':
                                newid += "7";
                                break;

                        }
                    }
                    newids += newid;
                }
                divid.InnerHtml = newids;
            }
            catch { }   
        }
    }
}