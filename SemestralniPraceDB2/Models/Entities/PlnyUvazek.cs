using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public class PlnyUvazek : Zamestnanec, IDBEntity
    {
        public double Plat { set; get; }
        public DateTime PlatnostDo { set; get; }

        public PlnyUvazek(Zamestnanec zamestnanec, double plat, DateTime platnostDo) : base(zamestnanec)
        {
            Plat = plat;
            PlatnostDo = platnostDo;
        }

        public new string DataToText()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"{base.DataToText()} ");
            sb.Append($"{Plat} ");
            sb.Append($"{PlatnostDo} ");

            return sb.ToString().Trim();
        }

        public PlnyUvazek()
        {
        }

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
