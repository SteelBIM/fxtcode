using FxtDataAcquisition.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtDataAcquisition.Application.Interfaces
{
    public interface ITempletService
    {
        /// <summary>
        /// 获取默认模板
        /// </summary>
        /// <param name="datType">类型</param>
        /// <returns></returns>
        Templet GetTempletDefult(int datType, int fxtCompanyId);
    }
}
