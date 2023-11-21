using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public class InventarniPolozka
    {
        [Browsable(false)]
        public int Id { get; set; }

        [DisplayName("Sklad")]
        public short Sklad { get; set; }

        [DisplayName("Množství")]
        public int Mnozstvi{ get; set; }

        [DisplayName("Označení pozice")]
        public string OznaceniPozice { get; set; }

        [DisplayName("Naskladněno")]
        public DateTime Naskladneno { get; set; }

        [DisplayName("Supermarket")]
        public Supermarket Supermarket { get; set; }

        [DisplayName("Zboží")]
        public Zbozi Zbozi { get; set; }
    }
}
