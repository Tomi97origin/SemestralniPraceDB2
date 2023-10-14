using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    internal class Supermarket
    {
        public int Id { get; set; }
        public double RozlohaProdejny { get; set; }
        public double RozlohaSkladu { get; set; }
        public int ParkovaciMista { get; set; }
        public int Voziky { get; set; }
        public Adresa Adresa { get; set; }
    }
}
