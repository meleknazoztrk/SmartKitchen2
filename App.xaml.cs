using SmartKitchen.Views;
using SmartKitchen.ViewModels;

namespace SmartKitchen;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

#if DEBUG
        Preferences.Clear();
#endif

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