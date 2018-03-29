using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using FxtCommonLibrary.LibraryUtils;
using FxtNHibernate.DTODomain.APIActualizeDTO;
using System.Reflection;
using CAS.Common;

/**
 * 作者: 李晓东
 * 时间: 2013.12.03
 * 摘要: 新建 Utility 工具类
 * 2013.12.30 新增GetEntitesName(string tableName)方法 修改人:曾智磊
 * * 2014.2.20 新增GetApiInfo(string type,out string dllName,out string className)方法,Utility构造函数,ApiConfig静态表里,修
改人:曾智磊
 * 2014.06.11 新增DataLoan库中客户表、用户表变量
 * **/
namespace FxtService.Common
{
    public static class Utility
    {
        public static XmlDocument ApiConfig = new XmlDocument();
        #region 数组
        public static string[] filter = { "recordcount", "CustomPrimaryKeyIdentify", "IsSetCustomerFields" };
        #endregion
        #region 数据库
        /// <summary>
        /// FxtData数据库
        /// </summary>
        public const string DBFxtData = "FxtData";
        /// <summary>
        /// 贷后数据库
        /// </summary>
        public const string DBFxtLoan = "FxtLoan";
        /// <summary>
        /// 房讯通跑盘临时数据库
        /// </summary>
        public const string DBFxtTemp = "FxtTemp";
        #endregion

        #region DataLoan 库中的表

        /// <summary>
        /// DataLoan 库中 Data_Project临时表
        /// </summary>
        public const string loan_DataProject = "Data_Project";
        /// <summary>
        /// DataLoan 库中 Data_Building临时表
        /// </summary>
        public const string loan_DataBuilding = "Data_Building";
        /// <summary>
        /// DataLoan 库中 Data_House临时表
        /// </summary>
        public const string loan_DataHouse = "Data_House";
        /// <summary>
        /// DataLoan 库中 押品表
        /// </summary>
        public const string loan_Data_Collateral = "FxtLoan.dbo.Data_Collateral";
        /// <summary>
        /// DataLoan 库中 复估表
        /// </summary>
        public const string loan_Data_Data_Reassessment = "FxtLoan.dbo.Data_Reassessment";
        /// <summary>
        ///  DataLoan 库中 任务表(Sys_Task)
        /// </summary>
        public const string loand_Sys_Task = "Sys_Task";
        /// <summary>
        ///  DataLoan 库中 任务执行日志表(Sys_Task)
        /// </summary>
        public const string loand_Sys_TaskLog = "Sys_TaskLog";
        /// <summary>
        /// DataLoan 库中 文件与押品关联表
        /// </summary>
        public const string loand_Relation_File_Collateral = "Relation_File_Collateral";
        /// <summary>
        /// DataLoan 库中 客户表
        /// </summary>
        public const string loan_Sys_Customer = "Sys_Customer";
        /// <summary>
        /// DataLoan 库中 用户表
        /// </summary>
        public const string loan_Sys_User = "Sys_User";
        /// <summary>
        /// DataLoan 库中 文件项目表
        /// </summary>
        public const string loand_Sys_BankProject = "Sys_BankProject";
        /// <summary>
        /// 城市对应公司表
        /// </summary>
        public const string loand_SysCityFxtCompany = "Sys_CityFxtCompany";
        #endregion

        #region FxtProject 库中的表
        /// <summary>
        /// 省份表
        /// </summary>
        public const string SYSProvince = "SYS_Province";
        /// <summary>
        ///行政区表
        /// </summary>
        public const string SYSArea = "SYS_Area";
        /// <summary>
        /// 城市表
        /// </summary>
        public const string SYSCity = "SYS_City";
        /// <summary>
        /// 城市区域表
        /// </summary>
        public const string SYSCityTable = "SYS_City_Table";
        /// <summary>
        /// 楼盘网络名称表
        /// </summary>
        public const string SYSProjectMatch = "SYS_ProjectMatch";
        /// <summary>
        /// 楼盘均价表
        /// </summary>
        public const string DATProjectAvgPrice = "DAT_ProjectAvgPrice";
        /// <summary>
        /// Code表
        /// </summary>
        public const string SYSCode = "SYS_Code";
        /// <summary>
        /// 公司、银行、评估机构表
        /// </summary>
        public const string PriviCompany = "Privi_Company";

