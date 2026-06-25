using Newtonsoft.Json;

namespace SmartKitchen.Models;

public class Recipe
{
    [JsonIgnore]
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public string PortionSize { get; set; } = string.Empty;
    public string CaloriesPerPortion { get; set; } = string.Empty;
    public List<string> Steps { get; set; } = new();
    public List<RecipeIngredient> Ingredients { get; set; } = new();

    // --- Compatibility Properties ---
    private string _description = string.Empty;
    [JsonIgnore]
    public string Description
    {
        get => string.IsNullOrEmpty(_description) ? $"Porsiyon: {PortionSize}" : _description;
        set => _description = value;
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
        set => _instructions = value;
    }

    [JsonIgnore]
    public bool IsPublic { get; set; } = true;

    [JsonIgnore]
    public string UserId { get; set; } = string.Empty;
}

