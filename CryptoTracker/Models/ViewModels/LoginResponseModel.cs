using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoTracker.Models.ViewModels
{
    public class LoginResponseModel
    {
        public string AccessToken { get; set; }
        public DateTime Expires { get; set; }
    }
}
