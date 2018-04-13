using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.BLL;
using Kingsun.SynchronousStudy.Models;

namespace Kingsun.SynchronousStudyHopeChina.Web.HopeChina
{
    public partial class ArticleManageMent : System.Web.UI.Page
    {
        private IWorkbook _workbook;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ImportVideoDetails();
        }

        /// <summary>
        /// 导入活动资源
        /// </summary>
        private void ImportVideoDetails()
        {
            string newFileName = string.Empty;
            string strName = FileUpload1.PostedFile.FileName; //使用fileupload控件获取上传文件的文件名
            if (string.IsNullOrEmpty(strName))
            {
                ClientScript.RegisterStartupScript(GetType(), "tishi",
                    "<script type=\"text/javascript\">alert('请选择文件！');</script>");
                return;
            }
            newFileName = GetNewFileName(strName, newFileName);
            DataTable dt = new DataTable();
            DataTable dtSheet2 = new DataTable();
            try
            {
                dt = ExcelToDataTable("总视频信息", newFileName, true);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(GetType(), "tishi",
                    "<script type=\"text/javascript\">alert('导入：" + ex.Message + "');</script>");
                throw ex;
            }
            if (dt.Rows.Count > 0)
            {
                List<TbArticle> list = new List<TbArticle>();
                foreach (DataRow item in dt.Rows)
                {
                    TbArticle art = new TbArticle();
                    art.ATitle = item["标题"].ToString();
                    art.AContent = item["原文"].ToString();
                    art.ARemark = item["评测内容"].ToString();
                    art.APeriod = item["所属组别"].ToString();
                    list.Add(art);
                }

                KingRequest req = new KingRequest();
                object obj = null;
                KingResponse res = null;
                ArticleImplement artImplement = new ArticleImplement();
                obj = new { AContent = list };
                req.Data = JsonHelper.EncodeJson(list);
                req.Function = "AddArticle";
                res = artImplement.ProcessRequest(req);
                if (res.Success)
                {
                    ClientScript.RegisterStartupScript(GetType(), "tishi",
                   "<script type=\"text/javascript\">alert('导入：" + res.Data + "');</script>");
                }
            }
        }

        private string GetNewFileName(string strName, string newFileName)
        {
            if (strName != "") //如果文件名存在
            {
                bool fileOk = false;
                int i = strName.LastIndexOf(".", StringComparison.Ordinal); //获取。的索引顺序号，在这里。代表文件名字与后缀的间隔
                string kzm = strName.Substring(i);
                //获取文件扩展名的另一种方法 string fileExtension = System.IO.Path.GetExtension(FileUpload1.FileName).ToLower();
                string juedui = Server.MapPath("~\\Upload\\Excel\\");
                //设置文件保存的本地目录绝对路径，对于路径中的字符“＼”在字符串中必须以“＼＼”表示，因为“＼”为特殊字符。或者可以使用上一行的给路径前面加上＠
                newFileName = juedui + strName + kzm;
                if (FileUpload1.HasFile) //验证 FileUpload 控件确实包含文件
                {
                    String[] allowedExtensions = { ".xls", ".xlsx" };
                    for (int j = 0; j < allowedExtensions.Length; j++)
                    {
                        if (kzm == allowedExtensions[j])
                        {
                            fileOk = true;
                        }
                    }
                }
                if (fileOk)
                {
                    try
                    {
                        // 判定该路径是否存在
                        if (!Directory.Exists(juedui))
                            Directory.CreateDirectory(juedui);
                        FileUpload1.PostedFile.SaveAs(newFileName); //将图片存储到服务器上
                    }
                    catch (Exception ex)
                    {
                        ClientScript.RegisterStartupScript(GetType(), "tishi",
                            "<script type=\"text/javascript\">alert('" + ex.Message + "');</script>");
                    }
                }
            }
            return newFileName;
        }

        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="fileName"></param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <returns>返回的DataTable</returns>
        public DataTable ExcelToDataTable(string sheetName, string fileName, bool isFirstRowColumn)
        {
            DataTable data = new DataTable();
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                //FileStream fs = File.OpenRead(HttpContext.Current.Server.MapPath(fileName));
                if (fileName.IndexOf(".xlsx", StringComparison.Ordinal) > 0) // 2007版本
                    _workbook = new XSSFWorkbook(fs);
                else if (fileName.IndexOf(".xls", StringComparison.Ordinal) > 0) // 2003版本
                    _workbook = new HSSFWorkbook(fs);

                ISheet sheet;
                if (sheetName != null)
                {
                    sheet = _workbook.GetSheet(sheetName) ?? _workbook.GetSheetAt(0);
                }
                else
                {
                    sheet = _workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

                    int startRow;
                    if (isFirstRowColumn)
                    {
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            ICell cell = firstRow.GetCell(i);
                            if (cell != null)
                            {
                                string cellValue = cell.StringCellValue;
                                if (cellValue != null)
                                {
                                    DataColumn column = new DataColumn(cellValue);
                                    data.Columns.Add(column);
                                }
                            }
                        }
                        startRow = sheet.FirstRowNum + 1;
                    }
                    else
                    {
                        startRow = sheet.FirstRowNum;
                    }

                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null || row.Cells[0].ToString().Trim() == "") continue; //没有数据的行默认是null　　　　　　　

                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                dataRow[j] = row.GetCell(j).ToString();
                        }
                        data.Rows.Add(dataRow);
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 判断是否属于时间格式
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        public bool IsDate(string strDate)
        {
            try
            {
                DateTime.Parse(strDate);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}