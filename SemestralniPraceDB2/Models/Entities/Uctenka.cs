using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public class Uctenka : IDBEntity
    {
        [Browsable (false)]
        public int Id { get; set; }

        [DisplayName("Vytvořeno")]
        public DateTime Vytvoreno { get; set; }

        [DisplayName("Celková cena")]
        public double CelkovaCena { get; set; }

        [DisplayName("Pokladna")]
        public Pokladna Pokladna { get; set; }

        [DisplayName("Platba")]
        public Platba Platba { get; set; }

        public string DataToText()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"{Vytvoreno} ");
            sb.Append($"{CelkovaCena} ");
            sb.Append($"{Pokladna.DataToText()} ");
            sb.Append($"{Platba.DataToText()} ");

            return sb.ToString().Trim();
        }


        public override string ToString()
        {
            if (Vytvoreno == DateTime.MinValue)
            {
                return $"Účtenka {Id}";
            }else
            {
                StringBuilder str = new();
                
                str.Append(Vytvoreno.ToString("dd. MM. yyyy"));
                if (CelkovaCena != 0) str.Append($", {CelkovaCena} Kč") ;

                return str.ToString();
            }
        }
    }
}
