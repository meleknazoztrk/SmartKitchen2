using Newtonsoft.Json;

namespace SmartKitchen.Models;

public class Recipe
{
    [JsonIgnore]
    public int Id { get; set; }

    private string _title = string.Empty;
    public string Title
    {
        get => _title ?? string.Empty;
        set => _title = value ?? string.Empty;
    }

    private string _portionSize = string.Empty;
    public string PortionSize
    {
        get => _portionSize ?? string.Empty;
        set => _portionSize = value ?? string.Empty;
    }

    private string _caloriesPerPortion = string.Empty;
    public string CaloriesPerPortion
    {
        get => _caloriesPerPortion ?? string.Empty;
        set => _caloriesPerPortion = value ?? string.Empty;
    }

    private List<string> _steps = new();
    public List<string> Steps
    {
        get => _steps ??= new();
        set => _steps = value ?? new();
    }

    private List<RecipeIngredient> _ingredients = new();
    public List<RecipeIngredient> Ingredients
    {
        get => _ingredients ??= new();
        set => _ingredients = value ?? new();
    }

    // --- Compatibility Properties ---
    private string _description = string.Empty;
    [JsonIgnore]
    public string Description
    {
        get => string.IsNullOrEmpty(_description) ? (!string.IsNullOrEmpty(PortionSize) ? $"Porsiyon: {PortionSize}" : string.Empty) : _description;
        set => _description = value ?? string.Empty;
    }

    private int _calories = -1;
    [JsonIgnore]
    public int Calories
    {
        get
        {
            if (_calories >= 0) return _calories;
            if (string.IsNullOrEmpty(CaloriesPerPortion)) return 0;
            var match = System.Text.RegularExpressions.Regex.Match(CaloriesPerPortion, @"\d+");
            return match.Success && int.TryParse(match.Value, out int val) ? val : 0;
        }
        set => _calories = value;
    }

    [JsonIgnore]
    public int PrepTimeMinutes { get; set; } = 20;

    [JsonIgnore]
    public int ProteinGrams { get; set; }

    [JsonIgnore]
    public bool IsHighProtein { get; set; }

    [JsonIgnore]
    public string ImageUrl { get; set; } = string.Empty;

    private string _instructions = string.Empty;
    [JsonIgnore]
    public string Instructions
    {
        get => string.IsNullOrEmpty(_instructions) ? JsonConvert.SerializeObject(Steps) : _instructions;
        set => _instructions = value ?? string.Empty;
    }

    [JsonIgnore]
    public bool IsPublic { get; set; } = true;

    [JsonIgnore]
    public string UserId { get; set; } = string.Empty;
}

