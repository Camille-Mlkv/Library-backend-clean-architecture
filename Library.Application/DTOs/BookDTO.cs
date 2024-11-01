using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.DTOs
{
    public class BookDTO:BaseEntityDTO
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public int AuthorId { get; set; }
        public string ClientId { get; set; }
        public DateTime? TakenTime { get; set; }
        public DateTime? ReturnBy { get; set; }
    }
}
