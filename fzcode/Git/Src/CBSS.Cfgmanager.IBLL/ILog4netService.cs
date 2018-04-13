using CBSS.Cfgmanager.Contract.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Cfgmanager.IBLL
{
    public interface ILog4netService
    {
        IEnumerable<Log4net> GetLog4net(out int totalcount, Log4netRequest request = null);
        /// <summary>
        /// 删除日志信息
        /// </summary>
        /// <param name="ids"></param>
        bool DeleteLog4net(List<int> ids);
        Log4net GetLog4net(int Id);
    }
}
