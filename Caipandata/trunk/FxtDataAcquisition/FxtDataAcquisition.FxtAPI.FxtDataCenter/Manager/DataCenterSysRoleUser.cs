using FxtDataAcquisition.Common;
using FxtDataAcquisition.Domain.DTO.FxtDataCenterDTO;
using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtDataAcquisition.FxtAPI.FxtDataCenter.Manager
{
    public class DataCenterSysRoleUser
    {
        public static List<int> GetSysRoleUserIds(string userName,string signName, List<Apps> appList)
        {
            var para = new { username = userName};
            DataCenterResult result = Common.PostDataCenter(userName, signName, Common.getsysroleuserids, para, appList);
            List<int> list = new List<int>();
            if (!string.IsNullOrEmpty(result.data))
            {
                list = result.data.ParseJSONList<int>();
            }
            return list;
        }
    }
}
