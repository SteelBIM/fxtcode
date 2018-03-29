using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenPlatform.Domain.DTO;
using OpenPlatform.Domain.Models;

namespace OpenPlatform.Domain.Repositories
{
   public interface IEvaluationRepository
   {
       /// <summary>
       /// 根据身份证ID获取委估业务状态
       /// </summary>
       /// <param name="id"></param>
       /// <param name="fxtCompanyId"></param>
       /// <returns></returns>
       IQueryable<long> GetAppraiseStatusById(string id, int fxtCompanyId);
       /// <summary>
       /// 根据身份证获取委估对象ID
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
       IQueryable<long> GetEoId(string id);
       
       /// <summary>
       ///  根据省份证获取委估业务信息
       /// </summary>
       /// <param name="id">身份证ID</param>
       /// <returns></returns>
       IQueryable<EvaluationDto> GetEntrustAppraiseById(string id);

       /// <summary>
       /// 根据委估业务ID获取委估对象
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
       IQueryable<EntrustObjectDto> GetEntrustObjectById(long id);

       /// <summary>
       /// 获取产权人
       /// </summary>
       IQueryable<PropertyInfoDto> GetPropertyBuyerById(long objectId);

       /// <summary>
       /// 获取图片列表
       /// </summary>
       IQueryable<string> GetPathListById(long objectId);
       /// <summary>
       /// 获取买房人信息
       /// </summary>
       /// <param name="objectId"></param>
       /// <returns></returns>
       IQueryable<BuyerInfoDto> GetBuyerInfoById(long objectId);

       /// <summary>
       /// 添加委估业务信息
       /// </summary>
       /// <param name="ed"></param>
       /// <returns></returns>
       int AddEntrust(OpenPlatform.Domain.DTO.Evaluation4GjbDto ed);

       /// <summary>
       /// 增加委估对象信息
       /// </summary>
       /// <param name="eo"></param>
       /// <returns></returns>
       int AddEntrustObject(EntrustObject4GjbDto eo);

       /// <summary>
       /// 增加产权信息
       /// </summary>
       /// <param name="pi"></param>
       /// <returns></returns>
       int AddProperty(PropertyInfo pi);

       /// <summary>
       /// 增加够烦人信息
       /// </summary>
       /// <param name="bi"></param>
       /// <returns></returns>
       int AddBuyer(BuyerInfo bi);

       /// <summary>
       /// 添加照片信息
       /// </summary>
       /// <param name="sf"></param>
       /// <returns></returns>
       int AddPictures(Surveyfiles sf);
   }
}
