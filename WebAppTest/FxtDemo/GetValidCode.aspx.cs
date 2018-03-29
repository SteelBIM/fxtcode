using CAS.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading.Tasks;
using System.Data;

namespace FxtDemo.FxtGjbApi
{
    public partial class GetValidCode : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("zh-cn");
            DataTable dt = new DataTable();
            dt.Columns.Add("name");
            dt.Columns.Add("age");
            dt.Rows.Add("圣诞快乐101", "60");
            dt.Rows.Add("圣诞快乐100", "10");
            dt.Rows.Add("圣诞快乐37", "30");
            dt.Columns.Add("nameorder", typeof(string), "name like '老[[][*][]]马%'");
            DataView dv = dt.DefaultView;
            dv.Sort = "name";
            string[] ss = new string[] { "圣诞快乐101", "圣诞快乐100", "圣诞快乐25", "圣诞快乐一" };
            IComparer<string> fileNameComparer = new CNComparer();
            List<string> orderli = ss.ToList();
            orderli.Sort(fileNameComparer);
        }

        protected void btn_GetCode_Click(object sender, EventArgs e)
        {
            string functionname = txt_functionname.Text;
            string appid = txt_appid.Text;
            string apppwd = txt_apppwd.Text;
            string appkey = txt_appkey.Text;
            string signname = txt_signname.Text;
            string time = txt_time.Text; 
            string[] pwdArray = { appid, apppwd, signname, time, functionname };
            string code = EncryptHelper.GetMd5(pwdArray, appkey);
            string sinfo = "{\\\\\"appid\\\\\":\\\\\"" + appid + "\\\\\",\\\\\"apppwd\\\\\":\\\\\"" + apppwd + "\\\\\",\\\\\"signname\\\\\":\\\\\"" + signname + "\\\\\",\\\\\"time\\\\\":\\\\\"" + time + "\\\\\",\\\\\"code\\\\\":\\\\\"" + code + "\\\\\",\\\\\"functionname\\\\\":\\\\\"" + functionname + "\\\\\"}";
            Response.Write("{\\\"sinfo\\\":\\\"" + sinfo + "\\\",其他参数}");
        }
    }

    public class CNComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x == null || y == null)
                throw new ArgumentException("Parameters can't be null");
            string fileA = x;
            string fileB = y;
            char[] arr1 = fileA.ToCharArray();
            char[] arr2 = fileB.ToCharArray();
            int i = 0, j = 0;
            while (i < arr1.Length && j < arr2.Length)
            {
                if (char.IsDigit(arr1[i]) && char.IsDigit(arr2[j]))
                {
                    string s1 = "", s2 = "";
                    while (i < arr1.Length && char.IsDigit(arr1[i]))
                    {
                        s1 += arr1[i];
                        i++;
                    }
                    while (j < arr2.Length && char.IsDigit(arr2[j]))
                    {
                        s2 += arr2[j];
                        j++;
                    }
                    if (int.Parse(s1) > int.Parse(s2))
                    {
                        return 1;
                    }
                    if (int.Parse(s1) < int.Parse(s2))
                    {
                        return -1;
                    }
                }
                else
                {
                    if (arr1[i] > arr2[j])
                    {
                        return 1;
                    }
                    if (arr1[i] < arr2[j])
                    {
                        return -1;
                    }
                    i++;
                    j++;
                }
            }
            if (arr1.Length == arr2.Length)
            {
                return 0;
            }
            else
            {
                return arr1.Length > arr2.Length ? 1 : -1;
            }
            //            return string.Compare( fileA, fileB );
            //            return( (new CaseInsensitiveComparer()).Compare( y, x ) );
        }
    }
}