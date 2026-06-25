using SmartKitchen.ViewModels;
using Microsoft.Maui.Controls;

namespace SmartKitchen.Views;

public partial class FridgePage : ContentPage
{
    private readonly FridgeViewModel _viewModel;

    public FridgePage(FridgeViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await Task.Delay(500);
        await _viewModel.LoadDataAsync();
    }

    private void OnIngredientTapped(object sender, TappedEventArgs e)
    {
        if (sender is Border border && border.BindingContext is IngredientItemViewModel item)
        {
            item.IsSelected = !item.IsSelected;
            _viewModel.ToggleCartItem(item.Ingredient, item.IsSelected);
            
            if (item.IsSelected && CartIconContainer != null)
            {
                // Animation logic
                CartIconContainer.ScaleTo(1.2, 100, Easing.CubicOut).ContinueWith(t => 
                {
                    MainThread.BeginInvokeOnMainThread(() => 
                    {
                        CartIconContainer.ScaleTo(1.0, 100, Easing.CubicIn);
                    });
                });
            }
        }
    }
}
