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
    public class ObrazekZamestnance
    {
        [Browsable(false)]
        public int Id { get; set; }

        [DisplayName("Soubor")]
        public string? Soubor { get; set; }

        [DisplayName("Obrázek")]
        public Image? Image { get; set; }
    }
}
