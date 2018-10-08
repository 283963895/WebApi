using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KunchiLibrary.WebAPI
{
   public class WebApiErrorResults
    {
        public Exception getError(string eMsg,string Code)
        {
            Exception ex = new Exception();
            ex.Data[Code] = eMsg;
            return ex;
        }
    }
}
