using Oracle.ManagedDataAccess.Client;
using SemestralniPraceDB2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models
{
    public static class ZamestnanecService
    {
        ///<summary>
        /// Transactional 
        /// Adreasa + Obrazek + Zamestananec
        ///</summary>
        public static bool Create(Zamestnanec zamestnanec)
        {
            return ProcedureTransactionalCall(zamestnanec);
        }

        private static bool ProcedureTransactionalCall(Zamestnanec zamestnanec)
        {
            if (zamestnanec == null) return false;
            using (OracleConnection connection = DatabaseConnector.GetConnection())
            {
                connection.Open();

                // Start a transaction
                using (OracleTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        List<OracleParameter> parameters = new();
                        AdresaService.PrepareProcedureCall(zamestnanec.Adresa, out string prom, out List<OracleParameter> param);
                        zamestnanec.Adresa.Id = DatabaseConnector.ExecuteCommandNonQueryForTransactionAsync(prom, param, connection).Result;
                        if (zamestnanec.ObrazekZamestnance is not null)
                        {
                            ObrazekZamestnanceService.PrepareProcedureCall(zamestnanec.ObrazekZamestnance, out prom, out param);
                            zamestnanec.ObrazekZamestnance.Id = DatabaseConnector.ExecuteCommandNonQueryForTransactionAsync(prom, param, connection).Result;
                        }
                        if (zamestnanec is Brigadnik)
                        {
                            BrigadnikService.PrepareProcedureCall(zamestnanec as Brigadnik, out prom, out param);
                        } else if (zamestnanec is PlnyUvazek)
                        {
                            PlnyUvazekService.PrepareProcedureCall(zamestnanec as PlnyUvazek, out prom, out param);
                        } else
                        {
                            throw new Exception("Neplatny typ zamestnance");
                        }
                        zamestnanec.Id = DatabaseConnector.ExecuteCommandNonQueryForTransactionAsync(prom, param, connection).Result;

                        // Commit the transaction if all commands are successful
                        transaction.Commit();
                        Console.WriteLine("Transaction committed successfully");
                        return true;
                    }
                    catch (Exception ex)
                    {
                        // An error occurred, rollback the transaction
                        transaction.Rollback();
                        Console.WriteLine("Transaction rolled back due to an error: " + ex.Message);
                        return false;
                    }
                }
            }
        }
        ///<summary>
        /// Transactional 
        /// Adreasa + Obrazek + Zamestananec
        ///</summary>
        public static bool Update(Zamestnanec zamestnanec)
        {
            return ProcedureTransactionalCall(zamestnanec);
        }

        public static bool Delete(Zamestnanec zamestnanec)
        {
            if (zamestnanec == null) return false;
            using (OracleConnection connection = DatabaseConnector.GetConnection())
            {
                connection.Open();

                // Start a transaction
                using (OracleTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        DeleteTransactional(zamestnanec, connection);
                        // Commit the transaction if all commands are successful
                        transaction.Commit();
                        Console.WriteLine("Transaction committed successfully");
                        return true;
                    }
                    catch (Exception ex)
                    {
                        // An error occurred, rollback the transaction
                        transaction.Rollback();
                        Console.WriteLine("Transaction rolled back due to an error: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        private static void DeleteTransactional(Zamestnanec zamestnanec, OracleConnection connection)
        {
            string prom = "";
            List<OracleParameter> param = new();
            int result;
            if (zamestnanec.ObrazekZamestnance is not null)
            {
                ObrazekZamestnanceService.PrepareDeleteCall(zamestnanec.ObrazekZamestnance, out prom, out param);
                result = DatabaseConnector.ExecuteCommandNonQueryForTransactionAsync(prom, param, connection,CommandType.Text).Result;
            }
            if (zamestnanec is Brigadnik || zamestnanec.TypUvazku == 0)
            {
                BrigadnikService.PrepareDeleteCall(new Brigadnik() { Id = zamestnanec.Id}, out prom, out param);
            }
            else if (zamestnanec is PlnyUvazek || zamestnanec.TypUvazku == 1)
            {
                PlnyUvazekService.PrepareDeleteCall(new PlnyUvazek() { Id = zamestnanec.Id }, out prom, out param);
            }
            result = DatabaseConnector.ExecuteCommandNonQueryForTransactionAsync(prom, param, connection, CommandType.Text).Result;
            PrepareDeleteCall(zamestnanec, out prom, out param);
            result = DatabaseConnector.ExecuteCommandNonQueryForTransactionAsync(prom, param, connection, CommandType.Text).Result;
            AdresaService.PrepareDeleteCall(zamestnanec.Adresa, out prom, out param);
            result = DatabaseConnector.ExecuteCommandNonQueryForTransactionAsync(prom, param, connection, CommandType.Text).Result;
        }

        private static void PrepareDeleteCall(Zamestnanec zamestnanec, out string sql, out List<OracleParameter> prm)
        {
            sql = "DELETE FROM zamestnanci WHERE id_zamestnance = :id_zamestnance";
            prm = new List<OracleParameter>();
            prm.Add(new OracleParameter(":id_zamestnance", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = zamestnanec.Id;
        }

        public static Zamestnanec? Get(Zamestnanec zamestnanec)
        {
            if (zamestnanec is Brigadnik)
            {
                return BrigadnikService.Get(zamestnanec as Brigadnik);
            }
            else
            {
                return PlnyUvazekService.Get(zamestnanec as PlnyUvazek);
            }
        }

        public static List<Zamestnanec> GetAll()
        {
            string sql = "SELECT z.*,  m.jmeno AS manager_jmeno, m.prijmeni AS manager_prijmeni FROM  zamestnanci z LEFT JOIN zamestnanci m ON z.id_vedouci = m.id_zamestnance order by z.id_zamestnance ";
            List<OracleParameter> prm = new();
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToZamestnanec).Result;
            return result;
        }

        public static List<Zamestnanec> GetSubordinates(Zamestnanec zamestnanec)
        {
            string query = "SELECT z.*,  m.jmeno AS manager_jmeno, m.prijmeni AS manager_prijmeni, LEVEL, (SELECT COUNT(*) FROM zamestnanci WHERE id_vedouci = z.id_zamestnance) AS num_subordinates " +
                "FROM  zamestnanci z " +
                "JOIN zamestnanci m ON z.id_vedouci = m.id_zamestnance " +
                "WHERE  LEVEL != 0 OR (SELECT COUNT(*) FROM zamestnanci WHERE id_vedouci = z.id_zamestnance) != 0 " +
                "START WITH  z.id_vedouci = :id_vedouci " +
                "CONNECT BY  PRIOR z.id_zamestnance = z.id_vedouci "
                 + "ORDER BY LEVEL, num_subordinates DESC";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_vedouci", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = zamestnanec.Id;
            var subordinates = DatabaseConnector.ExecuteCommandQueryAsync(query, prm, MapOracleResultToZamestnanec).Result;
            return subordinates;
        }

        private static Zamestnanec MapOracleResultToZamestnanec(OracleDataReader result)
        {
            return new Zamestnanec
            {
                Id = result.GetInt32("id_zamestnance"),
                Jmeno = result.GetString("jmeno"),
                Prijmeni = result.GetString("prijmeni"),
                OsobniCislo = result.GetString("osobni_cislo"),
                TelCislo = result.GetString("tel_cislo"),
                Nastup = result.GetDateTime("nastup"),
                TypUvazku = result.GetInt16("typ_uvazku"),
                Vedouci = result.IsDBNull("id_vedouci") ? null : new PlnyUvazek
                {
                    Id = result.GetInt32("id_vedouci"),
                    Jmeno = result.GetString("manager_jmeno"),
                    Prijmeni = result.GetString("manager_prijmeni")
                },
                Supermarket = result.IsDBNull("id_supermarketu") ? null : new Supermarket
                {
                    Id = result.GetInt32("id_supermarketu")
                },
                Adresa = new Adresa
                {
                    Id = result.GetInt32("id_adresy")
                },
                Role = result.IsDBNull("id_role") ? null : new Role
                {
                    Id = result.GetInt32("id_role")
                },
                ObrazekZamestnance = result.IsDBNull("id_obrazku") ? null : new ObrazekZamestnance
                {
                    Id = result.GetInt32("id_obrazku")
                }
            };
        }

        internal static bool DeleteFromAdresa(Adresa adresa)
        {
            Zamestnanec zamestananec = BrigadnikService.GetFromAdresa(adresa);
            if (zamestananec != null)
            {
                return Delete(zamestananec);
            }
            zamestananec = PlnyUvazekService.GetFromAdresa(adresa);
            if (zamestananec != null)
            {
                return Delete(zamestananec);
            }
            return false;
        }
    }
}
