﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using System.Data;
using SemestralniPraceDB2.Models.Entities;

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
            string query = "SELECT first_name,last_name FROM  A_EMPLOYEES.employees WHERE employee_id = :employee_id";
            OracleParameter[] prm = new OracleParameter[1];
            prm[0] = new OracleParameter(":employee_id", OracleDbType.Int32, System.Data.ParameterDirection.Input);
            prm[0].Value = 100;
            var x = ExecuteCommandQueryAsync(query, prm, Map).Result;
            dbResult = x.Count == 0 ? "Nenalezen":x[0];
            /*
            OracleParameter[] prm = new OracleParameter[0] { };
            var x = ExecuteCommandNonQueryAsync("TRPASLIK_PLATY", prm).Result;
            dbResult = x.ToString();*/
            return dbResult;
        }
            static string Map(OracleDataReader reader)
        {
           return reader.GetString(0) + reader.GetString(1);
        }


        public static async Task<List<T>> ExecuteCommandQueryAsync<T>(string query, OracleParameter[] oracleParameters, Func<OracleDataReader, T> mapResult)
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
                        command.Parameters.AddRange(oracleParameters);

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
