using CBSS.Cfgmanager.Contract.ViewModel;
using CBSS.Framework.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBSS.Cfgmanager.Contract;
using System.Xml.Linq;
using System.Data;
using System.Transactions;
using CBSS.Core.Utility;
using CBSS.Cfgmanager.IBLL;

namespace CBSS.Cfgmanager.BLL
{
    public partial class CfgmanagerService : ICfgmanagerService
    {
        public List<DbEntity> GetDBList()
        {
            string xmlPath = System.Web.HttpContext.Current.Server.MapPath("~/Config/DaoConfig.xml");
            XDocument xdc = new XDocument(XDocument.Load(xmlPath));

            var pElement = xdc.Elements().First();
            List<DbEntity> dbs = new List<DbEntity>();
            foreach (var n in pElement.Elements())
            {
                dbs.Add(new DbEntity { DbName = n.Name.LocalName, ConnectString = n.Value });
            }
            return dbs;
        }


        public List<DataBaseTableFieldEntity> GetTableFiledList(string dbName, string tableName)
        {
            Repository repository = new Repository(dbName);
            if (dbName != null)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat(@"select *,precision= ISNULL((SELECT NUMERIC_SCALE FROM information_schema.columns A where  A.Table_Name ='{0}' and A.COLUMN_NAME=t.[column]),0)
from(
SELECT
a.colorder as [number]  ,
 a.name as [column] ,
 b.name as [datatype],
 COLUMNPROPERTY(a.id, a.name, 'PRECISION') as [length] , 
 CASE WHEN COLUMNPROPERTY(a.id, a.name, 'IsIdentity') = 1 THEN '1' ELSE '' END as  [identity] ,
 CASE WHEN EXISTS ( SELECT 1 FROM sysobjects WHERE xtype = 'PK' AND parent_obj = a.id AND name IN ( SELECT name FROM sysindexes WHERE indid IN ( SELECT indid FROM sysindexkeys WHERE id = a.id AND colid = a.colid ) ) ) THEN '1' ELSE '' END as [key] ,
 CASE WHEN a.isnullable = 1 THEN '1' ELSE '' END as [isnullable] , 
 ISNULL(e.text, '') as [defaults] ,
 ISNULL(g.[value], a.name) as  [remark] 
                                FROM syscolumns a LEFT JOIN systypes b ON a.xusertype = b.xusertype INNER JOIN sysobjects d ON a.id = d.id AND d.xtype = 'U' AND d.name <> 'dtproperties' LEFT JOIN syscomments e ON a.cdefault = e.id LEFT JOIN sys.extended_properties g ON a.id = g.major_id AND a.colid = g.minor_id LEFT JOIN sys.extended_properties f ON d.id = f.major_id AND f.minor_id = 0
                               
                                WHERE d.name = '{0}'
                               ) t  ", tableName);

                var dt = repository.CustomEntitySelect<DataBaseTableFieldEntity>(strSql.ToString().Replace("\r", " ").Replace("\n", " "));

                return dt.ToList();
            }
            return null;
        }

