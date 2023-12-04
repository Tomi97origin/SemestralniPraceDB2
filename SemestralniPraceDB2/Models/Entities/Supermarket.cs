using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public class Supermarket : IDBEntity
    {
        [Browsable(false)]
        public int Id { get; set; }

        [DisplayName("Rozloha prodejny")]
        public double RozlohaProdejny { get; set; }

        [DisplayName("Rozloha skladu")]
        public double RozlohaSkladu { get; set; }

        [DisplayName("Parkovací místa")]
        public int ParkovaciMista { get; set; }

        [DisplayName("Vozíky")]
        public int Voziky { get; set; }

        [DisplayName("Adresa")]
        public Adresa Adresa { get; set; }

        [Browsable(false)]
        public string OznaceniProdejny
        {
            get
            {
                return ToString();
            }
        }


        public string DataToText()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"{RozlohaProdejny} ");
            sb.Append($"{RozlohaSkladu} ");
            sb.Append($"{ParkovaciMista} ");
            sb.Append($"{Voziky} ");
            sb.Append($"{Adresa?.DataToText()} ");

            return sb.ToString().Trim();
        }


        public override string ToString()
        {
            if(Adresa is null || Adresa.Mesto is null)
            {
                return $"Supermarket {Id}";
            }
            else
            {
                return $"Sup: {Adresa.Mesto}, {Adresa.Ulice}";
            }
        }
    }
}
