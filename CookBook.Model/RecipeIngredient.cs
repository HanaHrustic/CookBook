using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBook.Model
{
    public class RecipeIngredient
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Recipe))]
        public int RecipeId { get; set; }

        [ForeignKey(nameof(Ingredient))]
        public int IngredientId { get; set; }
        public Ingredient? Ingredient { get; set; }

        public int Amount { get; set; }

        [ForeignKey(nameof(Size))]
        public int SizeId { get; set; }
        public Size? Size { get; set; }
    }
}
