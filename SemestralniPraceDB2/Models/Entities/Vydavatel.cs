using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public class Vydavatel
    {
        [Browsable(false)]
        public int Id { get; set; }

        [DisplayName("Název")]
        public string Nazev { get; set; }

        public override string ToString()
        {
            if (Nazev is null)
            {
                return $"Vydavatel {Id}";
            }
            else
            {
                return Nazev;
            }
        }
    }
}
