using SemestralniPraceDB2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models
{
    public class BrigadnikService
    {
        ZamestnanecService zamestnanecService;

        public BrigadnikService()
        {
            zamestnanecService = new ZamestnanecService();
        }

        public bool Create(Brigadnik brigadnik)
        {
            //TODO start transaction
            zamestnanecService.Create(brigadnik);
            throw new NotImplementedException();
        }

        public bool Update(Brigadnik brigadnik)
        {
            //TODO start transaction
            zamestnanecService.Update(brigadnik);
            throw new NotImplementedException();
        }
        public bool Delete(Brigadnik brigadnik)
        {
            //TODO start transaction
            //delete from Brigadnici
            zamestnanecService.Delete(brigadnik);
            //Delete where Brigadnik.Id
            throw new NotImplementedException();
        }
        public bool Get(Brigadnik brigadnik)
        {
            zamestnanecService.Get(brigadnik);
            //fill data specific for brigadnik
            throw new NotImplementedException();
            //return zamestnanec;
        }
        public List<Brigadnik> GetAll()
        {
            //TODO start transaction
            var brigadnici = zamestnanecService.GetAll();

            throw new NotImplementedException();
        }
    }
}
