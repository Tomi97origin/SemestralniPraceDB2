﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public class Platba : IDBEntity
    {
        [Browsable(false)]
        public int Id { get; set; }

        [DisplayName("Vráceno")]
        public double Vraceno { get; set; }

        [Browsable(false)]
        public short? Debit { get; set; }

        [DisplayName("Typ karty")]
        public string? DebitText => Debit == 1 ? "Debit" : "Kredit";

        [Browsable(false)]
        public string? CisloKarty { get; set; }

        [DisplayName("Číslo karty")]
        public string CisloKartyCensored => ReplaceAllExceptLastFour(CisloKarty?.Trim() ?? string.Empty, '#');

        [Browsable(false)]
        public bool Hotovost { get; set; }

        [DisplayName("Způsob platby")]
        public string HotovostText => Hotovost ? "Hotovost" : "Kartou";

        [DisplayName("Vydavatel")]
        public Vydavatel? Vydavatel { get; set; }

        [DisplayName("Věrnostní karta")]
        public Vernostni_karta? Vernostni_Karta { get; set; }

        public override string ToString()
        {
            if (Hotovost)
            {
                return $"Hotovost, vráceno {Vraceno}";
            }
            else if (CisloKarty is not null)
            {
                return $"Kartou {CisloKartyCensored}";
            }
            else return "Kartou";
        }
        private static string ReplaceAllExceptLastFour(string input, char replacementChar)
        {
            if (input.Length <= 4)
            {
                // Pokud je délka řetězce menší než nebo rovna 4, nemá smysl nic měnit.
                return new string(replacementChar, input.Length);
            }

            // Nahraď všechny znaky kromě posledních 4 znaků.
            string replaced = new string(replacementChar, input.Length - 4) + input.Substring(input.Length - 4);

            return replaced;
        }

        public string DataToText()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"{Vraceno} ");
            sb.Append($"{DebitText} ");
            sb.Append($"{CisloKartyCensored} ");
            sb.Append($"{HotovostText} ");
            sb.Append($"{Vydavatel?.DataToText()} ");
            sb.Append($"{Vernostni_Karta?.DataToText()} ");

            return sb.ToString().Trim();
        }
    }
}