        /// <summary>
        /// 楼盘表信息
        /// </summary>
        public const string DATProject = "DAT_Project";
        public const string DATProjectcsj = "DAT_Project_csj";
        public const string DATProjecthbh = "DAT_Project_hbh";
        public const string DATProjectxb = "DAT_Project_xb";
        public const string DATProjectzb = "DAT_Project_zb";
        public const string DATProjectzsj = "DAT_Project_zsj";
        /// <summary>
        /// 每月自动估价表
        /// </summary>
        public const string DATAvgPriceMonth = "DAT_AvgPrice_Month";
        public const string SysFloorPrice = "Sys_FloorPrice";
        public const string SysModulusPrice = "Sys_ModulusPrice";
        /// <summary>
        /// 案例表信息
        /// </summary>
        public const string DATCase = "DAT_Case";
        public const string DATCasecsj = "DAT_Case_csj";
        public const string DATCasehbh = "DAT_Case_hbh";
        public const string DATCasexb = "DAT_Case_xb";
        public const string DATCasezb = "DAT_Case_zb";
        public const string DATCasezsj = "DAT_Case_zsj";

        public const string LNKPPhotosub = "LNK_P_Photo_sub";
        public const string LNKPPhoto = "LNK_P_Photo";
        public const string DATAvgPriceDay = "DAT_AvgPrice_Day";
        #endregion

        #region FxtData库中的表

        /// <summary>
        /// FxtData库中 文件表(SysUploadFile)
        /// </summary>
        public const string SysUploadFile = "Sys_UploadFile";
        /// <summary>
        /// FxtData库中 权限表(Sys_Purview)
        /// </summary>
        public const string SysPurview = "Sys_Purview";
        /// <summary>
        /// FxtData库中 用户产品表(Sys_UserProduct)
        /// </summary>
        public const string SysUserProduct = "Sys_UserProduct";
        /// <summary>
        /// FxtData库中 用户权限表(Sys_UserPurview)
        /// </summary>
        public const string SysUserPurview = "Sys_UserPurview";
        /// <summary>
        /// FxtData库中 菜单表(Sys_Menu)
        /// </summary>
        public const string SysMenu = "Sys_Menu";
        /// <summary>
        /// FxtData库中 产品菜单表(Sys_ProductMenu)
        /// </summary>
        public const string SysProductMenu = "Sys_ProductMenu";
        /// <summary>
        /// FxtData库中 菜单权限表(Sys_MenuPurview)
        /// </summary>
        public const string SysMenuPurview = "Sys_MenuPurview";
        /// <summary>
        /// FxtData库中 产品表(Sys_Product)
        /// </summary>
        public const string SysProduct = "Sys_Product";

        #endregion

        #region 用途
        /// <summary>
        /// 土地用途TypeID
        /// </summary>
        public const int CodeID_1 = 1001;
        /// <summary>
        /// 居住用途TypeID
        /// </summary>
        public const int CodeID_2 = 1002;
        /// <summary>
        /// 案例类型
        /// </summary>
        public const int CodeID_3 = 3001;
        /// <summary>
        /// 建筑结构
        /// </summary>
        public const int CodeID_4 = 2005;
        /// <summary>
        /// 建筑类型
        /// </summary>
        public const int CodeID_5 = 2003;
        /// <summary>
        /// 户型
        /// </summary>
        public const int CodeID_6 = 4001;
        /// <summary>
        /// 朝向
        /// </summary>
        public const int CodeID_7 = 2004;
        /// <summary>
        /// 币种
        /// </summary>
        public const int CodeID_8 = 2002;
        /// <summary>
        /// 装修
        /// </summary>
        public const int CodeID_9 = 6026;
        /// <summary>
        /// 楼盘均价面积段
        /// </summary>
        public const int CodeID_10 = 8006;
        #endregion

        static Utility()
        {
            ApiConfig.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MatchClass.xml"));
        }
        /// <summary>
        /// 反射动态创建对象
        /// </summary>
        /// <param name="dllName">程序集名称</param>
        /// <param name="nameSpaceName">命名空间.类名</param>
        /// <returns></returns>
        public static object LoadAssembly(string dllName, string nameSpaceName)
        {
            return System.Reflection.Assembly.Load(dllName)
                            .CreateInstance(nameSpaceName.Replace("_", "").Replace("dbo.", ""), false);
        }

