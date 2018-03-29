using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Common;
using System.Data;

// Project - Building - Floor - House 联动功能 kevin 2013-3-21
namespace CAS.DataAccess.DA.PBFH
{
    public class FloorDA : Base
    {

        /// <summary>
        /// 获取单元或楼层列表
        /// </summary>
        /// <param name="buildingid">楼栋ID</param>
        /// <param name="files">字段 FloorNo/UnitNo </param>
        /// <returns></returns>
        public static List<string> GetFloorDropDownList(int buildingid, string files)
        {
            files = files == "floorno" ? files : "unitno";

            List<string> list = null;
            string sql = SQL.PBFH.FloorDropDownList;
            sql = sql.Replace("@dat_house", TableByCity.housetable);
            sql = sql.Replace("@cityid", WebCommon.LoginInfo.cityid.ToString());
            sql = sql.Replace("@buildingid", buildingid.ToString());
            sql = sql.Replace("@filed", files);

            DataSet ds = ExecuteDataSet(sql);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                list = new List<string>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    list.Add(ds.Tables[0].Rows[i][0].ToString());
                }
            }

            return list;
        }
    }
}
