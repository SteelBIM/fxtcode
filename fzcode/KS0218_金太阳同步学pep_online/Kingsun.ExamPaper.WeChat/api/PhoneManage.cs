using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kingsun.ExamPaper.Common;
using Kingsun.ExamPaper.Model;

namespace Kingsun.ExamPaper.WeChat.api
{
    class PhoneManage : BaseManagement
    {
        /// <summary>
        /// 把手机 号和验证码插入数据库
        /// </summary>
        /// <param name="phonecode"></param>
        /// <returns></returns>
        public bool InInsert(Tb_PhoneCode phonecode)
        {
            if (Insert<Tb_PhoneCode>(phonecode))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 校验短信验证码
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool CheckPhoneCode(string phone, string code)
        {
            IList<Tb_PhoneCode> pclist = Search<Tb_PhoneCode>("TelePhone='" + phone + "'", "ID desc");
            if (pclist == null || pclist[0].State == 0 || pclist[0].Code != code || pclist[0].EndDate < DateTime.Now)
            {
                return false;
            }
            else
            {
                pclist[0].State = 0;
                Update<Tb_PhoneCode>(pclist[0]);
                return true;
            }
        }
    }
}