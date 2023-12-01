using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public class Vydavatel : IDBEntity
    {
        [Browsable(false)]
        public int Id { get; set; }

        [DisplayName("Název")]
        public string Nazev { get; set; }

        public string DataToText()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"{Nazev} ");

            return sb.ToString().Trim();
        }


        public override string ToString()
        {
            if (Nazev is null)
            {
                return $"Vydavatel {Id}";
            }
            else
            {
                return Nazev;
            }
        }
    }
}
