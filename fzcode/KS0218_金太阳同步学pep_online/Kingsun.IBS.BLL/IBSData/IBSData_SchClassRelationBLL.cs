using Kingsun.IBS.IBLL;
using Kingsun.IBS.Model;
using Kingsun.IBS.Model.MOD;
using Kingsun.IBS.Model.TBX;
using Kingsun.SynchronousStudy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.IBS.BLL
{
    public class IBSData_SchClassRelationBLL : CommonBLL,IIBSData_SchClassRelationBLL
    {
        static RedisHashOtherHelper hashRedis = new RedisHashOtherHelper();
        static MetadataService.ServiceSoapClient metadataService = new MetadataService.ServiceSoapClient();

        static IIBSData_AreaSchRelationBLL areaBLL = new IBSData_AreaSchRelationBLL();

        /// <summary>
        /// 获取学校班级关系
        /// </summary>
        /// <param name="schId">用户ID</param>
        /// <returns></returns>
        public IBS_SchClassRelation GetSchClassRelationBySchlId(int schId)
        {
            var schInfo = hashRedis.Get<IBS_SchClassRelation>("IBS_SchClassRelation", schId.ToString());
            if (schInfo == null)
            {
                schInfo = BuildSchoolInfoBySchoolId(schId);
                if (schInfo != null) 
                {
                    hashRedis.Set<IBS_SchClassRelation>("IBS_SchClassRelation", schId.ToString(), schInfo);
                }
            }
            return schInfo;
        }

        /// <summary>
        /// 获取学校班级关系+区域名字
        /// </summary>
        /// <param name="schId">用户ID</param>
        /// <returns></returns>
        public TBX_SchClassRelation GetSchoolALLInfoBySchoolId(int schId)
        {
            TBX_SchClassRelation result=new TBX_SchClassRelation();
            var schInfo = GetSchClassRelationBySchlId(schId);
            if (schInfo != null)
            {
                result.iBS_SchClassRelation = schInfo;
                if (schInfo.AreaID > 0)
                {
                    var areainfo = areaBLL.GetAreaSchRelationByAreaId(schInfo.AreaID);
                    if (areainfo != null)
                    {
                        result.AreaName = areainfo.AreaName;
                        result.iBS_SchClassRelation = schInfo;
                    }
                }

            }
            else
            {
                return null;
            }

            return result;
        }
        #region 增删改
        public bool Add(TBX_SchClassRelation SchInfo)
        {
            throw new NotImplementedException();
        }

        public bool Update(TBX_SchClassRelation SchInfo)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int SchId)
        {
            throw new NotImplementedException();
        }
        #endregion


    }
}
