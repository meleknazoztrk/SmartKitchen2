using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartKitchen.Data;
using SmartKitchen.Models;
using System.Collections.ObjectModel;

namespace SmartKitchen.ViewModels;

public partial class FridgeViewModel : ObservableObject
{
    private readonly DatabaseService _databaseService;
    private List<Ingredient> _allIngredientsDb = new();

    [ObservableProperty]
    private ObservableCollection<Ingredient> availableIngredients = new();

    [ObservableProperty]
    private ObservableCollection<Ingredient> selectedIngredients = new();

    [ObservableProperty]
    private string searchText = string.Empty;

    public FridgeViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task InitializeAsync()
    {
        _allIngredientsDb = await _databaseService.GetIngredientsAsync();
        FilterIngredients();
    }

    partial void OnSearchTextChanged(string value)
    {
        FilterIngredients();
    }

    private void FilterIngredients()
    {
        var filtered = _allIngredientsDb.Where(i => 
            !SelectedIngredients.Any(s => s.Id == i.Id) && 
            (string.IsNullOrWhiteSpace(SearchText) || i.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
        ).ToList();

        AvailableIngredients.Clear();
        foreach (var item in filtered)
        {
            AvailableIngredients.Add(item);
        }
    }

    [RelayCommand]
    private void SelectIngredient(Ingredient ingredient)
    {
        if (ingredient != null && !SelectedIngredients.Contains(ingredient))
        {
            SelectedIngredients.Add(ingredient);
            AvailableIngredients.Remove(ingredient);
            AppState.SelectedIngredients = SelectedIngredients.ToList();
        }
    }

    [RelayCommand]
    private void RemoveIngredient(Ingredient ingredient)
    {
        if (ingredient != null && SelectedIngredients.Contains(ingredient))
        {
            SelectedIngredients.Remove(ingredient);
            AppState.SelectedIngredients = SelectedIngredients.ToList();
            FilterIngredients();
        }
    }

    [RelayCommand]
    private async Task AddNewIngredientAsync()
    {
        if (Application.Current?.MainPage != null)
        {
            string result = await Application.Current.MainPage.DisplayPromptAsync("Yeni Malzeme", "Malzeme adını giriniz:");
            if (!string.IsNullOrWhiteSpace(result))
            {
                var newIng = new Ingredient { Name = result, Category = "Diğer", IconUrl = "❓" };
                await _databaseService.SaveIngredientAsync(newIng);
                _allIngredientsDb.Add(newIng);
                FilterIngredients();
            }
        }
    }

}
