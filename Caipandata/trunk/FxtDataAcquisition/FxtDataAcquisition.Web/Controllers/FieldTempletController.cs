namespace FxtDataAcquisition.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Collections.Generic;

    using FxtDataAcquisition.Common;
    using FxtDataAcquisition.Web.Common;
    using FxtDataAcquisition.Application.Interfaces;
    using FxtDataAcquisition.Domain.Models;
    using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;

    using System.Linq.Expressions;

    /// <summary>
    /// 字段模板
    /// </summary>
    public class FieldTempletController : BaseController
    {
        public FieldTempletController(IAdminService unitOfWork)
            : base(unitOfWork)
        {

        }

        public ActionResult Index()
        {
            return View();
        }
    
        public ActionResult Create(FieldTemplet groupTemplet)
        {
            return View(groupTemplet ?? new FieldTemplet());
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(FieldTemplet templet, LoginUser loginUserInfo)
        {
            AjaxResult result = new AjaxResult("保存成功！");

            #region 判断是否已存在

            FieldTemplet exists = null;

            if (templet.FieldTempletId > 0)
            {
                exists = _unitOfWork.FieldTempletRepository.GetBy(m => m.FieldName == templet.FieldName && m.DatType == templet.DatType && m.Vaild == 1 && m.FieldTempletId != templet.FieldTempletId);
            }
            else
            {
                exists = _unitOfWork.FieldTempletRepository.GetBy(m => m.FieldName == templet.FieldName && m.DatType == templet.DatType && m.Vaild == 1);
            }

            if (exists != null)
            {
                result.Result = false;

                result.Message = "字段已存在！";

                return AjaxJson(result);
            }

            #endregion

            if (templet.FieldTempletId > 0)
            {
                templet.SaveTime = DateTime.Now;

                templet.SaveUser = loginUserInfo.UserName;

            }
            else
            {
                templet.AddTime = DateTime.Now;

                templet.AddUser = loginUserInfo.UserName;

                templet.Vaild = 1;

                result.Data = _unitOfWork.FieldTempletRepository.Insert(templet);
            }

            _unitOfWork.Commit();

            return AjaxJson(result);
        }

        public ActionResult LoadData(string name, string title, int dataType, LoginUser loginUserInfo, int pageIndex = 1, int pageSize = 10)
        {
            AjaxResult result = new AjaxResult("");

            #region 查询条件

            var filter = PredicateBuilder.True<FieldTemplet>();

            filter = filter.And(m => m.Vaild == 1);

            if (!name.IsNullOrEmpty())
            {
                filter = filter.And(m => m.FieldName.Contains(name));
            }

            if (!title.IsNullOrEmpty())
            {
                filter = filter.And(m => m.Title.Contains(title));
            }

            if (dataType > 0)
            {
                filter = filter.And(m => m.DatType == dataType);
            }

            #endregion

            int count = _unitOfWork.FieldTempletRepository.Get(filter).Count();

            var list = _unitOfWork.FieldTempletRepository.Get(filter).OrderByDescending(m => m.AddTime).ThenBy(m => m.DatType).Skip((pageIndex - 1) * pageSize).Take(pageSize);

            result.Data = new { count = count, list = list };

            return AjaxJson(result, false);
        }

        public ActionResult Delete(List<int> ids)
        {
            AjaxResult result = new AjaxResult("删除成功！");

            var templets = _unitOfWork.FieldTempletRepository.Get(m => ids.Contains(m.FieldTempletId));

            foreach (var templet in templets)
            {
                templet.Vaild = 0;

                _unitOfWork.FieldTempletRepository.Update(templet);
            }

            _unitOfWork.Commit();

            return AjaxJson(result);
        }
    }
}