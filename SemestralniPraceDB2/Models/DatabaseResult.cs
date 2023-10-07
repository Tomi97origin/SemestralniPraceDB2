using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models
{
    public class DatabaseResult
    {
        public string? Text { get; set; } = "nothing yet";

        public override string ToString()
        {
            return "DatabaseResultDefaultToString";
        }
    }

}
