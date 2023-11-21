using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public class Role
    {
        [Browsable(false)]
        public int Id { get; set; }

        [DisplayName("Název")]
        public string Nazev { get; set; }

        public Role()
        {
            Id = 0;
            Nazev = "";
        }
        public Role(int id, string nazev)
        {
            Id = id;
            Nazev = nazev;
        }
    }
}
