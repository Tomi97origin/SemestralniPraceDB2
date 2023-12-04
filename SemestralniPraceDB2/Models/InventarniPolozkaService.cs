using Oracle.ManagedDataAccess.Client;
using SemestralniPraceDB2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace SemestralniPraceDB2.Models
{
    public static class InventarniPolozkaService
    {
        public static bool Create(List<InventarniPolozka> polozky)
        {
            polozky.ForEach(polozka => polozka.Id = 0);
            return TransactionProcedureCall(polozky);
        }

        public static void PrepareProcedureCall(InventarniPolozka polozka, out string procedureName, out List<OracleParameter> prm)
        {
            procedureName = "pinventarni_polozky";
            prm = MapInventarniPolozkaIntoParams(polozka);
        }

        public static bool Update(List<InventarniPolozka> polozky)
        {
            return TransactionProcedureCall(polozky);
        }
        public static bool Delete(InventarniPolozka polozka)
        {
            PrepareDeleteCall(polozka, out string sql, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(sql, prm, CommandType.Text).Result;
            return result == 1;
        }

        public static void PrepareDeleteCall(InventarniPolozka polozka, out string sql, out List<OracleParameter> prm)
        {
            sql = "DELETE FROM inventarni_polozky WHERE id_inventarni_polozky = :id_inventarni_polozky";
            prm = new();
            prm.Add(new OracleParameter(":id_inventarni_polozky", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = polozka.Id;
        }

        public static InventarniPolozka? Get(InventarniPolozka polozka)
        {
            string sql = "SELECT * FROM inventarni_polozky WHERE id_inventarni_polozky = :id_inventarni_polozky";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_inventarni_polozky", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = polozka.Id;
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToInventarniPolozka).Result;
            return result.Count == 0 ? null : result[0];
        }
        public static List<InventarniPolozka> GetAll()
        {
            string sql = "SELECT * FROM inventarni_polozky";
            List<OracleParameter> prm = new();
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToInventarniPolozka).Result;
            return result;
        }

        private static List<OracleParameter> MapInventarniPolozkaIntoParams(InventarniPolozka polozka)
        {
            List<OracleParameter> prm = new List<OracleParameter>();

            prm.Add(new OracleParameter("p_id_inventarni_polozky", OracleDbType.Int32, System.Data.ParameterDirection.InputOutput));
            prm[0].Value = polozka.Id <= 0 ? null : polozka.Id;

            prm.Add(new OracleParameter("p_sklad", OracleDbType.Int16, System.Data.ParameterDirection.Input));
            prm[1].Value = polozka.Sklad;

            prm.Add(new OracleParameter("p_mnozstvi", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[2].Value = polozka.Mnozstvi;

            prm.Add(new OracleParameter("p_oznaceni_pozice", OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            prm[3].Value = polozka.OznaceniPozice;

            prm.Add(new OracleParameter("p_naskladneno", OracleDbType.Date, System.Data.ParameterDirection.Input));
            prm[4].Value = polozka.Naskladneno;

            prm.Add(new OracleParameter("p_id_supermarketu", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[5].Value = polozka.Supermarket.Id;

            prm.Add(new OracleParameter("p_id_zbozi", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[6].Value = polozka.Zbozi.Id;

            return prm;
        }

        private static InventarniPolozka MapOracleResultToInventarniPolozka(OracleDataReader reader)
        {
            return new InventarniPolozka()
            {
                Id = reader.GetInt32("id_inventarni_polozky"),
                Sklad = reader.GetInt16("sklad"),
                Mnozstvi = reader.GetInt32("mnozstvi"),
                OznaceniPozice = reader.GetString("oznaceni_pozice"),
                Naskladneno = reader.GetDateTime("naskladneno"),
                Supermarket = new Supermarket() { Id = reader.GetInt32("id_supermarketu") },
                Zbozi = new Zbozi() { Id = reader.GetInt32("id_zbozi") }
            };

        }

        private static bool TransactionProcedureCall(List<InventarniPolozka> polozky)
        {
            using (OracleConnection connection = DatabaseConnector.GetConnection())
            {
                connection.Open();
                using (OracleTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (InventarniPolozka polozka in polozky)
                        {
                            PrepareProcedureCall(polozka, out string procedure, out List<OracleParameter> parameters);
                            polozka.Id = DatabaseConnector.ExecuteCommandNonQueryForTransactionAsync(procedure, parameters, connection).Result;
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

        private static InventarniPolozkaSCenou MapOracleResultToInventarniPolozkaSCenou(OracleDataReader reader)
        {
            var collumns = reader.GetColumnSchema().Select(col => col.ColumnName.ToLower()).ToList();
            return new InventarniPolozkaSCenou()
            {

                IdInventPolozky = reader.GetInt32("id_inventarni_polozky"),
                IdZbozi = reader.GetInt32("id_zbozi"),
                Nazev = collumns.Contains("nazev") && !reader.IsDBNull("nazev") ? reader.GetString("nazev") : string.Empty,
                Popis = collumns.Contains("popis") && !reader.IsDBNull("popis") ? reader.GetString("popis") : string.Empty,
                EAN = collumns.Contains("ean") && !reader.IsDBNull("ean") ? reader.GetString("ean") : string.Empty,
                Mnozstvi = reader.GetInt32("mnozstvi"),
                Cena = collumns.Contains("cena") && !reader.IsDBNull("cena") ? reader.GetDouble("cena") : 0,
                Kategorie = collumns.Contains("knazev") && !reader.IsDBNull("knazev") ? reader.GetString("knazev") : string.Empty,
                Vyrobce = collumns.Contains("vnazev") && !reader.IsDBNull("vnazev") ? reader.GetString("vnazev") : string.Empty,
            };

        }

        public static List<InventarniPolozkaSCenou> GetAllZboziWithCurentPriceFromInventory(Supermarket supermarket)
        {
            List<InventarniPolozkaSCenou> res = new();
            using (OracleConnection connection = DatabaseConnector.GetConnection())
            {
                connection.Open();
                using (OracleTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string sql = "SELECT i.id_inventarni_polozky,z.id_zbozi,z.nazev,z.popis,z.ean,i.mnozstvi,z.cena,v.nazev as vnazev,k.nazev as knazev FROM zbozi_s_cenou z JOIN inventarni_polozky i ON z.id_zbozi = i.id_zbozi JOIN vyrobci v ON z.id_vyrobce = v.id_vyrobce JOIN kategorie k ON z.id_kategorie = k.id_kategorie WHERE i.id_supermarketu = :id_supermarketu";
                        List<OracleParameter> prm = new();
                        prm.Add(new OracleParameter(":id_supermarketu", OracleDbType.Int32, System.Data.ParameterDirection.Input));
                        prm[0].Value = supermarket.Id;
                        var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToInventarniPolozkaSCenou).Result;
                        res.AddRange(result);
                        return res;
                    }
                    catch (Exception ex)
                    {
                        return res;
                    }
                }
            }
        }

        internal static InventarniPolozka? GetTransactional(InventarniPolozka polozka, OracleConnection connection)
        {
            string sql = "SELECT * FROM inventarni_polozky WHERE id_inventarni_polozky = :id_inventarni_polozky";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_inventarni_polozky", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = polozka.Id;
            var result = DatabaseConnector.ExecuteCommandQueryForTransactionAsync(sql, prm,connection, MapOracleResultToInventarniPolozka).Result;
            return result.Count == 0 ? null : result[0];
        }

        internal static void UpdateTransactional(InventarniPolozka polozka, OracleConnection connection)
        {
            PrepareProcedureCall(polozka, out string procedure, out List<OracleParameter> parameters);
            polozka.Id = DatabaseConnector.ExecuteCommandNonQueryForTransactionAsync(procedure, parameters, connection).Result;
        }

        internal static void DeleteTransactional(InventarniPolozka polozka, OracleConnection connection)
        {
            PrepareDeleteCall(polozka, out string sql, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryForTransactionAsync(sql, prm, connection, CommandType.Text).Result;
        }
    }
}
