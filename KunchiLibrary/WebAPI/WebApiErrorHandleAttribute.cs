using KunchiLibrary.JsonCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace KunchiLibrary.WebAPI
{/*
    作用： 规范WebAPI返回异常信息的统一性
    作者： Jason
    时间： 2018-09-27
    说明： 该类是为WebAPI提供返回异常信息的统一类
       */
    public class WebApiErrorHandleAttribute : System.Web.Http.Filters.ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            //异常处理
            base.OnException(actionExecutedContext);
            //重新封装
            WebAPiResults result = new WebAPiResults();
            if (actionExecutedContext.Exception.Data.Count > 0)
            {
                foreach (DictionaryEntry de in actionExecutedContext.Exception.Data)
                {
                    result.ErrorMessage = de.Value.ToString();
                    result.Code = de.Key.ToString();
                }
            }
            else
            {
                result.ErrorMessage = actionExecutedContext.Exception.Message;
                result.Code = "未知编号";
            }
            result.Success = false;
            result.StatusCode = System.Net.HttpStatusCode.BadRequest;
            //结果转为自定义消息格式
            HttpResponseMessage httpResponseMessage = JsonHelper.toJson(result);

            // 重新封装回传格式
            actionExecutedContext.Response = httpResponseMessage;

        }

    }
}
