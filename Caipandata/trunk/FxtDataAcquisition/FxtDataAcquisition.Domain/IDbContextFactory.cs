using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.Domain
{
    /// <summary>
    /// 上下文工厂
    /// </summary>
    public interface IDbContextFactory
    {
        /// <summary>
        /// 获取上下文
        /// </summary>
        /// <returns></returns>
        DbContext GetDbContext();
    }
}
