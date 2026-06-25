using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartKitchen.Services;
using SmartKitchen.Models;
using System.Text.RegularExpressions;

namespace SmartKitchen.ViewModels;

public partial class AuthViewModel : ObservableObject
{
    private readonly FirebaseService _firebaseService;

    public AuthViewModel(FirebaseService firebaseService)
    {
        _firebaseService = firebaseService;
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

        try
        {
            var user = await _firebaseService.GetUserByUsernameAsync(Username);

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
            if (Username == "meleknazzx")
            {
                Preferences.Set("IsAdmin", true);
            }
            else
            {
                Preferences.Set("IsAdmin", false);
            }

            Preferences.Set("IsLoggedIn", true);
            Preferences.Set("CurrentUsername", user.Username);

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.GoToAsync("..");
            });
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Hata", $"Giriş yapılırken bir sorun oluştu: {ex.Message}", "Tamam");
        }
    }

    // --- Kaydol ---
    [RelayCommand]
    private async Task RegisterAsync()
    {
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(Email))
        {
            await Shell.Current.DisplayAlert("Hata", "Tüm alanları doldurmanız gerekmektedir.", "Tamam");
            return;
        }

        if (!Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            if (Application.Current?.MainPage != null)
            {
                await Application.Current.MainPage.DisplayAlert("Hata", "Geçersiz e-posta formatı. Lütfen tam bir e-posta adresi girin.", "Tamam");
            }
            return;
        }

        try
        {
            bool isTaken = await _firebaseService.IsUsernameTakenAsync(Username);
            if (isTaken)
            {
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Hata", "Bu kullanıcı adı zaten kullanılıyor. Lütfen başka bir isim seçin.", "Tamam");
                }
                return;
            }

            bool isEmailTaken = await _firebaseService.IsEmailTakenAsync(Email);
            if (isEmailTaken)
            {
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Hata", "Bu e-posta adresi zaten kullanılıyor.", "Tamam");
                }
                return;
            }

            var newUser = new User
            {
                Username = Username,
                Email = Email,
                Password = Password
            };
            await _firebaseService.SaveUserAsync(newUser);

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                // Kayıt başarılı → Login moduna geç, uyarı ver
                await Shell.Current.DisplayAlert("Kayıt Başarılı!", "Lütfen giriş yapın.", "Tamam");
                IsLoginVisible = true;
                Username = string.Empty;
                Email = string.Empty;
                Password = string.Empty;
            });
        }
        catch (Exception ex)
        {
            if (Application.Current?.MainPage != null)
            {
                await Application.Current.MainPage.DisplayAlert("Hata", $"Kayıt olurken bir sorun oluştu: {ex.Message}", "Tamam");
            }
        }
    }

    // --- Misafir ---
    [RelayCommand]
    private async Task ContinueAsGuestAsync()
    {
        try
        {
            Preferences.Set("IsLoggedIn", false);
            Preferences.Set("CurrentUsername", string.Empty);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error setting guest preferences: {ex.Message}");
        }
        finally
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                try
                {
                    await Shell.Current.GoToAsync("///FridgePage");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Guest navigation failed: {ex.Message}");
                }
            });
        }
    }
}
