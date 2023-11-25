using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public class Log { 

        public int Id { get; set; }

        public string Tabulka { get; set; }

        public string Operace { get; set; }

        public DateTime Cas { get; set; }

        public string Uzivatel { get; set; }
    }
}
