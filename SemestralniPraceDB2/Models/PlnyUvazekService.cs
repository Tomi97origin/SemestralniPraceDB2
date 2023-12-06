using Oracle.ManagedDataAccess.Client;
using SemestralniPraceDB2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SemestralniPraceDB2.Models
{
    public static class PlnyUvazekService
    {
        
        public static bool Create(PlnyUvazek zamestnanec)
        {
            PrepareProcedureCall(zamestnanec, out string procedureName, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm).Result;
            zamestnanec.Id = result;
            return result > 0;
        }

        public static bool Update(PlnyUvazek zamestnanec)
        {
            PrepareProcedureCall(zamestnanec, out string procedureName, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm).Result;
            return result > 0;
        }

        /*public bool Delete(PlnyUvazek zamestnanec)
            throw new NotImplementedException();
        }*/

        public static PlnyUvazek? Get(PlnyUvazek zamestnanec)
        {
            string sql = "SELECT * FROM zamestnanci JOIN plne_uvazky USING(id_zamestnance) WHERE id_zamestnance = :id_zamestnance";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_zamestnance", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = zamestnanec.Id;
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToPlnyUvazek).Result;
            return result.Count == 0 ? null : result[0];
        }
        public static List<PlnyUvazek> GetAll()
        {
            string sql = "SELECT * FROM zamestnanci JOIN plne_uvazky USING(id_zamestnance)";
            List<OracleParameter> prm = new();
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToPlnyUvazek).Result;
            return result;
        }

        internal static void PrepareProcedureCall(PlnyUvazek zamestnanec, out string procedureName, out List<OracleParameter> param)
        {
            procedureName = "pzamestnanci_plne_uvazky";
            param = MapPlnyUvazekIntoParams(zamestnanec);
        }

        private static List<OracleParameter> MapPlnyUvazekIntoParams(PlnyUvazek zamestnanec)
        {
            List<OracleParameter> prm = new List<OracleParameter>();

            prm.Add(new OracleParameter("p_id_zamestnance", OracleDbType.Int32, System.Data.ParameterDirection.InputOutput));
            prm[0].Value = zamestnanec.Id <= 0 ? null : zamestnanec.Id;

            prm.Add(new OracleParameter("p_jmeno", OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            prm[1].Value = zamestnanec.Jmeno;

            prm.Add(new OracleParameter("p_prijmeni", OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            prm[2].Value = zamestnanec.Prijmeni;

            prm.Add(new OracleParameter("p_osobni_cislo", OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            prm[3].Value = zamestnanec.OsobniCislo;

            prm.Add(new OracleParameter("p_tel_cislo", OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            prm[4].Value = zamestnanec.TelCislo;

            prm.Add(new OracleParameter("p_nastup", OracleDbType.Date, System.Data.ParameterDirection.Input));
            prm[5].Value = zamestnanec.Nastup;

            prm.Add(new OracleParameter("p_typ_uvazku", OracleDbType.Int16, System.Data.ParameterDirection.Input));
            prm[6].Value = zamestnanec.TypUvazku;

            prm.Add(new OracleParameter("p_id_vedouci", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[7].Value = zamestnanec.Vedouci is null ? null : zamestnanec.Vedouci.Id;

            prm.Add(new OracleParameter("p_id_supermarketu", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[8].Value = zamestnanec.Supermarket is null ? null : zamestnanec.Supermarket.Id;

            prm.Add(new OracleParameter("p_id_adresy", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[9].Value = zamestnanec.Adresa.Id;

            prm.Add(new OracleParameter("p_id_role", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[10].Value = zamestnanec.Role is null ? null : zamestnanec.Role.Id;

            prm.Add(new OracleParameter("p_id_obrazku", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[11].Value = zamestnanec.ObrazekZamestnance is null ? null : zamestnanec.ObrazekZamestnance.Id;

            prm.Add(new OracleParameter("p_plat", OracleDbType.Double, System.Data.ParameterDirection.Input));
            prm[12].Value = zamestnanec.Plat;

            prm.Add(new OracleParameter("p_platnost_do", OracleDbType.Date, System.Data.ParameterDirection.Input));
            prm[13].Value = zamestnanec.PlatnostDo;

            return prm;
        }

        private static PlnyUvazek MapOracleResultToPlnyUvazek(OracleDataReader result)
        {
            return new PlnyUvazek
            {
                Id = result.GetInt32("id_zamestnance"),
                Jmeno = result.GetString("jmeno"),
                Prijmeni = result.GetString("prijmeni"),
                OsobniCislo = result.GetString("osobni_cislo"),
                TelCislo = result.GetString("tel_cislo"),
                Nastup = result.GetDateTime("nastup"),
                TypUvazku = result.GetInt16("typ_uvazku"),
                Vedouci = result.IsDBNull("id_vedouci") ? null : new PlnyUvazek
                {
                    Id = result.GetInt32("id_vedouci")
                },
                Supermarket = result.IsDBNull("id_supermarketu") ? null : new Supermarket
                {
                    Id = result.GetInt32("id_supermarketu")
                },
                Adresa = new Adresa
                {
                    Id = result.GetInt32("id_adresy")
                },
                Role = result.IsDBNull("id_role") ? null : new Role
                {
                    Id = result.GetInt32("id_role")
                },
                ObrazekZamestnance = result.IsDBNull("id_obrazku") ? null : new ObrazekZamestnance
                {
                    Id = result.GetInt32("id_obrazku")
                },
                Plat = result.GetDouble("plat"),
                PlatnostDo = result.GetDateTime("platnost_do")
            };

        }

        internal static void PrepareDeleteCall(PlnyUvazek? plnyUvazek, out string sql, out List<OracleParameter> prm)
        {
            sql = "DELETE FROM plne_uvazky WHERE id_zamestnance = p_id_zamestnance";
            prm = new List<OracleParameter>();
            prm.Add(new OracleParameter("p_id_zamestnance", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = plnyUvazek.Id;
        }
    }
}
