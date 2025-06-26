using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PrzychodniaAlfred
{
    public partial class KalendarzWizytWindow : Window
    {
        public KalendarzWizytWindow()
        {
            InitializeComponent();
        }

        private class Wizyta
        {
            public string Godzina { get; set; }
            public string LekarzImie { get; set; }
            public string LekarzNazwisko { get; set; }
            public string PacjentImie { get; set; }
            public string PacjentNazwisko { get; set; }
            public string Opis { get; set; }
        }

        private class Urlop
        {
            public string LekarzImie { get; set; }
            public string LekarzNazwisko { get; set; }
            public DateTime DataOd { get; set; }
            public DateTime DataDo { get; set; }
            public string Powod { get; set; }
        }

        private async void kalendarz_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (kalendarz.SelectedDate == null)
                return;

            listaWizyt.Items.Clear();
            string data = kalendarz.SelectedDate.Value.ToString("yyyy-MM-dd");

            try
            {
                var http = new HttpClient();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                // --- WIZYTY ---
                string wizytyUrl = $"https://kineh.smallhost.pl/przychodnia/pobierzwizyty.php?data={data}";
                var wizytyJson = await http.GetStringAsync(wizytyUrl);
                var wizyty = JsonSerializer.Deserialize<List<Wizyta>>(wizytyJson, options);

                foreach (var w in wizyty)
                {
                    string wpis = $"{w.Godzina} – {w.PacjentImie} {w.PacjentNazwisko} / {w.LekarzImie} {w.LekarzNazwisko} – {w.Opis}";

                    listaWizyt.Items.Add(new ListBoxItem
                    {
                        Content = wpis,
                        Foreground = Brushes.Black
                    });
                }

                // --- URLOPY ---
                var urlopyJson = await http.GetStringAsync("https://kineh.smallhost.pl/przychodnia/pobierzurlopy.php");
                var urlopy = JsonSerializer.Deserialize<List<Urlop>>(urlopyJson, options);

                foreach (var u in urlopy)
                {
                    var selectedDate = kalendarz.SelectedDate.Value;
                    if (selectedDate >= u.DataOd && selectedDate <= u.DataDo)
                    {
                        string wpis = $"URLOP – {u.DataOd:yyyy-MM-dd}–{u.DataDo:yyyy-MM-dd} — {u.LekarzImie} {u.LekarzNazwisko} ({u.Powod})";

                        listaWizyt.Items.Add(new ListBoxItem
                        {
                            Content = wpis,
                            Foreground = Brushes.DarkBlue,
                            FontWeight = FontWeights.Bold
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas pobierania wizyt lub urlopów:\n" + ex.Message);
            }
        }
    }
}
