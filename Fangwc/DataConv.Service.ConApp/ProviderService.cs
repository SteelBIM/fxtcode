using DataConv.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace DataConv.Service.ConApp
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“ProviderService”。
    public class ProviderService : IProviderService
    {
        public void DoWork()
        {
        }

        public List<Model.City> QueryCityInfoList()
        {
            ServiceFacade service = new ServiceFacade();
            return service.QueryCityInfoList();
        }


        public List<Model.Area> QueryAreaInfoMap(int cityID)
        {
            ServiceFacade service = new ServiceFacade();
            return service.QueryAreaInfoMap(cityID);
        }

        public List<Model.SysCode> QueryPurposeInfoMap()
        {
            ServiceFacade service = new ServiceFacade();
            return service.QueryPurposeInfoMap();
        }

        public List<Model.SysCode> QueryFrontInfoMap()
        {
            ServiceFacade service = new ServiceFacade();
            return service.QueryFrontInfoMap();
        }

        public List<Model.SysCode> QueryBuildingTypeInfoMap()
        {
            ServiceFacade service = new ServiceFacade();
            return service.QueryBuildingTypeInfoMap();
        }

        public List<Model.SysCode> QueryHouseTypeInfoMap()
        {
            ServiceFacade service = new ServiceFacade();
            return service.QueryHouseTypeInfoMap();
        }

        public List<Model.SysCode> QueryStructureInfoMap()
        {
            ServiceFacade service = new ServiceFacade();
            return service.QueryStructureInfoMap();
        }

        public List<Model.SysCode> QueryFitmentInfoMap()
        {
            ServiceFacade service = new ServiceFacade();
            return service.QueryFitmentInfoMap();
        }

        public List<Model.SysCode> QueryMoenyUnitInfoMap()
        {
            ServiceFacade service = new ServiceFacade();
            return service.QueryMoneyUnitInfoMap();
        }

        public int BatchInsertDataCase(Model.DataCase[] dc, string tableName)
        {
            ServiceFacade service = new ServiceFacade();
            return service.BatchInsertDataCase(dc, tableName);
        }


        public List<Model.DataProject> QueryDataProjectList(int cityID, int AreaID, string tableName)
        {
            ServiceFacade service = new ServiceFacade();
            return service.QueryDataProjectList(cityID, AreaID, tableName);
        }


        public List<Model.DataProject> PagingQueryProjectList(int CityID, int AreaID, string tName, int page)
        {
            ServiceFacade service = new ServiceFacade();
            return service.PagingQueryProjectList(CityID, AreaID, tName, page);
        }


        public List<Model.City> PagingQueryCityList(int page)
        {
            ServiceFacade service = new ServiceFacade();
            return service.PagingQueryCityList(page);
        }
    }
}
