using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public class Uzivatel : IDBEntity
    {
        [Browsable(false)]
        public int Id { get; set; }

        [DisplayName("Uživatelské jméno")]
        public string Username { get; set; }

        [DisplayName("Heslo")]
        public string Password { get; set; }

        [DisplayName("Adminský účet")]
        public bool Admin { get; set; }

        [DisplayName("Aktivován")]
        public bool Active { get; set; }

        [DisplayName("Poslední přihlášení")]
        public DateTime PosledniPrihlaseni { get; set; }

        public string DataToText()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"{Username} ");
            sb.Append($"{Password} ");
            sb.Append($"{Admin} ");
            sb.Append($"{Active} ");
            sb.Append($"{PosledniPrihlaseni} ");

            return sb.ToString().Trim();
        }

    }
}
