using KunchiLibrary.JsonCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Diagnostics;
using System.Net;
/*
作用： 规范WebAPI返回信息的统一性
作者： Jason
时间： 2018-09-27
说明： 该类是为WebAPI提供返回值信息的统一类
*/
namespace KunchiLibrary.WebAPI
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class WebApiResultAttributecs :  ActionFilterAttribute
    {
        private const string Key = "action";
        private bool _IsDebugLog = true;
        public override void OnActionExecuting(HttpActionContext actionContext)
        {

            base.OnActionExecuting(actionContext);
            if (!actionContext.ModelState.IsValid)
            {
           
                WebAPiResults result = new WebAPiResults() { Success = false, Code = "E100009", Message = "请求无效", StatusCode = HttpStatusCode.BadRequest };

                foreach (var item in actionContext.ModelState.Values)
                {
                    foreach (var error in item.Errors)
                    {
                        result.ErrorMessage += error.ErrorMessage + "|";
                    }
                }

                HttpResponseMessage httpResponseMessage = JsonHelper.toJson(result);
            }
 
            if (_IsDebugLog)
            {
                Stopwatch stopWatch = new Stopwatch();

                actionContext.Request.Properties[Key] = stopWatch;

                string actionName = actionContext.ActionDescriptor.ActionName;

                // 获取域名
                string domainName = actionContext.Request.RequestUri.AbsoluteUri;

                //获取模块名称
                //  module = filterContext.HttpContext.Request.Url.Segments[1].Replace('/', ' ').Trim();

                //获取 controllerName 名称
                string controllerName = actionContext.Request.RequestUri.Authority;

                //获取ACTION 名称
                string actionName1 = actionContext.ActionDescriptor.ActionName;

                Debug.Print(getParametes(actionContext));

                Debug.Print(actionContext.Request.RequestUri.AbsoluteUri);

                stopWatch.Start();
            }

        }
        private string getParametes(HttpActionContext actionContext)
        {
            string postStr = "";
          
            var test = actionContext.ActionArguments;
            foreach (var b in test)
            {
                var post = actionContext.ActionArguments[b.Key];

                if (null != post)
                {
                    //Type t = post.GetType();
                    //var typeArr = t.GetProperties();
                    //var str = "";
                    //foreach (var a in typeArr.OrderBy(x => x.Name))
                    //{
                    //    var n = a.Name;
                    //    var v = a.GetValue(post, null);
                    //    if (null != v && v.ToString() != "")
                    //    {
                    //        str += @"""" + n + @""":" + @"""" + v + @""",";
                    //    }
                    //}
                    //str = str.TrimEnd(',');
                    //str = @"{" + str + "}";
                    //postStr += str + ",";
                    return post.ToString();
                }
            }
          return  postStr.TrimEnd(',');

       
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception != null) return; //异常不在这里处理
            base.OnActionExecuted(actionExecutedContext);

            var noPackage = actionExecutedContext.ActionContext.ActionDescriptor.GetCustomAttributes<NoPackageResult>();
            if (!noPackage.Any())
            {
                //执行封装
                if (actionExecutedContext.ActionContext.Response != null)
                {
                    WebAPiResults result = new WebAPiResults();

                    result.StatusCode = actionExecutedContext.ActionContext.Response.StatusCode;
                   
                    var a = actionExecutedContext.ActionContext.Response.Content.ReadAsAsync<object>();
                    if (!a.IsFaulted)
                    {
                        // 取得由 API 返回的资料
                        result.Data = a.Result;
                        if (result.Data == null)
                        { result.Code = "99";
                            result.Message = "未找到任何数据！";
                        }
                        else
                        {
                            result.Code = "0";
                        }
                    }
                    

                    result.Success = actionExecutedContext.ActionContext.Response.IsSuccessStatusCode;

                    //结果转为自定义消息格式
                    HttpResponseMessage httpResponseMessage = JsonHelper.toJson(result);

                    // 重新封装回传格式
                    actionExecutedContext.Response = httpResponseMessage;
                }
            }
        }
 



    }
}
