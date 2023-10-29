using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public class Zbozi
    {
        public int Id { get; set; }
        public string Nazev { get; set; }
        public string Popis { get; set; }
        public string EAN { get; set; }
        public Kategorie Kategorie { get; set; }
        public Vyrobce Vyrobce { get; set; }
    }
}
