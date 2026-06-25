using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartKitchen.Services;
using SmartKitchen.Models;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace SmartKitchen.ViewModels;

public partial class AddRecipePageViewModel : ObservableObject
{
    private readonly FirebaseService _firebaseService;

    [ObservableProperty]
    private string title = string.Empty;

    [ObservableProperty]
    private string instructions = string.Empty;

    [ObservableProperty]
    private string prepTimeMinutes = string.Empty;

    [ObservableProperty]
    private string calories = string.Empty;

    [ObservableProperty]
    private bool isPublic = false;

    [ObservableProperty]
    private bool isEmojiPickerVisible = false;

    public ObservableCollection<string> AvailableEmojis { get; } = new() 
    { 
        "🍅", "🧅", "🥦", "🥩", "🍗", "🍞", "🧀", "🍎", "🥛", "🥚", 
        "🥔", "🥕", "🧈", "🍚", "🍝", "🥬", "🍄", "🫑", "🫒", "🧂", 
        "🌾", "🧊", "🍋", "🍌", "🥣", "🥫", "🌿", "🌶️", "🧆", "🫘", 
        "🌻", "🏺", "🥓", "🌭", "🌯", "🟢", "🍆", "🥒", "🪵", "🍫", 
        "🧁", "🌼", "🥥", "🍪", "🍓", "🍒", "🍯", "🍃", "🌮", "🥑", 
        "🫚", "🍜", "🍔", "🍕"
    };

    [ObservableProperty]
    private string selectedEmoji = string.Empty;

    private string _pendingIngredientName = string.Empty;

    private List<IngredientItemViewModel> _allIngredients = new();

    [ObservableProperty]
    private string searchQuery = string.Empty;

    partial void OnSearchQueryChanged(string value) => FilterIngredients();

    private void FilterIngredients()
    {
        if (string.IsNullOrWhiteSpace(SearchQuery))
        {
            Ingredients.Clear();
            foreach (var item in _allIngredients) Ingredients.Add(item);
            return;
        }

        var filtered = _allIngredients.Where(i => i.Ingredient.Name.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase)).ToList();
        Ingredients.Clear();
        foreach (var item in filtered) Ingredients.Add(item);
    }

    public ObservableCollection<IngredientItemViewModel> Ingredients { get; set; } = new();

    public AddRecipePageViewModel(FirebaseService firebaseService)
    {
        _firebaseService = firebaseService;
        LoadIngredientsAsync();
    }

    private async void LoadIngredientsAsync()
    {
        var dbIngredients = await _firebaseService.GetIngredientsAsync();
        foreach (var ing in dbIngredients)
        {
            var item = new IngredientItemViewModel
            {
                Ingredient = ing,
                IsSelected = false
            };
            _allIngredients.Add(item);
            Ingredients.Add(item);
        }
    }

    [RelayCommand]
    private async Task AddNewIngredientAsync()
    {
        if (Application.Current?.MainPage == null) return;

        string name = await Application.Current.MainPage.DisplayPromptAsync("Yeni Malzeme", "Malzeme Adı:");
        if (string.IsNullOrWhiteSpace(name)) return;

        bool exists = _allIngredients.Any(i => i.Ingredient.Name.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase));
        if (exists)
        {
            await Application.Current.MainPage.DisplayAlert("Hata", "Bu malzeme zaten listede bulunuyor.", "Tamam");
            return;
        }

        _pendingIngredientName = name.Trim();
        SelectedEmoji = string.Empty;
        IsEmojiPickerVisible = true;
    }

    [RelayCommand]
    private async Task ConfirmEmojiAsync()
    {
        string emojiToUse = string.IsNullOrWhiteSpace(SelectedEmoji) ? "❓" : SelectedEmoji;
        await FinalizeIngredientAddAsync(emojiToUse);
    }

    [RelayCommand]
    private async Task SkipEmojiAsync()
    {
        await FinalizeIngredientAddAsync("❓");
    }

    private async Task FinalizeIngredientAddAsync(string emoji)
    {
        IsEmojiPickerVisible = false;

        if (string.IsNullOrWhiteSpace(_pendingIngredientName)) return;

        string currentUserId = Preferences.Get("CurrentUsername", string.Empty);
        var newIngredient = new Ingredient
        {
            Name = _pendingIngredientName,
            IconUrl = emoji,
            Category = "Diğer",
            UserId = currentUserId
        };

        await _firebaseService.SaveIngredientAsync(newIngredient);

        var newItem = new IngredientItemViewModel
        {
            Ingredient = newIngredient,
            IsSelected = true
        };

        _allIngredients.Insert(0, newItem);
        SearchQuery = string.Empty; // Resetting search query will automatically trigger FilterIngredients
        _pendingIngredientName = string.Empty;
    }

    [RelayCommand]
    private async Task SaveRecipeAsync()
    {
        if (string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(Instructions))
        {
            await Shell.Current.DisplayAlert("Hata", "Tarif adı ve yapılışı boş bırakılamaz.", "Tamam");
            return;
        }

        var selectedIngredients = _allIngredients.Where(i => i.IsSelected).ToList();
        
        int time = 0;
        if (string.IsNullOrWhiteSpace(PrepTimeMinutes))
        {
            time = (selectedIngredients.Count * 5) + 10;
        }
        else
        {
            int.TryParse(PrepTimeMinutes, out time);
        }

        int cal = 0;
        if (string.IsNullOrWhiteSpace(Calories))
        {
            cal = selectedIngredients.Count * 115;
        }
        else
        {
            int.TryParse(Calories, out cal);
        }

        string userId = Preferences.Get("CurrentUsername", string.Empty);
        if (string.IsNullOrEmpty(userId))
        {
            userId = "Guest";
        }

        var lines = Instructions.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        var serializedInstructions = JsonSerializer.Serialize(lines);

        var recipe = new Recipe
        {
            Title = Title,
            PortionSize = "4 Kişilik",
            CaloriesPerPortion = $"{cal} kcal",
            Steps = lines.ToList(),
            Ingredients = selectedIngredients.Select(si => new RecipeIngredient
            {
                IngredientName = si.Ingredient.Name,
                Amount = "Göz kararı",
                RecipeId = 0,
                IngredientId = si.Ingredient.Id,
                Quantity = "Göz kararı",
                IsOptional = false
            }).ToList(),

            Instructions = serializedInstructions,
            PrepTimeMinutes = time,
            Calories = cal,
            IsPublic = IsPublic,
            UserId = userId,
            ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=" + new Random().Next(200, 1000),
            Description = "Kullanıcı tarifi",
            ProteinGrams = 0,
            IsHighProtein = false
        };

        await _firebaseService.SaveRecipeAsync(recipe);

        await Shell.Current.DisplayAlert("Başarılı", "Tarif başarıyla eklendi", "Tamam");
        await Shell.Current.GoToAsync("..");
    }
}

public partial class IngredientItemViewModel : ObservableObject
{
    public Ingredient Ingredient { get; set; }

    [ObservableProperty]
    private bool isSelected;
}
