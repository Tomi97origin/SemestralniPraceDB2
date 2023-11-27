using CommunityToolkit.Mvvm.ComponentModel;
using SemestralniPraceDB2.Models;
using SemestralniPraceDB2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.ViewModels
{
    partial class SystemCatalogViewModel : BaseViewModel
    {
        [ObservableProperty]
        ObservableCollection<CatalogItem> systemCatalogData;

        public SystemCatalogViewModel()
        {
            var dbObjects = SystemCatalogService.GetAll();
            systemCatalogData = new(dbObjects);

        }
    }
}
