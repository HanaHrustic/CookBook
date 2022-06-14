using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBook.Model
{
    public class Recipe
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string Title { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string Author { get; set; }
        [Required]
        public int Difficulty { get; set; }
        [Required]
        public int Minutes  { get; set; }

        public virtual List<Step>? Steps { get; set; }
        public virtual List<RecipeIngredient>? RecipeIngredients { get; set; }
    }
}
