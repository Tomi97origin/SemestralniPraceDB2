using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public class Brigadnik : Zamestnanec
    {
        [DisplayName("Hodinová sazba")]
        public double? HodinovaSazba { set; get; }

        [DisplayName("Hodiny")]
        public double Hodiny { set; get; }

        public override string ToString()
        {

            return base.ToString();
            //if (Jmeno is null)
            //{
            //    return $"Brigádník {Id}";
            //}
            //else
            //{
            //    StringBuilder str = new();

            //    str.Append("Brigádník");
            //    str.Append($" {base.ToString()}");
            //    //if (HodinovaSazba is not null) { str.Append($", hodinová sazba: {HodinovaSazba}"); }
            //    //str.Append($", odpracované hodiny: {Hodiny}");

            //    return str.ToString();
            //}
        }
    }
}
