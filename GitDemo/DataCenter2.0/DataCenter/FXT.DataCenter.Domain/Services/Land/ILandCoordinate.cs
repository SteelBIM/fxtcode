using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;

namespace FXT.DataCenter.Domain.Services
{
    public interface ILandCoordinate 
    {
        /// <summary>
        /// 新增土地坐标
        /// </summary>
        /// <param name="modal"></param>
        /// <returns></returns>
        int AddLandCoordinate(DAT_Land_Coordinate modal);
       
        /// <summary>
        /// 更新坐标点
        /// </summary>
        /// <param name="modal"></param>
        /// <returns></returns>
        int UpdateLandCoordinate(DAT_Land_Coordinate modal);
        

        /// <summary>
        /// 删除坐标点
        /// </summary>
        /// <param name="fxtcompanyid">评估机构ID</param>
        /// <param name="cityId">城市ID</param>
        /// <param name="landId">土地ID</param>
        /// <returns></returns>
        int DeleteLandCoordinate(int fxtcompanyid, int cityId, long landId);
      
    }
}
