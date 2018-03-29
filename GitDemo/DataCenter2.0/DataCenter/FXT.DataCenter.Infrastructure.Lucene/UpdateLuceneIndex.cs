using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading;
using Lucene.Net.Store;
using System.IO;
using Lucene.Net.Index;
using Lucene.Net.Documents;
using Lucene.Net.Analysis.PanGu;

namespace FXT.DataCenter.Lucene
{
   public class UpdateLuceneIndex
    {
        public static readonly UpdateLuceneIndex index = new UpdateLuceneIndex();
        public static readonly string indexPath = HttpContext.Current.Server.MapPath("~/IndexData");
        private UpdateLuceneIndex()
        {
        }
        //请求队列 解决索引目录同时操作的并发问题
        private Queue<SearchViewMode> queue = new Queue<SearchViewMode>();
        /// <summary>
        /// 新增表信息时 添加邢增索引请求至队列
        /// </summary>
        /// <param name="books"></param>
        public void Add(int id, string title, string content)
        {
            SearchViewMode svm = new SearchViewMode();
            svm.Id = id;
            svm.Title = title;
            svm.Content = content;
            svm.IT = IndexType.Insert;
            queue.Enqueue(svm);
        }
        /// <summary>
        /// 删除表信息时 添加删除索引请求至队列
        /// </summary>
        /// <param name="bid"></param>
        public void Del(int id) {
            SearchViewMode svm = new SearchViewMode();
            svm.Id = id;
            svm.IT = IndexType.Delete;
            queue.Enqueue(svm);
        }
        /// <summary>
        /// 修改表信息时 添加修改索引(实质上是先删除原有索引 再新增修改后索引)请求至队列
        /// </summary>
        /// <param name="books"></param>
        public void Mod(int id, string title, string content)
        {
            SearchViewMode svm = new SearchViewMode();
            svm.Id = id;
            svm.Title = title;
            svm.Content = content;
            svm.IT = IndexType.Modify;
            queue.Enqueue(svm);
        }

        public void StartNewThread() {
            ThreadPool.QueueUserWorkItem(new WaitCallback(QueueToIndex));
        }

        //定义一个线程 将队列中的数据取出来 插入索引库中
        private void QueueToIndex(object para) {
            while(true) {
                if (queue.Count > 0)
                {
                    CRUDIndex();
                } else {
                    Thread.Sleep(3000);
                }
            }
        }
        /// <summary>
        /// 更新索引库操作
        /// </summary>
        private void CRUDIndex() {
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NativeFSLockFactory());
            bool isExist = IndexReader.IndexExists(directory);
            if(isExist) {
                if(IndexWriter.IsLocked(directory)) {
                    IndexWriter.Unlock(directory);
                }
            }
            IndexWriter writer = new IndexWriter(directory, new PanGuAnalyzer(), !isExist, IndexWriter.MaxFieldLength.UNLIMITED);
            while (queue.Count > 0)
            {
                Document document = new Document();
                SearchViewMode model = queue.Dequeue();
                if (model.IT == IndexType.Insert)
                {
                    document.Add(new Field("id", model.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("url", model.Url.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("title", model.Content, Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("content", model.Content, Field.Store.YES, Field.Index.ANALYZED,
                                          Field.TermVector.WITH_POSITIONS_OFFSETS));
                    writer.AddDocument(document);
                }
                else if (model.IT == IndexType.Delete)
                {
                    writer.DeleteDocuments(new Term("id", model.Id.ToString()));
                }
                else if (model.IT == IndexType.Modify)
                {
                    //先删除 再新增
                    writer.DeleteDocuments(new Term("id", model.Id.ToString()));
                    document.Add(new Field("id", model.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("url", model.Url.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("title", model.Content, Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("content", model.Content, Field.Store.YES, Field.Index.ANALYZED,
                                          Field.TermVector.WITH_POSITIONS_OFFSETS));
                    writer.AddDocument(document);
                }
            }
            writer.Close();
            directory.Close();
        }
    }
   public class SearchViewMode
   {
       public int Id
       {
           get;
           set;
       }
       public string Title
       {
           get;
           set;
       }
       public string Url
       {
           get;
           set;
       }
       public string Content
       {
           get;
           set;
       }
       public int FxtCompanyId
       {
           get;
           set;
       }
       public int CityId
       {
           get;
           set;
       }

       public IndexType IT
       {
           get;
           set;
       }
   }
   //操作类型枚举
   public enum IndexType
   {
       Insert,
       Modify,
       Delete
   }
}
