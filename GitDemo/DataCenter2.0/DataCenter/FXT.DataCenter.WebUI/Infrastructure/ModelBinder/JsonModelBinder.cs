using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace FXT.DataCenter.WebUI.Infrastructure.ModelBinder
{
    public class JsonModelBinderAttribute : CustomModelBinderAttribute
    {
        public override IModelBinder GetBinder()
        {
            return new JsonBinder();
        }
    }

    public class JsonBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            //return base.BindModel(controllerContext, bindingContext);
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }

            if (bindingContext == null)
            {
                throw new ArgumentNullException("bindingContext");
            }

            var prefix = bindingContext.ModelName;
            string jsonString = controllerContext.RequestContext.HttpContext.Request.Params[prefix];
            if (jsonString != null)
            {
                var serializer = new JavaScriptSerializer();
                var result = serializer.Deserialize(jsonString, bindingContext.ModelType);
                return result;

            }
            else
            {
                return null;
            }

        }
    }
}
