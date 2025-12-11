# Cspdf Kütüphanesini Yayınlama Rehberi

Bu rehber, Cspdf kütüphanesini NuGet paketi olarak yayınlama sürecini açıklar.

## Ön Hazırlık

### 1. NuGet Hesabı Oluşturma

1. [nuget.org](https://www.nuget.org/) adresine gidin
2. "Sign in" butonuna tıklayın
3. Microsoft hesabınızla giriş yapın veya yeni hesap oluşturun
4. Profil sayfanızdan API Key oluşturun

### 2. Proje Yapılandırması

Proje dosyası (`Cspdf.csproj`) zaten NuGet paketi için yapılandırılmış durumda. Gerekirse aşağıdaki bilgileri güncelleyin:

- `Authors`: Paket yazarları
- `Description`: Paket açıklaması
- `RepositoryUrl`: GitHub repository URL'i
- `Version`: Paket versiyonu

## Yerel Test (Paketi Oluşturma)

### 1. Paketi Oluştur

```bash
dotnet pack --configuration Release
```

Bu komut `bin/Release/` klasöründe `.nupkg` dosyası oluşturur.

### 2. Paketi Yerel Olarak Test Et

```bash
# Yerel NuGet feed oluştur (opsiyonel)
dotnet nuget add source C:\LocalNuGet --name LocalFeed

# Paketi yerel feed'e ekle
dotnet nuget push bin/Release/Cspdf.1.0.0.nupkg --source LocalFeed
```

Veya doğrudan dosya yolundan test edebilirsiniz:

```bash
# Test projesinde
dotnet add package Cspdf --version 1.0.0 --source C:\path\to\packages
```

## NuGet.org'a Yayınlama

### Yöntem 1: dotnet CLI ile (Önerilen)

```bash
# 1. Paketi oluştur
dotnet pack --configuration Release

# 2. NuGet.org'a yayınla
dotnet nuget push bin/Release/Cspdf.1.0.0.nupkg --api-key YOUR_API_KEY --source https://api.nuget.org/v3/index.json
```

### Yöntem 2: NuGet CLI ile

1. [NuGet CLI](https://www.nuget.org/downloads) indirin
2. Komut satırından:

```bash
nuget push bin\Release\Cspdf.1.0.0.nupkg YOUR_API_KEY -Source https://api.nuget.org/v3/index.json
```

### Yöntem 3: NuGet.org Web Arayüzü ile

1. [nuget.org](https://www.nuget.org/) adresine gidin
2. "Upload" butonuna tıklayın
3. `.nupkg` dosyasını seçin ve yükleyin

## API Key Alma

1. [nuget.org](https://www.nuget.org/) → Profil → API Keys
2. "Create" butonuna tıklayın
3. Key adı verin (örn: "Cspdf Publishing")
4. Expiration süresi seçin
5. "Select scopes" → "Push new packages and package versions" seçin
6. "Create" butonuna tıklayın
7. Oluşturulan key'i kopyalayın (sadece bir kez gösterilir!)

## Versiyonlama

Semantic Versioning (SemVer) kullanın: `MAJOR.MINOR.PATCH`

- **MAJOR**: Geriye dönük uyumsuz değişiklikler
- **MINOR**: Yeni özellikler (geriye dönük uyumlu)
- **PATCH**: Hata düzeltmeleri

Versiyonu güncellemek için `Cspdf.csproj` dosyasındaki `<Version>` etiketini değiştirin.

## Otomatik Yayınlama (GitHub Actions)

GitHub Actions ile otomatik yayınlama için `.github/workflows/publish.yml` dosyası oluşturulmuştur.

### Kullanım

1. GitHub repository'de "Settings" → "Secrets and variables" → "Actions"
2. `NUGET_API_KEY` adında bir secret ekleyin (NuGet API key'inizi)
3. Yeni bir release oluşturun veya tag oluşturun
4. GitHub Actions otomatik olarak paketi yayınlayacaktır

## Yayınlama Sonrası Kontroller

1. [nuget.org/packages/cspdf](https://www.nuget.org/packages/cspdf) adresinde paketinizi kontrol edin
2. Paket açıklaması, versiyon, bağımlılıklar doğru mu kontrol edin
3. Bir test projesi oluşturup paketi yükleyerek test edin:

```bash
dotnet new console -n TestApp
cd TestApp
dotnet add package Cspdf
```

## Önemli Notlar

- İlk yayınlamadan sonra paket adı rezerve edilir
- Paket adı değiştirilemez
- Versiyon numarası her yayınlamada artırılmalıdır
- Paket yayınlandıktan sonra silinemez (unlist edilebilir)
- Unlist edilen paketler hala indirilebilir ama arama sonuçlarında görünmez

## Sorun Giderme

### "Package already exists" hatası
- Versiyon numarasını artırın

### "API key is invalid" hatası
- API key'inizi kontrol edin
- Key'in expiration tarihini kontrol edin

### "Package validation failed" hatası
- Paket metadata'sını kontrol edin
- Gerekli alanların doldurulduğundan emin olun

