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
    using System.Text.RegularExpressions;
    using FxtDataAcquisition.Application.Services;
    using FxtDataAcquisition.Domain.DTO;

    /// <summary>
    /// 模板
    /// </summary>
    public class TempletController : BaseController
    {
        public TempletController(IAdminService unitOfWork)
            : base(unitOfWork)
        {

        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create(Templet templet, int type, LoginUser user)
        {
            ViewBag.FieldTemplets = _unitOfWork.FieldTempletRepository.Get(m => m.Vaild == 1).ToList();

            if (type == SYSCodeManager.DATATYPECODE_1)
            {
                ViewBag.GroupList = _unitOfWork.FieldGroupTempletRepository.Get(m => m.FxtCompanyId == user.FxtCompanyId && m.DatType == SYSCodeManager.DATATYPECODE_1 && m.Vaild == 1).OrderBy(m=>m.Sort).ToList();

                return View("CreateProject", templet ?? new Templet());
            }
            else if (type == SYSCodeManager.DATATYPECODE_2)
            {
                ViewBag.GroupList = _unitOfWork.FieldGroupTempletRepository.Get(m => m.FxtCompanyId == user.FxtCompanyId && m.DatType == SYSCodeManager.DATATYPECODE_2 && m.Vaild == 1).OrderBy(m => m.Sort).ToList();

                return View("CreateBuilding", templet ?? new Templet());
            }
            else
            {
                ViewBag.GroupList = _unitOfWork.FieldGroupTempletRepository.Get(m => m.FxtCompanyId == user.FxtCompanyId && m.DatType == SYSCodeManager.DATATYPECODE_4 && m.Vaild == 1).OrderBy(m => m.Sort).ToList();

                return View("CreateHouse", templet ?? new Templet());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formeditor"></param>
        /// <param name="templet"></param>
        /// <param name="user"></param>
        /// <param name="selects">必选字段</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(string formeditor, Templet templet, LoginUser user, List<FieldDto> selects)
        {
            AjaxResult result = new AjaxResult("保存成功！");

            #region 是否已存在

            Templet isExists = null;

            if (templet.TempletId > 0)
            {
                isExists = _unitOfWork.TempletRepository.GetBy(m => m.TempletName == templet.TempletName && m.FxtCompanyId == user.FxtCompanyId && m.Vaild == 1 && m.TempletId != templet.TempletId);
            }
            else
            {
                isExists = _unitOfWork.TempletRepository.GetBy(m => m.TempletName == templet.TempletName && m.FxtCompanyId == user.FxtCompanyId && m.Vaild == 1);
            }

            if (isExists != null)
            {
                result.Result = false;

                result.Message = "模板已存在！";

                return AjaxJson(result);
            }

            #endregion

            #region

            //var templet = new Templet();

            //获取分组标签
            var pregGroup = @"<fieldset.*?</fieldset>";

            MatchCollection matchs = Regex.Matches(formeditor, pregGroup);

            //必选
            List<string> isselects = new List<string>();

            if (matchs != null && matchs.Count > 0)
            {
                for (int i = 0; i < matchs.Count; i++)
                {
                    //取分组
                    var pregGroupName = "groupname=\"(.?|.+?)\"";

                    Match gourpMatch = Regex.Match(matchs[i].Value, pregGroupName);

                    if (!gourpMatch.Value.IsNullOrEmpty())
                    {
                        string groupName = gourpMatch.Value.Split('=')[1].Replace(@"\", "").Replace("\"", "");

                        if (!groupName.IsNullOrEmpty())
                        {
                            var fieldGroup = new FieldGroup();

                            fieldGroup.FieldGroupName = groupName;

                            fieldGroup.Sort = i;

                            fieldGroup.AddUser = user.UserName;

                            fieldGroup.AddTime = DateTime.Now;

                            fieldGroup.Vaild = 1;

                            _unitOfWork.FieldGroupRepository.Delete(m => m.TempletId == templet.TempletId);

                            templet.FieldGroups = templet.FieldGroups == null ? new List<FieldGroup>() : templet.FieldGroups;
                            //templet.FieldGroups = new List<FieldGroup>(); 

                            templet.FieldGroups.Add(fieldGroup);

                            //获取字段标签
                            var preg = @"<(img|input|textarea|select|fieldset).*?(</select>|</textarea>|</fieldset>|>)";

                            MatchCollection fieldMatchs = Regex.Matches(matchs[i].Value, preg);

                            if (fieldMatchs != null && fieldMatchs.Count > 0)
                            {
                                for (int f = 0; f < fieldMatchs.Count; f++)
                                {
                                    //取字段
                                    var pregFieldName = "fieldname=\"(.?|.+?)\"";

                                    Match fieldMatch = Regex.Match(fieldMatchs[f].Value, pregFieldName);

                                    if (!string.IsNullOrEmpty(fieldMatch.Value))
                                    {
                                        string fieldName = fieldMatch.Value.Split('=')[1].Replace(@"\", "").Replace("\"", "");

                                        if (!fieldName.IsNullOrEmpty())
                                        {
                                            //字段类型
                                            var pregFieldType = "fieldtype=\"(.?|.+?)\"";

                                            Match fieldTypeMatch = Regex.Match(fieldMatchs[f].Value, pregFieldType);

                                            string fieldType = fieldTypeMatch.Value.Split('=')[1].Replace(@"\", "").Replace("\"", "");

                                            //标题
                                            var pregFieldTitle = "title=\"(.?|.+?)\"";

                                            Match fieldTitleMatch = Regex.Match(fieldMatchs[f].Value, pregFieldTitle);

                                            string fieldTitle = fieldTitleMatch.Value.Split('=')[1].Replace(@"\", "").Replace("\"", "");

                                            //字段最大长度
                                            var pregFieldMaxLength = "fieldmaxlength=\"(.?|.+?)\"";

                                            Match fieldMaxLengthMatch = Regex.Match(fieldMatchs[f].Value, pregFieldMaxLength);

                                            string[] fieldMaxLengths = fieldMaxLengthMatch.Value.Split('=');

                                            string fieldMaxLength = fieldMaxLengths.Length > 1 ? fieldMaxLengths[1].Replace(@"\", "").Replace("\"", "") : "";

                                            //字段最小长度
                                            var pregFieldMinLength = "fieldminlength=\"(.?|.+?)\"";

                                            Match fieldMinLengthMatch = Regex.Match(fieldMatchs[f].Value, pregFieldMinLength);

                                            string[] fieldMinLengths = fieldMinLengthMatch.Value.Split('=');

                                            string fieldMinLength = fieldMinLengths.Length > 1 ? fieldMaxLengths[1].Replace(@"\", "").Replace("\"", "") : "";

                                            //必填
                                            var pregFieldIsRequire = "fieldisrequire=\"(.?|.+?)\"";

                                            Match pregFieldIsRequireMatch = Regex.Match(fieldMatchs[f].Value, pregFieldIsRequire);

                                            string[] fieldIsRequires = pregFieldIsRequireMatch.Value.Split('=');

                                            string fieldIsRequire = fieldIsRequires.Length > 1 ? fieldIsRequires[1].Replace(@"\", "").Replace("\"", "") : "";

                                            //可空
                                            var pregFieldIsNull = "fieldisnull=\"(.?|.+?)\"";

                                            Match pregFieldIsNullMatch = Regex.Match(fieldMatchs[f].Value, pregFieldIsNull);

                                            string[] fieldIsNulls = pregFieldIsNullMatch.Value.Split('=');

                                            string fieldIsNull = fieldIsNulls.Length > 1 ? fieldIsNulls[1].Replace(@"\", "").Replace("\"", "") : "";

                                            //必选
                                            var pregFieldIsSelect = "fieldisselect=\"(.?|.+?)\"";

                                            Match pregFieldIsSelectMatch = Regex.Match(fieldMatchs[f].Value, pregFieldIsSelect);

                                            string[] fieldIsSelects = pregFieldIsSelectMatch.Value.Split('=');

                                            string fieldIsSelect = fieldIsSelects.Length > 1 ? fieldIsSelects[1].Replace(@"\", "").Replace("\"", "") : "";

                                            //字段值类型
                                            var pregFieldEditextType = "fieldeditexttype=\"(.?|.+?)\"";

                                            Match pregFieldEditextTypeMatch = Regex.Match(fieldMatchs[f].Value, pregFieldEditextType);

                                            string[] fieldeditexttypes = pregFieldEditextTypeMatch.Value.Split('=');

                                            string fieldeditexttype = fieldeditexttypes.Length > 1 ? fieldeditexttypes[1].Replace(@"\", "").Replace("\"", "") : "";

                                            //默认值
                                            var pregfielddefultvalue = "fielddefultvalue=\"(.?|.+?)\"";

                                            Match pregfielddefultvalueMatch = Regex.Match(fieldMatchs[f].Value, pregfielddefultvalue);

                                            string[] fielddefultvalues = pregfielddefultvalueMatch.Value.Split('=');

                                            string fielddefultvalue = fielddefultvalues.Length > 1 ? fielddefultvalues[1].Replace(@"\", "").Replace("\"", "") : "";

                                            //选项值
                                            //var pregfieldchoise = "fieldchoise=\"(.?|.+?)\"";

                                            //Match pregfieldchoiseMatch = Regex.Match(fieldMatchs[f].Value, pregfieldchoise);

                                            //string[] fieldchoises = pregfieldchoiseMatch.Value.Split('=');

                                            //string fieldchoise = fieldchoises.Length > 1 ? fieldchoises[1].Replace(@"\", "").Replace("\"", "") : "";

                                            var field = new Field();

                                            field.FieldName = fieldName;

                                            field.Title = fieldTitle;

                                            int maxlength = 0;
                                            if (int.TryParse(fieldMaxLength, out maxlength))
                                            {
                                                field.MaxLength = maxlength;
                                            }

                                            int minlength = 0;
                                            if (int.TryParse(fieldMinLength, out maxlength))
                                            {
                                                field.MinLength = minlength;
                                            }

                                            int isrequire = 0;
                                            if (int.TryParse(fieldIsRequire, out isrequire))
                                            {
                                                field.IsRequired = isrequire;
                                            }
                                            else
                                            {
                                                field.IsRequired = 0;
                                            }

                                            int isnull = 0;
                                            if (int.TryParse(fieldIsNull, out isnull))
                                            {
                                                field.IsNull = isnull;
                                            }
                                            else
                                            {
                                                field.IsNull = 0;
                                            }

                                            int editexttype = 0;
                                            if (int.TryParse(fieldeditexttype, out editexttype))
                                            {
                                                field.EdiTextType = editexttype;
                                            }

                                            int isselect = 0;
                                            if (int.TryParse(fieldIsSelect, out isselect) && isselect > 0)
                                            {
                                                isselects.Add(fieldName);
                                                field.IsSelect = isselect;
                                            }
                                            else
                                            {
                                                field.IsSelect = 0;
                                            }

                                            field.DefaultValue = fielddefultvalue;

                                            //field.Choise = fieldchoise;

                                            int filetype = 0;
                                            if (int.TryParse(fieldType, out filetype))
                                            {
                                                field.FieldType = filetype;

                                                switch (filetype)
                                                {
                                                    case 1:
                                                        field.Type = "E";
                                                        break;
                                                    case 2:
                                                        field.Type = "T";
                                                        break;
                                                    case 3:
                                                        field.Type = "R";
                                                        break;
                                                    case 5:
                                                        field.Type = "C";
                                                        break;
                                                    case 6:
                                                        field.Type = "DT";
                                                        break;
                                                    default:
                                                        break;
                                                }
                                            }

                                            field.Sort = f;

                                            field.AddUser = user.UserName;

                                            field.AddTime = DateTime.Now;

                                            field.Vaild = 1;

                                            _unitOfWork.FieldRepository.Delete(m => m.FieldGroupId == fieldGroup.FieldGroupId);

                                            fieldGroup.Fields = fieldGroup.Fields == null ? new List<Field>() : fieldGroup.Fields;
                                            //fieldGroup.Fields = new List<Field>(); 

                                            fieldGroup.Fields.Add(field);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //必选
            if (selects != null && selects.Count() > 0)
            {
                var isse = selects.Select(m => m.FieldName).Except(isselects);

                if (isse != null && isse.Count() > 0)
                {
                    string msg = string.Join(",", selects.Where(m => isse.Contains(m.FieldName)).Select(m=>m.Title));

                    result.Result = false;

                    result.Message = "请选择必选字段：" + msg;

                    return AjaxJson(result);
                }
            }

            //templet.TempletName = formname;

            if (templet.TempletId > 0)
            {
                templet.SaveUser = user.UserName;

                templet.SaveTime = DateTime.Now;
            }
            else
            {
                templet.AddUser = user.UserName;

                templet.AddTime = DateTime.Now;

                //templet.DatType = datType;

                templet.FxtCompanyId = user.FxtCompanyId;

                templet.Vaild = 1;

                _unitOfWork.TempletRepository.Insert(templet);
            }

            _unitOfWork.Commit();

            #endregion

            return AjaxJson(result);
        }

        public ActionResult LoadData(string templetName, int dataType, int pageIndex = 1, int pageSize = 10)
        {
            AjaxResult result = new AjaxResult("");

            #region 查询条件

            var filter = PredicateBuilder.True<Templet>();

            filter = filter.And(m => m.Vaild == 1);

            if (!templetName.IsNullOrEmpty())
            {
                filter = filter.And(m => m.TempletName.Contains(templetName));
            }

            if (dataType > 0)
            {
                filter = filter.And(m => m.DatType == dataType);
            }

            #endregion

            int count = _unitOfWork.TempletRepository.Get(filter).Count();

            var list = _unitOfWork.TempletRepository.Get(filter).OrderByDescending(m => m.IsCurrent).ThenBy(m => m.DatType).Skip((pageIndex - 1) * pageSize).Take(pageSize);

            result.Data = new { count = count, list = list };

            return AjaxJson(result, false);
        }

        public ActionResult Delete(List<int> ids)
        {
            AjaxResult result = new AjaxResult("删除成功！");

            var templets = _unitOfWork.TempletRepository.Get(m => ids.Contains(m.TempletId));

            foreach (var templet in templets)
            {
                templet.Vaild = 0;

                _unitOfWork.TempletRepository.Update(templet);
            }

            _unitOfWork.Commit();

            return AjaxJson(result);
        }

        /// <summary>
        /// 设置默认模板
        /// </summary>
        /// <param name="templet"></param>
        /// <returns></returns>
        public ActionResult SetCurrent(Templet templet, LoginUser user)
        {
            AjaxResult result = new AjaxResult("设置成功！");

            templet.IsCurrent = true;

            templet.SaveTime = DateTime.Now;

            templet.SaveUser = user.UserName;

            var templets = _unitOfWork.TempletRepository.Get(m => m.FxtCompanyId == user.FxtCompanyId && m.DatType == templet.DatType && m.TempletId != templet.TempletId);

            foreach (var temp in templets)
            {
                if (temp.IsCurrent)
                {
                    temp.IsCurrent = false;

                    temp.SaveTime = DateTime.Now;

                    temp.SaveUser = user.UserName;

                    _unitOfWork.TempletRepository.Update(temp);
                }
            }

            _unitOfWork.Commit();

            return AjaxJson(result);
        }
    }
}