using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public class ObjednaneZbozi
    {
        [DisplayName("Množství")]
        public int Mnozstvi { get; set; }

        [DisplayName("Cena")]
        public double Cena { get; set; }

        [DisplayName("Objednávka")]
        public Objednavka Objednavka { get; set; }

        [DisplayName("Zboží")]
        public Zbozi Zbozi { get; set; }
    }
}
