using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    internal class Adresa
    {
       public int Id { get; set; }
        public string? Ulice { get; set; }
        public int? Cp { get; set; }
        public string? Mesto { get; set; }
        public string? Stat { get; set; }
        public string? Psc { get; set; }


    }
}
