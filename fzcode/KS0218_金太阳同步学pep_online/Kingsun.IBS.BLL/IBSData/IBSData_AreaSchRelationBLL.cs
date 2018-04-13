
using Kingsun.IBS.IBLL;
using Kingsun.IBS.Model;
using Kingsun.IBS.Model.MOD;
using Kingsun.SynchronousStudy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.IBS.BLL
{
    public class IBSData_AreaSchRelationBLL : CommonBLL,IIBSData_AreaSchRelationBLL
    {
        static RedisHashOtherHelper hashRedis = new RedisHashOtherHelper();
    
        
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

        #region 增删改
        public bool Add(IBS_AreaSchRelation areaInfo)
        {
            throw new NotImplementedException();
        }

        public bool Update(IBS_AreaSchRelation areaInfo)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int AreaID)
        {
            throw new NotImplementedException();
        }


        #endregion
       
    }
}
