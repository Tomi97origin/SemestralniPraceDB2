﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public class Adresa
    {
        public int Id { get; set; }
        public string? Ulice { get; set; }
        public int? Cp { get; set; }
        public string? Mesto { get; set; }
        public string? Stat { get; set; }
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
