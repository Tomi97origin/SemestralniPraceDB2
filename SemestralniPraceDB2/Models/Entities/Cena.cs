using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public class Cena
    {
        [Browsable(false)]
        public int Id { get; set; }
        [DisplayName("Platnost od")]
        public DateTime PlatnostOd { get; set; }
        [DisplayName("Platnost do")]
        public DateTime? PlatnostDo { get; set; }
        [DisplayName("Částka")]
        public double Castka { get; set; }
        [DisplayName("Zboží")]
        public Zbozi? Zbozi { get; set; }


    }
}
