using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartKitchen.Services;
using SmartKitchen.Models;
using System.Collections.ObjectModel;

namespace SmartKitchen.ViewModels;

public partial class ProfilePageViewModel : ObservableObject
{
    private readonly FirebaseService _firebaseService;

    public ProfilePageViewModel(FirebaseService firebaseService)
    {
        _firebaseService = firebaseService;
    }

    [ObservableProperty]
    private string username = string.Empty;

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [ObservableProperty]
    private ObservableCollection<Recipe> myRecipes = new();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasNoRecipes))]
    private bool hasRecipes = false;

    public bool HasNoRecipes => !HasRecipes;

    [ObservableProperty]
    private ObservableCollection<Ingredient> myIngredients = new();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasNoIngredients))]
    private bool hasIngredients = false;

    public bool HasNoIngredients => !HasIngredients;

    public async Task InitializeAsync()
    {
        var currentUsername = Preferences.Get("CurrentUsername", string.Empty);
        if (string.IsNullOrWhiteSpace(currentUsername)) return;

        var user = await _firebaseService.GetUserByUsernameAsync(currentUsername);
        if (user != null)
        {
            Username = user.Username;
            Email = string.IsNullOrEmpty(user.Email) ? "—" : user.Email;
            Password = new string('•', user.Password.Length); // gizlenmiş gösterim
        }

        var recipes = await _firebaseService.GetRecipesByUserIdAsync(currentUsername);
        MyRecipes.Clear();
        foreach (var r in recipes)
            MyRecipes.Add(r);

        HasRecipes = MyRecipes.Count > 0;

        var allIngredients = await _firebaseService.GetIngredientsAsync();
        MyIngredients.Clear();
        foreach (var i in allIngredients)
        {
            if (i.UserId == currentUsername && !string.IsNullOrEmpty(currentUsername))
            {
                MyIngredients.Add(i);
            }
        }
        HasIngredients = MyIngredients.Count > 0;
    }

    [RelayCommand]
    private async Task CloseAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task LogoutAsync()
    {
        Preferences.Set("IsLoggedIn", false);
        Preferences.Set("CurrentUsername", string.Empty);
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task DeleteRecipeAsync(Recipe recipe)
    {
        if (recipe == null) return;

        bool confirm = await Shell.Current.DisplayAlert("Emin misiniz?", "Bu tarifi silmek istediğinize emin misiniz?", "Evet", "Hayır");
        if (confirm)
        {
            await _firebaseService.DeleteRecipeAsync(recipe);
            MyRecipes.Remove(recipe);
            HasRecipes = MyRecipes.Count > 0;
        }
    }

    [RelayCommand]
    private async Task DeleteIngredientAsync(Ingredient ingredient)
    {
        if (ingredient == null) return;

        bool confirm = await Shell.Current.DisplayAlert("Emin misiniz?", "Bu malzemeyi silmek istediğinize emin misiniz?", "Evet", "Hayır");
        if (confirm)
        {
            await _firebaseService.DeleteIngredientAsync(ingredient);
            MyIngredients.Remove(ingredient);
            HasIngredients = MyIngredients.Count > 0;
        }
    }

    [RelayCommand]
    private void ChangeTheme(string hexColor)
    {
        if (string.IsNullOrEmpty(hexColor)) return;
        
        Application.Current.Resources["PrimaryColor"] = Color.FromArgb(hexColor);
        Preferences.Set("SelectedThemeColor", hexColor);
    }
}
