using FxtNHibernate.FxtTempDomain.Entities;
using FxtNHibernater.Data;
using FxtService.Common;
using FxtService.Contract.FxtRunFlatsInterface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

/**
 * 作者: 曾智磊
 * 时间:2014-03-03
 * 摘要:新建手机端跑盘工具服务FxtRunFlatsActualize.FxtRunFlats契约(接口)实现
 * **/
namespace FxtService.Service.FxtRunFlatsActualize
{
    [FxtService.Service.ServiceBehavior]
    public class FxtRunFlats:IFxtRunFlats
    {
        public string test(int? code)
        {
            MSSQLDBDAL db = new MSSQLDBDAL(Utility.DBFxtTemp);
            var obj = db.GetCustom<SYSCode>((Expression<Func<SYSCode, bool>>)(tbl => tbl.Code == Convert.ToInt32(code)));
            db.Close();
            string json=Utility.GetJson(1, "", data: obj);
            return json;
        }

    }
}
