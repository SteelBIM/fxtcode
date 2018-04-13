using CBSS.Framework.Redis;
using CBSS.IBS.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBSS.Core.Utility;
using CBSS.Core.Log;

namespace CBSS.IBS.BLL
{
    public class MOD2IBS_AreaInfoBLL : IIBS_MOD_AreaInfoBLL
    {

        static RedisSortedSetHelper sortedSetRedis = new RedisSortedSetHelper("IBS");
        static RedisHashHelper hashRedis = new RedisHashHelper("IBS");
        //消息队列
        static RedisListHelper listRedis = new RedisListHelper("IBS");

        public bool Add(IBS_AreaSchRelation AreaInfo)
        {
            var result = false;
            try
            {
                var areaInfo = hashRedis.Get<IBS_AreaSchRelation>("IBS_AreaSchRelation", AreaInfo.AreaID.ToString());
                if (areaInfo == null) 
                {
                    areaInfo = new IBS_AreaSchRelation();
                }
                areaInfo.AreaID = AreaInfo.AreaID;
                areaInfo.AreaName = AreaInfo.AreaName;
                hashRedis.Set<IBS_AreaSchRelation>("IBS_AreaSchRelation", AreaInfo.AreaID.ToString(), areaInfo);
                result = true;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(LoggerType.ServiceExceptionLog, "区域Add接口异常，AreaInfo=" + AreaInfo.ToJson(), ex);
                //新增到异常处理消息队列
                MOD2IBS_PushDataException excep = new MOD2IBS_PushDataException();
                excep.DataType = 5;
                excep.ChangeType = 2;
                excep.Json = AreaInfo.ToJson();
                excep.ErrorMessage = "区域Add接口异常！" + ex.Message;
                listRedis.LPush("MOD2IBS_PushDataException", excep.ToJson());


                result = false;

            }
            return result;
        }

        public bool Update(IBS_AreaSchRelation AreaInfo)
        {
            var result = false;
            try
            {
                var areaInfo = hashRedis.Get<IBS_AreaSchRelation>("IBS_AreaSchRelation", AreaInfo.AreaID.ToString());
                if (areaInfo == null)
                {
                    areaInfo = new IBS_AreaSchRelation();
                }
                areaInfo.AreaID = AreaInfo.AreaID;
                areaInfo.AreaName = AreaInfo.AreaName;
                hashRedis.Set<IBS_AreaSchRelation>("IBS_AreaSchRelation", AreaInfo.AreaID.ToString(), areaInfo);
                result = true;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(LoggerType.ServiceExceptionLog, "区域Update接口异常，AreaInfo=" + AreaInfo.ToJson(), ex);
                //新增到异常处理消息队列
                MOD2IBS_PushDataException excep = new MOD2IBS_PushDataException();
                excep.DataType = 5;
                excep.ChangeType = 1;
                excep.Json = AreaInfo.ToJson();
                excep.ErrorMessage = "区域Update接口异常！" + ex.Message;
                listRedis.LPush("MOD2IBS_PushDataException", excep.ToJson());

                result = false;

            }
            return result;
        }

        public bool Delete(IBS_AreaSchRelation AreaInfo)
        {
            var result = false;
            try
            {
                hashRedis.Remove("IBS_AreaSchRelation",AreaInfo.AreaID.ToString());
                result = true;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(LoggerType.ServiceExceptionLog, "区域Del接口异常，AreaInfo=" + AreaInfo.ToJson(), ex);
                //新增到异常处理消息队列
                MOD2IBS_PushDataException excep = new MOD2IBS_PushDataException();
                excep.DataType = 5;
                excep.ChangeType = 3;
                excep.Json = AreaInfo.ToJson();
                excep.ErrorMessage = "区域Del接口异常！" + ex.Message;
                listRedis.LPush("MOD2IBS_PushDataException", excep.ToJson());

                result = false;

            }
            return result;
        }


    }
}
