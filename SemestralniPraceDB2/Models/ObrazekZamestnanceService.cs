using Oracle.ManagedDataAccess.Client;
using SemestralniPraceDB2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models
{
    public static class ObrazekZamestnanceService
    {
        public static bool Create(ObrazekZamestnance obrazek)
        {
            PrepareProcedureCall(obrazek, out string procedureName, out List<OracleParameter> prm);
            prm[0].Value = null;
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm);
            obrazek.Id = result.Result;
            return result.Result > 0;
        }

        public static void PrepareProcedureCall(ObrazekZamestnance obrazek, out string procedureName, out List<OracleParameter> prm)
        {
            procedureName = "pobrazkyzamestnancu";
            prm = MapObrazekZamestnanceIntoParams(obrazek);
        }

        public static bool Update(ObrazekZamestnance obrazek)
        {
            PrepareProcedureCall(obrazek, out string procedureName, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm);
            return result.Result > 0;
        }

        public static bool Delete(ObrazekZamestnance obrazek)
        {
            PrepareDeleteCall(obrazek, out string prom, out List<OracleParameter> param);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(prom, param, System.Data.CommandType.Text).Result;
            return result == 1;
        }
        
        public static ObrazekZamestnance? Get(ObrazekZamestnance obrazek)
        {
            string sql = "SELECT * FROM obrazkyzamestnancu WHERE id_obrazku = :id_obrazku";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_obrazku", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = obrazek.Id;
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToObrazekZamestnance).Result;
            return result.Count > 0 ? result[0] : null;
        }
        
        public static List<ObrazekZamestnance> GetAll()
        {
            string sql = "SELECT * FROM obrazkyzamestnancu";
            List<OracleParameter> prm = new();
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToObrazekZamestnance).Result;
            return result;
        }

        private static ObrazekZamestnance MapOracleResultToObrazekZamestnance(OracleDataReader reader)
        {
            ObrazekZamestnance obrazek = new ObrazekZamestnance();

            obrazek.Id = reader.GetInt32("id_obrazku");
            obrazek.Soubor = reader.GetString("soubor");

            if (!reader.IsDBNull("image"))
            {
                byte[] imageBytes = (byte[])reader.GetValue("image");
                obrazek.Image = ConvertBytesToImage(imageBytes);
            }

            return obrazek;
        }

        private static Image? ConvertBytesToImage(byte[] imageBytes)
        {
            if (imageBytes == null || imageBytes.Length == 0)
            {
                return null;
            }

            try
            {
                using (MemoryStream memoryStream = new MemoryStream(imageBytes))
                {
                    Image image = Image.FromStream(memoryStream);
                    Bitmap pngImage = new Bitmap(image);
                    return pngImage;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error converting byte array to image: {ex.Message}");
                return null;
            }
        }


        private static List<OracleParameter> MapObrazekZamestnanceIntoParams(ObrazekZamestnance obrazek)
        {
            List<OracleParameter> prm = new List<OracleParameter>();

            prm.Add(new OracleParameter("p_id_obrazku", OracleDbType.Int32, System.Data.ParameterDirection.InputOutput));
            prm[0].Value = obrazek.Id <= 0 ? null : obrazek.Id;

            prm.Add(new OracleParameter("p_soubor", OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            prm[1].Value = obrazek.Soubor;

            prm.Add(new OracleParameter("p_image", OracleDbType.Blob, System.Data.ParameterDirection.Input));
            prm[2].Value = ConvertImageToByteArray(obrazek.Image, obrazek.Soubor);

            return prm;
        }

        private static byte[] ConvertImageToByteArray(Image image,string file)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

        public static void PrepareDeleteCall(ObrazekZamestnance obrazekZamestnance, out string prom, out List<OracleParameter> param)
        {
            prom = "DELETE FROM obrazkyzamestnancu WHERE id_obrazku = :id_obrazku";
            param = new List<OracleParameter>();
            param.Add(new OracleParameter(":id_obrazku", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            param[0].Value = obrazekZamestnance.Id;
        }
    }
}
