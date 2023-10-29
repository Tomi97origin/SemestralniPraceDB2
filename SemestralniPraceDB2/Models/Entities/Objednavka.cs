using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public class Objednavka
    {
        public int Id { get; set; }
        public DateTime Vytvoreno { get; set; }
        public DateTime? Splatnost { get; set; }
        public double? CelkovaCena { get; set; }
        public Supermarket Supermarket { get; set; }
        public Dodavatel Dodavatel { get; set; }

    }
}
