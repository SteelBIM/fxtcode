using CDI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDI.Client
{
    public interface IProxy
    {

        string Echo(string msg);

        ResponseModel ValidateAddress(AddressRequestModel addr);

        LoginResponseModel Login(LoginRequestModel user);

        ResponseModel Logout(TokenRequestModel user);



        CityResponseModel QueryCityInfoList();

        AreaResponseModel QueryAreaInfoMap(int cityId);

        SysCodeResponseModel QueryPurposeInfoMap();

        SysCodeResponseModel QueryFrontInfoMap();

        SysCodeResponseModel QueryBuildingTypeInfoMap();

        SysCodeResponseModel QueryHouseTypeInfoMap();

        SysCodeResponseModel QueryStructureInfoMap();

        SysCodeResponseModel QueryFitmentInfoMap();

        SysCodeResponseModel QueryMoneyUnitInfoMap();

        BatchInsertResponseModel BatchInsertDataCase(DataCase[] dc, string tableName);

        DataProjectResponseModel QueryDataProjectList(int cityId, int areaId, string tableName);

        DataProjectResponseModel PagingQueryProjectList(int cityId, int areaId, string tableName, int pageIndex);

        ProjectNameResponseModel GetNetworkNames(int cityId, int pageNumber, int pageSize);

        CityResponseModel PagingQueryCityList(int pageIndex);

    }
}
