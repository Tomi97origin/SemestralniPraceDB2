﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public class PlnyUvazek : Zamestnanec
    {
        public double Plat { set; get; }
        public DateTime PlatnostDo { set; get; }
        public override string ToString()
        {
            return base.ToString();
            //if (Jmeno is null)
            //{
            //    return $"Plný úvazek {Id}";
            //}
            //else
            //{
            //    StringBuilder str = new();

            //    str.Append("Plný úvazek");
            //    str.Append($" {base.ToString()}");
            //    //str.Append($", plat: {Plat}");
            //    //str.Append($", platnost do: {PlatnostDo.ToString("dd. MM. yyyy")}");

            //    return str.ToString();
            //}
        }

    }
}
