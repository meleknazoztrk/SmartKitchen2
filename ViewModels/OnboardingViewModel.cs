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
    private readonly List<OnboardingItem> _items;

    public OnboardingViewModel()
    {
        _items = new List<OnboardingItem>
        {
            new OnboardingItem
            {
                Title = "SmartKitchen'a Hoş Geldiniz",
                Description = "Evdeki malzemelerle harikalar yaratmaya hazır mısınız?",
                ImageUrl = "👨‍🍳"
            },
            new OnboardingItem
            {
                Title = "Malzemelerinizi Seçin",
                Description = "Dolabınızdaki malzemeleri seçin, biz size en uygun tarifleri bulalım.",
                ImageUrl = "🛒"
            },
            new OnboardingItem
            {
                Title = "Akıllı Eşleşme",
                Description = "Dünya mutfaklarından yüzlerce tarif arasından size özel önerileri keşfedin.",
                ImageUrl = "✨"
            }
        };

        UpdateCurrentItem();
    }

    [ObservableProperty]
    private string currentEmoji = string.Empty;

    [ObservableProperty]
    private string currentTitle = string.Empty;

    [ObservableProperty]
    private string currentDescription = string.Empty;

    [ObservableProperty]
    private int currentPosition = 0;

    // Nokta göstergeleri için renkler
    [ObservableProperty] private string dot0Color = "#00FF88";
    [ObservableProperty] private string dot1Color = "#444444";
    [ObservableProperty] private string dot2Color = "#444444";

    [RelayCommand]
    private void Next()
    {
        if (CurrentPosition < _items.Count - 1)
        {
            CurrentPosition++;
            UpdateCurrentItem();
        }
    }

    [RelayCommand]
    private void Previous()
    {
        if (CurrentPosition > 0)
        {
            CurrentPosition--;
            UpdateCurrentItem();
        }
    }

    private void UpdateCurrentItem()
    {
        var item = _items[CurrentPosition];
        CurrentEmoji = item.ImageUrl;
        CurrentTitle = item.Title;
        CurrentDescription = item.Description;

        Dot0Color = CurrentPosition == 0 ? "#00FF88" : "#444444";
        Dot1Color = CurrentPosition == 1 ? "#00FF88" : "#444444";
        Dot2Color = CurrentPosition == 2 ? "#00FF88" : "#444444";
    }

    [RelayCommand]
    private void StartApp()
    {
        Preferences.Set("IsFirstLaunch", false);
        Application.Current.MainPage = new AppShell();
    }
}
