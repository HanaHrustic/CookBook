namespace CookBook.Web.DTO
{
    public class RecipeDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Difficulty { get; set; }
        public int Minutes { get; set; }

        public List<StepDTO> Steps { get; set; }
        public List<RecipeIngredientDTO> RecipeIngredients { get; set; }
    }
}
