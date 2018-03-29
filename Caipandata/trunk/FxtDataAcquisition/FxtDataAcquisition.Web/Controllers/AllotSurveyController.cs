namespace FxtDataAcquisition.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using FxtDataAcquisition.Common;
    using FxtDataAcquisition.Web.Common;
    using FxtDataAcquisition.Application.Interfaces;

    public class AllotSurveyController : BaseController
    {
        public AllotSurveyController(IAdminService unitOfWork)
            : base(unitOfWork)
        {
        }

        /// <summary>
        /// 任务跟踪
        /// </summary>
        /// <param name="allotId"></param>
        /// <returns></returns>
        [AuthorizeFilterAttribute(NowRequestType = RequestType.OPEN, NowFunctionPageUrl = WebCommon.Url_AllotFlowInfo_AllotFlowManager, OrNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_1, SYSCodeManager.FunOperCode_2, SYSCodeManager.FunOperCode_3 })]
        public ActionResult Index(int allotId)
        {
            var surveys = _unitOfWork.AllotSurveyRepository.Get(m => m.AllotId == allotId).OrderBy(m => m.StateDate).ToList();

            return View(surveys);
        }
    }
}