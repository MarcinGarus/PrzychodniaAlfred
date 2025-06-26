using PrzychodniaAlfred.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace PrzychodniaAlfred
{
    public partial class UrlopyWindow : Window
    {
        private List<User> lekarze;

        public UrlopyWindow()
        {
            InitializeComponent();
            WczytajLekarzy();
            WczytajUrlopy();
        }

        private async void WczytajLekarzy()
        {
            try
            {
                using var http = new HttpClient();
                var json = await http.GetStringAsync("https://kineh.smallhost.pl/przychodnia/pobierzlekarzy.php");
                lekarze = JsonSerializer.Deserialize<List<User>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                comboLekarz.ItemsSource = lekarze;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas pobierania lekarzy:\n" + ex.Message);
            }
        }

        private async void WczytajUrlopy()
        {
            try
            {
                using var http = new HttpClient();
                var json = await http.GetStringAsync("https://kineh.smallhost.pl/przychodnia/pobierzurlopy.php");
                var urlopy = JsonSerializer.Deserialize<List<UrlopDTO>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                listaUrlopow.ItemsSource = null;
                listaUrlopow.Items.Clear();

                foreach (var u in urlopy)
                {
                    string wpis = $"{u.DataOd:yyyy-MM-dd} do {u.DataDo:yyyy-MM-dd} — {u.LekarzImie} {u.LekarzNazwisko} ({u.Powod})";
                    listaUrlopow.Items.Add(wpis);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas pobierania urlopów:\n" + ex.Message);
            }
        }

        private async void BtnDodajUrlop_Click(object sender, RoutedEventArgs e)
        {
            if (comboLekarz.SelectedItem is not User wybranyLekarz)
            {
                MessageBox.Show("Wybierz lekarza.");
                return;
            }

            if (dpOd.SelectedDate == null || dpDo.SelectedDate == null)
            {
                MessageBox.Show("Wybierz daty urlopu.");
                return;
            }

            var urlop = new
            {
                IdLekarza = wybranyLekarz.Id,
                DataOd = dpOd.SelectedDate.Value.ToString("yyyy-MM-dd"),
                DataDo = dpDo.SelectedDate.Value.ToString("yyyy-MM-dd"),
                Powod = txtPowod.Text
            };

            try
            {
                using var http = new HttpClient();
                var content = new StringContent(JsonSerializer.Serialize(urlop), Encoding.UTF8, "application/json");
                var response = await http.PostAsync("https://kineh.smallhost.pl/przychodnia/dodajurlop.php", content);
                var responseJson = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Urlop dodany.");
                    WczytajUrlopy();
                }
                else
                {
                    MessageBox.Show("Błąd: " + responseJson);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas dodawania urlopu:\n" + ex.Message);
            }
        }

        private class UrlopDTO
        {
            public string LekarzImie { get; set; }
            public string LekarzNazwisko { get; set; }
            public DateTime DataOd { get; set; }
            public DateTime DataDo { get; set; }
            public string Powod { get; set; }
        }
    }
}
