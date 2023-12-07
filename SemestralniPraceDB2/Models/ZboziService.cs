using Oracle.ManagedDataAccess.Client;
using SemestralniPraceDB2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection.PortableExecutable;
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
            using (OracleConnection connection = DatabaseConnector.GetConnection())
            {
                connection.Open();
                using (OracleTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        PreZboziDelete(zbozi, connection);
                        PrepareDeleteCall(zbozi, out string sql, out List<OracleParameter> prm);
                        var result = DatabaseConnector.ExecuteCommandNonQueryForTransactionAsync(sql, prm, connection, CommandType.Text).Result;
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        private static void PreZboziDelete(Zbozi zbozi, OracleConnection connection)
        {
            InventarniPolozkaService.DeleteFromZbozi(zbozi, connection);
            ObjednaneZboziService.DeleteFromZbozi(zbozi, connection);
            ProdaneZboziService.DeleteFromZbozi(zbozi, connection);
            CenaService.DeleteFromZbozi(zbozi, connection);
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


        internal static Zbozi MapOracleResultToZbozi(OracleDataReader result)
        {
            var collumns = result.GetColumnSchema().Select(col => col.ColumnName.ToLower()).ToList();
            return new Zbozi()
            {
                Id = result.GetInt32("id_zbozi"),
                Nazev = collumns.Contains("nazev") ? result.GetString("nazev") : string.Empty,
                Popis = collumns.Contains("popis") && !result.IsDBNull("popis") ? result.GetString("popis"): string.Empty,
                EAN = collumns.Contains("ean") ? result.GetString("ean") : string.Empty,
                Kategorie = !collumns.Contains("id_kategorie") ? null : new Kategorie() { Id = result.GetInt32("id_kategorie") },
                Vyrobce = !collumns.Contains("id_vyrobce") ? null : new Vyrobce() { Id = result.GetInt32("id_vyrobce") }
            };
        }

        public static bool ZlevniNejmeneProdavane()
        {
            string procedureName = "ZLEVNI_NEJMENE_PRODAVANE";
            List<OracleParameter> prm = new();
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm).Result;
            return result == -1;
        }

        internal static void DeleteFromKategorie(Kategorie kategorie, OracleConnection connection)
        {
            List<Zbozi> zbozi = GetFromKategorie(kategorie, connection);
            foreach(Zbozi obj in zbozi)
            {
                PreZboziDelete(obj, connection);
                PrepareDeleteCall(obj, out string sql, out List<OracleParameter> prm);
                var result = DatabaseConnector.ExecuteCommandNonQueryForTransactionAsync(sql, prm, connection, CommandType.Text).Result;
            }
        }

        private static List<Zbozi> GetFromKategorie(Kategorie kategorie, OracleConnection connection)
        {
            string sql = "SELECT * FROM zbozi WHERE id_kategorie = :id_kategorie";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_kategorie", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = kategorie.Id;
            var result = DatabaseConnector.ExecuteCommandQueryForTransactionAsync(sql, prm, connection, MapOracleResultToZbozi).Result;
            return result;
        }

        internal static void DeleteFromVyrobce(Vyrobce vyrobce, OracleConnection connection)
        {
            List<Zbozi> zbozi = GetFromVyrobce(vyrobce, connection);
            foreach (Zbozi obj in zbozi)
            {
                PreZboziDelete(obj, connection);
                PrepareDeleteCall(obj, out string sql, out List<OracleParameter> prm);
                var result = DatabaseConnector.ExecuteCommandNonQueryForTransactionAsync(sql, prm, connection, CommandType.Text).Result;
            }
        }

        private static List<Zbozi> GetFromVyrobce(Vyrobce vyrobce, OracleConnection connection)
        {
            string sql = "SELECT * FROM zbozi WHERE id_vyrobce = :id_vyrobce";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_vyrobce", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = vyrobce.Id;
            var result = DatabaseConnector.ExecuteCommandQueryForTransactionAsync(sql, prm, connection, MapOracleResultToZbozi).Result;
            return result;
        }
    }
}
