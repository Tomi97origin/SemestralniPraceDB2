using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public class Platba
    {
        public int Id { get; set; }
        public double Vraceno { get; set; }
        public short Debit { get; set; }
        public string CisloKarty { get; set; }
        public short Hotovost { get;}
        public Vydavatel? Vydavatel { get; set; }
        public Vernostni_karta? Vernostni_Karta { get; set; }
    }
}
