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
    public class ObjednavkaService
    {
        public static bool Create(Objednavka objednavka)
        {
            string procedureName = "pobjednavky";
            List<OracleParameter> prm = MapObjednavkaIntoParams(objednavka);
            prm[0].Value = null;
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm);
            return result.Result == -1;
        }

        public static bool Update(Objednavka objednavka)
        {
            string procedureName = "pobjednavky";
            List<OracleParameter> prm = MapObjednavkaIntoParams(objednavka);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm);
            return result.Result == -1;
        }
        public static bool Delete(Objednavka objednavka)
        {
            string sql = "DELETE FROM objednavky WHERE id_objednavky = :id_objednavky";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_objednavky", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = objednavka.Id;
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(sql, prm, CommandType.Text).Result;
            return result == 1;
        }
        public static Objednavka? Get(Objednavka objednavka)
        {
            string sql = "Select * FROM objednavky WHERE id_objednavky = :id_objednavky";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_objednavky", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = objednavka.Id;
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToObjednavka).Result;
            return result.Count == 0 ? null : result[0];
        }

        private static Objednavka MapOracleResultToObjednavka(OracleDataReader reader)
        {
            return new Objednavka()
            {
                Id = reader.GetInt32(0),
                Vytvoreno = reader.GetDateTime(1),
                Splatnost = reader.IsDBNull(2) ? (DateTime?)null : reader.GetDateTime(2),
                CelkovaCena = reader.IsDBNull(3) ? (double?)null : reader.GetDouble(3),
                Supermarket = new Supermarket() { Id = reader.GetInt32(4) },
                Dodavatel = new Dodavatel() { Id = reader.GetInt32(5) } 
            };
        }

        public static List<Objednavka> GetAll()
        {
            string sql = "Select * FROM objednavky";
            List<OracleParameter> prm = new();
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToObjednavka).Result;
            return result;
        }

        private static List<OracleParameter> MapObjednavkaIntoParams(Objednavka objednavka)
        {
            List<OracleParameter> prm = new List<OracleParameter>();

            prm.Add(new OracleParameter("p_id_objednavky", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = objednavka.Id;

            prm.Add(new OracleParameter("p_vytvoreno", OracleDbType.Date, System.Data.ParameterDirection.Input));
            prm[1].Value = objednavka.Vytvoreno;

            prm.Add(new OracleParameter("p_splatnost", OracleDbType.Date, System.Data.ParameterDirection.Input));
            prm[2].Value = objednavka.Splatnost;

            prm.Add(new OracleParameter("p_celkova_cena", OracleDbType.Double, System.Data.ParameterDirection.Input));
            prm[3].Value = objednavka.CelkovaCena;

            prm.Add(new OracleParameter("p_id_supermarketu", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[4].Value = objednavka.Supermarket.Id;

            prm.Add(new OracleParameter("p_id_dodavatele", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[5].Value = objednavka.Dodavatel.Id;

            return prm;
        }

    }
}
