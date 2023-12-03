using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public class ZboziStatisticka
    {
        public Supermarket Supermarket { get; set; }
        public string ZboziNazev { get; set; }
        public int CelkoveMnozstvi { get; set; }
        public double CelkovaCena { get; set; }
    }
}
