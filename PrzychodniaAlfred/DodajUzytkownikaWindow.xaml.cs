using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace PrzychodniaAlfred
{
    public partial class DodajUzytkownikaWindow : Window
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public DodajUzytkownikaWindow()
        {
            InitializeComponent();
        }

        private async void Dodaj_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string haslo = txtHaslo.Password.Trim();
            string imie = txtImie.Text.Trim();
            string nazwisko = txtNazwisko.Text.Trim();
            string rola = (cmbRola.SelectedItem as ComboBoxItem)?.Content.ToString();
            string specjalizacja = txtSpecjalizacja.Text.Trim();

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(haslo) ||
                string.IsNullOrWhiteSpace(imie) || string.IsNullOrWhiteSpace(nazwisko) ||
                string.IsNullOrWhiteSpace(rola))
            {
                MessageBox.Show("Uzupełnij wszystkie wymagane pola.");
                return;
            }

            if (rola == "L" && string.IsNullOrWhiteSpace(specjalizacja))
            {
                MessageBox.Show("Lekarz musi mieć specjalizację.");
                return;
            }

            var payload = new
            {
                login,
                haslo,
                imie,
                nazwisko,
                rola,
                specjalizacja = rola == "L" ? specjalizacja : ""
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("https://kineh.smallhost.pl/przychodnia/dodajuser.php", content);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ApiResponse>(responseBody);

                if (result?.success == true)
                {
                    MessageBox.Show("Użytkownik dodany.");
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(result?.message ?? "Nie udało się dodać użytkownika.");
                }
            }
            catch
            {
                MessageBox.Show("Błąd połączenia z serwerem.");
            }
        }

        private void cmbRola_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = (cmbRola.SelectedItem as ComboBoxItem)?.Content.ToString();
            specjalizacjaPanel.Visibility = selected == "L" ? Visibility.Visible : Visibility.Collapsed;
        }

        private class ApiResponse
        {
            public bool success { get; set; }
            public string message { get; set; }
        }
    }
}
