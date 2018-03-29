using FXT.VQ.DataService;
using FxtCenterServiceOpen.Actualize.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtCenterServiceOpen.Actualize.DataCenter
{
    public class CallDataCenter
    {
        #region 楼盘栋房 信息
        /// <summary>
        /// 楼盘信息
        /// </summary>
        /// <param name="cityid"></param>
        /// <param name="projectid"></param>
        /// <param name="returnData"></param>
        /// <returns></returns>
        public static TempProject GetProjectInfo(int cityid, int projectid
            , out JsonReturnData returnData, out string outJson)
        {
            List<TempProject> list = JsonDataHelper.JsonToList<TempProject>(
                CenterDataServices.GetProjectInfo(cityid, projectid, out outJson), out returnData
                );

            if (list != null && list.Count > 0)
                return list[0];
            else
                return null;
        }

        /// <summary>
        /// 楼栋信息
        /// </summary>
        /// <param name="cityid"></param>
        /// <param name="projectid"></param>
        /// <param name="returnData"></param>
        /// <returns></returns>
        public static TempProject GetBuildingInfo(int cityid, int houseid
            , out JsonReturnData returnData, out string outJson)
        {
            List<TempProject> list = JsonDataHelper.JsonToList<TempProject>(
                CenterDataServices.GetBuildingInfo(cityid, houseid, out outJson), out returnData
                );

            if (list != null && list.Count > 0)
                return list[0];
            else
                return null;
        }

        /// <summary>
        /// 房号信息
        /// </summary>
        /// <param name="cityid"></param>
        /// <param name="projectid"></param>
        /// <param name="returnData"></param>
        /// <returns></returns>
        public static TempProject GetHouseInfo(int cityid, int houseid
            , out JsonReturnData returnData, out string outJson)
        {
            List<TempProject> list = JsonDataHelper.JsonToList<TempProject>(
                CenterDataServices.GetHouseInfo(cityid, houseid, out outJson), out returnData
                );

            if (list != null && list.Count > 0)
                return list[0];
            else
                return null;
        }

        #endregion


    }
}
