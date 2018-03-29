namespace FxtDataAcquisition.Web.Common
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using Ninject;
    using FxtDataAcquisition.Domain;
    using FxtDataAcquisition.Domain.Models;
    using FxtDataAcquisition.Framework.Ioc;

    /// <summary>
    /// 模型绑定
    /// </summary>
    public class ModelBinder : DefaultModelBinder
    {
        private static readonly Regex LISTREG = new Regex(@"[\d+]", RegexOptions.Compiled);
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            var controller = controllerContext.Controller as BaseController;
            if (controller != null)
            {

            var rid = controllerContext.RouteData.Values["Id"];
            var id = rid != null ? rid.ToString() : "";
            if (string.IsNullOrEmpty(id))
                id = controllerContext.HttpContext.Request["id"];
            if (string.IsNullOrEmpty(id))
                id = controllerContext.HttpContext.Request["Id"];
            if (string.IsNullOrEmpty(id))
                id = controllerContext.HttpContext.Request["ID"];
            if (!string.IsNullOrEmpty(id) && !LISTREG.IsMatch(bindingContext.ModelName))
            {
                int i;
                if (int.TryParse(id, out i) && i > 0)
                {
                    try
                    {
                        int a = int.Parse(id);
                        //根据类型查询实体
                        var r = controller._unitOfWork.AllotFlowRepository.GetByType(modelType, a);
                        if (r != null)
                            return r;
                    }
                    catch (Exception ex){ 

                    }
                }
            }
            }
            return base.CreateModel(controllerContext, bindingContext, modelType);
        }
    }
}