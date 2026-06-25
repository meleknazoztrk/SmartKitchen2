using SQLite;

namespace SmartKitchen.Models;

public class Ingredient
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string IconUrl { get; set; } = string.Empty; // Holds emoji
    public string UserId { get; set; } = string.Empty; // Identifies the creator
    public bool IsDeleted { get; set; } = false; // Soft delete flag
}
