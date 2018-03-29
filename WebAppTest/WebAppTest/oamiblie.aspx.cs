using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;

namespace WebAppTest
{
    public partial class oamiblie : System.Web.UI.Page
    {
        public string db = string.Empty;
        public string strdate = string.Empty;
        public string strcode = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }
        /// <summary>
        /// 得到系统验证码
        /// </summary>
        /// <returns></returns>
        public string GetSysCode(string strCode)
        {
            //string strdate = DateTime.Now.ToShortDateString();
            return GetMd5("123" + strCode + "321");
        }

        /// <summary>
        /// 进行MD5效验
        /// </summary>
        /// <param name="strmd5"></param>
        /// <returns></returns>
        public string GetMd5(string strmd5)
        {
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
             string date = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            db = "41F11989F545640A6426E812639CAA60D17386DFAADE5FCEC6460727104FB040507A3346586FDE12BCA39F81E0088A3C2E3FB5A57F69AE13DE69C6D3261FD2397F2164EFB8769C91BD19B2624413EDE16C742D3D29824FEE";

            strdate = GetSysCode(date);
            string url = "http://localhost:9998/";

            url += sendTb.Text + "&strcode=" + strdate+"&token1="+db+"&strdate="+date;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";

            MemoryStream memory = new MemoryStream();


            //ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] postdata = Encoding.GetEncoding("UTF-8").GetBytes("");
            request.ContentLength = postdata.Length;
            Stream newStream = request.GetRequestStream();
            newStream.Write(postdata, 0, postdata.Length);
            newStream.Close();

            HttpWebResponse myResponse = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            string content = reader.ReadToEnd();//得到结果
            acceptTb.Text = content;
        }
    }
}