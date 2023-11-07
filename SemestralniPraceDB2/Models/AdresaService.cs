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
        public bool Create(Adresa adresa)
        {
            string procedureName = "InsertAddress";
            OracleParameter[] prm = MapAddressIntoParams(adresa);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm);
            return result.Result == -1;
        }

        private OracleParameter[] MapAddressIntoParams(Adresa adresa)
        {
            OracleParameter[] prm = new OracleParameter[6];
            prm[0] = new OracleParameter("id_adresy", OracleDbType.Int32, System.Data.ParameterDirection.Input);
            prm[0].Value = adresa.Id;
            prm[1] = new OracleParameter("ulice", OracleDbType.Varchar2, System.Data.ParameterDirection.Input);
            prm[1].Value = adresa.Ulice;
            prm[2] = new OracleParameter("cp", OracleDbType.Int32, System.Data.ParameterDirection.Input);
            prm[2].Value = adresa.Cp;
            prm[3] = new OracleParameter("mesto", OracleDbType.Varchar2, System.Data.ParameterDirection.Input);
            prm[3].Value = adresa.Mesto;
            prm[4] = new OracleParameter("stat", OracleDbType.Varchar2, System.Data.ParameterDirection.Input);
            prm[4].Value = adresa.Stat;
            prm[4] = new OracleParameter("psc", OracleDbType.Varchar2, System.Data.ParameterDirection.Input);
            prm[4].Value = adresa.Psc;
            return prm;
        }

        public bool Update(Adresa adresa)
        {
            string procedureName = "UpdateAddress";
            OracleParameter[] prm = MapAddressIntoParams(adresa);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm).Result;
            return result == -1;
        }
        public bool Delete(Adresa adresa)
        {
            string sql = "DELETE FROM adresy WHERE id_adresy = :id_adresy";
            OracleParameter[] prm = new OracleParameter[6];
            prm[0] = new OracleParameter(":id_adresy", OracleDbType.Int32, System.Data.ParameterDirection.Input);
            prm[0].Value = adresa.Id;
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(sql, prm, CommandType.Text).Result;
            return result == -1;
        }
        public Adresa? Get(Adresa adresa)
        {
            string sql = "SELECT * FROM adresy WHERE id_adresy = :id_adresy";
            OracleParameter[] prm = new OracleParameter[6];
            prm[0] = new OracleParameter(":id_adresy", OracleDbType.Int32, System.Data.ParameterDirection.Input);
            prm[0].Value = adresa.Id;
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToAddress).Result;
            return result.Count == 0 ? null : result[0];
        }

        private Adresa MapOracleResultToAddress(OracleDataReader result)
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

        public List<Adresa> GetAll()
        {
            string sql = "SELECT * FROM adresy";
            OracleParameter[] prm = new OracleParameter[0];
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToAddress).Result;
            return result;
        }
    }
}
