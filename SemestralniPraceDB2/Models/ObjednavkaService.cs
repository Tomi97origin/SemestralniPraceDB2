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
    public static class ObjednavkaService
    {
        public static bool Create(Objednavka objednavka,List<ObjednaneZbozi> polozky)
        {
            objednavka.Id = 0;
            objednavka.CelkovaCena = 0; // Dopocita trigger v DB
            return TransactionProcedureCall(objednavka, polozky);
        }
        public static void PrepareProcedureCall(Objednavka objednavka, out string procedureName, out List<OracleParameter> prm)
        {
            procedureName = "pobjednavky";
            prm = MapObjednavkaIntoParams(objednavka);
        }
        public static bool Update(Objednavka objednavka)
        {
            PrepareProcedureCall(objednavka, out string procedureName, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm);
            return result.Result > 0;
        }
        public static bool Delete(Objednavka objednavka)
        {
            PrepareDeleteCall(objednavka, out string sql, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(sql, prm, CommandType.Text).Result;
            return result == 1;
        }

        public static void PrepareDeleteCall(Objednavka objednavka, out string sql, out List<OracleParameter> prm)
        {
            sql = "DELETE FROM objednavky WHERE id_objednavky = :id_objednavky";
            prm = new();
            prm.Add(new OracleParameter(":id_objednavky", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = objednavka.Id;
        }

        public static Objednavka? Get(Objednavka objednavka)
        {
            string sql = "Select * FROM objednavky WHERE id_objednavky = :id_objednavky";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_objednavky", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = objednavka.Id;
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToObjednavka).Result;
            return result.Count == 0 ? null : result[0];
        }

        private static Objednavka MapOracleResultToObjednavka(OracleDataReader reader)
        {
            return new Objednavka()
            {
                Id = reader.GetInt32("id_objednavky"),
                Vytvoreno = reader.GetDateTime("vytvoreno"),
                Splatnost = reader.IsDBNull("splatnost") ? (DateTime?)null : reader.GetDateTime("splatnost"),
                CelkovaCena = reader.IsDBNull("celkova_cena") ? (double?)null : reader.GetDouble("celkova_cena"),
                Prijato = reader.IsDBNull("prijato") ? false : reader.GetInt32("prijato") == 0 ? false : true, 
                Supermarket = new Supermarket() { Id = reader.GetInt32("id_supermarketu") },
                Dodavatel = new Dodavatel() { Id = reader.GetInt32("id_dodavatele") }
            };
        }

        public static List<Objednavka> GetAll(bool pouzeNeprijate = false)
        {
            string sql = "Select * FROM objednavky";
            if (pouzeNeprijate)
            {
                sql += " WHERE prijato = 0";
            }
            List<OracleParameter> prm = new();
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToObjednavka).Result;
            return result;
        }

        private static List<OracleParameter> MapObjednavkaIntoParams(Objednavka objednavka)
        {
            List<OracleParameter> prm = new List<OracleParameter>();

            prm.Add(new OracleParameter("p_id_objednavky", OracleDbType.Int32, System.Data.ParameterDirection.InputOutput));
            prm[0].Value = objednavka.Id <= 0 ? null : objednavka.Id;

            prm.Add(new OracleParameter("p_vytvoreno", OracleDbType.Date, System.Data.ParameterDirection.Input));
            prm[1].Value = objednavka.Vytvoreno;

            prm.Add(new OracleParameter("p_splatnost", OracleDbType.Date, System.Data.ParameterDirection.Input));
            prm[2].Value = objednavka.Splatnost;

            prm.Add(new OracleParameter("p_celkova_cena", OracleDbType.Double, System.Data.ParameterDirection.Input));
            prm[3].Value = objednavka.CelkovaCena;

            prm.Add(new OracleParameter("p_prijato", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[4].Value = objednavka.Prijato ? 1 : 0;

            prm.Add(new OracleParameter("p_id_supermarketu", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[5].Value = objednavka.Supermarket.Id;

            prm.Add(new OracleParameter("p_id_dodavatele", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[6].Value = objednavka.Dodavatel.Id;

            return prm;
        }

        private static bool TransactionProcedureCall(Objednavka objednavka,List<ObjednaneZbozi> polozky)
        {
            using (OracleConnection connection = DatabaseConnector.GetConnection())
            {
                connection.Open();
                using (OracleTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        PrepareProcedureCall(objednavka, out string procedureName, out List<OracleParameter> prm);
                        objednavka.Id = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm).Result;
                        foreach (var polozka in polozky)
                        {
                            polozka.Objednavka = objednavka;
                            ObjednaneZboziService.PrepareProcedureCall(polozka, out string procedure, out List<OracleParameter> parameters);
                            var result = DatabaseConnector.ExecuteCommandNonQueryForTransactionAsync(procedure, parameters, connection).Result;
                        }
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        public static bool ProdlouzeniSplatnostiUDodavatele(Dodavatel dodavatel,int days = 7)
        {
            string procedureName = "PRODLUZ_SPLATNOST_U_DODAVATLE_PRO_BUDOUCI";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter("idodavatele", OracleDbType.Varchar2, System.Data.ParameterDirection.InputOutput));
            prm[0].Value = dodavatel.Nazev;

            prm.Add(new OracleParameter("idays", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[1].Value = days;
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm).Result;
            return result == -1;
        }
        public static bool Prijato(Objednavka obj)
        {
            var objednavka = Get(obj);
            if (objednavka == null) {  
                return false; 
            }
            objednavka.Prijato = true;
            return Update(objednavka);
        }

    }
}
