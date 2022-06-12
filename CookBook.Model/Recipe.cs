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
        public string Title { get; set; }
        public string Author { get; set; }
        public int Difficulty { get; set; }
        public int Minutes  { get; set; }

        public virtual List<Step> Steps { get; set; }
        public virtual List<RecipeIngredient> RecipeIngredients { get; set; }
    }
}
