using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenPlatform.Domain.Models;

namespace OpenPlatform.Domain.Repositories
{
   public interface ICustomerRepository
   {
       /// <summary>
       /// 根据授权用户ID，获取该条授权用户信息
       /// </summary>
       /// <param name="userId">用户名</param>
       /// <returns></returns>
       Customer GetCustomer(string userId);
   }
}
