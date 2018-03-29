using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace CDI.Service
{
    [ServiceContract]
    public interface ICDIService
    {
        [OperationContract]
        string Echo(string msg);

        [OperationContract]
        byte[] ValidateAddress(byte[] addr);

        [OperationContract]
        byte[] Login(byte[] user);

        [OperationContract]
        byte[] Logout(byte[] user);


        [OperationContract]
        byte[] QueryCityInfoList(byte[] userToken);

        [OperationContract]
        byte[] QueryAreaInfoMap(byte[] userToken, byte[] args);

        [OperationContract]
        byte[] QueryPurposeInfoMap(byte[] userToken);

        [OperationContract]
        byte[] QueryFrontInfoMap(byte[] userToken);

        [OperationContract]
        byte[] QueryBuildingTypeInfoMap(byte[] userToken);

        [OperationContract]
        byte[] QueryHouseTypeInfoMap(byte[] userToken);

        [OperationContract]
        byte[] QueryStructureInfoMap(byte[] userToken);

        [OperationContract]
        byte[] QueryFitmentInfoMap(byte[] userToken);

        [OperationContract]
        byte[] QueryMoneyUnitInfoMap(byte[] userToken);

        [OperationContract]
        byte[] BatchInsertDataCase(byte[] userToken, byte[] args);

        [OperationContract]
        byte[] QueryDataProjectList(byte[] userToken, byte[] args);

        [OperationContract]
        byte[] PagingQueryProjectList(byte[] userToken, byte[] args);

        [OperationContract]
        byte[] GetNetworkNames(byte[] userToken, byte[] args);

        [OperationContract]
        byte[] PagingQueryCityList(byte[] userToken, byte[] args);



    }



}
