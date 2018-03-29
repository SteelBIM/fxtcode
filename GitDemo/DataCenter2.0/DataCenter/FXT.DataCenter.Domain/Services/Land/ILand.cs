using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;
using System.Data;

namespace FXT.DataCenter.Domain.Services
{
    public interface IDAT_Land
    {
        /// <summary>
        /// 新增土地信息
        /// 2014-04-04 
        /// 刘晓博</summary>
        /// <param name="modal">土地模型</param>
        /// <returns></returns>
        int AddDAT_Land(DAT_Land modal);

        /// <summary>
        /// 根据土地信息Id更新土地信息
        /// 2014-04-04 
        /// 刘晓博
        /// </summary>
        /// <param name="modal">土地模型</param>
        /// <param name="landId">土地Id</param>
        /// <param name="fxtcompanyId">fxtcompanyId</param>
        /// <returns></returns>
        int UpdateDAT_Land(DAT_Land modal, int currFxtcompanyId);

        /// <summary>
        /// 根据土地信息Id删除土地数据
        /// 2014-04-04 
        /// 刘晓博
        /// </summary>
        /// <param name="landId"></param>
        /// <returns></returns>
        bool DeleteDAT_Land(int landId);
        /// <summary>
        /// 获取所有的土地信息
        /// 2014-04-014
        /// 刘晓博
        /// </summary>
        /// <param name="landName"></param>
        /// <returns></returns>
        IQueryable<DAT_Land> GetAllLandInfo(DAT_Land mode,int pageIndex,int pageSize,out int totalCount, bool self);
        /// <summary>
        /// 查询土地
        /// </summary>
        /// <param name="landName">土地Id</param>
        /// <param name="cityId">城市Id</param>
        /// <param name="companyId">评估机构Id</param>
        /// <returns></returns>
        DAT_Land GetAllLandByLandId(int landId, int cityId, int companyId);
        /// <summary>
        /// excel导入
        /// </summary>
        /// <param name="la"></param>
        /// <returns></returns>
        int AddExcelImport(DAT_Land la);


        /// <summary>
        /// 验证宗地号是否唯一
        /// </summary>
        /// <param name="citiId">城市Id</param>
        /// <param name="fxtcomId">公司ID</param>
        /// <param name="landNo">宗地号</param>
        /// <returns></returns>
        bool ValidLandNo(int citiId, int fxtcomId, string landNo);

        /// <summary>
        /// 根据关键字查询相关宗地号
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        IQueryable<string> GetLandNo(int cityId, int companyId);

        DAT_Land GetAllLandInfo(int fxtcompanyId, int cityId, int areaId,string landNo);

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="model"></param>
        /// <param name="self"></param>
        /// <returns></returns>
        IQueryable<DAT_Land> GetAllLandInfoImport(DAT_Land model, bool self);

        /// <summary>
        /// 更新土地信息
        /// </summary>
        /// <param name="modal"></param>
        /// <param name="currFxtcompanyId">当前操作者公司ID</param>
        /// <param name="modifiedProperty">要修改的属性</param>
        /// <returns></returns>
        int UpdateLandInfo4Excel(DAT_Land modal, int currFxtcompanyId, List<string> modifiedProperty);
    }
}
