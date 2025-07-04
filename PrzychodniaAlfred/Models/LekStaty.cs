﻿using PrzychodniaAlfred.Models;

namespace PrzychodniaAlfred.Statystyki
{
    public class LekStaty : Raport, InterfejsRaportu
    {
        public string Nazwa { get; set; }
        public int Wartosc { get; set; }

        public LekStaty(string nazwa, int liczbaWizyt)
        {
            Nazwa = nazwa;
            Wartosc = liczbaWizyt;
        }

        public string GenerujRaport()
        {
            return $"{Nazwa};{Wartosc} wizyt";
        }

        public override string ToString()
        {
            return $"{Nazwa} - Ilość wizyt:{Wartosc} ";
        }
    }
}
