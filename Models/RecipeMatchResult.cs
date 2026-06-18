namespace SmartKitchen.Models;

public class RecipeMatchResult
{
    public Recipe Recipe { get; set; } = new();
    public double MatchPercentage { get; set; }
    public List<Ingredient> MissingIngredients { get; set; } = new();
}
