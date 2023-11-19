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
    public static class VyrobceService
    {
        public static bool Create(Vyrobce vyrobce)
        {
            PrepareProcedureCall(vyrobce, out string procedureName, out List<OracleParameter> prm);
            prm[0].Value = null;
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm);
            return result.Result > 0;
        }

        public static void PrepareProcedureCall(Vyrobce vyrobce, out string procedureName, out List<OracleParameter> prm)
        {
            procedureName = "pvyrobce";
            prm = MapVyrobceIntoParams(vyrobce);
        }

        private static List<OracleParameter> MapVyrobceIntoParams(Vyrobce vyrobce)
        {
            List<OracleParameter> prm = new List<OracleParameter>();

            prm.Add(new OracleParameter("p_id_vyrobce", OracleDbType.Int32, System.Data.ParameterDirection.InputOutput));
            prm[0].Value = vyrobce.Id <= 0? null : vyrobce.Id;

            prm.Add(new OracleParameter("p_nazev", OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            prm[1].Value = vyrobce.Nazev;

            prm.Add(new OracleParameter("p_zkratka", OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            prm[2].Value = vyrobce.Zkratka;

            return prm;
        }

        public static bool Update(Vyrobce vyrobce)
        {
            PrepareProcedureCall(vyrobce, out string procedureName, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm);
            return result.Result > 0;
        }
        public static bool Delete(Vyrobce vyrobce)
        {
            PrepareDeleteCall(vyrobce, out string sql, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(sql, prm, CommandType.Text).Result;
            return result == 1;
        }

        public static void PrepareDeleteCall(Vyrobce vyrobce, out string sql, out List<OracleParameter> prm)
        {
            sql = "DELETE FROM vyrobci WHERE id_vyrobce = :id_vyrobce";
            prm = new();
            prm.Add(new OracleParameter(":id_vyrobce", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = vyrobce.Id;
        }

        public static Vyrobce? Get(Vyrobce vyrobce)
        {
            string sql = "Select * FROM vyrobci WHERE id_vyrobce = :id_vyrobce";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_dodavatele", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = vyrobce.Id;
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToVyrobce).Result;
            return result.Count == 0 ? null : result[0];
        }

        private static Vyrobce MapOracleResultToVyrobce(OracleDataReader reader)
        {
            return new Vyrobce
            {
                Id = reader.GetInt32("id_vyrobce"),
                Nazev = reader.GetString("nazev"),
                Zkratka = reader.GetString("zkratka")
            };
        }

        public static List<Vyrobce> GetAll()
        {
            string sql = "Select * FROM vyrobci";
            List<OracleParameter> prm = new();

            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToVyrobce).Result;
            return result;
        }
    }
}
