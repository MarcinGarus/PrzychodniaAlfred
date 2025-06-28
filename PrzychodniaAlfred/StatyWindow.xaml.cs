using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace PrzychodniaAlfred
{
    public partial class StatyWindow : Window
    {
        public StatyWindow(List<string> staty)
        {
            InitializeComponent();

            foreach (var stat in staty)
            {
                listaStatystyk.Items.Add(stat);
            }
        }

        private void btnZapiszStatystyki_Click(object sender, RoutedEventArgs e)
        {
            if (listaStatystyk.Items.Count == 0)
            {
                MessageBox.Show("Brak danych do zapisania.");
                return;
            }

            var dialog = new SaveFileDialog
            {
                Filter = "Pliki tekstowe (*.txt)|*.txt",
                FileName = "statystyki.txt"
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
                    MessageBox.Show("Błąd zapisu: " + ex.Message);
                }
            }
        }
    }
}
