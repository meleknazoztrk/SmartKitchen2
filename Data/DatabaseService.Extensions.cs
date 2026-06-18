using SmartKitchen.Models;

namespace SmartKitchen.Data;

public partial class DatabaseService
{
    public async Task<List<RecipeIngredientDisplay>> GetRecipeIngredientsDetailedAsync(int recipeId)
    {
        await Init();
        
        var recipeIngredients = await _database.Table<RecipeIngredient>()
            .Where(ri => ri.RecipeId == recipeId)
            .ToListAsync();
            
        var ingredients = await _database.Table<Ingredient>().ToListAsync();
        
        var detailedList = new List<RecipeIngredientDisplay>();
        
        foreach (var ri in recipeIngredients)
        {
            var ing = ingredients.FirstOrDefault(i => i.Id == ri.IngredientId);
            if (ing != null)
            {
                detailedList.Add(new RecipeIngredientDisplay
                {
                    Name = ing.Name,
                    Quantity = ri.Quantity,
                    IconUrl = ing.IconUrl
                });
            }
        }
        
        return detailedList;
    }
}
