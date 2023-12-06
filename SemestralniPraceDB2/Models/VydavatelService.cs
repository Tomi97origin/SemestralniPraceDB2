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
    public static class VydavatelService
    {
        public static bool Create(Vydavatel vydavatel)
        {
            PrepareProcedureCall(vydavatel, out string procedureName, out List<OracleParameter> prm);
            prm[0].Value = null;
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm);
            return result.Result > 0;
        }

        public static void PrepareProcedureCall(Vydavatel vydavatel, out string procedureName, out List<OracleParameter> prm)
        {
            procedureName = "pvydavatele";
            prm = MapVydavatelIntoParams(vydavatel);
        }

        private static List<OracleParameter> MapVydavatelIntoParams(Vydavatel vydavatel)
        {
            List<OracleParameter> prm = new List<OracleParameter>();

            prm.Add(new OracleParameter("p_id_vydavatele", OracleDbType.Int32, System.Data.ParameterDirection.InputOutput));
            prm[0].Value = vydavatel.Id <= 0 ? null : vydavatel.Id;

            prm.Add(new OracleParameter("p_nazev", OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            prm[1].Value = vydavatel.Nazev;

            return prm;
        }

        public static bool Update(Vydavatel vydavatel)
        {
            PrepareProcedureCall(vydavatel, out string procedureName, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm);
            return result.Result > 0;
        }
        public static bool Delete(Vydavatel vydavatel)
        {
            PrepareDeleteCall(vydavatel, out string sql, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(sql, prm, CommandType.Text).Result;
            return result == 1;
        }

        public static void PrepareDeleteCall(Vydavatel vydavatel, out string sql, out List<OracleParameter> prm)
        {
            sql = "DELETE FROM vydavatele WHERE id_vydavatele = :id_vydavatele";
            prm = new();
            prm.Add(new OracleParameter(":id_vydavatele", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = vydavatel.Id;
        }

        public static Vydavatel? Get(Vydavatel vydavatel)
        {
            string sql = "Select * FROM vydavatele WHERE id_vydavatele = :id_vydavatele";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_vydavatele", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = vydavatel.Id;
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToVydavatel).Result;
            return result.Count == 0 ? null : result[0];
        }

        private static Vydavatel MapOracleResultToVydavatel(OracleDataReader reader)
        {
            return new Vydavatel
            {
                Id = reader.GetInt32("id_vydavatele"),
                Nazev = reader.GetString("nazev")
            };
        }

        public static List<Vydavatel> GetAll()
        {
            string sql = "Select * FROM vydavatele";
            List<OracleParameter> prm = new();
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToVydavatel).Result;
            return result;
        }
    }
}
