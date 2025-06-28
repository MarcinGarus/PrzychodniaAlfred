// users.xaml.cs
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;
using PrzychodniaAlfred.Models;

namespace PrzychodniaAlfred
{
    public partial class users : Window
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string apiUrl = "https://kineh.smallhost.pl/przychodnia/users.php";

        public users()
        {
            InitializeComponent();
            WczytajUzytkownikow();
        }

        private async Task WczytajUzytkownikow()
        {
            try
            {
                var response = await _httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                var uzytkownicy = JsonSerializer.Deserialize<List<User>>(responseBody);

                dgUzytkownicy.ItemsSource = uzytkownicy;
            }
            catch
            {
                MessageBox.Show("Błąd pobierania danych z serwera.");
            }
        }
        private async void Odswiez_Click(object sender, RoutedEventArgs e)
        {
            await WczytajUzytkownikow();
        }

        private async void Dodaj_Click(object sender, RoutedEventArgs e)
        {
            var okno = new DodajUzytkownikaWindow();
            if (okno.ShowDialog() == true)
            {
                await WczytajUzytkownikow(); // przeładuj listę użytkowników po dodaniu
            }
        }


        private async void Usun_Click(object sender, RoutedEventArgs e)
        {
            var zaznaczony = dgUzytkownicy.SelectedItem as User;
            if (zaznaczony == null)
            {
                MessageBox.Show("Zaznacz użytkownika do usunięcia.");
                return;
            }

            if (MessageBox.Show($"Czy na pewno usunąć {zaznaczony.Imie} {zaznaczony.Nazwisko}?", "Potwierdź", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    var response = await _httpClient.PostAsync("https://kineh.smallhost.pl/przychodnia/usunuser.php",
                        new StringContent(JsonSerializer.Serialize(new { id = zaznaczony.Id }), Encoding.UTF8, "application/json"));

                    var wynik = JsonSerializer.Deserialize<ApiResponse>(await response.Content.ReadAsStringAsync());

                    if (wynik?.success == true)
                    {
                        MessageBox.Show("Użytkownik usunięty.");
                        await WczytajUzytkownikow();
                    }
                    else
                    {
                        MessageBox.Show(wynik?.message ?? "Błąd usuwania.");
                    }
                }
                catch
                {
                    MessageBox.Show("Błąd połączenia.");
                }
            }
        }
        private async void Edytuj_Click(object sender, RoutedEventArgs e)
        {
            var zaznaczony = dgUzytkownicy.SelectedItem as User;
            if (zaznaczony == null)
            {
                MessageBox.Show("Zaznacz użytkownika do edycji.");
                return;
            }

            var okno = new DodajUzytkownikaWindow(zaznaczony);
            if (okno.ShowDialog() == true)
            {
                await WczytajUzytkownikow();
            }
        }

        public class ApiResponse
        {
            public bool success { get; set; }
            public string message { get; set; }
        }
    }
}
