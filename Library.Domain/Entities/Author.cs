using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Entities
{
    public class Author:BaseEntity
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public DateTime BirthDay { get; set; }
    }
}
