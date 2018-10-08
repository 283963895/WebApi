using System;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Runtime.Caching;
using System.Net.Http.Headers;
using System.Threading;
using System.Net.Http;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using KunchiLibrary.JsonCommon;
using KunchiLibrary.WebAPI;
using System.Net;

namespace KunchiLibrary.WebApiFilters.PreventSpamAttribute
{
    public class PreventSpamAttribute : ActionFilterAttribute 
    {
        // 缓存时间 /秒        
        private int _timespan;
        // 客户端缓存时间 /秒        
        private int _clientTimeSpan;
        // 是否为匿名用户缓存        
        private bool _anonymousOnly;
        // 缓存索引键        
        private string _cachekey;
        // 缓存仓库       
        private static readonly ObjectCache WebApiCache = MemoryCache.Default;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="timespan"></param>
        /// <param name="clientTimeSpan"></param>
        /// <param name="anonymousOnly"></param>
        public PreventSpamAttribute(int timespan, int clientTimeSpan, bool anonymousOnly)
        {
            _timespan = timespan;
            _clientTimeSpan = clientTimeSpan;
            _anonymousOnly = anonymousOnly;
        }
        //是否缓存
        private bool _isCacheable(HttpActionContext ac)
        {
            if (_timespan > 0 && _clientTimeSpan > 0)
            {
                if (_anonymousOnly)
                    if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
                        return false;
                if (ac.Request.Method == HttpMethod.Post) return true;
            }
            else
            {
                throw new InvalidOperationException("Wrong Arguments");
            }
            return false;
        }

        private CacheControlHeaderValue SetClientCache()
        {
            var cachecontrol = new CacheControlHeaderValue
            {
                MaxAge = TimeSpan.FromSeconds(_clientTimeSpan),
                MustRevalidate = true
            };
            return cachecontrol;
        }


        /// <summary>
        /// Action调用前执行的方法
        /// </summary>
        /// <param name="ac"></param>
        public override void OnActionExecuting(HttpActionContext ac)
        {
            //Grab the IP Address from the originating Request 
            var originationInfo = ac.Request.RequestUri.Host;
            //Append the User Agent
            //originationInfo += ac.Request.RequestUri.h;
            //目标URL信息
            var targetInfo = ac.Request.RequestUri + ac.ActionDescriptor.ActionName;
 
            if (ac != null)
            {
                if (_isCacheable(ac))
                {
                    HttpContextBase context = (HttpContextBase)ac.Request.Properties["MS_HttpContext"];//获取传统context
                    HttpRequestBase request = context.Request;//定义传统request对象       
                    _cachekey = string.Join("", MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(originationInfo + targetInfo)).Select(s => s.ToString("x2"))); ;
                    if (WebApiCache.Contains(_cachekey))
                    {
                        var val = (string)WebApiCache.Get(_cachekey);
                        if (val != null)
                        {
                            ac.Response = ac.Request.CreateResponse();
                            ac.Response.Content = new StringContent(val);
                            var contenttype = (MediaTypeHeaderValue)WebApiCache.Get(_cachekey + ":response-ct") ??
                                              new MediaTypeHeaderValue(_cachekey.Split(':')[1]);
                            ac.Response.Content.Headers.ContentType = contenttype;
                            ac.Response.Headers.CacheControl = SetClientCache();
                            return;
                          
                        }
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("ac");
            }
        }


        /// <summary>
        /// Action调用后执行方法
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (_cachekey != null && !WebApiCache.Contains(_cachekey))
            {
                var body = actionExecutedContext.Response.Content.ReadAsStringAsync().Result;
                WebApiCache.Add(_cachekey, body, DateTime.Now.AddSeconds(_timespan));
                WebApiCache.Add(_cachekey + ":response-ct", actionExecutedContext.Response.Content.Headers.ContentType, DateTime.Now.AddSeconds(_timespan));
            }
            if (_isCacheable(actionExecutedContext.ActionContext))
                actionExecutedContext.ActionContext.Response.Headers.CacheControl = SetClientCache();
        }
    }
}
