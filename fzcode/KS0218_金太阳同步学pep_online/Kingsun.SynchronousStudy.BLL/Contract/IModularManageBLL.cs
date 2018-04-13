using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kingsun.SynchronousStudy.Models;


namespace Kingsun.SynchronousStudy.BLL.Contract
{
    /// <summary>
    /// IModularManageBLL 的摘要说明
    /// </summary>
    public interface IModularManageBLL
    {
        /// <summary>
        /// 获取模块列表
        /// </summary>
        /// <returns></returns>
        List<TB_ModularManage> GetModularList();

    }
}