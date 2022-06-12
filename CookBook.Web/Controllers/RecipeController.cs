using CookBook.DAL;
using Microsoft.AspNetCore.Mvc;

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
            return View();
        }
        public IActionResult Explore()
        {
            return View();
        }

        public IActionResult Search()
        {
            return View();
        }
    }
}
