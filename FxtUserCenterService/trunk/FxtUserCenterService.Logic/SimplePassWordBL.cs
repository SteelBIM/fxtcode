using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtUserCenterService.DataAccess;

namespace FxtUserCenterService.Logic
{
    public class SimplePassWordBL
    {
        /// <summary>
        /// 修改用户密码 hody 2014-07-25
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static int CheckIsSimplePassWord(string simplePassWord)
        {
            return SimplePassWordDA.CheckIsSimplePassWord(simplePassWord);
        }
    }
}
