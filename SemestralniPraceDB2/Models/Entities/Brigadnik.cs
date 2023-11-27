using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public class Brigadnik : Zamestnanec
    {
        [DisplayName("Hodinová sazba")]
        public double? HodinovaSazba { set; get; }
        [DisplayName("Hodiny")]
        public double Hodiny { set; get; }

    }
}
