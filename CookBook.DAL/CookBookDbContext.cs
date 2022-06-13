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
        public CookBookDbContext(DbContextOptions<CookBookDbContext> options) : base(options){
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Size>().HasData(new Size() { Id = 1, Name = "teaspoon" });
            modelBuilder.Entity<Size>().HasData(new Size() { Id = 2, Name = "tablespoon" });
            modelBuilder.Entity<Size>().HasData(new Size() { Id = 3, Name = "ounce" });
            modelBuilder.Entity<Size>().HasData(new Size() { Id = 4, Name = "cup" });
            modelBuilder.Entity<Size>().HasData(new Size() { Id = 5, Name = "pint" });
            modelBuilder.Entity<Size>().HasData(new Size() { Id = 6, Name = "gallon" });
            modelBuilder.Entity<Size>().HasData(new Size() { Id = 7, Name = "milliliter" });
            modelBuilder.Entity<Size>().HasData(new Size() { Id = 8, Name = "litre" });
            modelBuilder.Entity<Size>().HasData(new Size() { Id = 9, Name = "deciliter" });

            modelBuilder.Entity<Size>().HasData(new Size() { Id = 10, Name = "pound" });
            modelBuilder.Entity<Size>().HasData(new Size() { Id = 11, Name = "ounce" });
            modelBuilder.Entity<Size>().HasData(new Size() { Id = 12, Name = "mg" });
            modelBuilder.Entity<Size>().HasData(new Size() { Id = 13, Name = "g" });
            modelBuilder.Entity<Size>().HasData(new Size() { Id = 14, Name = "kg" });
        }

        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Step> Steps { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
    }
}
