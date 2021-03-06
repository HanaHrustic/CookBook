using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBook.Model
{
    public class Step
    {
        [Key]
        public int Id { get; set; }
        public int Number { get; set; }
        public string Description { get; set; }

        [ForeignKey(nameof(Recipe))]
        public int RecipeId { get; set; }
    }
}
