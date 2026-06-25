using SmartKitchen.Views;

namespace SmartKitchen;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute("RecipeDetailPage", typeof(RecipeDetailPage));
        Routing.RegisterRoute("AuthPage", typeof(AuthPage));
        Routing.RegisterRoute("ProfilePage", typeof(ProfilePage));
        Routing.RegisterRoute("AddRecipePage", typeof(SmartKitchen.Views.AddRecipePage));
    }
}
