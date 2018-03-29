using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.WebUI.Infrastructure.WebSecurity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace FXT.DataCenter.WebUI.Areas.Company.Controllers
{
    [Authorize]
    public class PublicPeiTaoController : BaseController
    {
        private readonly IPublicPeiTao _publicPeiTao;
        private readonly ILog _log;
        private readonly IDropDownList _dropDownList;

        public PublicPeiTaoController(IPublicPeiTao publicPeiTao, ILog log, IDropDownList dropDownList)
        {
            this._publicPeiTao = publicPeiTao;
            this._log = log;
            this._dropDownList = dropDownList;
        }

        public ActionResult Index(string name, int? pageIndex)
        {
            var companys = _publicPeiTao.GetPeiTaoList(Passport.Current.CityId, 1);
            var model = companys.ToPagedList(pageIndex ?? 1, 30);
            return View(model);
        }

    }
}
