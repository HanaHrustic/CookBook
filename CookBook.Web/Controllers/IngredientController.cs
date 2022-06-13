using CookBook.DAL;
using CookBook.Model;
using CookBook.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace CookBook.Web.Controllers
{
    public class IngredientController : Controller
    {
        private CookBookDbContext _dbContext;

        public IngredientController(CookBookDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var recipeQuery = this._dbContext.Ingredients.AsQueryable();

            var model = recipeQuery.ToList();
            return View("Search", model);
        }

        public IActionResult Details(int? id = null)
        {
            var ingredient = this._dbContext.Ingredients
                .Where(p => p.Id == id)
                .FirstOrDefault();

            return View(ingredient);
        }

        public async Task<IActionResult> Search(IngredientFilterModel filter)
        {
            filter ??= new IngredientFilterModel();

            var ingredientQuery = this._dbContext.Ingredients
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Name))
                ingredientQuery = ingredientQuery.Where(Ingredient => Ingredient.Name.ToLower().Contains(filter.Name.ToLower()));

            var model = ingredientQuery.ToList();

            return PartialView("_IndexTable", model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Ingredient model)
        {
            this._dbContext.Ingredients.Add(model);
            this._dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [ActionName(nameof(Edit))]
        public IActionResult Edit(int id)
        {
            var model = this._dbContext.Ingredients.FirstOrDefault(c => c.Id == id);
            return View(model);
        }

        [HttpPost]
        [ActionName(nameof(Edit))]
        public async Task<IActionResult> EditPost(Ingredient model)
        {
            var ingredient = _dbContext.Ingredients.Where(ingredient => ingredient.Id == model.Id).FirstOrDefault();

            ingredient.Name = model.Name;

            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int Id)
        {
            Ingredient ingredient = this._dbContext.Ingredients
                .Where(ingredient => ingredient.Id.Equals(Id))
                .First();

            this._dbContext.Ingredients.Remove(ingredient);
            this._dbContext.SaveChanges();

            return RedirectToAction("Search");
        }
    }
}