using SmartKitchen.Models;

namespace SmartKitchen.Services;

public class RecipeService
{
    private readonly Data.DatabaseService _databaseService;

    public RecipeService(Data.DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task<List<RecipeMatchResult>> CalculateBestMatchesAsync(List<Ingredient> userIngredients)
    {
        var allRecipes = await _databaseService.GetRecipesAsync();
        var allRecipeIngredients = await _databaseService.GetRecipeIngredientsAsync();
        var allIngredients = await _databaseService.GetIngredientsAsync();

        var results = new List<RecipeMatchResult>();
        var userIngredientIds = userIngredients.Select(i => i.Id).ToHashSet();

        foreach (var recipe in allRecipes)
        {
            var requiredRecipeIngredients = allRecipeIngredients.Where(ri => ri.RecipeId == recipe.Id).ToList();
            if (requiredRecipeIngredients.Count == 0) continue;

            int matchCount = 0;
            var missingIngredients = new List<Ingredient>();

            foreach (var req in requiredRecipeIngredients)
            {
                if (userIngredientIds.Contains(req.IngredientId))
                {
                    matchCount++;
                }
                else
                {
                    var ing = allIngredients.FirstOrDefault(i => i.Id == req.IngredientId);
                    if (ing != null)
                        missingIngredients.Add(ing);
                }
            }

            double matchPercentage = (double)matchCount / requiredRecipeIngredients.Count * 100.0;

            results.Add(new RecipeMatchResult
            {
                Recipe = recipe,
                MatchPercentage = matchPercentage,
                MissingIngredients = missingIngredients
            });
        }

        return results.OrderByDescending(r => r.MatchPercentage).ToList();
    }
}
