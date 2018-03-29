using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtUserCenterService.DataAccess;
using FxtUserCenterService.Entity;
using CAS.Entity.DBEntity;

namespace FxtUserCenterService.Logic
{
    /// <summary>
    /// 手机消息推送记录
    /// </summary>
    public class DatPushMessageDA:Base
    {
        public static int Add(DatPushMessage model)
        {
            return InsertFromEntity<DatPushMessage>(model);
        }
    }
}
