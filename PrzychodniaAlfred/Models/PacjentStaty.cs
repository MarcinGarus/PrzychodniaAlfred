using PrzychodniaAlfred.Models;

namespace PrzychodniaAlfred.Statystyki
{
    public class PacjentStaty : Raport, InterfejsRaportu
    {
        public string Nazwa { get; set; }
        public int Wartosc { get; set; }

        public PacjentStaty(string nazwa, int wartosc)
        {
            Nazwa = nazwa;
            Wartosc = wartosc;
        }

        public string GenerujRaport()
        {
            return $"{Nazwa};{Wartosc} wizyt";
        }

        public override string ToString()
        {
            return $"{Nazwa} - Ilość wizyt:{Wartosc}";
        }
    }
}
