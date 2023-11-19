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
    public static class UctenkaService
    {
        public static bool Create(Uctenka uctenka)
        {
            PrepareProcedureCall(uctenka, out string procedureName, out List<OracleParameter> prm);
            prm[0].Value = null;
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm);
            return result.Result > 0;
        }

        public static void PrepareProcedureCall(Uctenka uctenka, out string procedureName, out List<OracleParameter> prm)
        {
            procedureName = "puctenky";
            prm = MapUctenkaIntoParams(uctenka);
        }

        private static List<OracleParameter> MapUctenkaIntoParams(Uctenka uctenka)
        {
            List<OracleParameter>  prm = new List<OracleParameter>();

            prm.Add(new OracleParameter("p_id_uctenky", OracleDbType.Int32, System.Data.ParameterDirection.InputOutput));
            prm[0].Value = uctenka.Id <= 0 ? null : uctenka.Id;

            prm.Add(new OracleParameter("p_vytvoreno", OracleDbType.Date, System.Data.ParameterDirection.Input));
            prm[1].Value = uctenka.Vytvoreno;

            prm.Add(new OracleParameter("p_celkova_castka", OracleDbType.Double, System.Data.ParameterDirection.Input));
            prm[2].Value = uctenka.CelkovaCena;

            prm.Add(new OracleParameter("p_id_pokladny", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[3].Value = uctenka.Pokladna.Id;

            prm.Add(new OracleParameter("p_id_platby", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[4].Value = uctenka.Platba.Id;

            return prm;
        }

        public static bool Update(Uctenka uctenka)
        {
            PrepareProcedureCall(uctenka, out string procedureName, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm);
            return result.Result > 0;
        }
        public static bool Delete(Uctenka uctenka)
        {
            PrepareDeleteCall(uctenka, out string sql, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(sql, prm, CommandType.Text).Result;
            return result == 1;
        }

        public static void PrepareDeleteCall(Uctenka uctenka, out string sql, out List<OracleParameter> prm)
        {
            sql = "DELETE FROM uctenky WHERE id_uctenky = :id_uctenky";
            prm = new();
            prm.Add(new OracleParameter(":id_uctenky", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = uctenka.Id;
        }

        public static Uctenka? Get(Uctenka uctenka)
        {
            string sql = "Select * FROM uctenky WHERE id_uctenky = :id_uctenky";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_uctenky", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = uctenka.Id;
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToUctenka).Result;
            return result.Count == 0 ? null : result[0];
        }

        private static Uctenka MapOracleResultToUctenka(OracleDataReader reader)
        {
            return new Uctenka()
            {
                Id = reader.GetInt32("id_uctenky"),
                Vytvoreno = reader.GetDateTime("vytvoreno"),
                CelkovaCena = reader.GetDouble("celkova_castka"),
                Pokladna = new Pokladna(){
                    Id = reader.GetInt32("id_pokladny") 
                },
                Platba = new Platba() {
                   Id = reader.GetInt32("id_platby")
                }
            };
        }

        public static List<Uctenka> GetAll()
        {
            string sql = "Select * FROM uctenky";
            List<OracleParameter> prm = new();
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToUctenka).Result;
            return result;
        }
    }
}
