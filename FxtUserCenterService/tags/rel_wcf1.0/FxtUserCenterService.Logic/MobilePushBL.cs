using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtUserCenterService.DataAccess;
using FxtUserCenterService.Entity;
using CAS.Entity.DBEntity;

namespace FxtUserCenterService.Logic
{
    public class MobilePushBL
    {


        /// <summary>
        /// 用户绑定手机设备Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int Bind(DatMobilePush model)
        {
            List<DatMobilePush> lstpush =  GetCheckUser(model.username,model.producttypecode);
            int result = 1;
            if (lstpush == null || lstpush.Count <= 0)
            {
               result= MobilePushDA.Add(model);
            }
            else {
                if (model.splatype == "android")
                {
                    model.iosphshuserid = "";
                }
                else if (model.splatype == "ios")
                {
                    model.andphshuserid = "";
                    model.channelid = "";
                }
                result=MobilePushDA.UpdatePush(model);
            }
            return result;
        }


        /// <summary>
        /// 获取用户 设备Id
        /// </summary>
        /// <param name="username"></param>
        /// <param name="producttypecode">产品</param>
        /// <returns></returns>
        public static List<DatMobilePush> GetCheckUser(string username, int? producttypecode)
        {
            return MobilePushDA.GetCheckUser(username, producttypecode);
        }

        /// 退出清空手机设备Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int ExitReMoveDiviceId(DatMobilePush model)
        {
            return MobilePushDA.ExitReMoveDiviceId(model);
        }

        /// <summary>
        /// 效验token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static int VerifyToken(string token)
        {
            return MobilePushDA.VerifyToken(token);
        }
    }
}
