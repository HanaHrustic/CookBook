using CookBook.DAL;
using CookBook.Model;
using CookBook.Web.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CookBook.Web.Controllers
{
    [Route("api/recipe")]
    [ApiController]
    public class RecipeApiController : Controller
    {
        private CookBookDbContext _dbContext;

        public RecipeApiController(CookBookDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        [HttpGet]
        public List<RecipeDTO> Get()
        {
            return this._dbContext.Recipes
                .Include(recipe => recipe.RecipeIngredients)
                .ThenInclude(recipeIngredient => recipeIngredient.Ingredient)
                .Include(recipe => recipe.RecipeIngredients)
                .ThenInclude(recipeIngredient => recipeIngredient.Size)
                .Include(recipe => recipe.Steps)
                .Select(recipe => recipeToRecipeDTO(recipe))
                .ToList();
        }

        [HttpGet]
        [Route("{id}")]
        public RecipeDTO Get(int id)
        {
            return this._dbContext.Recipes
                .Where(recipe => recipe.Id.Equals(id))
                .Include(recipe => recipe.RecipeIngredients)
                .ThenInclude(recipeIngredient => recipeIngredient.Ingredient)
                .Include(recipe => recipe.RecipeIngredients)
                .ThenInclude(recipeIngredient => recipeIngredient.Size)
                .Include(recipe => recipe.Steps)
                .Select(recipe => recipeToRecipeDTO(recipe))
                .First();
        }

        [Route("pretraga/{q}")]
        public List<RecipeDTO> Get(string q)
        {
            return this._dbContext.Recipes
                .Where(recipe => recipe.Title.Contains(q))
                .Include(recipe => recipe.RecipeIngredients)
                .ThenInclude(recipeIngredient => recipeIngredient.Ingredient)
                .Include(recipe => recipe.RecipeIngredients)
                .ThenInclude(recipeIngredient => recipeIngredient.Size)
                .Include(recipe => recipe.Steps)
                .Select(recipe => recipeToRecipeDTO(recipe))
                .ToList();
        }

        [HttpPost]
        public async Task<ActionResult<RecipeDTO>> Post(int id, [FromBody] Recipe model)
        {
            this._dbContext.Recipes.Add(model);
            this._dbContext.SaveChanges();

            return recipeToRecipeDTO(this._dbContext.Recipes
                .Where(recipe => recipe.Id.Equals(model.Id))
                .Include(recipe => recipe.RecipeIngredients)
                .ThenInclude(recipeIngredient => recipeIngredient.Ingredient)
                .Include(recipe => recipe.RecipeIngredients)
                .ThenInclude(recipeIngredient => recipeIngredient.Size)
                .Include(recipe => recipe.Steps)
                .First());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<RecipeDTO>> Put(int id, [FromBody] Recipe model)
        {
            Recipe recipeToChange = this._dbContext.Recipes
                .Where(recipe => recipe.Id.Equals(id))
                .Include(recipe => recipe.RecipeIngredients)
                .ThenInclude(recipeIngredient => recipeIngredient.Ingredient)
                .Include(recipe => recipe.RecipeIngredients)
                .ThenInclude(recipeIngredient => recipeIngredient.Size)
                .Include(recipe => recipe.Steps)
                .First();

            if (recipeToChange != null)
            {
                recipeToChange.Title = model.Title;
                recipeToChange.Author = model.Author;
                recipeToChange.Difficulty = model.Difficulty;
                recipeToChange.Minutes = model.Minutes;
                recipeToChange.Steps = model.Steps;
                recipeToChange.RecipeIngredients = model.RecipeIngredients;
            }

            this._dbContext.SaveChanges();

            return recipeToRecipeDTO(recipeToChange);
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Recipe recipe = this._dbContext.Recipes
                .Where(recipe => recipe.Id.Equals(id))
                .First();

            this._dbContext.Recipes.Remove(recipe);
            this._dbContext.SaveChanges();

            return Ok();
        }

        private static RecipeDTO recipeToRecipeDTO(Recipe recipe)
        {
            return new RecipeDTO()
            {
                Id = recipe.Id,
                Title = recipe.Title,
                Author = recipe.Author,
                Difficulty = recipe.Difficulty,
                Minutes = recipe.Minutes,
                Steps = recipe.Steps.Select(step => stepToStepDTO(step)).ToList(),
                RecipeIngredients = recipe.RecipeIngredients.Select(recipeIngredient => recipeIngredientToRecipeIngredientDTO(recipeIngredient)).ToList()
            };
        }

        private static StepDTO stepToStepDTO(Step step)
        {
            return new StepDTO()
            {
                Id = step.Id,
                Number = step.Number,
                Description = step.Description
            };
        }

        private static RecipeIngredientDTO recipeIngredientToRecipeIngredientDTO(RecipeIngredient recipeIngredient)
        {
            return new RecipeIngredientDTO()
            {
                Id = recipeIngredient.Id,
                Ingredient = ingredientToIngredientDTO(recipeIngredient.Ingredient),
                Amount = recipeIngredient.Amount,
                Size = sizeToSizeDTO(recipeIngredient.Size)
            };
        }

        private static SizeDTO sizeToSizeDTO(Size size)
        {
            return new SizeDTO()
            {
                Id = size.Id,
                Name = size.Name
            };
        }

        private static IngredientDTO ingredientToIngredientDTO(Ingredient ingredient)
        {
            return new IngredientDTO()
            {
                Id = ingredient.Id,
                Name = ingredient.Name
            };
        }
    }
}