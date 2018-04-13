
using Kingsun.IBS.IBLL.IBS2MOD;
using Kingsun.IBS.Model;
using Kingsun.IBS.Model.MOD;
using Kingsun.SynchronousStudy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.IBS.BLL.IBS2MOD
{
    public class MOD2IBS_SchInfoBLL :CommonBLL, IIBS_MOD_SchInfoBLL
    {
        static RedisHashOtherHelper hashRedis = new RedisHashOtherHelper();
        //消息队列
        static RedisListOtherHelper listRedis = new RedisListOtherHelper();
        public bool Add(IBS_SchClassRelation SchInfo)
        {
            var result = false;
            try
            {
                var schInfo = hashRedis.Get<IBS_SchClassRelation>("IBS_SchClassRelation", SchInfo.SchID.ToString());
                if (schInfo != null) 
                {
                    //移除IBS_AreaSchRelation学校关系
                    var area=hashRedis.Get<IBS_AreaSchRelation>("IBS_AreaSchRelation", schInfo.AreaID.ToString());
                    if (area != null) 
                    {
                        area.AreaSchList.RemoveAll(a => a.SchD == schInfo.SchID);
                        hashRedis.Set<IBS_AreaSchRelation>("IBS_AreaSchRelation", schInfo.AreaID.ToString(), area);
                    }
                }else
                {
                    schInfo = new IBS_SchClassRelation();
                }
                
                schInfo.SchID = SchInfo.SchID;
                schInfo.SchName = SchInfo.SchName;
                schInfo.AreaID = SchInfo.AreaID;
                hashRedis.Set<IBS_SchClassRelation>("IBS_SchClassRelation", SchInfo.SchID.ToString(), schInfo);

                //新增IBS_AreaSchRelation学校关系
                var newArea = hashRedis.Get<IBS_AreaSchRelation>("IBS_AreaSchRelation", SchInfo.AreaID.ToString());
                if (newArea != null)
                {
                    var sc = newArea.AreaSchList.Where(a => a.SchD == SchInfo.SchID).FirstOrDefault();
                    if (sc == null)
                    {
                        newArea.AreaSchList.Add(new AreaSchS()
                        {
                            SchD = SchInfo.SchID,
                            SchName = SchInfo.SchName
                        });
                         hashRedis.Set<IBS_AreaSchRelation>("IBS_AreaSchRelation", SchInfo.AreaID.ToString(), newArea);
                    }
                    
                }
                else 
                {
                    var are = BuildAreaInfoByAreaId(SchInfo.AreaID);
                    if (are != null) 
                    {
                        AreaSchS asc=new AreaSchS();
                        asc.SchD=SchInfo.SchID;
                        asc.SchName=SchInfo.SchName;
                        are.AreaSchList.Add(asc);
                        hashRedis.Set<IBS_AreaSchRelation>("IBS_AreaSchRelation", SchInfo.AreaID.ToString(), are);
                    }
                    
                }
                
                result = true;
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "学校Add接口异常，SchInfo=" + SchInfo.ToJson());
                //新增到异常处理消息队列
                MOD2IBS_PushDataException excep = new MOD2IBS_PushDataException();
                excep.DataType = 3;
                excep.ChangeType = 2;
                excep.Json = SchInfo.ToJson();
                excep.ErrorMessage = "学校Add接口异常！" + ex.Message;
                listRedis.LPush("MOD2IBS_PushDataException", excep.ToJson());

                result=false;

            }
            return result;
        }

        public bool Update(IBS_SchClassRelation SchInfo)
        {

            var result = false;
            try
            {
                var schInfo = hashRedis.Get<IBS_SchClassRelation>("IBS_SchClassRelation", SchInfo.SchID.ToString());
                if (schInfo != null)
                {
                    //移除IBS_AreaSchRelation学校关系
                    var area = hashRedis.Get<IBS_AreaSchRelation>("IBS_AreaSchRelation", schInfo.AreaID.ToString());
                    if (area != null)
                    {
                        area.AreaSchList.RemoveAll(a => a.SchD == schInfo.SchID);
                        hashRedis.Set<IBS_AreaSchRelation>("IBS_AreaSchRelation", schInfo.AreaID.ToString(), area);
                    }
                }
                else
                {
                    schInfo = new IBS_SchClassRelation();
                }

                schInfo.SchID = SchInfo.SchID;
                schInfo.SchName = SchInfo.SchName;
                schInfo.AreaID = SchInfo.AreaID;
                hashRedis.Set<IBS_SchClassRelation>("IBS_SchClassRelation", SchInfo.SchID.ToString(), schInfo);

                //新增IBS_AreaSchRelation学校关系
                var newArea = hashRedis.Get<IBS_AreaSchRelation>("IBS_AreaSchRelation", SchInfo.AreaID.ToString());
                if (newArea != null)
                {
                    var sc = newArea.AreaSchList.Where(a => a.SchD == SchInfo.SchID).FirstOrDefault();
                    if (sc == null)
                    {
                        newArea.AreaSchList.Add(new AreaSchS()
                        {
                            SchD = SchInfo.SchID,
                            SchName = SchInfo.SchName
                        });
                        hashRedis.Set<IBS_AreaSchRelation>("IBS_AreaSchRelation", schInfo.AreaID.ToString(), newArea);
                    }
                }
                else 
                {
                    var are = BuildAreaInfoByAreaId(SchInfo.AreaID);
                    if (are != null)
                    {
                        AreaSchS asc = new AreaSchS();
                        asc.SchD = SchInfo.SchID;
                        asc.SchName = SchInfo.SchName;
                        are.AreaSchList.Add(asc);
                        hashRedis.Set<IBS_AreaSchRelation>("IBS_AreaSchRelation", SchInfo.AreaID.ToString(), are);
                    }
                }

                result = true;
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "学校Update接口异常，SchInfo=" + SchInfo.ToJson());
                //新增到异常处理消息队列
                MOD2IBS_PushDataException excep = new MOD2IBS_PushDataException();
                excep.DataType = 3;
                excep.ChangeType = 1;
                excep.Json = SchInfo.ToJson();
                excep.ErrorMessage = "学校Update接口异常！" + ex.Message;
                listRedis.LPush("MOD2IBS_PushDataException", excep.ToJson());

                result = false;

            }
            return result;
        }

        public bool Delete(IBS_SchClassRelation SchInfo)
        {
            var result = false;
            try
            {
                var sch = hashRedis.Get<IBS_SchClassRelation>("IBS_SchClassRelation", SchInfo.SchID.ToString());
                if (sch != null) 
                {

                    sch.SchClassList.ForEach(a => 
                    {
                        //移除学校关联班级信息
                        var classinfo = hashRedis.Get<IBS_ClassUserRelation>("IBS_ClassUserRelation", a.ClassID.ToUpper());
                        if (classinfo != null) 
                        {
                            //移除班级与学生关联信息
                            classinfo.ClassStuList.ForEach(x => 
                            {
                                var stu = hashRedis.Get<IBS_UserInfo>("IBS_UserInfo", x.StuID.ToString());
                                if (stu != null) 
                                {
                                    stu.ClassSchList.RemoveAll(y => y.ClassID.ToUpper() == classinfo.ClassID.ToUpper());
                                    hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", x.StuID.ToString(), stu);
                                }
                            });
                            //移除班级与教师关联信息
                            classinfo.ClassTchList.ForEach(x =>
                            {
                                var tea = hashRedis.Get<IBS_UserInfo>("IBS_UserInfo", x.TchID.ToString());
                                if (tea != null)
                                {
                                    tea.ClassSchList.RemoveAll(y => y.ClassID.ToUpper() == classinfo.ClassID.ToUpper());
                                    hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", x.TchID.ToString(), tea);
                                }
                            });
                        }
                        hashRedis.Remove("IBS_ClassUserRelation", a.ClassID.ToUpper());
                    });
                }
                //移除IBS_AreaSchRelation学校关系
                var area = hashRedis.Get<IBS_AreaSchRelation>("IBS_AreaSchRelation", SchInfo.AreaID.ToString());
                if (area != null)
                {
                    area.AreaSchList.RemoveAll(a => a.SchD == SchInfo.SchID);
                    hashRedis.Set<IBS_AreaSchRelation>("IBS_AreaSchRelation", SchInfo.AreaID.ToString(), area);
                }

                hashRedis.Remove("IBS_SchClassRelation", SchInfo.SchID.ToString());
                result = true;
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "学校DEL接口异常，SchInfo=" + SchInfo.ToJson());
                //新增到异常处理消息队列
                MOD2IBS_PushDataException excep = new MOD2IBS_PushDataException();
                excep.DataType = 3;
                excep.ChangeType = 3;
                excep.Json = SchInfo.ToJson();
                excep.ErrorMessage = "学校DEL接口异常！" + ex.Message;
                listRedis.LPush("MOD2IBS_PushDataException", excep.ToJson());

                result = false;

            }
            return result;
        }

    }
}
