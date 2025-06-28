using System;

namespace PrzychodniaAlfred.Models
{
    public class LekStaty : IStaty, IZapisywalny
    {
        public string NazwaLekarza { get; set; }
        public int LiczbaWizyt { get; set; }

        public string Kto => NazwaLekarza;
        public int Ile => LiczbaWizyt;

        public string DoPliku()
        {
            return $"{NazwaLekarza};{LiczbaWizyt} wizyt";
        }

        public override string ToString()
        {
            return $"{NazwaLekarza} - {LiczbaWizyt} wizyt";
        }
    }
}
