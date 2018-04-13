using CBSS.Framework.Redis;
using CBSS.Core.Utility;
using CBSS.IBS.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBSS.Framework.DAL;
using CBSS.Core.Log;
using CBSS.Framework.Contract;
using CBSS.IBS.DAL;
using CBSS.IBS.IBLL;

namespace CBSS.IBS.BLL
{
    /// <summary>
    /// MOD2IBS数据变动业务类
    /// </summary>
    public class MOD2IBSChangeBLL : IMOD2IBSChangeBLL
    {
        static Repository repository = new Repository();
        //消息队列
        static RedisListHelper listRedis = new RedisListHelper("IBS");

        static RedisSortedSetHelper sortedSetRedis = new RedisSortedSetHelper("IBS");
        static RedisHashHelper hashRedis = new RedisHashHelper("IBS");

        IIBS_MOD_UserInfoBLL userBLL = new MOD2IBS_UserInfoBLL();
        IIBS_MOD_UserClassRelationBLL userClassBLL = new MOD2IBS_UserClassRelationBLL();
        IIBS_MOD_ClassInfoBLL classBLL = new MOD2IBS_ClassInfoBLL();
        IIBS_MOD_SchInfoBLL schBLL = new MOD2IBS_SchInfoBLL();
        IIBS_MOD_AreaInfoBLL areaBLL=new MOD2IBS_AreaInfoBLL();

        IIBSService tbxBLL = new IBSService();

        ProcDBContext proc = new ProcDBContext();
        public void Change()
        {
            var listCount = listRedis.Count("MODIBSListKey");
            int Count = Convert.ToInt32(listCount) > 1000 ? 1000 : Convert.ToInt32(listCount);
            int Changetype = 0;
            int DataType = 0;
            for (int i = 0; i < Count; i++)
            {
                var model = listRedis.RemoveStartFromList("MODIBSListKey");
                try 
                {
                    if (!string.IsNullOrEmpty(model))
                    {
                        MOD_PushData data = model.FromJson<MOD_PushData>();
                        DataType = data.DataType;
                        Changetype = data.ChangeType;
                        //数据类型,1=账号信息（暂不用）,2=班级信息,3=学校信息,4=用户信息,5=区域信息,6=课程信息,7=用户班级关系,8=班级学校关系,9=学校区域关系
                        switch (data.DataType)
                        {
                            case 2:
                                var classInfo = data.Data.FromJson<IBS_ClassUserRelation>();
                                if (classInfo != null)
                                {
                                    switch (data.ChangeType)
                                    {
                                        case 1:
                                            classBLL.Update(classInfo);
                                            break;
                                        case 2:
                                            classBLL.Add(classInfo);
                                            break;
                                        case 3:
                                            classBLL.Delete(classInfo);
                                            break;
                                    }
                                }
                                break;
                            case 3:
                                var schInfo = data.Data.FromJson<IBS_SchClassRelation>();
                                if (schInfo != null)
                                {
                                    switch (data.ChangeType)
                                    {
                                        case 1:
                                            schBLL.Update(schInfo);
                                            break;
                                        case 2:
                                            schBLL.Add(schInfo);
                                            break;
                                        case 3:
                                            schBLL.Delete(schInfo);
                                            break;
                                    }
                                }
                                break;
                            case 4:
                                var user = data.Data.FromJson<IBS_UserInfo>();
                                if (user != null)
                                {
                                    switch (data.ChangeType)
                                    {
                                        case 1:
                                            userBLL.Update(user);
                                            break;
                                        case 2:
                                            userBLL.Add(user);
                                            break;
                                        case 3:
                                            userBLL.Delete(user);
                                            break;
                                    }
                                }
                                break;
                            case 5:
                                var areaInfo = data.Data.FromJson<IBS_AreaSchRelation>();
                                if (areaInfo != null)
                                {
                                    switch (data.ChangeType)
                                    {
                                        case 1:
                                            areaBLL.Update(areaInfo);
                                            break;
                                        case 2:
                                            areaBLL.Add(areaInfo);
                                            break;
                                        case 3:
                                            areaBLL.Delete(areaInfo);
                                            break;
                                    }
                                }
                                break;
                            case 7:
                                var userClass = data.Data.FromJson<IBS_UserInfo>();
                                //0=查询,1=更新,2=增加,3=删除
                                switch (data.ChangeType)
                                {
                                    case 2:
                                        userClassBLL.ClassUserAdd(userClass);
                                        break;
                                    case 3:
                                        userClassBLL.ClassUserRemove(userClass);
                                        break;
                                   
                                }
                                break;
                        }
                    }
                }
                catch (Exception ex) 
                {
                    Log4NetHelper.Error(LoggerType.ServiceExceptionLog, "MOD同步IBS同步异常,异常数据：" + model, ex);
                    MOD2IBS_PushDataException exc = new MOD2IBS_PushDataException();
                    exc.ErrorMessage = ex.Message;
                    exc.Json = model;
                    exc.ChangeType = Changetype;
                    exc.DataType = DataType;
                    listRedis.LPush("MOD2IBS_PushDataException", exc.ToJson());
                }
                
            }
        }
        
    }
}
