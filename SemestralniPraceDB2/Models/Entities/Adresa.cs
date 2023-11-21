using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public class Adresa
    {
        [Browsable(false)]
        public int Id { get; set; }

        [DisplayName("Ulice")]
        public string? Ulice { get; set; }

        [DisplayName("ČP")]
        public int? Cp { get; set; }

        [DisplayName("Město")]
        public string? Mesto { get; set; }

        [DisplayName("Stát")]
        public string? Stat { get; set; }

        [DisplayName("PSČ")]
        public string? Psc { get; set; }

        override public string ToString()
        {
            return $"{Ulice} {Cp}, {Mesto} {Psc}, {Stat}";
        }

        public Adresa(int id, string? ulice, int? cp, string? mesto, string? stat, string? psc)
        {
            Id = id;
            Ulice = ulice;
            Cp = cp;
            Mesto = mesto;
            Stat = stat;
            Psc = psc;
        }

        public Adresa()
        {
        }
    }
}
