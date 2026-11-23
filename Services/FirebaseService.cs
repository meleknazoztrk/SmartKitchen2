using Firebase.Database;
using Firebase.Database.Query;
using SmartKitchen.Models;
using Newtonsoft.Json;

namespace SmartKitchen.Services;

public class FirebaseService
{
    private readonly FirebaseClient _firebaseClient;

    public FirebaseService()
    {
        _firebaseClient = new FirebaseClient("https://smart-234-default-rtdb.firebaseio.com/");
    }

    private bool _isInitialized = false;

    public async Task Init()
    {
        _isInitialized = true;
        await Task.CompletedTask;
    }

    public async Task<List<Ingredient>> GetIngredientsAsync()
    {
        try
        {
            await Init();
            var items = await _firebaseClient.Child("Ingredients").OnceAsync<Ingredient>();
            return items.Select(x => x.Object).Where(i => !i.IsDeleted).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"GetIngredientsAsync Error: {ex}");
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                if (Shell.Current != null)
                {
                    await Shell.Current.DisplayAlert("GetIngredientsAsync Hatası", ex.ToString(), "Tamam");
                }
            });
            return new List<Ingredient>();
        }
    }

    public async Task<int> SaveIngredientAsync(Ingredient item)
    {
        await Init();
        if (item.Id == 0)
        {
            var existing = await _firebaseClient.Child("Ingredients").OnceAsync<Ingredient>();
            item.Id = existing.Count > 0 ? existing.Max(x => x.Object.Id) + 1 : 1;
        }
        await _firebaseClient.Child("Ingredients").Child("Ingredient_" + item.Id).PutAsync(item);
        return item.Id;
    }

    public async Task<List<Recipe>> GetRecipesAsync()
    {
        try
        {
            await Init();
            var list = await _firebaseClient.Child("Recipes").OnceSingleAsync<List<Recipe>>();
            if (list == null) return new List<Recipe>();
            
            var result = new List<Recipe>();
            for (int i = 0; i < list.Count; i++)
            {
                var recipe = list[i];
                if (recipe != null)
                {
                    recipe.Id = i;
                    recipe.Steps ??= new();
                    recipe.Ingredients ??= new();
                    result.Add(recipe);
                }
            }
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"GetRecipesAsync Error: {ex}");
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                if (Shell.Current != null)
                {
                    await Shell.Current.DisplayAlert("GetRecipesAsync Hatası", ex.ToString(), "Tamam");
                }
            });
            return new List<Recipe>();
        }
    }

    public async Task<int> SaveRecipeAsync(Recipe recipe)
    {
        try
        {
            await Init();
            var existing = await GetRecipesAsync();
            int newIndex = existing.Count;
            recipe.Id = newIndex;
            await _firebaseClient.Child("Recipes").Child(newIndex.ToString()).PutAsync(recipe);
            return newIndex;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"SaveRecipeAsync Error: {ex}");
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Hata", $"Tarif kaydedilirken bir hata oluştu: {ex.Message}", "Tamam");
                }
            });
            return -1;
        }
    }

    public async Task<List<RecipeIngredient>> GetRecipeIngredientsAsync()
    {
        return new List<RecipeIngredient>();
    }

    public async Task<int> SaveRecipeIngredientAsync(RecipeIngredient item)
    {
        return 0;
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        try
        {
            await Init();
            var items = await _firebaseClient.Child("Users").OnceAsync<User>();
            return items.Select(x => x.Object).FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"GetUserByUsernameAsync Error: {ex}");
            return null;
        }
    }

    public async Task<int> SaveUserAsync(User user)
    {
        await Init();
        if (user.Id == 0)
        {
            var existing = await _firebaseClient.Child("Users").OnceAsync<User>();
            user.Id = existing.Count > 0 ? existing.Max(x => x.Object.Id) + 1 : 1;
        }
        await _firebaseClient.Child("Users").Child("User_" + user.Id).PutAsync(user);
        return user.Id;
    }

    public async Task<List<Recipe>> GetRecipesByUserIdAsync(string userId)
    {
        try
        {
            await Init();
            var items = await GetRecipesAsync();
            return items.Where(r => r.UserId == userId).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"GetRecipesByUserIdAsync Error: {ex}");
            return new List<Recipe>();
        }
    }

    public async Task<List<RecipeIngredientDisplay>> GetRecipeIngredientsDetailedAsync(int recipeId)
    {
        try
        {
            await Init();
            
            var recipes = await GetRecipesAsync();
            var recipe = recipes.FirstOrDefault(r => r.Id == recipeId);
            if (recipe == null || recipe.Ingredients == null) return new List<RecipeIngredientDisplay>();

            var ingredientsData = await _firebaseClient.Child("Ingredients").OnceAsync<Ingredient>();
            var ingredients = ingredientsData.Select(x => x.Object).ToList();
            
            var detailedList = new List<RecipeIngredientDisplay>();
            
            foreach (var ri in recipe.Ingredients)
            {
                if (ri == null || string.IsNullOrWhiteSpace(ri.IngredientName)) continue;
                var ing = ingredients?.FirstOrDefault(i => i != null && i.Name.Equals(ri.IngredientName, StringComparison.OrdinalIgnoreCase));
                detailedList.Add(new RecipeIngredientDisplay
                {
                    Name = ri.IngredientName,
                    Quantity = ri.Amount ?? string.Empty,
                    IconUrl = ing?.IconUrl ?? "🥘"
                });
            }
            
            return detailedList;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"GetRecipeIngredientsDetailedAsync Error: {ex}");
            return new List<RecipeIngredientDisplay>();
        }
    }


    public async Task<bool> IsUsernameTakenAsync(string username)
    {
        await Init();
        var items = await _firebaseClient.Child("Users").OnceAsync<User>();
        return items.Select(x => x.Object).Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<bool> IsEmailTakenAsync(string email)
    {
        await Init();
        var items = await _firebaseClient.Child("Users").OnceAsync<User>();
        return items.Select(x => x.Object).Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<int> DeleteRecipeAsync(Recipe recipe)
    {
        try
        {
            await Init();
            await _firebaseClient.Child("Recipes").Child(recipe.Id.ToString()).DeleteAsync();
            return 1;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"DeleteRecipeAsync Error: {ex}");
            return 0;
        }
    }

    public async Task<int> DeleteIngredientAsync(Ingredient ingredient)
    {
        await Init();
        ingredient.IsDeleted = true;
        await _firebaseClient.Child("Ingredients").Child("Ingredient_" + ingredient.Id).PutAsync(ingredient);
        return 1;
    }

    private async Task SeedDataAsync()
    {
        var items = await _firebaseClient.Child("Ingredients").OnceAsync<Ingredient>();
        List<Ingredient> insertedIngredients;

        if (items.Count == 0)
        {

        var ingredients = new List<Ingredient>
        {
            new Ingredient { Name = "Domates", Category = "Sebze", IconUrl = "🍅" },
            new Ingredient { Name = "Yumurta", Category = "Kahvaltılık", IconUrl = "🥚" },
            new Ingredient { Name = "Kaşar Peyniri", Category = "Süt Ürünleri", IconUrl = "🧀" },
            new Ingredient { Name = "Tavuk Göğsü", Category = "Et", IconUrl = "🍗" },
            new Ingredient { Name = "Soğan", Category = "Sebze", IconUrl = "🧅" },
            new Ingredient { Name = "Sarımsak", Category = "Sebze", IconUrl = "🧄" },
            new Ingredient { Name = "Patates", Category = "Sebze", IconUrl = "🥔" },
            new Ingredient { Name = "Havuç", Category = "Sebze", IconUrl = "🥕" },
            new Ingredient { Name = "Süt", Category = "Süt Ürünleri", IconUrl = "🥛" },
            new Ingredient { Name = "Tereyağı", Category = "Süt Ürünleri", IconUrl = "🧈" },
            new Ingredient { Name = "Kıyma", Category = "Et", IconUrl = "🥩" },
            new Ingredient { Name = "Ekmek", Category = "Fırın", IconUrl = "🍞" },
            new Ingredient { Name = "Pirinç", Category = "Bakliyat", IconUrl = "🍚" },
            new Ingredient { Name = "Makarna", Category = "Bakliyat", IconUrl = "🍝" },
            new Ingredient { Name = "Ispanak", Category = "Sebze", IconUrl = "🥬" },
            new Ingredient { Name = "Mantar", Category = "Sebze", IconUrl = "🍄" },
            new Ingredient { Name = "Biber", Category = "Sebze", IconUrl = "🫑" },
            new Ingredient { Name = "Zeytinyağı", Category = "Yağ", IconUrl = "🫒" },
            new Ingredient { Name = "Tuz", Category = "Baharat", IconUrl = "🧂" },
            new Ingredient { Name = "Karabiber", Category = "Baharat", IconUrl = "🧂" },
            new Ingredient { Name = "Un", Category = "Fırın", IconUrl = "🌾" },
            new Ingredient { Name = "Şeker", Category = "Tatlandırıcı", IconUrl = "🧊" },
            new Ingredient { Name = "Limon", Category = "Meyve", IconUrl = "🍋" },
            new Ingredient { Name = "Elma", Category = "Meyve", IconUrl = "🍎" },
            new Ingredient { Name = "Muz", Category = "Meyve", IconUrl = "🍌" },
            new Ingredient { Name = "Yoğurt", Category = "Süt Ürünleri", IconUrl = "🥣" },
            new Ingredient { Name = "Salça", Category = "Sos", IconUrl = "🥫" },
            new Ingredient { Name = "Maydanoz", Category = "Sebze", IconUrl = "🌿" },
            new Ingredient { Name = "Nane", Category = "Baharat", IconUrl = "🌿" },
            new Ingredient { Name = "Kekik", Category = "Baharat", IconUrl = "🌿" },
            new Ingredient { Name = "Pul Biber", Category = "Baharat", IconUrl = "🌶️" },
            new Ingredient { Name = "Kimyon", Category = "Baharat", IconUrl = "🧂" },
            new Ingredient { Name = "Mercimek", Category = "Bakliyat", IconUrl = "🧆" },
            new Ingredient { Name = "Nohut", Category = "Bakliyat", IconUrl = "🧆" },
            new Ingredient { Name = "Fasulye", Category = "Bakliyat", IconUrl = "🫘" },
            new Ingredient { Name = "Bulgur", Category = "Bakliyat", IconUrl = "🌾" },
            new Ingredient { Name = "Sıvı Yağ", Category = "Yağ", IconUrl = "🌻" },
            new Ingredient { Name = "Sirke", Category = "Sos", IconUrl = "🏺" },
            new Ingredient { Name = "Tavuk Baget", Category = "Et", IconUrl = "🍗" },
            new Ingredient { Name = "Kuşbaşı Et", Category = "Et", IconUrl = "🥩" },
            new Ingredient { Name = "Sucuk", Category = "Şarküteri", IconUrl = "🥓" },
            new Ingredient { Name = "Sosis", Category = "Şarküteri", IconUrl = "🌭" },
            new Ingredient { Name = "Lavaş", Category = "Fırın", IconUrl = "🌯" },
            new Ingredient { Name = "Sucuk", Category = "Şarküteri", IconUrl = "🥓" },
            new Ingredient { Name = "Sosis", Category = "Şarküteri", IconUrl = "🌭" },
            new Ingredient { Name = "Bezelye", Category = "Sebze", IconUrl = "🟢" },
            new Ingredient { Name = "Patlıcan", Category = "Sebze", IconUrl = "🍆" },
            new Ingredient { Name = "Kabak", Category = "Sebze", IconUrl = "🥒" },
            new Ingredient { Name = "Salatalık", Category = "Sebze", IconUrl = "🥒" },
            new Ingredient { Name = "Marul", Category = "Sebze", IconUrl = "🥬" },
            new Ingredient { Name = "Lahana", Category = "Sebze", IconUrl = "🥬" },
            new Ingredient { Name = "Nar Ekşisi", Category = "Sos", IconUrl = "🏺" },
            new Ingredient { Name = "Tarçın", Category = "Baharat", IconUrl = "🪵" },
            new Ingredient { Name = "Kakao", Category = "Tatlandırıcı", IconUrl = "🍫" },
            new Ingredient { Name = "Çikolata", Category = "Tatlı", IconUrl = "🍫" },
            new Ingredient { Name = "Kabartma Tozu", Category = "Fırın", IconUrl = "🧁" },
            new Ingredient { Name = "Vanilya", Category = "Fırın", IconUrl = "🌼" },
            new Ingredient { Name = "Süt Kreması", Category = "Süt Ürünleri", IconUrl = "🥛" },
            new Ingredient { Name = "Hindistan Cevizi", Category = "Meyve", IconUrl = "🥥" },
            new Ingredient { Name = "İrmik", Category = "Fırın", IconUrl = "🌾" },
            new Ingredient { Name = "Yulaf", Category = "Tahıl", IconUrl = "🥣" },
            new Ingredient { Name = "Bisküvi", Category = "Tatlı", IconUrl = "🍪" },
            new Ingredient { Name = "Çilek", Category = "Meyve", IconUrl = "🍓" },
            new Ingredient { Name = "Vişne", Category = "Meyve", IconUrl = "🍒" },
            new Ingredient { Name = "Nar", Category = "Meyve", IconUrl = "🍎" },
            new Ingredient { Name = "Beyaz Peynir", Category = "Süt Ürünleri", IconUrl = "🧀" },
            new Ingredient { Name = "Yeşil Zeytin", Category = "Kahvaltılık", IconUrl = "🫒" },
            new Ingredient { Name = "Siyah Zeytin", Category = "Kahvaltılık", IconUrl = "🫒" },
            new Ingredient { Name = "Bal", Category = "Tatlı", IconUrl = "🍯" },
            new Ingredient { Name = "Pekmez", Category = "Tatlı", IconUrl = "🍯" },
            new Ingredient { Name = "Tel Şehriye", Category = "Bakliyat", IconUrl = "🍝" },
            new Ingredient { Name = "Asma Yaprağı", Category = "Sebze", IconUrl = "🍃" },
            new Ingredient { Name = "Yufka", Category = "Fırın", IconUrl = "🌯" },
            new Ingredient { Name = "Tortilla", Category = "Fırın", IconUrl = "🌮" },
            new Ingredient { Name = "Çedar Peyniri", Category = "Süt Ürünleri", IconUrl = "🧀" },
            new Ingredient { Name = "Jalapeno", Category = "Sebze", IconUrl = "🌶️" },
            new Ingredient { Name = "Avokado", Category = "Meyve", IconUrl = "🥑" },
            new Ingredient { Name = "Misket Limonu", Category = "Meyve", IconUrl = "🍋" },
            new Ingredient { Name = "Soya Sosu", Category = "Sos", IconUrl = "🏺" },
            new Ingredient { Name = "Zencefil", Category = "Sebze", IconUrl = "🫚" },
            new Ingredient { Name = "Susam", Category = "Baharat", IconUrl = "🌾" },
            new Ingredient { Name = "Noodle", Category = "Bakliyat", IconUrl = "🍜" },
            new Ingredient { Name = "Mozzarella", Category = "Süt Ürünleri", IconUrl = "🧀" },
            new Ingredient { Name = "Fesleğen", Category = "Sebze", IconUrl = "🌿" },
            new Ingredient { Name = "Parmesan", Category = "Süt Ürünleri", IconUrl = "🧀" },
            new Ingredient { Name = "Burger Ekmeği", Category = "Fırın", IconUrl = "🍔" },
            new Ingredient { Name = "Turşu", Category = "Sebze", IconUrl = "🥒" },
            new Ingredient { Name = "Hardal", Category = "Sos", IconUrl = "🏺" },
            new Ingredient { Name = "Ketçap", Category = "Sos", IconUrl = "🍅" },
            new Ingredient { Name = "Kuşüzümü", Category = "Kuruyemiş", IconUrl = "🍇" },
            new Ingredient { Name = "Fıstık", Category = "Kuruyemiş", IconUrl = "🥜" },
            new Ingredient { Name = "Sarımsak Tozu", Category = "Baharat", IconUrl = "🧄" },
            new Ingredient { Name = "Krepa", Category = "Fırın", IconUrl = "🥞" }
        };

        int ingId = 1;
        foreach(var ing in ingredients) {
            ing.Id = ingId++;
            await _firebaseClient.Child("Ingredients").Child("Ingredient_" + ing.Id).PutAsync(ing);
        }
        insertedIngredients = ingredients;
    }
    else
    {
        insertedIngredients = items.Select(x => x.Object).ToList();
    }

        int GetId(string name) {
            var ing = insertedIngredients.FirstOrDefault(i => i.Name == name);
            if (ing != null) return ing.Id;
            return 1;
        }

        List<Recipe> recs = null;
        try
        {
            recs = await _firebaseClient.Child("Recipes").OnceSingleAsync<List<Recipe>>();
        }
        catch
        {
            // Ignore
        }
        if (recs == null || recs.Count == 0)
        {
            var recipes = new List<Recipe>
            {
            new Recipe
            {
                Title = "Süzme Mercimek Çorbası",
                Description = "4 Porsiyonluk. Porsiyon başı: 180 kcal, 8g protein, 22g karbonhidrat. Tam kıvamında, lokanta usulü süzme mercimek çorbası.",
                PrepTimeMinutes = 35,
                Calories = 180,
                ProteinGrams = 8,
                IsHighProtein = false,
                ImageUrl = "https://images.unsplash.com/photo-1547592166-23ac45744acd?q=80&w=500",
                Instructions = "[\"Soğan, patates ve havucu küp küp doğrayın. Çorba tenceresine 3 yemek kaşığı sıvı yağ alıp soğanları pembeleşene kadar orta ateşte kavurun.\",\"Doğranmış patates ve havuçları ekleyip 2-3 dakika daha kavurmaya devam edin.\",\"Yıkanmış mercimekleri ekleyin ve tencereye 1.5 litre sıcak su ilave edin.\",\"Sebzeler tamamen yumuşayana kadar kapağı kapalı şekilde yaklaşık 25 dakika orta-kısık ateşte pişirin.\",\"Çorbayı pürüzsüz olana kadar el blenderından geçirin. Tuz ve karabiberini ekleyin.\",\"Küçük bir sos tavasında 1 yemek kaşığı tereyağını eritin. Üzerine 1 tatlı kaşığı nane ekleyip kızdırın. Servis yaparken çorbanın üzerine gezdirin.\"]"
            },
            new Recipe
            {
                Title = "Klasik Menemen",
                Description = "2 Porsiyonluk. Porsiyon başı: 210 kcal, 11g protein, 8g karbonhidrat. Geleneksel kahvaltıların vazgeçilmezi, sulu ve tam kıvamında menemen.",
                PrepTimeMinutes = 15,
                Calories = 210,
                ProteinGrams = 11,
                IsHighProtein = false,
                ImageUrl = "https://images.unsplash.com/photo-1594514757270-e64e101bdce2?q=80&w=500",
                Instructions = "[\"Biberlerin çekirdeklerini çıkarıp ince ince kıyın. Domateslerin kabuklarını soyup küp küp doğrayın.\",\"Tavaya tereyağı ve sıvı yağı alın. Orta ateşte biberleri hafif renk alana kadar yaklaşık 3 dakika soteleyin.\",\"Küp doğranmış domatesleri ekleyin. Tavanın kapağını kapatıp domatesler suyunu salıp yumuşayana kadar yaklaşık 7-8 dakika kısık ateşte pişirin.\",\"Tuz, karabiber og pul biberi ekleyip karıştırın.\",\"Yumurtaları doğrudan tavaya kırın. Akları hafifçe pişene kadar sarılarıyla birlikte yavaşça karıştırın. Yumurtalar çok kurumadan nemli kıvamdayken ocaktan alın.\"]"
            },
            new Recipe
            {
                Title = "Tavuk Sote",
                Description = "3 Porsiyonluk. Porsiyon başı: 290 kcal, 32g protein, 6g karbonhidrat. Yüksek proteinli, sulu ve yumuşacık sebzeli tavuk sote.",
                PrepTimeMinutes = 25,
                Calories = 290,
                ProteinGrams = 32,
                IsHighProtein = true,
                ImageUrl = "https://images.unsplash.com/photo-1604908176997-125f25cc6f3d?q=80&w=500",
                Instructions = "[\"Tavuk göğsünü kuşbaşı şeklinde doğrayın. Soğan ve biberleri yemeklik, domatesleri küp küp doğrayın. Sarımsakları incecik kıyın.\",\"Geniş bir teflon tavayı yüksek ateşte ısıtın. Doğranmış tavukları ekleyin ve suyunu salıp çekene kadar karıştırarak soteleyin.\",\"Suyunu çeken tavuklara 3 yemek kaşığı sıvı yağ, soğanları ve sarımsakları ilave edin. Orta ateşte 4-5 dakika kavurun.\",\"Biberleri ekleyip 2-3 dakika daha soteleyin. Ardından domates salçasını ekleyip kokusu çıkana kadar 1 dakika kavurun.\",\"Son olarak domatesleri, tuzu, pul biberi ve kekiği ekleyin. Tencerenin kapağını kapatıp kısık ateşte 10 dakika pişmeye bırakın.\"]"
            },
            new Recipe
            {
                Title = "Kremalı Mantarlı Makarna",
                Description = "4 Porsiyonluk. Porsiyon başı: 460 kcal, 12g protein, 58g karbonhidrat. İtalyan usulü, krema ve fesleğenle harmanlanmış nefis mantarlı makarna.",
                PrepTimeMinutes = 20,
                Calories = 460,
                ProteinGrams = 12,
                IsHighProtein = false,
                ImageUrl = "https://images.unsplash.com/photo-1645112411341-6c4fd023714a?q=80&w=500",
                Instructions = "[\"Derin bir tencerede bol su kaynatın, içine 1 tatlı kaşığı tuz ekleyin. Makarnayı ekleyip dişe gelecek kıvamda (yaklaşık 9-10 dakika) haşlayıp süzün.\",\"Haşlama esnasında mantarları temizleyip ince ince dilimleyin. Sarımsakları ezin.\",\"Geniş bir tavada tereyağı ve zeytinyağını eritin. Dilimlenmiş mantarları ekleyip yüksek ateşte soteleyin (suyunu salıp çekene kadar).\",\"Ezilmiş sarımsakları ve tuzu ekleyip 1 dakika karıştırın. Ardından sıvı kremayı ve fesleğeni ilave edin. Kısık ateşte 2-3 dakika sosu tıkırdatın.\",\"Süzülen makarnaları sosun içine aktarın. Sosla makarna iyice bütünleşene kadar 1 dakika karıştırıp sıcak servis yapın.\"]"
            },
            new Recipe
            {
                Title = "Anne Köftesi",
                Description = "4 Porsiyonluk. Porsiyon başı: 310 kcal, 24g protein, 10g karbonhidrat. Yumuşacık kıvamı ve tam ölçülü baharatlarıyla geleneksel anne köftesi.",
                PrepTimeMinutes = 30,
                Calories = 310,
                ProteinGrams = 24,
                IsHighProtein = true,
                ImageUrl = "https://images.unsplash.com/photo-1529042410759-befb1204b468?q=80&w=500",
                Instructions = "[\"Soğanı rendeleyip suyunu iyice sıkın (acılık vermemesi için). Maydanozları incecik kıyın. Bayat ekmek içlerini ufalayın.\",\"Geniş bir yoğurma kabına kıymayı, rendelenmiş soğanı, yumurtayı, ufalanmış ekmek içini, maydanozu, tuzu, kimyonu ve karabiberi alın.\",\"Tüm malzemeler tamamen özdeşleşene ve harç toparlanana kadar yaklaşık 10 dakika yoğurun.\",\"Köfte harcından ceviz büyüklüğünde parçalar koparıp elinizle yuvarlayın ve üzerlerine hafifçe bastırarak yassı şekil verin.\",\"Tavaya sıvı yağı ekleyip ısıtın. Köfteleri arkalı önlü, orta ateşte içi sulu kalacak şekilde yaklaşık 3'er dakika kızartın.\"]"
            }
        };

        int recId = 1;
        foreach(var rec in recipes) {
            rec.Id = recId++;
            await _firebaseClient.Child("Recipes").Child("Recipe_" + rec.Id).PutAsync(rec);
        }
        var insertedRecipes = recipes;
        
        int GetRecipeId(string title) {
            var rec = insertedRecipes.FirstOrDefault(r => r.Title == title);
            if (rec != null) return rec.Id;
            return 1;
        }

        var recipeIngredients = new List<RecipeIngredient>
        {
            // Süzme Mercimek Çorbası
            new RecipeIngredient { RecipeId = GetRecipeId("Süzme Mercimek Çorbası"), IngredientId = GetId("Mercimek"), Quantity = "1.5 su bardağı" },
            new RecipeIngredient { RecipeId = GetRecipeId("Süzme Mercimek Çorbası"), IngredientId = GetId("Soğan"), Quantity = "1 adet orta boy" },
            new RecipeIngredient { RecipeId = GetRecipeId("Süzme Mercimek Çorbası"), IngredientId = GetId("Patates"), Quantity = "1 adet orta boy" },
            new RecipeIngredient { RecipeId = GetRecipeId("Süzme Mercimek Çorbası"), IngredientId = GetId("Havuç"), Quantity = "1 adet küçük boy" },
            new RecipeIngredient { RecipeId = GetRecipeId("Süzme Mercimek Çorbası"), IngredientId = GetId("Sıvı Yağ"), Quantity = "3 yemek kaşığı" },
            new RecipeIngredient { RecipeId = GetRecipeId("Süzme Mercimek Çorbası"), IngredientId = GetId("Tereyağı"), Quantity = "1 yemek kaşığı" },
            new RecipeIngredient { RecipeId = GetRecipeId("Süzme Mercimek Çorbası"), IngredientId = GetId("Tuz"), Quantity = "1 tatlı kaşığı" },
            new RecipeIngredient { RecipeId = GetRecipeId("Süzme Mercimek Çorbası"), IngredientId = GetId("Karabiber"), Quantity = "Yarım çay kaşığı" },
            new RecipeIngredient { RecipeId = GetRecipeId("Süzme Mercimek Çorbası"), IngredientId = GetId("Nane"), Quantity = "1 tatlı kaşığı" },

            // Klasik Menemen
            new RecipeIngredient { RecipeId = GetRecipeId("Klasik Menemen"), IngredientId = GetId("Domates"), Quantity = "3 adet olgun orta boy" },
            new RecipeIngredient { RecipeId = GetRecipeId("Klasik Menemen"), IngredientId = GetId("Yumurta"), Quantity = "3 adet büyük boy" },
            new RecipeIngredient { RecipeId = GetRecipeId("Klasik Menemen"), IngredientId = GetId("Biber"), Quantity = "3 adet sivri biber" },
            new RecipeIngredient { RecipeId = GetRecipeId("Klasik Menemen"), IngredientId = GetId("Tereyağı"), Quantity = "1 yemek kaşığı" },
            new RecipeIngredient { RecipeId = GetRecipeId("Klasik Menemen"), IngredientId = GetId("Sıvı Yağ"), Quantity = "1 yemek kaşığı" },
            new RecipeIngredient { RecipeId = GetRecipeId("Klasik Menemen"), IngredientId = GetId("Tuz"), Quantity = "1 çay kaşığı" },
            new RecipeIngredient { RecipeId = GetRecipeId("Klasik Menemen"), IngredientId = GetId("Karabiber"), Quantity = "Yarım çay kaşığı" },
            new RecipeIngredient { RecipeId = GetRecipeId("Klasik Menemen"), IngredientId = GetId("Pul Biber"), Quantity = "Yarım çay kaşığı" },

            // Tavuk Sote
            new RecipeIngredient { RecipeId = GetRecipeId("Tavuk Sote"), IngredientId = GetId("Tavuk Göğsü"), Quantity = "500 gr" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tavuk Sote"), IngredientId = GetId("Soğan"), Quantity = "1 adet büyük boy" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tavuk Sote"), IngredientId = GetId("Biber"), Quantity = "3 adet yeşil/kırmızı biber" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tavuk Sote"), IngredientId = GetId("Domates"), Quantity = "2 adet orta boy" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tavuk Sote"), IngredientId = GetId("Salça"), Quantity = "1 yemek kaşığı" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tavuk Sote"), IngredientId = GetId("Sarımsak"), Quantity = "2 diş" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tavuk Sote"), IngredientId = GetId("Sıvı Yağ"), Quantity = "3 yemek kaşığı" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tavuk Sote"), IngredientId = GetId("Tuz"), Quantity = "1 tatlı kaşığı" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tavuk Sote"), IngredientId = GetId("Kekik"), Quantity = "1 tatlı kaşığı" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tavuk Sote"), IngredientId = GetId("Pul Biber"), Quantity = "1 çay kaşığı" },

            // Kremalı Mantarlı Makarna
            new RecipeIngredient { RecipeId = GetRecipeId("Kremalı Mantarlı Makarna"), IngredientId = GetId("Makarna"), Quantity = "1 paket (500 gr)" },
            new RecipeIngredient { RecipeId = GetRecipeId("Kremalı Mantarlı Makarna"), IngredientId = GetId("Mantar"), Quantity = "1 paket (300 gr)" },
            new RecipeIngredient { RecipeId = GetRecipeId("Kremalı Mantarlı Makarna"), IngredientId = GetId("Süt Kreması"), Quantity = "1 kutu (200 ml)" },
            new RecipeIngredient { RecipeId = GetRecipeId("Kremalı Mantarlı Makarna"), IngredientId = GetId("Sarımsak"), Quantity = "2 diş" },
            new RecipeIngredient { RecipeId = GetRecipeId("Kremalı Mantarlı Makarna"), IngredientId = GetId("Tereyağı"), Quantity = "2 yemek kaşığı" },
            new RecipeIngredient { RecipeId = GetRecipeId("Kremalı Mantarlı Makarna"), IngredientId = GetId("Zeytinyağı"), Quantity = "1 yemek kaşığı" },
            new RecipeIngredient { RecipeId = GetRecipeId("Kremalı Mantarlı Makarna"), IngredientId = GetId("Tuz"), Quantity = "1.5 tatlı kaşığı" },
            new RecipeIngredient { RecipeId = GetRecipeId("Kremalı Mantarlı Makarna"), IngredientId = GetId("Karabiber"), Quantity = "Yarım çay kaşığı" },
            new RecipeIngredient { RecipeId = GetRecipeId("Kremalı Mantarlı Makarna"), IngredientId = GetId("Fesleğen"), Quantity = "1 tatlı kaşığı" },

            // Anne Köftesi
            new RecipeIngredient { RecipeId = GetRecipeId("Anne Köftesi"), IngredientId = GetId("Kıyma"), Quantity = "500 gr" },
            new RecipeIngredient { RecipeId = GetRecipeId("Anne Köftesi"), IngredientId = GetId("Soğan"), Quantity = "1 adet orta boy" },
            new RecipeIngredient { RecipeId = GetRecipeId("Anne Köftesi"), IngredientId = GetId("Yumurta"), Quantity = "1 adet" },
            new RecipeIngredient { RecipeId = GetRecipeId("Anne Köftesi"), IngredientId = GetId("Ekmek"), Quantity = "3 dilim bayat ekmek içi" },
            new RecipeIngredient { RecipeId = GetRecipeId("Anne Köftesi"), IngredientId = GetId("Maydanoz"), Quantity = "Çeyrek demet" },
            new RecipeIngredient { RecipeId = GetRecipeId("Anne Köftesi"), IngredientId = GetId("Kimyon"), Quantity = "1 çay kaşığı" },
            new RecipeIngredient { RecipeId = GetRecipeId("Anne Köftesi"), IngredientId = GetId("Karabiber"), Quantity = "Yarım çay kaşığı" },
            new RecipeIngredient { RecipeId = GetRecipeId("Anne Köftesi"), IngredientId = GetId("Tuz"), Quantity = "1 tatlı kaşığı" },
            new RecipeIngredient { RecipeId = GetRecipeId("Anne Köftesi"), IngredientId = GetId("Sıvı Yağ"), Quantity = "Yarım çay bardağı" }
        };

            int riId = 1;
            foreach(var ri in recipeIngredients) {
                ri.Id = riId++;
                await _firebaseClient.Child("RecipeIngredients").Child("RecipeIngredient_" + ri.Id).PutAsync(ri);
            }
        }
    }
}
