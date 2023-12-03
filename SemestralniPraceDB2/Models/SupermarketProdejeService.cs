using Oracle.ManagedDataAccess.Client;
using SemestralniPraceDB2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models
{
    public static class SupermarketProdejeService
    {

        public static List<ZboziStatisticka> ProdejeSupermarketu(Supermarket supermarket)
        {
            string sql = "SELECT * FROM SUPERMARKET_PRODANE_ZBOZI sp JOIN SUPERMARKETY s ON sp.id_supermarketu = s.id_supermarketu  JOIN ADRESY a ON s.id_adresy = a.id_adresy JOIN ZBOZI z ON sp.id_zbozi = z.id_zbozi WHERE s.id_supermarketu = :id_supermarketu";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_supermarketu", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = supermarket.Id;
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToZboziStatisticka).Result;
            return result;
        }

        private static ZboziStatisticka MapOracleResultToZboziStatisticka(OracleDataReader reader)
        {
            return new ZboziStatisticka()
            {
                Supermarket = SupermarketService.MapOracleResultToSupermarket(reader),
                ZboziNazev = reader.GetString("nazev"),
                CelkovaCena = reader.GetDouble("total_price"),
                CelkoveMnozstvi = reader.GetInt32("total_mnozstvi")
            };
        }
    }
}
