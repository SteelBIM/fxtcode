using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Infrastructure.Data.ServicesImpl;
using FXT.DataCenter.Lucene;
using Lucene.Net.Store;
using Lucene.Net.Index;
using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Documents;
using Lucene.Net.Search;

namespace FXT.DataCenter.Infrastructure.Lucene
{
    public class GoLucene
    {
        /// <summary>
        /// 创建索引
        /// </summary>
        public static void CreateIndexByData()
        {
            string indexPath = HttpContext.Current.Server.MapPath("~/IndexData");//索引文档保存位置         
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NativeFSLockFactory());
            //IndexReader:对索引库进行读取的类
            bool isExist = IndexReader.IndexExists(directory); //是否存在索引库文件夹以及索引库特征文件
            if (isExist)
            {
                //如果索引目录被锁定（比如索引过程中程序异常退出或另一进程在操作索引库），则解锁
                //Q:存在问题 如果一个用户正在对索引库写操作 此时是上锁的 而另一个用户过来操作时 将锁解开了 于是产生冲突 --解决方法后续
                if (IndexWriter.IsLocked(directory))
                {
                    IndexWriter.Unlock(directory);
                }
            }

            //创建向索引库写操作对象  IndexWriter(索引目录,指定使用盘古分词进行切词,最大写入长度限制)
            //补充:使用IndexWriter打开directory时会自动对索引库文件上锁
            IndexWriter writer = new IndexWriter(directory, new PanGuAnalyzer(), !isExist, IndexWriter.MaxFieldLength.UNLIMITED);

            #region 把数据添加到索引库

            //var landCase = new LandCase().GetLandCases(new FXT.DataCenter.Entity.POCO.DataBase_FXTLand.DAT_CaseLand(), false);
            var landCase = new List<DAT_CaseLand>();
            //var projectCase = new ProjectCase().GetProjectCase(new Entity.ProjectCaseParams() {casedateStart=Convert.ToDateTime("1900-01-01"),casedateEnd=DateTime.Now}, false);
            var projectCase = new List<DAT_Case>();

