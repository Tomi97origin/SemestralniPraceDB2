using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public class Pokladna
    {
        [Browsable(false)]
        public int Id { get; set; }

        [DisplayName("Číslo")]
        public string Cislo { get; set; }

        [DisplayName("Otevřeno")]
        public short Otevreno { get; set; }

        [DisplayName("Automatická")]
        public short Automaticka { get; set; }

        [DisplayName("Supermarket")]
        public Supermarket Supermarket { get; set; }
    }
}
