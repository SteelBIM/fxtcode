using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using FxtCenterService.DataAccess;
using System.Data;
using System.Collections;
using CAS.Common;

//创建人:曾智磊,日期:2014-06-26
namespace FxtCenterService.Logic
{
    public class LNKPCompanyBL
    {
        /// <summary>
        /// 新增公司
        /// 创建人:曾智磊,日期:2014-06-26
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int Add(LNKPCompany model)
        {
            return LNKPCompanyDA.Add(model);
        }
        /// <summary>
        /// 修改关联公司
        /// 创建人:曾智磊,日期:2014-06-30
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int Update(LNKPCompany model)
        {
            return LNKPCompanyDA.Update(model);
        }
        /// <summary>
        /// 获取楼盘管理的企业
        /// 创建人:曾智磊,日期:2014-06-30
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="projectId"></param>
        /// <param name="cityId"></param>
        /// <param name="companyTypeCode"></param>
        /// <returns></returns>
        public static LNKPCompany GetLNKPCompanyByComIdAndProjIdAndType(int companyId, int projectId, int cityId, int companyTypeCode)
        {
            return LNKPCompanyDA.GetLNKPCompanyByComIdAndProjIdAndType(companyId, projectId, cityId, companyTypeCode);
        }

        /// <summary>
        /// 获取楼盘管理的企业
        /// 创建人:曾智磊,日期:2014-07-03
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public static List<LNKPCompany> GetLNKPCompanyByProjId(int projectId, int cityId)
        {
            return LNKPCompanyDA.GetLNKPCompanyByProjId(projectId, cityId);
        }
       
    }
}
