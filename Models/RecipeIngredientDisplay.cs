namespace SmartKitchen.Models;

public class RecipeIngredientDisplay
{
    public string Name { get; set; } = string.Empty;
    public string Quantity { get; set; } = string.Empty;
    public string IconUrl { get; set; } = string.Empty;
    
    public string DisplayText => Name;
}
