using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/**
 * 作者: 曾智磊
 * 时间: 2013.12.06
 * 摘要: 新建wcf方法FxtspiderClient.SpiderExport返回值实体
 * **/
namespace FxtSpider.FxtApi.Model
{
    /// <summary>
    /// wcf方法FxtspiderClient.SpiderExport返回值实体
    /// </summary>
    public class SpiderExportResult
    {
        public static readonly string Remark_楼盘名不存在 = "1";
        public static readonly string Remark_系统异常 = "2";
        public string ID
        {
            get;
            set;
        }
        public string Remark
        {
            get;
            set;
        }
        public string Success
        {
            get;
            set;
        }
        public int FxtId
        {
            get;
            set;
        }

        public SpiderExportResult()
        {
        }
        public SpiderExportResult(string _id, string _remark, string _success,int _fxtId )
        {
            this.ID = _id;
            this.Remark = _remark;
            this.Success = _success;
            this.FxtId = _fxtId;
        }
    }

}
