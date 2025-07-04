﻿namespace PrzychodniaAlfred.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Rola { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }

        public string? Specjalizacja { get; set; }

        public string FullName => $"{Imie} {Nazwisko}";
        public override string ToString()
        {
            return $"{Imie} {Nazwisko}";
        }
    }
}

