using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.SynchronousStudy.Common
{
    public class ErrorMsgCode
    {
        /// <summary>
        /// 错误代码字典
        /// </summary>
        public static readonly Dictionary<int, string> ErrorDic = new Dictionary<int, string>() { {0,"" }
        ,{ 101, "参数错误" }
        ,{ 102, "地区ID不能为空" }
        ,{ 103, "地区不能为空" }
        ,{ 104, "用户ID不能为空" }
        ,{ 105, "用户名不能为空" }
        ,{ 106, "城市ID不能为空"}
        ,{ 107, "城市不能为空"}
        ,{ 108, "省份ID不能为空"}
        ,{ 109, "省份不能为空"}
        ,{ 110, "学科ID不能为空"}
        ,{ 111, "学科不能为空" }
        ,{ 112, "学校ID不能为空"}
        ,{ 113, "学校不能为空"}
        ,{ 114, "无订单信息"}
        ,{ 115, "所在地区没有学校信息"}
        ,{ 116, "版本号不能为空"}
        ,{ 201, "信息不存在"}
        ,{ 301, "服务内部错误，插入数据操作失败"}
        ,{ 401, "请求接口失效"}
        };
    }
}
