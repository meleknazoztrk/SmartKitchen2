using SQLite;
using SmartKitchen.Models;
using Newtonsoft.Json;

namespace SmartKitchen.Data;

public partial class DatabaseService
{
    private SQLiteAsyncConnection _database;
    private readonly string _dbPath;

    public DatabaseService()
    {
        _dbPath = Path.Combine(FileSystem.AppDataDirectory, "smartkitchen_v5.db3");
    }

    public async Task Init()
    {
        if (_database is not null)
            return;

        _database = new SQLiteAsyncConnection(_dbPath);

#if DEBUG
        await _database.DropTableAsync<User>();
        await _database.DropTableAsync<RecipeIngredient>();
        await _database.DropTableAsync<Recipe>();
        await _database.DropTableAsync<Ingredient>();
#endif

        await _database.CreateTableAsync<Ingredient>();
        await _database.CreateTableAsync<Recipe>();
        await _database.CreateTableAsync<RecipeIngredient>();
        await _database.CreateTableAsync<User>();

        await SeedDataAsync();
    }

    public async Task<List<Ingredient>> GetIngredientsAsync()
    {
        await Init();
        return await _database.Table<Ingredient>().Where(i => !i.IsDeleted).ToListAsync();
    }

    public async Task<int> SaveIngredientAsync(Ingredient item)
    {
        await Init();
        if (item.Id != 0)
            return await _database.UpdateAsync(item);
        else
            return await _database.InsertAsync(item);
    }

    public async Task<List<Recipe>> GetRecipesAsync()
    {
        await Init();
        return await _database.Table<Recipe>().ToListAsync();
    }

    public async Task<List<RecipeIngredient>> GetRecipeIngredientsAsync()
    {
        await Init();
        return await _database.Table<RecipeIngredient>().ToListAsync();
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        await Init();
        return await _database.Table<User>()
            .Where(u => u.Username == username)
            .FirstOrDefaultAsync();
    }

    public async Task<int> SaveUserAsync(User user)
    {
        await Init();
        return await _database.InsertAsync(user);
    }

    public async Task<List<Recipe>> GetRecipesByUserIdAsync(string userId)
    {
        await Init();
        return await _database.Table<Recipe>()
            .Where(r => r.UserId == userId)
            .ToListAsync();
    }