            //--------------------------------遍历数据源 将数据转换成为文档对象 存入索引库
            foreach (var item in landCase)
            {
                Document document = new Document(); //new一篇文档对象 --一条记录对应索引库中的一个文档

                //向文档中添加字段  Add(字段,值,是否保存字段原始值,是否针对该列创建索引)
                document.Add(new Field("id", item.caseid.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));//--所有字段的值都将以字符串类型保存 因为索引库只存储字符串类型数据

                //Field.Store:表示是否保存字段原值。指定Field.Store.YES的字段在检索时才能用document.Get取出原值  
                //Field.Index.NOT_ANALYZED:指定不按照分词后的结果保存--是否按分词后结果保存取决于是否对该列内容进行模糊查询
                document.Add(new Field("url", "/Land/LandCase/Index", Field.Store.YES, Field.Index.NOT_ANALYZED));
                document.Add(new Field("title", "土地案例数据", Field.Store.YES, Field.Index.NOT_ANALYZED));
                document.Add(new Field("fxtcompanyid", item.fxtcompanyid.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                document.Add(new Field("cityid", item.cityid.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));

                //Field.Index.ANALYZED:指定文章内容按照分词后结果保存 否则无法实现后续的模糊查询 
                //WITH_POSITIONS_OFFSETS:指示不仅保存分割后的词 还保存词之间的距离
                document.Add(new Field("content", GetCaseLandContent(item), Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));

                writer.AddDocument(document); //文档写入索引库
            }
            foreach (var item in projectCase)
            {
                Document document = new Document(); //new一篇文档对象 --一条记录对应索引库中的一个文档

                //向文档中添加字段  Add(字段,值,是否保存字段原始值,是否针对该列创建索引)
                document.Add(new Field("id", item.caseid.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));//--所有字段的值都将以字符串类型保存 因为索引库只存储字符串类型数据

                //Field.Store:表示是否保存字段原值。指定Field.Store.YES的字段在检索时才能用document.Get取出原值  
                //Field.Index.NOT_ANALYZED:指定不按照分词后的结果保存--是否按分词后结果保存取决于是否对该列内容进行模糊查询
                document.Add(new Field("url", "/House/projectCase/Index", Field.Store.YES, Field.Index.NOT_ANALYZED));
                document.Add(new Field("title", "楼盘案例数据", Field.Store.YES, Field.Index.NOT_ANALYZED));
                document.Add(new Field("fxtcompanyid", item.fxtcompanyid.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                document.Add(new Field("cityid", item.cityid.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                //Field.Index.ANALYZED:指定文章内容按照分词后结果保存 否则无法实现后续的模糊查询 
                //WITH_POSITIONS_OFFSETS:指示不仅保存分割后的词 还保存词之间的距离
                document.Add(new Field("content", GetProjectCaseContent(item), Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));

                writer.AddDocument(document); //文档写入索引库
            }

            #endregion


            writer.Close();//会自动解锁
            directory.Close(); //不要忘了Close，否则索引结果搜不到
        }

        private static string GetCaseLandContent(DAT_CaseLand item)
        {
            var _dropDownList = new DropDownList();
            //买卖方式
            var barginType = string.Join("|", _dropDownList.GetDictById(3004).Select(m => m.CodeName));
            //土地用途
            var landUse = string.Join("|", _dropDownList.GetLandPurpose(1001).Select(m => m.CodeName));
            //土地来源
            var landSource = string.Join("|", _dropDownList.GetDictById(3002).Select(m => m.CodeName));

            return item.landno  + item.landaddress  + barginType  + landUse  + landSource  + item.winner  + item.seller;
        }

        private static string GetProjectCaseContent(DAT_Case item)
        {
            var _dropDownList = new DropDownList();
            //居住用途
            var purpose = string.Join("|", _dropDownList.GetDictById(1002).Select(m => m.CodeName));
            //建筑类型
            var buildingType = string.Join("|", _dropDownList.GetDictById(2003).Select(m => m.CodeName));
            //案例类型
            var caseType = string.Join("|", _dropDownList.GetDictById(3001).Select(m => m.CodeName));
            //户型
            var houseType = string.Join("|", _dropDownList.GetDictById(4001).Select(m => m.CodeName));


            return item.ProjectName + purpose + buildingType + caseType + houseType;
        }
        /// <summary>
        /// 删除所有索引文件
        /// </summary>
        public static void DeleteIndex()
        {
            string indexPath = HttpContext.Current.Server.MapPath("~/IndexData");//索引文档保存位置      
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NativeFSLockFactory());
            bool isExist = IndexReader.IndexExists(directory);
            if (isExist)
            {
                if (IndexWriter.IsLocked(directory))
                {
                    IndexWriter.Unlock(directory);
                }
            }
            IndexWriter writer = new IndexWriter(directory, new PanGuAnalyzer(), !isExist, IndexWriter.MaxFieldLength.UNLIMITED);

            writer.DeleteAll();
            writer.Close();
            directory.Close();
        }

        /// <summary>
        /// 从索引库中检索关键字
        /// </summary>
        public static void SearchFromIndexData(string searchKey, out List<SearchViewMode> data)
        {
            string indexPath = HttpContext.Current.Server.MapPath("~/IndexData");
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NoLockFactory());
            IndexReader reader = IndexReader.Open(directory, true);
            IndexSearcher searcher = new IndexSearcher(reader);

            //--------------------------------------这里配置搜索条件
            //PhraseQuery query = new PhraseQuery();
            //foreach(string word in Common.SplitContent.SplitWords(Request.QueryString["SearchKey"])) {
            //    query.Add(new Term("content", word));//这里是 and关系
            //}
            //query.SetSlop(100);

            //关键词Or关系设置
            BooleanQuery queryOr = new BooleanQuery();
            TermQuery query = null;
            foreach (string word in SplitContent.SplitWords(searchKey))
            {
                query = new TermQuery(new Term("content", word));

                queryOr.Add(query, BooleanClause.Occur.SHOULD);//这里设置 条件为Or关系
            }
            //--------------------------------------
            TopScoreDocCollector collector = TopScoreDocCollector.create(1000, true);
            //searcher.Search(query, null, collector);
            searcher.Search(queryOr, null, collector);


            ScoreDoc[] docs = collector.TopDocs(0, 100).scoreDocs;//取前十条数据  可以通过它实现LuceneNet搜索结果分页
            List<SearchViewMode> list = new List<SearchViewMode>();
            for (int i = 0; i < docs.Length; i++)
            {
                int docId = docs[i].doc;
                Document doc = searcher.Doc(docId);

                var view = new SearchViewMode();
                view.Content = SplitContent.HightLight(searchKey, doc.Get("content"));
                view.Title = doc.Get("title");
                view.Url = doc.Get("url");
                view.Id = Convert.ToInt32(doc.Get("id"));
                view.FxtCompanyId = Convert.ToInt32(doc.Get("fxtcompanyid"));
                view.CityId = Convert.ToInt32(doc.Get("cityid"));
                list.Add(view);
            }

            data = list;

        }
    }
}
