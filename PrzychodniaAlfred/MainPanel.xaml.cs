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
    }
}
