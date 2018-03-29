using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CDI.Models
{
    /// <summary>
    /// User model class
    /// </summary>
    [DataContract]
    [Serializable]
    public class LoginRequestModel
    {
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string MacAddress { get; set; }

    }

    [DataContract]
    [Serializable]
    public class AddressRequestModel
    {
        [DataMember]
        public string MacAddress { get; set; }
    }

    [DataContract]
    [Serializable]
    public class TokenRequestModel
    {
        [DataMember]
        public string Token { get; set; }

    }

    #region Request Models

    [DataContract]
    [Serializable]
    public class IntRequestModel
    {
        [DataMember]
        public int Number { get; set; }

    }

    [DataContract]
    [Serializable]
    public class BatchInsertRequestModel
    {
        [DataMember]
        public DataCase[] DC { get; set; }

        [DataMember]
        public string TableName { get; set; }
        
    }

    [DataContract]
    [Serializable]
    public class DataProjectRequestModel
    {
        [DataMember]
        public int CityID { get; set; }
        [DataMember]
        public int AreaID { get; set; }
        [DataMember]
        public string TableName { get; set; }

    }

    [DataContract]
    [Serializable]
    public class ProjectPageRequestModel : DataProjectRequestModel
    {
        [DataMember]
        public int PageIndex { get; set; }

    }

    [DataContract]
    [Serializable]
    public class ProjectNameRequestModel
    {
        [DataMember]
        public int CityID { get; set; }
        [DataMember]
        public int PageNumber { get; set; }
        [DataMember]
        public int PageSize { get; set; }

    }

    #endregion


    [DataContract]
    [Serializable]
    public class ResponseModel
    {
        /// <summary>
        /// -3:relogin; -2:notlogin; -1:Exception; 0:False; 1:True or success
        /// </summary>
        [DataMember]
        public int Status { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        public T ValidateStatus<T>() where T : ResponseModel
        {
            if (Status == -2 || Status == -3)
            {
                throw new Exception(Message);
            }
            else if (Status == -1 || Status == 0)
            {
                throw new Exception(Message ?? "读取数据库失败");
            }
            return this as T;
        }
    }

    [DataContract]
    [Serializable]
    public class LoginResponseModel : ResponseModel
    {
        [DataMember]
        public string Token { get; set; }

    }

    #region Response Models

    [DataContract]
    [Serializable]
    public class CityResponseModel : ResponseModel
    {
        [DataMember]
        public List<City> Citys { get; set; }

    }

    [DataContract]
    [Serializable]
    public class AreaResponseModel : ResponseModel
    {
        [DataMember]
        public List<Area> Areas { get; set; }

    }

    [DataContract]
    [Serializable]
    public class SysCodeResponseModel : ResponseModel
    {
        [DataMember]
        public List<SysCode> SysCodes { get; set; }

    }
    
    [DataContract]
    [Serializable]
    public class DataProjectResponseModel : ResponseModel
    {
        [DataMember]
        public List<DataProject> DataProjects { get; set; }

    }

    [DataContract]
    [Serializable]
    public class ProjectNameResponseModel : ResponseModel
    {
        [DataMember]
        public List<SYS_ProjectMatch> NetworkNames { get; set; }

    }

    [DataContract]
    [Serializable]
    public class BatchInsertResponseModel : ResponseModel
    {
        [DataMember]
        public int Count { get; set; }

    }


    #endregion

    

    


}
