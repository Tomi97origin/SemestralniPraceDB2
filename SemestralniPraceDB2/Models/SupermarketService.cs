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
            PrepareProcedureCall(supermarket, out string procedureName, out List<OracleParameter> prm);
            prm[0].Value = null;
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm);
            return result.Result > 0;
        }

        public static void PrepareProcedureCall(Supermarket supermarket, out string procedureName, out List<OracleParameter> prm)
        {
            procedureName = "psupermarkety";
            prm = MapSupermarketIntoParams(supermarket);
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
            PrepareProcedureCall(supermarket, out string procedureName, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm);
            return result.Result > 0;
        }
        public static bool Delete(Supermarket supermarket)
        {
            PrepareDeleteCall(supermarket, out string sql, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(sql, prm, CommandType.Text).Result;
            return result == 1;
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
            string sql = "Select * FROM supermarkety WHERE id_supermarketu = :id_supermarketu";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_supermarketu", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = supermarket.Id;
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToSupermarket).Result;
            var super = result.Count == 0 ? null : result[0];
            if (super is null)
            {
                return super;
            }
            super.Adresa = AdresaService.Get(super.Adresa);
            return super;
        }

        private static Supermarket MapOracleResultToSupermarket(OracleDataReader reader)
        {
            return new Supermarket
            {
                Id = reader.GetInt32("id_supermarketu"),
                RozlohaProdejny = reader.GetDouble("rozloha_prodejny"),
                RozlohaSkladu = reader.GetDouble("rozloha_skladu"),
                ParkovaciMista = reader.GetInt32("parkovaci_mista"),
                Voziky = reader.GetInt32("voziky"),
                Adresa = new Adresa {
                    Id = reader.GetInt32("id_adresy")
                }
            };
        }

        public static List<Supermarket> GetAll()
        {
            string sql = "Select * FROM supermarkety";
            List<OracleParameter> prm = new();
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToSupermarket).Result;
            foreach (var p in result)
            {
                p.Adresa = AdresaService.Get(p.Adresa);
            }
            return result;
        }
    }
}
