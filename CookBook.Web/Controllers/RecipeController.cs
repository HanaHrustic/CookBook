using CookBook.DAL;
using CookBook.Model;
using CookBook.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CookBook.Web.Controllers
{
    public class RecipeController : Controller
    {
        private CookBookDbContext _dbContext;

        public RecipeController(CookBookDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var recipeQuery = this._dbContext.Recipes
                .Include(recipe => recipe.RecipeIngredients)
                .ThenInclude(recipeIngredient => recipeIngredient.Ingredient)
                .Include(recipe => recipe.RecipeIngredients)
                .ThenInclude(recipeIngredient => recipeIngredient.Size)
                .Include(recipe => recipe.Steps)
                .AsQueryable();

            var model = recipeQuery.ToList();
            return View("Search", model);
        }
        public IActionResult Explore()
        {
            return View();
        }

        public async Task<IActionResult> Search(RecipeFilterModel filter)
        {
            filter ??= new RecipeFilterModel();

            var recipeQuery = this._dbContext.Recipes
                .Include(recipe => recipe.RecipeIngredients)
                .ThenInclude(recipeIngredient => recipeIngredient.Ingredient)
                .Include(recipe => recipe.RecipeIngredients)
                .ThenInclude(recipeIngredient => recipeIngredient.Size)
                .Include(recipe => recipe.Steps)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Title))
                recipeQuery = recipeQuery.Where(recipe => recipe.Title.ToLower().Contains(filter.Title.ToLower()));

            if (!string.IsNullOrWhiteSpace(filter.Author))
                recipeQuery = recipeQuery.Where(recipe => recipe.Author.ToLower().Contains(filter.Author.ToLower()));

            if (filter.Difficulty != 0)
                recipeQuery = recipeQuery.Where(recipe => recipe.Difficulty.Equals(filter.Difficulty));

            if (filter.Minutes != 0)
                recipeQuery = recipeQuery.Where(recipe => recipe.Minutes.Equals(filter.Minutes));

            var model = recipeQuery.ToList();

            return PartialView("_IndexTable", model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Recipe model)
        {
            this._dbContext.Recipes.Add(model);
            this._dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [ActionName(nameof(Edit))]
        public IActionResult Edit(int id)
        {
            var model = this._dbContext.Recipes.FirstOrDefault(c => c.Id == id);
            return View(model);
        }

        [HttpPost]
        [ActionName(nameof(Edit))]
        public async Task<IActionResult> EditPost(Recipe model)
        {
            var recipe = _dbContext.Recipes
                .Include(recipe => recipe.RecipeIngredients)
                .ThenInclude(recipeIngredient => recipeIngredient.Ingredient)
                .Include(recipe => recipe.RecipeIngredients)
                .ThenInclude(recipeIngredient => recipeIngredient.Size)
                .Include(recipe => recipe.Steps)
                .Where(recipe => recipe.Id == model.Id).FirstOrDefault();

            recipe.Title = model.Title;
            recipe.Author = model.Author;
            recipe.Difficulty = model.Difficulty;
            recipe.Minutes = model.Minutes;

            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}