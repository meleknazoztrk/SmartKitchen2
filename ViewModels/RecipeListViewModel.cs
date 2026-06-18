using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartKitchen.Models;
using SmartKitchen.Services;
using System.Collections.ObjectModel;

namespace SmartKitchen.ViewModels;

public partial class RecipeListViewModel : ObservableObject
{
    private readonly RecipeService _recipeService;
    private List<RecipeMatchResult> _allMatches = new();

    [ObservableProperty]
    private ObservableCollection<RecipeMatchResult> recipes = new();

    [ObservableProperty]
    private string searchText = string.Empty;

    [ObservableProperty]
    private string selectedTimeFilter = "Tümü";

    [ObservableProperty]
    private string selectedCalorieFilter = "Tümü";

    public List<string> TimeFilters { get; } = new() { "Tümü", "15 dk altı", "30 dk altı", "30 dk üstü" };
    public List<string> CalorieFilters { get; } = new() { "Tümü", "300 kcal altı", "500 kcal altı", "500 kcal üstü" };

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotLoggedIn))]
    private bool isLoggedIn = false;

    [ObservableProperty]
    private string currentUsername = string.Empty;

    public bool IsNotLoggedIn => !IsLoggedIn;

    public RecipeListViewModel(RecipeService recipeService)
    {
        _recipeService = recipeService;
    }

    public async Task InitializeAsync()
    {
        LoadAuthState();
        _allMatches = await _recipeService.CalculateBestMatchesAsync(AppState.SelectedIngredients);
        ApplyFilters();
    }

    private void LoadAuthState()
    {
        IsLoggedIn = Preferences.Get("IsLoggedIn", false);
        CurrentUsername = Preferences.Get("UserFullName",
                          Preferences.Get("UserEmail", string.Empty));
    }

    partial void OnSearchTextChanged(string value) => ApplyFilters();
    partial void OnSelectedTimeFilterChanged(string value) => ApplyFilters();
    partial void OnSelectedCalorieFilterChanged(string value) => ApplyFilters();

    private void ApplyFilters()
    {
        var filtered = _allMatches.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            filtered = filtered.Where(r => r.Recipe.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
        }

        if (SelectedTimeFilter == "15 dk altı") filtered = filtered.Where(r => r.Recipe.PrepTimeMinutes < 15);
        else if (SelectedTimeFilter == "30 dk altı") filtered = filtered.Where(r => r.Recipe.PrepTimeMinutes < 30);
        else if (SelectedTimeFilter == "30 dk üstü") filtered = filtered.Where(r => r.Recipe.PrepTimeMinutes >= 30);

        if (SelectedCalorieFilter == "300 kcal altı") filtered = filtered.Where(r => r.Recipe.Calories < 300);
        else if (SelectedCalorieFilter == "500 kcal altı") filtered = filtered.Where(r => r.Recipe.Calories < 500);
        else if (SelectedCalorieFilter == "500 kcal üstü") filtered = filtered.Where(r => r.Recipe.Calories >= 500);

        Recipes.Clear();
        foreach (var item in filtered)
        {
            Recipes.Add(item);
        }
    }

    [RelayCommand]
    private async Task RecipeSelectedAsync(RecipeMatchResult match)
    {
        if (match == null) return;
        AppState.SelectedRecipe = match.Recipe;
        await Shell.Current.GoToAsync("RecipeDetailPage");
    }

    [RelayCommand]
    private async Task AddOwnRecipeAsync()
    {
        if (Application.Current?.MainPage != null)
        {
            await Application.Current.MainPage.DisplayAlert("Bilgi", "Kendi tarifini ekleme özelliği yakında gelecek!", "Tamam");
        }
    }

    [RelayCommand]
    private async Task GoToAuthAsync()
    {
        await Shell.Current.GoToAsync("AuthPage");
    }

    [RelayCommand]
    private async Task GoToProfileAsync()
    {
        await Shell.Current.GoToAsync("ProfilePage");
    }
}
