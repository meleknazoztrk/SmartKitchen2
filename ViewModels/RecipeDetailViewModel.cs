using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartKitchen.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace SmartKitchen.ViewModels;

public partial class RecipeDetailViewModel : ObservableObject
{
    [ObservableProperty]
    private Recipe recipe = new();

    [ObservableProperty]
    private ObservableCollection<string> instructions = new();

    [ObservableProperty]
    private ObservableCollection<RecipeIngredientDisplay> ingredientsList = new();

    private readonly SmartKitchen.Data.DatabaseService _databaseService;

    public RecipeDetailViewModel(SmartKitchen.Data.DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async void Initialize()
    {
        if (AppState.SelectedRecipe != null)
        {
            Recipe = AppState.SelectedRecipe;
            try
            {
                var steps = JsonConvert.DeserializeObject<List<string>>(Recipe.Instructions);
                Instructions.Clear();
                if (steps != null)
                {
                    for (int i = 0; i < steps.Count; i++)
                    {
                        Instructions.Add($"{i + 1}. Adım: {steps[i]}");
                    }
                }
            }
            catch
            {
                // Fallback if parsing fails
                Instructions.Clear();
                Instructions.Add($"1. Adım: {Recipe.Instructions}");
            }
            
            await LoadIngredientsAsync();

        }
    }

    private async Task LoadIngredientsAsync()
    {
        var list = await _databaseService.GetRecipeIngredientsDetailedAsync(Recipe.Id);
        IngredientsList.Clear();
        foreach(var item in list)
        {
            IngredientsList.Add(item);
        }
    }

    [RelayCommand]
    private async Task CloseAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

}
