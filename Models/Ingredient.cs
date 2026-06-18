using SQLite;

namespace SmartKitchen.Models;

public class Ingredient
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string IconUrl { get; set; } = string.Empty; // Holds emoji
}
