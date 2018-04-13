using CourseActivate.Activate.Constract.Model;
using CourseActivate.Activate.DAL;
using CourseActivate.Core.Utility;
using CourseActivate.Framework.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseActivate.Account.Constract.VW;
using CourseActivate.Account.Constract.Models;

namespace CourseActivate.Activate.BLL
{
    public class LoginRedis
    {       
        //public void SetActionInfo(string key, List<vw_action> value)
        //{
        //    new DoRedisString().Set<List<vw_action>>(key, value);
        //}
        //public void SetLoginInfo(string key, com_master value)
        //{
        //    com_master loginfo = new com_master() 
        //    {
        //        mastername = value.mastername
        //    };
        //    new DoRedisString().Set<com_master>(key, loginfo);
        //}

        //public List<vw_action> GetActionInfo(string key)
        //{
        //    return new DoRedisString().Get<List<vw_action>>(key);
        //}
        //public com_master GetLoginInfo(string key)
        //{
        //    return new DoRedisString().Get<com_master>(key);
        //}

        public void Remove(string key)
        {
            new DoRedisString().Remove(key);
        }
    }
}
