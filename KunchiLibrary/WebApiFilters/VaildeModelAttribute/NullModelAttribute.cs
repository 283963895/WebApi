using KunchiLibrary.JsonCommon;
using KunchiLibrary.WebAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace KunchiLibrary.WebApiFilters.VaildeModelAttribute
{
  public  class NullModelAttribute : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {

            if (!actionContext.ModelState.IsValid)
            {
                WebAPiResults result = new WebAPiResults() { Success = false,Code= "E100009",Message= "请求无效",StatusCode= HttpStatusCode.BadRequest };

                foreach (var item in actionContext.ModelState.Values)
                {
                    foreach (var error in item.Errors)
                    {
                        result.ErrorMessage += error.ErrorMessage + "|";
                    }
                }

                actionContext.Response = JsonHelper.toJson(result);
            }
            //base.OnActionExecuting(actionContext);
        }
    }
}
