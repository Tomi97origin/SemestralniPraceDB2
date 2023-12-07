using Oracle.ManagedDataAccess.Client;
using SemestralniPraceDB2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models
{
    public static class BrigadnikService
    {

        public static bool Create(Brigadnik brigadnik)
        {
            PrepareProcedureCall(brigadnik, out string procedureName, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm).Result;
            brigadnik.Id = result;
            return result > 0;
        }

        public static void PrepareProcedureCall(Brigadnik brigadnik, out string procedureName, out List<OracleParameter> prm)
        {
            procedureName = "pzamestnanci_brigadnici";
            prm = MapBrigadnikIntoParams(brigadnik);
        }

        private static List<OracleParameter> MapBrigadnikIntoParams(Brigadnik brigadnik)
        {
            List<OracleParameter> prm = new List<OracleParameter>();

            prm.Add(new OracleParameter("p_id_zamestnance", OracleDbType.Int32, System.Data.ParameterDirection.InputOutput));
            prm[0].Value = brigadnik.Id <= 0 ? null : brigadnik.Id;

            prm.Add(new OracleParameter("p_jmeno", OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            prm[1].Value = brigadnik.Jmeno;

            prm.Add(new OracleParameter("p_prijmeni", OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            prm[2].Value = brigadnik.Prijmeni;

            prm.Add(new OracleParameter("p_osobni_cislo", OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            prm[3].Value = brigadnik.OsobniCislo;

            prm.Add(new OracleParameter("p_tel_cislo", OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            prm[4].Value = brigadnik.TelCislo;

            prm.Add(new OracleParameter("p_nastup", OracleDbType.Date, System.Data.ParameterDirection.Input));
            prm[5].Value = brigadnik.Nastup;

            prm.Add(new OracleParameter("p_typ_uvazku", OracleDbType.Int16, System.Data.ParameterDirection.Input));
            prm[6].Value = brigadnik.TypUvazku;

            prm.Add(new OracleParameter("p_id_vedouci", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[7].Value = brigadnik.Vedouci is null ? null : brigadnik.Vedouci.Id;

            prm.Add(new OracleParameter("p_id_supermarketu", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[8].Value = brigadnik.Supermarket is null ? null : brigadnik.Supermarket.Id;

            prm.Add(new OracleParameter("p_id_adresy", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[9].Value = brigadnik.Adresa.Id;

            prm.Add(new OracleParameter("p_id_role", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[10].Value = brigadnik.Role is null ? null : brigadnik.Role.Id;

            prm.Add(new OracleParameter("p_id_obrazku", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[11].Value = brigadnik.ObrazekZamestnance is null ? null : brigadnik.ObrazekZamestnance.Id;

            prm.Add(new OracleParameter("p_hodinova_sazba", OracleDbType.Double, System.Data.ParameterDirection.Input));
            prm[12].Value = brigadnik.HodinovaSazba;

            prm.Add(new OracleParameter("p_hodiny", OracleDbType.Double, System.Data.ParameterDirection.Input));
            prm[13].Value = brigadnik.Hodiny;

            return prm;
        }


        public static bool Update(Brigadnik brigadnik)
        {
            PrepareProcedureCall(brigadnik, out string procedureName, out List<OracleParameter> prm);
            var result = DatabaseConnector.ExecuteCommandNonQueryAsync(procedureName, prm).Result;
            return result > 0;
        }

        public static void PrepareDeleteCall(Brigadnik brigadnik, out string sql, out List<OracleParameter> prm)
        {
            sql = "DELETE FROM brigadnici WHERE id_zamestnance = :id_zamestnance";
            prm = new List<OracleParameter>();
            prm.Add(new OracleParameter(":id_zamestnance", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = brigadnik.Id;

        }

        public static Brigadnik? Get(Brigadnik brigadnik)
        {
            string sql = "SELECT * FROM zamestnanci JOIN brigadnici USING(id_zamestnance) WHERE id_zamestnance = :id_zamestnance";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_zamestnance", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = brigadnik.Id;
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToBrigadnik).Result;
            return result.Count == 0 ? null : result[0];
        }
        public static List<Brigadnik> GetAll()
        {
            string sql = "SELECT * FROM zamestnanci JOIN brigadnici USING(id_zamestnance)";
            List<OracleParameter> prm = new();
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToBrigadnik).Result;
            return result;
        }

        private static Brigadnik MapOracleResultToBrigadnik(OracleDataReader result)
        {
            return new Brigadnik
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
                HodinovaSazba = result.GetDouble("hodinova_sazba"),
                Hodiny = result.GetDouble("hodiny")
            };

        }

        internal static Brigadnik? GetFromAdresa(Adresa adresa)
        {
            string sql = "SELECT * FROM zamestnanci JOIN brigadnici USING(id_zamestnance) WHERE id_adresy = :id_adresy";
            List<OracleParameter> prm = new();
            prm.Add(new OracleParameter(":id_adresy", OracleDbType.Int32, System.Data.ParameterDirection.Input));
            prm[0].Value = adresa.Id;
            var result = DatabaseConnector.ExecuteCommandQueryAsync(sql, prm, MapOracleResultToBrigadnik).Result;
            return result.Count == 0 ? null : result[0];
        }
    }
}
