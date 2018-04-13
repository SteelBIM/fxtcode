using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using CourseActivate.Activate.BLL;
using CourseActivate.Activate.Constract.Model;
using CourseActivate.Core.Utility;
using CourseActivate.Framework.BLL;
using CourseActivate.Web.Admin.Models;
using NPOI.HSSF.UserModel;
using NPOI.OpenXml4Net.OPC.Internal;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using tb_batch = CourseActivate.Activate.Constract.Model.tb_batch;
using System.Transactions;

namespace CourseActivate.Web.Admin.Controllers
{
    public class ImportExcelController : BaseController
    {
        private IWorkbook _workbook;

        //
        // GET: /ImportExcel/
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult SaveAsExcel()
        {
            var oFile = Request.Files["txt_file"];
            string name = "";
            if (oFile != null)
            {
                name = GetNewFileName(oFile.FileName, "", oFile);
            }
            return Json(KingResponse.GetResponse(name));
        }

        struct WrongData
        {
            public string batch { get; set; }
            public string activatecode { get; set; }
        }
        public JsonResult CheckExcel(string activatetypeid, string bookid, string remark, string excel)
        {
            Manage ma = new Manage();
            string newFileName = string.Empty;
            string strName = string.Empty;
            if (!string.IsNullOrEmpty(excel))
            {
                strName = excel;
            }
            if (string.IsNullOrEmpty(strName))
            {
                return Json(KingResponse.GetErrorResponse("请选择文件！"));
            }

            DataTable dt = ExcelToDataTable("激活码信息", strName, true);
            if (dt.Rows.Count <= 0)
            {
                return Json(KingResponse.GetErrorResponse("Excel表数据为空或Excel是打开状态！！"));
            }

            StringBuilder sbBatch = new StringBuilder();
            StringBuilder sbActivate = new StringBuilder();
            StringBuilder sbBooks = new StringBuilder();

            List<tb_batchactivate> actCodeList = new List<tb_batchactivate>();
            string batchcode = dt.Rows[0][0].ToString().Substring(0, 3);
            List<WrongData> wrongData = new List<WrongData>();
            foreach (DataRow row in dt.Rows)
            {

                if (batchcode != row[0].ToString().Substring(0, 3))
                {
                    wrongData.Add(new WrongData { batch = row[0].ToString(), activatecode = row[0].ToString().Replace("-", "").Replace("_", "") });
                }
                else if (row[0].ToString().Replace("-", "").Replace("_", "").Length != 9)
                {
                    wrongData.Add(new WrongData { batch = row[0].ToString(), activatecode = row[0].ToString().Replace("-", "").Replace("_", "") });
                }
                else
                {
                    actCodeList.Add(new tb_batchactivate { activatecode = row[0].ToString().Replace("-", "").Replace("_", ""), createtime = DateTime.Now });
                }
            }

            if (wrongData.Any())
            {
                string codeString = "";
                for (int i = 0; i < wrongData.Count; i++) {
                    codeString += wrongData[i].activatecode + ",";
                    if (i % 5 == 0)
                        codeString += "<br/>";
                }
                    return Json(KingResponse.GetErrorResponse("<div style='width:500px;height:400px'>以下激活码有误(位数不等于9或者批次号不等于第一个激活码的批次号),请修改后再导入:" + codeString));
            }

            List<tb_batch> batch = ma.SelectSearch<tb_batch>(i => i.batchcode == batchcode);

            if (batch.Any())
            {
                return Json(KingResponse.GetErrorResponse("批次重复:批次码在系统中已存在,请修改后再导入"));
            }
            string[] codeArray = actCodeList.Select(o => o.activatecode).ToArray();

            var count = actCodeList.Count();
            var distinctCount = actCodeList.Select(o => o.activatecode).Distinct().Count();
            var differ = count - distinctCount;
            if (differ > 0)
            {
                var groups = actCodeList.GroupBy(o => o.activatecode);
                var sameCodes = groups.Where(o => o.Count() > 1).Select(o => o.Key).ToArray();
                string codeString = "";
                for (int i = 0; i < sameCodes.Count(); i++) {
                    codeString += sameCodes[i] + ",";
                    if (i % 5 == 0) codeString += "<br/>";
                }
                return Json(KingResponse.GetErrorResponse("检测到有" + differ + "条数据重复,请修改后再导入,重复的激活码:" + codeString));
            }
            return Json(KingResponse.GetResponse(""));
        }

