using System;
using System.Configuration;
namespace SqlTCP
{
	/// <summary>
	/// pubFunc 的摘要说明。
	/// </summary>
	public class pubFunc
	{
		public pubFunc()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}

        public static System.Data.DataTable CurrentTable = null;
        public static string Str_Enter = "\n";
        public static string Str_Tab = "\t";
        public static string Str_Tab2 = "\t\t";
        public static string Str_Tab3 = "\t\t\t";
        public static string Str_Tab4 = "\t\t\t\t";
        public static string Str_Tab5 = "\t\t\t\t\t";
        public static string Str_Tab6 = "\t\t\t\t\t\t";
        public static string GetFieldType(string o)
        {
            string sType = o;
            switch (o)
            {
                case "nchar":
                case "char":
                case "varchar":
                case "nvarchar":
                case "text":
                case "ntext":
                    sType = "string";
                    break;
                case "real":
                case "float":
                    sType = "double";
                    break;
                case "bigint":
                    sType = "long";
                    break;
                case "smallint":
                    sType = "int";
                    break;
                case "tinyint":
                    sType = "byte";
                    break;
                case "smalldatetime":
                case "datetime":
                    sType = "DateTime";
                    break;
                case "money":
                case "smallmoney":
                case "numeric":
                    sType = "decimal";
                    break;
                case "bit":
                    sType = "bool";
                    break;

            }
            return sType;	
        }

        public static string GetFieldValue(string type, string o)
        {
            string rtn = o;
            switch (type)
            {
                case "int":
                case "long":
                case "double":                    
                    rtn = o.Replace("((", "").Replace("))", "");
                    break;
                case "decimal":
                    rtn = o.Replace("((", "").Replace("))", "") + "M";
                    break;
                case "DateTime":
                    if(o.Contains("getdate()")) rtn = "DateTime.Now";
                    else o="";
                    break;
                case "string":
                    rtn = o.Replace("(", "").Replace(")", "").Replace("'","\"");
                    break;
                case "bool":
                    rtn = o == "((0))" ? "false" : "true";
                    break;
            }
            return rtn;
        }

		//用户表列表所在的表
		public static readonly string UserTable="UserTableList";
		
		//备份路径
		public static string BackupPath
		{
			get{
				string path=System.Windows.Forms.Application.StartupPath + ConfigurationSettings.AppSettings["BackupPath"];
				if(!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);
				return path;
			}
		}

		//文档路径
		public static string DocPath
		{
			get
			{
				string path=System.Windows.Forms.Application.StartupPath + ConfigurationSettings.AppSettings["DocPath"];
				if(!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);
				return path;
			}
		}

        public static string DefaultDB = ConfigurationSettings.AppSettings["database"];

		public static string OleDBConnectionString
		{
			get{
                return string.Format("OLEDB;Provider=sqloledb;Data Source={0};database={1};uid={2};password={3}"
                    , ConfigurationSettings.AppSettings["server"]
                    , DefaultDB
                    , ConfigurationSettings.AppSettings["user"]
                    , ConfigurationSettings.AppSettings["pwd"]);         
                
                //return ConfigurationSettings.AppSettings["Db_Conn_OleString"];
            }
		}

		public static string DBConnectionString
		{
			get{
                //return ConfigurationSettings.AppSettings["Db_Conn_String"];
                return string.Format("server={0};database={1};uid={2};pwd={3};"
                    , ConfigurationSettings.AppSettings["server"]
                    , DefaultDB
                    , ConfigurationSettings.AppSettings["user"]
                    , ConfigurationSettings.AppSettings["pwd"]);                
            }
		}

		
	}
}
