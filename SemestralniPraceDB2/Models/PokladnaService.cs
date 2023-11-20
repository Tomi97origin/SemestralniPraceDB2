using Oracle.ManagedDataAccess.Client;
using SemestralniPraceDB2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models
{
    public static class PokladnaService
    {
        public static bool Create(Pokladna pokladna)
        {
            PrepareProcedureCall(pokladna, out string procedureName, out List<OracleParameter> prm);
            prm[0].Value = null;
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm);
            return result.Result > 0;
        }

        public static void PrepareProcedureCall(Pokladna pokladna, out string procedureName, out List<OracleParameter> prm)
        {
            procedureName = "ppokladny";
            prm = MapPokladnaIntoParams(pokladna);
        }

        private static List<OracleParameter> MapPokladnaIntoParams(Pokladna pokladna)
        {
            List<OracleParameter> prm = new List<OracleParameter>();

            prm.Add(new OracleParameter("p_id_pokladny", OracleDbType.Int32, System.Data.ParameterDirection.InputOutput));
            prm[0].Value = pokladna.Id <= 0 ? null : pokladna.Id;

            prm.Add(new OracleParameter("p_cislo", OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            prm[1].Value = pokladna.Cislo;

            prm.Add(new OracleParameter("p_otevreno", OracleDbType.Int16, System.Data.ParameterDirection.Input));
            prm[2].Value = pokladna.Otevreno;

            prm.Add(new OracleParameter("p_automaticka", OracleDbType.Int16, System.Data.ParameterDirection.Input));
            prm[3].Value = pokladna.Automaticka;

            prm.Add(new OracleParameter("p_id_supermarketu", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[4].Value = pokladna.Supermarket.Id;

            return prm;
        }

        public static bool Update(Pokladna pokladna)
        {
            PrepareProcedureCall(pokladna, out string procedureName, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm);
            return result.Result > 0;
        }
        public static bool Delete(Pokladna pokladna)
        {
            PrepareDeleteCall(pokladna, out string sql, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(sql, prm, CommandType.Text).Result;
            return result == 1;
        }

        public static void PrepareDeleteCall(Pokladna pokladna, out string sql, out List<OracleParameter> prm)
        {
            sql = "DELETE FROM pokladny WHERE id_pokladny = :id_pokladny";
            prm = new();
            prm.Add(new OracleParameter(":id_pokladny", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = pokladna.Id;
        }

        public static Pokladna? Get(Pokladna pokladna)
        {
            string sql = "SELECT * FROM pokladny WHERE id_pokladny = :id_pokladny";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_pokladny", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = pokladna.Id;
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToPokladna).Result;
            return result.Count == 0 ? null : result[0];
        }

        private static Pokladna MapOracleResultToPokladna(OracleDataReader reader)
        {
            return new Pokladna
            {
                Id = reader.GetInt32("id_pokladny"),
                Cislo = reader.GetString("cislo"),
                Otevreno = reader.GetInt16("otevreno"),
                Automaticka = reader.GetInt16("automaticka"),
                Supermarket = new Supermarket {
                    Id = reader.GetInt32("id_supermarketu")
                }
            };
        }

        public static List<Pokladna> GetAll()
        {
            string sql = "SELECT * FROM pokladny";
            List<OracleParameter> prm = new();
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToPokladna).Result;
            return result;
        }

        public static List<Pokladna> GetFromSupermarket(Supermarket supermarket)
        {
            string sql = "SELECT * FROM pokladny WHERE id_supermarketu = :id_supermarketu";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_supermarketu", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = supermarket.Id;
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToPokladna).Result;
            return result;
        }
    }
}
