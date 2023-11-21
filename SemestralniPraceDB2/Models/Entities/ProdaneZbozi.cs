using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public class ProdaneZbozi
    {
        [DisplayName("Množství")]
        public int Mnozstvi { get; set; }

        [DisplayName("Cena")]
        public double Cena { get; set; }

        [DisplayName("Zboží")]
        public Zbozi Zbozi { get; set; }

        [DisplayName("Účtenka")]
        public Uctenka Uctenka { get; set; }
    }
}
