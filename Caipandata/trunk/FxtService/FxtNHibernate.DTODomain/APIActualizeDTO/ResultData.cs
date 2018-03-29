using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace FxtNHibernate.DTODomain.APIActualizeDTO
{
    public class ResultData
    {

        /// <summary>
        /// 结果,1:成功,0:失败
        /// </summary>
        public int Type
        {
            get;
            set;
        }
        /// <summary>
        /// 消息结果
        /// </summary>
        public string Message
        {
            get;
            set;
        }
        /// <summary>
        /// 返回的其他内容  "{\"asdff\":\"asdf\"}"
        /// </summary>
        public string Data
        {
            get;
            set;
        }
        /// <summary>
        /// 数据个数
        /// </summary>
        public int Count
        {
            get;
            set;
        }
    }
}
