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
    public class KategorieService
    {
        public static bool Create(Kategorie kategorie)
        {
            string procedureName = "pkategorie";
            List<OracleParameter> prm = MapKategorieIntoParams(kategorie);
            prm[0].Value = null;
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm);
            return result.Result == -1;
        }

        public static bool Update(Kategorie kategorie)
        {
            string procedureName = "pkategorie";
            List<OracleParameter> prm = MapKategorieIntoParams(kategorie);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm);
            return result.Result == -1;
        }
        public static bool Delete(Kategorie kategorie)
        {
            string sql = "DELETE FROM kategorie WHERE id_kategorie = :id_kategorie";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_kategorie", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = kategorie.Id;
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(sql, prm, CommandType.Text).Result;
            return result == 1;
        }
        public static Kategorie? Get(Kategorie kategorie)
        {
            string sql = "SELECT * FROM kategorie WHERE id_kategorie = :id_kategorie";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_kategorie", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = kategorie.Id;
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToKategorie).Result;
            return result.Count == 0 ? null : result[0];
        }

        private static Kategorie MapOracleResultToKategorie(OracleDataReader reader)
        {
            return new Kategorie()
            {
                Id = reader.GetInt32(0),
                Nazev = reader.GetString(1),
                Zkratka = reader.GetString(2)
            };
        }

        public static List<Kategorie> GetAll()
        {
            string sql = "SELECT * FROM kategorie";
            List<OracleParameter> prm = new();
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToKategorie).Result;
            return result;
        }

        private static List<OracleParameter> MapKategorieIntoParams(Kategorie kategorie)
        {
            List<OracleParameter> prm = new List<OracleParameter>();

            prm.Add(new OracleParameter("p_id_kategorie", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = kategorie.Id;

            prm.Add(new OracleParameter("p_nazev", OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            prm[1].Value = kategorie.Nazev;

            prm.Add(new OracleParameter("p_zkratka", OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            prm[2].Value = kategorie.Zkratka;

            return prm;
        }

    }
}
