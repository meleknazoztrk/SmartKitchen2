using SmartKitchen.Models;

namespace SmartKitchen.Services;

public class RecipeService
{
    private readonly FirebaseService _firebaseService;

    public RecipeService(FirebaseService firebaseService)
    {
        _firebaseService = firebaseService;
    }

    public async Task<List<RecipeMatchResult>> CalculateBestMatchesAsync(List<Ingredient> userIngredients)
    {
        var allRecipes = await _firebaseService.GetRecipesAsync();
        var allIngredients = await _firebaseService.GetIngredientsAsync();

        var results = new List<RecipeMatchResult>();
        var userIngredientNames = userIngredients?
            .Where(i => i != null && !string.IsNullOrWhiteSpace(i.Name))
            .Select(i => i.Name.Trim().ToLower())
            .ToHashSet() ?? new HashSet<string>();

        foreach (var recipe in allRecipes)
        {
            if (recipe == null) continue;
            var requiredRecipeIngredients = recipe.Ingredients;
            if (requiredRecipeIngredients == null || requiredRecipeIngredients.Count == 0) continue;

            int matchCount = 0;
            var missingIngredients = new List<Ingredient>();
            int totalValidIngredients = 0;

            foreach (var req in requiredRecipeIngredients)
            {
                if (req == null || string.IsNullOrWhiteSpace(req.IngredientName)) continue;
                totalValidIngredients++;
                string reqName = req.IngredientName.Trim().ToLower();

                // Check if user has this ingredient (exact or partial name match)
                bool isMatched = userIngredientNames.Contains(reqName) || 
                                 userIngredientNames.Any(un => reqName.Contains(un) || un.Contains(reqName));
                
                if (isMatched)
                {
                    matchCount++;
                }
                else
                {
                    // Find the ingredient in allIngredients to show in missing list
                    var ing = allIngredients?.FirstOrDefault(i => i != null && i.Name.Equals(req.IngredientName, StringComparison.OrdinalIgnoreCase));
                    if (ing == null)
                    {
                        // Fallback: create a temporary Ingredient object so it displays correctly
                        ing = new Ingredient { Name = req.IngredientName, IconUrl = "🔍", Category = "Diğer" };
                    }
                    missingIngredients.Add(ing);
                }
            }

            if (totalValidIngredients == 0) continue;

            double matchPercentage = (double)matchCount / totalValidIngredients * 100.0;

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
