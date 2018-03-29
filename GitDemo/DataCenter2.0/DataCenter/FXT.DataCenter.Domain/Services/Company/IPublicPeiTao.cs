using FXT.DataCenter.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Services
{
    public interface IPublicPeiTao
    {
        IQueryable<Dat_PeiTao> GetPeiTaoList(int cityId, int typeCode);
    }
}
