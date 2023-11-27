﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public class Pokladna
    {
        [Browsable(false)]
        public int Id { get; set; }

        [DisplayName("Číslo")]
        public string Cislo { get; set; }

        [Browsable(false)]
        public short Otevreno { get; set; }

        [DisplayName("Otevřeno")]
        public string OtevrenoText => Otevreno == 1 ? "Otevřeno" : "Zavřeno";

        [Browsable(false)]
        public short Automaticka { get; set; }

        [DisplayName("Automatická")]
        public string AutomatickaText => Automaticka == 1 ? "Automatická" : "Neautomatická";

        [DisplayName("Supermarket")]
        public Supermarket Supermarket { get; set; }

        public override string ToString()
        {
            if (Supermarket is null)
            {
                return $"Pokladna {Id}";
            }
            else
            {
                return $"{Cislo} - {Supermarket}";
            }
        }
    }
}
