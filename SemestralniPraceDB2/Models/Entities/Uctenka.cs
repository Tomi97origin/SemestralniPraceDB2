﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public class Uctenka
    {
        [Browsable (false)]
        public int Id { get; set; }

        [DisplayName("Vytvořeno")]
        public DateTime Vytvoreno { get; set; }

        [DisplayName("Celková cena")]
        public double CelkovaCena { get; set; }

        [DisplayName("Pokladna")]
        public Pokladna Pokladna { get; set; }

        [DisplayName("Platba")]
        public Platba Platba { get; set; }
    }
}
