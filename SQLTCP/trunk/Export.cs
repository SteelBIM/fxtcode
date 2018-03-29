using System;
using System.Data;
using System.Collections;
using System.Reflection; 
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;


namespace SqlTCP
{
	public class Export 
	{
		
		private object m_objOpt = System.Reflection.Missing.Value;
		private ApplicationClass exc =null;
		private Workbooks  workbooks=null;
		private _Workbook workbook=null;
		private Sheets sheets=null;
		private _Worksheet worksheet=null;
		private _Worksheet worksheetAll=null;//����
		private Range rngtitle=null;
		private Range rng=null;
		private _QueryTable qt=null;
		public int ShtIndex=1;
		private int MulSheet=1;

		private int RowAt=1;

		public Export()
		{
			exc= new ApplicationClass();
			workbooks=exc.Workbooks;
			workbook = workbooks.Add(m_objOpt); 
			sheets = workbook.Worksheets;
		
		}
		
		//��������
		public bool ExportBackup(string _Table)
		{
			try
			{
				DataSet ds=TableColumns.Exec_P_TableColumnBackupProperty(_Table);
				if(ds.Tables[0].Rows.Count<=0) return true;
				worksheet=(_Worksheet)sheets[ShtIndex];
				rng=worksheet.get_Range(worksheet.Cells[RowAt,1],worksheet.Cells[RowAt+ds.Tables[0].Rows.Count-1,ds.Tables[0].Columns.Count]);
				
				string[,] str=new string[ds.Tables[0].Rows.Count,ds.Tables[0].Columns.Count];
				for(int i=0 ; i<ds.Tables[0].Rows.Count;i++)
				{
					for(int j=0;j<ds.Tables[0].Columns.Count;j++)
					{
						str[i,j]=ds.Tables[0].Rows[i][j].ToString();
					}
				}
				
				rng.Value2=str;
				RowAt+=ds.Tables[0].Rows.Count; 
				
				return true;
				
			}
			catch(Exception ex) {throw ex;}
		}

		public void ShowExcel()
		{
			exc.Visible=true;
		}

		//�����ļ�
		public bool SaveAs(string filename)
		{
			try
			{
				workbook.SaveAs(filename,m_objOpt,m_objOpt,m_objOpt,m_objOpt,m_objOpt,XlSaveAsAccessMode.xlNoChange,m_objOpt,m_objOpt,m_objOpt,m_objOpt,m_objOpt);
				return true;
			}
			catch(Exception ex) {throw ex;}
		}

		public void Dispose()
		{
			try
			{
				workbook.Close(false,m_objOpt,m_objOpt);
				exc.Quit();
			}
			catch{}
		}

		//���������ĵ�
//		public bool ExportExcel(string _Table)
//		{
//			try
//			{
//				rngtitle=worksheet.get_Range("A"+RowAt.ToString(),m_objOpt);
//				rngtitle.Font.Bold=true;
//				rngtitle.Font.Size=12;
//				rngtitle.Value2="������" + _Table;
//				RowAt++;
//				worksheet.Cells[RowAt,1]="--------------------------------------------------------------";
//				RowAt++;
//				rng=worksheet.get_Range("A"+RowAt.ToString(),m_objOpt);
//									
//				qt=worksheet.QueryTables.Add(pubFunc.OleDBConnectionString,rng,TableColumns.GetTableColumnString(_Table));
//				qt.RefreshStyle=XlCellInsertionMode.xlInsertEntireRows;
//					
//				qt.Refresh(false);
//				RowAt+=qt.ResultRange.Rows.Count; 
//				RowAt++;
//				return true;
//				
//			}
//			catch(Exception ex) {throw ex;}
//		}

