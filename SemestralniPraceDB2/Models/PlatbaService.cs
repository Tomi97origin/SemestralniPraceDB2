using Oracle.ManagedDataAccess.Client;
using SemestralniPraceDB2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models
{
    public static class PlatbaService
    {
        public static bool Create(Platba platba)
        {
            PrepareProcedureCall(platba, out string procedureName, out List<OracleParameter> prm);
            prm[0].Value = null;
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm);
            return result.Result > 0;
        }

        public static void PrepareProcedureCall(Platba platba, out string procedureName, out List<OracleParameter> prm)
        {
            procedureName = "pplatby";
            prm = MapPlatbaIntoParams(platba);
        }

        private static List<OracleParameter> MapPlatbaIntoParams(Platba platba)
        {
            List<OracleParameter> prm = new List<OracleParameter>();

            prm.Add(new OracleParameter("p_id_platby", OracleDbType.Int32, System.Data.ParameterDirection.InputOutput));
            prm[0].Value = platba.Id;

            prm.Add(new OracleParameter("p_vraceno", OracleDbType.Double, System.Data.ParameterDirection.Input));
            prm[1].Value = platba.Vraceno;

            prm.Add(new OracleParameter("p_debit", OracleDbType.Double, System.Data.ParameterDirection.Input));
            prm[2].Value = platba.Debit;

            prm.Add(new OracleParameter("p_cislo_karty", OracleDbType.Char, System.Data.ParameterDirection.Input));
            prm[3].Value = platba.CisloKarty;

            prm.Add(new OracleParameter("p_hotovost", OracleDbType.Int16, System.Data.ParameterDirection.Input));
            prm[4].Value = platba.Hotovost ? 1 : 0;

            prm.Add(new OracleParameter("p_id_vydavatele", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[5].Value = platba.Vydavatel is null ? null : platba.Vydavatel.Id;

            prm.Add(new OracleParameter("p_id_vernostni_karty", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[6].Value = platba.Vernostni_Karta is null ? null : platba.Vernostni_Karta.Id;

            return prm;
        }

        public static bool Update(Platba platba)
        {
            PrepareProcedureCall(platba, out string procedureName, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm);
            return result.Result > 0;
        }
        public static bool Delete(Platba platba)
        {
            PrepareDeleteCall(platba, out string sql, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(sql, prm, CommandType.Text).Result;
            return result == 1;
        }

        public static void PrepareDeleteCall(Platba platba, out string sql, out List<OracleParameter> prm)
        {
            sql = "DELETE FROM platby WHERE id_platby = :id_platby";
            prm = new();
            prm.Add(new OracleParameter(":id_platby", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = platba.Id;
        }

        public static Platba? Get(Platba platba)
        {
            string sql = "SELECT * FROM platby WHERE id_platby = :id_platby";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_platby", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = platba.Id;
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToPlatba).Result;
            return result.Count == 0 ? null : result[0];
        }

        private static Platba MapOracleResultToPlatba(OracleDataReader reader)
        {
            return new Platba()
            {
                Id = reader.GetInt32("id_platby"),
                Vraceno = reader.GetDouble("vraceno"),
                Debit = reader.IsDBNull("debit") ? null : reader.GetInt16("debit"),
                CisloKarty = reader.IsDBNull("cislo_karty") ? null : reader.GetString("cislo_karty"),
                Hotovost = reader.GetInt16("hotovost") == 1,
                Vydavatel = reader.IsDBNull("id_vydavatele") ? null : new Vydavatel() { Id = reader.GetInt32("id_vydavatele") },
                Vernostni_Karta = reader.IsDBNull("id_vernostni_karty") ? null : new Vernostni_karta() { Id = reader.GetInt32("id_vernostni_karty") }
            };
        }

        public static List<Platba> GetAll()
        {
            string sql = "SELECT * FROM platby";
            List<OracleParameter> prm = new();
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToPlatba).Result;
            return result;
        }

        public static List<Platba> GetPlatbyZakaznika(string cisloVernostniKarty)
        {
            string sql = "SELECT p.* FROM PLATBY p JOIN vernostni_karty v ON p.id_vernostni_karty = v.id_vernostni_karty WHERE v.cislo_karty =   :cislo_karty";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":cislo_karty", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = cisloVernostniKarty;
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToPlatba).Result;
            return result;
        }
    }
}
