﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using System.Data;

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
                    for (int i = 0; i < 1; i++)
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


        public static async Task<OracleDataReader?> ExecuteCommandQueryAsync(string query, OracleParameter[] oracleParameters)
        {
            try
            {
                using (OracleConnection connection = GetConnection())
                {
                    await connection.OpenAsync();

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddRange(oracleParameters);

                        try
                        {
                            OracleDataReader reader = (OracleDataReader)await command.ExecuteReaderAsync();
                            return reader;
                        }
                        catch (Exception)
                        {
                            return null;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }

        }


        public static async Task<int> ExecuteCommandNonQueryAsync(string query, OracleParameter[] oracleParameters, CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                using (OracleConnection connection = GetConnection())
                {
                    await connection.OpenAsync();

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        command.CommandType = commandType;
                        command.Parameters.AddRange(oracleParameters);

                        try
                        {
                            int result = await command.ExecuteNonQueryAsync();
                            return result;
                        }
                        catch (Exception)
                        {
                            return 0;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
