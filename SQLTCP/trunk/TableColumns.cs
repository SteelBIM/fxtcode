using System;
using System.Data;
using System.Data.SqlClient;
namespace SqlTCP
{
	/// <summary>
	/// TableColumns 的摘要说明。
	/// </summary>
	public class TableColumns
	{
		public TableColumns()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}

		//获取表结构
		public static  DataSet  Exec_P_TableColumnProperty(string _tablename)
		{
			try
			{
				string sql=GetTableColumnString(_tablename);
				DataSet resultValue;
				resultValue= SqlHelper.ExecuteDataset(pubFunc.DBConnectionString,CommandType.Text,sql);
				return resultValue;
			}
			catch(Exception ex)
			{
				throw ex ;
			}
		}

		//获取表结构的SQL语句
		public static  string  GetTableColumnString(string _tablename)
		{
			try
			{
				//表名=case when a.colorder=1 then d.name else '' end,占用字节数=a.length,小数位数=isnull(COLUMNPROPERTY(a.id,a.name,'Scale'),0),
				//string sql="SELECT   字段序号=a.colorder, 字段名=a.name,  类型=b.name,    长度=COLUMNPROPERTY(a.id,a.name,'PRECISION'),   允许空=case when a.isnullable=1 then '√'else '' end,  默认值=isnull(e.text,''),  字段说明=isnull(g.[value],'') FROM syscolumns a    left join systypes b on a.xtype=b.xusertype    inner join sysobjects d on a.id=d.id  and d.xtype='U' and  d.name<>'dtproperties'    left join syscomments e on a.cdefault=e.id left join sysproperties g on a.id=g.id and a.colid=g.smallid  where d.name='" + _tablename + "'  order by a.id,a.colorder";
                string sql = "SELECT chk=convert(bit,1),  字段序号=a.colorder, 字段名=a.name,  类型=b.name,    长度=case when COLUMNPROPERTY(a.id,a.name,'PRECISION') <0 then '' else  COLUMNPROPERTY(a.id,a.name,'PRECISION') end,   允许空=case when a.isnullable=1 then '√'else '' end,主键=case when a.name=pk.col then '√' else '' end, (case  when COLUMNPROPERTY( a.id,a.name, 'IsIdentity' )=1 then  '√' else   ''  end)  as  标识,  默认值=isnull(e.text,''),  字段说明=isnull(g.[value],'') FROM syscolumns a    left join systypes b on a.xtype=b.xusertype    inner join sysobjects d on a.id=d.id  and d.xtype='U' and  d.name<>'dtproperties'    left join syscomments e on a.cdefault=e.id left join sys.extended_properties  g on a.id=g.major_id and a.colid=g.minor_id ";
                sql += @" left join (select col = convert(sysname,c.name)               
from                                                       
sysindexes i, syscolumns c, sysobjects o                   
where o.id = object_id('" + _tablename + @"')                  
and o.id = c.id                                            
and o.id = i.id                                            
and (i.status & 0x800) = 0x800                             
and (c.name = index_col ('" + _tablename + @"', i.indid,  1) or     
     c.name = index_col ('" + _tablename + @"', i.indid,  2) or     
     c.name = index_col ('" + _tablename + @"', i.indid,  3) or     
     c.name = index_col ('" + _tablename + @"', i.indid,  4) or     
     c.name = index_col ('" + _tablename + @"', i.indid,  5) or     
     c.name = index_col ('" + _tablename + @"', i.indid,  6) or     
     c.name = index_col ('" + _tablename + @"', i.indid,  7) or     
     c.name = index_col ('" + _tablename + @"', i.indid,  8) or     
     c.name = index_col ('" + _tablename + @"', i.indid,  9) or     
     c.name = index_col ('" + _tablename + @"', i.indid, 10) or     
     c.name = index_col ('" + _tablename + @"', i.indid, 11) or     
     c.name = index_col ('" + _tablename + @"', i.indid, 12) or     
     c.name = index_col ('" + _tablename + @"', i.indid, 13) or     
     c.name = index_col ('" + _tablename + @"', i.indid, 14) or     
     c.name = index_col ('" + _tablename + @"', i.indid, 15) or     
     c.name = index_col ('" + _tablename + @"', i.indid, 16)       
     ) ) pk on a.name=pk.col";
                sql +=" where d.name='" + _tablename + "'  order by a.id,a.colorder";
				return sql;
			}
			catch(Exception ex)
			{
				throw ex ;
			}
		}

		//获取表结构
		public static  DataSet  Exec_P_TableColumnBackupProperty(string _tablename)
		{
			try
			{
				string sql=GetTableColumnBackupString(_tablename);
				DataSet resultValue;
				resultValue= SqlHelper.ExecuteDataset(pubFunc.DBConnectionString,CommandType.Text,sql);
				return resultValue;
			}
			catch(Exception ex)
			{
				throw ex ;
			}
		}
		//获取表结构备份的SQL语句
		public static  string  GetTableColumnBackupString(string _tablename)
		{
			try
			{
				string sql=string.Format("SELECT '{0}' as 表名, objname as 字段名,value as 描述 FROM ::FN_LISTEXTENDEDPROPERTY('MS_Description', 'User','dbo','table','{0}', 'column',default)",_tablename);
				return sql;
			}
			catch(Exception ex)
			{
				throw ex ;
			}
		}

        public static DataSet Exec_P_DataBaseList()
        {
            try
            {
                //string sql="select b.remark,a.name from sysobjects a left outer join usertablelist b on a.name=b.tname where a.xtype='U' and a.status>0 and a.name<>'"+ pubFunc.UserTable +"' order by a.name";
                string sql = "select [name] from master.dbo.sysdatabases Order By [Name]";
                DataSet resultValue;
                resultValue = SqlHelper.ExecuteDataset(pubFunc.DBConnectionString, CommandType.Text, sql);
                return resultValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


		//获取表列表
		public static  DataSet  Exec_P_TableList()
		{
			try
			{
				//string sql="select b.remark,a.name from sysobjects a left outer join usertablelist b on a.name=b.tname where a.xtype='U' and a.status>0 and a.name<>'"+ pubFunc.UserTable +"' order by a.name";
				string sql="select a.name from sysobjects a where a.xtype='U' order by a.name";
				DataSet resultValue;
				resultValue= SqlHelper.ExecuteDataset(pubFunc.DBConnectionString,CommandType.Text,sql);
				return resultValue;
			}
			catch(Exception ex)
			{
				throw ex ;
			}
		}

		//更新字段描述
		public static bool UpdateColumn(string _tablename ,string _colname,string _colvalue)
		{
			string sql="";
			try
			{
				if(CheckColumnDescExists(_tablename,_colname))
				{
					if(_colvalue!="")
						sql=string.Format("exec sp_updateextendedproperty 'MS_Description','{0}','user','dbo','table','{1}','column','{2}'",_colvalue,_tablename,_colname);
					else
						sql=string.Format("exec sp_dropextendedproperty 'MS_Description','user','dbo','table','{0}','column','{1}'",_tablename,_colname);
				}
				else
				{
					if(_colvalue!="")
						sql=string.Format("exec sp_addextendedproperty 'MS_Description','{0}','user','dbo','table','{1}','column','{2}'",_colvalue,_tablename,_colname);
				}
				if(sql=="") return true;
				int suc=SqlHelper.ExecuteNonQuery(pubFunc.DBConnectionString,CommandType.Text,sql);
				if(suc==1)
					return true;
				else
					return false;
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		
		//检查字段描述是否存在
		public static bool CheckColumnDescExists(string _tablename ,string _colname)
		{
			try
			{
				string sql=string.Format("SELECT 1 FROM ::FN_LISTEXTENDEDPROPERTY('MS_Description', 'User','dbo','table','{0}', 'column','{1}')",_tablename,_colname);
				SqlDataReader dr;
				dr = SqlHelper.ExecuteReader(pubFunc.DBConnectionString,CommandType.Text,sql);
				bool suc=false;
				if(dr.Read())
					suc=true;
				dr.Close();
				return suc;
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
	}
}
