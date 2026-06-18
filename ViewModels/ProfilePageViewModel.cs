using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartKitchen.Data;
using SmartKitchen.Models;
using System.Collections.ObjectModel;

namespace SmartKitchen.ViewModels;

public partial class ProfilePageViewModel : ObservableObject
{
    private readonly DatabaseService _databaseService;

    public ProfilePageViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
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

    public async Task InitializeAsync()
    {
        var currentUsername = Preferences.Get("CurrentUsername", string.Empty);
        if (string.IsNullOrWhiteSpace(currentUsername)) return;

        var user = await _databaseService.GetUserByUsernameAsync(currentUsername);
        if (user != null)
        {
            Username = user.Username;
            Email = string.IsNullOrEmpty(user.Email) ? "—" : user.Email;
            Password = new string('•', user.Password.Length); // gizlenmiş gösterim
        }

        var recipes = await _databaseService.GetRecipesByUserIdAsync(currentUsername);
        MyRecipes.Clear();
        foreach (var r in recipes)
            MyRecipes.Add(r);

        HasRecipes = MyRecipes.Count > 0;
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
}
