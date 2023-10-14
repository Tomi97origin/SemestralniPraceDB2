using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    internal class Dodavatel
    {
        public int Id { get; set; }
        public string Nazev { get; set; }
        public Adresa adresa { get; set; }

    }
}
