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
    public static class InventarniPolozkaService
    {
        public static bool Create(InventarniPolozka polozka)
        {
            PrepareProcedureCall(polozka, out string procedureName, out List<OracleParameter> prm);
            prm[0].Value = null;
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm).Result;
            return result > 0;
        }

        public static void PrepareProcedureCall(InventarniPolozka polozka, out string procedureName, out List<OracleParameter> prm)
        {
            procedureName = "pinventarni_polozky";
            prm = MapInventarniPolozkaIntoParams(polozka);
        }

        public static bool Update(InventarniPolozka polozka)
        {
            PrepareProcedureCall(polozka, out string procedureName, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm).Result;
            return result > 0;
        }
        public static bool Delete(InventarniPolozka polozka)
        {
            PrepareDeleteCall(polozka, out string sql, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(sql, prm, CommandType.Text).Result;
            return result == 1;
        }

        public static void PrepareDeleteCall(InventarniPolozka polozka, out string sql, out List<OracleParameter> prm)
        {
            sql = "DELETE FROM inventarni_polozky WHERE id_inventarni_polozky = :id_inventarni_polozky";
            prm = new();
            prm.Add(new OracleParameter(":id_inventarni_polozky", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = polozka.Id;
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

            prm.Add(new OracleParameter("p_id_inventarni_polozky", OracleDbType.Int32, System.Data.ParameterDirection.InputOutput));
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
            prm[5].Value = polozka.Supermarket.Id;

            prm.Add(new OracleParameter("p_id_zbozi", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[6].Value = polozka.Zbozi.Id;

            return prm;
        }

        private static InventarniPolozka MapOracleResultToInventarniPolozka(OracleDataReader reader)
        {
            return new InventarniPolozka()
            {
                Id = reader.GetInt32("id_inventarni_polozky"),
                Sklad = reader.GetInt16("sklad"),
                Mnozstvi = reader.GetInt32("mnozstvi"),
                OznaceniPozice = reader.GetString("oznaceni_pozice"),
                Naskladneno = reader.GetDateTime("naskladneno"),
                Supermarket = new Supermarket() { Id = reader.GetInt32("id_supermarketu") },
                Zbozi = new Zbozi() { Id = reader.GetInt32("id_zbozi") }
            };

        }

    }
}