        /// <summary>
        /// 实体动态设置值
        /// </summary>
        /// <param name="_objModel">实体</param>
        /// <param name="name">属性名称</param>
        /// <param name="value">值</param>
        public static void ModelSetValue(object objModel, string name, object value)
        {
            objModel.GetType().GetProperty(name).SetValue(objModel, value, null);
        }

        /// <summary>
        /// 获得某个实体的所有属性
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        public static string GetModelFields(Type modelType)
        {
            StringBuilder stringBuilder = new StringBuilder();
            var propertys = modelType.GetProperties();
            int i = 0;
            string[] filter = { "recordcount", "CustomPrimaryKeyIdentify", "IsSetCustomerFields" };
            do
            {
                if (Array.IndexOf(filter, propertys[i].Name) == -1)
                {
                    stringBuilder.AppendFormat("{0},", propertys[i].Name);
                }
                i++;
            } while (propertys.Count() > i);
            return stringBuilder.ToString().TrimEnd(',');
        }

        /// <summary>
        /// 获得实体键值对
        /// </summary>
        /// <param name="modelType">实体 Type</param>
        /// <param name="condition">条件符号 and(默认) or</param>
        /// <returns></returns>
        public static string GetModelFieldKeyValue(object model, string condition = "and")
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (model != null)
            {
                var propertys = model.GetType().GetProperties();
                int i = 0;
                do
                {
                    object value = propertys[i].GetValue(model, null);
                    value = value != null ? value : "";
                    if ((TypeIsEquals(propertys[i].PropertyType, typeof(int)) ||
                        TypeIsEquals(propertys[i].PropertyType, typeof(decimal)) ||
                        TypeIsEquals(propertys[i].PropertyType, typeof(double)) ||
                        TypeIsEquals(propertys[i].PropertyType, typeof(float))) &&
                        valueEmpty(propertys[i].PropertyType, value.ToString()))
                    {
                        if (propertys.Count() == i)
                            stringBuilder.AppendFormat("{0}={1} ", propertys[i].Name, value, condition);
                        else
                            stringBuilder.AppendFormat("{0}={1} {2} ", propertys[i].Name, value, condition);
                    }
                    else if (propertys[i].PropertyType == typeof(string) &&
                        valueEmpty(propertys[i].PropertyType, value.ToString()))
                    {
                        if (propertys.Count() == i)
                            stringBuilder.AppendFormat("{0}={1} ", propertys[i].Name, value, condition);
                        else
                            stringBuilder.AppendFormat("{0}='{1}' {2} ", propertys[i].Name, value, condition);
                    }
                    i++;
                } while (propertys.Count() > i);
            }
            stringBuilder.Append(" 1=1 ");
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 计算月份,默认上个月最后一天
        /// </summary>
        /// <param name="months">今天之前第几个月,默认上个月</param>
        /// <param name="format">转换格式,默认yyyy-MM-dd</param>
        /// <returns>返回格式化后的信息</returns>
        public static string GetDateTimeMoths(string datetime = null, int months = 0, string format = "yyyy-MM-dd")
        {
            DateTime lastMonth = DateTime.Now.AddMonths(-1);//得到上一个月10.5
            if (!string.IsNullOrEmpty(datetime))
                lastMonth = Convert.ToDateTime(datetime).AddMonths(-1);
            DateTime currentDateTime = lastMonth.AddDays(1 - lastMonth.Day).AddMonths(1)
                .AddDays(-1).AddMonths(months);
            return currentDateTime.ToString(format);
        }

