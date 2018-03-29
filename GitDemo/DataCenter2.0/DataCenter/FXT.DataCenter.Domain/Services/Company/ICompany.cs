using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;

namespace FXT.DataCenter.Domain.Services
{
   public interface ICompany
    {
       /// <summary>
       /// 根据ID 获取公司信息
       /// </summary>
       /// <param name="companyId"></param>
       /// <returns></returns>
       DAT_Company GetCompanyById(int companyId);

       /// <summary>
       /// 获取公司信息（增加条件时做扩展）
       /// </summary>
       /// <param name="companyName"></param>
       /// <param name="cityId"></param>
       /// <returns></returns>
       IQueryable<DAT_Company> GetCompany_like(string companyName, int cityId);

       /// <summary>
       /// 获取公司信息ForOffice
       /// </summary>
       /// <param name="companyName"></param>
       /// <param name="cityId"></param>
       /// <returns></returns>
       IQueryable<DAT_Company> GetCompany_office(string companyName, int cityId);

       /// <summary>
       /// 获取指定城市的公司
       /// </summary>
       /// <returns></returns>
       IQueryable<DAT_Company> GetCompanyNameList(int cityId);

       /// <summary>
       /// 新增公司
       /// </summary>
       /// <param name="dc"></param>
       /// <returns></returns>
       int AddCompany(DAT_Company dc);

       /// <summary>
       /// 修改公司
       /// </summary>
       /// <param name="dc"></param>
       /// <returns></returns>
       int UpdateCompany(DAT_Company dc);
       /// <summary>
       /// 删除公司
       /// </summary>
       /// <param name="companyId"></param>
       /// <returns></returns>
       int DeleteCompany(int companyId);
       /// <summary>
       /// 公司土地数与楼盘数
       /// </summary>
       /// <param name="companyId"></param>
       /// <returns></returns>
       string CompanyStatistcs(int companyId);

       /// <summary>
       /// 该公司是否存在
       /// </summary>
       /// <param name="chineseName">公司中文名称</param>
       /// <param name="companyId">公司ID</param>
       /// <returns></returns>
       bool CompanyIsExit(string chineseName,int companyId);


      
    }
}
