using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.DataAccess.DA;
using CAS.Common;
using System.Data.SqlClient;
using System.Data;
using CAS.DataAccess.BaseDAModels;
using CAS.Entity;
using CAS.Entity.DBEntity;

namespace FxtCenterService.DataAccess
{
    public class SysCodePriceDA : BaseDA
    {
        /// <summary>
        /// 获取指定CODE影响价格的百分比
        /// </summary>
        /// <param name="cityid">城市ID</param>
        /// <param name="purposecode">用途Code（普通住宅：1002001）</param>
        /// <param name="code">直接指定CODE</param>
        /// <returns></returns>
        public static List<SysCodePrice> GetCodePriceList(int cityid, int purposecode, int[] code)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQL.SQLName.SysCode.CodePriceList;
            sql += " and cityid=" + cityid;
            sql += " and purposecode=" + purposecode;
            if (code != null && code.Length > 0)
            {
                sql += " and code in (-1";
                foreach (int item in code)
                {
                    sql += "," + item;
                }
                sql += ")";
            }
            return ExecuteToEntityList<SysCodePrice>(sql, System.Data.CommandType.Text, parameters);
        }

        /// <summary>
        /// 获取指定修正类型影响价格的百分比
        /// </summary>
        /// <param name="cityid">城市ID</param>
        /// <param name="purposecode">用途Code（普通住宅：1002001）</param>
        /// <param name="typecode">直接指定类型</param>
        /// <returns></returns>
        public static List<SysCodePrice> GetCodePriceList(int cityid, int purposecode, int typecode)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQL.SQLName.SysCode.CodePriceList;
            sql += " and cityid=" + cityid;
            sql += " and purposecode=" + purposecode;
            sql += " and typecode=" + typecode;
            return ExecuteToEntityList<SysCodePrice>(sql, System.Data.CommandType.Text, parameters);
        }

        /// <summary>
        /// 获取(楼层、装修)修正系数
        /// </summary>
        /// <param name="cityid">城市ID</param>
        /// <param name="purposecode">用途Code（普通住宅：1002001）</param>
        /// <param name="totalfloor">总楼层</param>
        /// <param name="floornumber">实际楼层</param>
        /// <param name="lv">装修档次</param>
        /// <param name="decorationprobabilit">装修成新率</param>
        /// <returns></returns>
        public static List<SysCodePrice> GetCodePriceList(int cityid, int purposecode, int totalfloor, int floornumber, int lv, int decorationprobabilit)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQL.SQLName.SysCode.CodePriceList;
            sql += " and cityid=" + cityid;
            sql += " and purposecode=" + purposecode;

            sql += " and (1=2";
            //楼层修正系数 1033003
            if (totalfloor > 0 || floornumber > 0)
            {
                sql += " or (typecode=1033003";
                sql += (totalfloor > 0) ? (" and code=" + totalfloor) : "";
                sql += (floornumber > 0) ? (" and subcode=" + floornumber) : "";
                sql += " )";
            }
            //装修修正系数 1033004
            if (lv > 0 || decorationprobabilit > 0)
            {
                sql += " or (typecode=1033004";
                sql += (totalfloor > 0) ? (" and code=" + lv) : "";
                sql += (floornumber > 0) ? (" and subcode=" + decorationprobabilit) : "";
                sql += " )";
            }
            sql += ")";
            return ExecuteToEntityList<SysCodePrice>(sql, CommandType.Text, parameters);
        }
    }
}