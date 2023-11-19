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
    public static class RoleService
    {
        public static bool Create(Role role)
        {
            PrepareProcedureCall(role, out string procedureName, out List<OracleParameter> prm);
            prm[0].Value = null;
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm);
            return result.Result > 0;
        }

        public static void PrepareProcedureCall(Role role, out string procedureName, out List<OracleParameter> prm)
        {
            procedureName = "prole";
            prm = MapRoleIntoParams(role);
        }

        private static List<OracleParameter> MapRoleIntoParams(Role role)
        {
            List<OracleParameter> prm = new List<OracleParameter>();

            prm.Add(new OracleParameter("p_id_role", OracleDbType.Int32, System.Data.ParameterDirection.InputOutput));
            prm[0].Value = role.Id <= 0 ? null : role.Id;

            prm.Add(new OracleParameter("p_nazev", OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            prm[1].Value = role.Nazev;

            return prm;
        }

        public static bool Update(Role role)
        {
            PrepareProcedureCall(role, out string procedureName, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm);
            return result.Result > 0;
        }
        public static bool Delete(Role role)
        {
            PrepareDeleteCall(role, out string sql, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(sql, prm, CommandType.Text).Result;
            return result == 1;
        }

        public static void PrepareDeleteCall(Role role, out string sql, out List<OracleParameter> prm)
        {
            sql = "DELETE FROM role WHERE id_role = :id_role";
            prm = new();
            prm.Add(new OracleParameter(":id_role", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = role.Id;
        }

        public static Role? Get(Role role)
        {
            string sql = "SELECT * FROM role WHERE id_role = :id_role";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_role", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = role.Id;
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToRole).Result;
            return result.Count == 0 ? null : result[0];
        }

        private static Role MapOracleResultToRole(OracleDataReader reader)
        {
            return new Role
            {
                Id = reader.GetInt32("id_role"),
                Nazev = reader.GetString("nazev")
            };
        }

        public static List<Role> GetAll()
        {
            string sql = "SELECT * FROM role";
            List<OracleParameter> prm = new();
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToRole).Result;
            return result;
        }
    }
}
