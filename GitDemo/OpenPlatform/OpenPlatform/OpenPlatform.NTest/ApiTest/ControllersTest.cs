using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http.Results;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using OpenPlatform.Api.Controllers;
using OpenPlatform.Application.Interfaces;
using OpenPlatform.Domain.DTO;

namespace OpenPlatform.NTest.ApiTest
{
    [TestFixture]
    public class ControllersTest
    {
        private EvaluationDto[] _entrustAppraise;
        private EntrustObjectDto[] _entrustObject;
        private PropertyInfoDto[] _propertyInfo;

        [SetUp]
        public void TestInit()
        {
            _entrustAppraise = new[]
            {
                new EvaluationDto
                {
                    GjbEntrustId=1,
                    ApplicationStatus = "申请后",
                    AppraiseAgency = "天津中盛勃然",
                    Appraiser = "张成",
                    AppraiseStatus = "已完成",
                    BuyingType = "按揭"
                }
            };
            _entrustObject = new[]
            {
                new EntrustObjectDto
                {
                    GjbObjId =2,
                    Address = "罗湖区宝安南路与红桂路交汇处东南",
                    AreaName = "凤岗镇",
                    CityName = "深圳市",
                    LandCertificateDate = DateTime.Now,

                }

            };

            _propertyInfo = new[]
            {
                new PropertyInfoDto
                {
                    Contacts = "王杰",
                    ContractPhone = "13345268679",
                    HasChildren = "无",
                    IdNum = "441621198307231211",
                    MaritalStatus = "未婚",
                    PersonName = "钟远军",
                    Phone = "18664556666",
                    Relation = "兄弟"
                }
            };

            #region HttpResponseMessage(缺点：需要设置Request和Configuration到Controller,不然，抛ArgumentNullException或InvalidOperationException异常)

            //mock interface IEvaluationRepository
            //var evaRepository = new Mock<IEvaluationRepository>();
            // evaRepository.Setup(foo => foo.GetEvaluationById(It.IsAny<string>())).Returns(properties.AsQueryable());
            // evaRepository.Setup(foo => foo.GetPropertyBuyerByTranId(It.IsAny<int>())).Returns(propertyOwner.AsQueryable());
            // evaRepository.Setup(foo => foo.GetPathListByEaId(It.IsAny<int>())).Returns(picList.AsQueryable());

            //EvaluationService service = new EvaluationService(evaRepository.Object);
            //HttpRequestMessage request = new HttpRequestMessage(){
            //    RequestUri = new Uri("http://url/api/evaluation/"),
            //    Method = HttpMethod.Post,
            //    Content = new FormUrlEncodedContent(new Dictionary<string, string>() { })
            //};
            //request.SetConfiguration(new HttpConfiguration());
            //evCtrl = new EvaluationController(service, request);

            //HttpResponseMessage responseMsg = evCtrl.Get(propertyOwner[0].IdNum);
            //responseMsg.Content.ReadAsStringAsync().ContinueWith<string>(t => apiResult = t.Result);


            //阻塞一下线程，以便于同步执行用例。同步获取结果方式待研究
            //Console.WriteLine(apiResult);

            #endregion

        }
        /// <summary>
        /// 测试获取委估业务参数输入
        /// </summary>
        [Test]
        [Category("Retrive")]
        public void TestGetEntrustAppraiseById()
        {
            var result = new List<EvaluationDto>().AsQueryable();

            var evaluationServices = new Mock<IEvaluationService>();

            evaluationServices
                .Setup(m => m.GetEntrustAppraiseById(It.IsNotIn<string>(string.Empty, null)))
                .Returns(result);

            Assert.AreEqual(result, evaluationServices.Object.GetEntrustAppraiseById("test"));
        }
        /// <summary>
        /// 测试获取委估对象参数输入
        /// </summary>
        [Test]
        [Category("Retrive")]
        public void TestGetEntrustObjectById()
        {
            var result = new List<EntrustObjectDto>().AsQueryable();

            var evaluationServices = new Mock<IEvaluationService>();

            evaluationServices
              .Setup(m => m.GetEntrustObjectById(It.IsAny<long>()))
              .Returns(result);

            Assert.AreEqual(result, evaluationServices.Object.GetEntrustObjectById(0));
        }
        /// <summary>
        /// 测试获取照片信息参数输入
        /// </summary>
        [Test]
        [Category("Retrive")]
        public void TestGetPathListByEaId()
        {
            var result = new List<string>().AsQueryable();

            var evaluationServices = new Mock<IEvaluationService>();

            evaluationServices
                .Setup(m => m.GetPathListById(It.IsAny<long>()))
                .Returns(result);

            Assert.AreEqual(result, evaluationServices.Object.GetPathListById(-1));
        }
        /// <summary>
        /// 测试获取产权信息参数输入
        /// </summary>
        [Test]
        [Category("Retrive")]
        public void TestGetPropertyBuyerByTranId()
        {
            var result = new List<PropertyInfoDto>().AsQueryable();

            var evaluationServices = new Mock<IEvaluationService>();

            evaluationServices
               .Setup(m => m.GetPropertyBuyerById(It.IsInRange<long>(1, Int64.MaxValue, Range.Inclusive)))
               .Returns(result);

            Assert.AreEqual(result, evaluationServices.Object.GetPropertyBuyerById(1));
        }
        /// <summary>
        /// 测试添加委估业务对象
        /// </summary>
        [Test]
        [Category("Create")]
        public void AddEntrust_ShouldReturnValue()
        {
            var dto = new Evaluation4GjbDto { GjbEntrustId = 1 };

            var evaluationServices = new Mock<IEvaluationService>();

            evaluationServices
               .Setup(m => m.AddEntrust(dto))
               .Returns(1);

            Assert.AreEqual(1, evaluationServices.Object.AddEntrust(dto));
        }

