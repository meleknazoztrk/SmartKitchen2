using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace SmartKitchen.ViewModels;

public partial class OnboardingItem : ObservableObject
{
    [ObservableProperty]
    private string title = string.Empty;

    [ObservableProperty]
    private string description = string.Empty;

    [ObservableProperty]
    private string imageUrl = string.Empty;
}

public partial class OnboardingViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<OnboardingItem> items = new();

    [ObservableProperty]
    private int currentPosition = 0;

    [RelayCommand]
    private void Next()
    {
        if (CurrentPosition < 2) CurrentPosition++;
    }

    [RelayCommand]
    private void Previous()
    {
        if (CurrentPosition > 0) CurrentPosition--;
    }

    public OnboardingViewModel()
    {
        Items.Add(new OnboardingItem
        {
            Title = "SmartKitchen'a Hoş Geldiniz",
            Description = "Evdeki malzemelerle harikalar yaratmaya hazır mısınız?",
            ImageUrl = "👨‍🍳"
        });
        Items.Add(new OnboardingItem
        {
            Title = "Malzemelerinizi Seçin",
            Description = "Dolabınızdaki malzemeleri seçin, biz size en uygun tarifleri bulalım.",
            ImageUrl = "🛒"
        });
        Items.Add(new OnboardingItem
        {
            Title = "Akıllı Eşleşme",
            Description = "Dünya mutfaklarından yüzlerce tarif arasından size özel önerileri keşfedin.",
            ImageUrl = "✨"
        });
    }

    [RelayCommand]
    private void StartApp()
    {
        Preferences.Set("IsFirstLaunch", false);
        Application.Current.MainPage = new AppShell();
    }
}
