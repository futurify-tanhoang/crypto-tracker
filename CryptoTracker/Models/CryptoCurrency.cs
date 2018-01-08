using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoTracker.Models
{
    public class CryptoCurrency: BaseObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
