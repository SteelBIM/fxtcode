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
    public class MOD2IBS_SchAreaRelationBLL : IIBS_MOD_SchAreaRelationBLL
    {
        static RedisSortedSetHelper sortedSetRedis = new RedisSortedSetHelper();
        static RedisHashHelper hashRedis = new RedisHashHelper();
        //消息队列
        static RedisListHelper listRedis = new RedisListHelper();

        RelationService.IRelationService relationservice = new RelationService.RelationServiceClient();
        FZUUMS_Relation2.FZUUMS_Relation2SoapClient relation2Client = new FZUUMS_Relation2.FZUUMS_Relation2SoapClient();
        FZUUMS_UserService.FZUUMS_UserServiceSoapClient userService = new FZUUMS_UserService.FZUUMS_UserServiceSoapClient();
        MetadataService.ServiceSoapClient metadataService = new MetadataService.ServiceSoapClient();


        public bool Change(int SchID)
        {
            var result = false;
            try 
            {
                var oldSch = hashRedis.Get<IBS_SchClassRelation>("IBS_SchClassRelation", SchID.ToString());
                var newSch = BuildSchoolInfoBySchoolId(SchID);

                //旧区域数据移除传入学校
                var oldArea = hashRedis.Get<IBS_AreaSchRelation>("IBS_AreaSchRelation", oldSch.AreaID.ToString());
                oldArea.AreaSchList.Remove(oldArea.AreaSchList.Where(a => a.SchD == SchID).First());
                hashRedis.Set<IBS_AreaSchRelation>("IBS_AreaSchRelation", oldSch.AreaID.ToString(), oldArea);

                //新区域数据新增学校
                var newArea = hashRedis.Get<IBS_AreaSchRelation>("IBS_AreaSchRelation", newSch.AreaID.ToString());
                AreaSchS areaSch = new AreaSchS();
                areaSch.SchD = newSch.SchID;
                areaSch.SchName = newSch.SchName;
                newArea.AreaSchList.Add(areaSch);
                hashRedis.Set<IBS_AreaSchRelation>("IBS_AreaSchRelation", newSch.AreaID.ToString(), newArea);

                //更新学校信息
                hashRedis.Set<IBS_SchClassRelation>("IBS_SchClassRelation", SchID.ToString(), newSch);
                result = true;
            }catch(Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "学校区域变动Change接口异常，SchID=" + SchID);
                //新增到异常处理消息队列
                MOD2IBS_PushDataException excep = new MOD2IBS_PushDataException();
                excep.Type = "";
                excep.Json = SchID.ToString();
                excep.ErrorMessage = "学校DEL学校区域变动Change接口异常,SchID=" + SchID;
                listRedis.LPush("MOD2IBS_PushDataException", excep.ToJson());

                result=false;
            }
           
            return result;
        }

        /// <summary>
        /// 构建MOD学校信息
        /// </summary>
        /// <param name="schId"></param>
        /// <returns></returns>
        public IBS_SchClassRelation BuildSchoolInfoBySchoolId(int schId)
        {
            IBS_SchClassRelation schClass = new IBS_SchClassRelation();
            //获取学校信息
            var modSchInfo = metadataService.GetSchoolInfo(schId);
            List<SchClassS> schClassList = new List<SchClassS>();
            //获取学校班级列表
            var modClassList = relation2Client.GetSchoolClass(schId);
            if (modClassList != null)
            {
                var classList = modClassList.ToList();
                classList.ForEach(a =>
                {
                    SchClassS sc = new SchClassS();
                    sc.ClassID = a.ID.ToString();
                    sc.ClassName = a.ClassName;
                    sc.GradeID = (int)a.GradeID;
                    sc.GradeName =a.GradeID.GetGradeName();
                    schClassList.Add(sc);
                });
            }
            MOD_SchoolInfoModel schoolInfo = JsonHelper.DecodeJson<MOD_SchoolInfoModel>(modSchInfo);
            schClass.SchID = schId;
            schClass.SchName = schoolInfo.SchoolName;
            schClass.AreaID = Convert.ToInt32(schoolInfo.DistrictID);
            schClass.SchClassList = schClassList;

            //Redis操作

            return schClass;

        }
    }
}
