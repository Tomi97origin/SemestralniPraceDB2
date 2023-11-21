using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public class Platba
    {
        [Browsable(false)]
        public int Id { get; set; }

        [DisplayName("Vráceno")]
        public double Vraceno { get; set; }

        [DisplayName("Debit")]
        public short Debit { get; set; }

        [DisplayName("Číslo karty")]
        public string? CisloKarty { get; set; }

        [DisplayName("Hotovost")]
        public bool Hotovost { get; set; }

        [DisplayName("Vydavatel")]
        public Vydavatel? Vydavatel { get; set; }

        [DisplayName("Věrnostní karta")]
        public Vernostni_karta? Vernostni_Karta { get; set; }
    }
}