        /// <summary>
        /// 获得SQL语句增删改
        /// </summary>
        /// <param name="modelType">实体类型</param>
        /// <param name="tablename">表名</param>
        /// <param name="cudtype">执行类型</param>
        /// <returns></returns>
        public static string GetMSSQL_CUDSQL(Type modelType, string tablename, string cudtype)
        {
            StringBuilder sb = new StringBuilder();
            cudtype = cudtype.ToLower();
            //新增
            if (cudtype.Equals("c"))
            {
                int i = 0;
                PropertyInfo[] info = modelType.GetProperties();
                sb.AppendFormat("Insert into {0} with(rowlock) ", tablename);
                StringBuilder sbColumn = new StringBuilder();
                StringBuilder sbValue = new StringBuilder();
                while (i < info.Length)
                {
                    string strName = info[i].Name;
                    if (i == info.Length - 1)
                    {
                        sbColumn.AppendFormat("{0}", strName);
                        sbValue.AppendFormat("@{0}", strName);
                    }
                    else
                    {
                        sbColumn.AppendFormat("{0},", strName);
                        sbValue.AppendFormat("@{0},", strName);
                    }
                    i++;
                }
                sb.AppendFormat(" ({0}) values ({1})", sbColumn.ToString(), sbValue.ToString());
            }
            else if (cudtype.Equals("u"))
            {
                sb.AppendFormat("Update {0} with(rowlock) set ", tablename);
            }
            else if (cudtype.Equals("d"))
            {
                sb.AppendFormat("Delete {0} where", tablename);
            }
            return sb.ToString().ToUpper();
        }
        /// <summary>
        /// 获得公共HSQL语句(HSQL)
        /// </summary>
        /// <param name="modelType">实体,TypeOf</param>
        /// <param name="tablename">表名</param>
        /// <param name="keyword">关键字,默认为空(top之类)</param>
        /// <returns></returns>
        public static string GetMSSQL_HSQL(Type modelType, string tablename = null)//, string keyword = null
        {
            if (string.IsNullOrEmpty(tablename))
                tablename = modelType.Name;
            //return string.Format("select {0} {1} from {2} where ", keyword, GetModelFields(modelType), GetEntitesName

            //(tablename));
            return string.Format("from {0} _tb where ", GetEntitesName(tablename));
        }

        /// <summary>
        /// HSQL
        /// </summary>
        /// <param name="tablename">表名</param>
        /// <returns></returns>
        public static string GetMSSQL_HSQL(string tablename)
        {
            tablename = tablename.Replace("_", "").Replace("dbo.", "");
            return string.Format("from {0} _tb where ", tablename);
        }
        /// <summary>
        /// 获得公共SQL语句(SQL)
        /// </summary>
        /// <param name="modelType">实体,TypeOf</param>
        /// <param name="tablename">表名</param>
        /// <param name="keyword">关键字,默认为空(top之类)</param>
        /// <returns></returns>
        public static string GetMSSQL_SQL(Type modelType, string tablename = null, string keyword = null, string keyword2 = null)
        {
            if (string.IsNullOrEmpty(tablename))
                tablename = modelType.Name;
            return string.Format("select {0} {1} from {2} as _tb {3} where ", keyword, GetModelFields(modelType), tablename, keyword2);
        }
        /// <summary>
        /// 获得公共SQL语句(SQL)
        /// </summary>
        /// <param name="modelType">实体,TypeOf</param>
        /// <param name="tablename">表名</param>
        /// <param name="keyword">关键字,默认为空(top之类)</param>
        /// <returns></returns>
        public static string GetMSSQL_SQL2(Type modelType, string tablename = null, string keyword = null, string keyword2 = null)
        {
            if (string.IsNullOrEmpty(tablename))
                tablename = modelType.Name;
            return string.Format("select * {0} from {1} as _tb {2} with(nolock) where ", keyword, tablename, keyword2);
        }
        public static string GetMSSQL_SQL_AVG(string column, string tablename)
        {
            return string.Format("select avg({0}) from {1} as _tb with(nolock) where ", column, tablename);
        }
        public static string GetMSSQL_SQL_COUNT(string tablename, string keyword = null)
        {
            return string.Format("select count(*) from {0} as _tb {1} with(nolock) where ", tablename, keyword);
        }
        /// <summary>
        /// SQL
        /// </summary>
        /// <param name="tablename">表名</param>
        /// <returns></returns>
        public static string GetMSSQL_SQL(string tablename)
        {
            return string.Format("from {0} _tb where ", tablename);
        }
        /// <summary>
        /// 获取实体名(用于HSQL)
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public static string GetEntitesName(string tableName)
        {
            return tableName.Replace("_", "").Replace("dbo.", "");
        }

