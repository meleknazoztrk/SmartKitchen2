using SmartKitchen.Views;
using SmartKitchen.ViewModels;

namespace SmartKitchen;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // 🌙 Uygulamayı telefonun ayarından bağımsız, sonsuza kadar karanlık moda kilitler!
        Application.Current.UserAppTheme = AppTheme.Dark;

#if DEBUG        
        Preferences.Clear();
#endif

        string savedColorHex = Preferences.Get("SelectedThemeColor", "#00FF88");
        Application.Current.Resources["PrimaryColor"] = Color.FromArgb(savedColorHex);

        bool isFirstLaunch = Preferences.Get("IsFirstLaunch", true);

        if (isFirstLaunch)
        {
            MainPage = new OnboardingPage(new OnboardingViewModel());
        }
        else
        {
            MainPage = new AppShell();
        }
    }
}