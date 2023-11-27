using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SemestralniPraceDB2.Models;
using SemestralniPraceDB2.Models.Entities;
using System.Collections.ObjectModel;
using System.Windows;

namespace SemestralniPraceDB2.ViewModels;

partial class DatabaseExplorerViewModel : BaseViewModel
{

    [ObservableProperty]
    public DBTable? selectedTable;



    [ObservableProperty]
    ObservableCollection<DBTable> tables;


    [ObservableProperty]
    private ObservableCollection<object>? selectedTableData;

    public DatabaseExplorerViewModel()
    {
        tables = new(SystemCatalogService.GetAllTables());
    }


    partial void OnSelectedTableChanged(DBTable? value)
    {
        if (SelectedTable != null)
        {
            switch (SelectedTable.TableName)
            {
                case "ADRESY":
                    SelectedTableData = new(AdresaService.GetAll());
                    break;

                case "BRIGADNICI":
                    SelectedTableData = new(BrigadnikService.GetAll());
                    break;

                case "CENY":
                    SelectedTableData = new(CenaService.GetAll());
                    break;

                case "DODAVATELE":
                    SelectedTableData = new(DodavatelService.GetAll());
                    break;

                case "INVENTARNI_POLOZKY":
                    SelectedTableData = new(InventarniPolozkaService.GetAll());
                    break;

                case "KATEGORIE":
                    SelectedTableData = new(KategorieService.GetAll());
                    break;

                case "LOGY":
                    SelectedTableData = new(LogyService.GetAll());
                    break;

                case "OBJEDNANE_ZBOZI":
                    SelectedTableData = new(ObjednaneZboziService.GetAll());
                    break;

                case "OBJEDNAVKY":
                    SelectedTableData = new(ObjednavkaService.GetAll());
                    break;

                case "OBRAZKYZAMESTNANCU":
                    SelectedTableData = new(ObrazekZamestnanceService.GetAll());
                    break;

                case "PLATBY":
                    SelectedTableData = new(PlatbaService.GetAll());
                    break;

                case "PLNE_UVAZKY":
                    SelectedTableData = new(PlnyUvazekService.GetAll());
                    break;

                case "POKLADNY":
                    SelectedTableData = new(PokladnaService.GetAll());
                    break;

                case "PRODANE_ZBOZI":
                    SelectedTableData = new(ProdaneZboziService.GetAll());
                    break;

                case "ROLE":
                    SelectedTableData = new(RoleService.GetAll());
                    break;

                case "SUPERMARKETY":
                    SelectedTableData = new(SupermarketService.GetAll());
                    break;

                case "UCTENKY":
                    SelectedTableData = new(UctenkaService.GetAll());
                    break;

                case "UZIVATELE":
                    //SelectedTableData = new(UzivateleService.GetAll());
                    break;

                case "VERNOSTNI_KARTY":
                    SelectedTableData = new(VernostniKartaService.GetAll());
                    break;

                case "VYDAVATELE":
                    SelectedTableData = new(VydavatelService.GetAll());
                    break;

                case "VYROBCI":
                    SelectedTableData = new(VyrobceService.GetAll());
                    break;

                case "ZAMESTNANCI":
                    SelectedTableData = new(ZamestnanecService.GetAll());
                    break;

                case "ZBOZI":
                    SelectedTableData = new(ZboziService.GetAll());
                    break;
                default: break;
            }
        }



    }


    [RelayCommand]
    private void AcceptChanges()
    {
        MessageBox.Show("zatím neimplementováno změny");
    }


}
