using System;
using System.Text;

namespace KSWF.Core.Utility
{
    /// <summary>
    /// 服务处理结果输出
    /// </summary>
    public class KingResponse
    {
        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool Success
        {
            get;
            set;
        }

        /// <summary>
        /// 业务数据
        /// </summary>
        public object Data
        {
            get;
            set;
        }

        public int Count { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMsg
        {
            get;
            set;
        }

        public static KingResponse GetErrorResponse(string errorMsg)
        {    
            KingResponse res = new KingResponse();
            res.Success = false;
            res.ErrorMsg = errorMsg;
            return res;
        }

        public static KingResponse GetResponse(object data)
        {
            KingResponse res = new KingResponse();
            res.Success = true;
            res.Data = data;
            return res;
        }
    }
}
