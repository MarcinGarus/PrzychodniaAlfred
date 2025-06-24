using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;
using PrzychodniaAlfred.Models;

namespace PrzychodniaAlfred
{
    public partial class PacjenciWindow : Window
    {
        public PacjenciWindow()
        {
            InitializeComponent();
            ZaladujPacjentow();
        }

        private async void ZaladujPacjentow()
        {
            try
            {
                using var http = new HttpClient();
                string url = "https://kineh.smallhost.pl/przychodnia/pobierzpacjentow.php";
                string json = await http.GetStringAsync(url);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var pacjenci = JsonSerializer.Deserialize<List<Pacjent>>(json, options);
                listaPacjentow.ItemsSource = pacjenci;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd pobierania pacjentów: " + ex.Message);
            }
        }

        private async void BtnDodajPacjenta_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtImie.Text) ||
                string.IsNullOrWhiteSpace(txtNazwisko.Text) ||
                string.IsNullOrWhiteSpace(txtPesel.Text) ||
                dpDataUrodzenia.SelectedDate == null)
            {
                MessageBox.Show("Uzupełnij wymagane pola: Imię, Nazwisko, PESEL, Data urodzenia.");
                return;
            }

            var pacjent = new
            {
                Imie = txtImie.Text,
                Nazwisko = txtNazwisko.Text,
                Pesel = txtPesel.Text,
                DataUrodzenia = dpDataUrodzenia.SelectedDate?.ToString("yyyy-MM-dd"),
                Telefon = txtTelefon.Text,
                Email = txtEmail.Text
            };

            try
            {
                using var http = new HttpClient();
                var json = JsonSerializer.Serialize(pacjent);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await http.PostAsync("https://kineh.smallhost.pl/przychodnia/dodajpacjenta.php", content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Pacjent został dodany.");
                    ClearForm();
                    ZaladujPacjentow();
                }
                else
                {
                    var blad = await response.Content.ReadAsStringAsync();
                    MessageBox.Show("Błąd podczas dodawania pacjenta:\n" + blad);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd połączenia z serwerem: " + ex.Message);
            }
        }

        private void ClearForm()
        {
            txtImie.Text = "";
            txtNazwisko.Text = "";
            txtPesel.Text = "";
            dpDataUrodzenia.SelectedDate = null;
            txtTelefon.Text = "";
            txtEmail.Text = "";
        }
    }
}
