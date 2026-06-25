using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartKitchen.Services;
using SmartKitchen.Models;
using System.Collections.ObjectModel;

namespace SmartKitchen.ViewModels;

public partial class FridgeViewModel : ObservableObject
{
    private readonly FirebaseService _firebaseService;
    private List<Ingredient> _allIngredientsDb = new();

    [ObservableProperty]
    private ObservableCollection<IngredientItemViewModel> availableIngredients = new();

    public ObservableCollection<Ingredient> ShoppingCartList { get; set; } = new();

    [ObservableProperty]
    private int cartCount = 0;

    [ObservableProperty]
    private bool isBusy = false;

    [ObservableProperty]
    private string searchText = string.Empty;

    [ObservableProperty]
    private bool isEmojiPickerVisible = false;

    [ObservableProperty]
    private bool isCartPopupVisible = false;

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

    public FridgeViewModel(FirebaseService firebaseService)
    {
        _firebaseService = firebaseService;
    }

    public async Task LoadDataAsync()
    {
        if (IsBusy) return;
        IsBusy = true;
        try
        {
            _allIngredientsDb = await _firebaseService.GetIngredientsAsync();
            
            ShoppingCartList.Clear();
            if (AppState.SelectedIngredients != null)
            {
                foreach(var ing in AppState.SelectedIngredients)
                    ShoppingCartList.Add(ing);
            }
            
            CartCount = ShoppingCartList.Count;
            FilterIngredients();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                if (Shell.Current != null)
                {
                    await Shell.Current.DisplayAlert("Dolap Yükleme Hatası", ex.ToString(), "Tamam");
                }
            });
        }
        finally
        {
            IsBusy = false;
        }
    }

    partial void OnSearchTextChanged(string value)
    {
        FilterIngredients();
    }

    private void FilterIngredients()
    {
        var filtered = _allIngredientsDb.Where(i => 
            (string.IsNullOrWhiteSpace(SearchText) || i.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
        ).ToList();

        AvailableIngredients.Clear();
        foreach (var item in filtered)
        {
            bool isSelected = ShoppingCartList.Any(s => s.Id == item.Id);
            AvailableIngredients.Add(new IngredientItemViewModel
            {
                Ingredient = item,
                IsSelected = isSelected
            });
        }
    }

    public void ToggleCartItem(Ingredient ingredient, bool isAdded)
    {
        var exists = ShoppingCartList.FirstOrDefault(i => i.Id == ingredient.Id);
        if (isAdded && exists == null)
        {
            ShoppingCartList.Add(ingredient);
        }
        else if (!isAdded && exists != null)
        {
            ShoppingCartList.Remove(exists);
        }
        
        CartCount = ShoppingCartList.Count;
        AppState.SelectedIngredients = ShoppingCartList.ToList();
    }

    [RelayCommand]
    private void OpenCartPopup()
    {
        if (ShoppingCartList.Count > 0)
        {
            IsCartPopupVisible = true;
        }
        else if (Application.Current?.MainPage != null)
        {
            Application.Current.MainPage.DisplayAlert("Bilgi", "Sepetiniz henüz boş.", "Tamam");
        }
    }

    [RelayCommand]
    private void CloseCartPopup()
    {
        IsCartPopupVisible = false;
    }

    [RelayCommand]
    private void RemoveFromCart(Ingredient ingredient)
    {
        var exists = ShoppingCartList.FirstOrDefault(i => i.Id == ingredient.Id);
        if (exists != null)
        {
            ShoppingCartList.Remove(exists);
            CartCount = ShoppingCartList.Count;
            AppState.SelectedIngredients = ShoppingCartList.ToList();
            
            var itemInList = AvailableIngredients.FirstOrDefault(i => i.Ingredient.Id == ingredient.Id);
            if (itemInList != null)
            {
                itemInList.IsSelected = false;
            }
        }
        
        if (ShoppingCartList.Count == 0)
        {
            IsCartPopupVisible = false;
        }
    }

    [RelayCommand]
    private void ClearCart()
    {
        ShoppingCartList.Clear();
        CartCount = 0;
        AppState.SelectedIngredients.Clear();
        
        foreach (var item in AvailableIngredients)
        {
            item.IsSelected = false;
        }
    }

    [RelayCommand]
    private async Task AddNewIngredientAsync()
    {
        bool isLoggedIn = Preferences.Get("IsLoggedIn", false);
        if (!isLoggedIn)
        {
            if (Application.Current?.MainPage != null)
            {
                await Application.Current.MainPage.DisplayAlert("Giriş Gerekli", "Dolabınıza kendi malzemenizi eklemek için lütfen giriş yapın.", "Tamam");
            }
            return;
        }

        if (Application.Current?.MainPage == null) return;

        string name = await Application.Current.MainPage.DisplayPromptAsync("Yeni Malzeme", "Malzeme Adı:");
        if (string.IsNullOrWhiteSpace(name)) return;

        bool exists = _allIngredientsDb.Any(i => i.Name.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase));
        if (exists)
        {
            await Application.Current.MainPage.DisplayAlert("Hata", "Bu malzeme zaten var.", "Tamam");
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
        var newIng = new Ingredient { 
            Name = _pendingIngredientName, 
            Category = "Diğer", 
            IconUrl = emoji,
            UserId = currentUserId
        };
        
        await _firebaseService.SaveIngredientAsync(newIng);
        _allIngredientsDb.Add(newIng);
        
        ShoppingCartList.Add(newIng);
        CartCount = ShoppingCartList.Count;
        AppState.SelectedIngredients = ShoppingCartList.ToList();

        FilterIngredients();
        _pendingIngredientName = string.Empty;
    }
}
