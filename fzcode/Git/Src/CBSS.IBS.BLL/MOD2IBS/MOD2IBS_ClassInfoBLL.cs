using CBSS.Core.Log;
using CBSS.Core.Utility;
using CBSS.Framework.Redis;
using CBSS.IBS.BLL;
using CBSS.IBS.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.IBS.BLL
{
    public class MOD2IBS_ClassInfoBLL :CommonBLL,IIBS_MOD_ClassInfoBLL
    {
        static RedisSortedSetHelper sortedSetRedis = new RedisSortedSetHelper("IBS");
        static RedisHashHelper hashRedis = new RedisHashHelper("IBS");
        //消息队列
        static RedisListHelper listRedis = new RedisListHelper("IBS");

        public bool Add(IBS_ClassUserRelation ClassInfo)
        {
            var result = false;
            try
            {
                var classInfo = hashRedis.Get<IBS_ClassUserRelation>("IBS_ClassUserRelation", ClassInfo.ClassID.ToUpper());
                if (classInfo != null)
                {
                    //移除IBS_ClassOtherID数据
                    hashRedis.Remove("IBS_ClassOtherID", classInfo.ClassNum.ToString() + "_" + 1);
                    //移除IBS_SchClassRelation数据
                    var sch = hashRedis.Get<IBS_SchClassRelation>("IBS_SchClassRelation", classInfo.SchID.ToString());
                    if (sch != null)
                    {
                        sch.SchClassList.RemoveAll(a => a.ClassID.ToUpper() == classInfo.ClassID.ToUpper());
                        hashRedis.Set<IBS_SchClassRelation>("IBS_SchClassRelation", classInfo.SchID.ToString(), sch);
                    }

                }
                else
                {
                    classInfo = new IBS_ClassUserRelation();
                }

                if (classInfo.SchID != ClassInfo.SchID)
                {
                    //修改学校时  需修改用户信息的班级信息对应学校区域字段
                    classInfo.ClassStuList.ForEach(a =>
                    {
                        var user = hashRedis.Get<IBS_UserInfo>("IBS_UserInfo", a.StuID.ToString());
                        if (user != null)
                        {
                            for (int i = 0; i < user.ClassSchList.Count; i++)
                            {
                                if (user.ClassSchList[i].ClassID.ToUpper() == ClassInfo.ClassID.ToUpper())
                                {
                                    user.ClassSchList[i].SchID = ClassInfo.SchID;
                                    var schinfo = hashRedis.Get<IBS_SchClassRelation>("IBS_SchClassRelation", ClassInfo.SchID.ToString());
                                    if (schinfo != null)
                                    {
                                        user.ClassSchList[i].AreaID = schinfo.AreaID;
                                    }

                                }
                            }
                            hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", a.StuID.ToString(), user);
                        }
                    });

                    //修改学校时  需修改用户信息的班级信息对应学校区域字段
                    classInfo.ClassTchList.ForEach(a =>
                    {
                        var user = hashRedis.Get<IBS_UserInfo>("IBS_UserInfo", a.TchID.ToString());
                        if (user != null)
                        {
                            for (int i = 0; i < user.ClassSchList.Count; i++)
                            {
                                if (user.ClassSchList[i].ClassID.ToUpper() == ClassInfo.ClassID.ToUpper())
                                {
                                    user.ClassSchList[i].SchID = ClassInfo.SchID;
                                    var schinfo = hashRedis.Get<IBS_SchClassRelation>("IBS_SchClassRelation", ClassInfo.SchID.ToString());
                                    if (schinfo != null)
                                    {
                                        user.ClassSchList[i].AreaID = schinfo.AreaID;
                                    }

                                }
                            }
                            hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", a.TchID.ToString(), user);
                        }
                    });
                }
                classInfo.ClassID = ClassInfo.ClassID.ToUpper();
                classInfo.ClassName = ClassInfo.ClassName;
                classInfo.ClassNum = ClassInfo.ClassNum;
                classInfo.GradeID = ClassInfo.GradeID;
                classInfo.GradeName = ClassInfo.GradeName;
                classInfo.SchID = ClassInfo.SchID;
                hashRedis.Set<IBS_ClassUserRelation>("IBS_ClassUserRelation", ClassInfo.ClassID.ToUpper(), classInfo);

                //新增IBS_ClassOtherID数据
                if (!string.IsNullOrEmpty(ClassInfo.ClassNum))
                {
                    IBS_ClassOtherID classnum = new IBS_ClassOtherID();
                    classnum.ClassIDOther = ClassInfo.ClassNum.ToString();
                    classnum.ClassID = ClassInfo.ClassID.ToUpper();
                    classnum.Type = 1;
                    hashRedis.Set<IBS_ClassOtherID>("IBS_ClassOtherID", ClassInfo.ClassNum.ToString() + "_" + 1, classnum);
                }

                //新增IBS_SchClassRelation关系
                if (ClassInfo.SchID > 0)
                {
                    var sch = hashRedis.Get<IBS_SchClassRelation>("IBS_SchClassRelation", ClassInfo.SchID.ToString());
                    if (sch != null)
                    {
                        var cl = sch.SchClassList.Where(a => a.ClassID.ToUpper() == ClassInfo.ClassID.ToUpper()).FirstOrDefault();
                        if (cl == null)
                        {
                            sch.SchClassList.Add(new SchClassS()
                            {
                                ClassID = ClassInfo.ClassID.ToUpper(),
                                ClassName = ClassInfo.ClassName,
                                GradeID = ClassInfo.GradeID,
                                GradeName = ClassInfo.GradeName
                            });
                            hashRedis.Set<IBS_SchClassRelation>("IBS_SchClassRelation", ClassInfo.SchID.ToString(), sch);
                        }
                    }
                    else
                    {
                        var modsc = BuildSchoolInfoBySchoolId(ClassInfo.SchID);
                        if (modsc != null)
                        {
                            SchClassS cc = new SchClassS();
                            cc.ClassID = ClassInfo.ClassID.ToUpper();
                            cc.ClassName = ClassInfo.ClassName;
                            cc.GradeID = ClassInfo.GradeID;
                            cc.GradeName = ClassInfo.GradeName;
                            modsc.SchClassList.Add(cc);
                            hashRedis.Set<IBS_SchClassRelation>("IBS_SchClassRelation", ClassInfo.SchID.ToString(), modsc);
                        }

                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(LoggerType.ServiceExceptionLog, "班级Add接口异常，ClassInfo=" + ClassInfo.ToJson(), ex);
                //新增到异常处理消息队列
                MOD2IBS_PushDataException excep = new MOD2IBS_PushDataException();
                excep.DataType = 2;
                excep.ChangeType = 2;
                excep.Json = ClassInfo.ToJson();
                excep.ErrorMessage = "班级Add接口异常！" + ex.Message;
                listRedis.LPush("MOD2IBS_PushDataException", excep.ToJson());

                result = false;

            }
            return result;
        }

        public bool Update(IBS_ClassUserRelation ClassInfo)
        {
            var result = false;
            try
            {
                var classInfo = hashRedis.Get<IBS_ClassUserRelation>("IBS_ClassUserRelation", ClassInfo.ClassID.ToUpper());
                if (classInfo != null)
                {
                    //移除IBS_ClassOtherID数据
                    hashRedis.Remove("IBS_ClassOtherID", classInfo.ClassNum.ToString() + "_" + 1);
                    //移除IBS_SchClassRelation数据
                    var sch = hashRedis.Get<IBS_SchClassRelation>("IBS_SchClassRelation", classInfo.SchID.ToString());
                    sch.SchClassList.RemoveAll(a => a.ClassID.ToUpper() == classInfo.ClassID.ToUpper());
                    hashRedis.Set<IBS_SchClassRelation>("IBS_SchClassRelation", classInfo.SchID.ToString(), sch);
                }
                else
                {
                    classInfo = new IBS_ClassUserRelation();
                }
                if (classInfo.SchID != ClassInfo.SchID)
                {
                    //修改学校时  需修改用户信息的班级信息对应学校区域字段
                    classInfo.ClassStuList.ForEach(a =>
                    {
                        var user = hashRedis.Get<IBS_UserInfo>("IBS_UserInfo", a.StuID.ToString());
                        if (user != null)
                        {
                            for (int i = 0; i < user.ClassSchList.Count; i++)
                            {
                                if (user.ClassSchList[i].ClassID.ToUpper() == ClassInfo.ClassID.ToUpper())
                                {
                                    user.ClassSchList[i].SchID = ClassInfo.SchID;
                                    var schinfo = hashRedis.Get<IBS_SchClassRelation>("IBS_SchClassRelation", ClassInfo.SchID.ToString());
                                    if (schinfo != null)
                                    {
                                        user.ClassSchList[i].AreaID = schinfo.AreaID;
                                    }

                                }
                            }
                            hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", a.StuID.ToString(), user);
                        }
                    });

                    //修改学校时  需修改用户信息的班级信息对应学校区域字段
                    classInfo.ClassTchList.ForEach(a =>
                    {
                        var user = hashRedis.Get<IBS_UserInfo>("IBS_UserInfo", a.TchID.ToString());
                        if (user != null)
                        {
                            for (int i = 0; i < user.ClassSchList.Count; i++)
                            {
                                if (user.ClassSchList[i].ClassID.ToUpper() == ClassInfo.ClassID.ToUpper())
                                {
                                    user.ClassSchList[i].SchID = ClassInfo.SchID;
                                    var schinfo = hashRedis.Get<IBS_SchClassRelation>("IBS_SchClassRelation", ClassInfo.SchID.ToString());
                                    if (schinfo != null)
                                    {
                                        user.ClassSchList[i].AreaID = schinfo.AreaID;
                                    }

                                }
                            }
                            hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", a.TchID.ToString(), user);
                        }
                    });
                }
                classInfo.ClassID = ClassInfo.ClassID.ToUpper();
                classInfo.ClassName = ClassInfo.ClassName;
                classInfo.ClassNum = ClassInfo.ClassNum;
                classInfo.GradeID = ClassInfo.GradeID;
                classInfo.GradeName = ClassInfo.GradeName;
                classInfo.SchID = ClassInfo.SchID;

                hashRedis.Set<IBS_ClassUserRelation>("IBS_ClassUserRelation", ClassInfo.ClassID.ToUpper(), classInfo);

                //新增IBS_ClassOtherID数据
                if (!string.IsNullOrEmpty(ClassInfo.ClassNum))
                {
                    IBS_ClassOtherID classnum = new IBS_ClassOtherID();
                    classnum.ClassIDOther = ClassInfo.ClassNum.ToString();
                    classnum.ClassID = ClassInfo.ClassID.ToUpper();
                    classnum.Type = 1;
                    hashRedis.Set<IBS_ClassOtherID>("IBS_ClassOtherID", ClassInfo.ClassNum.ToString() + "_" + 1, classnum);
                }

                //新增IBS_SchClassRelation关系
                if (ClassInfo.SchID > 0)
                {
                    var sch = hashRedis.Get<IBS_SchClassRelation>("IBS_SchClassRelation", ClassInfo.SchID.ToString());
                    if (sch != null)
                    {
                        var cl = sch.SchClassList.Where(a => a.ClassID.ToUpper() == ClassInfo.ClassID.ToUpper()).FirstOrDefault();
                        if (cl == null)
                        {
                            sch.SchClassList.Add(new SchClassS()
                            {
                                ClassID = ClassInfo.ClassID.ToUpper(),
                                ClassName = ClassInfo.ClassName,
                                GradeID = ClassInfo.GradeID,
                                GradeName = ClassInfo.GradeName
                            });
                        }
                        hashRedis.Set<IBS_SchClassRelation>("IBS_SchClassRelation", ClassInfo.SchID.ToString(), sch);
                    }
                    else
                    {
                        var modsc = BuildSchoolInfoBySchoolId(ClassInfo.SchID);
                        if (modsc != null)
                        {
                            SchClassS cc = new SchClassS();
                            cc.ClassID = ClassInfo.ClassID.ToUpper();
                            cc.ClassName = ClassInfo.ClassName;
                            cc.GradeID = ClassInfo.GradeID;
                            cc.GradeName = ClassInfo.GradeName;
                            modsc.SchClassList.Add(cc);
                            hashRedis.Set<IBS_SchClassRelation>("IBS_SchClassRelation", classInfo.SchID.ToString(), modsc);
                        }

                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(LoggerType.ServiceExceptionLog, "班级Update接口异常，ClassInfo=" + ClassInfo.ToJson(), ex);
                //新增到异常处理消息队列
                MOD2IBS_PushDataException excep = new MOD2IBS_PushDataException();
                excep.DataType = 2;
                excep.ChangeType = 1;
                excep.Json = ClassInfo.ToJson();
                excep.ErrorMessage = "班级Update接口异常！" + ex.Message;
                listRedis.LPush("MOD2IBS_PushDataException", excep.ToJson());

                result = false;

            }
            return result;
        }

        public bool Delete(IBS_ClassUserRelation ClassInfo)
        {
            var result = false;
            try
            {
                var classInfo = hashRedis.Get<IBS_ClassUserRelation>("IBS_ClassUserRelation", ClassInfo.ClassID.ToUpper());
                if (classInfo != null)
                {
                    //移除IBS_ClassOtherID数据
                    hashRedis.Remove("IBS_ClassOtherID", classInfo.ClassNum.ToString() + "_" + 1);

                    //移除IBS_SchClassRelation班级数据
                    var sch = hashRedis.Get<IBS_SchClassRelation>("IBS_SchClassRelation", classInfo.SchID.ToString());
                    if (sch != null)
                    {
                        sch.SchClassList.RemoveAll(a => a.ClassID.ToUpper() == ClassInfo.ClassID.ToUpper());
                        hashRedis.Set<IBS_SchClassRelation>("IBS_SchClassRelation", classInfo.SchID.ToString(), sch);
                    }
                }
                //更新IBS_UserInfo信息
                classInfo.ClassStuList.ForEach(a =>
                {
                    var user = hashRedis.Get<IBS_UserInfo>("IBS_UserInfo", a.StuID.ToString());
                    if (user != null)
                    {
                        user.ClassSchList.RemoveAll(x => x.ClassID.ToUpper() == classInfo.ClassID.ToUpper());
                        hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", a.StuID.ToString(), user);
                    }

                });
                classInfo.ClassTchList.ForEach(a =>
                {
                    var user = hashRedis.Get<IBS_UserInfo>("IBS_UserInfo", a.TchID.ToString());
                    if (user != null)
                    {
                        user.ClassSchList.RemoveAll(x => x.ClassID.ToUpper() == classInfo.ClassID.ToUpper());
                        hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", a.TchID.ToString(), user);
                    }

                });

                //移除IBS_ClassUserRelation班级信息
                hashRedis.Remove("IBS_ClassUserRelation", ClassInfo.ClassID.ToUpper());
                result = true;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(LoggerType.ServiceExceptionLog, "班级DEL接口异常，ClassID=" + ClassInfo.ToJson(), ex);
                //新增到异常处理消息队列
                MOD2IBS_PushDataException excep = new MOD2IBS_PushDataException();
                excep.DataType = 2;
                excep.ChangeType = 3;
                excep.Json = ClassInfo.ToJson();
                excep.ErrorMessage = "班级DEL接口异常！" + ex.Message;
                listRedis.LPush("MOD2IBS_PushDataException", excep.ToJson());

                result = false;

            }
            return result;
        }
    }
}
