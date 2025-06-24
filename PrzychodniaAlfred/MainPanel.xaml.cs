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
                // Pobierz lekarzy
                var lekarzeResponse = await new HttpClient().GetStringAsync("https://kineh.smallhost.pl/przychodnia/pobierzlekarzy.php");
                MessageBox.Show("Lekarze JSON:\n" + lekarzeResponse);
                var lekarze = JsonSerializer.Deserialize<List<User>>(lekarzeResponse);

                // Pobierz pacjentów
                var pacjenciResponse = await new HttpClient().GetStringAsync("https://kineh.smallhost.pl/przychodnia/pobierzpacjentow.php");
                MessageBox.Show("Pacjenci JSON:\n" + pacjenciResponse);
                var pacjenci = JsonSerializer.Deserialize<List<Pacjent>>(pacjenciResponse);
                    
                if (lekarze == null || pacjenci == null)
                {
                    MessageBox.Show("Nie udało się pobrać danych.");
                    return;
                }

                var okno = new DodajWizyteWindow(lekarze, pacjenci);
                okno.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd połączenia z serwerem:\n" + ex.Message);
            }
        }



    }
}
