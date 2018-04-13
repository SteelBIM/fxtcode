using KSWF.Core.Utility;
using KSWF.Framework.BLL;
using KSWF.WFM.Constract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Protocols;

namespace KSWF.Web.Admin.Service
{
    
    public class SoapCertficateAttribute : SoapExtensionAttribute
    {
        int _priority = 1;

        public override int Priority
        {
            get
            {
                return _priority;
            }
            set
            {
                _priority = value;
            }
        }

        public override Type ExtensionType
        {
            get { return typeof(MyExtension); }
        }
    }

    public class MyExtension : SoapExtension
    {
        Manage m = new Manage();
        public override object GetInitializer(Type serviceType)
        {
            return GetType();
        }

        public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute)
        {
            return null;
        }

        public override void Initialize(object initializer)
        {

        }

        //这个override的方法会被调用四次
        //分别是SoapMessageStage的BeforeSerialize,AfterSerialize,BeforeDeserialize,AfterDeserialize
        public override void ProcessMessage(SoapMessage message)
        {
            if (message.Stage == SoapMessageStage.AfterDeserialize) //反序列化后处理
            {
                bool check = false;
                foreach (SoapHeader header in message.Headers)
                {
                    if (header is CertficateSoapHeader)
                    {
                        CertficateSoapHeader myHeader = (CertficateSoapHeader)header;
                        if (myHeader.UserName == null || myHeader.PassWord == null)
                        {
                            break;
                        }
                        var pwd = PublicHelp.pswToSecurity(myHeader.PassWord);
                        List<com_master> master = m.SelectSearch<com_master>(x => (x.mastername == myHeader.UserName && x.password == pwd));
                        if (master != null && master.Count > 0)
                        {
                            check = true;
                            break;
                        }
                    }
                }
                if (!check)
                {
                    throw new SoapHeaderException("认证失败", SoapException.ClientFaultCode);
                }
            }
        }
    }



}
