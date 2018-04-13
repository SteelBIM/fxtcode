using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingsun.ExamPaper.Common;
using Kingsun.ExamPaper.Model;

namespace Kingsun.ExamPaper.DAL
{
    public class UserOpenDAL : BaseManagement
    {
        /// <summary>
        ///  获取OpenId对应手机号
        /// </summary>
        /// <returns></returns>
        public TB_UserOpenID GetPhoneByOpenId(string where)
        {
            TB_UserOpenID list = SelectByCondition<TB_UserOpenID>(where);
            return list;
        }
    }
}
