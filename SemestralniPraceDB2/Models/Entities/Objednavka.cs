using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public class Objednavka
    {
        [Browsable(false)]
        public int Id { get; set; }

        [DisplayName("Vytvořeno")]
        public DateTime Vytvoreno { get; set; }

        [DisplayName("Splatnost")]
        public DateTime? Splatnost { get; set; }

        [DisplayName("Celková cena")]
        public double? CelkovaCena { get; set; }

        [DisplayName("Supermarket")]
        public Supermarket Supermarket { get; set; }

        [DisplayName("Dodavatel")]
        public Dodavatel Dodavatel { get; set; }

    }
}
