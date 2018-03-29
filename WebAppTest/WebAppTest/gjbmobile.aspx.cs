using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace WebAppTest
{
    public partial class gjbmobile : System.Web.UI.Page
    {
        public string db = string.Empty;
        public string strdate = string.Empty;
        public string strcode = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            db = "41F11989F545640A6426E812639CAA60D17386DFAADE5FCEC6460727104FB040507A3346586FDE12BCA39F81E0088A3C2E3FB5A57F69AE13DE69C6D3261FD2397F2164EFB8769C91BD19B2624413EDE16C742D3D29824FEE";

            strdate = GetSysCode(date);
            strcode = date;
            string filename = "adf.jpg?123564";
            string ext = filename.Substring(0,filename.LastIndexOf("?")).ToLower();
            ext = ext.Substring(ext.LastIndexOf(".")).ToLower();
        }

      

        /// <summary>
        /// 检查验证码
        /// </summary>
        /// <param name="strDate">发送请求的时间</param>
        /// <param name="strCode">加密码</param>
        /// <returns>1：验证通过，0：时间误差过大，-1：加密码错误</returns>
        public int CheckSysCode()
        {
#if DEBUG
            return 1;
#endif
            HttpContext con = HttpContext.Current;
            string strDate = HttpUtility.UrlDecode(con.Request["strdate"].ToString(), Encoding.UTF8);
            string strCode = con.Request["strcode"].ToString();
            DateTime date = Convert.ToDateTime(strDate);
            long ltime = DateTime.Now.ToFileTime() - date.ToFileTime();
            if (ltime > 6000000000 || ltime < -6000000000)//误差超过10分钟
                return 0;
            if (strCode != GetSysCode(strDate))
                return -1;
            return 1;
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

        private void uploadfile() 
        {
            try
            {
                string urlpath = string.Empty;
                HttpContext context = System.Web.HttpContext.Current;
                HttpRequest request = context.Request;
                string uploadroot = "upload";
                //这里直接用DateTime.Now.ToString("yyyy/MM/dd")不行，代码返回还是yyyy-mm-dd格式，不知道为什么 kevin
                string uploadpath = "365/OA/" + DateTime.Now.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                //string uploadpath = DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Day.ToString("00");
                string savepath = context.Server.MapPath("/" + uploadroot + "/" + uploadpath + "/");
                urlpath = uploadroot + "/" + uploadpath + "/";
                if (!Directory.Exists(savepath))
                    Directory.CreateDirectory(savepath);
                string filename = context.Request.Files[0].FileName.ToLower();
                string filepath = urlpath + filename;
                string file = context.Server.MapPath("/" + filepath);
                context.Request.Files[0].SaveAs(file);
            }
            catch (Exception ex)
            {
                
                throw;
            }
            
        }
    }
}