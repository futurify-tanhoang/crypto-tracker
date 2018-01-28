using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoTracker.Exceptions
{
    public class CustomException : Exception
    {
        public string Code { get; set; }

        public CustomException(string code, string message = null) : base(message != null ? message : code)
        {
            this.Code = code;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(new
            {
                Code = this.Code,
                Message = this.Message
            });
        }
    }
}
