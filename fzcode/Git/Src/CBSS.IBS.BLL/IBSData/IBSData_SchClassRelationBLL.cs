using CBSS.Framework.Redis;
using CBSS.IBS.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBSS.Core.Utility;
using CBSS.IBS.IBLL;

namespace CBSS.IBS.BLL
{
    public partial class IBSService : CommonBLL,IIBSService
    {

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
            if (schInfo.AreaID > 0)
            {
                var modAreaSchJson = metadataService.GetAreaInfo(schInfo.AreaID);
                if (!string.IsNullOrEmpty(modAreaSchJson) && modAreaSchJson.Split('|')[0] != "错误")
                {
                    MOD_AreaInfoModel modAreaSch = modAreaSchJson.FromJson<MOD_AreaInfoModel>();
                    result.AreaName = modAreaSch.CodeName;
                }
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
        
        #endregion


        TBX_SchClassRelation IIBSData_SchClassRelationBLL.GetSchClassRelationBySchlId(int SchlId)
        {
            throw new NotImplementedException();
        }
    }
}
