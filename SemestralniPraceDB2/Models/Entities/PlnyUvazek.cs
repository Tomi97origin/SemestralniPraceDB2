using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    internal class PlnyUvazek : Zamestnanec
    {
        public double Plat { set; get; }
        public DateTime PlatnostDo { set; get; }
    }
}