        /// <summary>
        /// 替换相关字符
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string StringReplace(string str)
        {
            str = str.Replace("<", "");
            str = str.Replace("~", "");
            str = str.Replace(">", "");
            return str;
        }
        /// <summary>
        /// 根据数组和指定列获得Sql字符串
        /// </summary>
        /// <param name="array">数组</param>
        /// <param name="column">列名</param>
        /// <returns></returns>
        public static string GetArrayWhere(string[] array, string column, string convert = null)
        {
            StringBuilder sb = new StringBuilder();
            int arrayLen = array.Length;
            for (int i = 0; i < arrayLen; i++)
            {
                string[] strOneSplit = array[i].Split(new string[] { "&&" }, StringSplitOptions.None),
                         strSplit = null;
                if (!Utils.IsNullOrEmpty(strOneSplit[0]))
                {
                    if (strOneSplit[1].Equals("<"))
                    {
                        if (!Utils.IsNullOrEmpty(convert))
                        {
                            sb.AppendFormat("{0}<{1} or ", string.Format(convert, column), string.Format(convert, strOneSplit[0]));
                        }
                        else
                        {
                            sb.AppendFormat("{0}<{1} or ", column, strOneSplit[0]);
                        }
                    }
                    else if (strOneSplit[1].Equals("=="))
                    {
                        strSplit = strOneSplit[0].Split('~');
                        if (!Utils.IsNullOrEmpty(convert))
                        {
                            sb.AppendFormat("({0}>={1} and {0}<={2}) or ",
                                string.Format(convert, column),
                                string.Format(convert, strSplit[0]),
                                string.Format(convert, strSplit[1]));
                        }
                        else
                        {
                            sb.AppendFormat("({0}>={1} and {0}<={2}) or ", column, strSplit[0], strSplit[1]);
                        }
                    }
                    else if (strOneSplit[1].Equals(">"))
                    {
                        if (!Utils.IsNullOrEmpty(convert))
                        {
                            sb.AppendFormat("{0}>{1} or ", string.Format(convert, column), string.Format(convert, strOneSplit[0]));
                        }
                        else
                        {
                            sb.AppendFormat("{0}>{1} or ", column, strOneSplit[0]);
                        }
                    }
                    else
                    {
                        strSplit = strOneSplit[0].Split('~');
                        if (!Utils.IsNullOrEmpty(convert))
                        {
                            sb.AppendFormat("({0}>={1} and {0}<{2}) or ",
                                string.Format(convert, column),
                                string.Format(convert, strSplit[0]),
                                string.Format(convert, strSplit[1]));
                        }
                        else
                        {
                            sb.AppendFormat("({0}>={1} and {0}<{2}) or ", column, strSplit[0], strSplit[1]);
                        }
                    }
                }
            }
            string result = sb.ToString().Trim();
            return result.Substring(0, result.Length - 2);
        }
        /// <summary>
        /// 根据字符串获得Sql字符串
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="strings">字符串</param>
        /// <param name="index">下标</param>
        /// <param name="count">总数</param>
        /// <returns></returns>
        public static string GetArrayWhere(string column, string strings, int index, int count)
        {
            string result = string.Empty;
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[\u4E00-\u9FA5]");
            strings = reg.Replace(strings, "");
            if (index == 0)
            {
                if (column.Equals("LoanAmount"))
                    strings = (int.Parse(strings) * 10000).ToString();
                result = string.Format(" {0}<{1} ", column, strings);
            }
            else if (index == count - 2)
            {
                string[] arrTemp = strings.Split('~');
                if (column.Equals("LoanAmount"))
                {
                    arrTemp[0] = (int.Parse(arrTemp[0]) * 10000).ToString();
                    arrTemp[1] = (int.Parse(arrTemp[1]) * 10000).ToString();
                }
                result = string.Format(" ({0}>={1} and {0}<={2}) ", column, arrTemp[0], arrTemp[1]);
            }
            else if (index == count - 1)
            {
                if (column.Equals("LoanAmount"))
                    strings = (int.Parse(strings) * 10000).ToString();
                result = string.Format(" {0}>{1} ", column, strings);
            }
            else
            {
                string[] arrTemp = strings.Split('~');
                if (column.Equals("LoanAmount"))
                {
                    arrTemp[0] = (int.Parse(arrTemp[0]) * 10000).ToString();
                    arrTemp[1] = (int.Parse(arrTemp[1]) * 10000).ToString();
                }
                result = string.Format(" ({0}>={1} and {0}<{2}) ", column, arrTemp[0], arrTemp[1]);
            }
            return result;
        }

        static bool valueEmpty(Type t, string value)
        {
            bool o = false;
            string strName = t.Name;
            if (t.Name == "Nullable`1")
            {
                strName = t.GetGenericArguments()[0].Name;
            }
            switch (strName.Trim())
            {
                case "Decimal":
                    o = decimal.Parse(value) > 0;
                    break;
                case "Int32":
                    o = int.Parse(value) > 0;
                    break;
                case "Float":
                    o = float.Parse(value) > 0;
                    break;
                default:
                    o = !string.IsNullOrEmpty(value);
                    break;
            }
            return o;
        }

