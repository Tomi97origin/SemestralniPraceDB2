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
    public static class SystemCatalogService
    {
        public static List<DBTable> GetAllTables()
        {
            var tables = new List<DBTable>();
            using (OracleConnection connection = DatabaseConnector.GetConnection())
            {
                connection.Open();
                using (OracleTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string sql = "SELECT t.* FROM user_tables t";
                        List<OracleParameter> prm = new();
                        var result = DatabaseConnector.ExecuteCommandQueryForTransactionAsync(sql, prm,connection, MapOracleResultToTable).Result;
                        foreach(DBTable table in result)
                        {
                            sql = "SELECT count(*) as  row_count  FROM " + table.TableName+" t";
                            prm = new();
                            var res = DatabaseConnector.ExecuteCommandQueryForTransactionAsync(sql, prm, connection, (reader) => reader.GetInt32("row_count") ).Result;
                            table.RowCount = res[0];
                        }
                        tables.AddRange(result);
                        return tables;
                    }
                    catch (Exception ex)
                    {
                        return tables;
                    }
                }
            }
        }

        private static DBTable MapOracleResultToTable(OracleDataReader reader)
        {
            return new DBTable()
            {
                TableName = reader.GetString("table_name")
            };
        }
    }
}
