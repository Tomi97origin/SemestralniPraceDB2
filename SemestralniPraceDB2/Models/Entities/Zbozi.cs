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
        [Browsable(false)]
        public int Id { get; set; }

        [DisplayName("Název")]
        public string Nazev { get; set; }

        [DisplayName("Popis")]
        public string Popis { get; set; }

        [DisplayName("EAN")]
        public string EAN { get; set; }

        [DisplayName("Kategorie")]
        public Kategorie? Kategorie { get; set; }

        [DisplayName("Výrobce")]
        public Vyrobce? Vyrobce { get; set; }

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
            Kategorie = null;
            Vyrobce = null;
        }

        public override string ToString()
        {
            if (Nazev == string.Empty)
            {
                return $"Zboží {Id}";
            }
            else
            {
                StringBuilder str = new();
                str.Append(Nazev);
                if (Popis != string.Empty) { str.Append($", {Popis}"); }
                if (EAN != string.Empty) { str.Append($", {EAN}"); }
                return str.ToString();
            }
        }
    }
}
