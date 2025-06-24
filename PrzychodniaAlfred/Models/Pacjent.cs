using System;

namespace PrzychodniaAlfred.Models
{
    public class Pacjent
    {
        public int Id { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string Pesel { get; set; }
        public string DataUrodzenia { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }

        public override string ToString() => Nazwisko;
    }
}
