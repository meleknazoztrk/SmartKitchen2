using Newtonsoft.Json;

namespace SmartKitchen.Models;

public class RecipeIngredient
{
    public string IngredientName { get; set; } = string.Empty;
    public string Amount { get; set; } = string.Empty;

    // --- Compatibility Properties ---
    [JsonIgnore]
    public int Id { get; set; }

    [JsonIgnore]
    public int RecipeId { get; set; }

    [JsonIgnore]
    public int IngredientId { get; set; }

    [JsonIgnore]
    public string Quantity
    {
        get => Amount;
        set => Amount = value;
    }

    [JsonIgnore]
    public bool IsOptional { get; set; }
}

