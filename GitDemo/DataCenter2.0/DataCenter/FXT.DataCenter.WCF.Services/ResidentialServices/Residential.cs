using System;
using System.ServiceModel.Activation;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Common.Common;
using FXT.DataCenter.Infrastructure.Common.CommonWeb;
using FXT.DataCenter.WCF.Contract;

namespace FXT.DataCenter.WCF.Services
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Residential : IResidential
    {

        private readonly IProjectCase _projectCase;

        public Residential(IProjectCase projectCase)
        {
            this._projectCase = projectCase;
        }

        public void DeleteSameProjectCase(int fxtCompanyId, int cityId, DateTime caseDateFrom, DateTime caseDateTo, string saveUser)
        {
            try
            {
                _projectCase.DeleteSameProjectCase(fxtCompanyId, cityId, caseDateFrom, caseDateTo, saveUser);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/ProjectCase/DeleteSameCase", RequestHelper.GetIP(), "", cityId, fxtCompanyId, ex);
            }

        }
    }
}
