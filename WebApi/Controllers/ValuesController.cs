using KunchiLibrary.WebAPI;
using KunchiLibrary.WebApiFilters.PreventSpamAttribute;
using KunchiLibrary.WebApiFilters.VaildeModelAttribute;
using System.Web.Http;
using WebApi.Models;

namespace WebApi.Controllers
{

    public class ValuesController : ApiController
    {

        /// <summary>
        /// 测试业务逻辑异常返回给客户端的方式
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/values/5
        
        public IHttpActionResult Get(int id)
        {

            if (id < 0) WebApiExceptionData.ExceptionData( "A00001", "id不能小于0！");
            decimal i = 1 / id;
            return Ok(i);

        }

        /// <summary>
        /// 测试入参错误返回给客户端的方式
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [Route("test/v1/PostTestVaile")]
        [ValidateModel]//模型验证
        [PreventSpam(10,120,false)] //防止重复提交
        // POST api/values
        public IHttpActionResult PostTestVaile(test value)
        {
            var data = new test()
            {
                Id = "123123",
                Num = 123
            };

            return Ok(data);

        }
        /// <summary>
        /// 测试正确返回给客户端的封装后的数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [Route("test/v1/PostTest")]
        // POST api/values
        public IHttpActionResult PostTest(test value)
        {

            var data = new test()
            {
                Id = "123123",
                Num = 123
            };
            return Ok(data);

        }
        /// <summary>
        /// 测试入参正确返回给客户端系统异常
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [Route("test/v1/PostTestError")]
        public IHttpActionResult PostTestError(test value)
        {

            var data = new test();
           // throw new WebApiErrorResults().getError("业务逻辑异常返回错误信息！", "A00001"); //备用使用
            WebApiExceptionData.ExceptionData("A00001", "业务逻辑异常返回错误信息！"); //推荐使用
            return Ok(data);


        }

        /// <summary>
        /// 测试入参正确返回给客户端未找到数据的接口s
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [Route("test/v1/PostTestNull")]
        public IHttpActionResult PostTestNull(test value)
        {

            object data = null;
            return Ok(data);


        }

    }
}
