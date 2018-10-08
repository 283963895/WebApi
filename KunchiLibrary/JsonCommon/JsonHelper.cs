using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace KunchiLibrary.JsonCommon
{
   public class JsonHelper
    {
        public static HttpResponseMessage toJson(Object obj)
        {
            String str;
            if(obj is String || obj is Char)//是否是字符
            {
                str = obj.ToString();
            }
            else//否则序列化Json字符
            {

                //JavaScriptSerializer serializer = new JavaScriptSerializer();

                //str = serializer.Serialize(obj);
                var jsonSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };

                str = JsonConvert.SerializeObject(obj,Formatting.Indented,jsonSetting);
            }
            HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(str, Encoding.GetEncoding("UTF-8"), "application/json"), };
            return result;
        }
 
    }
}
