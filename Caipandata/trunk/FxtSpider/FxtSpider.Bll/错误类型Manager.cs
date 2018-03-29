using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.Dll.Manager;

namespace FxtSpider.Bll
{
    public static class 错误类型Manager
    {
        public static readonly int 楼盘名不存在_ID = StaticValue.楼盘名不存在_ID;
        public static readonly int 系统异常_ID  = StaticValue.系统异常_ID;
        public static readonly int 其他_ID = StaticValue.其他_ID;
    }
}
