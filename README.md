# PrzychodniaAlfred
Aplikacja napisana w języku C# (Windows Forms), służąca do rejestracji pacjentów w przychodni medycznej. Program umożliwia dodawanie, edytowanie i usuwanie pacjentów oraz zarządzanie ich danymi.
## Funkcje
- Dodawanie nowego pacjenta
- Edycja danych pacjenta
- Usuwanie pacjenta z listy
- Wyświetlanie listy zarejestrowanych pacjentów
- Walidacja danych (np. numer PESEL, imię, nazwisko)
- Prosty i intuicyjny interfejs graficzny (Windows Forms)
- ## Zrzuty ekranu
Ekran logowania - w zależności od użytkownika program przyjmuje rolę Administratror, Lekarz, Recepcjonista
![Alfred logowanie](https://github.com/user-attachments/assets/5c52c73e-9862-4421-bdd6-e8f56c8f1047)

Widok menu głównego
![Alfred menu główne](https://github.com/user-attachments/assets/6b0071c4-c8cd-427f-8ab1-12dfc787a882)

Widok menu Użytkownicy - możliwość dodania nowego użytkownika lub zmian w zapisanych użytkownikach
![Alfred użytkownicy](https://github.com/user-attachments/assets/03d3f64d-4229-4df8-997d-62e8819540fd)

Widok menu umawiania wizyty
![Alfred wizyty2](https://github.com/user-attachments/assets/6d0953c7-a162-4996-9d62-76370ffac087)

Widok opcji urlopy pokazującej absencje lekarzy
![Alfred urlopy](https://github.com/user-attachments/assets/a1d333d4-43b2-4f38-b50d-ccd85183c06a)

Widok listy pacjentów z opcją dodawania i edycji
![Alfred pacjęci](https://github.com/user-attachments/assets/21ca1982-de08-4911-ae7b-f39fc004547d)

Widok kalendarza
![Alfred kalendarz wizyt](https://github.com/user-attachments/assets/a5f6c393-85c7-4a43-a53d-1591e7dd2717)
## Wymagania
- .NET Framework 4.7.2 lub nowszy
- Windows 10/11
- Visual Studio 2022 (lub nowszy)
- Serwer z obsługą PHP
- Baza danych MySQL
##  Struktura projektu

- **PrzychodniaAlfred/**
  - **WPFApp/** – Aplikacja desktopowa (C#, WPF)
    - App.xaml  
    - MainWindow.xaml  
    - MainPanel.xaml  
    - DodajWizyteWindow.xaml  
    - DodajUzytkownikaWindow.xaml  
    - KalendarzWizydWindow.xaml  
    - StatyWindow.xaml  
    - PacjenciWindow.xaml  
    - UrlopyWindow.xaml  
    - users.xaml  
    - **Models/**
      - User.cs  
      - Wizyta.cs  
      - Pacjent.cs  
      - PacjentStaty.cs  
      - LekStaty.cs  
      - LoginResponse.cs  
      - Interfejsy.cs  
    - **images/**
      - koteg.png  

  - **PHPapi/** – Backend (API w PHP)
    - db.php  
    - dodajpacjenta.php  
    - dodajurlop.php  
    - dodajuser.php  
    - dodajwizyte.php  
    - edytujuser.php  
    - login.php  
    - pobierzlekarzy.php  
    - pobierzpacjentow.php  
    - pobierzulropy.php  
    - pobierzwizyty.php  
    - users.php  
    - usunuser.php  
    - wszystkiewizyty.php  

  - **DB/**
    - przychodDB.sql – struktura bazy MySQL

## Dziedziczenie i użycie bibliotek w projekcie *PrzychodniaAlfred*

### 1. Przykłady dziedziczenia

| Plik | Deklaracja | Dziedziczone / implementowane | Cel |
|------|-----------|------------------------------|-----|
| `Models/LekStaty.cs` | `public class LekStaty : Raport, IInterfejsRaportu` | • **Raport** (klasa bazowa z właściwościami `Nazwa`, `Wartosc`) <br>• **IInterfejsRaportu** (wymusza `GenerujRaport()`) |  Obiekt spełnia dwa kontrakty: reprezentuje wiersz raportu i potrafi się sam wygenerować. |
| `Models/PacjentStaty.cs` | `public class PacjentStaty : Raport, IInterfejsRaportu` | Jak wyżej, ale dla statystyk pacjentów. |  |
| `App.xaml.cs` | `public partial class App : Application` | **System.Windows.Application** | Uzyskuje cały cykl życia aplikacji WPF. |
| `*.Window.xaml.cs` (np. `MainWindow`) | `public partial class MainWindow : Window` | **System.Windows.Window** | Każde okno „dostaje” zdarzenia, renderowanie i binding WPF. |

> **Wielokrotne dziedziczenie w stylu C#**  
> Klasa może dziedziczyć **tylko jedną** klasę bazową, ale **dowolną liczbę interfejsów**. Dzięki temu logika raportów pozostaje odseparowana od UI.

---

### 2. Używane biblioteki (`using …;`)

| Fragment kodu | Biblioteka | Funkcjonalność |
|---------------|-----------|----------------|
using System.Net.Http;
using System.Text.Json;

using var http = new HttpClient();
string json = await http.GetStringAsync(url);
var pacjenci = JsonSerializer.Deserialize<List<Pacjent>>(json);
| **System.Net.Http** <br>**System.Text.Json** | Pobieranie danych z API i deserializacja JSON do listy `Pacjent`. |
|
using System.Data;
//using MySql.Data.MySqlClient;
| **System.Data** (ADO.NET) <br>**MySql.Data.MySqlClient** (opcjonalnie) | Planowane połączenie z MySQL, zapytania SQL, wypełnianie `DataTable`. |
|
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
| **PresentationFramework / PresentationCore** | Kontrolki, kolory, zdarzenia — kompletny stack WPF. |
|
using System.Collections.Generic;
| **mscorlib** | Kolekcje generyczne (`List<T>`, `Dictionary<K,V>`). |


### 3. Jak warstwy współpracują?

1. **Modele / Statystyki**  
   Implementują interfejsy i dziedziczą po bazowych klasach raportów.  
2. **Prezentacja (WPF)**  
   Klasy okien dziedziczą po `Window`, korzystając z bibliotek .NET (HTTP, JSON).  
3. **Dostęp do danych** *(opcjonalnie)*  
   Klasa `Database` (zakomentowana) miała korzystać z MySQL Connector, co pokazałoby współpracę z biblioteką zewnętrzną.

// Przykład pobrania pacjentów
private async void ZaladujPacjentow()
{
    using var http = new HttpClient();
    string url = "https://kineh.smallhost.pl/przychodnia/pobierzpacjentow.php";
    var pacjenci = JsonSerializer.Deserialize<List<Pacjent>>(
        await http.GetStringAsync(url),
        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    listaPacjentow.ItemsSource = pacjenci;
}

## Polimorfizm w projekcie *PrzychodniaAlfred*
Polimorfizm oparty na interfejsie `InterfejsRaportu`. Pozwala to na obsługę różnych typów raportów (`LekStaty`, `PacjentStaty`) w sposób jednolity, bez potrzeby znajomości ich konkretnych klas w miejscu użycia (np. w `StatyWindow`).

Dzięki temu można np. z łatwością dodawać kolejne typy raportów w przyszłości — wystarczy, że będą implementować `InterfejsRaportu`.


| Klasa           | Bazowa klasa | Implementowany interfejs | Kluczowa metoda        | Zastosowanie                                                  |
|------------------|---------------|----------------------------|-------------------------|----------------------------------------------------------------|
| `LekStaty`        | `Raport`      | `InterfejsRaportu`         | `GenerujRaport()`       | Generuje raport wizyt przypisanych do lekarzy.                |
| `PacjentStaty`    | `Raport`      | `InterfejsRaportu`         | `GenerujRaport()`       | Generuje raport wizyt przypisanych do pacjentów.              |
| `StatyWindow`     | —             | —                          | używa `InterfejsRaportu` | Przechowuje i wywołuje metody na liście raportów polimorficznie.|




## Hermetyzacja w projekcie *PrzychodniaAlfred*

W projekcie przykładem hermetyzacji jest klasa `Pacjent`, która zawiera dane wrażliwe użytkownika, takie jak PESEL, numer telefonu czy e-mail.
Metody 'get' i 'set' umożliwiają kontrolowany dostęp do prywatnych pól klasy.


| Składnik       | Modyfikator dostępu | Cel hermetyzacji                                      |
|----------------|---------------------|--------------------------------------------------------|
| `Id`           | `public`            | Unikalny identyfikator pacjenta.                       |
| `Imie`         | `public`            | Udostępniane jawnie jako część tożsamości.             |
| `Nazwisko`     | `public`            | Udostępniane jawnie jako część tożsamości.             |
| `Pesel`        | `public`            | Zmienna zawiera dane wrażliwe — warto ograniczyć dostęp. |
| `Telefon`      | `public`            | Dane kontaktowe — mogą wymagać walidacji przy zmianie. |
| `Email`        | `public`            | Dane kontaktowe — potencjalnie używane do komunikacji. |
| `DataUrodzenia`| `public`            | Informacja prywatna — może być używana w raportach.    |
| `FullName`     | `public (tylko get)`| Właściwość tylko do odczytu — składane imię i nazwisko. |






## TODO PRZYSZŁOŚCIOWE:
- [ ] Dodanie walidacji wprowadzanych danych (np. PESEL, e-mail, telefon)
- [ ] Rozbudowanie systemu statystyk (np. filtrowanie, eksport CSV, wykresy)

## Autor
Projekt stworzony przez [Iga Gocył, Mateusz Kaczmarczyk, Marcin Garus] w ramach realizacji nauki przedmiotu Programowania obiektowe.
## Licencja
Projekt dostępny na licencji MIT. Możesz swobodnie używać, kopiować i modyfikować.
