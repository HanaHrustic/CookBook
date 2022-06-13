using CookBook.DAL;
using CookBook.Model;
using CookBook.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

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

        public IActionResult Details(int? id = null)
        {
            var recipe = this._dbContext.Recipes
                .Include(recipe => recipe.RecipeIngredients)
                .ThenInclude(recipeIngredient => recipeIngredient.Ingredient)
                .Include(recipe => recipe.RecipeIngredients)
                .ThenInclude(recipeIngredient => recipeIngredient.Size)
                .Include(recipe => recipe.Steps)
                .Where(p => p.Id == id)
                .FirstOrDefault();

            return View(recipe);
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
            ViewBag.Ingredients = this._dbContext.Ingredients.ToList();
            ViewBag.Sizes = this._dbContext.Sizes.ToList();

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
            var model = this._dbContext.Recipes
                .Include(recipe => recipe.RecipeIngredients)
                .ThenInclude(recipeIngredient => recipeIngredient.Ingredient)
                .Include(recipe => recipe.RecipeIngredients)
                .ThenInclude(recipeIngredient => recipeIngredient.Size)
                .Include(recipe => recipe.Steps)
                .FirstOrDefault(c => c.Id == id);

            ViewBag.Ingredients = this._dbContext.Ingredients.ToList();
            ViewBag.Sizes = this._dbContext.Sizes.ToList();

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
            recipe.Steps = model.Steps;
            recipe.RecipeIngredients = model.RecipeIngredients;

            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int Id)
        {
            Recipe recipe = this._dbContext.Recipes
                .Where(recipe => recipe.Id.Equals(Id))
                .First();

            this._dbContext.Recipes.Remove(recipe);
            this._dbContext.SaveChanges();

            return RedirectToAction("Search");
        }
    }
}