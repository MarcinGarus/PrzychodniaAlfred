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
## Struktura projektu
PrzychodniaAlfred/
│
├── WPFApp/                   # Aplikacja desktopowa
│   ├── App.xaml
│   ├── MainWindow.xaml
│   ├── MainPanel.xaml
│   ├── DodajWizyteWindow.xaml
│   ├── DodajUzytkownikaWindow.xaml
│   ├── KalendarzWizydWindow.xaml
│   ├── StatyWindow.xaml
│   ├── PacjenciWindow.xaml
│   ├── UrlopyWindow.xaml
│   ├── users.xaml
│   ├── Models/               
│   │   ├── User.cs
│   │   ├── Wizyta.cs
│   │   ├── Pacjent.cs
│   │   ├── PacjentStaty.cs
│   │   ├── LekStaty.cs
│   │   ├── LoginResponse.cs
│   │   └── Interfejsy.cs
│   ├── images/
│   │   ├── koteg.png
│   
│       
│
├── PHPapi/                   # API w PHP (backend)
│   ├── db.php
│   ├── dodajpacjenta.php
│   ├── dodajurlop.php
│   ├── dodajuser.php
│   ├── dodajwizyte.php
│   ├── edytujuser.php
│   ├── login.php
│   ├── pobierzlekarzy.php
│   ├── pobierzpacjentow.php
│   ├── pobierzulropy.php
│   ├── pobierzwizyty.php
│   ├── users.php
│   ├── usunuser.php
│   └── wszystkiewizyty.php
│
└── DB/
    └── przychodDB.sql       # Struktura bazy danych MySQL

## Autor
Projekt stworzony przez [Iga Gocył, Mateusz Kaczmarczyk, Marcin Garus] w ramach realizacji nauki przedmiotu Programowania obiektowe.
## Licencja
Projekt dostępny na licencji MIT. Możesz swobodnie używać, kopiować i modyfikować.
