using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SemestralniPraceDB2.Models.Entities
{
    public class ObrazekZamestnance
    {
        public int Id { get; set; }
        public string Soubor { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
        public Image Image { get; set; }
    }
}
