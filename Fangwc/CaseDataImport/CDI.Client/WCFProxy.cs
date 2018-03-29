using CDI.Client.CDIService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CDI.Models;
using CDI.Common;

namespace CDI.Client
{
    public class WCFProxy : IProxy
    {
        #region Fields

        /// <summary>
        /// lock for echo method
        /// because echo is too freqence
        /// </summary>
        private static AutoResetEvent lockEcho = new AutoResetEvent(true);

        /// <summary>
        /// last echo status
        /// </summary>
        private static bool echoSuccessful = true;

        /// <summary>
        /// last echo time
        /// </summary>
        private static DateTime? lastEchoTime;

        #endregion

        #region IProxy Members

        public string Echo(string msg)
        {
            try
            {
                lockEcho.WaitOne();
                if (lastEchoTime != null && lastEchoTime.HasValue)
                {
                    TimeSpan timespan = System.DateTime.Now - lastEchoTime.Value;
                    if (timespan.Ticks < new TimeSpan(0, 0, 0, 0, 500).Ticks)
                    {
                        if (echoSuccessful)
                        {
                            return msg;
                        }
                        else
                        {
                            throw new Exception("echo error");
                        }
                    }
                }

                CDIServiceClient client = new CDIServiceClient(CurrentData.Instance.EndPointName);
                try
                {
                    string retu = client.Echo(msg);
                    echoSuccessful = true;
                    lastEchoTime = System.DateTime.Now;
                    return retu;
                }
                catch (Exception ex)
                {
                    CurrentData.Instance.Logger.Error(ex);
                    throw;
                }
                finally
                {
                    Task.Factory.StartNew(() => client.Close());
                }
            }
            finally
            {
                lockEcho.Set();
            }
        }
 
        public ResponseModel ValidateAddress(AddressRequestModel addr)
        {
            return this.RunAction<ResponseModel>((client) =>
            {
                var result = client.ValidateAddress(BinarySerialization.WriteToBytes(addr));
                return BinarySerialization.ReadFromBytes<ResponseModel>(result);
            });
        }

        public LoginResponseModel Login(LoginRequestModel user)
        {
            return this.RunAction<LoginResponseModel>((client) =>
            {
                var result = client.Login(BinarySerialization.WriteToBytes(user));
                return BinarySerialization.ReadFromBytes<LoginResponseModel>(result);
            });
        }

        public ResponseModel Logout(TokenRequestModel user)
        {
            return this.RunAction<ResponseModel>((client) =>
            {
                var result = client.Logout(BinarySerialization.WriteToBytes(user));
                return BinarySerialization.ReadFromBytes<ResponseModel>(result);
            });
        }


        #endregion

        #region Helper method

        private T RunAction<T>(Func<CDIServiceClient, T> handleAction) where T : ResponseModel
        {
            CDIServiceClient client = new CDIServiceClient(CurrentData.Instance.EndPointName);
            T retu = default(T);
            try
            {
                retu = handleAction(client);
            }
            catch (Exception ex)
            {
                CurrentData.Instance.Logger.Error(ex);
                throw;
            }
            finally
            {
                Task.Factory.StartNew(() => client.Close());
            }
            return retu;
        }

        #endregion


        public CityResponseModel QueryCityInfoList()
        {
            return this.RunAction<CityResponseModel>((client) =>
            {
                var result = client.QueryCityInfoList(BinarySerialization.WriteToBytes(CurrentData.Instance.Token));
                return BinarySerialization.ReadFromBytes<CityResponseModel>(result);
            });
        }

        public AreaResponseModel QueryAreaInfoMap(int cityId)
        {
            return this.RunAction<AreaResponseModel>((client) =>
            {
                var result = client.QueryAreaInfoMap(BinarySerialization.WriteToBytes(CurrentData.Instance.Token),
                    BinarySerialization.WriteToBytes(new IntRequestModel() { Number = cityId }));
                return BinarySerialization.ReadFromBytes<AreaResponseModel>(result);
            });
        }

        public SysCodeResponseModel QueryPurposeInfoMap()
        {
            return this.RunAction<SysCodeResponseModel>((client) =>
            {
                var result = client.QueryPurposeInfoMap(BinarySerialization.WriteToBytes(CurrentData.Instance.Token));
                return BinarySerialization.ReadFromBytes<SysCodeResponseModel>(result);
            });
        }

        public SysCodeResponseModel QueryFrontInfoMap()
        {
            return this.RunAction<SysCodeResponseModel>((client) =>
            {
                var result = client.QueryFrontInfoMap(BinarySerialization.WriteToBytes(CurrentData.Instance.Token));
                return BinarySerialization.ReadFromBytes<SysCodeResponseModel>(result);
            });
        }

