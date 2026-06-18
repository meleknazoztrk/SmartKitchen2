namespace SmartKitchen.Models;

public static class AppState
{
    public static List<Ingredient> SelectedIngredients { get; set; } = new();
    public static Recipe? SelectedRecipe { get; set; }
}
