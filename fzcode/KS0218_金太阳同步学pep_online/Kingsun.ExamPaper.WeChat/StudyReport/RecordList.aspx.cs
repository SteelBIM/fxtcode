
using Kingsun.ExamPaper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kingsun.ExamPaper.BLL;

namespace Kingsun.ExamPaper.WeChat.StudyReport
{
    public partial class RecordList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        { 
            if (!Page.IsPostBack)
            {
                bindInfo();
            }
        }
        public void bindInfo()
        {
            try
            {
                string ClassID = Request["ClassID"] == null ? "" : Request["ClassID"];
                int CatalogID = Request["CatalogID"] == null ? 0 : Convert.ToInt32(Request["CatalogID"]);
                if (ClassID == "" || CatalogID == 0)
                {
                    this.DivMain.InnerHtml = "<h5 style='text-align: center;'>参数错误</h5>";
                    return;
                }
                //绑定最佳成绩
                IList<StuCatalog> list = new StuCatalogBLL().GetClassStuCatalog(ClassID, CatalogID, 1).StuCatalogList;
                if (list != null && list.Count > 0)
                {
                    string contentBestHtml = "";
                    contentBestHtml += "  <p class=\"p1\">完成人数<span>" + list.Count + "</span>人</p>";

                    contentBestHtml += " <ul>";
                    foreach (var item in list)
                    {
                        IList<PQ> listPQ = item.QuestionList;
                        string pqUrl = "";
                        if (listPQ != null && listPQ.Count > 0)
                        {
                            foreach (var p in listPQ)
                            {
                                pqUrl += p.BestAnswer + ";";
                            }
                            pqUrl = pqUrl.Trim(';');
                        }
                        contentBestHtml += @"<li>
                            <p class='p2'>
                                <span>
                                    <img src='/images/report/tou.png' alt='' /><em>" + item.TrueName + @"</em></span><b><em>" + item.BestTotalScore + @"</em>分</b>
                            </p>
                            <div class='content_nr'>
                                <div class='p3'>
                                    <h2>" + item.CatalogName + @"</h2>
                                    <p>" + item.DoDate.ToString("yyyy-MM-dd") + @"</p>
                                </div>
                                <a url='" + pqUrl + @"'></a>
                            </div>
                        </li>";
                    }
                    this.contentBest.InnerHtml = contentBestHtml + "</ul>";
                }
                else
                {
                    this.contentBest.InnerHtml = "<h5 style='text-align: center;'>暂无数据</h5>";
                }
                //绑定最近成绩
                list = new StuCatalogBLL().GetClassStuCatalog(ClassID, CatalogID, 0).StuCatalogList;
                if (list != null && list.Count > 0)
                {
                    string contentBestHtml = "";
                    contentBestHtml += "  <p class=\"p1\">完成人数<span>" + list.Count + "</span>人</p>";

                    contentBestHtml += " <ul>";
                    foreach (var item in list)
                    {
                        IList<PQ> listPQ = item.QuestionList;
                        string pqUrl = "";
                        if (listPQ != null && listPQ.Count > 0)
                        {
                            foreach (var p in listPQ)
                            {
                                pqUrl += p.BestAnswer + ";";
                            }
                            pqUrl = pqUrl.Trim(';'); 
                        }
                        contentBestHtml += @"<li>
                            <p class='p2'>
                                <span>
                                    <img src='/images/report/tou.png' alt='' /><em>" + item.TrueName + @"</em></span><b><em>" + item.BestTotalScore + @"</em>分</b>
                            </p>
                            <div class='content_nr'>
                                <div class='p3'>
                                    <h2>" + item.CatalogName + @"</h2>
                                    <p>" + item.DoDate.ToString("yyyy-MM-dd") + @"</p>
                                </div>
                                 <a url='" + pqUrl + @"'></a>
                            </div>
                        </li>";
                    }
                    this.contentLately.InnerHtml = contentBestHtml + "</ul>";
                }
                else
                {
                    this.contentLately.InnerHtml = "<h5 style='text-align: center;'>暂无数据</h5>";
                }
            }
            catch (Exception ex)
            {
                this.DivMain.InnerHtml = "<h5 style='text-align: center;'>操作异常</h5>";
            }
        }
    }
}