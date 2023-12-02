using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace SemestralniPraceDB2.Models.Entities
{
    public class Log : IDBEntity{

        [Browsable(false)]
        public int Id { get; set; }

        [DisplayName("Tabulka")]
        public string Tabulka { get; set; }

        [DisplayName("Operace")]
        public string Operace { get; set; }

        [DisplayName("Čas")]
        public DateTime Cas { get; set; }

        [DisplayName("Uživatel")]
        public string Uzivatel { get; set; }

        [DisplayName("Původní")]
        public string Puvodni { get; set; }

        [DisplayName("Nové")]
        public string Nove { get; set; }

        public string DataToText()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"{Tabulka} ");
            sb.Append($"{Operace} ");
            sb.Append($"{Cas} ");
            sb.Append($"{Uzivatel} ");
            sb.Append($"{Puvodni} ");
            sb.Append($"{Nove} ");

            return sb.ToString().Trim();
        }

    }
}
