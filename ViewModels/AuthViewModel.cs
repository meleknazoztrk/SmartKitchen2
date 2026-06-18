using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartKitchen.Data;
using SmartKitchen.Models;

namespace SmartKitchen.ViewModels;

public partial class AuthViewModel : ObservableObject
{
    private readonly DatabaseService _databaseService;

    public AuthViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    // --- Mod: true = Giriş Yap, false = Kaydol ---
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsRegisterVisible))]
    private bool isLoginVisible = true;

    public bool IsRegisterVisible => !IsLoginVisible;

    // --- Form alanları ---
    [ObservableProperty]
    private string username = string.Empty;

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    // --- Sekme komutları ---
    [RelayCommand]
    private void ShowLogin()
    {
        IsLoginVisible = true;
        Username = string.Empty;
        Email = string.Empty;
        Password = string.Empty;
    }

    [RelayCommand]
    private void ShowRegister()
    {
        IsLoginVisible = false;
        Username = string.Empty;
        Email = string.Empty;
        Password = string.Empty;
    }

    // --- Giriş Yap ---
    [RelayCommand]
    private async Task LoginAsync()
    {
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            await Shell.Current.DisplayAlert("Hata", "Kullanıcı adı ve şifre boş olamaz.", "Tamam");
            return;
        }

        var user = await _databaseService.GetUserByUsernameAsync(Username);

        if (user == null)
        {
            bool goRegister = await Shell.Current.DisplayAlert(
                "Hesap Bulunamadı",
                "Bu kullanıcı adıyla bir hesap bulamadık. Yeni bir hesap oluşturmak için kayıt bölümüne geçmek ister misiniz?",
                "Kayıt Ol",
                "İptal");

            if (goRegister)
            {
                IsLoginVisible = false;
                Username = string.Empty;
                Email = string.Empty;
                Password = string.Empty;
            }
            return;
        }

        if (user.Password != Password)
        {
            await Shell.Current.DisplayAlert("Hata", "Şifre yanlış. Lütfen tekrar deneyin.", "Tamam");
            return;
        }

        Preferences.Set("IsLoggedIn", true);
        Preferences.Set("CurrentUsername", user.Username);
        await Shell.Current.GoToAsync("..");
    }

    // --- Kaydol ---
    [RelayCommand]
    private async Task RegisterAsync()
    {
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            await Shell.Current.DisplayAlert("Hata", "Kullanıcı adı ve şifre boş olamaz.", "Tamam");
            return;
        }

        var existing = await _databaseService.GetUserByUsernameAsync(Username);
        if (existing != null)
        {
            await Shell.Current.DisplayAlert("Hata", "Bu kullanıcı adı zaten kullanılıyor.", "Tamam");
            return;
        }

        var newUser = new User
        {
            Username = Username,
            Email = Email,
            Password = Password
        };
        await _databaseService.SaveUserAsync(newUser);

        // Kayıt başarılı → Login moduna geç, uyarı ver
        await Shell.Current.DisplayAlert("Kayıt Başarılı!", "Lütfen giriş yapın.", "Tamam");
        IsLoginVisible = true;
        Username = string.Empty;
        Email = string.Empty;
        Password = string.Empty;
    }

    // --- Misafir ---
    [RelayCommand]
    private async Task ContinueAsGuestAsync()
    {
        Preferences.Set("IsLoggedIn", false);
        Preferences.Set("CurrentUsername", string.Empty);
        await Shell.Current.GoToAsync("..");
    }
}
