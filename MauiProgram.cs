using Microsoft.Extensions.Logging;
using SmartKitchen.Data;
using SmartKitchen.Services;
using SmartKitchen.ViewModels;
using SmartKitchen.Views;

namespace SmartKitchen;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Services
        builder.Services.AddSingleton<FirebaseService>();
        builder.Services.AddSingleton<RecipeService>();

        // ViewModels
        builder.Services.AddTransient<OnboardingViewModel>();
        builder.Services.AddTransient<FridgeViewModel>();
        builder.Services.AddTransient<RecipeListViewModel>();
        builder.Services.AddTransient<RecipeDetailViewModel>();
        builder.Services.AddTransient<AuthViewModel>();
        builder.Services.AddTransient<ProfilePageViewModel>();

        // Pages
        builder.Services.AddTransient<OnboardingPage>();
        builder.Services.AddTransient<FridgePage>();
        builder.Services.AddTransient<RecipeListPage>();
        builder.Services.AddTransient<RecipeDetailPage>();
        builder.Services.AddTransient<AuthPage>();
        builder.Services.AddTransient<ProfilePage>();
        builder.Services.AddTransient<SmartKitchen.ViewModels.AddRecipePageViewModel>();
        builder.Services.AddTransient<SmartKitchen.Views.AddRecipePage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
