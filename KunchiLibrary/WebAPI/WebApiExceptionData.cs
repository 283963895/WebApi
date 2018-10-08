using System;
using System.Collections.Generic;
using System.Linq;
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
    public static class WebApiExceptionData
    {
        /// <summary>
        /// 返回系统定义的错误信息 方式为键值对的形式 如 "A0001"表示"未查询到任何信息"
        /// </summary>
        /// <param name="key">A0001</param>
        /// <param name="displayDetails">未查询到任何信息</param>
        public static void ExceptionData(string key, string displayDetails)
        {
            Exception e = new Exception("系统自定义错误编号以及错误信息");
            e.Data[key] = displayDetails;
            throw e;
        }
    }
}
