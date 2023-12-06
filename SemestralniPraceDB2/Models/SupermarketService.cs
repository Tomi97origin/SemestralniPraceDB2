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
    public static class SupermarketService
    {
        public static bool Create(Supermarket supermarket)
        {
            supermarket.Id = 0;
            return ProcedureCallTransactional(supermarket);
        }

        public static void PrepareProcedureCall(Supermarket supermarket, out string procedureName, out List<OracleParameter> prm)
        {
            procedureName = "psupermarkety";
            prm = MapSupermarketIntoParams(supermarket);
        }

        private static bool ProcedureCallTransactional(Supermarket supermarket)
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
                        AdresaService.PrepareProcedureCall(supermarket.Adresa,out prom,out param);
                        supermarket.Adresa.Id = DatabaseConnector.ExecuteCommandNonQueryForTransactionAsync(prom, param, connection).Result;
                        PrepareProcedureCall(supermarket, out string procedureName, out List<OracleParameter> prm);
                        var result = DatabaseConnector.ExecuteCommandNonQueryForTransactionAsync(procedureName, prm,connection);
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

        private static List<OracleParameter> MapSupermarketIntoParams(Supermarket supermarket)
        {
            List<OracleParameter> prm = new List<OracleParameter>();

            prm.Add(new OracleParameter("p_id_supermarketu", OracleDbType.Int32, System.Data.ParameterDirection.InputOutput));
            prm[0].Value = supermarket.Id <= 0 ? null : supermarket.Id;

            prm.Add(new OracleParameter("p_rozloha_prodejny", OracleDbType.Double, System.Data.ParameterDirection.Input));
            prm[1].Value = supermarket.RozlohaProdejny;

            prm.Add(new OracleParameter("p_rozloha_skladu", OracleDbType.Double, System.Data.ParameterDirection.Input));
            prm[2].Value = supermarket.RozlohaSkladu;

            prm.Add(new OracleParameter("p_parkovaci_mista", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[3].Value = supermarket.ParkovaciMista;

            prm.Add(new OracleParameter("p_voziky", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[4].Value = supermarket.Voziky;

            prm.Add(new OracleParameter("p_id_adresy", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[5].Value = supermarket.Adresa.Id;

            return prm;
        }

        public static bool Update(Supermarket supermarket)
        {
            return ProcedureCallTransactional(supermarket);
        }
        public static bool Delete(Supermarket supermarket)
        {
            using (OracleConnection connection = DatabaseConnector.GetConnection())
            {
                connection.Open();
                using (OracleTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        PrepareDeleteCall(supermarket, out string sql, out List<OracleParameter> prm);
                        var result = DatabaseConnector.ExecuteCommandNonQueryAsync(sql, prm, CommandType.Text).Result;
                        AdresaService.PrepareDeleteCall(supermarket.Adresa, out sql, out prm);
                        result = DatabaseConnector.ExecuteCommandNonQueryAsync(sql, prm, CommandType.Text).Result;
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

        public static void PrepareDeleteCall(Supermarket supermarket, out string sql, out List<OracleParameter> prm)
        {
            sql = "DELETE FROM supermarkety WHERE id_supermarketu = :id_supermarketu";
            prm = new();
            prm.Add(new OracleParameter(":id_supermarketu", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = supermarket.Id;
        }

        public static Supermarket? Get(Supermarket supermarket)
        {
            string sql = "Select * FROM supermarkety JOIN adresy USING(id_adresy) WHERE id_supermarketu = :id_supermarketu";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_supermarketu", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = supermarket.Id;
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToSupermarket).Result;
            return result.Count == 0 ? null : result[0];
        }

        internal static Supermarket MapOracleResultToSupermarket(OracleDataReader reader)
        {
            return new Supermarket
            {
                Id = reader.GetInt32("id_supermarketu"),
                RozlohaProdejny = reader.GetDouble("rozloha_prodejny"),
                RozlohaSkladu = reader.GetDouble("rozloha_skladu"),
                ParkovaciMista = reader.GetInt32("parkovaci_mista"),
                Voziky = reader.GetInt32("voziky"),
                Adresa = AdresaService.MapOracleResultToAddress(reader)
            };
        }

        public static List<Supermarket> GetAll()
        {
            string sql = "Select * FROM supermarkety JOIN adresy USING(id_adresy)";
            List<OracleParameter> prm = new();
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToSupermarket).Result;
            return result;
        }

        public static List<Supermarket> GetAllInOrderOfSales()
        {
            string sql = "Select id_supermarketu,rozloha_prodejny,rozloha_skladu,parkovaci_mista,voziky,a.id_adresy,ulice,cp,mesto,stat,psc,SALES_SUPERMARKET_ORDER(id_supermarketu) as prodeje  FROM supermarkety s JOIN adresy a ON s.id_adresy = a.id_adresy ORDER BY prodeje";
            List<OracleParameter> prm = new();
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToSupermarket).Result;
            return result;
        }
    }
}
