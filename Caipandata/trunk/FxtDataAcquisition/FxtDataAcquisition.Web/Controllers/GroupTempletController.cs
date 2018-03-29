namespace FxtDataAcquisition.Web.Controllers
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Collections.Generic;

    using FxtDataAcquisition.Common;
    using FxtDataAcquisition.Web.Common;
    using FxtDataAcquisition.Application.Interfaces;
    using FxtDataAcquisition.Domain.Models;
    using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;

    /// <summary>
    /// 分组模板
    /// </summary>
    public class GroupTempletController : BaseController
    {
        public GroupTempletController(IAdminService unitOfWork)
            : base(unitOfWork)
        {

        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create(FieldGroupTemplet groupTemplet)
        {
            return View(groupTemplet ?? new FieldGroupTemplet());
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(FieldGroupTemplet groupTemplet, LoginUser loginUserInfo)
        {
            AjaxResult result = new AjaxResult("保存成功！");

            #region 判断是否已存在

            FieldGroupTemplet exists = null;

            if (groupTemplet.FieldGroupTempletId > 0)
            {
                exists = _unitOfWork.FieldGroupTempletRepository.GetBy(m => m.FieldGroupTempletName == groupTemplet.FieldGroupTempletName && m.FxtCompanyId == loginUserInfo.FxtCompanyId

                    && m.DatType == groupTemplet.DatType && m.Vaild == 1 && m.FieldGroupTempletId != groupTemplet.FieldGroupTempletId);
            }
            else
            {
                exists = _unitOfWork.FieldGroupTempletRepository.GetBy(m => m.FieldGroupTempletName == groupTemplet.FieldGroupTempletName && m.FxtCompanyId == loginUserInfo.FxtCompanyId

                    && m.DatType == groupTemplet.DatType && m.Vaild == 1);
            }

            if (exists != null)
            {
                result.Result = false;

                result.Message = "分组已存在！";

                return AjaxJson(result);
            }

            #endregion

            if (groupTemplet.FieldGroupTempletId > 0)
            {
                groupTemplet.SaveTime = DateTime.Now;

                groupTemplet.SaveUser = loginUserInfo.UserName;

            }
            else
            {
                groupTemplet.AddTime = DateTime.Now;

                groupTemplet.AddUser = loginUserInfo.UserName;

                groupTemplet.FxtCompanyId = loginUserInfo.FxtCompanyId;

                groupTemplet.Vaild = 1;

                result.Data = _unitOfWork.FieldGroupTempletRepository.Insert(groupTemplet);
            }

            _unitOfWork.Commit();

            return AjaxJson(result);
        }

        public ActionResult LoadData(string name, int dataType, LoginUser loginUserInfo, int pageIndex = 1, int pageSize = 10)
        {
            AjaxResult result = new AjaxResult("");

            #region 查询条件

            var filter = PredicateBuilder.True<FieldGroupTemplet>();

            filter = filter.And(m => m.Vaild == 1);

            if (!name.IsNullOrEmpty())
            {
                filter = filter.And(m => m.FieldGroupTempletName.Contains(name));
            }

            if (dataType > 0)
            {
                filter = filter.And(m => m.DatType == dataType);
            }

            #endregion

            int count = _unitOfWork.FieldGroupTempletRepository.Get(filter).Count();

            var list = _unitOfWork.FieldGroupTempletRepository.Get(filter).OrderByDescending(m => m.AddTime).ThenBy(m => m.DatType).Skip((pageIndex - 1) * pageSize).Take(pageSize);

            result.Data = new { count = count, list = list };

            return AjaxJson(result, false);
        }

        public ActionResult Delete(List<int> ids)
        {
            AjaxResult result = new AjaxResult("删除成功！");

            var templets = _unitOfWork.FieldGroupTempletRepository.Get(m => ids.Contains(m.FieldGroupTempletId));

            if (templets != null && templets.Count() > 0)
            {
                foreach (var templet in templets)
                {
                    //_unitOfWork.FieldGroupRepository.Get(m=> m.FieldGroupName == templet && m.)


                    templet.Vaild = 0;

                    _unitOfWork.FieldGroupTempletRepository.Update(templet);
                }
            }

            _unitOfWork.Commit();

            return AjaxJson(result);
        }
    }
}