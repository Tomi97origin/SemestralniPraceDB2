using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public class ZboziStatisticka
    {
        [Browsable(false)]
        public Supermarket Supermarket { get; set; }

        [DisplayName("Název zboží")]
        public string ZboziNazev { get; set; }

        [DisplayName("Celkové množství")]
        public int CelkoveMnozstvi { get; set; }

        [DisplayName("Celková cena")]
        public double CelkovaCena { get; set; }
    }
}
