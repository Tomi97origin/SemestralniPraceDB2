using Oracle.ManagedDataAccess.Client;
using SemestralniPraceDB2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models
{
    public class InventarniPolozkaService
    {
        public static bool Create(InventarniPolozka polozka)
        {
            string procedureName = "pinventarni_polozky";
            List<OracleParameter> prm = MapInventarniPolozkaIntoParams(polozka);
            prm[0] = null;
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm);
            return result.Result == -1;
        }

        public static bool Update(InventarniPolozka polozka)
        {
            string procedureName = "pinventarni_polozky";
            List<OracleParameter> prm = MapInventarniPolozkaIntoParams(polozka);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm).Result;
            return result == -1;
        }
        public static bool Delete(InventarniPolozka polozka)
        {
            string sql = "DELETE FROM inventarni_polozky WHERE id_inventarni_polozky = :id_inventarni_polozky";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_inventarni_polozky", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = polozka.Id;
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(sql, prm, CommandType.Text).Result;
            return result == 1;
        }
        public static InventarniPolozka? Get(InventarniPolozka polozka)
        {
            string sql = "SELECT * FROM inventarni_polozky WHERE id_inventarni_polozky = :id_inventarni_polozky";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_inventarni_polozky", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = polozka.Id;
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToInventarniPolozka).Result;
            return result.Count == 0 ? null : result[0];
        }
        public static List<InventarniPolozka> GetAll()
        {
            string sql = "SELECT * FROM inventarni_polozky";
            List<OracleParameter> prm = new();
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToInventarniPolozka).Result;
            return result;
        }

        private static List<OracleParameter> MapInventarniPolozkaIntoParams(InventarniPolozka polozka)
        {
            List<OracleParameter> prm = new List<OracleParameter>();

            prm.Add(new OracleParameter("p_id_inventarni_polozky", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = polozka.Id <= 0 ? null : polozka.Id;

            prm.Add(new OracleParameter("p_sklad", OracleDbType.Int16, System.Data.ParameterDirection.Input));
            prm[1].Value = polozka.Sklad;

            prm.Add(new OracleParameter("p_mnozstvi", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[2].Value = polozka.Mnozstvi;

            prm.Add(new OracleParameter("p_oznaceni_pozice", OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            prm[3].Value = polozka.OznaceniPozice;

            prm.Add(new OracleParameter("p_naskladneno", OracleDbType.Date, System.Data.ParameterDirection.Input));
            prm[4].Value = polozka.Naskladneno;

            prm.Add(new OracleParameter("p_id_supermarketu", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[5].Value = polozka.Supermarket.Id; // Assuming Supermarket has an Id property

            prm.Add(new OracleParameter("p_id_zbozi", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[6].Value = polozka.Zbozi.Id; // Assuming Zbozi has an Id property

            return prm;
        }

        private static InventarniPolozka MapOracleResultToInventarniPolozka(OracleDataReader reader)
        {
            return new InventarniPolozka()
                {
                    Id = reader.GetInt32(0),
                    Sklad = reader.GetInt16(1),
                    Mnozstvi = reader.GetInt32(2),
                    OznaceniPozice = reader.GetString(3),
                    Naskladneno = reader.GetDateTime(4),
                    Supermarket = new Supermarket() { Id = reader.GetInt32(5) }, // Assuming Supermarket has an Id property
                    Zbozi = new Zbozi() { Id = reader.GetInt32(6) } // Assuming Zbozi has an Id property
                };
            
        }

    }
}
