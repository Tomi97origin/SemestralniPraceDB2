﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using System.Data;
using SemestralniPraceDB2.Models.Entities;
using Oracle.ManagedDataAccess.Types;

namespace SemestralniPraceDB2.Models
{
    public static class DatabaseConnector
    {

        public static OracleConnection GetConnection()
        {
            string connectionString = "User Id=st67084;Password=semestralkadb2;Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=fei-sql3.upceucebny.cz)(PORT=1521))(CONNECT_DATA=(SID=BDAS)(SERVER=DEDICATED)))";
            return new OracleConnection(connectionString);
        }
        public static string GetFromDatabase()
        {
            string? dbResult = string.Empty;
            int x = -1;
            using (OracleConnection connection = DatabaseConnector.GetConnection())
            {
                connection.Open();

                // Start a transaction
                using (OracleTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var adresa = new Adresa();
                        AdresaService.PrepareProcedureCall(adresa, out string prom, out List<OracleParameter> param);
                        adresa.Id = DatabaseConnector.ExecuteCommandProcedureForTransactionAsync(prom, param, connection).Result;
                        prom = "prole";
                        param.Clear();
                        param.Add(new OracleParameter("returnId", OracleDbType.Int32, System.Data.ParameterDirection.InputOutput));
                        param.Add(new OracleParameter("p_nazev", OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
                        param[1].Value = adresa.Id.ToString();
                        x = DatabaseConnector.ExecuteCommandProcedureForTransactionAsync(prom, param, connection).Result;
                        // Commit the transaction if all commands are successful
                        transaction.Commit();
                        Console.WriteLine("Transaction committed successfully");
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine($"Error {ex.Message}");
                        // An error occurred, rollback the transaction
                        transaction.Rollback();
                    }
                }
            }
            dbResult = x.ToString();
            /*string query = "SELECT TABULKA,OPERACE FROM LOGY";
            List<OracleParameter> prm = new();
            var x = ExecuteCommandQueryAsync(query, prm, Map).Result;
            dbResult = x.Count == 0 ? "Nenalezen" : x[0];*/
            return dbResult;
        }

        static string Map(OracleDataReader reader)
        {
            return reader.GetString("TABULKA");
        }


        public static async Task<List<T>> ExecuteCommandQueryAsync<T>(string query, List<OracleParameter> oracleParameters, Func<OracleDataReader, T> mapResult)
        {
            List<T> resultList = new List<T>();
            try
            {
                using (OracleConnection connection = GetConnection())
                {
                    await connection.OpenAsync();

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        command.CommandType = CommandType.Text;
                        if (oracleParameters != null && oracleParameters.Count > 0)
                        {
                            command.Parameters.AddRange(oracleParameters.ToArray());
                        }


                        try
                        {
                            using (OracleDataReader reader = (OracleDataReader)await command.ExecuteReaderAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    T result = mapResult(reader);
                                    resultList.Add(result);
                                }
                            }

                            return resultList;
                        }
                        catch (Exception)
                        {
                            return new List<T>();
                        }
                    }
                }
            }
            catch (Exception)
            {
                return new List<T>();
            }

        }


        public static async Task<int> ExecuteCommandNonQueryAsync(string query, List<OracleParameter> oracleParameters, CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                using (OracleConnection connection = GetConnection())
                {
                    await connection.OpenAsync();

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        command.CommandType = commandType;
                        command.Parameters.AddRange(oracleParameters.ToArray());

                        try
                        {
                            int result = await command.ExecuteNonQueryAsync();
                            if (oracleParameters[0].Direction == ParameterDirection.InputOutput)
                            {
                                OracleDecimal returnId = (OracleDecimal)command.Parameters[0].Value;
                                return returnId.ToInt32();
                            }
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

        public static async Task<List<T>> ExecuteCommandQueryForTransactionAsync<T>(string query, List<OracleParameter> oracleParameters, OracleConnection connection, Func<OracleDataReader, T> mapResult)
        {
            List<T> resultList = new List<T>();
            using (OracleCommand command = new OracleCommand(query, connection))
            {
                command.Parameters.AddRange(oracleParameters.ToArray());
                command.CommandType = CommandType.Text;

                using (OracleDataReader reader = (OracleDataReader)await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        T result = mapResult(reader);
                        resultList.Add(result);
                    }
                }
            }
            return resultList;
        }

        public static async Task<int> ExecuteCommandProcedureForTransactionAsync(string query, List<OracleParameter> oracleParameters, OracleConnection connection)
        {
            using (OracleCommand command = new OracleCommand(query, connection))
            {
                command.Parameters.AddRange(oracleParameters.ToArray());
                command.CommandType = CommandType.StoredProcedure;
                var result = await command.ExecuteNonQueryAsync();
                if (oracleParameters[0].Direction == ParameterDirection.InputOutput)
                {
                    OracleDecimal returnId = (OracleDecimal)command.Parameters[0].Value;
                    return returnId.ToInt32();
                }
                return result;
            }
        }
    }
}
