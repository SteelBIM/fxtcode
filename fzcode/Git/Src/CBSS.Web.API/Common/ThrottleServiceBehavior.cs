using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Web;

namespace CBSS.Web.API.Common
{
    // 应用自定义服务行为的2中方式：
    // 1. 继承Attribute作为特性 服务上打上标示
    // 2. 继承BehaviorExtensionElement, 然后修改配置文件
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ThrottleServiceBehaviorAttribute”的 XML 注释
    public class ThrottleServiceBehaviorAttribute : Attribute, IServiceBehavior
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ThrottleServiceBehaviorAttribute”的 XML 注释
    {
        #region implement IServiceBehavior
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ThrottleServiceBehaviorAttribute.AddBindingParameters(ServiceDescription, ServiceHostBase, Collection<ServiceEndpoint>, BindingParameterCollection)”的 XML 注释
        public void AddBindingParameters(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ThrottleServiceBehaviorAttribute.AddBindingParameters(ServiceDescription, ServiceHostBase, Collection<ServiceEndpoint>, BindingParameterCollection)”的 XML 注释
        {

        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ThrottleServiceBehaviorAttribute.ApplyDispatchBehavior(ServiceDescription, ServiceHostBase)”的 XML 注释
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ThrottleServiceBehaviorAttribute.ApplyDispatchBehavior(ServiceDescription, ServiceHostBase)”的 XML 注释
        {
            foreach (ChannelDispatcher channelDispather in serviceHostBase.ChannelDispatchers)
            {
                foreach (var endpoint in channelDispather.Endpoints)
                {
                    // holyshit DispatchRuntime 
                    endpoint.DispatchRuntime.MessageInspectors.Add(new ThrottleDispatchMessageInspector());
                }
            }
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ThrottleServiceBehaviorAttribute.Validate(ServiceDescription, ServiceHostBase)”的 XML 注释
        public void Validate(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ThrottleServiceBehaviorAttribute.Validate(ServiceDescription, ServiceHostBase)”的 XML 注释
        {

        }
        #endregion

        #region override BehaviorExtensionElement
        //public override Type BehaviorType
        //{
        //    get { return typeof(ThrottleServiceBehavior); }
        //}

        //protected override object CreateBehavior()
        //{
        //    return new ThrottleServiceBehavior();
        //}
        #endregion
    }
}