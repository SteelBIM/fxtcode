using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http.Results;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using FM = OpenPlatform.Framework.FlowMonitor;

namespace OpenPlatform.FlowMonitorTest.ModuleTest
{
    [TestFixture]
    public class ControllersTest
    {
        [SetUp]
        public void TestInit()
        {
        }
        /// <summary>
        /// 测试接口请求是否超过流量上限
        /// </summary>
        [Test]
        [Category("Create")]
        public void TestFlowOverflow()
        {
            bool isOverflow = false;
            int fxtCompanyId = 13;
            FM.ApiType apiType = FM.ApiType.Project;
            int itemCount = -1;
            string functionName = "projectdropdownlistmcas";
            int productTypeCode = 1003038;
            string ip = "127.0.0.1";
            //request 2 times
            isOverflow = FM.Api.Flow.Overflow(fxtCompanyId, DateTime.Now, apiType, itemCount, ip,functionName: functionName, productTypeCode: productTypeCode);
            isOverflow = FM.Api.Flow.Overflow(fxtCompanyId, DateTime.Now, apiType, itemCount,ip, functionName: functionName, productTypeCode: productTypeCode);

            Assert.IsTrue(isOverflow);
        }
    }
}