		//���������ĵ�
		public bool ExportExcel(string _Table,string _ShowName)
		{
			try
			{
				if(sheets.Count<ShtIndex) 
					worksheet=(_Worksheet)sheets.Add(m_objOpt,sheets[sheets.Count],m_objOpt,m_objOpt);
				else
					worksheet=(_Worksheet)sheets[ShtIndex];
				worksheet.Name=getString(_Table);
				worksheet.Rows.Font.Size=9;
				rngtitle=worksheet.get_Range("A"+RowAt.ToString(),m_objOpt);
				rngtitle.Font.Bold=true;
				rngtitle.Font.Size=12;
				rngtitle.Value2=_ShowName;
				//��Ҫ���ص���ҳ
				if(ShtIndex>1) worksheet.Hyperlinks.Add(worksheet.Cells[1,7],"","�ĵ�����!A1","�����ĵ�����ҳ","�����ĵ�����ҳ");
				 
				RowAt++;
				worksheet.Cells[RowAt,1]="--------------------------------------------------------------";
				RowAt++;
				rng=worksheet.get_Range("A"+RowAt.ToString(),m_objOpt);
									
				qt=worksheet.QueryTables.Add(pubFunc.OleDBConnectionString,rng,TableColumns.GetTableColumnString(_Table));
				qt.RefreshStyle=XlCellInsertionMode.xlInsertEntireRows;
					
				qt.Refresh(false);

				RowAt=1;
				if(ShtIndex>1) SetNavSheet(worksheet.Name,_ShowName);//��Ҫ����
				ShtIndex++;
				return true;
				
			}
			catch(Exception ex) {throw ex;}
		}

		public bool SetNavSheet(string _Table,string _ShowName)
		{	
			worksheet=(_Worksheet)sheets[1];
			worksheetAll=(_Worksheet)sheets[2];
			if(ShtIndex==3)
			{
				worksheet.Cells[1,1]="�ĵ�����";
				worksheetAll.Name="�ĵ�����";
				worksheetAll.Cells[1,1]="�ĵ�����";
				/*
				worksheetAll.Cells[1,1]="�ֶ����";
				worksheetAll.Cells[1,2]="�ֶ���";
				worksheetAll.Cells[1,3]="����";
				worksheetAll.Cells[1,4]="����";
				worksheetAll.Cells[1,5]="����Ϊ��";
				worksheetAll.Cells[1,6]="Ĭ��ֵ";
				worksheetAll.Cells[1,7]="�ֶ�˵��";
				worksheetAll.get_Range("A1","G1").Font.Bold=true;
				*/

				rng=worksheet.get_Range("B1",m_objOpt);
				rng.ColumnWidth=50;
				worksheet.Name="�ĵ�����";
			}

			worksheet.Hyperlinks.Add(worksheet.Cells[ShtIndex-1,2],"",_Table + "!A1",_ShowName,_ShowName);
			
			//���뵽���ܱ�
			//((_Worksheet)sheets[ShtIndex]).UsedRange.Copy(((_Worksheet)sheets[2]).get_Range("A" + ((_Worksheet)sheets[2]).UsedRange.Rows.Count.ToString(),m_objOpt));
			worksheetAll.Hyperlinks.Add(worksheetAll.Cells[worksheetAll.UsedRange.Rows.Count+1,1],"",_Table + "!A1",_ShowName,_ShowName);
			//A4:����������
			((_Worksheet)sheets[ShtIndex]).get_Range("A3","G"+((_Worksheet)sheets[ShtIndex]).UsedRange.Rows.Count.ToString()).Copy(worksheetAll.get_Range("A" + (worksheetAll.UsedRange.Rows.Count+1).ToString(),m_objOpt));
			return true;
		}

		public void Init()
		{
			worksheet=(_Worksheet)sheets[1];
			worksheet.Rows.Font.Size=9;
            worksheetAll = (_Worksheet)sheets[2];
			worksheetAll.Rows.Font.Size=9;
			worksheet.Activate();
		}

		
		private string getString(string oStr)
		{
			if(oStr.Length>31)
			{
				oStr = oStr.Substring(0,31-MulSheet.ToString().Length-3) + "..." + MulSheet.ToString() ;
				MulSheet++;
			}
			return oStr;
		}
	}
	
	
}