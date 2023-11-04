using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public class ObjednaneZbozi
    {
        public int Mnozstvi { get; set; }
        public double Cena { get; set; }
        public Objednavka Objednavka { get; set; }
        public Zbozi Zbozi { get; set; }
    }
}
