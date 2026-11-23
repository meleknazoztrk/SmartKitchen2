# 🍳 SmartKitchen - Akıllı Mutfak & Karar Motoru

> **"Evdeki malzemelerle bugün ne pişirsem?"** sorusuna saniyeler içinde yanıt veren modern mobil mutfak asistanı.

[![.NET MAUI](https://img.shields.io/badge/.NET%20MAUI-10.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/en-us/apps/maui)
[![Platform](https://img.shields.io/badge/Platform-Android-3DDC84?logo=android&logoColor=white)](https://www.android.com/)
[![Database](https://img.shields.io/badge/Database-Firebase%20Realtime-FFCA28?logo=firebase&logoColor=black)](https://firebase.google.com/)
[![Architecture](https://img.shields.io/badge/Architecture-MVVM-blue)](https://learn.microsoft.com/dotnet/architecture/maui/)
[![Release](https://img.shields.io/badge/Release-v1.0.0-green.svg)](https://github.com/meleknazoztrk/SmartKitchen2/releases/tag/v1.0.0)

---

## 📱 Proje Amacı ve Vizyonu

SmartKitchen, standart ve hantal bir tarif defteri uygulamasından farklı olarak **"Eldeki malzemeyle ne pişirilir?"** karar motoruna odaklanır. Kullanıcılar dolaplarındaki mevcut malzemeleri seçer; sistem algoritmik olarak veritabanındaki tariflerle karşılaştırıp **en yüksek eşleşme yüzdesine** sahip yemekleri listeler.

Kullanıcıyı uzun ve karmaşık metinlerle yormak yerine; tarif detayında ana malzemeleri gösterip **YouTube** ve **Nefis Yemek Tarifleri** gibi popüler platformlara tek tıkla doğrudan arama yönlendirmesi sunar.

---

## ✨ Öne Çıkan Özellikler

* 🛒 **Akıllı Dolap Yönetimi:** Dolabınızda veya kilerinizdeki malzemeleri kategori ve emoji ikonlarıyla hızla seçin ve sepetinize ekleyin.
* 🎯 **Yüzdelik Eşleşme Algoritması:** Seçtiğiniz malzemelere göre tarifleri analiz eder ve elinizdeki malzemelerle hangi yemeği % kaç oranında yapabileceğinizi hesaplar.
* 🔍 **Eksik Malzeme Tespiti:** Seçilen tarif için hangi malzemelerin eksik olduğunu anında listeler.
* 🚀 **Otomatik Tarif Arama & Yönlendirme:**
  * **▶ YouTube'da İzle:** Yemeğin adıyla videolu yapılış adımlarını YouTube uygulamasında veya tarayıcıda anında açar.
  * **📖 Nefis Yemek Tarifleri'nde Gör:** Türkiye'nin en popüler tarif platformunda ilgili yemeğin detaylarını ve püf noktalarını görüntüler.
* ☁️ **Firebase Realtime Database:** Bulut tabanlı veri yapısı sayesinde güncel tarif kataloğu ve hızlı kullanıcı oturum yönetimi.
* 🌙 **Modern & Akıcı Karanlık Tema:** Gözü yormayan neon yeşil vurgularla zenginleştirilmiş koyu tema (Dark Mode UI).

---

## 📸 Ekran Görüntüleri

| 1. Dolabım & Malzeme Seçimi | 2. Akıllı Keşfet & Eşleşme | 3. Tarif Detay & Karar Motoru |
|:---:|:---:|:---:|
| <img src="docs/screenshots/1_dolabim.png" width="260" alt="Dolabım"/> | <img src="docs/screenshots/2_kesfet.png" width="260" alt="Keşfet"/> | <img src="docs/screenshots/3_tarif_detay.png" width="260" alt="Tarif Detay"/> |

---

## 📥 Uygulamayı İndir (APK)

Uygulamanın en güncel imzalı (Signed) Android APK sürümünü doğrudan aşağıdaki bağlantıdan indirebilirsiniz:

👉 **[SmartKitchen Android APK İndir (v1.0.0)](https://github.com/meleknazoztrk/SmartKitchen2/releases/download/v1.0.0/com.companyname.smartkitchen.apk)**

Alternatif olarak [Releases](https://github.com/meleknazoztrk/SmartKitchen2/releases) sekmesini ziyaret edebilirsiniz.

---

## 🛠️ Kullanılan Teknolojiler ve Mimari

* **Framework:** .NET MAUI (.NET 10)
* **Tasarım Deseni:** MVVM (Model-View-ViewModel) with CommunityToolkit.Mvvm
* **Veritabanı:** Google Firebase Realtime Database
* **Ağ & API:** `FirebaseDatabase.net`, `System.Net.Http`, `Newtonsoft.Json`
* **Cihaz Özellikleri:** Microsoft.Maui.ApplicationModel (`Launcher.Default.OpenAsync` ile sistem tarayıcısı entegrasyonu)

---

## 🚀 Projeyi Yerelde Çalıştırma

Projeyi bilgisayarınızda derleyip çalıştırmak için:

1. **Gereksinimler:**
   * Visual Studio 2022 (v17.10 veya üzeri) veya .NET 10 SDK
   * ".NET Multi-platform App UI geliştirme" iş yükü
   * Android SDK (API 34 veya üzeri)

2. **Depoyu Klonlayın:**
   ```bash
   git clone https://github.com/meleknazoztrk/SmartKitchen2.git
   cd SmartKitchen2
   ```

3. **Derleme & Çalıştırma:**
   ```bash
   dotnet build -f net10.0-android
   ```

4. **APK Çıktısı Alma:**
   ```bash
   dotnet publish -f net10.0-android -c Release
   ```
