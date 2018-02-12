using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoTracker.Models
{
    public class BaseObject
    {
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
