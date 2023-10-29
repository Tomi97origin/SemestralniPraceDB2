using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public class Vernostni_karta
    {
        public int Id { get; set; }
        public string Jmeno { get; set; }
        public DateTime Zalozeni { get; set; }
        public string Cislo_Karty { get; set; }
    }
}
