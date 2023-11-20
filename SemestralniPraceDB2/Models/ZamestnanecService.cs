using Oracle.ManagedDataAccess.Client;
using SemestralniPraceDB2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
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
                        zamestnanec.Adresa.Id = DatabaseConnector.ExecuteCommandProcedureForTransactionAsync(prom, param, connection).Result;
                        if (zamestnanec.ObrazekZamestnance is not null)
                        {
                            ObrazekZamestnanceService.PrepareProcedureCall(zamestnanec.ObrazekZamestnance, out prom, out param);
                            zamestnanec.ObrazekZamestnance.Id = DatabaseConnector.ExecuteCommandProcedureForTransactionAsync(prom, param, connection).Result;
                        }
                        if (zamestnanec is Brigadnik)
                        {
                            BrigadnikService.PrepareProcedureCall(zamestnanec as Brigadnik, out prom, out param);
                        }
                        if (zamestnanec is PlnyUvazek)
                        {
                            PlnyUvazekService.PrepareProcedureCall(zamestnanec as PlnyUvazek, out prom, out param);
                        }
                        zamestnanec.Id = DatabaseConnector.ExecuteCommandProcedureForTransactionAsync(prom, param, connection).Result;

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
            throw new NotImplementedException();
        }
        public static bool Get(Zamestnanec zamestnanec)
        {
            throw new NotImplementedException();
        }
        public static List<Zamestnanec> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
