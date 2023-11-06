using Oracle.ManagedDataAccess.Client;
using OracleInternal.SqlAndPlsqlParser.LocalParsing;
using SemestralniPraceDB2.Models.Entities;
using System;
using System.Collections.Generic;
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
    public class UzivateleService
    {
        public static Uzivatel? Prihlaseny { get; set; }
        public static Uzivatel? Aktualni { get; set; }
        public bool Registration(string jmeno, string heslo)
        {
            
            Uzivatel uzivatel = new Uzivatel()
            {
                Id  =   0,
                Username = jmeno,
                Password = BCrypt.Net.BCrypt.HashPassword(heslo),
                Admin=false
            };
            return Create(uzivatel);
        }

        public bool Update(Uzivatel uzivatel)
        {
            if (uzivatel.Id == 0)
            {
                return Create(uzivatel);
            }
            string query = "UPDATE uzivatel SET username = :username, password = :password, admin = :admin, posledniPrihlaseni = :posledniPrihlaseni WHERE id_uzivatele = :id_uzivatele";
            OracleParameter[] prm = new OracleParameter[5];
            prm[0] = new OracleParameter(":username", OracleDbType.Varchar2, System.Data.ParameterDirection.Input);
            prm[0].Value = uzivatel.Username;
            prm[1] = new OracleParameter(":password", OracleDbType.Varchar2, System.Data.ParameterDirection.Input);
            prm[1].Value = uzivatel.Password;
            prm[2] = new OracleParameter(":admin", OracleDbType.Int32, System.Data.ParameterDirection.Input);
            prm[2].Value = uzivatel.Admin == true ? 1 : 0;
            prm[3] = new OracleParameter(":posledniPrihlaseni", OracleDbType.Date, System.Data.ParameterDirection.Input);
            prm[3].Value = uzivatel.PosledniPrihlaseni;
            prm[4] = new OracleParameter(":id_uzivatele", OracleDbType.Int32, System.Data.ParameterDirection.Input);
            prm[4].Value = uzivatel.Id;
            var result = DatabaseConnector.ExecuteCommandAsync(query, prm).Result;
            if (result == null)
                return false;
            return result.RecordsAffected == 1;
        }

        private bool Create(Uzivatel uzivatel)
        {
            string query = "INSERT INTO uzivatel (username,password,admin,posledniPrihlaseni) VALUES (:username,:password,:admin,:posledniPrihlaseni)";
            OracleParameter[] prm = new OracleParameter[4];
            prm[0] = new OracleParameter(":username", OracleDbType.Varchar2, System.Data.ParameterDirection.Input);
            prm[0].Value = uzivatel.Username;
            prm[1] = new OracleParameter(":password", OracleDbType.Varchar2, System.Data.ParameterDirection.Input);
            prm[1].Value = uzivatel.Password;
            prm[2] = new OracleParameter(":admin", OracleDbType.Int32, System.Data.ParameterDirection.Input);
            prm[2].Value = uzivatel.Admin == true ? 1 : 0;
            prm[3] = new OracleParameter(":posledniPrihlaseni", OracleDbType.Date, System.Data.ParameterDirection.Input);
            prm[3].Value = uzivatel.PosledniPrihlaseni;
            var result = DatabaseConnector.ExecuteCommandAsync(query, prm).Result;
            if (result == null)
                return false;
            return result.RecordsAffected == 1;
        }

        public Uzivatel? Login(string jmeno, string heslo)
        {
            Uzivatel? uzivatel = GetUzivatele(jmeno);
            if (uzivatel == null)
            {
                return null;
            }
            if (uzivatel.Password == BCrypt.Net.BCrypt.HashPassword(heslo))
            {
                uzivatel.PosledniPrihlaseni = DateTime.Now;
                Update(uzivatel);
                Prihlaseny = Aktualni = uzivatel;
                return uzivatel;
            }
            return null;
        }

        private Uzivatel? GetUzivatele(string jmeno)
        {
            string query = "SELECT * FROM Uzivatel WHERE username = :username";
            OracleParameter[] prm = new OracleParameter[1];
            prm[0] = new OracleParameter(":username", OracleDbType.Varchar2,System.Data.ParameterDirection.Input);
            prm[0].Value = jmeno;
            Uzivatel toReturn;

            var result = DatabaseConnector.ExecuteCommandAsync(query, prm).Result;
            if (result == null || !result.HasRows)
            {
                return null;
            }
            toReturn = MapUzivatelFromReader(result);
            result.Close();
            return toReturn;
        }

        public void Emulate(string jmeno)
        {
            if (Prihlaseny?.Admin == true)
            {
                var emulated = GetUzivatele(jmeno);
                if (emulated != null)
                {
                    Aktualni = emulated;
                }
            }
        }

        public void StopEmulating()
        {
            Aktualni = Prihlaseny;
        }

        public bool Delete(Uzivatel uzivatel)
        {
            if (uzivatel == null) return false;
            if (Prihlaseny?.Admin == false) { return false; }
            throw new NotImplementedException();
        }

        private static Uzivatel MapUzivatelFromReader(OracleDataReader reader)
        {
            reader.Read();

            return new Uzivatel()
            {
                Id = reader.GetInt32(0),
                Username = reader.GetString(1),
                Password = reader.GetString(2),
                Admin = reader.GetInt32(3) == 0,
                PosledniPrihlaseni = reader.GetDateTime(4)
            };
        }
        
    }
}
