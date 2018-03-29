using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FxtDemo
{
    public partial class appkey : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string content = GenerateCheckCode(12);//EncryptHelper.GetMd5(con, "gjbcqhf$%2014");
            Response.Write(content);
            Response.End();
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
                if ((i%2)==0)
                {
                    str = str + ch.ToString().ToLower();
                }
                else
                {
                    str = str + ch.ToString();
                }
            }
            return str;
        }
    }
}