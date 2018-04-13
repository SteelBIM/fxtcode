using CourseActivate.Core.Utility;
using CourseActivate.Framework.BLL;
using CourseActivate.Framework.DAL;
using CourseActivate.Resource.Constract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Resource.BLL
{
    public class ResourceBook : Manage
    {
        Repository repository = new Repository();
        public bool TranAddBook(Resource.Constract.Model.tb_res_book book, List<Resource.Constract.Model.tb_res_catalog> firstCatalog, List<Resource.Constract.Model.tb_res_catalog> secondCatalog, List<Resource.Constract.Model.tb_res_catalog> thirdCatalog)
        {
            using (var db = repository.GetInstance())
            {
                db.CommandTimeOut = 30000;//设置超时时间
                try
                {
                    db.BeginTran();//开启事务
                    int bookID;
                    if (book.BookID == 0)
                    {
                        bookID = Convert.ToInt32(db.Insert(book));
                        book.BookID = bookID;
                    }
                    else
                    {
                        bookID = book.BookID;
                        db.Update(book);

                        db.Delete<Resource.Constract.Model.tb_res_catalog>("BookID =" + bookID);
                    }

                    Dictionary<int, int> first = new Dictionary<int, int>();
                    Dictionary<int, int> second = new Dictionary<int, int>();

                    for (int i = 0; i < firstCatalog.Count; i++)
                    {
                        firstCatalog[i].BookID = bookID;
                        firstCatalog[i].Sort = i + 1;
                        var firstCatalogID = db.Insert(firstCatalog[i]);
                        first.Add(i, Convert.ToInt32(firstCatalogID));
                    }

                    if (secondCatalog.Any())
                    {
                        int firstParentID = first[secondCatalog[0].ParentID];
                        for (int i = 0; i < secondCatalog.Count; i++)
                        {
                            secondCatalog[i].BookID = bookID;
                            secondCatalog[i].Sort = i + 1;
                            int key = secondCatalog[i].ParentID;
                            int parentID = first[key];
                            if (parentID != firstParentID)
                            {
                                db.Update<tb_res_catalog>(new { PageNoEnd = secondCatalog[i - 1].PageNoEnd }, x => x.CatalogID == firstParentID);
                                firstParentID = parentID;
                            }
                            secondCatalog[i].ParentID = parentID;
                            var secondCatalogID = db.Insert(secondCatalog[i]);
                            second.Add(i, Convert.ToInt32(secondCatalogID));
                            if (i == secondCatalog.Count - 1)
                            {
                                db.Update<tb_res_catalog>(new { PageNoEnd = secondCatalog[i].PageNoEnd }, x => x.CatalogID == parentID);
                            }
                        }

                        if (thirdCatalog.Any())
                        {
                            for (int i = 0; i < thirdCatalog.Count; i++)
                            {
                                thirdCatalog[i].BookID = bookID;
                                thirdCatalog[i].Sort = i + 1;
                                int key = thirdCatalog[i].ParentID;
                                int parentID = second[key];
                                thirdCatalog[i].ParentID = parentID;
                                var thirdCatalogID = db.Insert(thirdCatalog[i]);
                            }
                        }
                    }


                    db.CommitTran();//提交事务
                    return true;
                }
                catch (Exception)
                {
                    db.RollbackTran();//回滚事务
                    throw;
                }
            }
        }

        public bool TranDelBook(int bookID)
        {
            using (var db = repository.GetInstance())
            {
                db.CommandTimeOut = 30000;//设置超时时间
                try
                {
                    db.BeginTran();//开启事务
                    db.Delete<tb_res_book>("BookID=" + bookID);
                    db.Delete<tb_res_catalog>("BookID=" + bookID);
                    db.Delete<tb_res_resource>("BookID=" + bookID);
                    db.CommitTran();//提交事务
                    return true;
                }
                catch (Exception)
                {
                    db.RollbackTran();//回滚事务
                    throw;
                }
            }
        }

        public bool TranBantchDelBook(string bookIDs)
        {
            using (var db = repository.GetInstance())
            {
                db.CommandTimeOut = 30000;//设置超时时间
                try
                {
                    db.BeginTran();//开启事务
                    db.Delete<tb_res_book>("BookID in (" + bookIDs + ")");
                    db.Delete<tb_res_catalog>("BookID in (" + bookIDs + ")");
                    db.Delete<tb_res_resource>("BookID in (" + bookIDs + ")");
                    db.CommitTran();//提交事务
                    return true;
                }
                catch (Exception)
                {
                    db.RollbackTran();//回滚事务
                    throw;
                }
            }
        }
    }
}