        static bool TypeIsEquals(Type type1, Type type2)
        {
            if (type1.Name.Equals("Nullable`1"))
            {
                return type1.GetGenericArguments()[0] == type2;
            }
            else
            {
                return type1 == type2;
            }
        }



        public static object valueType(Type t, object value)
        {
            string strName = t.Name;
            if (t.Name == "Nullable`1")
            {
                strName = t.GetGenericArguments()[0].Name;
            }
            if (value == null)
            {
                return null;
            }
            switch (strName.Trim())
            {
                case "Decimal":
                    value = Convert.ToDecimal(value);
                    break;
                case "Int32":
                    value = Convert.ToInt32(value);
                    break;
                case "Int64":
                    value = Convert.ToInt64(value);
                    break;
                case "Float":
                    value = float.Parse(Convert.ToString(value));
                    break;
                case "DateTime":
                    value = Convert.ToDateTime(value);
                    break;
                case "Double":
                    value = Convert.ToDouble(value);
                    break;
                case "Bool":
                    value = Convert.ToBoolean(value);
                    break;
                case "String":
                    value = Convert.ToString(value);
                    break;
                case "Array":
                    value = (Array)value;
                    break;
                default:
                    value = value as object;
                    break;
            }
            return value;
        }

        /// <summary>
        /// 月份转英文
        /// </summary>
        /// <param name="month">月份数</param>
        /// <param name="other">其他数字</param>
        /// <returns></returns>
        public static string MonthToEnglish(int month, int other)
        {
            string flag = string.Empty;
            switch (month)
            {
                case 1:
                    flag = "January";
                    break;
                case 2:
                    flag = "February";
                    break;
                case 3:
                    flag = "March";
                    break;
                case 4:
                    flag = "April";
                    break;
                case 5:
                    flag = "May";
                    break;
                case 6:
                    flag = "June";
                    break;
                case 7:
                    flag = "July";
                    break;
                case 8:
                    flag = "August";
                    break;
                case 9:
                    flag = "September";
                    break;
                case 10:
                    flag = "October";
                    break;
                case 11:
                    flag = "November";
                    break;
                case 12:
                    flag = "December";
                    break;
            }
            flag = string.Format("{0}{1}", flag, other);
            return flag;
        }

        /// <summary>
        /// 获得数组中的值组装成1,2,3,4结果
        /// </summary>
        /// <param name="array">数组</param>
        /// <returns></returns>
        public static string GetArrayString(int[] array)
        {
            int i = 0, len = array.Length;
            StringBuilder sbsql = new StringBuilder();
            while (i < len)
            {
                if (i == len - 1)
                    sbsql.AppendFormat("{0}", array[i]);
                else
                    sbsql.AppendFormat("{0},", array[i]);
                i++;
            }
            return sbsql.ToString();
        }

        /// <summary>
        /// 统一返回JSON
        /// </summary>
        /// <param name="type">类型:0 失败 1 成功</param>
        /// <param name="message">消息</param>
        /// <param name="data">数据</param>
        /// <param name="count">数据总和</param>
        /// <returns></returns>
        public static string GetJson(int type, string message, object data = null, int count = 0)
        {
            var json = new
            {
                type = type,
                message = Utils.IsNullOrEmpty(message) ? "失败" : message,
                data = data,
                count = count
            };
            return Utils.Serialize(json);
        }
        public static ResultData GetObjJson(int type, string message, object data = null, int count = 0)
        {
            return new ResultData
            {
                Type = type,
                Message = Utils.IsNullOrEmpty(message) ? "失败" : message,
                Data = data != null ? Utils.Serialize(data) : "",
                Count = count
            };
        }



    }


    /// <summary>
    /// 分页类
    /// </summary>
    public class UtilityPager
    {

        public UtilityPager(int pageSize = 10, int pageIndex = 1, bool isGetCount = true)
        {
            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            this.IsGetCount = isGetCount;
        }
        /// <summary>
        /// 每页显示条数
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 第几页
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 总个数
        /// </summary>
        public int Count { get; set; }
        private bool isGetCount = true;
        /// <summary>
        /// 是否获取总个数
        /// </summary>
        public bool IsGetCount
        {
            get { return isGetCount; }
            set { isGetCount = value; }
        }
    }
}

