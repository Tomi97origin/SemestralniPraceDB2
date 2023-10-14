using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    internal class InventarniPolozka
    {
        public int Id { get; set; }
        public short Sklad { get; set; }
        public int Mnozstvi{ get; set; }
        public string OznaceniPozice { get; set; }
        public DateTime Naskladneno { get; set; }
        public Supermarket Supermarket { get; set; }
        public Zbozi zbozi { get; set; }
    }
}
