using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenPlatform.Domain.DTO;
using OpenPlatform.Domain.Models;

namespace OpenPlatform.Application.Interfaces
{
   public interface ISurveyService
    {
        /// <summary>
        /// 根据身份证获取评估机构ID
        /// </summary>
        /// <param name="idNum"></param>
        /// <returns></returns>
        IQueryable<int> GetCompanyId(string idNum);
        /// <summary>
        /// 根据委估对象ID获取照片列表
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        IQueryable<Surveyfiles> GetPicturesById(long objectId);
        /// <summary>
        /// 根据委估对象ID获取查勘信息
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        IQueryable<SurveyInfoDto> GetSurveyInfoById(long objectId);
    }
}
