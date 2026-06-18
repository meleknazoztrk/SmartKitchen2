using SmartKitchen.ViewModels;

namespace SmartKitchen.Views;

public partial class AuthPage : ContentPage
{
    private readonly AuthViewModel _viewModel;

    public AuthPage(AuthViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }
}
