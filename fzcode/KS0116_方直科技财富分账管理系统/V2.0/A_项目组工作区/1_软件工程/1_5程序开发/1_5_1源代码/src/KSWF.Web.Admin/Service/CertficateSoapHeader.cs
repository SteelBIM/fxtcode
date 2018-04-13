using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Protocols;

namespace KSWF.Web.Admin.Service
{
    public class CertficateSoapHeader : SoapHeader
    {
        /// <summary>
        /// 属性
        /// </summary>
        public string UserName { get; set; }
        public string PassWord { get; set; }



        public CertficateSoapHeader() { }
        /// <summary>
        /// 构造函数认证
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="passWord">密码</param>
        public CertficateSoapHeader(string userName, string passWord)
        {
            this.UserName = userName;
            this.PassWord = passWord;
        }
    }
}
