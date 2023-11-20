using Oracle.ManagedDataAccess.Client;
using SemestralniPraceDB2.Models.Entities;
using System;
using System.Collections.Generic;
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
            return result.Result >= 0;
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
            return result.Result >= 0;
        }
        //TODO 
        public static bool Delete(ObrazekZamestnance obrazek)
        {
            throw new NotImplementedException();
        }
        //TODO
        public static bool Get(ObrazekZamestnance obrazek)
        {
            throw new NotImplementedException();
        }
        //TODO
        public static List<ObrazekZamestnance> GetAll()
        {
            throw new NotImplementedException();
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
    }
}
