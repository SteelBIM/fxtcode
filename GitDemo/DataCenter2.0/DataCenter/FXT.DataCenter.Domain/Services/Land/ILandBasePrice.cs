using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;

namespace FXT.DataCenter.Domain.Services
{
    public interface IDAT_Land_BasePrice  
    {
         /// <summary>
        /// 添加一条基准地价记录
        /// 2014-04-04 
        /// 刘晓博
        /// </summary>
        /// <param name="modal">model</param>
        /// <returns></returns>
        int AddLand_BasePrice(FXT.DataCenter.Domain.Models.DAT_Land_BasePrice modal);

         /// <summary>
        /// 更新
        /// 更新一条基准地价记录
        ///  2014-04-04 
        ///  刘晓博
        /// </summary>
        /// <param name="modal">model</param>
        /// <param name="id">Id</param>
        /// <returns></returns>
        int UpdateLand_BasePrice(FXT.DataCenter.Domain.Models.DAT_Land_BasePrice modal);


        /// <summary>
        /// 删除
        /// 删除一条基准地价记录
        /// 2014-04-04 
        /// 刘晓博
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        bool DeleteLand_BasePrice(int Id);

        /// <summary>
        /// 查询基准地价信息
        /// </summary>
        /// <returns></returns>
        IQueryable<DAT_Land_BasePrice> GetLand_BasePrice(DAT_Land_BasePrice mode, int pageIndex, int pageSize, out int totalCount, bool self);

        /// <summary>
        /// 根据Id得到土地基准地价
        /// </summary>
        /// <param name="Id">ID</param>
        /// <param name="cityId">cityId</param>
        /// <param name="fxtCompanyid">fxtCompanyid</param>
        /// <returns></returns>
        DAT_Land_BasePrice GetAllLand_BasePriceByLandId(int landId, int cityId, int fxtCompanyid);

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="model"></param>
        /// <param name="self"></param>
        /// <returns></returns>
        IQueryable<DAT_Land_BasePrice> GetLand_BasePriceExeclImport(DAT_Land_BasePrice model, bool self);
    }
}
