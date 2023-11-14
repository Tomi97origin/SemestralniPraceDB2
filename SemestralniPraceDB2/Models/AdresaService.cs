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
    public class AdresaService
    {
        public static bool Create(Adresa adresa)
        {
            string procedureName = "padresy";
            List<OracleParameter> prm = MapAddressIntoParams(adresa);
            prm[0] = null;
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm);
            return result.Result == -1;
        }

        private static List<OracleParameter> MapAddressIntoParams(Adresa adresa)
        {
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter("p_id_adresy", OracleDbType.Int32, System.Data.ParameterDirection.Input));
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
            string procedureName = "padresy";
            List<OracleParameter> prm = MapAddressIntoParams(adresa);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm).Result;
            return result == -1;
        }
        public static bool Delete(Adresa adresa)
        {
            string sql = "DELETE FROM adresy WHERE id_adresy = :id_adresy";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_adresy", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = adresa.Id;
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(sql, prm, CommandType.Text).Result;
            return result == 1;
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

        private static Adresa MapOracleResultToAddress(OracleDataReader result)
        {
            return new Adresa()
            {
                Id = result.GetInt32(0),
                Ulice = result.GetString(1),
                Cp = result.GetInt32(2),
                Mesto = result.GetString(3),
                Stat = result.GetString(4),
                Psc = result.GetString(5)
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
