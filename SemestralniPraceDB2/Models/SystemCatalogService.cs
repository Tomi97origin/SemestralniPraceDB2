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
    public static class SystemCatalogService
    {
        public static List<DBTable> GetAllTables()
        {
            var tables = new List<DBTable>();
            string sql = "SELECT table_name,ROWCOUNT(table_name) as row_count FROM user_tables t";
            List<OracleParameter> prm = new();
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToTable).Result;
            tables.AddRange(result);
            return tables;
        }

        private static DBTable MapOracleResultToTable(OracleDataReader reader)
        {
            return new DBTable()
            {
                TableName = reader.GetString("table_name"),
                RowCount = reader.GetInt32("row_count")
            };
        }

        public static List<CatalogItem> GetAll()
        {
            List<CatalogItem> items = new();
            string sql = "SELECT * FROM all_objects WHERE owner = USER";
            List<OracleParameter> prm = new();
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToCatalogItem).Result;
            items.AddRange(result);
            return items;
        }

        private static CatalogItem MapOracleResultToCatalogItem(OracleDataReader reader)
        {
            return new CatalogItem()
            {
                Name = reader.GetString("object_name"),
                Typ = reader.GetString("object_type"),
            };
        }
    }
}