        /// <summary>
        /// 测试结果中的日期格式是否正确
        /// </summary>
        [Test]
        [Category("Utils")]
        public void TestDatetimeStringFormat()
        {

            var evaluationServices = new Mock<IEvaluationService>();
            evaluationServices
                .Setup(m => m.GetEntrustAppraiseById(_propertyInfo[0].IdNum))
                .Returns(_entrustAppraise.AsQueryable());
            evaluationServices
                .Setup(m => m.GetEntrustObjectById(_entrustAppraise[0].GjbEntrustId))
                .Returns(_entrustObject.AsQueryable());
            evaluationServices
               .Setup(m => m.GetPropertyBuyerById(_entrustObject[0].GjbObjId))
               .Returns(_propertyInfo.AsQueryable());

            //var evCtrl = new EvaluationController(evaluationServices.Object);
            //var companyIds = new[] {25,365};
            //var contentResult = evCtrl.Get(_propertyInfo[0].IdNum, companyIds.ToList()) as OkNegotiatedContentResult<EvaluationApiDto>;
           

            //Assert.IsNotNull(contentResult);
            //Assert.IsNotNull(contentResult.Content);
            //Assert.IsNotNull(contentResult.Content.EstateInfo);

            ////var list = contentResult.Content.EstateInfo[0].EntrustAppraise;

            //var json = JsonConvert.SerializeObject(contentResult.Content.EstateInfo);
            //var reader = new JsonTextReader(new StringReader(json));

            //var landCertificateDate = string.Empty;
            //while (reader.Read())
            //{
            //    if (null != reader.Value && "LandCertificateDate" == reader.Value.ToString())
            //    {
            //        reader.Read();
            //        landCertificateDate = reader.Value.ToString();
            //        break;
            //    }
            //}

            //DateTime dtConvertResult;
            //Assert.IsTrue(DateTime.TryParse(landCertificateDate, out dtConvertResult));

            #region 旧代码

            //string propertyCertificateRegisteDate = null;
            ////var reader = new JsonTextReader(new StringReader(apiResult));
            //var reader = new JsonTextReader(new StringReader(""));
            //while (reader.Read())
            //{
            //    if (null != reader.Value && "PropertyCertificateRegisteDate" == reader.Value.ToString())
            //    {
            //        reader.Read();
            //        propertyCertificateRegisteDate = reader.Value.ToString();
            //        break;
            //    }
            //}
            //DateTime dtConvertResult;
            //Console.WriteLine(propertyCertificateRegisteDate);
            //Assert.IsTrue(DateTime.TryParse(propertyCertificateRegisteDate, out dtConvertResult));

            #endregion
        }


        /// <summary>
        /// 测试返回结果是否符合json格式
        /// </summary>
        [Test]
        [Ignore("对象序列化成JSON，由.NET自带组件实现，可忽略")]
        public void TestResultFormat()
        {
            var isJson = false;
            //JsonTextReader reader = new JsonTextReader(new StringReader(apiResult));
            var reader = new JsonTextReader(new StringReader(""));
            if (reader.Read())
            {
                isJson = JsonToken.StartObject == reader.TokenType;
            }
            Assert.IsTrue(isJson);
        }

        //待测试项目：
        ///驼峰命名法。调用controller不会应用WebApiConfig.Register中的设置，测试方式待研究。

    }
}
