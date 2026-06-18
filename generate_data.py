import json

ingredients = [
    ("Domates", "Sebze", "🍅"), ("Yumurta", "Kahvaltılık", "🥚"), ("Kaşar Peyniri", "Süt Ürünleri", "🧀"),
    ("Tavuk Göğsü", "Et", "🍗"), ("Soğan", "Sebze", "🧅"), ("Sarımsak", "Sebze", "🧄"),
    ("Patates", "Sebze", "🥔"), ("Havuç", "Sebze", "🥕"), ("Süt", "Süt Ürünleri", "🥛"),
    ("Tereyağı", "Süt Ürünleri", "🧈"), ("Kıyma", "Et", "🥩"), ("Ekmek", "Fırın", "🍞"),
    ("Pirinç", "Bakliyat", "🍚"), ("Makarna", "Bakliyat", "🍝"), ("Ispanak", "Sebze", "🥬"),
    ("Mantar", "Sebze", "🍄"), ("Biber", "Sebze", "🫑"), ("Zeytinyağı", "Yağ", "🫒"),
    ("Tuz", "Baharat", "🧂"), ("Karabiber", "Baharat", "🧂"), ("Un", "Fırın", "🌾"),
    ("Şeker", "Tatlandırıcı", "🧊"), ("Limon", "Meyve", "🍋"), ("Elma", "Meyve", "🍎"),
    ("Muz", "Meyve", "🍌"), ("Yoğurt", "Süt Ürünleri", "🥣"), ("Salça", "Sos", "🥫"),
    ("Maydanoz", "Sebze", "🌿"), ("Nane", "Baharat", "🌿"), ("Kekik", "Baharat", "🌿"),
    ("Pul Biber", "Baharat", "🌶️"), ("Kimyon", "Baharat", "🧂"), ("Mercimek", "Bakliyat", "🧆"),
    ("Nohut", "Bakliyat", "🧆"), ("Fasulye", "Bakliyat", "🫘"), ("Bulgur", "Bakliyat", "🌾"),
    ("Sıvı Yağ", "Yağ", "🌻"), ("Sirke", "Sos", "🏺"), ("Tavuk Baget", "Et", "🍗"),
    ("Kuşbaşı Et", "Et", "🥩"), ("Sucuk", "Şarküteri", "🥓"), ("Sosis", "Şarküteri", "🌭"),
    ("Lavaş", "Fırın", "🌯"),
    ("Sucuk", "Şarküteri", "🥓"), ("Sosis", "Şarküteri", "🌭"), ("Bezelye", "Sebze", "🟢"),
    ("Patlıcan", "Sebze", "🍆"), ("Kabak", "Sebze", "🥒"), ("Salatalık", "Sebze", "🥒"),
    ("Marul", "Sebze", "🥬"), ("Lahana", "Sebze", "🥬"), ("Nar Ekşisi", "Sos", "🏺"),
    ("Tarçın", "Baharat", "🪵"), ("Kakao", "Tatlandırıcı", "🍫"), ("Çikolata", "Tatlı", "🍫"),
    ("Kabartma Tozu", "Fırın", "🧁"), ("Vanilya", "Fırın", "🌼"), ("Süt Kreması", "Süt Ürünleri", "🥛"),
    ("Hindistan Cevizi", "Meyve", "🥥"), ("İrmik", "Fırın", "🌾"), ("Yulaf", "Tahıl", "🥣"),
    ("Bisküvi", "Tatlı", "🍪"), ("Çilek", "Meyve", "🍓"), ("Vişne", "Meyve", "🍒"),
    ("Nar", "Meyve", "🍎"), ("Beyaz Peynir", "Süt Ürünleri", "🧀"), ("Yeşil Zeytin", "Kahvaltılık", "🫒"),
    ("Siyah Zeytin", "Kahvaltılık", "🫒"), ("Bal", "Tatlı", "🍯"), ("Pekmez", "Tatlı", "🍯"),
    ("Tel Şehriye", "Bakliyat", "🍝"), ("Asma Yaprağı", "Sebze", "🍃"), ("Yufka", "Fırın", "🌯"),
    ("Tortilla", "Fırın", "🌮"), ("Çedar Peyniri", "Süt Ürünleri", "🧀"), ("Jalapeno", "Sebze", "🌶️"),
    ("Avokado", "Meyve", "🥑"), ("Misket Limonu", "Meyve", "🍋"), ("Soya Sosu", "Sos", "🏺"),
    ("Zencefil", "Sebze", "🫚"), ("Susam", "Baharat", "🌾"), ("Noodle", "Bakliyat", "🍜"),
    ("Mozzarella", "Süt Ürünleri", "🧀"), ("Fesleğen", "Sebze", "🌿"), ("Parmesan", "Süt Ürünleri", "🧀"),
    ("Burger Ekmeği", "Fırın", "🍔"), ("Turşu", "Sebze", "🥒"), ("Hardal", "Sos", "🏺"),
    ("Ketçap", "Sos", "🍅"), ("Sıcak Su", "Diğer", "💧"), ("Kuşüzümü", "Kuruyemiş", "🍇"),
    ("Fıstık", "Kuruyemiş", "🥜"), ("Sarımsak Tozu", "Baharat", "🧄"), ("Krepa", "Fırın", "🥞")
]

