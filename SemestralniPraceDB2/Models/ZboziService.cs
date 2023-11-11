using Oracle.ManagedDataAccess.Client;
using SemestralniPraceDB2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models
{
    public class ZboziService
    {
        public bool Create(Zbozi zbozi)
        {
            throw new NotImplementedException();
        }

        public bool Update(Zbozi zbozi)
        {
            throw new NotImplementedException();
        }
        public bool Delete(Zbozi zbozi)
        {
            throw new NotImplementedException();
        }
        public Zbozi? Get(Zbozi zbozi)
        {
            string sql = "SELECT * FROM zbozi WHERE id_zbozi = :id_zbozi";
            OracleParameter[] prm = new OracleParameter[6];
            prm[0] = new OracleParameter(":id_zbozi", OracleDbType.Int32, System.Data.ParameterDirection.Input);
            prm[0].Value = zbozi.Id;
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToZbozi).Result;
            return result.Count == 0 ? null : result[0];
        }
        public List<Zbozi> GetAll()
        {
            string sql = "SELECT * FROM zbozi";
            OracleParameter[] prm = new OracleParameter[0];
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToZbozi).Result;
            return result;
        }

        private OracleParameter[] MapZboziIntoParams(Zbozi zbozi)
        {
            OracleParameter[] prm = new OracleParameter[6];
            prm[0] = new OracleParameter("id_zbozi", OracleDbType.Int32, System.Data.ParameterDirection.Input);
            prm[0].Value = zbozi.Id;
            return prm;
        }

        private Zbozi MapOracleResultToZbozi(OracleDataReader result)
        {
            return new Zbozi()
            {
                Id = result.GetInt32(0)
            };
        }
    }
}
