using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtCenterService.DataAccess;
using System.Data;
using System.Collections;
using CAS.Common;
using CAS.Entity.FxtDataCenter;

namespace FxtCenterService.Logic
{
    public class DatProjectBizBL
    {
        public static List<Dictionary<string, object>> GetListBiz(SearchBase search, string strKey, int items)
        {
            List<Dictionary<string, object>> listResult = new List<Dictionary<string, object>>();
            
            DataTable dt = null;
            search.Top = items;
            dt = DatProjectBizDA.GetListBiz(search, strKey);
            listResult = JSONHelper.DataTableToList(dt);
            return listResult;
        }

        public static List<Dictionary<string, object>> GetBuildingListBiz(SearchBase search,int projectId, string strKey, int items)
        {
            List<Dictionary<string, object>> listResult = new List<Dictionary<string, object>>();
            DataTable dt = null;
            search.Top = items;
            dt = DatProjectBizDA.GetBuildingListBiz(search, projectId, strKey);
            listResult = JSONHelper.DataTableToList(dt);
            return listResult;
        }

        public static List<Dictionary<string, object>> GetFloorListBiz(SearchBase search, int buildingid, string strKey, int items)
        {
            List<Dictionary<string, object>> listResult = new List<Dictionary<string, object>>();
            DataTable dt = null;
            search.Top = items;
            dt = DatProjectBizDA.GetFloorListBiz(search, buildingid, strKey);
            listResult = JSONHelper.DataTableToList(dt);
            return listResult;
        }

        public static List<Dictionary<string, object>> GetHouseListBiz(SearchBase search, int floorId, string strKey, int items)
        {
            List<Dictionary<string, object>> listResult = new List<Dictionary<string, object>>();
            DataTable dt = null;
            search.Top = items;
            dt = DatProjectBizDA.GetHouseListBiz(search, floorId, strKey);
            listResult = JSONHelper.DataTableToList(dt);
            return listResult;
        }
    }
}
