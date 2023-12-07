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
    public static class DodavatelService
    {
        public static bool Create(Dodavatel dodavatel)
        {
            dodavatel.Id = 0;
            return CallProcedureTransactional(dodavatel);
        }

        public static void PrepareProcedureCall(Dodavatel dodavatel, out string procedureName, out List<OracleParameter> prm)
        {
            procedureName = "pdodavatele";
            prm = MapDodavatelIntoParams(dodavatel);
        }

        private static bool CallProcedureTransactional(Dodavatel dodavatel)
        {
            using (OracleConnection connection = DatabaseConnector.GetConnection())
            {
                connection.Open();
                using (OracleTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string prom = "";
                        List<OracleParameter> param = new();
                        AdresaService.PrepareProcedureCall(dodavatel.Adresa, out prom, out param);
                        dodavatel.Adresa.Id = DatabaseConnector.ExecuteCommandNonQueryForTransactionAsync(prom, param, connection).Result;
                        PrepareProcedureCall(dodavatel, out string procedureName, out List<OracleParameter> prm);
                        var result = DatabaseConnector.ExecuteCommandNonQueryForTransactionAsync(procedureName, prm, connection);
                        transaction.Commit();
                        Console.WriteLine("Transaction committed successfully");
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine("Transaction rolled back due to an error: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        public static bool Update(Dodavatel dodavatel)
        {
            return CallProcedureTransactional(dodavatel);
        }
        public static bool Delete(Dodavatel dodavatel)
        {
            using (OracleConnection connection = DatabaseConnector.GetConnection())
            {
                connection.Open();
                using (OracleTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        ObjednavkaService.DeleteFromDodavatel(dodavatel, connection);
                        PrepareDeleteCall(dodavatel, out string sql, out List<OracleParameter> prm);
                        var result = DatabaseConnector.ExecuteCommandNonQueryForTransactionAsync(sql, prm,connection, CommandType.Text).Result;
                        AdresaService.PrepareDeleteCall(dodavatel.Adresa, out sql,out prm);
                        result = DatabaseConnector.ExecuteCommandNonQueryForTransactionAsync(sql, prm,connection, CommandType.Text).Result;
                        transaction.Commit();
                        Console.WriteLine("Transaction committed successfully");
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine("Transaction rolled back due to an error: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        public static void PrepareDeleteCall(Dodavatel dodavatel, out string sql, out List<OracleParameter> prm)
        {
            sql = "DELETE FROM dodavatele WHERE id_dodavatele = :id_dodavatele";
            prm = new();
            prm.Add(new OracleParameter(":id_dodavatele", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = dodavatel.Id;
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

            prm.Add(new OracleParameter("p_id_dodavatele", OracleDbType.Int32, System.Data.ParameterDirection.InputOutput));
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
                Id = reader.GetInt32("id_dodavatele"),
                Nazev = reader.GetString("nazev"),
                Adresa = new Adresa() { Id = reader.GetInt32("id_adresy") }
            };
        }

        internal static bool DeleteFromAdresa(Adresa adresa)
        {
            var dodavatel = GetFromAdresa(adresa);
            if (dodavatel != null)
            {
                return Delete(dodavatel);
            }
            return false;
        }

        private static Dodavatel GetFromAdresa(Adresa adresa)
        {
            string sql = "SELECT * FROM dodavatele WHERE id_adresy = :id_adresy";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_adresy", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = adresa.Id;
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToDodavatel).Result;
            return result.Count == 0 ? null : result[0];
        }
    }
}
