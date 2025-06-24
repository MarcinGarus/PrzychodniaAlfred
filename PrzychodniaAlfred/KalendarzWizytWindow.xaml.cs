using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using PrzychodniaAlfred.Models;

namespace PrzychodniaAlfred
{
    public partial class KalendarzWizytWindow : Window
    {
        public KalendarzWizytWindow()
        {
            InitializeComponent();
        }

        private async void kalendarz_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (kalendarz.SelectedDate == null)
                return;

            string data = kalendarz.SelectedDate.Value.ToString("yyyy-MM-dd");

            try
            {
                using var http = new HttpClient();
                string url = $"https://kineh.smallhost.pl/przychodnia/pobierzwizyty.php?data={data}";
                string json = await http.GetStringAsync(url);

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var wizyty = JsonSerializer.Deserialize<List<Wizyta>>(json, options);

                listaWizyt.Items.Clear();

                if (wizyty != null && wizyty.Count > 0)
                {
                    foreach (var w in wizyty)
                    {
                        listaWizyt.Items.Add($"{w.Godzina} - dr {w.LekarzImie} {w.LekarzNazwisko} z {w.PacjentImie} {w.PacjentNazwisko} ({w.Opis})");
                    }
                }
                else
                {
                    listaWizyt.Items.Add("Brak wizyt.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd pobierania wizyt: " + ex.Message);
            }
        }

    }
}