        public List<DataBaseTableEntity> GetTableList(string dbName, string tableName)
        {
            Repository repository = new Repository(dbName);

            if (dbName != null)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"DECLARE @TableInfo TABLE ( name VARCHAR(50) , sumrows VARCHAR(11) , reserved VARCHAR(50) , data VARCHAR(50) , index_size VARCHAR(50) , unused VARCHAR(50) , pk VARCHAR(50) )
                            DECLARE @TableName TABLE ( name VARCHAR(50) )
                            DECLARE @name VARCHAR(50)
                            DECLARE @pk VARCHAR(50)
                            INSERT INTO @TableName ( name ) SELECT o.name FROM sysobjects o , sysindexes i WHERE o.id = i.id AND o.Xtype = 'U' AND i.indid < 2 ORDER BY i.rows DESC , o.name
                            WHILE EXISTS ( SELECT 1 FROM @TableName ) BEGIN SELECT TOP 1 @name = name FROM @TableName DELETE @TableName WHERE name = @name DECLARE @objectid INT SET @objectid = OBJECT_ID(@name) SELECT @pk = COL_NAME(@objectid, colid) FROM sysobjects AS o INNER JOIN sysindexes AS i ON i.name = o.name INNER JOIN sysindexkeys AS k ON k.indid = i.indid WHERE o.xtype = 'PK' AND parent_obj = @objectid AND k.id = @objectid INSERT INTO @TableInfo ( name , sumrows , reserved , data , index_size , unused ) EXEC sys.sp_spaceused @name UPDATE @TableInfo SET pk = @pk WHERE name = @name END
                            SELECT F.name , F.reserved , F.data , F.index_size , RTRIM(F.sumrows) AS sumrows , F.unused , ISNULL(p.tdescription, f.name) AS tdescription , F.pk
                            FROM @TableInfo F LEFT JOIN ( SELECT name = CASE WHEN A.COLORDER = 1 THEN D.NAME ELSE '' END , tdescription = CASE WHEN A.COLORDER = 1 THEN ISNULL(F.VALUE, '') ELSE '' END FROM SYSCOLUMNS A LEFT JOIN SYSTYPES B ON A.XUSERTYPE = B.XUSERTYPE INNER JOIN SYSOBJECTS D ON A.ID = D.ID AND D.XTYPE = 'U' AND D.NAME <> 'DTPROPERTIES' LEFT JOIN sys.extended_properties F ON D.ID = F.major_id WHERE a.COLORDER = 1 AND F.minor_id = 0 ) P ON F.name = p.name
                            WHERE 1 = 1 ");
                if (!string.IsNullOrEmpty(tableName))
                {
                    strSql.Append(" AND f.name like'%" + tableName + "%'");
                }
                strSql.Append(" ORDER BY f.name");
                //   return this.BaseRepository(dataBaseLinkEntity.DbConnection).FindTable(strSql.ToString());

                var dt = repository.CustomEntitySelect<DataBaseTableEntity>(strSql.ToString().Replace("\r", " ").Replace("\n", " "));

                //var list = DataTableHelper<DataBaseTableEntity>.ConvertToModel(dt);
                return dt.ToList();
            }
            return null;
        }

