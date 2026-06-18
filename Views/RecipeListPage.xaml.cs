using SmartKitchen.ViewModels;

namespace SmartKitchen.Views;

public partial class RecipeListPage : ContentPage
{
    private readonly RecipeListViewModel _viewModel;

    public RecipeListPage(RecipeListViewModel viewModel)
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
