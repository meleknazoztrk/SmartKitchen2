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
    private bool isBusy = false;

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

    public async Task LoadDataAsync()
    {
        if (IsBusy) return;
        IsBusy = true;
        
        try
        {
            LoadAuthState();
            _allMatches = await _recipeService.CalculateBestMatchesAsync(AppState.SelectedIngredients);
            ApplyFilters();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading recipes: {ex.Message}");
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                if (Shell.Current != null)
                {
                    await Shell.Current.DisplayAlert("Tarif Listeleme Hatası", ex.ToString(), "Tamam");
                }
            });
        }
        finally
        {
            IsBusy = false;
        }
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
    private async Task GoToAddRecipeAsync()
    {
        bool isLoggedIn = Preferences.Get("IsLoggedIn", false);
        if (isLoggedIn)
        {
            await Shell.Current.GoToAsync("AddRecipePage");
        }
        else
        {
            if (Application.Current?.MainPage != null)
            {
                await Application.Current.MainPage.DisplayAlert("Giriş Gerekli", "Kendi tarifini eklemek için lütfen önce giriş yapmalısın.", "Tamam");
            }
        }
    }

    [RelayCommand]
    private async Task GoToAuthAsync()
    {
        try 
        {
            await Shell.Current.GoToAsync("AuthPage");
        } 
        catch (Exception ex) 
        {
            if (Application.Current?.MainPage != null)
            {
                await Application.Current.MainPage.DisplayAlert("Hata", "Sayfa açılamadı: " + ex.Message, "Tamam");
            }
        }
    }

    [RelayCommand]
    private async Task GoToProfileAsync()
    {
        await Shell.Current.GoToAsync("ProfilePage");
    }
}
