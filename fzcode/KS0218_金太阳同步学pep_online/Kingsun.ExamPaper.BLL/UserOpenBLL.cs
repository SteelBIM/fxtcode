using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingsun.ExamPaper.DAL;
using Kingsun.ExamPaper.Model;

namespace Kingsun.ExamPaper.BLL
{
    public class UserOpenBLL
    {
        UserOpenDAL uoDAL = new UserOpenDAL();

        /// <summary>
        ///  获取OpenId对应手机号
        /// </summary>
        /// <returns></returns>
        public TB_UserOpenID GetPhoneByOpenId(string where)
        {
            return uoDAL.GetPhoneByOpenId(where);
        }
    }
}
