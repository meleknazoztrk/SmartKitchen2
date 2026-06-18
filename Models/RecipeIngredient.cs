using SQLite;

namespace SmartKitchen.Models;

public class RecipeIngredient
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    
    [Indexed]
    public int RecipeId { get; set; }
    
    [Indexed]
    public int IngredientId { get; set; }
    
    public string Quantity { get; set; } = string.Empty;
    public bool IsOptional { get; set; }
}
