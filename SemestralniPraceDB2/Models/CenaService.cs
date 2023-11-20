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
    public static class CenaService
    {
        
        public static bool Create(Cena cena)
        {
            PrepareProcedureCall(cena, out string procedureName, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm).Result;
            cena.Id = result;
            return result > 0;
        }

        public static void PrepareProcedureCall(Cena cena, out string procedureName, out List<OracleParameter> prm)
        {
            procedureName = "pcena";
            prm = MapCenaIntoParams(cena);
        }

        private static List<OracleParameter> MapCenaIntoParams(Cena cena)
        {
            List<OracleParameter> prm = new List<OracleParameter>();

            prm.Add(new OracleParameter("p_id_ceny", OracleDbType.Int32, System.Data.ParameterDirection.InputOutput));
            prm[0].Value = cena.Id;

            prm.Add(new OracleParameter("p_od", OracleDbType.Date, System.Data.ParameterDirection.Input));
            prm[1].Value = cena.PlatnostOd;

            prm.Add(new OracleParameter("p_cena", OracleDbType.Double, System.Data.ParameterDirection.Input));
            prm[2].Value = cena.Castka;

            prm.Add(new OracleParameter("p_id_zbozi", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[3].Value = cena.Zbozi.Id;

            return prm;
        }


        public static bool Update(Cena cena)
        {
            PrepareProcedureCall(cena, out string procedureName, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm).Result;
            return result > 0;
        }
        public static bool Delete(Cena cena)
        {
            PrepareDeleteCall(cena, out string sql, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(sql, prm, CommandType.Text).Result;
            return result == 1;
        }

        public static void PrepareDeleteCall(Cena cena, out string sql, out List<OracleParameter> prm)
        {
            sql = "DELETE FROM ceny WHERE id_ceny = :id_ceny";
            prm = new();
            prm.Add(new OracleParameter(":id_ceny", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = cena.Id;
        }

        //TODO USE VIEW
        public static bool Get(Cena cena)
        {
            throw new NotImplementedException();
        }

        //TODO USE VIEW
        public static List<Cena> GetAll()
        {
            throw new NotImplementedException();
        }

        //TODO USE VIEW
        public static List<Cena> GetPriceHistory(Zbozi zbozi)
        {
            throw new NotImplementedException();
        }
    }
}
