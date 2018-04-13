using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CourseActivate.Activate.BLL;
using CourseActivate.Core.Utility;

namespace CourseActivate.Web.Admin.Controllers
{
    public class ActivateController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

        //public IHttpActionResult ApiActivateCourse([FromBody]ActivateCourse model)
        //{
        //    if (model == null)
        //    {
        //        return Json(KingResponse.GetErrorResponse("传入的参数不正确"));
        //    }
        //    ActivateCourseBLL bll = new ActivateCourseBLL();
        //    string bookid = model.bookid.HasValue ? model.bookid.Value.ToString() : "";
        //    KingResponse res = bll.ActivateCourse(model.userid, model.username, bookid, model.activatecode, model.devicetype.Value, model.devicecode);
        //    return Json(res);
        //}


    }

    public class ActivateCourse
    {
        public string userid { get; set; }
        public string username { get; set; }
        public string activatecode { get; set; }
        public int? devicetype { get; set; }
        public string devicecode { get; set; }
        public int? bookid { get; set; }
    }

}