using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace SemestralniPraceDB2.Models.Entities
{
    public class Cena : IDBEntity
    {
        [Browsable(false)]
        public int Id { get; set; }

        [DisplayName("Platnost od")]
        public DateTime? PlatnostOd { get; set; }

        [DisplayName("Platnost do")]
        public DateTime? PlatnostDo { get; set; }

        [DisplayName("Částka")]
        public double Castka { get; set; }

        [DisplayName("Zboží")]
        public Zbozi? Zbozi { get; set; }

        public string DataToText()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"{PlatnostOd} ");
            sb.Append($"{PlatnostDo} ");
            sb.Append($"{Castka} ");
            sb.Append($"{Zbozi?.DataToText()} ");

            return sb.ToString().Trim();
        }


        public override string ToString()
        {
            return $"{Zbozi}, od {PlatnostOd:dd.MM.yyyy}, {Castka} Kč";
        }


    }
}
