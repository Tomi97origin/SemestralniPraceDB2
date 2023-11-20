using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public class Cena
    {
        public int Id { get; set; }
        public DateTime PlatnostOd { get; set; }
        public DateTime? PlatnostDo { get; set; }
        public double Castka { get; set; }
        public Zbozi? Zbozi { get; set; }


    }
}
