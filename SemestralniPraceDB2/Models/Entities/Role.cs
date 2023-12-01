using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public class Role : IDBEntity
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
        public override string ToString()
        {
            if (Nazev == string.Empty)
            {
                return $"Role {Id}";
            }
            else
            {
                return Nazev;
            }
        }

        public string DataToText()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"{Nazev} ");

            return sb.ToString().Trim();
        }


    }
}
