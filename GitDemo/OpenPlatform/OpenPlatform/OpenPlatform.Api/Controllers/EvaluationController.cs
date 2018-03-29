using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Ninject;
using OpenPlatform.Api.Infrastructure;
using OpenPlatform.Application.Interfaces;
using OpenPlatform.Domain.DTO;
using OpenPlatform.Framework.IoC;
using OpenPlatform.Framework.Utils.Log;

namespace OpenPlatform.Api.Controllers
{
    [RoutePrefix("api/evaluation")]
    public class EvaluationController : ApiController
    {
        private readonly IEvaluationService _services;

        public EvaluationController()
        {
            this._services = new StandardKernel(new EvaluationServiceBinder()).Get<IEvaluationService>();
        }
        // for moq
        public EvaluationController(IEvaluationService services)
        {
            this._services = services;
        }

        //[Authorize]
        //public IHttpActionResult Get(string idNum)
        //{
        //    return Get(idNum, new List<int>());
        //}

        // -1：失败；1：成功；40001：模板配置错误；40002：操作不正确，估价宝正式报告中没有查询到数据
        [Authorize]
        public IHttpActionResult Get(string idNum, string companyIds)
        {
            try
            {
             
                if (string.IsNullOrEmpty(companyIds)) companyIds = string.Empty;
                var companyId = companyIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                AppraiseApi.Invoke(idNum, companyId);
                
                //与传入的身份证相关的委估对象ID集合
                var eoIds = _services.GetEoId(idNum);
                //var strEoId = "'" + string.Join("','", eoIds) + "'";

                //获取所有的委估业务
                var entrustAppraises = _services.GetEntrustAppraiseById(idNum);

                var list = new List<EntrustAppraise4BonaDto>();
                AutoMapper.Mapper.CreateMap<EvaluationDto, Evaluation4BonaDto>();

                //根据委估业务获取委估对象
                foreach (var item in entrustAppraises)
                {
                    var e = AutoMapper.Mapper.Map<EvaluationDto, Evaluation4BonaDto>(item);
                    e.EntrustObject = GetEntrustObjects(item.EAId, eoIds);

                    var ea = new EntrustAppraise4BonaDto()
                    {
                        EntrustAppraise = e
                    };

                    list.Add(ea);

                }

               
                return ResponseMessage(list: list, ex: null);



            }
            catch (Exception ex)
            {
                NLogHelper.Error(new ErrorLog { Exception = ex });

                return ResponseMessage(ex);
            }
        }

        /// <summary>
        /// 根据委估业务ID获取委估对象
        /// </summary>
        /// <param name="eaId"></param>
        /// <param name="eoIds"></param>
        /// <returns></returns>
        private List<EntrustObject4BonaDto> GetEntrustObjects(long eaId, IQueryable<long> eoIds)
        {
            var entrustObjs = _services.GetEntrustObjectById(eaId);

            var list = new List<EntrustObject4BonaDto>();

            AutoMapper.Mapper.CreateMap<EntrustObjectDto, EntrustObject4BonaDto>();
            AutoMapper.Mapper.CreateMap<PropertyInfoDto, PropertyInfo4BonaDto>();
            AutoMapper.Mapper.CreateMap<BuyerInfoDto, BuyerInfo4BonaDto>();

            foreach (var item in entrustObjs)
            {
                //跟当前传入的身份证不相关的委估对象忽略掉
                if (!eoIds.Contains(item.EOId)) continue;

                var propertyInfo = _services.GetPropertyBuyerById(item.EOId).ToList();
                var propertyList = AutoMapper.Mapper.Map<List<PropertyInfoDto>, List<PropertyInfo4BonaDto>>(propertyInfo);

                var buyerInfo = _services.GetBuyerInfoById(item.EOId).ToList();
                var buyerList = AutoMapper.Mapper.Map<List<BuyerInfoDto>, List<BuyerInfo4BonaDto>>(buyerInfo);

                var o = AutoMapper.Mapper.Map<EntrustObjectDto, EntrustObject4BonaDto>(item);

                o.PropertyInfo = propertyList;
                o.BuyerInfo = buyerList;
                o.EntrustObjectPrice = new EntrustObjectPrice4BonaDto
                {
                    AutoPrice = item.AutoPrice,
                    MainHouseUnitPrice = item.MainHouseUnitPrice,
                    MainHouseTotalPrice = item.MainHouseTotalPrice,
                    OutbuildingTotalPrice = item.OutbuildingTotalPrice,
                    LandUnitPrice = item.LandUnitPrice,
                    LandTotalPrice = item.LandTotalPrice,
                    AppraiseTotalPrice = item.AppraiseTotalPrice,
                    ValueDate = item.ValueDate
                };

                list.Add(o);

            }

            return list;
        }

        /// <summary>
        /// 返回消息
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private IHttpActionResult ResponseMessage(Exception ex, List<EntrustAppraise4BonaDto> list = null)
        {
            if (null == ex)
            {
                if (list != null && list.Any())
                {
                    return
                     Ok(new EvaluationApiDto
                     {
                         Code = 1,
                         Message = "成功",
                         EstateStatus = "1",
                         EstateInfo = list
                     });
                }

                return Ok(new EvaluationApiDto { Code = 40002, Message = "查不到此身份证对应的数据,请检查估价宝和云查勘操作是否正确！", EstateStatus = "0" });

            }

            if (ex.Message == "40001")
            {
                return Ok(new EvaluationApiDto { Code = 40001, Message = "云查勘模板解析错误！", EstateStatus = "0" });
            }

            return Ok(new EvaluationApiDto { Code = -1, Message = "系统繁忙,请稍后再试！", EstateStatus = "0" });
        }

    }
}
