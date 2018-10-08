using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
/*
    作用： 规范WebAPI返回信息的统一性
    作者： Jason
    时间： 2018-09-27
    说明： 该类是为WebAPI提供返回值信息的统一类
       */
namespace KunchiLibrary.WebAPI
{

   
    public class WebAPiResults
    {
        private HttpStatusCode _statusCode; //api回发的状态码
        private bool _isSuccess; //是否成功标志
        private string _code;  //返回的代码号
        
        private string _errorMessage; //返回的错误信息
        private object _data;//返回的正确数据对象
        private string _Message;//返回业务逻辑信息

        public HttpStatusCode StatusCode
        {
            get
            {
                return _statusCode;
            }

            set
            {
                _statusCode = value;
            }
        }
        public string  Code
        {
            get
            {
                return _code;
            }

            set
            {
                _code = value;
            }
        }

        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }

            set
            {
                _errorMessage = value;
            }
        }

        public object Data
        {
            get
            {
                return _data;
            }

            set
            {
                _data = value;
            }
        }

        public bool Success
        {
            get
            {
                return _isSuccess;
            }

            set
            {
                _isSuccess = value;
            }
        }

        public string Message
        {
            get
            {
                return _Message;
            }

            set
            {
                _Message = value;
            }
        }
    }
}
