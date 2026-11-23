namespace SmartKitchen.Models;

public class RecipeIngredientDisplay
{
    public string Name { get; set; } = string.Empty;
    public string Quantity { get; set; } = string.Empty;
    public string IconUrl { get; set; } = "🥘";
    
    public string DisplayText => string.IsNullOrWhiteSpace(Quantity) || Quantity == "Göz kararı" 
        ? Name 
        : $"{Name} ({Quantity})";
}