recipes = [
    # TÜRKIYE
    {
        "Title": "Klasik Menemen", "Desc": "Kahvaltıların vazgeçilmezi, domates ve yumurtanın harika uyumu.",
        "Time": 15, "Cal": 250, "Prot": 12, "HiP": "false",
        "Ins": ["Zeytinyağını tavaya alın, doğranmış soğan ve biberleri hafifçe soteleyin.", "Küp küp doğranmış domatesleri ekleyip yumuşayana kadar pişirin.", "Üzerine yumurtaları kırın, tuz ve karabiber ekleyerek pişirin."],
        "Ing": ["Domates", "Yumurta", "Soğan", "Biber", "Zeytinyağı", "Tuz", "Karabiber"]
    },
    {
        "Title": "Tavuk Sote", "Desc": "Protein deposu leziz ve pratik tavuk sote.",
        "Time": 25, "Cal": 350, "Prot": 35, "HiP": "true",
        "Ins": ["Tavuk göğsünü küp küp doğrayın.", "Tavada zeytinyağı ile tavukları suyunu salıp çekene kadar soteleyin.", "Soğan, biber ve sarımsağı ekleyip kavurmaya devam edin.", "Domates ve salçayı ekleyin, kısık ateşte pişmeye bırakın."],
        "Ing": ["Tavuk Göğsü", "Soğan", "Sarımsak", "Biber", "Domates", "Salça", "Zeytinyağı", "Tuz"]
    },
    {
        "Title": "Kaşarlı Omlet", "Desc": "Pratik, doyurucu ve bol kaşarlı bir omlet.",
        "Time": 10, "Cal": 300, "Prot": 18, "HiP": "false",
        "Ins": ["Yumurtaları bir kasede tuz ile çırpın.", "Tavada tereyağını eritin.", "Çırpılmış yumurtayı tavaya dökün.", "Altı piştiğinde yarısına rendelenmiş kaşar peynirini ekleyip ikiye katlayın."],
        "Ing": ["Yumurta", "Kaşar Peyniri", "Tereyağı", "Tuz"]
    },
    {
        "Title": "Kremalı Mantarlı Makarna", "Desc": "İtalyan usulü lezzetli kremalı mantarlı makarna.",
        "Time": 20, "Cal": 450, "Prot": 14, "HiP": "false",
        "Ins": ["Makarnayı tuzlu suda haşlayın.", "Mantarları doğrayıp tereyağında soteleyin, sarımsağı ilave edin.", "Süt kreması ekleyip hafifçe koyulaşana kadar pişirin.", "Haşlanmış makarna ile sosu birleştirin."],
        "Ing": ["Makarna", "Mantar", "Süt Kreması", "Sarımsak", "Tereyağı", "Tuz"]
    },
    {
        "Title": "Fırın Patates", "Desc": "Dışı çıtır, içi yumuşacık baharatlı fırın patates.",
        "Time": 40, "Cal": 200, "Prot": 4, "HiP": "false",
        "Ins": ["Patatesleri elma dilimi şeklinde doğrayın.", "Zeytinyağı, tuz, kekik ve pul biberle harmanlayın.", "200 derece fırında üzeri kızarana kadar yaklaşık 35 dakika pişirin."],
        "Ing": ["Patates", "Zeytinyağı", "Tuz", "Kekik", "Pul Biber"]
    },
    {
        "Title": "Tavuklu Ispanak Salatası", "Desc": "Hafif ve besleyici sağlıklı salata.",
        "Time": 15, "Cal": 220, "Prot": 25, "HiP": "true",
        "Ins": ["Tavuk göğsünü ızgarada pişirip dilimleyin.", "Ispanakları yıkayıp iri iri doğrayın.", "Ispanak, domates ve tavuk dilimlerini bir kasede birleştirin.", "Limon, zeytinyağı ve tuz ile soslayıp servis yapın."],
        "Ing": ["Tavuk Göğsü", "Ispanak", "Domates", "Limon", "Zeytinyağı", "Tuz"]
    },
    {
        "Title": "Fit Krep", "Desc": "Şekersiz, muzla tatlandırılmış sağlıklı krep.",
        "Time": 15, "Cal": 180, "Prot": 6, "HiP": "false",
        "Ins": ["Muzu bir kasede çatalla iyice ezin.", "Yumurta, süt ve yulaf (veya un) ekleyip pürüzsüz bir hamur elde edene kadar çırpın.", "Isınmış yapışmaz tavaya az miktarda döküp arkalı önlü pişirin."],
        "Ing": ["Muz", "Yumurta", "Süt", "Yulaf"]
    },
    {
        "Title": "Fırında Soslu Tavuk Baget", "Desc": "Akşam yemeklerinin vazgeçilmez davet yemeği.",
        "Time": 45, "Cal": 400, "Prot": 30, "HiP": "true",
        "Ins": ["Salça, sıvı yağ, yoğurt, sarımsak, kekik ve tuzu karıştırarak bir sos hazırlayın.", "Tavuk bagetleri ve doğranmış patatesleri bu sosa bulayın.", "Fırın tepsisine dizip 200 derecede nar gibi kızarana kadar pişirin."],
        "Ing": ["Tavuk Baget", "Patates", "Salça", "Sıvı Yağ", "Yoğurt", "Sarımsak", "Kekik", "Tuz"]
    },
    {
        "Title": "Anne Köftesi", "Desc": "Yılların eskitemediği klasik anne köftesi lezzeti.",
        "Time": 30, "Cal": 320, "Prot": 22, "HiP": "true",
        "Ins": ["Kıyma, rendelenmiş soğan, ıslatılmış ufalanmış ekmek, yumurta, ince kıyım maydanoz, kimyon, karabiber ve tuzu derin bir kaba alın.", "Malzemeleri iyice yoğurarak özdeşleştirin.", "Ceviz büyüklüğünde parçalar koparıp yassı şekil verin.", "Az yağlı tavada arkalı önlü kızartın."],
        "Ing": ["Kıyma", "Soğan", "Ekmek", "Yumurta", "Maydanoz", "Kimyon", "Karabiber", "Tuz", "Sıvı Yağ"]
    },
    {
        "Title": "Zeytinyağlı Yaprak Sarması", "Desc": "Zeytinyağlıların şahı, incecik sarılmış kalem gibi sarmalar.",
        "Time": 90, "Cal": 250, "Prot": 3, "HiP": "false",
        "Ins": ["Soğanları zeytinyağında kavurun, üzerine yıkanmış pirinci ekleyin.", "Kavrulduktan sonra nane, karabiber, tuz ve biraz su ekleyip iç harcı demlendirin.", "Asma yapraklarının içine harçtan koyup incecik sarın.", "Tencereye dizin, üzerine limon dilimleri ve zeytinyağı gezdirip kısık ateşte pişirin."],
        "Ing": ["Asma Yaprağı", "Pirinç", "Soğan", "Zeytinyağı", "Nane", "Karabiber", "Tuz", "Limon"]
    },
    {
        "Title": "Islak Kek", "Desc": "Bol soslu, yumuşacık ve çikolataya doyuran ıslak kek.",
        "Time": 45, "Cal": 450, "Prot": 6, "HiP": "false",
        "Ins": ["Yumurta ve şekeri köpürene kadar çırpın.", "Süt, sıvı yağ ve kakaoyu ekleyip çırpmaya devam edin. Bu karışımdan bir bardak ayırın.", "Kalan harca un, kabartma tozu ve vanilya ekleyip karıştırın, fırın kabına dökün ve 180 derecede pişirin.", "Kek fırından çıkınca ilk sıcağı geçtikten sonra ayırdığınız sosu üzerine dökün."],
        "Ing": ["Yumurta", "Şeker", "Süt", "Sıvı Yağ", "Kakao", "Un", "Kabartma Tozu", "Vanilya"]
    },
    {
        "Title": "Ağlayan Pasta", "Desc": "Üzerindeki çikolata sosuyla ağlayan, içi yumuşacık pasta.",
        "Time": 60, "Cal": 500, "Prot": 7, "HiP": "false",
        "Ins": ["Kek harcı için yumurta ve şekeri çırpıp, sıvı malzemeleri ve ardından un, kakao, kabartma tozunu ekleyip fırında pişirin.", "Kek çıkıp soğuyunca üzerine süt dökerek ıslatın.", "Üzerine çırpılmış süt kreması veya krem şanti sürün.", "En üste hazırladığınız çikolata sosunu gezdirin ve dolapta dinlendirin."],
        "Ing": ["Yumurta", "Şeker", "Un", "Kakao", "Kabartma Tozu", "Süt Kreması", "Çikolata", "Süt"]
    },
    {
        "Title": "Kısır", "Desc": "Çay saatlerinin ve günlerin vazgeçilmez bol ekşili salatası.",
        "Time": 25, "Cal": 210, "Prot": 5, "HiP": "false",
        "Ins": ["İnce bulguru sıcak su ile ıslatıp şişmesini bekleyin.", "Ayrı bir tavada sıvı yağ ile salçayı kokusu çıkana kadar kavurun.", "Salçalı sosu bulgura ekleyip iyice yedirin.", "İnce kıyılmış taze soğan, maydanoz, marul, nar ekşisi, limon, tuz ve baharatları ekleyip harmanlayın."],
        "Ing": ["Bulgur", "Salça", "Sıvı Yağ", "Soğan", "Maydanoz", "Marul", "Nar Ekşisi", "Limon", "Tuz", "Pul Biber"]
    },
    {
        "Title": "Mercimek Köftesi", "Desc": "Hem doyurucu hem lezzetli, çay yanına çok yakışan meze.",
        "Time": 40, "Cal": 240, "Prot": 9, "HiP": "false",
        "Ins": ["Kırmızı mercimeği haşlayın, suyunu tam çekmeden içine ince bulguru ekleyip kapağını kapatarak demlenmeye bırakın.", "Soğanı sıvı yağda kavurun, salça ve baharatları ekleyip sosu hazırlayın.", "Salçalı sosu mercimekli bulgura dökün ve iyice yoğurun.", "Son olarak ince kıyılmış maydanoz ekleyip elinizle şekil verin."],
        "Ing": ["Mercimek", "Bulgur", "Soğan", "Sıvı Yağ", "Salça", "Maydanoz", "Kimyon", "Pul Biber", "Tuz"]
    },
    {
        "Title": "Karnıyarık", "Desc": "Patlıcan ve kıymanın en efsanevi buluşması.",
        "Time": 60, "Cal": 380, "Prot": 15, "HiP": "false",
        "Ins": ["Patlıcanları alacalı soyup kızartın ve ortalarını yarın.", "Kıymayı soğan, sarımsak, biber ve domatesle kavurup iç harcını hazırlayın, baharatlarını ekleyin.", "Kızarmış patlıcanların içine kıymalı harcı doldurun.", "Üzerine salçalı su gezdirip fırında veya tencerede pişirin."],
        "Ing": ["Patlıcan", "Kıyma", "Soğan", "Sarımsak", "Biber", "Domates", "Salça", "Sıvı Yağ", "Tuz", "Karabiber"]
    },
    {
        "Title": "İmambayıldı", "Desc": "Zeytinyağlı soğuk yenebilen hafif patlıcan yemeği.",
        "Time": 50, "Cal": 220, "Prot": 3, "HiP": "false",
        "Ins": ["Patlıcanları alacalı soyup zeytinyağında hafifçe kızartın.", "Piyazlık doğranmış bol soğan, sarımsak, domates ve biberi zeytinyağında soteleyip iç harç hazırlayın.", "Patlıcanların içine harcı doldurun.", "Zeytinyağı ve az su ilavesiyle kısık ateşte pişirin."],
        "Ing": ["Patlıcan", "Soğan", "Sarımsak", "Domates", "Biber", "Zeytinyağı", "Tuz", "Şeker"]
    },
    {
        "Title": "Süzme Mercimek Çorbası", "Desc": "Lokanta usulü, pürüzsüz ve sıcacık mercimek çorbası.",
        "Time": 35, "Cal": 180, "Prot": 8, "HiP": "false",
        "Ins": ["Soğan, patates ve havucu iri iri doğrayıp az sıvı yağda kavurun.", "Yıkanmış mercimekleri ve sıcak suyu ekleyip sebzeler yumuşayana kadar kaynatın.", "Blenderdan geçirip pürüzsüz hale getirin.", "Üzerine tereyağında kızdırılmış nane ve pul biber dökerek servis yapın."],
        "Ing": ["Mercimek", "Soğan", "Patates", "Havuç", "Sıvı Yağ", "Tereyağı", "Nane", "Pul Biber", "Tuz"]
    },
    {
        "Title": "Yayla Çorbası", "Desc": "Mideyi rahatlatan, ferah nane kokulu yoğurt çorbası.",
        "Time": 30, "Cal": 160, "Prot": 6, "HiP": "false",
        "Ins": ["Pirinci yıkayıp yumuşayana kadar haşlayın.", "Ayrı bir kasede yoğurt, un ve yumurta sarısını çırparak terbiyesini hazırlayın.", "Çorbanın suyundan terbiyeye ekleyip ılıtın, ardından terbiyeyi yavaşça tencereye karıştırarak ekleyin.", "Kaynadıktan sonra altını kapatın. Üzerine tereyağında nane kızdırıp dökün."],
        "Ing": ["Yoğurt", "Pirinç", "Un", "Yumurta", "Tereyağı", "Nane", "Tuz"]
    },
    {
        "Title": "Domates Çorbası", "Desc": "Kaşar peyniriyle bütünleşen, kıvamlı nefis domates çorbası.",
        "Time": 25, "Cal": 190, "Prot": 5, "HiP": "false",
        "Ins": ["Tereyağında unu kokusu çıkana kadar kavurun.", "Rendelenmiş veya püre haline getirilmiş domatesleri ekleyip kavurmaya devam edin.", "Üzerine su (veya et suyu) ekleyip kaynatın. Altını kapatmaya yakın süt ekleyin.", "Üzerine rendelenmiş kaşar peyniri serperek servis yapın."],
        "Ing": ["Domates", "Un", "Tereyağı", "Süt", "Kaşar Peyniri", "Tuz", "Karabiber"]
    },

    # MEKSIKA (MEXICO)
    {
        "Title": "Etli Taco", "Desc": "Meksika'nın meşhur çıtır çıtır etli tacosu.",
        "Time": 20, "Cal": 350, "Prot": 18, "HiP": "true",
        "Ins": ["Kıymayı tavada kavurun, içine soğan, sarımsak, tuz ve pul biber ekleyin.", "Tortillaları hafifçe ısıtın.", "İçine kıymalı harcı, küp doğranmış domatesi ve jalapeno biberini ekleyin."],
        "Ing": ["Tortilla", "Kıyma", "Domates", "Soğan", "Sarımsak", "Jalapeno", "Tuz", "Pul Biber"]
    },
    {
        "Title": "Tavuklu Quesadilla", "Desc": "Erimiş peynir ve baharatlı tavuğun tortilla ile dansı.",
        "Time": 25, "Cal": 400, "Prot": 25, "HiP": "true",
        "Ins": ["Tavuk göğsünü küp küp doğrayın ve biberlerle soteleyin.", "Tavaya bir tortilla yayın, üzerine tavuklu harcı ve çedar peynirini dökün.", "Üzerini başka bir tortilla ile kapatıp iki yüzünü de peynir eriyene kadar kızartın."],
        "Ing": ["Tortilla", "Tavuk Göğsü", "Çedar Peyniri", "Biber", "Sıvı Yağ"]
    },
    {
        "Title": "Guacamole Sos", "Desc": "Avokadonun en lezzetli, en ferah hali.",
        "Time": 10, "Cal": 150, "Prot": 2, "HiP": "false",
        "Ins": ["Avokadoları soyup bir kasede iyice ezin.", "İçine çok ince doğranmış domates, soğan ve sarımsak ekleyin.", "Misket limonu suyu, tuz ve zeytinyağı ile tatlandırın."],
        "Ing": ["Avokado", "Domates", "Soğan", "Sarımsak", "Misket Limonu", "Tuz", "Zeytinyağı"]
    },
    {
        "Title": "Tavuk Fajita", "Desc": "Cızırdayan tavada servis edilen enfes fajita.",
        "Time": 30, "Cal": 380, "Prot": 30, "HiP": "true",
        "Ins": ["Tavuk göğsünü julyen (uzun uzun) doğrayın ve soya sosu, zeytinyağı ile marine edin.", "Renkli biberleri ve soğanı julyen doğrayın.", "Kızgın tavada tavukları, ardından sebzeleri yüksek ateşte soteleyin.", "Sıcak tortilla ile servis yapın."],
        "Ing": ["Tavuk Göğsü", "Biber", "Soğan", "Soya Sosu", "Zeytinyağı", "Tortilla"]
    },
    {
        "Title": "Nachos", "Desc": "Film gecelerinin en çıtır, bol peynirli atıştırmalığı.",
        "Time": 15, "Cal": 450, "Prot": 15, "HiP": "false",
        "Ins": ["Tortillaları üçgen kesip fırınlayarak cips yapın (veya hazır cips kullanın).", "Fırın tepsisine dizin, üzerine kavrulmuş kıyma, jalapeno ve bolca çedar peyniri koyun.", "Peynirler eriyene kadar fırınlayın."],
        "Ing": ["Tortilla", "Kıyma", "Çedar Peyniri", "Jalapeno"]
    },
    
    # ITALYA (ITALY)
    {
        "Title": "Pizza Margherita", "Desc": "İtalya'nın en klasik, ince hamurlu domatesli pizzası.",
        "Time": 45, "Cal": 600, "Prot": 20, "HiP": "false",
        "Ins": ["Un, su, tuz ve maya (veya kabartma tozu) ile pizza hamuru yoğurun ve açın.", "Üzerine domates salçası/püresi sürün.", "Mozzarella peyniri ve taze fesleğen yapraklarını yerleştirin.", "200 derece fırında kenarları kızarana kadar pişirin."],
        "Ing": ["Un", "Domates", "Mozzarella", "Fesleğen", "Tuz", "Zeytinyağı"]
    },
    {
        "Title": "Lasagna (Lazanya)", "Desc": "Kat kat lezzet, bol beşamel soslu lazanya.",
        "Time": 60, "Cal": 700, "Prot": 25, "HiP": "true",
        "Ins": ["Kıymayı, domates ve soğanla kavurup bolonez sos yapın.", "Un, süt ve tereyağını kavurarak beşamel sos hazırlayın.", "Fırın kabına sırasıyla lazanya yaprağı (makarna), beşamel sos, bolonez sos ve parmesan peyniri döşeyin.", "En üste peynir serpip fırında pişirin."],
        "Ing": ["Makarna", "Kıyma", "Süt", "Un", "Tereyağı", "Domates", "Parmesan", "Soğan"]
    },
    {
        "Title": "Fettuccine Alfredo", "Desc": "Tavuklu, bol kremalı ve parmesanlı efsane makarna.",
        "Time": 25, "Cal": 550, "Prot": 22, "HiP": "true",
        "Ins": ["Fettuccine makarnayı haşlayın.", "Tavukları tereyağında soteleyin, sarımsak ekleyin.", "Süt kreması ve bol parmesan peynirini ekleyip sosu yoğunlaştırın.", "Haşlanan makarna ile sosu harmanlayın."],
        "Ing": ["Makarna", "Süt Kreması", "Tavuk Göğsü", "Parmesan", "Tereyağı", "Sarımsak"]
    },
    {
        "Title": "Risotto", "Desc": "Kıvamı ve lezzetiyle dillere destan mantarlı risotto.",
        "Time": 40, "Cal": 450, "Prot": 8, "HiP": "false",
        "Ins": ["Mantarları ince doğrayıp tereyağında soteleyin.", "Pirinçleri ekleyip şeffaflaşana kadar kavurun.", "Sıcak suyu azar azar ekleyerek pirinçlerin çekmesini sağlayın, sürekli karıştırın.", "Ocaktan almaya yakın parmesan peyniri ve ekstra tereyağı ekleyip bağlayın."],
        "Ing": ["Pirinç", "Mantar", "Parmesan", "Tereyağı", "Tuz"]
    },
    {
        "Title": "Bruschetta", "Desc": "Kızarmış ekmek üzerinde İtalyan rüzgarı.",
        "Time": 15, "Cal": 180, "Prot": 4, "HiP": "false",
        "Ins": ["Ekmek dilimlerini zeytinyağı ile fırçalayıp kızartın.", "Üzerine sarımsak sürtün.", "Domatesleri çok küçük küp doğrayıp fesleğen ve zeytinyağı ile karıştırın.", "Karışımı kızarmış ekmeklerin üzerine paylaştırın."],
        "Ing": ["Ekmek", "Domates", "Fesleğen", "Sarımsak", "Zeytinyağı"]
    },
    {
        "Title": "Tiramisu", "Desc": "Kahve aromalı, mascarpone lezzetinde harika tatlı.",
        "Time": 25, "Cal": 400, "Prot": 6, "HiP": "false",
        "Ins": ["Bisküvileri (kedi dili) kahveli suya (veya süte) batırıp tepsiye dizin.", "Süt kremasını az şeker ve labne (veya beyaz krem peynir) ile çırpın.", "Kremayı bisküvilerin üzerine yayın.", "Üzerine bolca kakao eleyerek dolapta dinlendirin."],
        "Ing": ["Bisküvi", "Süt Kreması", "Kakao", "Şeker", "Süt"]
    },

    # AMERİKA (USA)
    {
        "Title": "Klasik Hamburger", "Desc": "Ev yapımı, sulu sulu Amerikan burgeri.",
        "Time": 25, "Cal": 650, "Prot": 30, "HiP": "true",
        "Ins": ["Kıymaya sadece tuz ve karabiber ekleyerek kalın köfteler yoğurun.", "Izgara veya tavada mühürleyerek pişirin.", "Pişmeye yakın üzerine çedar peyniri koyup eritin.", "Burger ekmeğine ketçap, hardal sürüp turşu, marul ve köfteyi yerleştirin."],
        "Ing": ["Burger Ekmeği", "Kıyma", "Çedar Peyniri", "Turşu", "Marul", "Ketçap", "Hardal", "Tuz", "Karabiber"]
    },
    {
        "Title": "Mac and Cheese", "Desc": "Bol peynirli, akışkan fırın makarna.",
        "Time": 30, "Cal": 500, "Prot": 18, "HiP": "false",
        "Ins": ["Makarnayı haşlayın.", "Tencerede tereyağı ve unu kavurun, süt ekleyip kıvamlı bir beşamel sos yapın.", "İçine bolca çedar peyniri ve kaşar peyniri ekleyip eritin.", "Makarnayla harmanlayıp isterseniz fırınlayın."],
        "Ing": ["Makarna", "Çedar Peyniri", "Kaşar Peyniri", "Süt", "Tereyağı", "Un", "Tuz"]
    },
    {
        "Title": "Pancakes (Pankek)", "Desc": "Pofuduk pofuduk, ballı veya şuruplu kahvaltı pankeki.",
        "Time": 20, "Cal": 300, "Prot": 8, "HiP": "false",
        "Ins": ["Yumurta, süt ve şekeri çırpın.", "Un, kabartma tozu ve vanilyayı ekleyip kek hamuru kıvamına getirin.", "Yapışmaz tavaya küçük porsiyonlar halinde döküp, üzeri göz göz olunca çevirin.", "Bal veya muz ile servis yapın."],
        "Ing": ["Un", "Süt", "Yumurta", "Şeker", "Kabartma Tozu", "Vanilya", "Bal", "Muz"]
    },
    {
        "Title": "Brownie", "Desc": "İçi ıslak, üzeri çatlak, yoğun çikolatalı Amerikan keki.",
        "Time": 40, "Cal": 450, "Prot": 5, "HiP": "false",
        "Ins": ["Çikolata ve tereyağını benmari usulü eritin.", "Yumurta ve şekeri krema haline gelene kadar çırpın.", "Erimiş çikolatayı ekleyin, ardından un ve kakaoyu katıp karıştırın.", "Fırın kabına alıp 160 derecede içinin nemli kalması için 25 dakika pişirin."],
        "Ing": ["Çikolata", "Tereyağı", "Yumurta", "Şeker", "Un", "Kakao"]
    },

    # UZAK DOĞU & ASYA (ASIA)
    {
        "Title": "Teriyaki Tavuk", "Desc": "Tatlı ve tuzlunun harika dengesi.",
        "Time": 25, "Cal": 320, "Prot": 28, "HiP": "true",
        "Ins": ["Soya sosu, şeker, sirke ve rendelenmiş zencefili bir kapta karıştırın.", "Tavuk göğsünü lokmalık doğrayıp tavada kızartın.", "Sosu üzerine ekleyip sos yoğunlaşıp tavukları kaplayana kadar pişirin.", "Üzerine susam serperek servis yapın."],
        "Ing": ["Tavuk Göğsü", "Soya Sosu", "Şeker", "Sirke", "Zencefil", "Susam"]
    },
    {
        "Title": "Sebzeli Noodle", "Desc": "Sebzelerle sotelenmiş nefis uzak doğu eriştesi.",
        "Time": 20, "Cal": 350, "Prot": 10, "HiP": "false",
        "Ins": ["Noodle'ı kaynar suda haşlayıp süzün.", "Havuç, biber ve soğanı ince şeritler (julyen) halinde kesip tavada yüksek ateşte soteleyin.", "Haşlanmış noodle ve soya sosunu ekleyin.", "Kısık ateşte birbirine yedirerek servis yapın."],
        "Ing": ["Noodle", "Havuç", "Biber", "Soğan", "Soya Sosu", "Sıvı Yağ"]
    },
    {
        "Title": "Tatlı Ekşi Soslu Tavuk", "Desc": "Çin restoranlarının vazgeçilmez favorisi.",
        "Time": 35, "Cal": 380, "Prot": 25, "HiP": "true",
        "Ins": ["Tavukları una bulayıp kızgın yağda çıtır olana kadar kızartın.", "Ayrı bir tavada biber ve soğanları soteleyin.", "Soya sosu, şeker, sirke ve ketçabı karıştırıp sosa ekleyin.", "Tavukları sosa ekleyip karıştırarak servis yapın."],
        "Ing": ["Tavuk Göğsü", "Un", "Biber", "Soğan", "Soya Sosu", "Şeker", "Sirke", "Ketçap"]
    },
    {
        "Title": "Fried Rice (Kızarmış Pirinç)", "Desc": "Artan pilavı değerlendirmenin en lezzetli Asya yöntemi.",
        "Time": 15, "Cal": 300, "Prot": 8, "HiP": "false",
        "Ins": ["Tavada yumurtayı çırparak pişirin ve kenara alın.", "Havuç ve bezelyeyi sıvı yağda soteleyin.", "Önceden pişmiş (tercihen 1 günlük) pirinçleri ekleyip yüksek ateşte kavurun.", "Soya sosu ve yumurtayı ekleyip iyice harmanlayın."],
        "Ing": ["Pirinç", "Yumurta", "Havuç", "Bezelye", "Soya Sosu", "Sıvı Yağ"]
    },

    # NEW YÖRESEL/DÜNYA (MORE RECIPES)
    {
        "Title": "Adana Kebap", "Desc": "Zırhla çekilmiş efsanevi acılı Adana Kebabı.",
        "Time": 45, "Cal": 500, "Prot": 35, "HiP": "true",
        "Ins": ["Kıymaya pul biber ve tuz ekleyerek yoğurun.", "Şişlere ince uzun şekilde dizin.", "Izgarada çevirerek pişirin.", "Lavaş ve sumaklı soğanla servis yapın."],
        "Ing": ["Kıyma", "Pul Biber", "Tuz", "Lavaş"]
    },
    {
        "Title": "Ali Nazik", "Desc": "Közlenmiş patlıcanlı yoğurt yatağında kıymalı enfes lezzet.",
        "Time": 40, "Cal": 320, "Prot": 25, "HiP": "true",
        "Ins": ["Patlıcanları közleyip soyun ve ezin.", "Sarımsaklı yoğurt ile karıştırarak tabağa yayın.", "Kıymayı baharatlarla kavurun.", "Yoğurtlu yatağın üzerine ekleyip tereyağı gezdirin."],
        "Ing": ["Patlıcan", "Yoğurt", "Sarımsak", "Kıyma", "Tereyağı", "Pul Biber"]
    },
    {
        "Title": "Sütlaç", "Desc": "Fırında üzeri kızarmış Türk mutfağının başyapıtı hafif tatlı.",
        "Time": 60, "Cal": 250, "Prot": 8, "HiP": "false",
        "Ins": ["Pirinci suda haşlayıp suyunu çektirin.", "Süt ve şekeri ekleyip kaynamaya bırakın.", "Suyla açtığınız biraz nişasta veya pirinç unu ekleyip kıvam alınca kaselere bölün.", "Fırının üst rafında üzeri kızarana kadar pişirin."],
        "Ing": ["Süt", "Pirinç", "Şeker", "Un"]
    },
    {
        "Title": "Mercimek Çorbası (Sarı)", "Desc": "Sapsarı rengi ve naneli yağıyla iç ısıtan mercimek çorbası.",
        "Time": 30, "Cal": 150, "Prot": 7, "HiP": "false",
        "Ins": ["Mercimeği iyice yıkayın.", "Soğan ve patatesle tencereye koyup kaynatın.", "Blenderdan geçirin.", "Tereyağında nane kızdırarak çorbaya ekleyin."],
        "Ing": ["Mercimek", "Soğan", "Patates", "Tereyağı", "Nane", "Tuz"]
    },
    {
        "Title": "Ton Balıklı Salata", "Desc": "Pratik, doyurucu ve bol proteinli diyet dostu salata.",
        "Time": 10, "Cal": 200, "Prot": 22, "HiP": "true",
        "Ins": ["Marulları doğrayıp kaseye alın.", "Domates ve mısır ekleyin.", "Üzerine yağını süzdüğünüz ton balığını (veya haşlanmış tavuk) ekleyin.", "Limon, tuz ve zeytinyağı ile tatlandırın."],
        "Ing": ["Marul", "Domates", "Limon", "Zeytinyağı", "Tuz"]
    },
    {
        "Title": "Patates Kızartması", "Desc": "7'den 70'e herkesin favori çıtır atıştırmalığı.",
        "Time": 20, "Cal": 300, "Prot": 3, "HiP": "false",
        "Ins": ["Patatesleri parmak parmak doğrayın.", "Kızgın sıvı yağda altın sarısı olana kadar kızartın.", "Havlu kağıt üzerine alıp tuzlayın.", "Ketçap veya mayonez ile servis yapın."],
        "Ing": ["Patates", "Sıvı Yağ", "Tuz", "Ketçap"]
    },
    {
        "Title": "Sosisli Sandviç", "Desc": "Pratik ve hızlı ev yapımı Amerikan Hot Dog.",
        "Time": 15, "Cal": 400, "Prot": 12, "HiP": "false",
        "Ins": ["Sosisleri suda haşlayın veya ızgarada kızartın.", "Sandviç ekmeğini ısıtın.", "İçine sosisleri yerleştirin.", "Hardal ve ketçap sıkarak servis yapın."],
        "Ing": ["Sosis", "Ekmek", "Hardal", "Ketçap"]
    },
    {
        "Title": "Karışık Pizza", "Desc": "Evinizdeki her malzemeyle zenginleşebilecek ev pizzası.",
        "Time": 45, "Cal": 550, "Prot": 20, "HiP": "false",
        "Ins": ["Un, su, tuz ile pizza hamuru yoğurun ve incecik açın.", "Üzerine domates sosu sürün ve bol kaşar peyniri serpin.", "Sucuk, sosis, zeytin, biber gibi malzemeleri dizin.", "Fırında peynirler eriyip altı kızarana kadar pişirin."],
        "Ing": ["Un", "Domates", "Kaşar Peyniri", "Sucuk", "Siyah Zeytin", "Biber"]
    },
    {
        "Title": "İçli Köfte", "Desc": "İncecik bulgur kabuğu ve nefis cevizli iç harcı.",
        "Time": 90, "Cal": 300, "Prot": 12, "HiP": "false",
        "Ins": ["Kıyma, soğan, ceviz ve baharatlarla iç harcını hazırlayıp soğutun.", "İnce bulgur ve salça ile dış hamuru yoğurun.", "Hamurdan parçalar koparıp içini oyun, harcı koyup kapatın.", "İster suda haşlayın ister bol yağda kızartın."],
        "Ing": ["Bulgur", "Kıyma", "Soğan", "Ceviz", "Salça", "Sıvı Yağ"]
    },
    {
        "Title": "Humus", "Desc": "Nohut, tahin ve limonun Ortadoğu mucizesi.",
        "Time": 15, "Cal": 200, "Prot": 6, "HiP": "false",
        "Ins": ["Haşlanmış nohudu blenderdan geçirin.", "İçine tahin (veya susam), sarımsak, limon suyu, tuz ve zeytinyağı ekleyip kremamsı bir kıvama getirin.", "Tabağa yayıp üzerine kimyon ve zeytinyağı gezdirin."],
        "Ing": ["Nohut", "Limon", "Zeytinyağı", "Sarımsak", "Tuz", "Kimyon"]
    }
]

