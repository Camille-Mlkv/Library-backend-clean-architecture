﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Entities
{
    public class Author:BaseEntity
    {
        public Author()
        {
            this.Books=new HashSet<Book>();
        }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public DateTime BirthDay { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}
