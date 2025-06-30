using Microsoft.Win32;
using PrzychodniaAlfred.Models;
using PrzychodniaAlfred.Statystyki;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace PrzychodniaAlfred
{
    public partial class StatyWindow : Window
    {
        private List<InterfejsRaportu> raporty = new();

        public StatyWindow(List<InterfejsRaportu> raporty)
        {
            InitializeComponent();
            this.raporty = raporty;

            Loaded += StatyWindow_Loaded;
        }




        private void StatyWindow_Loaded(object sender, RoutedEventArgs e)
        {
            comboTypStatystyki.SelectedIndex = 0;
            listaStatystyk.Items.Clear();
            foreach (var r in raporty)
                listaStatystyk.Items.Add(r.GenerujRaport());
        }





        private void comboTyp(object sender, SelectionChangedEventArgs e)
        {
            if (comboTypStatystyki.SelectedItem is ComboBoxItem selectedItem && selectedItem.Content != null)
            {
                string typ = selectedItem.Content.ToString()!;
                if (typ == "Lekarze")
                    WczytajRaportLekarzy();
                else if (typ == "Pacjenci")
                    WczytajRaportPacjentow();
            }
        }

        private async void WczytajRaportLekarzy()
        {
            naglowekStatystyk.Text = "Statystyki lekarzy:";
            btnRaport.Content = "Raport - lekarze";
            listaStatystyk.Items.Clear();
            raporty.Clear();

            try
            {
                var http = new HttpClient();
                var json = await http.GetStringAsync("https://kineh.smallhost.pl/przychodnia/wszystkiewizyty.php");
                var wizyty = JsonSerializer.Deserialize<List<Wizyta>>(json) ?? new();

                var grupy = wizyty
                    .GroupBy(w => $"{w.LekarzImie} {w.LekarzNazwisko}")
                    .Select(g => new LekStaty(g.Key, g.Count()));

                foreach (var lekarz in grupy)
                {
                    raporty.Add(lekarz);
                    listaStatystyk.Items.Add(lekarz.GenerujRaport());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd pobierania danych:\n" + ex.Message);
            }
        }

        private async void WczytajRaportPacjentow()
        {
            naglowekStatystyk.Text = "Statystyki pacjentów:";
            btnRaport.Content = "Raport - pacjenci";
            listaStatystyk.Items.Clear();
            raporty.Clear();

            try
            {
                var http = new HttpClient();
                var json = await http.GetStringAsync("https://kineh.smallhost.pl/przychodnia/wszystkiewizyty.php");
                var wizyty = JsonSerializer.Deserialize<List<Wizyta>>(json) ?? new();

                var grupy = wizyty
                    .GroupBy(w => $"{w.PacjentImie} {w.PacjentNazwisko}")
                    .Select(g => new PacjentStaty(g.Key, g.Count()));

                foreach (var pacjent in grupy)
                {
                    raporty.Add(pacjent);
                    listaStatystyk.Items.Add(pacjent.GenerujRaport());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd pobierania danych:\n" + ex.Message);
            }
        }

        private void btnRaport_Click(object sender, RoutedEventArgs e)
        {
            if (listaStatystyk.Items.Count == 0)
            {
                MessageBox.Show("Brak danych do zapisania.");
                return;
            }

            var dialog = new SaveFileDialog
            {
                Filter = "Pliki tekstowe (*.txt)|*.txt",
                FileName = "raport.txt"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    var sb = new StringBuilder();
                    foreach (var item in listaStatystyk.Items)
                    {
                        sb.AppendLine(item.ToString());
                    }

                    File.WriteAllText(dialog.FileName, sb.ToString(), Encoding.UTF8);
                    MessageBox.Show("Zapisano pomyślnie!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd zapisu:\n" + ex.Message);
                }
            }
        }
    }
}
