using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public class Kategorie
    {
        [Browsable(false)]
        public int Id { get; set; }

        [DisplayName("Název")]
        public string Nazev { get; set; }

        [DisplayName("Zkratka")]
        public string? Zkratka { get; set; }

        public Kategorie(int id, string nazev, string zkratka)
        {
            Id = id;
            Nazev = nazev;
            Zkratka = zkratka;
        }

        public Kategorie()
        {
            Id = 0;
            Nazev = string.Empty;
            Zkratka = string.Empty;
        }
        public override string ToString()
        {
            if (Nazev == string.Empty)
            {
                return $"Kategorie {Id}";
            }
            else
            {
                StringBuilder str = new();

                str.Append(Nazev);
                if (Zkratka?.Length > 0 ) { str.Append($" ({Zkratka})"); }

                return str.ToString();
            }
        }
    }
}
