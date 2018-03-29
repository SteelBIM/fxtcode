using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using DataConv.Model;

namespace DataConv.Service.ConApp
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IProviderService”。
    [ServiceContract]
    public interface IProviderService
    {
        [OperationContract]
        void DoWork();

        [OperationContract]
        List<Model.City> QueryCityInfoList();

        [OperationContract]
        List<Model.Area> QueryAreaInfoMap(int cityID);

        [OperationContract]
        List<Model.SysCode> QueryPurposeInfoMap();

        [OperationContract]
        List<Model.SysCode> QueryFrontInfoMap();

        [OperationContract]
        List<Model.SysCode> QueryBuildingTypeInfoMap();

        [OperationContract]
        List<Model.SysCode> QueryHouseTypeInfoMap();

        [OperationContract]
        List<Model.SysCode> QueryStructureInfoMap();

        [OperationContract]
        List<Model.SysCode> QueryFitmentInfoMap();

        [OperationContract]
        List<Model.SysCode> QueryMoenyUnitInfoMap();

        [OperationContract]
        int BatchInsertDataCase(Model.DataCase[] dc,string tableName);

        [OperationContract]
        List<Model.DataProject> QueryDataProjectList(int cityID, int AreaID, string tableName);

        [OperationContract]
        List<Model.DataProject> PagingQueryProjectList(int CityID, int AreaID, string tName, int page);

        [OperationContract]
        List<Model.City> PagingQueryCityList(int page);
    }
}
