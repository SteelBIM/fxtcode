using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace FXT.DataCenter.WCF.Contract
{
   [ServiceContract]
   public interface IResidential
    {
        [OperationContract(IsOneWay = true)]
       void DeleteSameProjectCase(int fxtCompanyId, int cityId, DateTime caseDateFrom, DateTime caseDateTo, string saveUser);
    }
}
