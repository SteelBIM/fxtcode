using Kingsun.IBS.IBLL.IBS2MOD;
using Kingsun.IBS.Model;
using Kingsun.IBS.Model.MOD;
using Kingsun.SynchronousStudy.Common;
using System;

namespace Kingsun.IBS.BLL.IBS2MOD
{
    public class MOD2IBS_AreaInfoBLL : IIBS_MOD_AreaInfoBLL
    {
        static RedisHashOtherHelper hashRedis = new RedisHashOtherHelper();
        //消息队列
        static RedisListOtherHelper listRedis = new RedisListOtherHelper();

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
                Log4Net.LogHelper.Error(ex, "区域Add接口异常，AreaInfo=" + AreaInfo.ToJson());
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
                Log4Net.LogHelper.Error(ex, "区域Update接口异常，AreaInfo=" + AreaInfo.ToJson());
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
                Log4Net.LogHelper.Error(ex, "区域Del接口异常，AreaInfo=" + AreaInfo.ToJson());
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
