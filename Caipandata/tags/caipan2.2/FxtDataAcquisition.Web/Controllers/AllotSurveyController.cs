using FxtDataAcquisition.Application.Interfaces;
using FxtDataAcquisition.Application.Services;
using FxtDataAcquisition.Web.Common;
using System.Linq;
using System.Web.Mvc;
using FxtDataAcquisition.Domain.Models;

namespace FxtDataAcquisition.Web.Controllers
{
    public class AllotSurveyController : BaseController
    {
        public AllotSurveyController(IAdminService unitOfWork)
            : base(unitOfWork)
        {
        }

        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.OPEN, NowFunctionPageUrl = WebCommon.Url_AllotFlowInfo_AllotFlowManager, OrNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_1, SYSCodeManager.FunOperCode_2, SYSCodeManager.FunOperCode_3 })]
        public ActionResult Index(int allotId)
        {
            var surveys = _unitOfWork.AllotSurveyRepository.Get(m => m.AllotId == allotId).OrderBy(m => m.StateDate).ToList();

            return View(surveys);
        }
    }
}