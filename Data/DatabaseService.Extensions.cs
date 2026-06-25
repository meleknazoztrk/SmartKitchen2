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

    public async Task<bool> IsUsernameTakenAsync(string username)
    {
        await Init();
        var existingUser = await _database.Table<User>()
            .Where(u => u.Username.ToLower() == username.ToLower())
            .FirstOrDefaultAsync();
        return existingUser != null;
    }

    public async Task<bool> IsEmailTakenAsync(string email)
    {
        await Init();
        var existingUser = await _database.Table<User>()
            .Where(u => u.Email.ToLower() == email.ToLower())
            .FirstOrDefaultAsync();
        return existingUser != null;
    }

    public async Task<int> DeleteRecipeAsync(Recipe recipe)
    {
        await Init();
        
        var recipeIngredients = await _database.Table<RecipeIngredient>()
            .Where(ri => ri.RecipeId == recipe.Id)
            .ToListAsync();
            
        foreach (var ri in recipeIngredients)
        {
            await _database.DeleteAsync(ri);
        }
        
        return await _database.DeleteAsync(recipe);
    }

    public async Task<int> DeleteIngredientAsync(Ingredient ingredient)
    {
        await Init();
        ingredient.IsDeleted = true;
        return await _database.UpdateAsync(ingredient);
    }
}
