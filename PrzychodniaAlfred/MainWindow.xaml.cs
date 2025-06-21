using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using PrzychodniaAlfred.Models;

namespace PrzychodniaAlfred
{
    public partial class MainWindow : Window
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string apiUrl = "https://kineh.smallhost.pl/przychodnia/login.php";

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginBox.Text.Trim();
            string haslo = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(haslo))
            {
                MessageBox.Show("Wprowadź login i hasło.");
                return;
            }

            var payload = new
            {
                login = login,
                haslo = haslo
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(apiUrl, content);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<LoginResponse>(responseBody);

                if (result != null && result.success)
                {
                    var user = result.user;

                    MainPanel panel = new MainPanel(user.Rola, user.Imie);
                    panel.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show(result?.message ?? "Błąd logowania");
                }
            }
            catch
            {
                MessageBox.Show("Błąd połączenia z serwerem.");
            }
        }
    }
}
