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
    public static class KategorieService
    {
        public static bool Create(Kategorie kategorie)
        {
            PrepareProcedureCall(kategorie, out string procedureName, out List<OracleParameter> prm);
            prm[0].Value = null;
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm);
            return result.Result > 0;
        }

        public static void PrepareProcedureCall(Kategorie kategorie, out string procedureName, out List<OracleParameter> prm)
        {
            procedureName = "pkategorie";
            prm = MapKategorieIntoParams(kategorie);
        }

        public static bool Update(Kategorie kategorie)
        {
            PrepareProcedureCall(kategorie, out string procedureName, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm);
            return result.Result > 0;
        }
        public static bool Delete(Kategorie kategorie)
        {
            PrepareDeleteCall(kategorie, out string sql, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(sql, prm, CommandType.Text).Result;
            return result == 1;
        }

        public static void PrepareDeleteCall(Kategorie kategorie, out string sql, out List<OracleParameter> prm)
        {
            sql = "DELETE FROM kategorie WHERE id_kategorie = :id_kategorie";
            prm = new();
            prm.Add(new OracleParameter(":id_kategorie", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = kategorie.Id;
        }

        public static Kategorie? Get(Kategorie kategorie)
        {
            string sql = "SELECT * FROM kategorie WHERE id_kategorie = :id_kategorie";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_kategorie", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = kategorie.Id;
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToKategorie).Result;
            return result.Count == 0 ? null : result[0];
        }

        public static Kategorie? GetTransactional(Kategorie kategorie, OracleConnection connection)
        {
            string sql = "SELECT * FROM kategorie WHERE id_kategorie = :id_kategorie";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_kategorie", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = kategorie.Id;
            var result = DatabaseConnector.ExecuteCommandQueryForTransactionAsync(sql, prm,connection, MapOracleResultToKategorie).Result;
            return result.Count == 0 ? null : result[0];
        }

        private static Kategorie MapOracleResultToKategorie(OracleDataReader reader)
        {
            return new Kategorie()
            {
                Id = reader.GetInt32("id_kategorie"),
                Nazev = reader.GetString("nazev"),
                Zkratka = reader.IsDBNull("zkratka") ? null : reader.GetString("zkratka")
            };
        }

        public static List<Kategorie> GetAll()
        {
            string sql = "SELECT * FROM kategorie";
            List<OracleParameter> prm = new();
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToKategorie).Result;
            return result;
        }

        private static List<OracleParameter> MapKategorieIntoParams(Kategorie kategorie)
        {
            List<OracleParameter> prm = new List<OracleParameter>();

            prm.Add(new OracleParameter("p_id_kategorie", OracleDbType.Int32, System.Data.ParameterDirection.InputOutput));
            prm[0].Value = kategorie.Id <= 0 ? null : kategorie.Id;

            prm.Add(new OracleParameter("p_nazev", OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            prm[1].Value = kategorie.Nazev;

            prm.Add(new OracleParameter("p_zkratka", OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            prm[2].Value = kategorie.Zkratka;

            return prm;
        }

    }
}
