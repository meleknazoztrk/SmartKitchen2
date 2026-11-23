using Newtonsoft.Json;

namespace SmartKitchen.Models;

public class RecipeIngredient
{
    private string _ingredientName = string.Empty;
    public string IngredientName
    {
        get => _ingredientName ?? string.Empty;
        set => _ingredientName = value ?? string.Empty;
    }

    private string _amount = string.Empty;
    public string Amount
    {
        get => _amount ?? string.Empty;
        set => _amount = value ?? string.Empty;
    }

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
        set => Amount = value ?? string.Empty;
    }

    [JsonIgnore]
    public bool IsOptional { get; set; }
}

