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
    public static class VernostniKartaService
    {
        public static bool Create(Vernostni_karta karta)
        {
            PrepareProcedureCall(karta, out string procedureName, out List<OracleParameter> prm);
            prm[0].Value = null;
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm);
            return result.Result > 0;
        }

        private static void PrepareProcedureCall(Vernostni_karta karta, out string procedureName, out List<OracleParameter> prm)
        {
            procedureName = "pvernostni_karty";
            prm = MapVernostni_kartaIntoParams(karta);
        }

        private static List<OracleParameter> MapVernostni_kartaIntoParams(Vernostni_karta karta)
        {
            List<OracleParameter> prm = new List<OracleParameter>();

            prm.Add(new OracleParameter("p_id_vernostni_karty", OracleDbType.Int32, System.Data.ParameterDirection.InputOutput));
            prm[0].Value = karta.Id <= 0 ? null : karta.Id;

            prm.Add(new OracleParameter("p_jmeno", OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            prm[1].Value = karta.Jmeno;

            prm.Add(new OracleParameter("p_zalozeni", OracleDbType.Date, System.Data.ParameterDirection.Input));
            prm[2].Value = karta.Zalozeni;

            prm.Add(new OracleParameter("p_cislo_karty", OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            prm[3].Value = karta.Cislo_Karty;

            return prm;
        }

        public static bool Update(Vernostni_karta karta)
        {
            PrepareProcedureCall(karta, out string procedureName, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm);
            return result.Result > 0;
        }
        public static bool Delete(Vernostni_karta karta)
        {
            PrepareDeleteCall(karta, out string sql, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(sql, prm, CommandType.Text).Result;
            return result == 1;
        }

        public static void PrepareDeleteCall(Vernostni_karta karta, out string sql, out List<OracleParameter> prm)
        {
            sql = "DELETE FROM vernostni_karty WHERE id_vernostni_karty = :id_vernostni_karty";
            prm = new();
            prm.Add(new OracleParameter(":id_vernostni_karty", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = karta.Id;
        }

        public static Vernostni_karta? Get(Vernostni_karta karta)
        {
            string sql = "Select * FROM vernostni_karty WHERE id_vernostni_karty = :id_vernostni_karty";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_vernostni_karty", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = karta.Id;
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToVernostni_karta).Result;
            return result.Count == 0 ? null : result[0];
        }

        private static Vernostni_karta MapOracleResultToVernostni_karta(OracleDataReader reader)
        {
            return new Vernostni_karta()
            {
                Id = reader.GetInt32("id_vernostni_karty"),
                Jmeno = reader.GetString("jmeno"),
                Zalozeni = reader.GetDateTime("zalozeni"),
                Cislo_Karty = reader.GetString("cislo_karty")
            };
        }

        public static List<Vernostni_karta> GetAll()
        {
            string sql = "Select * FROM vernostni_karty";
            List<OracleParameter> prm = new();
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToVernostni_karta).Result;
            return result;
        }
    }
}
