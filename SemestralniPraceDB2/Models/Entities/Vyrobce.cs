using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public class Vyrobce : IDBEntity
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

            if (Nazev == string.Empty)
            {
                return $"Výrobce {Id}";
            }
            else
            {
                StringBuilder str = new();

                str.Append(Nazev);
                if (Zkratka != string.Empty) { str.Append($" ({Zkratka})"); }

                return str.ToString();
            }

        }

        public string DataToText()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"{Nazev} ");
            sb.Append($"{Zkratka} ");

            return sb.ToString().Trim();
        }

    }
}
