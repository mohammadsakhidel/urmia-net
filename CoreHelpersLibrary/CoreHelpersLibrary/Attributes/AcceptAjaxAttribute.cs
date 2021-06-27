using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoreHelpers
{
    public sealed class AcceptAjaxAttribute : ActionMethodSelectorAttribute
    {
        public override bool IsValidForRequest(ControllerContext controllerContext, System.Reflection.MethodInfo methodInfo)
        {
            if (controllerContext == null)
                throw new ArgumentNullException("controllerContext");

            return controllerContext.HttpContext.Request.IsAjaxRequest();
        }
    }
}