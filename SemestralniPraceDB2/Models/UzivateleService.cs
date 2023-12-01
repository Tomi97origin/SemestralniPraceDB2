using Oracle.ManagedDataAccess.Client;
using OracleInternal.SqlAndPlsqlParser.LocalParsing;
using SemestralniPraceDB2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;




namespace SemestralniPraceDB2.Models
{
    public static class UzivateleService
    {
        public static Uzivatel? Prihlaseny { get; set; }
        public static Uzivatel? Aktualni { get; set; }
        public static bool Registration(string jmeno, string heslo)
        {
            Uzivatel uzivatel = new Uzivatel()
            {
                Id = 0,
                Username = jmeno,
                Password = BCrypt.Net.BCrypt.HashPassword(heslo),
                Admin = false,
                Active = true,
            };
            return Create(uzivatel);
        }

        public static bool Update(Uzivatel uzivatel)
        {
            PrepareProcedureCall(uzivatel, out string procedureName, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm).Result;
            return result > 0;
        }

        private static void PrepareProcedureCall(Uzivatel uzivatel, out string procedureName, out List<OracleParameter> prm)
        {
            procedureName = "puzivatele";
            prm = MapUzivatelIntoParams(uzivatel);
        }

        private static bool Create(Uzivatel uzivatel)
        {
            PrepareProcedureCall(uzivatel, out string procedureName, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm).Result;
            return result > 0;
        }

        public static Uzivatel? Login(string jmeno, string heslo)
        {
            Uzivatel? uzivatel = GetUzivatele(jmeno);
            if (uzivatel == null)
            {
                return null;
            }
            if (BCrypt.Net.BCrypt.Verify(heslo,uzivatel.Password) && uzivatel.Active)
            {
                uzivatel.PosledniPrihlaseni = DateTime.Now;
                Update(uzivatel);
                Prihlaseny = Aktualni = uzivatel;
                return uzivatel;
            }
            return null;
        }

        private static Uzivatel? GetUzivatele(string jmeno)
        {
            string query = "SELECT * FROM uzivatele WHERE username = :username";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":username", OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            prm[0].Value = jmeno;

            var result = DatabaseConnector.ExecuteCommandQueryAsync(query, prm, MapUzivatelFromReader).Result;
            return result.Count == 0 ? null : result[0];
        }

        public static void Emulate(Uzivatel uzivatel)
        {
            if (Prihlaseny?.Admin == true)
            {
                var emulated = GetUzivatele(uzivatel.Username);
                if (emulated != null && emulated.Active)
                {
                    Aktualni = emulated;
                }
            }
        }

        public static void StopEmulating()
        {
            Aktualni = Prihlaseny;
        }

        public static bool Delete(Uzivatel uzivatel)
        {
            if (uzivatel == null) return false;
            if (Prihlaseny?.Admin == false) { return false; }
            string sql = "DELETE FROM uzivatele WHERE id_uzivatele = :id_uzivatele";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_uzivatele", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = uzivatel.Id;
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(sql, prm, CommandType.Text).Result;
            return result == 1;
        }
        private static Uzivatel MapUzivatelFromReader(OracleDataReader reader)
        {
            return new Uzivatel()
            {
                Id = reader.GetInt32("id_uzivatele"),
                Username = reader.GetString("username"),
                Password = reader.GetString("password"),
                Admin = reader.GetInt32("admin") == 1,
                Active = reader.GetInt32("active") == 1,
                PosledniPrihlaseni = reader.GetDateTime("posledni")
            };
        }

        private static List<OracleParameter> MapUzivatelIntoParams(Uzivatel uzivatel)
        {
            List<OracleParameter> prm = new List<OracleParameter>();

            prm.Add(new OracleParameter("p_id_uzivatele", OracleDbType.Int32, System.Data.ParameterDirection.InputOutput));
            prm[0].Value = uzivatel.Id <= 0 ? null : uzivatel.Id;

            prm.Add(new OracleParameter("p_username", OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            prm[1].Value = uzivatel.Username;

            prm.Add(new OracleParameter("p_password", OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            prm[2].Value = uzivatel.Password;

            prm.Add(new OracleParameter("p_admin", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[3].Value = uzivatel.Admin ? 1 : 0;

            prm.Add(new OracleParameter("p_active", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[4].Value = uzivatel.Active ? 1 : 0;

            prm.Add(new OracleParameter("p_posledni", OracleDbType.TimeStampTZ, System.Data.ParameterDirection.Input));
            prm[5].Value = uzivatel.PosledniPrihlaseni;

            return prm;
        }

        public static bool DeleteAllOldDeactivated()
        {
            string procedureName = "REMOVE_OLD_DEACTIVATED_USERS";
            List<OracleParameter> prm = new();
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm).Result;
            return result == -1;
        }

        public static List<Uzivatel> GetAll()
        {
            string query = "SELECT * FROM uzivatele";
            List<OracleParameter> prm = new();
            var result = DatabaseConnector.ExecuteCommandQueryAsync(query, prm, MapUzivatelFromReader).Result;
            return result;
        }
    }
}
