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


        public static string GetFromDatabase()
        {
            string? dbResult = string.Empty;

            string username = "st64515";
            string password = "hodiny";
            string hostname = "fei-sql3.upceucebny.cz";
            int port = 1521; // Oracle default port
            string sid = "BDAS";

            string connectionString = $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={hostname})(PORT={port}))(CONNECT_DATA=(SID={sid})));User Id={username};Password={password};";


            //    string connectionString = "Data Source=fei-sql3.upceucebny.cz;User Id=st67084;Password=semestralkadb2;";
            OracleConnection connection = new OracleConnection(connectionString); //ConfigurationManager.ConnectionStrings["mojePripojeni"].ConnectionString);

            try
            {
                connection.Open();
                // Spojení bylo úspěšně navázáno

                string query = "SELECT * FROM AUTO";
                OracleCommand command = new OracleCommand(query, connection);

                try
                {
                    OracleDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        // Zpracování dat z výsledku dotazu
                        //Console.WriteLine(reader["ColumnName"].ToString());
                        dbResult = reader.ToString();
                    }
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
}
}
