using PrzychodniaAlfred.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace PrzychodniaAlfred
{
    public partial class DodajWizyteWindow : Window
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly List<User> lekarze;
        private readonly List<Pacjent> pacjenci;

        public DodajWizyteWindow(List<User> lekarze, List<Pacjent> pacjenci)
        {
            InitializeComponent();

            this.lekarze = lekarze ?? new List<User>();
            this.pacjenci = pacjenci ?? new List<Pacjent>();

            Loaded += DodajWizyteWindow_Loaded;
        }

        private void DodajWizyteWindow_Loaded(object sender, RoutedEventArgs e)
        {
            cmbLekarz.ItemsSource = lekarze;
            cmbPacjent.ItemsSource = pacjenci;

            if (cmbLekarz.Items.Count > 0)
                cmbLekarz.SelectedIndex = 0;

            if (cmbPacjent.Items.Count > 0)
                cmbPacjent.SelectedIndex = 0;
        }

        private async void Zapisz_Click(object sender, RoutedEventArgs e)
        {
            var lekarz = cmbLekarz.SelectedItem as User;
            var pacjent = cmbPacjent.SelectedItem as Pacjent;
            var data = dataPicker.SelectedDate;
            var godzina = godzinaBox.Text.Trim();
            var opis = txtOpis.Text.Trim();

            if (lekarz == null || pacjent == null || data == null || string.IsNullOrWhiteSpace(godzina))
            {
                MessageBox.Show("Uzupełnij wszystkie pola.");
                return;
            }

            if (!TimeSpan.TryParse(godzina, out var godzinaCzas))
            {
                MessageBox.Show("Niepoprawna godzina.");
                return;
            }

            DateTime dataCzas = data.Value.Date + godzinaCzas;

            var payload = new
            {
                id_lekarza = lekarz.Id,
                id_pacjenta = pacjent.Id,
                data = dataCzas.ToString("yyyy-MM-dd HH:mm:ss"),
                opis = string.IsNullOrWhiteSpace(opis) ? null : opis
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("https://kineh.smallhost.pl/przychodnia/dodajwizyte.php", content);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var result = JsonSerializer.Deserialize<ApiResponse>(responseBody, options);

                if (result?.success == true)
                {
                    MessageBox.Show("Wizyta zapisana.");
                    DialogResult = true;
                    Close();
                }
                else
                {
                    MessageBox.Show(result?.message ?? "Nie udało się zapisać wizyty.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd połączenia z serwerem:\n" + ex.Message);
            }
        }

        private class ApiResponse
        {
            public bool success { get; set; }
            public string message { get; set; }
        }
    }
}