        public JsonResult LoadExcel(string activatetypeid, string bookid, string remark, string excel)
        {
            Manage ma = new Manage();
            string newFileName = string.Empty;
            string strName = string.Empty;
            if (!string.IsNullOrEmpty(excel))
            {
                strName = excel;
            }
            if (string.IsNullOrEmpty(strName))
            {
                return Json(KingResponse.GetErrorResponse("请选择文件！"));
            }

            DataTable dt = ExcelToDataTable("Sheet1", strName, true);

            if (dt.Rows.Count <= 0)
            {
                return Json(KingResponse.GetErrorResponse("Excel表数据为空或Excel是打开状态！！"));
            }

            #region 使用事务导入题目
            StringBuilder sbBatch = new StringBuilder();
            StringBuilder sbActivate = new StringBuilder();
            StringBuilder sbBooks = new StringBuilder();

            List<tb_batchactivate_copy> actCodeList = new List<tb_batchactivate_copy>();
            string batchcode = dt.Rows[0][0].ToString().Substring(0, 3);
            foreach (DataRow row in dt.Rows)
            {
                actCodeList.Add(new tb_batchactivate_copy { activatecode = row[0].ToString().Replace("-", "").Replace("_", ""), createtime = DateTime.Now });
            }
            using (TransactionScope scope = new TransactionScope())
            {
                List<tb_batch> batch = ma.SelectSearch<tb_batch>(i => i.batchcode == batchcode);
                #region ORM直接导入
                if (batch.Any())
                {
                    return Json(KingResponse.GetErrorResponse("批次已存在！"));
                }
                tb_batch newBatch = new tb_batch { activatenum = actCodeList.Count, batchcode = batchcode, status = 0, remark = remark, activatetypeid = int.Parse(activatetypeid), createtime = DateTime.Now, startdate = DateTime.Now, enddate = DateTime.Now.AddYears(1), purpose = 0, indate = 12, createtype = "人工导入" };
                var batchId = Manage.Add<tb_batch>(newBatch);
                actCodeList.ForEach(o =>
                {
                    o.batchid = batchId;
                });
                Manage.MSSqlBulkCopy<tb_batchactivate_copy>(actCodeList, "tb_batchactivate");

                if (!string.IsNullOrEmpty(bookid) && int.Parse(bookid) > -1)
                {
                    Manage.Add(new tb_batchbooks { batchid = batchId, bookid = int.Parse(bookid), createTime = DateTime.Now });
                }

                scope.Complete();

            }

            return Json(KingResponse.GetResponse("导入成功"));
                #endregion
            #region sql方式导入
            //if (batc.Count == 0)
            //{
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        sbBatch.Append(string.Format(@" UNION ALL SELECT  '{0}' ,'{1}' ,'{2}' ,'{3}' ,'{4}' ,'{5}' ,'{6}' ,'{7}' ,'{8}' ,'{9}' ,'{10}' ,GETDATE() ",
            //            dt.Rows[i]["批次码"].ToString(), dt.Rows[i]["批次起始日期"].ToString(), dt.Rows[i]["批次结束日期"].ToString(), dt.Rows[i]["激活码有效时长"].ToString(),
            //            dt.Rows[i]["用途"].ToString(), dt.Rows.Count, activatetypeid, 0, masterinfo.masterid, masterinfo.mastername, remark));
            //        sbActivate.Append(string.Format(@" UNION ALL SELECT '{0}', @EditionID , GETDATE() ", dt.Rows[0]["激活码"].ToString()));
            //        sbBooks.Append(string.Format(@" UNION ALL SELECT  @EditionID,'{0}', GETDATE() ", bookid));
            //    }

            //    StringBuilder sb = new StringBuilder();
            //    if (sbBatch.Length > 0)
            //    {
            //        sb.Append(" INSERT  INTO dbo.tb_batch( batchcode ,startdate ,enddate ,indate ,purpose ,activatenum ,activatetypeid ,status ,masterid ,mastername ,remark ,createtime) " + sbBatch.ToString().Substring(10));
            //        sb.Append(";declare @EditionID int;select @EditionID=@@IDENTITY");
            //    }
            //    if (sbActivate.Length > 0)
            //    {
            //        sb.Append(" ; INSERT INTO dbo.tb_batchactivate ( activatecode, batchid, createtime ) " + sbActivate.ToString().Substring(10));
            //    }
            //    if (sbBooks.Length > 0)
            //    {
            //        sb.Append(" ;INSERT INTO dbo.tb_batchbooks ( batchid, bookid, createTime ) " + sbBooks.ToString().Substring(10));
            //    }

            //    System.IO.StreamWriter sw = System.IO.File.AppendText(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\Upload\\log.txt");
            //    sw.WriteLine(string.Format("开始导入批次码时间={0}，bookName={1}\r\n{2}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), newFileName, sb.ToString()));
            //    sw.Flush();
            //    sw.Close();
            //    try
            //    {
            //        if (ma.ExcuteSqlWithTran(sb.ToString()))
            //        {
            //            System.IO.StreamWriter sw2 = System.IO.File.AppendText(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\Upload\\log.txt");
            //            sw2.WriteLine(string.Format("结束导入批次码时间={0}，bookName={1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), newFileName));
            //            sw2.Flush();
            //            sw2.Close();
            //        }
            //        else
            //        {
            //            throw new Exception("执行sql时候出错!");
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        System.IO.StreamWriter sw1 = System.IO.File.AppendText(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\Upload\\log.txt");
            //        sw1.WriteLine(string.Format("导入批次码异常时间={0}，bookName={1}，{2}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), newFileName, ex.ToString()));
            //        sw1.Flush();
            //        sw1.Close();
            //    }
            //}
            #endregion
            #endregion
        }

        private string GetNewFileName(string strName, string newFileName, HttpPostedFileBase oflie)
        {
            if (strName != "") //如果文件名存在
            {
                bool fileOk = false;
                int i = strName.LastIndexOf(".", StringComparison.Ordinal); //获取。的索引顺序号，在这里。代表文件名字与后缀的间隔
                string kzm = strName.Substring(i);
                //获取文件扩展名的另一种方法 string fileExtension = System.IO.Path.GetExtension(FileUpload1.FileName).ToLower();
                string juedui = Server.MapPath("~\\Upload\\Excel\\");
                //设置文件保存的本地目录绝对路径，对于路径中的字符“＼”在字符串中必须以“＼＼”表示，因为“＼”为特殊字符。或者可以使用上一行的给路径前面加上＠
                newFileName = juedui + strName;

                String[] allowedExtensions = { ".xls", ".xlsx", ".csv" };
                foreach (string t in allowedExtensions)
                {
                    if (kzm == t)
                    {
                        fileOk = true;
                    }
                }
                if (fileOk)
                {
                    try
                    {
                        // 判定该路径是否存在
                        if (!Directory.Exists(juedui))
                            Directory.CreateDirectory(juedui);
                        oflie.SaveAs(newFileName); //将文件存储到服务器上
                    }
                    catch (Exception ex)
                    {
                        return StringRep(ex.Message);
                    }
                }
            }
            return newFileName;
        }

        public string StringRep(string name)
        {
            string str = name.Replace("'", "\'");
            return str;
        }


        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="fileName"></param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <param name="of"></param>
        /// <returns>返回的DataTable</returns>
        public DataTable ExcelToDataTable(string sheetName, string fileName, bool isFirstRowColumn)
        {
            DataTable data = new DataTable();
            try
            {
                //var oStream = of.InputStream;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                if (fileName.IndexOf(".xlsx", StringComparison.Ordinal) > 0) // 2007版本
                    _workbook = new XSSFWorkbook(fs);
                else if (fileName.IndexOf(".xls", StringComparison.Ordinal) > 0) // 2003版本
                    _workbook = new HSSFWorkbook(fs);
                else
                    _workbook = new XSSFWorkbook(fs);

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
                    IRow firstRow = sheet.GetRow(sheet.FirstRowNum);
                    if (firstRow == null) return data;
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
                return data;
            }
        }

        public JsonResult GetBookInfo(string EditionID, string Subject, string Grade)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            ImportActicateBLL iab = new ImportActicateBLL();
            bookInfo[] bi;
            try
            {
                string ss = iab.GetBookInfoById(EditionID, Subject, Grade);
                bi = js.Deserialize<bookInfo[]>(ss);
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
            return Json(new { total = bi.Count(), rows = bi });
        }

        /// <summary>
        /// 获取激活码类型
        /// </summary>
        /// <returns></returns>
        public JsonResult GetActivateType()
        {
            return Json(base.SelectSearch<tb_activatetype>(o => o.status == 1));
        }

        /// <summary>
        /// 获取版本
        /// </summary>
        /// <returns></returns>
        public JsonResult GetManagement()
        {
            return Json(SelectAll<tb_code_edition>());
        }

        /// <summary>
        /// 获取年级
        /// </summary>
        /// <returns></returns>
        public JsonResult GetGrade()
        {
            return Json(SelectAll<tb_code_grade>());
        }

        /// <summary>
        /// 获取科目
        /// </summary>
        /// <returns></returns>
        public JsonResult GetSubject()
        {
            return Json(SelectAll<tb_code_subject>());
        }

        public class bookInfo
        {
            public string BookName { get; set; }
            public string BookID { get; set; }
        }

    }
}
