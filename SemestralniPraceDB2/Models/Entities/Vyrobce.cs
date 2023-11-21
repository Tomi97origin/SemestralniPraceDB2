using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public class Vyrobce
    {
        [Browsable (false)]
        public int Id { get; set; }

        [DisplayName("Název")]
        public string Nazev { get; set; }

        [DisplayName("Zkratka")]
        public string Zkratka { get; set; }

        public Vyrobce(int id, string nazev, string zkratka)
        {
            Id = id;
            Nazev = nazev;
            Zkratka = zkratka;
        }

        public Vyrobce()
        {
            Id = 0;
            Nazev = string.Empty;
            Zkratka = string.Empty;
        }
        public override string ToString()
        {
            return $"{Nazev}({Zkratka})";
        }
    }
}
