using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.DTODomain.APIActualizeDTO
{
    public class ResultData
    {

        /// <summary>
        /// 结果,1:成功,0:失败
        /// </summary>
        public int returntype
        {
            get;
            set;
        }
        /// <summary>
        /// 消息结果
        /// </summary>
        public string returntext
        {
            get;
            set;
        }
        /// <summary>
        /// 返回的其他内容  "{\"asdff\":\"asdf\"}"
        /// </summary>
        public string data
        {
            get;
            set;
        }
        /// <summary>
        /// 数据个数
        /// </summary>
        public string debug
        {
            get;
            set;
        }
        public ResultData()
        { }
        public ResultData(string _data)
        {
            this.returntype = 1;
            this.returntext = "";
            this.debug = "";
            this.data = _data;
        }
        public ResultData(int _returntype, string _returntext)
        {
            this.returntype = _returntype;
            this.returntext = _returntext;
            this.debug = "";
            this.data = null;
        }
    }
}
