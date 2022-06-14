using CookBook.DAL;
using CookBook.Model;
using CookBook.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CookBook.Web.Controllers
{
    public class IngredientController : Controller
    {
        private CookBookDbContext _dbContext;
        private UserManager<AppUser> _userManager;

        public IngredientController(CookBookDbContext dbContext, UserManager<AppUser> userManager)
        {
            this._dbContext = dbContext;
            this._userManager = userManager;
        }
        public string UserId
        {
            get
            {
                return this._userManager.GetUserId(base.User);
            }
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var recipeQuery = this._dbContext.Ingredients.AsQueryable();

            var model = recipeQuery.ToList();
            return View("Search", model);
        }

        [AllowAnonymous]
        public IActionResult Details(int? id = null)
        {
            var ingredient = this._dbContext.Ingredients
                .Where(p => p.Id == id)
                .FirstOrDefault();

            return View(ingredient);
        }

        [AllowAnonymous]
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

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(Ingredient model)
        {
            if (ModelState.IsValid)
            {
                this._dbContext.Ingredients.Add(model);
                this._dbContext.SaveChanges();            
                return RedirectToAction(nameof(Index));

            }
            var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();
            return View();
        }

        [Authorize(Roles = "Admin")]
        [ActionName(nameof(Edit))]
        public IActionResult Edit(int id)
        {
            var model = this._dbContext.Ingredients.FirstOrDefault(c => c.Id == id);
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ActionName(nameof(Edit))]
        public async Task<IActionResult> EditPost(Ingredient model)
        {
            var ingredient = _dbContext.Ingredients.Where(ingredient => ingredient.Id == model.Id).FirstOrDefault();

            ingredient.Name = model.Name;

            var ok = await this.TryUpdateModelAsync(ingredient);
            if (ok)
            {
                _dbContext.SaveChanges();
            }
                
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
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