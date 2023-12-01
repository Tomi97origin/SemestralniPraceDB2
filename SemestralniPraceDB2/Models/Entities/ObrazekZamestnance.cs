using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SemestralniPraceDB2.Models.Entities
{
    public class ObrazekZamestnance : IDBEntity
    {
        [Browsable(false)]
        public int Id { get; set; }

        [DisplayName("Soubor")]
        public string? Soubor { get; set; }

        [Browsable(false)]
        public Image? Image { get; set; }

        public string DataToText()
        {
            return string.Empty;
        }

        public override string ToString()
        {
            if(Soubor is null)
            {
                return $"Obrázek {Id}";
            }else
            {
                return $"{Soubor}";
            }
        }
    }
}
