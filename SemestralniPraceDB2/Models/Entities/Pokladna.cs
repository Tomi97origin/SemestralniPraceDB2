using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    internal class Pokladna
    {
        public int Id { get; set; }
        public string Cislo { get; set; }
        public short Otevreno { get; set; }
        public short Automaticka { get; set; }
        public Supermarket Supermarket { get; set; }
    }
}