with open("C:/smartkitchen2/Data/DatabaseService.cs", "w", encoding="utf-8") as f:
    f.write("using SQLite;\n")
    f.write("using SmartKitchen.Models;\n")
    f.write("using Newtonsoft.Json;\n\n")
    f.write("namespace SmartKitchen.Data;\n\n")
    f.write("public partial class DatabaseService\n{\n")
    f.write("    private SQLiteAsyncConnection _database;\n")
    f.write("    private readonly string _dbPath;\n\n")
    f.write("    public DatabaseService()\n    {\n")
    f.write("        _dbPath = Path.Combine(FileSystem.AppDataDirectory, \"smartkitchen_v5.db3\");\n")
    f.write("    }\n\n")
    f.write("    public async Task Init()\n    {\n")
    f.write("        if (_database is not null)\n            return;\n\n")
    f.write("        _database = new SQLiteAsyncConnection(_dbPath);\n")
    f.write("        await _database.CreateTableAsync<Ingredient>();\n")
    f.write("        await _database.CreateTableAsync<Recipe>();\n")
    f.write("        await _database.CreateTableAsync<RecipeIngredient>();\n\n")
    f.write("        await SeedDataAsync();\n    }\n\n")
    
    f.write("    public async Task<List<Ingredient>> GetIngredientsAsync()\n    {\n")
    f.write("        await Init();\n        return await _database.Table<Ingredient>().ToListAsync();\n    }\n\n")
    
    f.write("    public async Task<int> SaveIngredientAsync(Ingredient item)\n    {\n")
    f.write("        await Init();\n        if (item.Id != 0)\n            return await _database.UpdateAsync(item);\n        else\n            return await _database.InsertAsync(item);\n    }\n\n")
    
    f.write("    public async Task<List<Recipe>> GetRecipesAsync()\n    {\n")
    f.write("        await Init();\n        return await _database.Table<Recipe>().ToListAsync();\n    }\n\n")
    
    f.write("    public async Task<List<RecipeIngredient>> GetRecipeIngredientsAsync()\n    {\n")
    f.write("        await Init();\n        return await _database.Table<RecipeIngredient>().ToListAsync();\n    }\n\n")
    
    f.write("    private async Task SeedDataAsync()\n    {\n")
    f.write("        var count = await _database.Table<Ingredient>().CountAsync();\n")
    f.write("        if (count > 0)\n            return;\n\n")
    
    f.write("        var ingredients = new List<Ingredient>\n        {\n")
    
    for i, ing in enumerate(ingredients):
        comma = "," if i < len(ingredients)-1 else ""
        f.write(f'            new Ingredient {{ Name = "{ing[0]}", Category = "{ing[1]}", IconUrl = "{ing[2]}" }}{comma}\n')
    
    f.write("        };\n\n")
    f.write("        await _database.InsertAllAsync(ingredients);\n")
    f.write("        var insertedIngredients = await _database.Table<Ingredient>().ToListAsync();\n\n")
    f.write("        int GetId(string name) {\n"
            "            var ing = insertedIngredients.FirstOrDefault(i => i.Name == name);\n"
            "            if (ing != null) return ing.Id;\n"
            "            return 1; // Fallback to first item to avoid crashing\n"
            "        }\n\n")
    
    f.write("        var recipes = new List<Recipe>\n        {\n")
    
    for i, r in enumerate(recipes):
        comma = "," if i < len(recipes)-1 else ""
        ins_json = json.dumps(r['Ins'], ensure_ascii=False).replace('"', '\\"')
        
        # Use dynamic food images from loremflickr
        img_url = f"https://loremflickr.com/500/500/food,dish?lock={100 + i}"

        f.write("            new Recipe\n            {\n")
        f.write(f'                Title = "{r["Title"]}",\n')
        f.write(f'                Description = "{r["Desc"]}",\n')
        f.write(f'                PrepTimeMinutes = {r["Time"]},\n')
        f.write(f'                Calories = {r["Cal"]},\n')
        f.write(f'                ProteinGrams = {r["Prot"]},\n')
        f.write(f'                IsHighProtein = {r["HiP"]},\n')
        f.write(f'                ImageUrl = "{img_url}",\n')
        f.write(f'                Instructions = "{ins_json}"\n')
        f.write(f"            }}{comma}\n")
    f.write("        };\n\n")
    
    f.write("        await _database.InsertAllAsync(recipes);\n")
    f.write("        var insertedRecipes = await _database.Table<Recipe>().ToListAsync();\n        \n")
    f.write("        int GetRecipeId(string title) {\n"
            "            var rec = insertedRecipes.FirstOrDefault(r => r.Title == title);\n"
            "            if (rec != null) return rec.Id;\n"
            "            return 1;\n"
            "        }\n\n")
    
    f.write("        var recipeIngredients = new List<RecipeIngredient>\n        {\n")
    
    all_ri = []
    for r in recipes:
        for ing in r["Ing"]:
            all_ri.append(f'            new RecipeIngredient {{ RecipeId = GetRecipeId("{r["Title"]}"), IngredientId = GetId("{ing}"), Quantity = "Gerektiği kadar" }}')
            
    f.write(",\n".join(all_ri) + "\n")
    f.write("        };\n\n")
    f.write("        await _database.InsertAllAsync(recipeIngredients);\n")
    f.write("    }\n}\n")
