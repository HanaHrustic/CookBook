using CookBook.DAL;
using CookBook.Model;
using CookBook.Web.DTO;
using Microsoft.AspNetCore.Mvc;

namespace CookBook.Web.Controllers
{
    [Route("api/ingredient")]
    [ApiController]
    public class IngredientApiController : Controller
    {
        private CookBookDbContext _dbContext;

        public IngredientApiController(CookBookDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        [HttpGet]
        public List<IngredientDTO> Get()
        {
            return this._dbContext.Ingredients
                .Select(ingredient => ingredientToIngredientDTO(ingredient))
                .ToList();
        }

        [HttpGet]
        [Route("{id}")]
        public IngredientDTO Get(int id)
        {
            return this._dbContext.Ingredients
                .Where(ingredient => ingredient.Id.Equals(id))
                .Select(ingredient => ingredientToIngredientDTO(ingredient))
                .First();
        }

        [Route("pretraga/{q}")]
        public List<IngredientDTO> Get(string q)
        {
            return this._dbContext.Ingredients
                .Where(ingredient => ingredient.Name.Contains(q))
                .Select(ingredient => ingredientToIngredientDTO(ingredient))
                .ToList();
        }

        [HttpPost]
        public async Task<ActionResult<IngredientDTO>> Post(int id, [FromBody] Ingredient model)
        {
            this._dbContext.Ingredients.Add(model);
            this._dbContext.SaveChanges();

            return ingredientToIngredientDTO(this._dbContext.Ingredients
                .Where(ingredient => ingredient.Id.Equals(model.Id))
                .First());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<IngredientDTO>> Put(int id, [FromBody] Ingredient model)
        {
            Ingredient ingredientToChange = this._dbContext.Ingredients
                .Where(ingredient => ingredient.Id.Equals(id))
                .First();

            if (ingredientToChange != null)
            {
                ingredientToChange.Name = model.Name;
            }

            this._dbContext.SaveChanges();

            return ingredientToIngredientDTO(ingredientToChange);
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Ingredient ingredient = this._dbContext.Ingredients
                .Where(ingredient => ingredient.Id.Equals(id))
                .First();

            this._dbContext.Ingredients.Remove(ingredient);
            this._dbContext.SaveChanges();

            return Ok();
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
