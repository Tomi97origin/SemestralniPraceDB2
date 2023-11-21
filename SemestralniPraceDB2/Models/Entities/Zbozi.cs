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

        public Zbozi(int id, string nazev, string popis, string eAN, Kategorie kategorie, Vyrobce vyrobce)
        {
            Id = id;
            Nazev = nazev;
            Popis = popis;
            EAN = eAN;
            Kategorie = kategorie;
            Vyrobce = vyrobce;
        }

        public Zbozi()
        {
            Id = 0;
            Nazev = string.Empty;
            Popis = string.Empty;
            EAN = string.Empty;
            Kategorie = new();
            Vyrobce = new();
        }

        public override string ToString()
        {
            return $"{Nazev}, {Popis}, {EAN}, {Kategorie}, {Vyrobce}";
        }
    }
}
