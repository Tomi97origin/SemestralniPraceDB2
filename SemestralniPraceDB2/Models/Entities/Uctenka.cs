using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    internal class Uctenka
    {
        public int Id { get; set; }
        public DateTime Vytvoreno { get; set; }
        public double CelkovaCena { get; set; }
        public Pokladna Pokladna { get; set; }
        public Platba Platba { get; set; }
    }
}
