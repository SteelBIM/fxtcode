using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.DAL.LinqToSql;
using FxtSpider.DAL.DB;

namespace FxtSpider.Bll
{
    public static class SysCodeManager
    {
        /// <summary>
        /// 案例爬取错误类型-网络异常
        /// </summary>
        public const int Code_1_1 = 1000001;
        /// <summary>
        /// 案例爬取错误类型-未获取到字符
        /// </summary>
        public const int Code_1_2 = 1000002;
        /// <summary>
        /// 案例爬取错误类型-需输入验证码
        /// </summary>
        public const int Code_1_3 = 1000003;
        /// <summary>
        /// 案例爬取错误类型ID
        /// </summary>
        public const int CodeID_1 = 1000;
        /// <summary>
        /// 获取所有案例爬取错误类型
        /// </summary>
        /// <param name="_dc"></param>
        /// <returns></returns>
        public static List<SYS_Code> GetAllSpiderErrorCode(DataClass _dc=null)
        {
            DataClass dc = new DataClass(_dc);
            List<SYS_Code> list = GetCodeById(CodeID_1, dc);
            dc.Connection_Close();
            dc.Dispose();
            return list;
        }
        public static List<SYS_Code> GetCodeById(int id,DataClass _dc=null)
        {
            DataClass dc = new DataClass(_dc);
            var query=dc.DB.SYS_Code.Where(tbl => tbl.ID == id);
            List<SYS_Code> list = query.ToList();
            dc.Connection_Close();
            dc.Dispose();
            return list;
        }
    }
}
