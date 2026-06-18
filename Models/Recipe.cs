using SQLite;

namespace SmartKitchen.Models;

public class Recipe
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int PrepTimeMinutes { get; set; }
    public int Calories { get; set; }
    public int ProteinGrams { get; set; }
    public bool IsHighProtein { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string Instructions { get; set; } = string.Empty; // JSON serialized list of strings
    public bool IsPublic { get; set; } = true;
    public string UserId { get; set; } = string.Empty; // CurrentUsername from Preferences
}
