using PrzychodniaAlfred.Models;
using System.Net.Http;
using System.Text.Json;
using System.Windows;

namespace PrzychodniaAlfred
{
    public partial class MainPanel : Window
    {
        private string rola;
        private string imie;

        public MainPanel(string rola, string imie)
        {
            InitializeComponent();
            this.rola = rola;
            this.imie = imie;

            txtZalogowanoJako.Text = $"Zalogowano jako: {imie} ({rola})";
            OgraniczUprawnienia();
            btnZarzadzajUzytkownikami.Click += (s, e) =>
            {
                var okno = new users();
                okno.ShowDialog();
            };

            btnWyloguj.Click += BtnWyloguj_Click;
        }

        private void OgraniczUprawnienia()
        {
            switch (rola)
            {
                case "A":
                    break;
                case "L":
                    btnZarzadzajUzytkownikami.IsEnabled = false;
                    btnStatystyki.IsEnabled = false;
                    break;
                case "R":
                    btnZarzadzajUzytkownikami.IsEnabled = false;
                    btnStatystyki.IsEnabled = false;
                    btnUrlopy.IsEnabled = false;
                    break;
                default:
                    MessageBox.Show("Nieznana rola.");
                    Close();
                    break;
            }
        }

        private void BtnWyloguj_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }

        private async void btnDodajWizyte_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var http = new HttpClient();

                var lekarzeJson = await http.GetStringAsync("https://kineh.smallhost.pl/przychodnia/pobierzlekarzy.php");
                var pacjenciJson = await http.GetStringAsync("https://kineh.smallhost.pl/przychodnia/pobierzpacjentow.php");

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var lekarze = JsonSerializer.Deserialize<List<User>>(lekarzeJson, options);
                var pacjenci = JsonSerializer.Deserialize<List<Pacjent>>(pacjenciJson, options);

                //MessageBox.Show($"Lekarzy: {lekarze?.Count ?? 0}, Pacjentów: {pacjenci?.Count ?? 0}");

                var okno = new DodajWizyteWindow(lekarze, pacjenci);
                okno.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd połączenia z serwerem:\n" + ex.Message);
            }
        }
        private void btnKalendarz_Click(object sender, RoutedEventArgs e)
        {
            var okno = new KalendarzWizytWindow();
            okno.ShowDialog();
        }

        private void btnPacjenci_Click(object sender, RoutedEventArgs e)
        {
            var okno = new PacjenciWindow();
            okno.ShowDialog();
        }
        private void btnUrlopy_click(object sender, RoutedEventArgs e)
        {
               var okno = new UrlopyWindow();
            okno.ShowDialog();
        }
    }
}
