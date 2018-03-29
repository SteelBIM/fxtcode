using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.Domain.DTO.FxtDataWcfDTO
{
    public class FxtApi_PublicResult
    {
        /// <summary>
        /// 结果,1:成功,0:失败
        /// </summary>
        public int type
        {
            get;
            set;
        }
        /// <summary>
        /// 消息结果
        /// </summary>
        public string message
        {
            get;
            set;
        }
        /// <summary>
        /// 返回的其他内容
        /// </summary>
        public object data
        {
            get;
            set;
        }
        public FxtApi_PublicResult()
        { }
        public FxtApi_PublicResult(int _type, string _message, string _data)
        {
            this.type = _type;
            this.message = _message;
            this.data = _data;
        }
        public FxtApi_PublicResult(int _type, string _message)
        {
            this.type = _type;
            this.message = _message;
        }

    }
}
