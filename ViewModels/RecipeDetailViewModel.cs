using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartKitchen.Models;
using System.Collections.ObjectModel;

namespace SmartKitchen.ViewModels;

public partial class RecipeDetailViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasPortionSize))]
    [NotifyPropertyChangedFor(nameof(PortionSizeDisplay))]
    [NotifyPropertyChangedFor(nameof(HasCalories))]
    [NotifyPropertyChangedFor(nameof(CalorieDisplay))]
    private Recipe recipe = new();

    public bool HasPortionSize => !string.IsNullOrWhiteSpace(Recipe?.PortionSize);
    public string PortionSizeDisplay => !string.IsNullOrWhiteSpace(Recipe?.PortionSize) ? $"🍽️ {Recipe.PortionSize}" : string.Empty;

    public bool HasCalories => !string.IsNullOrWhiteSpace(Recipe?.CaloriesPerPortion) || (Recipe != null && Recipe.Calories > 0);
    public string CalorieDisplay
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(Recipe?.CaloriesPerPortion))
                return $"🔥 {Recipe.CaloriesPerPortion}";
            if (Recipe != null && Recipe.Calories > 0)
                return $"🔥 {Recipe.Calories} kcal";
            return string.Empty;
        }
    }

    [ObservableProperty]
    private ObservableCollection<RecipeIngredientDisplay> ingredientsList = new();

    private readonly SmartKitchen.Services.FirebaseService _firebaseService;

    public RecipeDetailViewModel(SmartKitchen.Services.FirebaseService firebaseService)
    {
        _firebaseService = firebaseService;
    }

    public async void Initialize()
    {
        if (AppState.SelectedRecipe != null)
        {
            Recipe = AppState.SelectedRecipe;
            await LoadIngredientsAsync();
        }
    }

    private async Task LoadIngredientsAsync()
    {
        IngredientsList.Clear();
        if (Recipe?.Ingredients == null || Recipe.Ingredients.Count == 0) return;

        List<Ingredient>? allIngredients = null;
        try
        {
            allIngredients = await _firebaseService.GetIngredientsAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Icon lookup info: {ex.Message}");
        }

        foreach (var ri in Recipe.Ingredients)
        {
            if (ri == null || string.IsNullOrWhiteSpace(ri.IngredientName)) continue;
            var match = allIngredients?.FirstOrDefault(i => i != null && i.Name.Equals(ri.IngredientName, StringComparison.OrdinalIgnoreCase));
            IngredientsList.Add(new RecipeIngredientDisplay
            {
                Name = ri.IngredientName,
                Quantity = ri.Amount ?? string.Empty,
                IconUrl = match?.IconUrl ?? "🥘"
            });
        }
    }

    [RelayCommand]
    private async Task OpenYouTubeAsync()
    {
        if (string.IsNullOrWhiteSpace(Recipe?.Title)) return;
        try
        {
            var encodedTitle = Uri.EscapeDataString(Recipe.Title.Trim()).Replace("%20", "+");
            var url = $"https://www.youtube.com/results?search_query={encodedTitle}+tarifi";
            await Launcher.Default.OpenAsync(new Uri(url));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"YouTube açma hatası: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task OpenNefisYemekTarifleriAsync()
    {
        if (string.IsNullOrWhiteSpace(Recipe?.Title)) return;
        try
        {
            var encodedTitle = Uri.EscapeDataString(Recipe.Title.Trim()).Replace("%20", "+");
            var url = $"https://www.nefisyemektarifleri.com/?s={encodedTitle}";
            await Launcher.Default.OpenAsync(new Uri(url));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Nefis Yemek Tarifleri açma hatası: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task CloseAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
