using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;


namespace SemestralniPraceDB2.Models
{
    public class DatabaseConnector
    {

        public static OracleConnection GetConnection()
        {
            string connectionString = "User Id=st67084;Password=semestralkadb2;Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=fei-sql3.upceucebny.cz)(PORT=1521))(CONNECT_DATA=(SID=BDAS)(SERVER=DEDICATED)))";
            return new OracleConnection(connectionString);
        }
        public static string GetFromDatabase()
        {
            string? dbResult = string.Empty;
            OracleConnection connection = GetConnection();
            try
            {
                connection.Open();
                // Spojení bylo úspěšně navázáno

                string query = "SELECT * FROM AUTO";
                OracleCommand command = new OracleCommand(query, connection);

                try
                {
                    OracleDataReader reader = command.ExecuteReader();
                    StringBuilder sb = new StringBuilder();

                    reader.Read();
                    for (int i = 0; i< 1; i++)
                    {
                        // Zpracování dat z výsledku dotazu
                        //Console.WriteLine(reader["ColumnName"].ToString());
                        
                        sb.Append(reader[i].ToString());
                    }
                    dbResult = sb.ToString();
                }
                catch (Exception ex)
                {
                    dbResult = "Chyba: " + ex.Message;
                }



            }
            catch (Exception ex)
            {
                // Chyba při připojování k databázi
                dbResult = "Chyba při připojování k databázi: " + ex.Message;
            }
            finally
            {
                // Vždy uzavírejte spojení po skončení práce s ním
                connection.Close();
            }

            return dbResult;
        }


        public static OracleDataReader ExecuteCommand(string query, OracleParameter[] oracleParameters)
        {
            OracleConnection connection = GetConnection();
            try
            {
                connection.Open();
                // Spojení bylo úspěšně navázáno
                OracleCommand command = new OracleCommand(query, connection);
                command.Parameters.Add(oracleParameters);
                try
                {
                    OracleDataReader reader = command.ExecuteReader();
                    return reader;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                // Vždy uzavírejte spojení po skončení práce s ním
                connection.Close();
            }

        }
    }
}
