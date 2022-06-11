using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBook.Model
{
    public class Step
    {
        [Key]
        public int Id { get; set; }
        public int number { get; set; }
        public string description { get; set; }
    }
}
