using Oracle.ManagedDataAccess.Client;
using SemestralniPraceDB2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.DirectoryServices;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models
{
    public static class AdresaService
    {
        public static bool Create(Adresa adresa)
        {
            PrepareProcedureCall(adresa, out string procedureName, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm).Result;
            adresa.Id = result;
            return result > 0;
        }

        public static void PrepareProcedureCall(Adresa adresa, out string procedureName, out List<OracleParameter> prm)
        {
            procedureName = "padresy";
            prm = MapAddressIntoParams(adresa);
        }

        private static List<OracleParameter> MapAddressIntoParams(Adresa adresa)
        {
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter("p_id_adresy", OracleDbType.Int32, System.Data.ParameterDirection.InputOutput));
            prm[0].Value = adresa.Id <= 0 ? null : adresa.Id;
            prm.Add(new OracleParameter("p_ulice", OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            prm[1].Value = adresa.Ulice;
            prm.Add(new OracleParameter("p_cp", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[2].Value = adresa.Cp;
            prm.Add(new OracleParameter("p_mesto", OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            prm[3].Value = adresa.Mesto;
            prm.Add(new OracleParameter("p_stat", OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            prm[4].Value = adresa.Stat;
            prm.Add(new OracleParameter("p_psc", OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            prm[4].Value = adresa.Psc;
            return prm;
        }

        public static bool Update(Adresa adresa)
        {
            PrepareProcedureCall(adresa, out string procedureName, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm).Result;
            return result > 0;
        }
        public static bool Delete(Adresa adresa)
        {
            PrepareDeleteCall(adresa, out string sql, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(sql, prm, CommandType.Text).Result;
            return result == 1;
        }

        public static void PrepareDeleteCall(Adresa adresa, out string sql, out List<OracleParameter> prm)
        {
            sql = "DELETE FROM adresy WHERE id_adresy = :id_adresy";
            prm = new();
            prm.Add(new OracleParameter(":id_adresy", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = adresa.Id;
        }

        public static Adresa? Get(Adresa adresa)
        {
            string sql = "SELECT * FROM adresy WHERE id_adresy = :id_adresy";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_adresy", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = adresa.Id;
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToAddress).Result;
            return result.Count == 0 ? null : result[0];
        }

        public static Adresa? GetTransactional(Adresa adresa, OracleConnection connection)
        {
            string sql = "SELECT * FROM adresy WHERE id_adresy = :id_adresy";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_adresy", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = adresa.Id;
            var result = DatabaseConnector.ExecuteCommandQueryForTransactionAsync(sql, prm,connection, MapOracleResultToAddress).Result;
            return result.Count == 0 ? null : result[0];
        }

        internal static Adresa MapOracleResultToAddress(OracleDataReader result)
        {
            return new Adresa()
            {
                Id = result.GetInt32("id_adresy"),
                Ulice = result.GetString("ulice"),
                Cp = result.IsDBNull("cp") ? (int?)null : result.GetInt32("cp"),
                Mesto = result.GetString("mesto"),
                Stat = result.GetString("stat"),
                Psc = result.GetString("psc")
            };
        }

        public static List<Adresa> GetAll()
        {
            string sql = "SELECT * FROM adresy";
            List<OracleParameter> prm = new();
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToAddress).Result;
            return result;
        }
    }
}
