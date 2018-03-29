using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using Ninject;
using OpenPlatform.Application.Interfaces;
using OpenPlatform.Domain.DTO;
using OpenPlatform.Framework.IoC;
using OpenPlatform.Framework.Utils;

namespace OpenPlatform.Api.Controllers
{
    [RoutePrefix("api/flowmonitor")]
    public class FlowMonitorController : ApiController
    {
        private readonly IFlowMonitorService _service;

        public FlowMonitorController()
        {
            this._service = new StandardKernel(new FlowMonitorServiceBinder()).Get<IFlowMonitorService>();
        }

        public IHttpActionResult GetInvokeLog(int companyId, string invokedDate, int apiType, int productTypeCode)
        {
            return Ok(_service.GetInvokeLog(companyId, invokedDate, apiType, productTypeCode));
        }

        public IHttpActionResult GetFlowControlConfig(int companyId, int apiType, int productTypeCode)
        {
            return Ok(_service.GetFlowControlConfig(companyId, apiType, productTypeCode));
        }

        public int PostApiInvokeLog([FromBody]ApiInvokeLogDto jsonApiLog)
        {
            if (string.IsNullOrEmpty(jsonApiLog.Ip))
            {
                jsonApiLog.Ip = NetHelper.GetRequestClientAddress();
            }
            return _service.AddApiInvokeLog(jsonApiLog);
        }

    }
}
