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
        ZamestnanecService zamestnanecService;

        public PlnyUvazekService()
        {
            zamestnanecService = new ZamestnanecService();
        }

        public bool Create(PlnyUvazek zamestnanec)
        {
            //TODO start transaction
            zamestnanecService.Create(zamestnanec);

            throw new NotImplementedException();
        }

        public bool Update(PlnyUvazek zamestnanec)
        {
            //TODO start transaction
            zamestnanecService.Update(zamestnanec);
            throw new NotImplementedException();
        }
        public bool Delete(PlnyUvazek zamestnanec)
        {
            //TODO start transaction
            //delete from PlnyUvazek
            zamestnanecService.Delete(zamestnanec);
            //Delete where zamestnanec.Id
            throw new NotImplementedException();
        }
        public bool Get(PlnyUvazek zamestnanec)
        {
            zamestnanecService.Get(zamestnanec);
            //fill data specific for zamestnanec
            throw new NotImplementedException();
            //return zamestnanec;
        }
        public List<PlnyUvazek> GetAll()
        {
            //TODO start transaction
            var brigadnici = zamestnanecService.GetAll();

            throw new NotImplementedException();
        }
    }
}
