using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.DTODomain.FxtDataCenterDTO
{
    public class DataCenterResult
    {
        /// <summary>
        /// 结果,1表示成功；-1表示失败
        /// </summary>
        public int returntype
        {
            get;
            set;
        }
        /// <summary>
        /// 状态说明
        /// </summary>
        public string returntext
        {
            get;
            set;
        }
        /// <summary>
        /// 返回的数据
        /// </summary>
        public string data
        {
            get;
            set;
        }
        /// <summary>
        /// 调试信息
        /// </summary>
        public string debug
        {
            get;
            set;
        }
    }
}
