using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models.DTO;

using FXT.DataCenter.Domain.Models;

namespace FXT.DataCenter.Domain.Services
{
    public interface IBusinessCircle
    {
        /// <summary>
        /// 获取商圈信息
        /// </summary>
        /// <param name="subAreaBiz">查询条件</param>
        /// <param name="totalCount"></param>
        /// <param name="self">是否为查询自己，默认为查询自己</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IQueryable<SYS_SubArea_Biz> GetSubAreaBiz(SYS_SubArea_Biz subAreaBiz, int pageIndex, int pageSize, out int totalCount, bool self = true);

        ///// <summary>
        ///// 获取商圈id
        ///// </summary>
        ///// <param name="fxtCompanyId">评估机构ID</param>
        ///// <param name="name">商圈名称</param>
        ///// <param name="areaId">行政区ID</param>
        ///// <returns></returns>
        //int GetSubAreaId(int areaId, int fxtCompanyId, string name);

        /// <summary>
        /// 根据行政区ID获取其所属的商圈
        /// </summary>
        /// <param name="areaId">行政区ID</param>
        /// <returns></returns>
        IQueryable<SYS_SubArea_Biz> GetSubAreaBizByAreaId(int areaId, int cityId, int fxtcompanyId);

        /// <summary>
        /// 获取单条商圈信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SYS_SubArea_Biz GetSubAreaBizById(int id);

        /// <summary>
        /// 添加商圈信息
        /// </summary>
        /// <param name="subAreaBiz"></param>
        /// <returns></returns>
        int AddSubAreaBiz(SYS_SubArea_Biz subAreaBiz);

        /// <summary>
        /// 修改商圈信息
        /// </summary>
        /// <param name="subAreaBiz"></param>
        /// <returns></returns>
        int UpdateSubAreaBiz(SYS_SubArea_Biz subAreaBiz);

        /// <summary>
        /// 删除商圈信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int DeleteSubAreaBiz(int id);

        /// <summary>
        /// 插入商圈范围
        /// </summary>
        /// <returns></returns>
        int AddSubAreaBizCoordinate(SYS_SubArea_Biz_Coordinate subAreaBizCoordinate);

        /// <summary>
        /// 修改商圈范围
        /// </summary>
        /// <returns></returns>
        int UpdateSubAreaBizCoordinate(SYS_SubArea_Biz_Coordinate subAreaBizCoordinate);

        /// <summary>
        /// 查询商圈范围，返回记录条数
        /// </summary>
        /// <returns>记录条数</returns>
        int GetSubAreaBizCoordinate(int subAreaId, int areaId, int fxtCompanyId);

        /// <summary>
        /// 获取商圈统计信息
        /// </summary>
        /// <param name="areaId">行政区ID</param>
        /// <param name="fxtCompanyId">评估公司Id</param>
        /// <param name="cityId">城市Id</param>
        /// <returns></returns>
        IQueryable<SubAreaBizStatisticDTO> GetSubAreaBizStatistic(int areaId, int fxtCompanyId, int cityId);

        /// <summary>
        /// 验证商圈是否存在
        /// </summary>
        /// <param name="areaId">行政区ID</param>
        /// <param name="fxtCompanyId">评估机构ID</param>
        /// <param name="subAreaId">商圈ID（新增状态时，subAreaId=-1）</param>
        /// <param name="subAreaName">商圈名称</param>
        /// <returns></returns>
        bool IsExistSubAreaBiz(int areaId, int fxtCompanyId, int subAreaId, string subAreaName);

    }
}
