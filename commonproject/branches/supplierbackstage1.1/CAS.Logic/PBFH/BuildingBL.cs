using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity;
using CAS.DataAccess.DA.PBFH;
using CAS.Common;

namespace CAS.Logic.PBFH
{
    public class BuildingBL
    {
        /// <summary>
        /// 获取楼栋下拉列表  
        /// </summary>
        public static List<DATBuilding> GetBuildingDropDownList(SearchBase search,int projectid)
        {
            return BuildingDA.GetBuildingDropDownList(search,projectid);            
        }
    }
}