        public SysCodeResponseModel QueryBuildingTypeInfoMap()
        {
            return this.RunAction<SysCodeResponseModel>((client) =>
            {
                var result = client.QueryBuildingTypeInfoMap(BinarySerialization.WriteToBytes(CurrentData.Instance.Token));
                return BinarySerialization.ReadFromBytes<SysCodeResponseModel>(result);
            });
        }

        public SysCodeResponseModel QueryHouseTypeInfoMap()
        {
            return this.RunAction<SysCodeResponseModel>((client) =>
            {
                var result = client.QueryHouseTypeInfoMap(BinarySerialization.WriteToBytes(CurrentData.Instance.Token));
                return BinarySerialization.ReadFromBytes<SysCodeResponseModel>(result);
            });
        }

        public SysCodeResponseModel QueryStructureInfoMap()
        {
            return this.RunAction<SysCodeResponseModel>((client) =>
            {
                var result = client.QueryStructureInfoMap(BinarySerialization.WriteToBytes(CurrentData.Instance.Token));
                return BinarySerialization.ReadFromBytes<SysCodeResponseModel>(result);
            });
        }

        public SysCodeResponseModel QueryFitmentInfoMap()
        {
            return this.RunAction<SysCodeResponseModel>((client) =>
            {
                var result = client.QueryFitmentInfoMap(BinarySerialization.WriteToBytes(CurrentData.Instance.Token));
                return BinarySerialization.ReadFromBytes<SysCodeResponseModel>(result);
            });
        }

        public SysCodeResponseModel QueryMoneyUnitInfoMap()
        {
            return this.RunAction<SysCodeResponseModel>((client) =>
            {
                var result = client.QueryMoneyUnitInfoMap(BinarySerialization.WriteToBytes(CurrentData.Instance.Token));
                return BinarySerialization.ReadFromBytes<SysCodeResponseModel>(result);
            });
        }

        public BatchInsertResponseModel BatchInsertDataCase(DataCase[] dc, string tableName)
        {
            return this.RunAction<BatchInsertResponseModel>((client) =>
            {
                var result = client.BatchInsertDataCase(BinarySerialization.WriteToBytes(CurrentData.Instance.Token),
                    BinarySerialization.WriteToBytes(new BatchInsertRequestModel() { DC = dc, TableName = tableName }));
                return BinarySerialization.ReadFromBytes<BatchInsertResponseModel>(result);
            });
        }

        public DataProjectResponseModel QueryDataProjectList(int cityId, int areaId, string tableName)
        {
            return this.RunAction<DataProjectResponseModel>((client) =>
            {
                var result = client.QueryDataProjectList(BinarySerialization.WriteToBytes(CurrentData.Instance.Token),
                    BinarySerialization.WriteToBytes(new DataProjectRequestModel() { CityID = cityId, AreaID = areaId, TableName = tableName }));
                return BinarySerialization.ReadFromBytes<DataProjectResponseModel>(result);
            });
        }

        public DataProjectResponseModel PagingQueryProjectList(int cityId, int areaId, string tableName, int pageIndex)
        {
            return this.RunAction<DataProjectResponseModel>((client) =>
            {
                var result = client.PagingQueryProjectList(BinarySerialization.WriteToBytes(CurrentData.Instance.Token),
                    BinarySerialization.WriteToBytes(new ProjectPageRequestModel() { CityID = cityId, AreaID = areaId, TableName = tableName, PageIndex = pageIndex }));
                return BinarySerialization.ReadFromBytes<DataProjectResponseModel>(result);
            });
        }

        public ProjectNameResponseModel GetNetworkNames(int cityId, int pageNumber, int pageSize)
        {
            return this.RunAction<ProjectNameResponseModel>((client) =>
            {
                var result = client.GetNetworkNames(BinarySerialization.WriteToBytes(CurrentData.Instance.Token),
                    BinarySerialization.WriteToBytes(new ProjectNameRequestModel() { CityID = cityId, PageNumber = pageNumber, PageSize = pageSize }));
                return BinarySerialization.ReadFromBytes<ProjectNameResponseModel>(result);
            });
        }

        public CityResponseModel PagingQueryCityList(int pageIndex)
        {
            return this.RunAction<CityResponseModel>((client) =>
            {
                var result = client.PagingQueryCityList(BinarySerialization.WriteToBytes(CurrentData.Instance.Token),
                    BinarySerialization.WriteToBytes(new IntRequestModel() { Number = pageIndex }));
                return BinarySerialization.ReadFromBytes<CityResponseModel>(result);
            });
        }
    }
}
