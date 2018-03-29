using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenPlatform.Application.Interfaces;
using OpenPlatform.Domain.Repositories;

namespace OpenPlatform.Application.Services
{
   public class FlowMonitorService : IFlowMonitorService
   {
       private readonly IFlowMonitorRepository _flowMonitorRepository;

       public FlowMonitorService(IFlowMonitorRepository flowMonitorRepository)
       {
           this._flowMonitorRepository = flowMonitorRepository;
       }

       public int AddApiInvokeLog(Domain.DTO.ApiInvokeLogDto apiInvokeLog)
       {
           return _flowMonitorRepository.AddApiInvokeLog(apiInvokeLog);
       }

       public Domain.DTO.FlowAccessDto GetInvokeLog(int companyId, string invokedDate, int apiType, int productTypeCode)
       {
           return _flowMonitorRepository.GetInvokeLog(companyId, invokedDate, apiType, productTypeCode);
       }

       public Domain.DTO.FlowAccessDto GetFlowControlConfig(int companyId, int apiType, int productTypeCode)
       {
           return _flowMonitorRepository.GetFlowControlConfig(companyId, apiType, productTypeCode);
       }
   }
}
