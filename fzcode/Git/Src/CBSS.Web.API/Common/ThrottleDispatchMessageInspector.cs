using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Dispatcher;
using System.Web;
using System.Runtime.Caching;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.IO;
using System.Xml;
using System.Runtime.Serialization.Json;

namespace CBSS.Web.API.Common
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ThrottleDispatchMessageInspector”的 XML 注释
    public class ThrottleDispatchMessageInspector : IDispatchMessageInspector
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ThrottleDispatchMessageInspector”的 XML 注释
    {
        //TODO 这两个参数根据系统的配置处理方式存储，作为示例就直接写了 
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ThrottleDispatchMessageInspector.throttleNum”的 XML 注释
        public static int throttleNum = 10; // 限流个数
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ThrottleDispatchMessageInspector.throttleNum”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ThrottleDispatchMessageInspector.throttleUnit”的 XML 注释
        public static int throttleUnit = 4; // s
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ThrottleDispatchMessageInspector.throttleUnit”的 XML 注释

        CacheItemPolicy policy = new CacheItemPolicy();  //！ 过期策略，保证第一个set和之后set的绝对过期时间保持一致

        #region implement IDispatchMessageInspector

        // 此方法的返回值 将作为方法BeforeSendReply的第二个参数 object correlationState传入
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ThrottleDispatchMessageInspector.AfterReceiveRequest(ref Message, IClientChannel, InstanceContext)”的 XML 注释
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ThrottleDispatchMessageInspector.AfterReceiveRequest(ref Message, IClientChannel, InstanceContext)”的 XML 注释
        {

            string s = request.Headers.ToString();
            //string s1 = request.GetBody();
            string s2 = request.Properties.Values.ToString();
            string s3 = channel.InputSession.ToString();
            //string s4 = channel.LocalAddress.Uri;
            //string s5 = channel.RemoteAddress.Uri;
            //string s6 = channel.Via;
            //string s7 = instanceContext.Host.Description;
            
            // 获取ContractName和OperationName 用来作为缓存键
            var context = OperationContext.Current;
            string contractName = context.EndpointDispatcher.ContractName;
            string operationName = string.Empty;
            if (context.IncomingMessageHeaders.Action == null)
            {
                operationName = request.Properties.Values.LastOrDefault().ToString();
            }
            else
            {
                if (context.IncomingMessageHeaders.Action.Contains("/"))
                {
                    operationName = context.IncomingMessageHeaders.Action.Split('/').LastOrDefault();
                }
            }
            string throttleCacheKey = contractName + "_" + operationName;
            // 缓存当前请求频率， 以内存缓存System.Runtime.Caching.MemoryCache为例(.net4.0+)
            ObjectCache cache = MemoryCache.Default;
            var requestCount = cache.Get(throttleCacheKey);
            int currRequestCount = 1;
            if (requestCount != null && int.TryParse(requestCount.ToString(), out currRequestCount))
            {
                // 访问次数+1
                currRequestCount++;
                cache.Set(throttleCacheKey, currRequestCount, policy);  //必须保证过期策略和第一次set的时候一致，不然过期时间会有问题
            }
            else
            {
                policy.AbsoluteExpiration = DateTime.Now.AddSeconds(throttleUnit);
                cache.Set(throttleCacheKey, currRequestCount, policy);
            }

            // 如果当前请求数大于阀值，直接关闭
            if (currRequestCount > throttleNum)
            {
                request.Close();
            }

            //作为返回值 传给BeforeSendReply 
            LogVO log = new LogVO
            {
                BeginTime = DateTime.Now,
                ContractName = contractName,
                OperationName = operationName,
                Request = this.MessageToString(ref request),
                Response = string.Empty
            };
            return log;
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ThrottleDispatchMessageInspector.BeforeSendReply(ref Message, object)”的 XML 注释
        public void BeforeSendReply(ref Message reply, object correlationState)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ThrottleDispatchMessageInspector.BeforeSendReply(ref Message, object)”的 XML 注释
        {

            // 补充AfterReceiveRequest 传递过来的日志实体的属性, 记录
            LogVO log = correlationState as LogVO;
            log.EndTime = DateTime.Now;
            log.Response = this.MessageToString(ref reply);
            log.Duration = (log.EndTime - log.BeginTime).TotalMilliseconds;

            //attention 为不影响接口性能，日志实体push进队列（redis .etc），然后慢慢落地
            //TODO 这里直接写文本啦~
            try
            {
                string logPath = "D:\\WcfLog.txt";
                if (!File.Exists(logPath))
                {
                    File.Create(logPath);
                }
                StreamWriter writer = new StreamWriter(logPath, true);
                writer.Write(string.Format("at {0} , {1} is called , duration: {2} \r\n", log.BeginTime, log.ContractName + "." + log.OperationName, log.Duration));
                writer.Close();
            }
#pragma warning disable CS0168 // 声明了变量“ex”，但从未使用过
            catch (Exception ex) { }
#pragma warning restore CS0168 // 声明了变量“ex”，但从未使用过
        }
        #endregion

        #region method:  serialize message to string according to MessageBodyFormat set
        private string MessageToString(ref System.ServiceModel.Channels.Message message)
        {
            if (message.IsEmpty)
                return "no message";
            WebContentFormat messageFormat = this.GetMessageContentFormat(message);
            MemoryStream ms = new MemoryStream();
            //XmlDictionaryWriter writer = null;
            //switch (messageFormat)
            //{
            //    case WebContentFormat.Default:
            //    case WebContentFormat.Xml:
            //        writer = XmlDictionaryWriter.CreateTextWriter(ms);
            //        break;
            //    case WebContentFormat.Json:
            //        writer = JsonReaderWriterFactory.CreateJsonWriter(ms);
            //        break;
            //    case WebContentFormat.Raw:
            //        // special case for raw, easier implemented separately 
            //        return this.ReadRawBody(ref message);
            //}

            //message.WriteMessage(writer);
            //writer.Flush();
            //string messageBody = Encoding.UTF8.GetString(ms.ToArray());

            //// Here would be a good place to change the message body, if so desired. 

            //// now that the message was read, it needs to be recreated. 
            //ms.Position = 0;

            //// if the message body was modified, needs to reencode it, as show below 
            //// ms = new MemoryStream(Encoding.UTF8.GetBytes(messageBody)); 

            //XmlDictionaryReader reader;
            //if (messageFormat == WebContentFormat.Json)
            //{
            //    reader = JsonReaderWriterFactory.CreateJsonReader(ms, XmlDictionaryReaderQuotas.Max);
            //}
            //else
            //{
            //    reader = XmlDictionaryReader.CreateTextReader(ms, XmlDictionaryReaderQuotas.Max);
            //}

            //System.ServiceModel.Channels.Message newMessage = System.ServiceModel.Channels.Message.CreateMessage(reader, int.MaxValue, message.Version);
            //newMessage.Properties.CopyProperties(message.Properties);
            //message = newMessage;

            return "";//messageBody;
        }


        //assemble: System.ServiceModel.Web
        private WebContentFormat GetMessageContentFormat(System.ServiceModel.Channels.Message message)
        {
            WebContentFormat format = WebContentFormat.Default;
            if (message.Properties.ContainsKey(WebBodyFormatMessageProperty.Name))
            {
                WebBodyFormatMessageProperty bodyFormat;
                bodyFormat = (WebBodyFormatMessageProperty)message.Properties[WebBodyFormatMessageProperty.Name];
                format = bodyFormat.Format;
            }

            return format;
        }

        private string ReadRawBody(ref System.ServiceModel.Channels.Message message)
        {
            ////assemble: System.Runtime.Serialization 
            //XmlDictionaryReader bodyReader = message.GetReaderAtBodyContents();
            //bodyReader.ReadStartElement("Binary");
            //byte[] bodyBytes = bodyReader.ReadContentAsBase64();
            //string messageBody = Encoding.UTF8.GetString(bodyBytes);

            //// Now to recreate the message 
            //MemoryStream ms = new MemoryStream();
            //XmlDictionaryWriter writer = XmlDictionaryWriter.CreateBinaryWriter(ms);
            //writer.WriteStartElement("Binary");
            //writer.WriteBase64(bodyBytes, 0, bodyBytes.Length);
            //writer.WriteEndElement();
            //writer.Flush();
            //ms.Position = 0;
            //XmlDictionaryReader reader = XmlDictionaryReader.CreateBinaryReader(ms, XmlDictionaryReaderQuotas.Max);
            //System.ServiceModel.Channels.Message newMessage = System.ServiceModel.Channels.Message.CreateMessage(reader, int.MaxValue, message.Version);
            //newMessage.Properties.CopyProperties(message.Properties);
            //message = newMessage;

            return "";//messageBody;
        }
        #endregion
    }
}