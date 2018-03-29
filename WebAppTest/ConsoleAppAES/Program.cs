using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.Serialization;
using System.Net;
using System.Threading;
using System.Text.RegularExpressions;
//using ClassLibrary;

namespace ConsoleAppAES
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 例
            //string[] arr = {"20140326125647","3","ddf67458966d8099ebe4a2d6ee1e894b","fxtgjb","fl_func_1" };
            //string[] arr = { "2", "3", "d", "f", "a","e" };
            //Array.Sort(arr);
            //string str = string.Join(",",arr);
            #region MyRegion
            //string access_token = "你的token";
            //string posturl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=wx28c6ec86f47f4f02&secret=4c20856105f7144bd9df0b3adbfed98a";
            //string menuStr = " 菜单结构";

            //HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(posturl);
            //request.ContentType = "application/x-www-form-urlencoded";
            //request.Method = "POST";

            ////ASCIIEncoding encoding = new ASCIIEncoding();
            //byte[] postdata = Encoding.GetEncoding("UTF-8").GetBytes(menuStr);
            //request.ContentLength = postdata.Length;

            //Stream newStream = request.GetRequestStream();
            //newStream.Write(postdata, 0, postdata.Length);
            //newStream.Close();

            //HttpWebResponse myResponse = (HttpWebResponse)request.GetResponse();
            //StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            //string content = reader.ReadToEnd();//得到结果

            //content = "a\n\0\01\nb\n\0\02";

            //string url = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token=gjb";
            //string param = "{\"button\":[{\"name\":\"关于\",\"sub_button\":[{\"type\":\"click\",\"name\":\"公司介绍\","
            //    + "\"key\":\"gjb_company_introduce\"},{\"type\":\"click\",\"name\":\"业务范围\",\"key\":\"gjb_company_business_area\""
            //+ "},{\"type\":\"click\",\"name\":\"经典案例\",\"key\":\"gjb_company_classic_case\"},{\"type\":\"click\",\"name\":\"联系方式\","
            //+ "\"key\":\"gjb_company_contact\"},{\"type\":\"click\",\"name\":\"意见反馈\",\"key\":\"gjb_company_feedback\""
            //+ "}] },{\"name\":\"微服务\",\"sub_button\":[{\"type\":\"click\",\"name\":\"住宅询价\",\"key\":\"gjb_house_enquiry\""
            //+ "},{\"type\":\"click\",\"name\":\"其它询价\",\"key\":\"gjb_other_enquiry\"},{\"type\":\"click\",\"name\":\"业务进度\","
            // + "\"key\":\"gjb_business_progress\"}]},{\"name\":\"OA\",\"sub_button\":[{\"type\":\"click\",\"name\":\"待办\","
            // + "\"key\":\"gjb_oa_pend\"},{\"type\":\"click\",\"name\":\"查价\",\"key\":\"gjb_oa_query\" }]}]}]}";

            //byte[] byteArray = Encoding.UTF8.GetBytes(param);
            //HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(url));
            //webReq.Method = "POST";
            //webReq.ContentType = "application/x-www-form-urlencoded";
            //webReq.ContentLength = byteArray.Length;

            //Stream newStream = webReq.GetRequestStream();
            //newStream.Write(byteArray, 0, byteArray.Length);
            //newStream.Close();

            //access_token  
            //expires_in
            //HttpWebResponse myResponse = (HttpWebResponse)webReq.GetResponse();
            //StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            //string content = reader.ReadToEnd();//得到结果
            #endregion
            //Program p = new Program();
            //string result = p.GenerateCheckCode(12);
            //Console.WriteLine(str);
            //Console.WriteLine(result);
            //Dictionary<string, string> MethodDic = new Dictionary<string, string>
            //    {
            //        //方法注释 QueryFromNoInternal  
            //        { "splist", "GetSearchProjectListByKey" },
            //        { "pev", "GetProjectDetailsByProjectid" },
            //        { "pdbp", "GetProjectEValue" },
            //        {"garealist","GetSYSAreaList"},
            //        {"gscodelist","GetSYSCodeList"},
            //        {"buildinglist","GetBuildingListByPid"},
            //        {"buildingbaseinfolist","GetBuildingBaseInfoList"},
            //        {"autobuildinginfolist","GetAutoBuildingInfoList"},
            //        {"houseunitlist","GetHouseUnitList"},
            //        {"housefloorlist","GetHouseNoList"},
            //        {"housedropdownlist","GetHouseDropDownList"},
            //        {"autohouselistlist","GetAutoHouseList"},
            //    };
            //bool result = MethodDic.ContainsKey("aaaaa");
            #endregion

            string r = string.Empty;
            string []result =  {"A","1","a"};
            for (int i = 0; i < result.Length; i++)
            {
                if (Regex.IsMatch(result[i], "[#-]$|[0-9]$|[a-z]$|[A-Z]$"))
                {
                    r += result[i]+"层\n";
                }
            }
            

             Console.WriteLine(r);
        }

       

        private int rep = 0;
        /// 
        /// 生成随机数字字符串
        /// 
        /// 待生成的位数
        /// 生成的数字字符串
        private string GenerateCheckCode(int codeCount)
        {
            string str = string.Empty;
            long num2 = DateTime.Now.Ticks + this.rep;
            this.rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> this.rep)));
            for (int i = 0; i < codeCount; i++)
            {
                char ch;
                int num = random.Next();
                if ((num % 2) == 0)
                {
                    ch = (char)(0x30 + ((ushort)(num % 10)));
                }
                else
                {
                    ch = (char)(0x41 + ((ushort)(num % 0x1a)));
                }
                str = str + ch.ToString();
            }
            return str;
        }


    }

    public class MyThread 
    {
        public static string data { get; set; }
        //public MyThread(string data) 
        //{
        //    this.data = data;
        //}

        public static void ThreadMain() 
        {
            Console.WriteLine( "Running in a thread data: {0}",data);
        }
    }
}
