using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenPlatform.Domain.DTO;

namespace OpenPlatform.Domain.Repositories
{
   public interface IFlowMonitorRepository
   {
       int AddApiInvokeLog(ApiInvokeLogDto apiInvokeLog);
       FlowAccessDto GetInvokeLog(int companyId, string invokedDate, int apiType, int productTypeCode);
       FlowAccessDto GetFlowControlConfig(int companyId, int apiType, int productTypeCode);
   }
}
