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
        public DateTime od { get; set; }
        public double cena { get; set; }
        public Zbozi zbozi { get; set; }


    }
}
