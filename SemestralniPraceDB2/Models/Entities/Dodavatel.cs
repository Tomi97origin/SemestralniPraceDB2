using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public class Dodavatel : IDBEntity
    {
        [Browsable(false)]
        public int Id { get; set; }

        [DisplayName("Název")]
        public string Nazev { get; set; }

        [DisplayName("Adresa")]
        public Adresa Adresa { get; set; }

        public string DataToText()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"{Nazev} ");
            sb.Append($"{Adresa?.DataToText()} ");

            return sb.ToString().Trim();
        }


        public override string ToString()
        {
            if (Nazev is null)
            {
                return $"Dodavatel {Id}";
            }
            else
            {
                return Nazev;
            }
        }
    }
}
