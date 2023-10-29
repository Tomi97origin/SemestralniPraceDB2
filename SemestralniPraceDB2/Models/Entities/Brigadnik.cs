using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public class Brigadnik : Zamestnanec
    {
        public double? HodinovaSazba { set; get; }
        public double Hodiny { set; get; }

    }
}
