using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Reflection;
using System.Web.Script.Serialization;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Models;
using Kingsun.SynchronousStudy.Common.Base;


/// <summary>
/// IndustryDAL 的摘要说明
/// </summary>
public class ModularManageDAL : BaseManagement
{
    /// <summary>
    /// 获取模块列表
    /// </summary>
    /// <returns></returns>
    public List<TB_ModularManage> GetModularList()
    {
        DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "SELECT ID,ModularID,ModularName,SuperiorID,Level,State FROM  TB_ModularManage WHERE State=1");
        List<TB_ModularManage> list = new List<TB_ModularManage>();

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            list = DataSetToIList<TB_ModularManage>(ds, 0);
        }
        return list;
    }

    /// <summary>
    /// 根据条件获取模块列表
    /// </summary>
    /// <param name="where"></param>
    /// <returns></returns>
    public IList<TB_ModularManage> GetModuleList(string where)
    {
        IList<TB_ModularManage> list = Search<TB_ModularManage>(where);
        return list;
    }

    /// <summary> 
    /// DataSet装换为泛型集合 
    /// </summary> 
    /// <typeparam name="T"></typeparam> 
    /// <param name="ds">DataSet</param> 
    /// <param name="tableIndex">待转换数据表索引</param> 
    /// <returns></returns> 
    public static List<TB_ModularManage> DataSetToIList<T>(DataSet ds, int tableIndex)
    {
        if (ds == null || ds.Tables.Count < 0)
            return null;
        if (tableIndex > ds.Tables.Count - 1)
            return null;
        if (tableIndex < 0)
            tableIndex = 0;

        DataTable p_Data = ds.Tables[tableIndex];
        // 返回值初始化 
        List<TB_ModularManage> result = new List<TB_ModularManage>();
        for (int j = 0; j < p_Data.Rows.Count; j++)
        {
            TB_ModularManage _t = (TB_ModularManage)Activator.CreateInstance(typeof(TB_ModularManage));
            PropertyInfo[] propertys = _t.GetType().GetProperties();
            foreach (PropertyInfo pi in propertys)
            {
                for (int i = 0; i < p_Data.Columns.Count; i++)
                {
                    // 属性与字段名称一致的进行赋值 
                    if (pi.Name.Equals(p_Data.Columns[i].ColumnName))
                    {
                        // 数据库NULL值单独处理 
                        if (p_Data.Rows[j][i] != DBNull.Value)
                            pi.SetValue(_t, p_Data.Rows[j][i], null);
                        else
                            pi.SetValue(_t, null, null);
                        break;
                    }
                }
            }
            result.Add(_t);
        }
        return result;
    }
}