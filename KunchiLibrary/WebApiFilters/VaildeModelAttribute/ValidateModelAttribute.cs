﻿using KunchiLibrary.JsonCommon;
using KunchiLibrary.WebAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;

namespace KunchiLibrary.WebApiFilters.VaildeModelAttribute
{
 public   class ValidateModelAttribute : ActionFilterAttribute
          
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            //base.OnActionExecuting(actionContext);
            if (actionContext.ModelState.IsValid == false)
            {
               actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, actionContext.ModelState);
            }
        }
    }
}
