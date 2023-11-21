using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public class Zbozi
    {
        [Browsable (false)]
        public int Id { get; set; }

        [DisplayName("Název")]
        public string Nazev { get; set; }

        [DisplayName("Popis")]
        public string Popis { get; set; }

        [DisplayName("EAN")]
        public string EAN { get; set; }

        [DisplayName("Kategorie")]
        public Kategorie Kategorie { get; set; }

        [DisplayName("Výrobce")]
        public Vyrobce Vyrobce { get; set; }
    }
}
