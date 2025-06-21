// users.xaml.cs
using System.Collections.Generic;
using System.Net.Http;
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


        private void Usun_Click(object sender, RoutedEventArgs e)
        {
            var zaznaczony = dgUzytkownicy.SelectedItem as User;
            var lista = dgUzytkownicy.ItemsSource as List<User>;
            if (zaznaczony != null && lista != null)
            {
                lista.Remove(zaznaczony);
                dgUzytkownicy.Items.Refresh();
            }
        }

        private async void Zapisz_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Tu będzie zapisywanie zmian do bazy przez API (do zrobienia 🚧)");
        }

    }
}
