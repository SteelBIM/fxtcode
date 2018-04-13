using CBSS.Tbx.Contract.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract
{
    public interface IApiFunctionService
    {
        /// <summary>
        /// 保存接口信息
        /// </summary>
        /// <returns></returns>
        bool SaveApiFunction(Sys_ApiFunction apiFun, List<Sys_ApiFunctionParam> param);
    }
}
