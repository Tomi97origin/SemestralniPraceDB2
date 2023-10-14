using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    internal class ObjednaneZbozi
    {
        public int Mnozstvi { get; set; }
        public double Cena { get; set; }
        public Objednavka objednavka { get; set; }
        public Zbozi zbozi { get; set; }
    }
}
