using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace CAS.Common
{
    /// <summary>
    /// sql注入过滤
    /// </summary>
    public class SQLFilterHelper
    {
        /// <summary>
        /// 防SQL注入
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static bool ProcessSqlStr(string Str, int type,ref string key)
        {
            string SqlStr;

            SqlStr = "exec |insert |select |delete |update |truncate |declare ";
            
            bool ReturnValue = true;
            try
            {
                if (Str != "")
                {
                    string[] anySqlStr = SqlStr.Split('|');
                    foreach (string ss in anySqlStr)
                    {
                        if (Str.IndexOf(ss) >= 0)
                        {
                            key = ss;
                            return false;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
            return ReturnValue;
        }

        /// <summary>
        /// 处理like子句中的特殊字符,把特殊字符当做普通字符，like子句需添加 escape '<escape char>'。
        /// 例：like '%@key%' escape '$', 再对key做处理：key = EscapeLikeString(key, "$");
        /// </summary>
        /// <param name="value"></param>
        /// <param name="escapechar"></param>
        /// <returns></returns>
        public static string EscapeLikeString(string value, string escapechar)
        {
            string[] str_escape = { escapechar, "%", "_", "[", "]", "^" }; // escapechar must go first
            if (!string.IsNullOrEmpty(value))
            {
                foreach (string str in str_escape)
                {
                    value = value.Replace(str, escapechar + str);
                }
            }

            return value;
        }
    }
}
