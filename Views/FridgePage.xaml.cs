using SmartKitchen.ViewModels;

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
        await _viewModel.InitializeAsync();
    }
}
