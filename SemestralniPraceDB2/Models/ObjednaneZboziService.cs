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
    public static class ObjednaneZboziService
    {
        public static bool Create(ObjednaneZbozi zbozi)
        {
            PrepareProcedureCall(zbozi, out string procedureName, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm);
            return result.Result == -1;
        }

        public static void PrepareProcedureCall(ObjednaneZbozi zbozi, out string procedureName, out List<OracleParameter> prm)
        {
            procedureName = "pobjednane_zbozi";
            prm = ObjednaneZboziIntoParams(zbozi);
        }

        public static bool Update(ObjednaneZbozi zbozi)
        {
            PrepareProcedureCall(zbozi, out string procedureName, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm);
            return result.Result == -1;
        }
        public static bool Delete(ObjednaneZbozi zbozi)
        {
            PrepareDeleteCall(zbozi, out string sql, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(sql, prm, CommandType.Text).Result;
            return result == 1;
        }

        public static void PrepareDeleteCall(ObjednaneZbozi zbozi, out string sql, out List<OracleParameter> prm)
        {
            sql = "DELETE FROM objednane_zbozi WHERE id_objednavky = :id_objednavky AND id_zbozi = :id_zbozi";
            prm = new();
            prm.Add(new OracleParameter(":id_objednavky", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = zbozi.Objednavka.Id;
            prm.Add(new OracleParameter(":id_zbozi", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[1].Value = zbozi.Zbozi.Id;
        }

        public static ObjednaneZbozi? Get(ObjednaneZbozi zbozi)
        {
            string sql = "Select * FROM objednane_zbozi WHERE id_objednavky = :id_objednavky AND id_zbozi = :id_zbozi";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_objednavky", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = zbozi.Objednavka.Id;
            prm.Add(new OracleParameter(":id_zbozi", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[1].Value = zbozi.Zbozi.Id;
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToObjednaneZbozi).Result;
            return result.Count == 0 ? null : result[0];
        }

        private static ObjednaneZbozi MapOracleResultToObjednaneZbozi(OracleDataReader reader)
        {

            return new ObjednaneZbozi()
            {
                Mnozstvi = reader.GetInt32("mnozstvi"),
                Cena = reader.GetDouble("cena"),
                Objednavka = new Objednavka() { Id = reader.GetInt32("id_objednavky") },
                Zbozi = new Zbozi() { Id = reader.GetInt32("id_zbozi") }
            };
        }

        public static List<ObjednaneZbozi> GetAll()
        {
            string sql = "SELECT * FROM objednane_zbozi";
            List<OracleParameter> prm = new();
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToObjednaneZbozi).Result;
            return result;
        }

        public static List<ObjednaneZbozi> GetFromObjednavka(Objednavka objednavka)
        {
            string sql = "Select * FROM objednane_zbozi WHERE id_objednavky = :id_objednavky";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_objednavky", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = objednavka.Id;
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToObjednaneZbozi).Result;
            return result;
        }

        private static List<OracleParameter> ObjednaneZboziIntoParams(ObjednaneZbozi zbozi)
        {
            List<OracleParameter> prm = new List<OracleParameter>();

            prm.Add(new OracleParameter("p_mnozstvi", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = zbozi.Mnozstvi;

            prm.Add(new OracleParameter("p_cena", OracleDbType.Double, System.Data.ParameterDirection.Input));
            prm[1].Value = zbozi.Cena;

            prm.Add(new OracleParameter("p_id_objednavky", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[2].Value = zbozi.Objednavka.Id;

            prm.Add(new OracleParameter("p_id_zbozi", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[3].Value = zbozi.Zbozi.Id;

            return prm;
        }

        internal static void DeleteFromObjednavka(Objednavka objednavka, OracleConnection connection)
        {
            PrepareDeleteCallFromObjednavka(objednavka, out string sql, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryForTransactionAsync(sql, prm, connection,CommandType.Text).Result;
        }

        private static void PrepareDeleteCallFromObjednavka(Objednavka objednavka, out string sql, out List<OracleParameter> prm)
        {
            sql = "DELETE FROM objednane_zbozi WHERE id_objednavky = :id_objednavky";
            prm = new();
            prm.Add(new OracleParameter(":id_objednavky", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = objednavka.Id;
        }

        internal static void DeleteFromZbozi(Zbozi zbozi, OracleConnection connection)
        {
            PrepareDeleteCallFromZbozi(zbozi, out string sql, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryForTransactionAsync(sql, prm, connection, CommandType.Text).Result;
        }

        private static void PrepareDeleteCallFromZbozi(Zbozi zbozi, out string sql, out List<OracleParameter> prm)
        {
            sql = "DELETE FROM objednane_zbozi WHERE id_zbozi = :id_zbozi";
            prm = new();
            prm.Add(new OracleParameter(":id_zbozi", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = zbozi.Id;
        }
    }
}
