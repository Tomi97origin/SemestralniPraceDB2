using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string  Nazev { get; set; }

        public Role(int id, string nazev)
        {
            Id = id;
            Nazev = nazev;
        }
    }
}
