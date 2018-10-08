using KunchiLibrary.WebAPI;
using KunchiLibrary.WebApiFilters.VaildeModelAttribute;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiTest.Models;

namespace WebApiTest.Controllers
{
    
    public class ValuesController : ApiController
    {
        
        //测试业务逻辑异常返回给客户端的方式
        // GET api/values/5
        public string Get(int id)
        {
             
                if (id < 0) WebApiExceptionData.ExceptionData("A000001","不能为0");
                double i = 1 / id;
                return "value";
          
        }
     
        /// <summary>
        /// 测试入参错误返回给客户端的方式
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [Route("test/v1/PostTestVaile")]
        [ValidateModel]
        // POST api/values
        public IHttpActionResult PostTestVaile(test value)
        {
            //if (!ModelState.IsValid)
            //{
               
            //    WebApiExceptionData.ExceptionData("入参错误", ModelState.Values.Select(e => e.Errors).FirstOrDefault().First().ErrorMessage);
            //}
            var data = new test()
            {
                Id = "123123",
                Num = 123
            };
         
            return Ok(data);

        }
        //测试正确返回给客户端的封装后的数据
        [Route("test/v1/PostTest")]
        // POST api/values
        public IHttpActionResult PostTest([FromBody]JObject value)
        {

            var data = new test()
            {
                Id = "123123",
                Num = 123
            };
            return Ok(data);

        }
        //测试入参正确返回给客户端系统异常
        [Route("test/v1/PostTestError")]
        public IHttpActionResult PostTestError([FromBody]JObject value)
        {

            var data = new test();
            throw new WebApiErrorResults().getError( "业务逻辑异常返回错误信息！","A00001");
            return Ok(data);
           

        }

        //测试入参正确返回给客户端未找到数据的接口
        [Route("test/v1/PostTestNull")]
        public IHttpActionResult PostTestNull([FromBody]JObject value)
        {

            object data = null;
            return Ok(data);
          

        }

    }
}
