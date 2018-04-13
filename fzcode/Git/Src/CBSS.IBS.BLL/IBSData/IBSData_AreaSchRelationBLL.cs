
using CBSS.Core.Utility;
using CBSS.Framework.Redis;
using CBSS.IBS.Contract;
using CBSS.IBS.IBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.IBS.BLL
{
    public partial class IBSService : CommonBLL, IIBSService
    {

        public IBS_AreaSchRelation GetAreaSchRelationByAreaId(int AreaId)
        {
            var areaInfo = hashRedis.Get<IBS_AreaSchRelation>("IBS_AreaSchRelation", AreaId.ToString());
            if (areaInfo == null)
            {
                areaInfo = BuildAreaInfoByAreaId(AreaId);
                if (areaInfo != null)
                {
                    hashRedis.Set<IBS_AreaSchRelation>("IBS_AreaSchRelation", AreaId.ToString(), areaInfo);
                }
            }
            return areaInfo;
        }

        public List<MOD_AreaDataModel> GetAreaInfo(int areaID)
        {
            List<MOD_AreaDataModel> modAreaSch = new List<MOD_AreaDataModel>();
            string modAreaSchJson = metadataService.GetAreaData(areaID);
            if (!string.IsNullOrEmpty(modAreaSchJson) && modAreaSchJson.Split('|')[0] != "错误")
            {
                modAreaSch = modAreaSchJson.FromJson<List<MOD_AreaDataModel>>();

            }
            return modAreaSch;
        }

        #region 增删改
        public bool Add(IBS_AreaSchRelation areaInfo)
        {
            throw new NotImplementedException();
        }

        public bool Update(IBS_AreaSchRelation areaInfo)
        {
            throw new NotImplementedException();
        }


        #endregion

    }
}
