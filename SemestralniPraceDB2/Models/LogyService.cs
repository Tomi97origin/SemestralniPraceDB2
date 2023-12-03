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
    public static class LogyService
    {
        public static bool Create(Log log)
        {
            PrepareProcedureCall(log, out string procedureName, out List<OracleParameter> prm);
            prm[0].Value = null;
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm);
            return result.Result > 0;
        }

        public static void PrepareProcedureCall(Log log, out string procedureName, out List<OracleParameter> prm)
        {
            procedureName = "plogy";
            prm = MapLogIntoParams(log);
        }

        private static List<OracleParameter> MapLogIntoParams(Log log)
        {
            List<OracleParameter> prm = new List<OracleParameter>();

            prm.Add(new OracleParameter("p_id", OracleDbType.Int32, System.Data.ParameterDirection.InputOutput));
            prm[0].Value = log.Id <= 0 ? null : log.Id;

            prm.Add(new OracleParameter("p_tabulka", OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            prm[1].Value = log.Tabulka;

            prm.Add(new OracleParameter("p_operace", OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            prm[2].Value = log.Operace;

            prm.Add(new OracleParameter("p_cas", OracleDbType.TimeStamp, System.Data.ParameterDirection.Input));
            prm[3].Value = log.Cas;

            prm.Add(new OracleParameter("p_uzivatel", OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            prm[4].Value = log.Uzivatel;
            return prm;
        }

        public static bool Update(Log log)
        {
            PrepareProcedureCall(log, out string procedureName, out List<OracleParameter> prm);
            prm[0].Value = null;
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm);
            return result.Result > 0;
        }
        public static bool Delete(Log log)
        {
            PrepareDeleteCall(log, out string sql, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(sql, prm, CommandType.Text).Result;
            return result == 1;
        }

        public static void PrepareDeleteCall(Log log, out string sql, out List<OracleParameter> prm)
        {
            sql = "DELETE FROM logy WHERE id = :id_logu";
            prm = new();
            prm.Add(new OracleParameter(":id_logu", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = log.Id;
        }

        public static Log? Get(Log log)
        {
            string sql = "SELECT * FROM logy WHERE id = :id_logu";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_logu", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = log.Id;
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToLog).Result;
            return result.Count == 0 ? null : result[0];
        }

        private static Log MapOracleResultToLog(OracleDataReader reader)
        {
            return new Log
            {
                Id = reader.GetInt32("id"),
                Tabulka = reader.GetString("tabulka"),
                Operace = reader.GetString("operace"),
                Cas = reader.GetDateTime("cas"),
                Uzivatel = reader.GetString("uzivatel"),
                Puvodni = reader.IsDBNull("puvodni") ? string.Empty : reader.GetString("puvodni"),
                Nove = reader.IsDBNull("nove") ? string.Empty : reader.GetString("nove")
            };
        }

        public static List<Log> GetAll()
        {
            string sql = "SELECT * FROM logy ORDER BY id";
            List<OracleParameter> prm = new();
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToLog).Result;
            return result;
        }
    }
}
