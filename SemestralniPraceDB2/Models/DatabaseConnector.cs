using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using System.Data;
using SemestralniPraceDB2.Models.Entities;
using Oracle.ManagedDataAccess.Types;
using System.Data.Common;

namespace SemestralniPraceDB2.Models
{
    public static class DatabaseConnector
    {

        public static OracleConnection GetConnection()
        {
            string connectionString = "User Id=st67084;Password=semestralkadb2;Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=fei-sql3.upceucebny.cz)(PORT=1521))(CONNECT_DATA=(SID=BDAS)(SERVER=DEDICATED)))";
            return new OracleConnection(connectionString);
        }

        public static async Task<List<T>> ExecuteCommandQueryAsync<T>(string query, List<OracleParameter> oracleParameters, Func<OracleDataReader, T> mapResult)
        {
            List<T> resultList = new List<T>();
            try
            {
                using (OracleConnection connection = GetConnection())
                {
                    await connection.OpenAsync();
                    SetOpenConnectionDateFormat(connection);
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
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
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
                    SetOpenConnectionDateFormat(connection);

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        command.CommandType = commandType;
                        command.Parameters.AddRange(oracleParameters.ToArray());

                        try
                        {
                            int result = await command.ExecuteNonQueryAsync();
                            if (oracleParameters.Count > 0 && oracleParameters[0].Direction == ParameterDirection.InputOutput)
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
            SetOpenConnectionDateFormat(connection);
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

        public static async Task<int> ExecuteCommandNonQueryForTransactionAsync(string query, List<OracleParameter> oracleParameters, OracleConnection connection, CommandType commandType = CommandType.StoredProcedure)
        {
            SetOpenConnectionDateFormat(connection);
            using (OracleCommand command = new OracleCommand(query, connection))
            {
                command.Parameters.AddRange(oracleParameters.ToArray());
                command.CommandType = commandType;
                var result = await command.ExecuteNonQueryAsync();
                if (oracleParameters[0].Direction == ParameterDirection.InputOutput)
                {
                    OracleDecimal returnId = (OracleDecimal)command.Parameters[0].Value;
                    return returnId.ToInt32();
                }
                return result;
            }
        }

        private static void SetOpenConnectionDateFormat(OracleConnection connection)
        {
            OracleGlobalization info = connection.GetSessionInfo();
            info.DateFormat = "YYYY-MM-DD\"T\"HH24:mi:ss\"Z\"";
            connection.SetSessionInfo(info);
        }
    }
}
