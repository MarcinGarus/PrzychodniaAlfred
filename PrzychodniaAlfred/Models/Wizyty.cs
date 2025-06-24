using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrzychodniaAlfred.Models
{
    public class Wizyta
    {
        public int Id { get; set; }
        public int IdPacjenta { get; set; }
        public int IdLekarza { get; set; }
        public DateTime Data { get; set; }
        public string? Opis { get; set; }
    }
}

