# İkinci El Ürün Satış Uygulaması

Bu proje, SQL Server veritabanı ve C# Windows Forms kullanılarak geliştirilmiş bir ikinci el ürün satış uygulamasıdır. Uygulama, kullanıcıların ürünleri listelemesine, filtrelemesine ve admin tarafından ürünlerin eklenip güncellenmesine olanak tanır.

## 📌 Özellikler

- Ürün bilgilerini detaylı listeleme
- SQL Server ile ilişkisel veritabanı kullanımı
- Kullanıcı dostu Windows Forms arayüzü

## 🧱 Kullanılan Teknolojiler

- C# (Windows Forms)
- SQL Server Management Studio (SSMS)
- .NET Framework

## 🗃️ Veritabanı Yapısı

### Tablolar:

- **kullaniciBilgileri**
  - `kullaniciID` (PK)
  - `kullaniciAdi`
- **esya**
  - `esyaID` (PK)
  - `esyaAdi`
  - `kategori`
  - `fiyat`

- **urunBilgi**
  - `urunID` (PK)
  - `kullaniciID` (FK)
  - `esyaID` (FK)

> Not: `urunBilgi` tablosu, `kullaniciBilgileri` ve `esya` tabloları ile foreign key ilişkisine sahiptir.

## 🖼️ Arayüz Özellikleri

- Ürün listeleme ekranı
- Sade ve anlaşılır kullanıcı arayüzü
👨‍🎓 Hazırlayan
-  Ad Soyad: Burak Dere
- Öğrenci No: 234410123