        public bool AlterTableColumn(string dbName, string tableName, string fieldJson)
        {

            Repository repository = new Repository(dbName);
            var field = fieldJson.ToObject<DataBaseTableFieldEntity>();
            string alterSql = "";
            //先去掉约束
            try
            {
                var dts = repository.SelectDataSet("exec  sp_helpconstraint [" + tableName + "]").Tables;
                if (dts.Count > 1)
                {
                    foreach (DataRow row in dts[1].Rows)
                    {
                        if (row["constraint_type"].ToString().ToLower() == ("DEFAULT on column " + field.oldcolumn).ToLower())
                        {
                            alterSql = string.Format("alter table [{0}] drop [{1}]", tableName, row["constraint_name"].ToString());
                            repository.SelectString(alterSql);
                        }
                    }
                }
                //alterSql = string.Format("alter table {0} drop constraint DF_{0}_{1} ", tableName, field.column);//  ------说明：删除表的字段的原有约束

            }
            catch
            {
            }
            finally
            {
                alterSql = "";
            }

            var keyColunmDs = repository.SelectDataSet(string.Format(@"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE 
WHERE TABLE_NAME = '{0}'",tableName));
            var keyColumn = keyColunmDs.Tables[0].Rows.Count > 0 ? keyColunmDs.Tables[0].Rows[0][0] : "";

            if (keyColumn == field.oldcolumn) {

            }

            if (field.oldcolumn != field.column)
            {
                string sql = string.Format("EXEC sp_rename '{0}.{1}', '{2}', 'COLUMN' ", tableName, field.oldcolumn, field.column);
                repository.SelectString(sql);
            }
            using (TransactionScope scope = new TransactionScope())
            {

                if (!string.IsNullOrEmpty(field.datatype)) //alter table 表名 alter column 列名 decimal(18, 4) not null 
                {
                    if (field.datatype.Contains("char"))
                    {
                        if (field.datatype.Contains("(max)"))
                        {
                            alterSql = string.Format("alter table [{0}] alter column [{1}] {2} ", tableName, field.column, field.datatype);
                        }
                        else
                        {
                            alterSql = string.Format("alter table [{0}] alter column [{1}] {2}({3}) ", tableName, field.column, field.datatype, field.length);
                        }

                    }
                    else if (field.datatype.Contains("decimal"))
                    {
                        alterSql = string.Format("alter table [{0}] alter column [{1}] {2}({3}, {4})  ", tableName, field.column, field.datatype, field.length, field.precision);
                    }
                    else
                    {
                        alterSql = string.Format("alter table [{0}] alter column [{1}] {2} ", tableName, field.column, field.datatype);
                    }

                    if (Convert.ToBoolean(field.isnullable))
                    {
                        alterSql += " null";
                    }
                    else
                    {
                        alterSql += " not null";
                    }
                    repository.SelectString(alterSql);
                }
                if (!string.IsNullOrEmpty(field.defaults))
                {
                    alterSql = string.Format("alter table [{0}] add constraint  DF_{0}_{1} DEFAULT '{2}' for {1}", tableName, field.column, field.defaults);  //alter table 表名 add constraint 约束名字 DEFAULT 默认值 for 字段名称 -------说明：添加一个表的字段的约束并指定默认值"
                    repository.SelectString(alterSql);
                }
                if (Convert.ToBoolean(field.key))
                {
                    alterSql = string.Format(@"ALTER TABLE {0} ADD CONSTRAINT
                         PK_{0} PRIMARY KEY CLUSTERED 
                         (
                         {1}
                         ) ON [PRIMARY];", tableName, field.column);  // 设置主键
                    repository.SelectString(alterSql);
                }
                if (Convert.ToBoolean(field.identity))//标识列
                {

                }
                field.remark = field.remark ?? "-";
                //   EXEC sp_updateextendedproperty 'MS_Description','字段1','user',dbo,'table','表','column',a1
                try
                {
                    alterSql = string.Format("EXEC sp_updateextendedproperty 'MS_Description','{0}','user',dbo,'table','{1}','column',{2}", field.remark, tableName, field.column);
                    repository.SelectString(alterSql);
                }
                catch
                {
                    alterSql = string.Format("EXECUTE sp_addextendedproperty N'MS_Description', '{0}', N'user', N'dbo', N'table', N'{1}', N'column', N'{2}'", field.remark, tableName, field.column);
                    repository.SelectString(alterSql);
                }


                scope.Complete();
                return true;
            }
        }


        public bool AddTableColumn(string dbName, string tableName, string fieldJson)
        {

            Repository repository = new Repository(dbName);
            var field = fieldJson.ToObject<DataBaseTableFieldEntity>();
            string alterSql = "";
            //先去掉约束
            try
            {
                var dts = repository.SelectDataSet("exec  sp_helpconstraint [" + tableName + "]").Tables;
                if (dts.Count > 1)
                {
                    foreach (DataRow row in dts[1].Rows)
                    {
                        if (row["constraint_type"].ToString().ToLower() == ("DEFAULT on column " + field.oldcolumn).ToLower())
                        {
                            alterSql = string.Format("alter table [{0}] drop {1}", tableName, row["constraint_name"].ToString());
                            repository.SelectString(alterSql);
                        }
                    }
                }
            }
            catch
            {
            }
            finally
            {
                alterSql = "";
            }
            using (TransactionScope scope = new TransactionScope())
            {
                if (!string.IsNullOrEmpty(field.datatype)) //alter table 表名 alter column 列名 decimal(18, 4) not null 
                {
                    if (field.datatype.Contains("char"))
                    {
                        if (field.datatype.Contains("max"))
                        {
                            alterSql = string.Format("alter table [{0}] add  [{1}] {2} ", tableName, field.column, field.datatype);
                        }
                        else
                        {
                            alterSql = string.Format("alter table [{0}] add  [{1}] {2}({3}) ", tableName, field.column, field.datatype, field.length);
                        }

                    }
                    else if (field.datatype.Contains("decimal") || field.datatype.Contains("numeric"))
                    {
                        alterSql = string.Format("alter table [{0}] add  [{1}] {2}({3}, {4})  ", tableName, field.column, field.datatype, field.length, field.precision);
                    }
                    else
                    {
                        alterSql = string.Format("alter table [{0}] add  [{1}] {2} ", tableName, field.column, field.datatype);
                    }
                    if (Convert.ToBoolean(field.identity))//标识列肯定非空
                    {
                        field.defaults = "";//标识列不需要默认值
                        alterSql += " IDENTITY";
                    }
                    else if (Convert.ToBoolean(field.isnullable))
                    {
                        alterSql += " null";
                    }
                    else
                    {
                        alterSql += " not null";
                    }
                    if (!string.IsNullOrEmpty(field.defaults))
                    {
                        alterSql += string.Format(" default ('{0}')", field.defaults);
                    }
                    repository.SelectString(alterSql);
                }
                //if (!string.IsNullOrEmpty(field.defaults))
                //{
                //    alterSql = string.Format("alter table {0} add  constraint DF_{0}_{1} DEFAULT '{2}' for {1}", tableName, field.column, field.defaults);  //alter table 表名 add constraint 约束名字 DEFAULT 默认值 for 字段名称 -------说明：添加一个表的字段的约束并指定默认值"
                //    repository.SelectString(alterSql);
                //}
                if (Convert.ToBoolean(field.key))
                {
                    alterSql = string.Format("alter table [{0}] add constraint [{1}] primary key([{1}])", tableName, field.column);  // alter table test add constraint ID primary key(id)
                    repository.SelectString(alterSql);
                }
                if (Convert.ToBoolean(field.identity))//标识列
                {

                }
                field.remark = field.remark ?? "-";

                alterSql = string.Format("EXECUTE sp_addextendedproperty N'MS_Description', '{0}', N'user', N'dbo', N'table', N'{1}', N'column', N'{2}'", field.remark, tableName, field.column);
                repository.SelectString(alterSql);

                scope.Complete();
                return true;
            }
        }


        public bool AddTable(string dbName, string tableName, string des)
        {
            string createTable = string.Format(@"CREATE TABLE [{0}](
	[ID] varchar  NULL
) ;", tableName);

            string addDes = string.Format(@"EXEC sp_addextendedproperty 
    'MS_Description', 
    '{0}', 
    'user', 
     dbo, 
    'table', 
     {1}", des, tableName);
            using (TransactionScope scope = new TransactionScope())
            {
                Repository repository = new Repository(dbName);
                repository.SelectString(createTable + addDes);
                scope.Complete();
            }
            return true;
        }

        public bool DropTable(string dbName, string tableName)
        {
            string sql = string.Format("DROP TABLE [{0}]", tableName);
            Repository repository = new Repository(dbName);
            var r = repository.SelectString(sql);
            return true;
        }

        public bool DropField(string dbName, string tableName, string field)
        {
            //            删除有默认值的列：先删除约束（默认值）alter table Test DROP CONSTRAINT DF__Test__BazaarType__3C4ACB5F，然后在删除列
            //alter table Test DROP COLUMN BazaarType
            Repository repository = new Repository(dbName);
            var dts = repository.SelectDataSet("exec  sp_helpconstraint [" + tableName + "]").Tables;
            if (dts.Count > 1)
            {
                foreach (DataRow row in dts[1].Rows)
                {
                    if (row["constraint_type"].ToString().ToLower() == ("DEFAULT on column " + field).ToLower())
                    {
                        var alterSql = string.Format("alter table [{0}] drop [{1}]", tableName, row["constraint_name"].ToString());
                        repository.SelectString(alterSql);
                    }
                }
            }

            string dropConstr = string.Format("alter table [{0}] DROP COLUMN [{1}]", tableName, field);

            repository.SelectString(dropConstr);
            return true;
        }
    }
}
