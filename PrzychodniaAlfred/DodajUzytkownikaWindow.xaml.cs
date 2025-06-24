using PrzychodniaAlfred.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Net.Http.Headers;

namespace PrzychodniaAlfred
{
    public partial class DodajUzytkownikaWindow : Window
    {
        private readonly HttpClient _httpClient = new HttpClient();

        private bool isEdycja => this.Tag is int;

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

            if (string.IsNullOrWhiteSpace(imie) || string.IsNullOrWhiteSpace(nazwisko) ||
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

            object payload;

            if (isEdycja)
            {
                payload = new
                {
                    id = (int)this.Tag,
                    imie,
                    nazwisko,
                    rola,
                    specjalizacja = rola == "L" ? specjalizacja : null
                };
            }
            else
            {
                payload = new
                {
                    login,
                    haslo,
                    imie,
                    nazwisko,
                    rola,
                    specjalizacja = rola == "L" ? specjalizacja : null
                };
            }

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            string endpoint = isEdycja
                ? "https://kineh.smallhost.pl/przychodnia/edytujuser.php"
                : "https://kineh.smallhost.pl/przychodnia/dodajuser.php";

            try
            {
                var response = await _httpClient.PostAsync(endpoint, content);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ApiResponse>(responseBody);

                if (result?.success == true)
                {
                    MessageBox.Show(isEdycja ? "Użytkownik zaktualizowany." : "Użytkownik dodany.");
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(result?.message ?? "Nie udało się zapisać użytkownika.");
                }
            }
            catch
            {
                MessageBox.Show("Błąd połączenia z serwerem.");
            }
        }

        public DodajUzytkownikaWindow(User edytowany) : this()
        {
            txtLogin.Text = edytowany.Login;
            txtImie.Text = edytowany.Imie;
            txtNazwisko.Text = edytowany.Nazwisko;
            cmbRola.SelectedValue = edytowany.Rola;
            txtSpecjalizacja.Text = edytowany.Specjalizacja ?? "";

            txtHaslo.Password = "*******";
            txtHaslo.IsEnabled = false;

            this.Tag = edytowany.Id;

            // Ustawienie przycisku "Dodaj" na "Zapisz" - potrzebne nazwane odniesienie
            var button = LogicalTreeHelper.FindLogicalNode(this, "DodajButton") as Button;
            if (button != null)
                button.Content = "Zapisz";
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
