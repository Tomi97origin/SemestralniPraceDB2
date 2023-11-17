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
    public class DodavatelService
    {
        public static bool Create(Dodavatel dodavatel)
        {
            string procedureName = "pdodavatele";
            List<OracleParameter> prm = MapDodavatelIntoParams(dodavatel);
            prm[0].Value = null;
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm);
            return result.Result == -1;
        }

        public static bool Update(Dodavatel dodavatel)
        {
            string procedureName = "pdodavatele";
            List<OracleParameter> prm = MapDodavatelIntoParams(dodavatel);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm).Result;
            return result == -1;
        }
        public static bool Delete(Dodavatel dodavatel)
        {
            string sql = "DELETE FROM dodavatele WHERE id_dodavatele = :id_dodavatele";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_dodavatele", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = dodavatel.Id;
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(sql, prm, CommandType.Text).Result;
            return result == 1;
        }
        public static Dodavatel? Get(Dodavatel dodavatel)
        {
            string sql = "SELECT * FROM dodavatele WHERE id_dodavatele = :id_dodavatele";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_dodavatele", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = dodavatel.Id;
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToDodavatel).Result;
            return result.Count == 0 ? null : result[0];
        }

        public static List<Dodavatel> GetAll()
        {
            string sql = "SELECT * FROM dodavatele";
            List<OracleParameter> prm = new();
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToDodavatel).Result;
            return result;
        }

        private static List<OracleParameter> MapDodavatelIntoParams(Dodavatel dodavatel)
        {
            List<OracleParameter> prm = new List<OracleParameter>();

            prm.Add(new OracleParameter("p_id_dodavatele", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = dodavatel.Id <= 0 ? null : dodavatel.Id;

            prm.Add(new OracleParameter("p_nazev", OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            prm[1].Value = dodavatel.Nazev;

            prm.Add(new OracleParameter("p_id_adresy", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[2].Value = dodavatel.Adresa.Id; 

            return prm;
        }

        private static Dodavatel MapOracleResultToDodavatel(OracleDataReader reader)
        {
            return new Dodavatel()
            {
                Id = reader.GetInt32(0),
                Nazev = reader.GetString(1),
                Adresa = new Adresa() { Id = reader.GetInt32(2) }
            };
        }


    }
}
