using CourseActivate.Account.Constract.VW;
using CourseActivate.Core.Utility;
using CourseActivate.Resource.BLL;
using CourseActivate.Resource.Constract.Model;
using CourseActivate.Web.Admin.Models;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace CourseActivate.Web.Admin.Controllers
{
    public class ResourceMgrController : BaseController
    {
        // GET: ResourceMgr
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetcurrentAction()
        {
            return Json(action);
        }

        [HttpPost]
        public JsonResult GetSelectValue()
        {
            var list = base.Manage.SelectAll<vw_book_selectKeyValue>();
            return Json(list);
        }

        [HttpPost]
        public JsonResult Detail(int pagesize, int pageindex, string sort, string order, [System.Web.Http.FromBody]CourseCondition ccInfo)
        {
            PageParameter<tb_res_book> param = new PageParameter<tb_res_book>();
            param.PageIndex = setpageindex(pageindex, pagesize);
            param.PageSize = pagesize;
            if (sort == "CreateTime")
            {
                param.OrderColumns = x => x.CreateTime;
            }
            else if (sort == "UpdateTime")
            {
                param.OrderColumns = x => x.UpdateTime;
            }
            if (order == "asc")
            {
                param.IsOrderByASC = 1;
            }
            else if (order == "desc")
            {
                param.IsOrderByASC = 0;
            }
            param.Wheres = GetCourseCondition(ccInfo);
            int totalcount = 0;
            var list = base.Manage.SelectPage<tb_res_book>(param, out totalcount);
            return Json(new { total = totalcount, rows = list });
        }

        private List<System.Linq.Expressions.Expression<Func<tb_res_book, bool>>> GetCourseCondition(CourseCondition ccInfo)
        {
            List<Expression<Func<tb_res_book, bool>>> expression = new List<Expression<Func<tb_res_book, bool>>>();
            if (ccInfo != null)
            {
                if (ccInfo.Edition > 0)
                {
                    expression.Add(i => i.EditionID == ccInfo.Edition);
                }
                if (ccInfo.Grade > 0)
                {
                    expression.Add(i => i.GradeID == ccInfo.Grade);
                }
                if (ccInfo.Period > 0)
                {
                    expression.Add(i => i.PeriodID == ccInfo.Period);
                }
                if (ccInfo.Publish > 0)
                {
                    expression.Add(i => i.Publishid == ccInfo.Publish);
                }
                if (ccInfo.Reel > 0)
                {
                    expression.Add(i => i.ReelID == ccInfo.Reel);
                }
                if (ccInfo.Status >= 0)
                {
                    expression.Add(i => i.Status == ccInfo.Status);
                }
                if (ccInfo.Subject > 0)
                {
                    expression.Add(i => i.SubjectID == ccInfo.Subject);
                }
            }
            return expression;
        }

        public JsonResult AddBook([System.Web.Http.FromBody]tb_res_book book)
        {
            //   book.BookName = book.EditionName + book.PeriodName + book.SubjectName + book.GradeName + book.ReelName;
            book.CreateTime = DateTime.Now;
            book.UpdateTime = DateTime.Now;

            System.Web.HttpFileCollection _file = System.Web.HttpContext.Current.Request.Files;

            List<tb_res_catalog> firstCatalogs = new List<tb_res_catalog>();
            List<tb_res_catalog> secondCatalogs = new List<tb_res_catalog>();
            List<tb_res_catalog> thirdCatalogs = new List<tb_res_catalog>();
            string saveName = "";
            if (_file.Count > 0)
            {
                var catalog = System.Web.HttpContext.Current.Request.Files["Catalog"];
                var bookCover = System.Web.HttpContext.Current.Request.Files["BookCover"];
                //if (catalog!=null)
                //{
                //    SaveFile(catalog, "Excel/");
                //}

                if (bookCover != null && bookCover.ContentLength > 0)
                {
                    SaveFile(bookCover, "BookResource/", out saveName);
                }

                //book的封面保存
                //string path = System.Configuration.ConfigurationManager.AppSettings["ResourceRoot"];
                string path = "@/Upload/BookResource/";
                book.BookCover = path + saveName;

                //目录的导入   
                if (catalog != null && catalog.ContentLength > 0)
                {
                    IWorkbook _workbook;
                    string catalogFileType = System.IO.Path.GetExtension(catalog.FileName);
                    if (catalogFileType == ".xls")
                    {
                        _workbook = new HSSFWorkbook(catalog.InputStream);
                    }
                    else
                    {
                        _workbook = new XSSFWorkbook(catalog.InputStream);
                    }
                    ISheet sheet = _workbook.GetSheetAt(0);
                    if (sheet != null)
                    {
                        IRow firstRow = sheet.GetRow(0);
                        int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数
                        int startRow = sheet.FirstRowNum + 1;
                        //最后一列的标号
                        int rowCount = sheet.LastRowNum;

                        for (int i = startRow; i <= rowCount; ++i)
                        {
                            IRow row = sheet.GetRow(i);
                            if (row == null) break; //没有数据的行默认是null　　　　　　　
                            if (row.GetCell(0) != null && row.GetCell(0).ToString() != "")
                            {
                                tb_res_catalog firstCatalog = new tb_res_catalog
                                {
                                    CatalogLevel = 1,
                                    CatalogName = row.GetCell(0).ToString(),
                                    PageNoStart = Convert.ToInt32(row.GetCell(3).ToString()),
                                    PageNoEnd = Convert.ToInt32(row.GetCell(4).ToString()),
                                    ParentID = 0,
                                    CreateTime = DateTime.Now,
                                    Status = 0,
                                    ModularIDS = "0"
                                };
                                firstCatalogs.Add(firstCatalog);
                            }
                            if (row.GetCell(1) != null && row.GetCell(1).ToString() != "")
                            {
                                tb_res_catalog secondCatalog = new tb_res_catalog
                                {
                                    CatalogLevel = 2,
                                    CatalogName = row.GetCell(1).ToString(),
                                    PageNoStart = Convert.ToInt32(row.GetCell(3).ToString()),
                                    PageNoEnd = Convert.ToInt32(row.GetCell(4).ToString()),
                                    ParentID = firstCatalogs.Count - 1,
                                    CreateTime = DateTime.Now,
                                    Status = 0,
                                    ModularIDS = "0"
                                };
                                secondCatalogs.Add(secondCatalog);
                            }
                            if (row.GetCell(2) != null && row.GetCell(2).ToString() != "")
                            {
                                tb_res_catalog thirdCatalog = new tb_res_catalog
                                {
                                    CatalogLevel = 3,
                                    CatalogName = row.GetCell(2).ToString(),
                                    PageNoStart = Convert.ToInt32(row.GetCell(3).ToString()),
                                    PageNoEnd = Convert.ToInt32(row.GetCell(4).ToString()),
                                    ParentID = secondCatalogs.Count - 1,
                                    CreateTime = DateTime.Now,
                                    Status = 0,
                                    ModularIDS = "0"
                                };
                                thirdCatalogs.Add(thirdCatalog);
                            }
                        }
                    }
                }
            }

            //插入数据库
            bool isAdd = true;
            if (book.BookID > 0)
            {
                isAdd = false;
                var re = base.Manage.Select<tb_res_book>(book.BookID);
                //未上传封面，保留原有封面
                if (string.IsNullOrEmpty(saveName))
                {
                    book.BookCover = re.BookCover;
                }
                //上传目录,删除原有目录
                if (firstCatalogs != null && firstCatalogs.Count > 0)
                {
                    base.Manage.Delete<tb_res_catalog>(o => o.BookID == book.BookID);
                }
                book.CreateTime = re.CreateTime;
            }
            if (base.Manage.TranAddBook(book, firstCatalogs, secondCatalogs, thirdCatalogs))
            {
                new ResourceBLL().GetResJson(book.BookID, Server.MapPath("~/Upload/BookResource/" + book.BookID + ".json"));
                string resRoot = System.Configuration.ConfigurationManager.AppSettings["Webhost"];
                new ResourceBLL().CopyFileRequest(book.BookCover.Replace("@/",resRoot));
                if (isAdd)
                {
                    return Json(KingResponse.GetResponse("新建成功！"));
                }
                else
                {
                    return Json(KingResponse.GetResponse("修改成功！"));
                }
            }
            else
            {
                if (isAdd)
                {
                    return Json(KingResponse.GetErrorResponse("新建失败！"));
                }
                else
                {
                    return Json(KingResponse.GetErrorResponse("修改失败！"));
                }
            }
        }

        private bool SaveFile(HttpPostedFile file, string filePath, out string saveName)
        {
            saveName = "";
            //文件大小  
            long size = file.ContentLength;
            //文件类型  
            string type = file.ContentType;
            //文件名  
            string name = file.FileName;
            //文件格式  
            string _tp = System.IO.Path.GetExtension(name);

            if (_tp.ToLower() == ".jpg" || _tp.ToLower() == ".jpeg" || _tp.ToLower() == ".gif" || _tp.ToLower() == ".png" || _tp.ToLower() == ".swf"
                || _tp.ToLower() == ".xls" || _tp.ToLower() == ".xlsx")
            {
                //获取文件流  
                System.IO.Stream stream = file.InputStream;
                //保存文件  
                saveName = DateTime.Now.ToString("yyyyMMddHHmmss") + _tp;
                string path = Server.MapPath("~/Upload/") + filePath + saveName;
                file.SaveAs(path);
                return true;
            }
            return false;
        }

        public JsonResult GetCourseByID(int bookID)
        {
            var re = base.Manage.Select<tb_res_book>(bookID);
            if (re != null)
            {
                return Json(KingResponse.GetResponse(re));
            }
            else
            {
                return Json(KingResponse.GetErrorResponse("获取书本信息失败！"));
            }
        }

        public JsonResult StopCourseByID(int bookID)
        {
            var re = base.Manage.CustomUpdateEntity<tb_res_book>(o => o.BookID.ToString(), new tb_res_book { BookID = bookID, Status = 2, UpdateTime = DateTime.Now }, o => o.Status.ToString(), o => o.UpdateTime.ToString());
            if (re)
            {
                new ResourceBLL().GetResJson(bookID, Server.MapPath("~/Upload/BookResource/" + bookID + ".json"));
                return Json(KingResponse.GetResponse("禁用成功！"));
            }
            else
            {
                return Json(KingResponse.GetErrorResponse("禁用失败！"));
            }
        }

        public JsonResult StartCourseByID(int bookID)
        {
            var re = base.Manage.CustomUpdateEntity<tb_res_book>(o => o.BookID.ToString(), new tb_res_book { BookID = bookID, Status = 1, UpdateTime = DateTime.Now }, o => o.Status.ToString(), o => o.UpdateTime.ToString());
            if (re)
            {
                new ResourceBLL().GetResJson(bookID, Server.MapPath("~/Upload/BookResource/" + bookID + ".json"));

                return Json(KingResponse.GetResponse("启用成功！"));
            }
            else
            {
                return Json(KingResponse.GetErrorResponse("启用失败！"));
            }
        }

        public JsonResult DelCourseByID(int bookID)
        {
            var books = base.Manage.SelectSearch<tb_res_book>(x => x.BookID == bookID);
            if (books.Count > 0)
            {
                var status = books.First().Status;
                if (status != 0)
                {
                    return Json(KingResponse.GetErrorResponse("该课程不可删除！"));
                }
            }
            var re = base.Manage.TranDelBook(bookID);
            if (re)
            {
                new ResourceBLL().GetResJson(bookID, Server.MapPath("~/Upload/BookResource/" + bookID + ".json"));

                return Json(KingResponse.GetResponse("删除成功！"));
            }
            else
            {
                return Json(KingResponse.GetErrorResponse("删除失败！"));
            }
        }

        public JsonResult BatchStopCourse(string bookIDs)
        {
            if (string.IsNullOrWhiteSpace(bookIDs))
            {
                return Json(KingResponse.GetErrorResponse("请选择需要禁用的课程！"));
            }
            string[] arrayBookIDs = bookIDs.Split(new[] { ',' });
            List<tb_res_book> listBooks = new List<tb_res_book>();
            for (int i = 0; i < arrayBookIDs.Length; i++)
            {
                listBooks.Add(new tb_res_book { BookID = Convert.ToInt32(arrayBookIDs[i]), Status = 2 });
            }
            var re = base.Manage.CustomUpdateRange<tb_res_book>(o => o.BookID.ToString(), listBooks, x => x.Status.ToString(), x => x.BookID.ToString());
            if (re)
            {
                foreach (var bookID in arrayBookIDs)
                {
                    int bookid = int.Parse(bookID);
                    new ResourceBLL().GetResJson(bookid, Server.MapPath("~/Upload/BookResource/" + bookid + ".json"));
                }

                return Json(KingResponse.GetResponse("禁用成功！"));
            }
            else
            {
                return Json(KingResponse.GetErrorResponse("禁用失败！"));
            }
        }

        public JsonResult BatchStartCourse(string bookIDs)
        {
            if (string.IsNullOrWhiteSpace(bookIDs))
            {
                return Json(KingResponse.GetErrorResponse("请选择需要启用的课程！"));
            }
            string[] arrayBookIDs = bookIDs.Split(new[] { ',' });
            List<tb_res_book> listBooks = new List<tb_res_book>();
            for (int i = 0; i < arrayBookIDs.Length; i++)
            {
                listBooks.Add(new tb_res_book { BookID = Convert.ToInt32(arrayBookIDs[i]), Status = 1 });
            }
            var re = base.Manage.CustomUpdateRange<tb_res_book>(o => o.BookID.ToString(), listBooks, x => x.Status.ToString(), x => x.BookID.ToString());
            if (re)
            {
                foreach (var bookID in arrayBookIDs)
                {
                    int bookid = int.Parse(bookID);
                    new ResourceBLL().GetResJson(bookid, Server.MapPath("~/Upload/BookResource/" + bookid + ".json"));
                }
                return Json(KingResponse.GetResponse("启用成功！"));
            }
            else
            {
                return Json(KingResponse.GetErrorResponse("启用失败！"));
            }
        }

        public JsonResult BatchDelCourse(string bookIDs)
        {
            if (string.IsNullOrWhiteSpace(bookIDs))
            {
                return Json(KingResponse.GetErrorResponse("请选择需要删除的课程！"));
            }
            string[] arrayBookIDs = bookIDs.Split(new[] { ',' });
            List<string> listBookIDs = new List<string>();
            for (int i = 0; i < arrayBookIDs.Length; i++)
            {
                listBookIDs.Add(arrayBookIDs[i]);
            }
            var books = base.Manage.SelectIn<tb_res_book>("BookID", listBookIDs);
            if (books.Count > 0)
            {
                foreach (var book in books)
                {
                    if (book.Status != 0)
                    {
                        return Json(KingResponse.GetErrorResponse(book.BookName + ",该课程不可删除！"));
                    }
                }

            }
            var re = base.Manage.TranBatchDelBook(bookIDs);
            if (re)
            {
                foreach (var bookID in arrayBookIDs)
                {
                    int bookid = int.Parse(bookID);
                    new ResourceBLL().GetResJson(bookid, Server.MapPath("~/Upload/BookResource/" + bookid + ".json"));
                }
                return Json(KingResponse.GetResponse("删除成功！"));
            }
            else
            {
                return Json(KingResponse.GetErrorResponse("删除失败！"));
            }
        }
    }
}