    private async Task SeedDataAsync()
    {
        var count = await _database.Table<Ingredient>().CountAsync();
        if (count > 0)
            return;

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

        await _database.InsertAllAsync(ingredients);
        var insertedIngredients = await _database.Table<Ingredient>().ToListAsync();

        int GetId(string name) {
            var ing = insertedIngredients.FirstOrDefault(i => i.Name == name);
            if (ing != null) return ing.Id;
            return 1; // Fallback to first item to avoid crashing
        }

        var recipes = new List<Recipe>
        {
            new Recipe
            {
                Title = "Klasik Menemen",
                Description = "Kahvaltıların vazgeçilmezi, domates ve yumurtanın harika uyumu.",
                PrepTimeMinutes = 15,
                Calories = 250,
                ProteinGrams = 12,
                IsHighProtein = false,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=100",
                Instructions = "[\"Zeytinyağını tavaya alın, doğranmış soğan ve biberleri hafifçe soteleyin.\", \"Küp küp doğranmış domatesleri ekleyip yumuşayana kadar pişirin.\", \"Üzerine yumurtaları kırın, tuz ve karabiber ekleyerek pişirin.\"]"
            },
            new Recipe
            {
                Title = "Tavuk Sote",
                Description = "Protein deposu leziz ve pratik tavuk sote.",
                PrepTimeMinutes = 25,
                Calories = 350,
                ProteinGrams = 35,
                IsHighProtein = true,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=101",
                Instructions = "[\"Tavuk göğsünü küp küp doğrayın.\", \"Tavada zeytinyağı ile tavukları suyunu salıp çekene kadar soteleyin.\", \"Soğan, biber ve sarımsağı ekleyip kavurmaya devam edin.\", \"Domates ve salçayı ekleyin, kısık ateşte pişmeye bırakın.\"]"
            },
            new Recipe
            {
                Title = "Kaşarlı Omlet",
                Description = "Pratik, doyurucu ve bol kaşarlı bir omlet.",
                PrepTimeMinutes = 10,
                Calories = 300,
                ProteinGrams = 18,
                IsHighProtein = false,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=102",
                Instructions = "[\"Yumurtaları bir kasede tuz ile çırpın.\", \"Tavada tereyağını eritin.\", \"Çırpılmış yumurtayı tavaya dökün.\", \"Altı piştiğinde yarısına rendelenmiş kaşar peynirini ekleyip ikiye katlayın.\"]"
            },
            new Recipe
            {
                Title = "Kremalı Mantarlı Makarna",
                Description = "İtalyan usulü lezzetli kremalı mantarlı makarna.",
                PrepTimeMinutes = 20,
                Calories = 450,
                ProteinGrams = 14,
                IsHighProtein = false,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=103",
                Instructions = "[\"Makarnayı tuzlu suda haşlayın.\", \"Mantarları doğrayıp tereyağında soteleyin, sarımsağı ilave edin.\", \"Süt kreması ekleyip hafifçe koyulaşana kadar pişirin.\", \"Haşlanmış makarna ile sosu birleştirin.\"]"
            },
            new Recipe
            {
                Title = "Fırın Patates",
                Description = "Dışı çıtır, içi yumuşacık baharatlı fırın patates.",
                PrepTimeMinutes = 40,
                Calories = 200,
                ProteinGrams = 4,
                IsHighProtein = false,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=104",
                Instructions = "[\"Patatesleri elma dilimi şeklinde doğrayın.\", \"Zeytinyağı, tuz, kekik ve pul biberle harmanlayın.\", \"200 derece fırında üzeri kızarana kadar yaklaşık 35 dakika pişirin.\"]"
            },
            new Recipe
            {
                Title = "Tavuklu Ispanak Salatası",
                Description = "Hafif ve besleyici sağlıklı salata.",
                PrepTimeMinutes = 15,
                Calories = 220,
                ProteinGrams = 25,
                IsHighProtein = true,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=105",
                Instructions = "[\"Tavuk göğsünü ızgarada pişirip dilimleyin.\", \"Ispanakları yıkayıp iri iri doğrayın.\", \"Ispanak, domates ve tavuk dilimlerini bir kasede birleştirin.\", \"Limon, zeytinyağı ve tuz ile soslayıp servis yapın.\"]"
            },
            new Recipe
            {
                Title = "Fit Krep",
                Description = "Şekersiz, muzla tatlandırılmış sağlıklı krep.",
                PrepTimeMinutes = 15,
                Calories = 180,
                ProteinGrams = 6,
                IsHighProtein = false,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=106",
                Instructions = "[\"Muzu bir kasede çatalla iyice ezin.\", \"Yumurta, süt ve yulaf (veya un) ekleyip pürüzsüz bir hamur elde edene kadar çırpın.\", \"Isınmış yapışmaz tavaya az miktarda döküp arkalı önlü pişirin.\"]"
            },
            new Recipe
            {
                Title = "Fırında Soslu Tavuk Baget",
                Description = "Akşam yemeklerinin vazgeçilmez davet yemeği.",
                PrepTimeMinutes = 45,
                Calories = 400,
                ProteinGrams = 30,
                IsHighProtein = true,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=107",
                Instructions = "[\"Salça, sıvı yağ, yoğurt, sarımsak, kekik ve tuzu karıştırarak bir sos hazırlayın.\", \"Tavuk bagetleri ve doğranmış patatesleri bu sosa bulayın.\", \"Fırın tepsisine dizip 200 derecede nar gibi kızarana kadar pişirin.\"]"
            },
            new Recipe
            {
                Title = "Anne Köftesi",
                Description = "Yılların eskitemediği klasik anne köftesi lezzeti.",
                PrepTimeMinutes = 30,
                Calories = 320,
                ProteinGrams = 22,
                IsHighProtein = true,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=108",
                Instructions = "[\"Kıyma, rendelenmiş soğan, ıslatılmış ufalanmış ekmek, yumurta, ince kıyım maydanoz, kimyon, karabiber ve tuzu derin bir kaba alın.\", \"Malzemeleri iyice yoğurarak özdeşleştirin.\", \"Ceviz büyüklüğünde parçalar koparıp yassı şekil verin.\", \"Az yağlı tavada arkalı önlü kızartın.\"]"
            },
            new Recipe
            {
                Title = "Zeytinyağlı Yaprak Sarması",
                Description = "Zeytinyağlıların şahı, incecik sarılmış kalem gibi sarmalar.",
                PrepTimeMinutes = 90,
                Calories = 250,
                ProteinGrams = 3,
                IsHighProtein = false,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=109",
                Instructions = "[\"Soğanları zeytinyağında kavurun, üzerine yıkanmış pirinci ekleyin.\", \"Kavrulduktan sonra nane, karabiber, tuz ve biraz su ekleyip iç harcı demlendirin.\", \"Asma yapraklarının içine harçtan koyup incecik sarın.\", \"Tencereye dizin, üzerine limon dilimleri ve zeytinyağı gezdirip kısık ateşte pişirin.\"]"
            },
            new Recipe
            {
                Title = "Islak Kek",
                Description = "Bol soslu, yumuşacık ve çikolataya doyuran ıslak kek.",
                PrepTimeMinutes = 45,
                Calories = 450,
                ProteinGrams = 6,
                IsHighProtein = false,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=110",
                Instructions = "[\"Yumurta ve şekeri köpürene kadar çırpın.\", \"Süt, sıvı yağ ve kakaoyu ekleyip çırpmaya devam edin. Bu karışımdan bir bardak ayırın.\", \"Kalan harca un, kabartma tozu ve vanilya ekleyip karıştırın, fırın kabına dökün ve 180 derecede pişirin.\", \"Kek fırından çıkınca ilk sıcağı geçtikten sonra ayırdığınız sosu üzerine dökün.\"]"
            },
            new Recipe
            {
                Title = "Ağlayan Pasta",
                Description = "Üzerindeki çikolata sosuyla ağlayan, içi yumuşacık pasta.",
                PrepTimeMinutes = 60,
                Calories = 500,
                ProteinGrams = 7,
                IsHighProtein = false,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=111",
                Instructions = "[\"Kek harcı için yumurta ve şekeri çırpıp, sıvı malzemeleri ve ardından un, kakao, kabartma tozunu ekleyip fırında pişirin.\", \"Kek çıkıp soğuyunca üzerine süt dökerek ıslatın.\", \"Üzerine çırpılmış süt kreması veya krem şanti sürün.\", \"En üste hazırladığınız çikolata sosunu gezdirin ve dolapta dinlendirin.\"]"
            },
            new Recipe
            {
                Title = "Kısır",
                Description = "Çay saatlerinin ve günlerin vazgeçilmez bol ekşili salatası.",
                PrepTimeMinutes = 25,
                Calories = 210,
                ProteinGrams = 5,
                IsHighProtein = false,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=112",
                Instructions = "[\"İnce bulguru sıcak su ile ıslatıp şişmesini bekleyin.\", \"Ayrı bir tavada sıvı yağ ile salçayı kokusu çıkana kadar kavurun.\", \"Salçalı sosu bulgura ekleyip iyice yedirin.\", \"İnce kıyılmış taze soğan, maydanoz, marul, nar ekşisi, limon, tuz ve baharatları ekleyip harmanlayın.\"]"
            },
            new Recipe
            {
                Title = "Mercimek Köftesi",
                Description = "Hem doyurucu hem lezzetli, çay yanına çok yakışan meze.",
                PrepTimeMinutes = 40,
                Calories = 240,
                ProteinGrams = 9,
                IsHighProtein = false,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=113",
                Instructions = "[\"Kırmızı mercimeği haşlayın, suyunu tam çekmeden içine ince bulguru ekleyip kapağını kapatarak demlenmeye bırakın.\", \"Soğanı sıvı yağda kavurun, salça ve baharatları ekleyip sosu hazırlayın.\", \"Salçalı sosu mercimekli bulgura dökün ve iyice yoğurun.\", \"Son olarak ince kıyılmış maydanoz ekleyip elinizle şekil verin.\"]"
            },
            new Recipe
            {
                Title = "Karnıyarık",
                Description = "Patlıcan ve kıymanın en efsanevi buluşması.",
                PrepTimeMinutes = 60,
                Calories = 380,
                ProteinGrams = 15,
                IsHighProtein = false,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=114",
                Instructions = "[\"Patlıcanları alacalı soyup kızartın ve ortalarını yarın.\", \"Kıymayı soğan, sarımsak, biber ve domatesle kavurup iç harcını hazırlayın, baharatlarını ekleyin.\", \"Kızarmış patlıcanların içine kıymalı harcı doldurun.\", \"Üzerine salçalı su gezdirip fırında veya tencerede pişirin.\"]"
            },
            new Recipe
            {
                Title = "İmambayıldı",
                Description = "Zeytinyağlı soğuk yenebilen hafif patlıcan yemeği.",
                PrepTimeMinutes = 50,
                Calories = 220,
                ProteinGrams = 3,
                IsHighProtein = false,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=115",
                Instructions = "[\"Patlıcanları alacalı soyup zeytinyağında hafifçe kızartın.\", \"Piyazlık doğranmış bol soğan, sarımsak, domates ve biberi zeytinyağında soteleyip iç harç hazırlayın.\", \"Patlıcanların içine harcı doldurun.\", \"Zeytinyağı ve az su ilavesiyle kısık ateşte pişirin.\"]"
            },
            new Recipe
            {
                Title = "Süzme Mercimek Çorbası",
                Description = "Lokanta usulü, pürüzsüz ve sıcacık mercimek çorbası.",
                PrepTimeMinutes = 35,
                Calories = 180,
                ProteinGrams = 8,
                IsHighProtein = false,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=116",
                Instructions = "[\"Soğan, patates ve havucu iri iri doğrayıp az sıvı yağda kavurun.\", \"Yıkanmış mercimekleri ve sıcak suyu ekleyip sebzeler yumuşayana kadar kaynatın.\", \"Blenderdan geçirip pürüzsüz hale getirin.\", \"Üzerine tereyağında kızdırılmış nane ve pul biber dökerek servis yapın.\"]"
            },
            new Recipe
            {
                Title = "Yayla Çorbası",
                Description = "Mideyi rahatlatan, ferah nane kokulu yoğurt çorbası.",
                PrepTimeMinutes = 30,
                Calories = 160,
                ProteinGrams = 6,
                IsHighProtein = false,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=117",
                Instructions = "[\"Pirinci yıkayıp yumuşayana kadar haşlayın.\", \"Ayrı bir kasede yoğurt, un ve yumurta sarısını çırparak terbiyesini hazırlayın.\", \"Çorbanın suyundan terbiyeye ekleyip ılıtın, ardından terbiyeyi yavaşça tencereye karıştırarak ekleyin.\", \"Kaynadıktan sonra altını kapatın. Üzerine tereyağında nane kızdırıp dökün.\"]"
            },
            new Recipe
            {
                Title = "Domates Çorbası",
                Description = "Kaşar peyniriyle bütünleşen, kıvamlı nefis domates çorbası.",
                PrepTimeMinutes = 25,
                Calories = 190,
                ProteinGrams = 5,
                IsHighProtein = false,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=118",
                Instructions = "[\"Tereyağında unu kokusu çıkana kadar kavurun.\", \"Rendelenmiş veya püre haline getirilmiş domatesleri ekleyip kavurmaya devam edin.\", \"Üzerine su (veya et suyu) ekleyip kaynatın. Altını kapatmaya yakın süt ekleyin.\", \"Üzerine rendelenmiş kaşar peyniri serperek servis yapın.\"]"
            },
            new Recipe
            {
                Title = "Etli Taco",
                Description = "Meksika'nın meşhur çıtır çıtır etli tacosu.",
                PrepTimeMinutes = 20,
                Calories = 350,
                ProteinGrams = 18,
                IsHighProtein = true,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=119",
                Instructions = "[\"Kıymayı tavada kavurun, içine soğan, sarımsak, tuz ve pul biber ekleyin.\", \"Tortillaları hafifçe ısıtın.\", \"İçine kıymalı harcı, küp doğranmış domatesi ve jalapeno biberini ekleyin.\"]"
            },
            new Recipe
            {
                Title = "Tavuklu Quesadilla",
                Description = "Erimiş peynir ve baharatlı tavuğun tortilla ile dansı.",
                PrepTimeMinutes = 25,
                Calories = 400,
                ProteinGrams = 25,
                IsHighProtein = true,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=120",
                Instructions = "[\"Tavuk göğsünü küp küp doğrayın ve biberlerle soteleyin.\", \"Tavaya bir tortilla yayın, üzerine tavuklu harcı ve çedar peynirini dökün.\", \"Üzerini başka bir tortilla ile kapatıp iki yüzünü de peynir eriyene kadar kızartın.\"]"
            },
            new Recipe
            {
                Title = "Guacamole Sos",
                Description = "Avokadonun en lezzetli, en ferah hali.",
                PrepTimeMinutes = 10,
                Calories = 150,
                ProteinGrams = 2,
                IsHighProtein = false,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=121",
                Instructions = "[\"Avokadoları soyup bir kasede iyice ezin.\", \"İçine çok ince doğranmış domates, soğan ve sarımsak ekleyin.\", \"Misket limonu suyu, tuz ve zeytinyağı ile tatlandırın.\"]"
            },
            new Recipe
            {
                Title = "Tavuk Fajita",
                Description = "Cızırdayan tavada servis edilen enfes fajita.",
                PrepTimeMinutes = 30,
                Calories = 380,
                ProteinGrams = 30,
                IsHighProtein = true,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=122",
                Instructions = "[\"Tavuk göğsünü julyen (uzun uzun) doğrayın ve soya sosu, zeytinyağı ile marine edin.\", \"Renkli biberleri ve soğanı julyen doğrayın.\", \"Kızgın tavada tavukları, ardından sebzeleri yüksek ateşte soteleyin.\", \"Sıcak tortilla ile servis yapın.\"]"
            },
            new Recipe
            {
                Title = "Nachos",
                Description = "Film gecelerinin en çıtır, bol peynirli atıştırmalığı.",
                PrepTimeMinutes = 15,
                Calories = 450,
                ProteinGrams = 15,
                IsHighProtein = false,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=123",
                Instructions = "[\"Tortillaları üçgen kesip fırınlayarak cips yapın (veya hazır cips kullanın).\", \"Fırın tepsisine dizin, üzerine kavrulmuş kıyma, jalapeno ve bolca çedar peyniri koyun.\", \"Peynirler eriyene kadar fırınlayın.\"]"
            },
            new Recipe
            {
                Title = "Pizza Margherita",
                Description = "İtalya'nın en klasik, ince hamurlu domatesli pizzası.",
                PrepTimeMinutes = 45,
                Calories = 600,
                ProteinGrams = 20,
                IsHighProtein = false,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=124",
                Instructions = "[\"Un, su, tuz ve maya (veya kabartma tozu) ile pizza hamuru yoğurun ve açın.\", \"Üzerine domates salçası/püresi sürün.\", \"Mozzarella peyniri ve taze fesleğen yapraklarını yerleştirin.\", \"200 derece fırında kenarları kızarana kadar pişirin.\"]"
            },
            new Recipe
            {
                Title = "Lasagna (Lazanya)",
                Description = "Kat kat lezzet, bol beşamel soslu lazanya.",
                PrepTimeMinutes = 60,
                Calories = 700,
                ProteinGrams = 25,
                IsHighProtein = true,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=125",
                Instructions = "[\"Kıymayı, domates ve soğanla kavurup bolonez sos yapın.\", \"Un, süt ve tereyağını kavurarak beşamel sos hazırlayın.\", \"Fırın kabına sırasıyla lazanya yaprağı (makarna), beşamel sos, bolonez sos ve parmesan peyniri döşeyin.\", \"En üste peynir serpip fırında pişirin.\"]"
            },
            new Recipe
            {
                Title = "Fettuccine Alfredo",
                Description = "Tavuklu, bol kremalı ve parmesanlı efsane makarna.",
                PrepTimeMinutes = 25,
                Calories = 550,
                ProteinGrams = 22,
                IsHighProtein = true,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=126",
                Instructions = "[\"Fettuccine makarnayı haşlayın.\", \"Tavukları tereyağında soteleyin, sarımsak ekleyin.\", \"Süt kreması ve bol parmesan peynirini ekleyip sosu yoğunlaştırın.\", \"Haşlanan makarna ile sosu harmanlayın.\"]"
            },
            new Recipe
            {
                Title = "Risotto",
                Description = "Kıvamı ve lezzetiyle dillere destan mantarlı risotto.",
                PrepTimeMinutes = 40,
                Calories = 450,
                ProteinGrams = 8,
                IsHighProtein = false,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=127",
                Instructions = "[\"Mantarları ince doğrayıp tereyağında soteleyin.\", \"Pirinçleri ekleyip şeffaflaşana kadar kavurun.\", \"Sıcak suyu azar azar ekleyerek pirinçlerin çekmesini sağlayın, sürekli karıştırın.\", \"Ocaktan almaya yakın parmesan peyniri ve ekstra tereyağı ekleyip bağlayın.\"]"
            },
            new Recipe
            {
                Title = "Bruschetta",
                Description = "Kızarmış ekmek üzerinde İtalyan rüzgarı.",
                PrepTimeMinutes = 15,
                Calories = 180,
                ProteinGrams = 4,
                IsHighProtein = false,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=128",
                Instructions = "[\"Ekmek dilimlerini zeytinyağı ile fırçalayıp kızartın.\", \"Üzerine sarımsak sürtün.\", \"Domatesleri çok küçük küp doğrayıp fesleğen ve zeytinyağı ile karıştırın.\", \"Karışımı kızarmış ekmeklerin üzerine paylaştırın.\"]"
            },
            new Recipe
            {
                Title = "Tiramisu",
                Description = "Kahve aromalı, mascarpone lezzetinde harika tatlı.",
                PrepTimeMinutes = 25,
                Calories = 400,
                ProteinGrams = 6,
                IsHighProtein = false,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=129",
                Instructions = "[\"Bisküvileri (kedi dili) kahveli suya (veya süte) batırıp tepsiye dizin.\", \"Süt kremasını az şeker ve labne (veya beyaz krem peynir) ile çırpın.\", \"Kremayı bisküvilerin üzerine yayın.\", \"Üzerine bolca kakao eleyerek dolapta dinlendirin.\"]"
            },
            new Recipe
            {
                Title = "Klasik Hamburger",
                Description = "Ev yapımı, sulu sulu Amerikan burgeri.",
                PrepTimeMinutes = 25,
                Calories = 650,
                ProteinGrams = 30,
                IsHighProtein = true,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=130",
                Instructions = "[\"Kıymaya sadece tuz ve karabiber ekleyerek kalın köfteler yoğurun.\", \"Izgara veya tavada mühürleyerek pişirin.\", \"Pişmeye yakın üzerine çedar peyniri koyup eritin.\", \"Burger ekmeğine ketçap, hardal sürüp turşu, marul ve köfteyi yerleştirin.\"]"
            },
            new Recipe
            {
                Title = "Mac and Cheese",
                Description = "Bol peynirli, akışkan fırın makarna.",
                PrepTimeMinutes = 30,
                Calories = 500,
                ProteinGrams = 18,
                IsHighProtein = false,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=131",
                Instructions = "[\"Makarnayı haşlayın.\", \"Tencerede tereyağı ve unu kavurun, süt ekleyip kıvamlı bir beşamel sos yapın.\", \"İçine bolca çedar peyniri ve kaşar peyniri ekleyip eritin.\", \"Makarnayla harmanlayıp isterseniz fırınlayın.\"]"
            },
            new Recipe
            {
                Title = "Pancakes (Pankek)",
                Description = "Pofuduk pofuduk, ballı veya şuruplu kahvaltı pankeki.",
                PrepTimeMinutes = 20,
                Calories = 300,
                ProteinGrams = 8,
                IsHighProtein = false,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=132",
                Instructions = "[\"Yumurta, süt ve şekeri çırpın.\", \"Un, kabartma tozu ve vanilyayı ekleyip kek hamuru kıvamına getirin.\", \"Yapışmaz tavaya küçük porsiyonlar halinde döküp, üzeri göz göz olunca çevirin.\", \"Bal veya muz ile servis yapın.\"]"
            },
            new Recipe
            {
                Title = "Brownie",
                Description = "İçi ıslak, üzeri çatlak, yoğun çikolatalı Amerikan keki.",
                PrepTimeMinutes = 40,
                Calories = 450,
                ProteinGrams = 5,
                IsHighProtein = false,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=133",
                Instructions = "[\"Çikolata ve tereyağını benmari usulü eritin.\", \"Yumurta ve şekeri krema haline gelene kadar çırpın.\", \"Erimiş çikolatayı ekleyin, ardından un ve kakaoyu katıp karıştırın.\", \"Fırın kabına alıp 160 derecede içinin nemli kalması için 25 dakika pişirin.\"]"
            },
            new Recipe
            {
                Title = "Teriyaki Tavuk",
                Description = "Tatlı ve tuzlunun harika dengesi.",
                PrepTimeMinutes = 25,
                Calories = 320,
                ProteinGrams = 28,
                IsHighProtein = true,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=134",
                Instructions = "[\"Soya sosu, şeker, sirke ve rendelenmiş zencefili bir kapta karıştırın.\", \"Tavuk göğsünü lokmalık doğrayıp tavada kızartın.\", \"Sosu üzerine ekleyip sos yoğunlaşıp tavukları kaplayana kadar pişirin.\", \"Üzerine susam serperek servis yapın.\"]"
            },
            new Recipe
            {
                Title = "Sebzeli Noodle",
                Description = "Sebzelerle sotelenmiş nefis uzak doğu eriştesi.",
                PrepTimeMinutes = 20,
                Calories = 350,
                ProteinGrams = 10,
                IsHighProtein = false,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=135",
                Instructions = "[\"Noodle'ı kaynar suda haşlayıp süzün.\", \"Havuç, biber ve soğanı ince şeritler (julyen) halinde kesip tavada yüksek ateşte soteleyin.\", \"Haşlanmış noodle ve soya sosunu ekleyin.\", \"Kısık ateşte birbirine yedirerek servis yapın.\"]"
            },
            new Recipe
            {
                Title = "Tatlı Ekşi Soslu Tavuk",
                Description = "Çin restoranlarının vazgeçilmez favorisi.",
                PrepTimeMinutes = 35,
                Calories = 380,
                ProteinGrams = 25,
                IsHighProtein = true,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=136",
                Instructions = "[\"Tavukları una bulayıp kızgın yağda çıtır olana kadar kızartın.\", \"Ayrı bir tavada biber ve soğanları soteleyin.\", \"Soya sosu, şeker, sirke ve ketçabı karıştırıp sosa ekleyin.\", \"Tavukları sosa ekleyip karıştırarak servis yapın.\"]"
            },
            new Recipe
            {
                Title = "Fried Rice (Kızarmış Pirinç)",
                Description = "Artan pilavı değerlendirmenin en lezzetli Asya yöntemi.",
                PrepTimeMinutes = 15,
                Calories = 300,
                ProteinGrams = 8,
                IsHighProtein = false,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=137",
                Instructions = "[\"Tavada yumurtayı çırparak pişirin ve kenara alın.\", \"Havuç ve bezelyeyi sıvı yağda soteleyin.\", \"Önceden pişmiş (tercihen 1 günlük) pirinçleri ekleyip yüksek ateşte kavurun.\", \"Soya sosu ve yumurtayı ekleyip iyice harmanlayın.\"]"
            },
            new Recipe
            {
                Title = "Adana Kebap",
                Description = "Zırhla çekilmiş efsanevi acılı Adana Kebabı.",
                PrepTimeMinutes = 45,
                Calories = 500,
                ProteinGrams = 35,
                IsHighProtein = true,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=138",
                Instructions = "[\"Kıymaya pul biber ve tuz ekleyerek yoğurun.\", \"Şişlere ince uzun şekilde dizin.\", \"Izgarada çevirerek pişirin.\", \"Lavaş ve sumaklı soğanla servis yapın.\"]"
            },
            new Recipe
            {
                Title = "Ali Nazik",
                Description = "Közlenmiş patlıcanlı yoğurt yatağında kıymalı enfes lezzet.",
                PrepTimeMinutes = 40,
                Calories = 320,
                ProteinGrams = 25,
                IsHighProtein = true,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=139",
                Instructions = "[\"Patlıcanları közleyip soyun ve ezin.\", \"Sarımsaklı yoğurt ile karıştırarak tabağa yayın.\", \"Kıymayı baharatlarla kavurun.\", \"Yoğurtlu yatağın üzerine ekleyip tereyağı gezdirin.\"]"
            },
            new Recipe
            {
                Title = "Sütlaç",
                Description = "Fırında üzeri kızarmış Türk mutfağının başyapıtı hafif tatlı.",
                PrepTimeMinutes = 60,
                Calories = 250,
                ProteinGrams = 8,
                IsHighProtein = false,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=140",
                Instructions = "[\"Pirinci suda haşlayıp suyunu çektirin.\", \"Süt ve şekeri ekleyip kaynamaya bırakın.\", \"Suyla açtığınız biraz nişasta veya pirinç unu ekleyip kıvam alınca kaselere bölün.\", \"Fırının üst rafında üzeri kızarana kadar pişirin.\"]"
            },
            new Recipe
            {
                Title = "Mercimek Çorbası (Sarı)",
                Description = "Sapsarı rengi ve naneli yağıyla iç ısıtan mercimek çorbası.",
                PrepTimeMinutes = 30,
                Calories = 150,
                ProteinGrams = 7,
                IsHighProtein = false,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=141",
                Instructions = "[\"Mercimeği iyice yıkayın.\", \"Soğan ve patatesle tencereye koyup kaynatın.\", \"Blenderdan geçirin.\", \"Tereyağında nane kızdırarak çorbaya ekleyin.\"]"
            },
            new Recipe
            {
                Title = "Ton Balıklı Salata",
                Description = "Pratik, doyurucu ve bol proteinli diyet dostu salata.",
                PrepTimeMinutes = 10,
                Calories = 200,
                ProteinGrams = 22,
                IsHighProtein = true,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=142",
                Instructions = "[\"Marulları doğrayıp kaseye alın.\", \"Domates ve mısır ekleyin.\", \"Üzerine yağını süzdüğünüz ton balığını (veya haşlanmış tavuk) ekleyin.\", \"Limon, tuz ve zeytinyağı ile tatlandırın.\"]"
            },
            new Recipe
            {
                Title = "Patates Kızartması",
                Description = "7'den 70'e herkesin favori çıtır atıştırmalığı.",
                PrepTimeMinutes = 20,
                Calories = 300,
                ProteinGrams = 3,
                IsHighProtein = false,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=143",
                Instructions = "[\"Patatesleri parmak parmak doğrayın.\", \"Kızgın sıvı yağda altın sarısı olana kadar kızartın.\", \"Havlu kağıt üzerine alıp tuzlayın.\", \"Ketçap veya mayonez ile servis yapın.\"]"
            },
            new Recipe
            {
                Title = "Sosisli Sandviç",
                Description = "Pratik ve hızlı ev yapımı Amerikan Hot Dog.",
                PrepTimeMinutes = 15,
                Calories = 400,
                ProteinGrams = 12,
                IsHighProtein = false,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=144",
                Instructions = "[\"Sosisleri suda haşlayın veya ızgarada kızartın.\", \"Sandviç ekmeğini ısıtın.\", \"İçine sosisleri yerleştirin.\", \"Hardal ve ketçap sıkarak servis yapın.\"]"
            },
            new Recipe
            {
                Title = "Karışık Pizza",
                Description = "Evinizdeki her malzemeyle zenginleşebilecek ev pizzası.",
                PrepTimeMinutes = 45,
                Calories = 550,
                ProteinGrams = 20,
                IsHighProtein = false,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=145",
                Instructions = "[\"Un, su, tuz ile pizza hamuru yoğurun ve incecik açın.\", \"Üzerine domates sosu sürün ve bol kaşar peyniri serpin.\", \"Sucuk, sosis, zeytin, biber gibi malzemeleri dizin.\", \"Fırında peynirler eriyip altı kızarana kadar pişirin.\"]"
            },
            new Recipe
            {
                Title = "İçli Köfte",
                Description = "İncecik bulgur kabuğu ve nefis cevizli iç harcı.",
                PrepTimeMinutes = 90,
                Calories = 300,
                ProteinGrams = 12,
                IsHighProtein = false,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=146",
                Instructions = "[\"Kıyma, soğan, ceviz ve baharatlarla iç harcını hazırlayıp soğutun.\", \"İnce bulgur ve salça ile dış hamuru yoğurun.\", \"Hamurdan parçalar koparıp içini oyun, harcı koyup kapatın.\", \"İster suda haşlayın ister bol yağda kızartın.\"]"
            },
            new Recipe
            {
                Title = "Humus",
                Description = "Nohut, tahin ve limonun Ortadoğu mucizesi.",
                PrepTimeMinutes = 15,
                Calories = 200,
                ProteinGrams = 6,
                IsHighProtein = false,
                ImageUrl = "https://loremflickr.com/500/500/food,dish?lock=147",
                Instructions = "[\"Haşlanmış nohudu blenderdan geçirin.\", \"İçine tahin (veya susam), sarımsak, limon suyu, tuz ve zeytinyağı ekleyip kremamsı bir kıvama getirin.\", \"Tabağa yayıp üzerine kimyon ve zeytinyağı gezdirin.\"]"
            }
        };

        await _database.InsertAllAsync(recipes);
        var insertedRecipes = await _database.Table<Recipe>().ToListAsync();
        
        int GetRecipeId(string title) {
            var rec = insertedRecipes.FirstOrDefault(r => r.Title == title);
            if (rec != null) return rec.Id;
            return 1;
        }

        var recipeIngredients = new List<RecipeIngredient>
        {
            new RecipeIngredient { RecipeId = GetRecipeId("Klasik Menemen"), IngredientId = GetId("Domates"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Klasik Menemen"), IngredientId = GetId("Yumurta"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Klasik Menemen"), IngredientId = GetId("Soğan"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Klasik Menemen"), IngredientId = GetId("Biber"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Klasik Menemen"), IngredientId = GetId("Zeytinyağı"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Klasik Menemen"), IngredientId = GetId("Tuz"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Klasik Menemen"), IngredientId = GetId("Karabiber"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tavuk Sote"), IngredientId = GetId("Tavuk Göğsü"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tavuk Sote"), IngredientId = GetId("Soğan"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tavuk Sote"), IngredientId = GetId("Sarımsak"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tavuk Sote"), IngredientId = GetId("Biber"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tavuk Sote"), IngredientId = GetId("Domates"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tavuk Sote"), IngredientId = GetId("Salça"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tavuk Sote"), IngredientId = GetId("Zeytinyağı"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tavuk Sote"), IngredientId = GetId("Tuz"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Kaşarlı Omlet"), IngredientId = GetId("Yumurta"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Kaşarlı Omlet"), IngredientId = GetId("Kaşar Peyniri"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Kaşarlı Omlet"), IngredientId = GetId("Tereyağı"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Kaşarlı Omlet"), IngredientId = GetId("Tuz"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Kremalı Mantarlı Makarna"), IngredientId = GetId("Makarna"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Kremalı Mantarlı Makarna"), IngredientId = GetId("Mantar"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Kremalı Mantarlı Makarna"), IngredientId = GetId("Süt Kreması"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Kremalı Mantarlı Makarna"), IngredientId = GetId("Sarımsak"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Kremalı Mantarlı Makarna"), IngredientId = GetId("Tereyağı"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Kremalı Mantarlı Makarna"), IngredientId = GetId("Tuz"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Fırın Patates"), IngredientId = GetId("Patates"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Fırın Patates"), IngredientId = GetId("Zeytinyağı"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Fırın Patates"), IngredientId = GetId("Tuz"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Fırın Patates"), IngredientId = GetId("Kekik"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Fırın Patates"), IngredientId = GetId("Pul Biber"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tavuklu Ispanak Salatası"), IngredientId = GetId("Tavuk Göğsü"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tavuklu Ispanak Salatası"), IngredientId = GetId("Ispanak"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tavuklu Ispanak Salatası"), IngredientId = GetId("Domates"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tavuklu Ispanak Salatası"), IngredientId = GetId("Limon"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tavuklu Ispanak Salatası"), IngredientId = GetId("Zeytinyağı"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tavuklu Ispanak Salatası"), IngredientId = GetId("Tuz"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Fit Krep"), IngredientId = GetId("Muz"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Fit Krep"), IngredientId = GetId("Yumurta"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Fit Krep"), IngredientId = GetId("Süt"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Fit Krep"), IngredientId = GetId("Yulaf"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Fırında Soslu Tavuk Baget"), IngredientId = GetId("Tavuk Baget"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Fırında Soslu Tavuk Baget"), IngredientId = GetId("Patates"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Fırında Soslu Tavuk Baget"), IngredientId = GetId("Salça"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Fırında Soslu Tavuk Baget"), IngredientId = GetId("Sıvı Yağ"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Fırında Soslu Tavuk Baget"), IngredientId = GetId("Yoğurt"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Fırında Soslu Tavuk Baget"), IngredientId = GetId("Sarımsak"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Fırında Soslu Tavuk Baget"), IngredientId = GetId("Kekik"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Fırında Soslu Tavuk Baget"), IngredientId = GetId("Tuz"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Anne Köftesi"), IngredientId = GetId("Kıyma"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Anne Köftesi"), IngredientId = GetId("Soğan"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Anne Köftesi"), IngredientId = GetId("Ekmek"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Anne Köftesi"), IngredientId = GetId("Yumurta"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Anne Köftesi"), IngredientId = GetId("Maydanoz"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Anne Köftesi"), IngredientId = GetId("Kimyon"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Anne Köftesi"), IngredientId = GetId("Karabiber"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Anne Köftesi"), IngredientId = GetId("Tuz"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Anne Köftesi"), IngredientId = GetId("Sıvı Yağ"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Zeytinyağlı Yaprak Sarması"), IngredientId = GetId("Asma Yaprağı"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Zeytinyağlı Yaprak Sarması"), IngredientId = GetId("Pirinç"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Zeytinyağlı Yaprak Sarması"), IngredientId = GetId("Soğan"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Zeytinyağlı Yaprak Sarması"), IngredientId = GetId("Zeytinyağı"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Zeytinyağlı Yaprak Sarması"), IngredientId = GetId("Nane"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Zeytinyağlı Yaprak Sarması"), IngredientId = GetId("Karabiber"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Zeytinyağlı Yaprak Sarması"), IngredientId = GetId("Tuz"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Zeytinyağlı Yaprak Sarması"), IngredientId = GetId("Limon"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Islak Kek"), IngredientId = GetId("Yumurta"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Islak Kek"), IngredientId = GetId("Şeker"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Islak Kek"), IngredientId = GetId("Süt"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Islak Kek"), IngredientId = GetId("Sıvı Yağ"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Islak Kek"), IngredientId = GetId("Kakao"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Islak Kek"), IngredientId = GetId("Un"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Islak Kek"), IngredientId = GetId("Kabartma Tozu"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Islak Kek"), IngredientId = GetId("Vanilya"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Ağlayan Pasta"), IngredientId = GetId("Yumurta"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Ağlayan Pasta"), IngredientId = GetId("Şeker"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Ağlayan Pasta"), IngredientId = GetId("Un"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Ağlayan Pasta"), IngredientId = GetId("Kakao"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Ağlayan Pasta"), IngredientId = GetId("Kabartma Tozu"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Ağlayan Pasta"), IngredientId = GetId("Süt Kreması"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Ağlayan Pasta"), IngredientId = GetId("Çikolata"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Ağlayan Pasta"), IngredientId = GetId("Süt"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Kısır"), IngredientId = GetId("Bulgur"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Kısır"), IngredientId = GetId("Salça"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Kısır"), IngredientId = GetId("Sıvı Yağ"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Kısır"), IngredientId = GetId("Soğan"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Kısır"), IngredientId = GetId("Maydanoz"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Kısır"), IngredientId = GetId("Marul"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Kısır"), IngredientId = GetId("Nar Ekşisi"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Kısır"), IngredientId = GetId("Limon"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Kısır"), IngredientId = GetId("Tuz"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Kısır"), IngredientId = GetId("Pul Biber"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Mercimek Köftesi"), IngredientId = GetId("Mercimek"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Mercimek Köftesi"), IngredientId = GetId("Bulgur"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Mercimek Köftesi"), IngredientId = GetId("Soğan"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Mercimek Köftesi"), IngredientId = GetId("Sıvı Yağ"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Mercimek Köftesi"), IngredientId = GetId("Salça"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Mercimek Köftesi"), IngredientId = GetId("Maydanoz"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Mercimek Köftesi"), IngredientId = GetId("Kimyon"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Mercimek Köftesi"), IngredientId = GetId("Pul Biber"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Mercimek Köftesi"), IngredientId = GetId("Tuz"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Karnıyarık"), IngredientId = GetId("Patlıcan"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Karnıyarık"), IngredientId = GetId("Kıyma"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Karnıyarık"), IngredientId = GetId("Soğan"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Karnıyarık"), IngredientId = GetId("Sarımsak"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Karnıyarık"), IngredientId = GetId("Biber"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Karnıyarık"), IngredientId = GetId("Domates"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Karnıyarık"), IngredientId = GetId("Salça"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Karnıyarık"), IngredientId = GetId("Sıvı Yağ"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Karnıyarık"), IngredientId = GetId("Tuz"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Karnıyarık"), IngredientId = GetId("Karabiber"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("İmambayıldı"), IngredientId = GetId("Patlıcan"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("İmambayıldı"), IngredientId = GetId("Soğan"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("İmambayıldı"), IngredientId = GetId("Sarımsak"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("İmambayıldı"), IngredientId = GetId("Domates"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("İmambayıldı"), IngredientId = GetId("Biber"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("İmambayıldı"), IngredientId = GetId("Zeytinyağı"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("İmambayıldı"), IngredientId = GetId("Tuz"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("İmambayıldı"), IngredientId = GetId("Şeker"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Süzme Mercimek Çorbası"), IngredientId = GetId("Mercimek"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Süzme Mercimek Çorbası"), IngredientId = GetId("Soğan"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Süzme Mercimek Çorbası"), IngredientId = GetId("Patates"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Süzme Mercimek Çorbası"), IngredientId = GetId("Havuç"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Süzme Mercimek Çorbası"), IngredientId = GetId("Sıvı Yağ"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Süzme Mercimek Çorbası"), IngredientId = GetId("Tereyağı"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Süzme Mercimek Çorbası"), IngredientId = GetId("Nane"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Süzme Mercimek Çorbası"), IngredientId = GetId("Pul Biber"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Süzme Mercimek Çorbası"), IngredientId = GetId("Tuz"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Yayla Çorbası"), IngredientId = GetId("Yoğurt"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Yayla Çorbası"), IngredientId = GetId("Pirinç"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Yayla Çorbası"), IngredientId = GetId("Un"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Yayla Çorbası"), IngredientId = GetId("Yumurta"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Yayla Çorbası"), IngredientId = GetId("Tereyağı"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Yayla Çorbası"), IngredientId = GetId("Nane"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Yayla Çorbası"), IngredientId = GetId("Tuz"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Domates Çorbası"), IngredientId = GetId("Domates"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Domates Çorbası"), IngredientId = GetId("Un"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Domates Çorbası"), IngredientId = GetId("Tereyağı"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Domates Çorbası"), IngredientId = GetId("Süt"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Domates Çorbası"), IngredientId = GetId("Kaşar Peyniri"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Domates Çorbası"), IngredientId = GetId("Tuz"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Domates Çorbası"), IngredientId = GetId("Karabiber"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Etli Taco"), IngredientId = GetId("Tortilla"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Etli Taco"), IngredientId = GetId("Kıyma"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Etli Taco"), IngredientId = GetId("Domates"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Etli Taco"), IngredientId = GetId("Soğan"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Etli Taco"), IngredientId = GetId("Sarımsak"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Etli Taco"), IngredientId = GetId("Jalapeno"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Etli Taco"), IngredientId = GetId("Tuz"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Etli Taco"), IngredientId = GetId("Pul Biber"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tavuklu Quesadilla"), IngredientId = GetId("Tortilla"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tavuklu Quesadilla"), IngredientId = GetId("Tavuk Göğsü"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tavuklu Quesadilla"), IngredientId = GetId("Çedar Peyniri"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tavuklu Quesadilla"), IngredientId = GetId("Biber"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tavuklu Quesadilla"), IngredientId = GetId("Sıvı Yağ"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Guacamole Sos"), IngredientId = GetId("Avokado"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Guacamole Sos"), IngredientId = GetId("Domates"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Guacamole Sos"), IngredientId = GetId("Soğan"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Guacamole Sos"), IngredientId = GetId("Sarımsak"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Guacamole Sos"), IngredientId = GetId("Misket Limonu"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Guacamole Sos"), IngredientId = GetId("Tuz"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Guacamole Sos"), IngredientId = GetId("Zeytinyağı"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tavuk Fajita"), IngredientId = GetId("Tavuk Göğsü"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tavuk Fajita"), IngredientId = GetId("Biber"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tavuk Fajita"), IngredientId = GetId("Soğan"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tavuk Fajita"), IngredientId = GetId("Soya Sosu"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tavuk Fajita"), IngredientId = GetId("Zeytinyağı"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tavuk Fajita"), IngredientId = GetId("Tortilla"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Nachos"), IngredientId = GetId("Tortilla"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Nachos"), IngredientId = GetId("Kıyma"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Nachos"), IngredientId = GetId("Çedar Peyniri"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Nachos"), IngredientId = GetId("Jalapeno"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Pizza Margherita"), IngredientId = GetId("Un"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Pizza Margherita"), IngredientId = GetId("Domates"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Pizza Margherita"), IngredientId = GetId("Mozzarella"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Pizza Margherita"), IngredientId = GetId("Fesleğen"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Pizza Margherita"), IngredientId = GetId("Tuz"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Pizza Margherita"), IngredientId = GetId("Zeytinyağı"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Lasagna (Lazanya)"), IngredientId = GetId("Makarna"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Lasagna (Lazanya)"), IngredientId = GetId("Kıyma"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Lasagna (Lazanya)"), IngredientId = GetId("Süt"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Lasagna (Lazanya)"), IngredientId = GetId("Un"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Lasagna (Lazanya)"), IngredientId = GetId("Tereyağı"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Lasagna (Lazanya)"), IngredientId = GetId("Domates"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Lasagna (Lazanya)"), IngredientId = GetId("Parmesan"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Lasagna (Lazanya)"), IngredientId = GetId("Soğan"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Fettuccine Alfredo"), IngredientId = GetId("Makarna"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Fettuccine Alfredo"), IngredientId = GetId("Süt Kreması"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Fettuccine Alfredo"), IngredientId = GetId("Tavuk Göğsü"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Fettuccine Alfredo"), IngredientId = GetId("Parmesan"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Fettuccine Alfredo"), IngredientId = GetId("Tereyağı"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Fettuccine Alfredo"), IngredientId = GetId("Sarımsak"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Risotto"), IngredientId = GetId("Pirinç"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Risotto"), IngredientId = GetId("Mantar"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Risotto"), IngredientId = GetId("Parmesan"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Risotto"), IngredientId = GetId("Tereyağı"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Risotto"), IngredientId = GetId("Tuz"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Bruschetta"), IngredientId = GetId("Ekmek"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Bruschetta"), IngredientId = GetId("Domates"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Bruschetta"), IngredientId = GetId("Fesleğen"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Bruschetta"), IngredientId = GetId("Sarımsak"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Bruschetta"), IngredientId = GetId("Zeytinyağı"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tiramisu"), IngredientId = GetId("Bisküvi"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tiramisu"), IngredientId = GetId("Süt Kreması"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tiramisu"), IngredientId = GetId("Kakao"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tiramisu"), IngredientId = GetId("Şeker"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tiramisu"), IngredientId = GetId("Süt"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Klasik Hamburger"), IngredientId = GetId("Burger Ekmeği"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Klasik Hamburger"), IngredientId = GetId("Kıyma"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Klasik Hamburger"), IngredientId = GetId("Çedar Peyniri"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Klasik Hamburger"), IngredientId = GetId("Turşu"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Klasik Hamburger"), IngredientId = GetId("Marul"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Klasik Hamburger"), IngredientId = GetId("Ketçap"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Klasik Hamburger"), IngredientId = GetId("Hardal"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Klasik Hamburger"), IngredientId = GetId("Tuz"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Klasik Hamburger"), IngredientId = GetId("Karabiber"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Mac and Cheese"), IngredientId = GetId("Makarna"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Mac and Cheese"), IngredientId = GetId("Çedar Peyniri"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Mac and Cheese"), IngredientId = GetId("Kaşar Peyniri"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Mac and Cheese"), IngredientId = GetId("Süt"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Mac and Cheese"), IngredientId = GetId("Tereyağı"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Mac and Cheese"), IngredientId = GetId("Un"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Mac and Cheese"), IngredientId = GetId("Tuz"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Pancakes (Pankek)"), IngredientId = GetId("Un"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Pancakes (Pankek)"), IngredientId = GetId("Süt"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Pancakes (Pankek)"), IngredientId = GetId("Yumurta"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Pancakes (Pankek)"), IngredientId = GetId("Şeker"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Pancakes (Pankek)"), IngredientId = GetId("Kabartma Tozu"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Pancakes (Pankek)"), IngredientId = GetId("Vanilya"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Pancakes (Pankek)"), IngredientId = GetId("Bal"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Pancakes (Pankek)"), IngredientId = GetId("Muz"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Brownie"), IngredientId = GetId("Çikolata"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Brownie"), IngredientId = GetId("Tereyağı"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Brownie"), IngredientId = GetId("Yumurta"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Brownie"), IngredientId = GetId("Şeker"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Brownie"), IngredientId = GetId("Un"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Brownie"), IngredientId = GetId("Kakao"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Teriyaki Tavuk"), IngredientId = GetId("Tavuk Göğsü"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Teriyaki Tavuk"), IngredientId = GetId("Soya Sosu"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Teriyaki Tavuk"), IngredientId = GetId("Şeker"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Teriyaki Tavuk"), IngredientId = GetId("Sirke"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Teriyaki Tavuk"), IngredientId = GetId("Zencefil"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Teriyaki Tavuk"), IngredientId = GetId("Susam"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Sebzeli Noodle"), IngredientId = GetId("Noodle"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Sebzeli Noodle"), IngredientId = GetId("Havuç"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Sebzeli Noodle"), IngredientId = GetId("Biber"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Sebzeli Noodle"), IngredientId = GetId("Soğan"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Sebzeli Noodle"), IngredientId = GetId("Soya Sosu"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Sebzeli Noodle"), IngredientId = GetId("Sıvı Yağ"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tatlı Ekşi Soslu Tavuk"), IngredientId = GetId("Tavuk Göğsü"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tatlı Ekşi Soslu Tavuk"), IngredientId = GetId("Un"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tatlı Ekşi Soslu Tavuk"), IngredientId = GetId("Biber"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tatlı Ekşi Soslu Tavuk"), IngredientId = GetId("Soğan"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tatlı Ekşi Soslu Tavuk"), IngredientId = GetId("Soya Sosu"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tatlı Ekşi Soslu Tavuk"), IngredientId = GetId("Şeker"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tatlı Ekşi Soslu Tavuk"), IngredientId = GetId("Sirke"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Tatlı Ekşi Soslu Tavuk"), IngredientId = GetId("Ketçap"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Fried Rice (Kızarmış Pirinç)"), IngredientId = GetId("Pirinç"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Fried Rice (Kızarmış Pirinç)"), IngredientId = GetId("Yumurta"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Fried Rice (Kızarmış Pirinç)"), IngredientId = GetId("Havuç"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Fried Rice (Kızarmış Pirinç)"), IngredientId = GetId("Bezelye"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Fried Rice (Kızarmış Pirinç)"), IngredientId = GetId("Soya Sosu"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Fried Rice (Kızarmış Pirinç)"), IngredientId = GetId("Sıvı Yağ"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Adana Kebap"), IngredientId = GetId("Kıyma"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Adana Kebap"), IngredientId = GetId("Pul Biber"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Adana Kebap"), IngredientId = GetId("Tuz"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Adana Kebap"), IngredientId = GetId("Lavaş"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Ali Nazik"), IngredientId = GetId("Patlıcan"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Ali Nazik"), IngredientId = GetId("Yoğurt"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Ali Nazik"), IngredientId = GetId("Sarımsak"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Ali Nazik"), IngredientId = GetId("Kıyma"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Ali Nazik"), IngredientId = GetId("Tereyağı"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Ali Nazik"), IngredientId = GetId("Pul Biber"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Sütlaç"), IngredientId = GetId("Süt"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Sütlaç"), IngredientId = GetId("Pirinç"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Sütlaç"), IngredientId = GetId("Şeker"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Sütlaç"), IngredientId = GetId("Un"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Mercimek Çorbası (Sarı)"), IngredientId = GetId("Mercimek"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Mercimek Çorbası (Sarı)"), IngredientId = GetId("Soğan"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Mercimek Çorbası (Sarı)"), IngredientId = GetId("Patates"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Mercimek Çorbası (Sarı)"), IngredientId = GetId("Tereyağı"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Mercimek Çorbası (Sarı)"), IngredientId = GetId("Nane"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Mercimek Çorbası (Sarı)"), IngredientId = GetId("Tuz"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Ton Balıklı Salata"), IngredientId = GetId("Marul"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Ton Balıklı Salata"), IngredientId = GetId("Domates"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Ton Balıklı Salata"), IngredientId = GetId("Limon"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Ton Balıklı Salata"), IngredientId = GetId("Zeytinyağı"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Ton Balıklı Salata"), IngredientId = GetId("Tuz"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Patates Kızartması"), IngredientId = GetId("Patates"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Patates Kızartması"), IngredientId = GetId("Sıvı Yağ"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Patates Kızartması"), IngredientId = GetId("Tuz"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Patates Kızartması"), IngredientId = GetId("Ketçap"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Sosisli Sandviç"), IngredientId = GetId("Sosis"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Sosisli Sandviç"), IngredientId = GetId("Ekmek"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Sosisli Sandviç"), IngredientId = GetId("Hardal"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Sosisli Sandviç"), IngredientId = GetId("Ketçap"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Karışık Pizza"), IngredientId = GetId("Un"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Karışık Pizza"), IngredientId = GetId("Domates"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Karışık Pizza"), IngredientId = GetId("Kaşar Peyniri"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Karışık Pizza"), IngredientId = GetId("Sucuk"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Karışık Pizza"), IngredientId = GetId("Siyah Zeytin"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Karışık Pizza"), IngredientId = GetId("Biber"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("İçli Köfte"), IngredientId = GetId("Bulgur"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("İçli Köfte"), IngredientId = GetId("Kıyma"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("İçli Köfte"), IngredientId = GetId("Soğan"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("İçli Köfte"), IngredientId = GetId("Ceviz"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("İçli Köfte"), IngredientId = GetId("Salça"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("İçli Köfte"), IngredientId = GetId("Sıvı Yağ"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Humus"), IngredientId = GetId("Nohut"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Humus"), IngredientId = GetId("Limon"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Humus"), IngredientId = GetId("Zeytinyağı"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Humus"), IngredientId = GetId("Sarımsak"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Humus"), IngredientId = GetId("Tuz"), Quantity = "Gerektiği kadar" },
            new RecipeIngredient { RecipeId = GetRecipeId("Humus"), IngredientId = GetId("Kimyon"), Quantity = "Gerektiği kadar" }
        };

        await _database.InsertAllAsync(recipeIngredients);
    }
}
