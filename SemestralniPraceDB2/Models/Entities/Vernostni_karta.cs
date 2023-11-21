using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public class Vernostni_karta
    {
        [Browsable(false)]
        public int Id { get; set; }

        [DisplayName("Jméno")]
        public string Jmeno { get; set; }

        [DisplayName("Založení")]
        public DateTime Zalozeni { get; set; }

        [DisplayName("Číslo karty")]
        public string Cislo_Karty { get; set; }
    }
}
