using CookBook.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBook.DAL
{
    public class CookBookDbContext : DbContext
    {
        protected CookBookDbContext() { }
        public CookBookDbContext(DbContextOptions<CookBookDbContext> options) : base(options){ }

        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Step> Steps { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
    }
}
