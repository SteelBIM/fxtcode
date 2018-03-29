using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;

namespace FXT.DataCenter.Domain.Services
{
    public interface IPropertyAddress
    {
        IQueryable<LNK_P_PAddress> GetPropertyAddress(string projectname, int cityId, int fxtCompanyId);
        LNK_P_PAddress GetPropertyAddressById(int id, int cityId, int fxtCompanyId);
        LNK_P_PAddress IsExistPropertyAddressByProjectid(int ProjectId, string PropertyAddress, int cityId, int fxtCompanyId);
        int AddPropertyAddress(LNK_P_PAddress pa);
        int UpdatePropertyAddress(LNK_P_PAddress pa);
        int DeletePropertyAddress(int id);
    }
}
