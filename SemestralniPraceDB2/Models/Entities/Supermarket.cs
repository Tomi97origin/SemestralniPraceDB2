using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public class Supermarket
    {
        [Browsable(false)]
        public int Id { get; set; }

        [DisplayName("Rozloha prodejny")]
        public double RozlohaProdejny { get; set; }

        [DisplayName("Rozloha skladu")]
        public double RozlohaSkladu { get; set; }

        [DisplayName("Parkovací místa")]
        public int ParkovaciMista { get; set; }

        [DisplayName("Vozíky")]
        public int Voziky { get; set; }

        [DisplayName("Adresa")]
        public Adresa Adresa { get; set; }

        public override string ToString()
        {
            return $"Sup: {Adresa.Mesto}, {Adresa.Ulice}";
        }
    }
}
