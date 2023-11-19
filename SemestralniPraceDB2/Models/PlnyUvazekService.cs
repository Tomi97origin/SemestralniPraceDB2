using Oracle.ManagedDataAccess.Client;
using SemestralniPraceDB2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models
{
    public class PlnyUvazekService
    {
        
        public bool Create(PlnyUvazek zamestnanec)
        {
            //TODO start transaction

            throw new NotImplementedException();
        }

        public bool Update(PlnyUvazek zamestnanec)
        {
            //TODO start transaction
            throw new NotImplementedException();
        }
        public bool Delete(PlnyUvazek zamestnanec)
        {
            //TODO start transaction
            //delete from PlnyUvazek
            //Delete where zamestnanec.Id
            throw new NotImplementedException();
        }
        public bool Get(PlnyUvazek zamestnanec)
        {
            //fill data specific for zamestnanec
            throw new NotImplementedException();
            //return zamestnanec;
        }
        public List<PlnyUvazek> GetAll()
        {
            //TODO start transaction

            throw new NotImplementedException();
        }

        internal static void PrepareProcedureCall(PlnyUvazek zamestnanec, out string prom, out List<OracleParameter> param)
        {
            throw new NotImplementedException();
        }
    }
}
