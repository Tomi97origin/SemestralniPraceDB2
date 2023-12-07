using Oracle.ManagedDataAccess.Client;
using SemestralniPraceDB2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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
            procedureName = "pceny";
            prm = MapCenaIntoParams(cena);
        }

        private static List<OracleParameter> MapCenaIntoParams(Cena cena)
        {
            List<OracleParameter> prm = new List<OracleParameter>();

            prm.Add(new OracleParameter("p_id_ceny", OracleDbType.Int32, System.Data.ParameterDirection.InputOutput));
            prm[0].Value = cena.Id <= 0 ? null : cena.Id;

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

        
        public static Cena? Get(Cena cena)
        {
            string sql = "SELECT * FROM ceny WHERE id_ceny = :id_ceny";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_ceny", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = cena.Id;
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToCena).Result;
            return result.Count == 0 ? null : result[0];
        }

        private static Cena MapOracleResultToCena(OracleDataReader reader)
        {
            var collumns = reader.GetColumnSchema().Select(col => col.ColumnName.ToLower()).ToList();
            return new Cena() {
                Id = reader.GetInt32("id_ceny"),
                PlatnostOd = collumns.Contains("od") && !reader.IsDBNull("od") ? reader.GetDateTime("od") : null,
                Castka = collumns.Contains("cena") && !reader.IsDBNull("cena") ? reader.GetDouble("cena") : 0,
                PlatnostDo = collumns.Contains("do") && !reader.IsDBNull("do") ? reader.GetDateTime("do") : null,
                Zbozi = new Zbozi()
                {
                    Id = reader.GetInt32("id_zbozi"),
                    Nazev = collumns.Contains("nazev") && !reader.IsDBNull("nazev") ? reader.GetString("nazev") : string.Empty,
                    Popis = collumns.Contains("popis") && !reader.IsDBNull("popis") ? reader.GetString("popis") : string.Empty,
                    EAN = collumns.Contains("ean") && !reader.IsDBNull("ean") ? reader.GetString("ean") : string.Empty,
                    Kategorie = collumns.Contains("id_kategorie") && !reader.IsDBNull("id_kategorie") ? new Kategorie() { Id = reader.GetInt32("id_kategorie") } : null,
                    Vyrobce = collumns.Contains("id_vyrobce") && !reader.IsDBNull("id_vyrobce") ? new Vyrobce() { Id = reader.GetInt32("id_vyrobce") } : null
                }
            };
        }

        public static Cena? GetCurrent(Zbozi zbozi)
        {
            string sql = "SELECT * FROM ceny c WHERE c.id_ceny = CURRENT_PRICE(:id_zbozi)";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_zbozi", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = zbozi.Id;
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToCena).Result;
            return result.Count == 0 ? null : result[0];
        }

        public static List<Cena> GetAll()
        {
            string sql = "SELECT * FROM historie_cen";
            List<OracleParameter> prm = new();
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToCena).Result;
            return result;
        }

        public static List<Cena> GetAllCurrent()
        {
            string sql = "SELECT * FROM historie_cen WHERE id_ceny = CURRENT_PRICE(id_zbozi)";
            List<OracleParameter> prm = new();
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToCena).Result;
            return result;
        }

        
        public static List<Cena> GetPriceHistory(Zbozi zbozi)
        {
            string sql = "SELECT * FROM historie_cen WHERE id_zbozi = :id_zbozi";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_zbozi", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = zbozi.Id;
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToCena).Result;
            return result;
        }

        public static Cena? GetZboziWithCurentPrice(Zbozi zbozi)
        {
            string sql = "SELECT * FROM zbozi_s_cenou WHERE id_zbozi = :id_zbozi";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_zbozi", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = zbozi.Id;
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToCena).Result;
            return result.Count == 0 ? null : result[0];
        }

        public static List<Cena> GetAllZboziWithCurentPrice()
        {
            List<Cena> res = new();
            using (OracleConnection connection = DatabaseConnector.GetConnection())
            {
                connection.Open();
                using (OracleTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string sql = "SELECT * FROM zbozi_s_cenou";
                        List<OracleParameter> prm = new();
                        var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToCena).Result;
                        foreach (Cena cena in result)
                        {
                            if(cena.Zbozi is not null && cena.Zbozi.Vyrobce is not null)
                            cena.Zbozi.Vyrobce = VyrobceService.GetTransactional(cena.Zbozi.Vyrobce, connection);
                            if (cena.Zbozi is not null && cena.Zbozi.Kategorie is not null)
                                cena.Zbozi.Kategorie = KategorieService.GetTransactional(cena.Zbozi.Kategorie, connection);
                        }
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

        internal static void DeleteFromZbozi(Zbozi zbozi, OracleConnection connection)
        {
            PrepareDeleteCall(zbozi, out string sql, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryForTransactionAsync(sql, prm,connection, CommandType.Text).Result;
        }

        private static void PrepareDeleteCall(Zbozi zbozi, out string sql, out List<OracleParameter> prm)
        {
            sql = "DELETE FROM ceny WHERE id_zbozi = :id_zbozi";
            prm = new();
            prm.Add(new OracleParameter(":id_zbozi", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = zbozi.Id;
        }
    }
}
