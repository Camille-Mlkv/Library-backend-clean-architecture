using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.DTOs
{
    public class AuthorDTO:BaseEntityDTO
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public DateTime BirthDay { get; set; }
    }
}
