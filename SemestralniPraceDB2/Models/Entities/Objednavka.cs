using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public class Objednavka : IDBEntity
    {
        [Browsable(false)]
        public int Id { get; set; }

        [DisplayName("Vytvořeno")]
        public DateTime Vytvoreno { get; set; }

        [DisplayName("Splatnost")]
        public DateTime? Splatnost { get; set; }

        [DisplayName("Celková cena")]
        public double? CelkovaCena { get; set; }

        [DisplayName("Přijato")]
        public bool Prijato { get; set; } = false;

        [DisplayName("Supermarket")]
        public Supermarket Supermarket { get; set; }

        [DisplayName("Dodavatel")]
        public Dodavatel Dodavatel { get; set; }

        public Objednavka()
        {
            Id = 0;
            Vytvoreno = DateTime.Now;
            Splatnost = null;
            CelkovaCena = null;
            Supermarket = new();
            Dodavatel = new();
        }

        public Objednavka(int id, DateTime vytvoreno, DateTime? splatnost, double? celkovaCena, Supermarket supermarket, Dodavatel dodavatel)
        {
            Id = id;
            Vytvoreno = vytvoreno;
            Splatnost = splatnost;
            CelkovaCena = celkovaCena;
            Supermarket = supermarket;
            Dodavatel = dodavatel;
        }

        public override string ToString()
        {
            if (Dodavatel is not null && Dodavatel.Nazev is not null)
            {
                return $"{Dodavatel.Nazev} {Vytvoreno.ToString("dd. MM. yyyy")}";
            }else
            {
                return $"{Vytvoreno.ToString("dd. MM. yyyy")}";

            }
        }

        public string DataToText()
        {
            StringBuilder sb = new();

            sb.Append($"{Vytvoreno} ");
            sb.Append($"{Splatnost} ");
            sb.Append($"{CelkovaCena} ");
            sb.Append($"{Prijato} ");
            sb.Append($"{Supermarket.DataToText()} ");
            sb.Append($"{Dodavatel.DataToText()} ");

            return sb.ToString().Trim();
        }

    }
}
