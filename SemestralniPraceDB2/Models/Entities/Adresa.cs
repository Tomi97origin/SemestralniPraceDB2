using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SemestralniPraceDB2.Models.Entities
{
    public class Adresa:IDBEntity
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
            if (Ulice is  null)
            {
                return $"Adresa {Id}";
            }else
            {
                StringBuilder str = new();

                str.Append($"{Ulice}");

                if (Cp is not null) { str.Append($" {Cp}"); }
                if (Mesto is not null) { str.Append($", {Mesto}"); }
                if (Stat is not null) { str.Append($", {Stat}"); }
                if (Psc is not null) { str.Append($", {Psc}"); }
                
                return str.ToString();
            }
        }

        public string DataToText()
        {
            StringBuilder sb = new();

            sb.Append($"{Ulice} ");
            sb.Append($"{Cp} ");
            sb.Append($"{Mesto} ");
            sb.Append($"{Stat} ");
            sb.Append($"{Psc} ");

            return sb.ToString().Trim();
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
