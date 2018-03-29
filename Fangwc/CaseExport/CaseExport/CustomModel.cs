using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CaseExport
{
    [DataContract]
    [Serializable]
    public class TableRequestModel
    {
        [DataMember]
        public string DBServer { get; set; }
        [DataMember]
        public string DBName { get; set; }
        [DataMember]
        public string TableName { get; set; }
    }

    [DataContract]
    [Serializable]
    public class TableQueryCriteria : TableRequestModel
    {
        [DataMember]
        public string Where { get; set; }
        /// <summary>
        /// 页码,从1开始
        /// </summary>
        [DataMember]
        public int PageNumber { get; set; }
        [DataMember]
        public string DefaultOrderByColumn { get; set; }

    }


    [DataContract]
    [Serializable]
    public class ResponseModel
    {
        /// <summary>
        /// -1:Exception; 0:False; 1:True or success
        /// </summary>
        [DataMember]
        public int Status { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        [DataMember]
        public string Message { get; set; }
    }

    [DataContract]
    [Serializable]
    public class TableQueryResponseModel : ResponseModel
    {
        /// <summary>
        /// 页码
        /// </summary>
        [DataMember]
        public int PageNumber { get; set; }
        /// <summary>
        /// 每页显示项目条数
        /// </summary>
        [DataMember]
        public int PageSize { get; set; }

        /// <summary>
        /// 实际数据总行数
        /// </summary>
        [DataMember]
        public int TotalCount { get; set; }

        /// <summary>
        /// 查询结果
        /// </summary>
        [DataMember]
        public DataTable Result { get; set; }

        /// <summary>
        /// 获取总页数
        /// </summary>
        public int TotalPage
        {
            get
            {
                var totalPage = TotalCount / PageSize;
                if (TotalCount % PageSize > 0)
                {
                    totalPage++;
                }
                return totalPage;
            }
        }

    }


}
