using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Entities
{
    public class TokenData
    {
        public string AccessToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}
