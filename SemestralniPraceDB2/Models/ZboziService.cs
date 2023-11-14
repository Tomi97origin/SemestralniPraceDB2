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
    public class ZboziService
    {
        public static bool Create(Zbozi zbozi)
        {
            string procedureName = "pzbozi";
            List<OracleParameter> prm = MapZboziIntoParams(zbozi);
            prm[0] = null;
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm);
            return result.Result == -1;
        }

        public static bool Update(Zbozi zbozi)
        {
            string procedureName = "pzbozi";
            List<OracleParameter> prm = MapZboziIntoParams(zbozi);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm);
            return result.Result == -1;
        }
        public static bool Delete(Zbozi zbozi)
        {
            string sql = "DELETE FROM zbozi WHERE id_zbozi = :id_zbozi";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_zbozi", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = zbozi.Id;
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(sql, prm, CommandType.Text).Result;
            return result == 1;
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

            prm.Add(new OracleParameter("p_id_zbozi", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = zbozi.Id <= 0 ? null : zbozi.Id;

            prm.Add(new OracleParameter("p_nazev", OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            prm[1].Value = zbozi.Nazev;

            prm.Add(new OracleParameter("p_popis", OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            prm[2].Value = zbozi.Popis;

            prm.Add(new OracleParameter("p_ean", OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            prm[3].Value = zbozi.EAN;

            prm.Add(new OracleParameter("p_id_kategorie", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[4].Value = zbozi.Kategorie.Id; // Assuming Kategorie has an Id property

            prm.Add(new OracleParameter("p_id_vyrobce", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[5].Value = zbozi.Vyrobce.Id; // Assuming Vyrobce has an Id property

            return prm;
        }


        private static Zbozi MapOracleResultToZbozi(OracleDataReader result)
        {
            return new Zbozi()
            {
                Id = result.GetInt32(0),
                Nazev = result.GetString(1),
                Popis = result.GetString(2),
                EAN = result.GetString(3),
                Kategorie = new Kategorie() { Id = result.GetInt32(4) }, // Assuming Kategorie has an Id property
                Vyrobce = new Vyrobce() { Id = result.GetInt32(5) } // Assuming Vyrobce has an Id property
            };
        }
    }
}
