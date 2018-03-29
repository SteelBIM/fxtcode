using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;
using CAS.Entity;

namespace FxtUserCenterService.Contract
{
     [ServiceContract]
    public interface IGeTui
    {
         [OperationContract]
         [WebInvoke(UriTemplate = "send"
             , Method = "POST"
             , RequestFormat = WebMessageFormat.Json
             , ResponseFormat = WebMessageFormat.Json
             , BodyStyle = WebMessageBodyStyle.WrappedRequest)]
         WCFJsonData GeTuiSend(string funinfo);
    }
}
