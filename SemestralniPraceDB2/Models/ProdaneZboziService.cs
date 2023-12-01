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
    public static class ProdaneZboziService
    {
        public static bool Create(ProdaneZbozi zbozi)
        {
            PrepareProcedureCall(zbozi, out string procedureName, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm);
            return result.Result == -1;
        }

        public static void PrepareProcedureCall(ProdaneZbozi zbozi, out string procedureName, out List<OracleParameter> prm)
        {
            procedureName = "pprodane_zbozi";
            prm = ProdaneZboziIntoParams(zbozi);
        }

        private static List<OracleParameter> ProdaneZboziIntoParams(ProdaneZbozi zbozi)
        {
            List<OracleParameter> prm = new List<OracleParameter>();

            prm.Add(new OracleParameter("p_mnozstvi", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = zbozi.Mnozstvi;

            prm.Add(new OracleParameter("p_cena", OracleDbType.Double, System.Data.ParameterDirection.Input));
            prm[1].Value = zbozi.Cena;

            prm.Add(new OracleParameter("p_id_zbozi", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[2].Value = zbozi.Zbozi.Id;

            prm.Add(new OracleParameter("p_id_uctenky", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[3].Value = zbozi.Uctenka.Id;

            return prm;
        }

        public static bool Update(ProdaneZbozi zbozi)
        {
            PrepareProcedureCall(zbozi, out string procedureName, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm);
            return result.Result == -1;
        }
        public static bool Delete(ProdaneZbozi zbozi)
        {
            PrepareDeleteCall(zbozi, out string sql, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(sql, prm, CommandType.Text).Result;
            return result == 1;
        }

        public static void PrepareDeleteCall(ProdaneZbozi zbozi, out string sql, out List<OracleParameter> prm)
        {
            sql = "DELETE FROM prodane_zbozi WHERE id_zbozi = :id_zbozi AND id_uctenky = :id_uctenky";
            prm = new();
            prm.Add(new OracleParameter(":id_zbozi", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = zbozi.Zbozi.Id;
            prm.Add(new OracleParameter(":id_uctenky", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[1].Value = zbozi.Uctenka.Id;
        }

        public static ProdaneZbozi? Get(ProdaneZbozi zbozi)
        {
            string sql = "Select * FROM prodane_zbozi WHERE id_uctenky = :id_uctenky AND id_zbozi = :id_zbozi";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_uctenky", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = zbozi.Uctenka.Id;
            prm.Add(new OracleParameter(":id_zbozi", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[1].Value = zbozi.Zbozi.Id;
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToProdaneZbozi).Result;
            return result.Count == 0 ? null : result[0];
        }

        private static ProdaneZbozi MapOracleResultToProdaneZbozi(OracleDataReader reader)
        {
            return new ProdaneZbozi()
            {
                Mnozstvi = reader.GetInt32("mnozstvi"),
                Cena = reader.GetDouble("cena"),
                Uctenka = new Uctenka() { Id = reader.GetInt32("id_uctenky") },
                Zbozi = ZboziService.MapOracleResultToZbozi(reader)
            };
        }

        public static List<ProdaneZbozi> GetAll()
        {
            string sql = "SELECT * FROM prodane_zbozi";
            List<OracleParameter> prm = new();
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToProdaneZbozi).Result;
            return result;
        }
        public static List<ProdaneZbozi> GetFromUctenka(Uctenka uctenka)
        {
            string sql = "SELECT * FROM prodane_zbozi WHERE id_uctenky = :id_uctenky";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_uctenky", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = uctenka.Id;
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToProdaneZbozi).Result;
            return result;
        }
        public static List<ProdaneZbozi> GetFromPlatba(Platba platba)
        {
            string sql = "SELECT p.*,z.* FROM prodane_zbozi p JOIN UCTENKY u ON p.id_uctenky = u.id_uctenky JOIN ZBOZI z ON z.id_zbozi = p.id_zbozi WHERE u.id_platby = :id_platby";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_platby", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = platba.Id;
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToProdaneZbozi).Result;
            return result;
        }

    }
}
