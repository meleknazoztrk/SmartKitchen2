using Microsoft.Maui.Controls;
using SmartKitchen.ViewModels;

namespace SmartKitchen.Views;

public partial class AddRecipePage : ContentPage
{
    public AddRecipePage(AddRecipePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
