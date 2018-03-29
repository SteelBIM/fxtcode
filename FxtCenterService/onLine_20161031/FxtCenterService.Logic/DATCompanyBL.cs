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
    public class DATCompanyBL
    {
        /// <summary>
        /// 根据公司名称获取表信息
        /// 创建人:曾智磊,日期:2014-06-26
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static DATCompany GetByName(string name)
        {
            return DATCompanyDA.GetByName(name);
        }        
        /// <summary>
        /// 新增公司
        /// 创建人:曾智磊,日期:2014-06-26
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int Add(DATCompany model)
        {
            return DATCompanyDA.Add(model);
        }
    }
}
