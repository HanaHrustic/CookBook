namespace CookBook.Web.DTO
{
    public class RecipeIngredientDTO
    {
        public int Id { get; set; }
        public IngredientDTO Ingredient { get; set; }
        public int Amount { get; set; }
        public SizeDTO Size { get; set; }
    }
}
