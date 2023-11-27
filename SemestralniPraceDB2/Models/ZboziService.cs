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
    public static class ZboziService
    {
        public static bool Create(Zbozi zbozi)
        {
            PrepareProcedureCall(zbozi, out string procedureName, out List<OracleParameter> prm);
            prm[0].Value = null;
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm);
            return result.Result > 0;
        }

        public static bool Update(Zbozi zbozi)
        {
            PrepareProcedureCall(zbozi, out string procedureName, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm);
            return result.Result > 0;
        }

        public static void PrepareProcedureCall(Zbozi zbozi, out string procedureName, out List<OracleParameter> prm)
        {
            procedureName = "pzbozi";
            prm = MapZboziIntoParams(zbozi);
        }

        public static bool Delete(Zbozi zbozi)
        {
            PrepareDeleteCall(zbozi, out string sql, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(sql, prm, CommandType.Text).Result;
            return result == 1;
        }

        private static void PrepareDeleteCall(Zbozi zbozi, out string sql, out List<OracleParameter> prm)
        {
            sql = "DELETE FROM zbozi WHERE id_zbozi = :id_zbozi";
            prm = new();
            prm.Add(new OracleParameter(":id_zbozi", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = zbozi.Id;
        }

        public static Zbozi? Get(Zbozi zbozi)
        {
            string sql = "SELECT * FROM zbozi WHERE id_zbozi = :id_zbozi";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_zbozi", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = zbozi.Id;
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToZbozi).Result;
            return result.Count == 0 ? null : result[0];
        }
        public static List<Zbozi> GetAll()
        {
            string sql = "SELECT * FROM zbozi";
            List<OracleParameter> prm = new();
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToZbozi).Result;
            return result;
        }

        private static List<OracleParameter> MapZboziIntoParams(Zbozi zbozi)
        {
            List<OracleParameter> prm = new List<OracleParameter>();

            prm.Add(new OracleParameter("p_id_zbozi", OracleDbType.Int32, System.Data.ParameterDirection.InputOutput));
            prm[0].Value = zbozi.Id <= 0 ? null : zbozi.Id;

            prm.Add(new OracleParameter("p_nazev", OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            prm[1].Value = zbozi.Nazev;

            prm.Add(new OracleParameter("p_popis", OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            prm[2].Value = zbozi.Popis;

            prm.Add(new OracleParameter("p_ean", OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            prm[3].Value = zbozi.EAN;

            prm.Add(new OracleParameter("p_id_kategorie", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[4].Value = zbozi.Kategorie.Id;

            prm.Add(new OracleParameter("p_id_vyrobce", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[5].Value = zbozi.Vyrobce.Id;

            return prm;
        }


        private static Zbozi MapOracleResultToZbozi(OracleDataReader result)
        {
            return new Zbozi()
            {
                Id = result.GetInt32("id_zbozi"),
                Nazev = result.GetString("nazev"),
                Popis = result.IsDBNull("popis") ? string.Empty : result.GetString("popis"),
                EAN = result.GetString("ean"),
                Kategorie = new Kategorie() { Id = result.GetInt32("id_kategorie") },
                Vyrobce = new Vyrobce() { Id = result.GetInt32("id_vyrobce") }
            };
        }
    }
}
