using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public class Log : IDBEntity{ 

        public int Id { get; set; }

        public string Tabulka { get; set; }

        public string Operace { get; set; }

        public DateTime Cas { get; set; }

        public string Uzivatel { get; set; }

        public string Puvodni { get; set; }
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
