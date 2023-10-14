using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    internal class ProdaneZbozi
    {
        public int Mnozstvi { get; set; }
        public double Cena { get; set; }
        public Zbozi Zbozi { get; set; }
        public Uctenka Uctenka { get; set; }
    }
}
