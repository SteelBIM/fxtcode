using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Messaging;
using System.Web;
using Newtonsoft.Json;

namespace CBSS.Framework.Contract
{
    /// <summary>
    /// 服务调用上下文，主要传输IP，操作人，认证等数据，可用于WCF传输用
    /// </summary>
    [Serializable]
    public class WCFContext:Dictionary<string,object>
    {
        private const string CallContextKey = "__CallContext";    
        internal const string ContextHeaderLocalName = "__CallContext";
        internal const string ContextHeaderNamespace = "urn:CBSS.com";

        private void EnsureSerializable(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            if (!value.GetType().IsSerializable)
            {
                throw new ArgumentException(string.Format("The argument of the type \"{0}\" is not serializable!", value.GetType().FullName));
            }
        }       

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“WCFContext.this[string]”的 XML 注释
        public new  object this[string key]
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“WCFContext.this[string]”的 XML 注释
        {
            get
            {
                return base[key];
            }
            set
            {
                this.EnsureSerializable(value);
                base[key] = value;
            }
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“WCFContext.Operater”的 XML 注释
        public Operater Operater
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“WCFContext.Operater”的 XML 注释
        {
            get
            {
                return JsonConvert.DeserializeObject<Operater>(this["__Operater"].ToString());
            }
            set
            {
                this["__Operater"] = JsonConvert.SerializeObject(value);
            }
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“WCFContext.Current”的 XML 注释
        public static WCFContext Current
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“WCFContext.Current”的 XML 注释
        {
            get
            {
                if (CallContext.GetData(CallContextKey) == null)
                { 
                    CallContext.SetData(CallContextKey, new WCFContext());
                }

                return CallContext.GetData(CallContextKey) as WCFContext;
            }
            set
            {
                CallContext.SetData(CallContextKey, value);
            }
        }     
    }
}
