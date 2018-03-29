using System;
using System.Data;
using System.Data.OleDb;
using System.Collections;
using System.Reflection; 
using System.Runtime.InteropServices; 
using Microsoft.Office.Interop.Excel;

namespace SqlTCP
{
	public class Import 
	{
		
		private string strConn="";

		public Import(string _filename)
		{
			strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + _filename + ";Extended Properties='Excel 8.0;HDR=No'"; 
			
		}


		//更新数据库
		public bool ImportBackup(string _Table,string _Column , string _Value)
		{
			try
			{
				TableColumns.UpdateColumn(_Table, _Column , _Value);
				return true;
			}
			catch(Exception ex){throw ex;}
		}

		//导入备份文件
		public DataSet GetImportData()
		{
			try
			{
				OleDbConnection conn = new OleDbConnection(strConn);
				OleDbDataAdapter adp = new OleDbDataAdapter("Select * from [Sheet1$]",conn);
				DataSet ds = new DataSet();
				adp.Fill(ds);
				conn.Close();
				conn.Dispose();
				adp.Dispose();
				return ds;
			}
			catch(Exception ex){throw ex;}
		}
	}
}
