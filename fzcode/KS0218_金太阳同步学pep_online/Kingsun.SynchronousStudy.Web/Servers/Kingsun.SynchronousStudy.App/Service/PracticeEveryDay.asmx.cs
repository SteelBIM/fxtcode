using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Services;
using Kingsun.SynchronousStudy.App.Common;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.SynchronousStudy.Models;

namespace Kingsun.SynchronousStudy.App.Service
{
    /// <summary>
    /// PracticeEveryDay 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class PracticeEveryDay : System.Web.Services.WebService
    {
        BaseManagement bm = new BaseManagement();

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        /// <summary>
        /// 通过BookID获取MOD中对应的版本ID、年级ID和册别
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [WebMethod]
        public string GetBookInfo(int BookID)
        {
            //bookInfo submitData = JsonHelper.DecodeJson<bookInfo>(request.Data);
            //if (submitData == null)
            //{
            //    return ObjectToJson.GetErrorResult("当前信息为空");
            //}
            //if (submitData.BookID == 0)
            //{
            //    return ObjectToJson.GetErrorResult("当前信息有误");
            //}
            IList<TB_CurriculumManage> tc = bm.Search<TB_CurriculumManage>(" BookID=" + BookID);
            List<bookInfo> curriculum = new List<bookInfo>();
            bool Success = false;
            string Message = "";
            if (tc != null && tc.Count > 0)
            {
                Success = true;
                foreach (var item in tc)
                {
                    bookInfo cm = new bookInfo();
                    cm.BookID = item.BookID ?? 0;
                    cm.EditionID = item.EditionID;
                    cm.GradeID = item.GradeID;
                    cm.BookReel = item.BreelID;
                    curriculum.Add(cm);
                }
            }
            else
            {
                Message = "书籍信息为空";
            }

            object obj = new { Success = Success, Data = curriculum, Message = Message };
            return JsonHelper.EncodeJson(obj);
        }
    }

    public class bookInfo
    {
        public int BookID { get; set; }
        public int? EditionID { get; set; }
        public int? GradeID { get; set; }
        public int? BookReel { get; set; }
    }
}
