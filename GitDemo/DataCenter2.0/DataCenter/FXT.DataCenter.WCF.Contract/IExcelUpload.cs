using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace FXT.DataCenter.WCF.Contract
{
    [ServiceContract]
    public interface IExcelUpload
    {
        [OperationContract(IsOneWay = true)]
        void Start(int cityid, int fxtcompanyid, string filePath, string userid, string taskName, string type);

        [OperationContract]
        void HouseConvert(string filePath, string newPath);
    }
}
