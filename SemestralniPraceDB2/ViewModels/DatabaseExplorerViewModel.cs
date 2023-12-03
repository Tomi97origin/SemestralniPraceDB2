using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SemestralniPraceDB2.Models;
using SemestralniPraceDB2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
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

    [ObservableProperty]
    private string? textToSearch;

    private List<string> tableDataStrings = new();

    private ObservableCollection<object>? backupSelectedTableData;


    public DatabaseExplorerViewModel()
    {
        Tables = new();
        Refresh();
    }


    partial void OnSelectedTableChanged(DBTable? value)
    {
        RefreshTablesRowCounts();

        NaplnTableDataPodleSelectedTable();
        backupSelectedTableData = SelectedTableData;

        NaplnTableDataStrings();
    }

    private void NaplnTableDataStrings()
    {
        tableDataStrings = new();
        if (SelectedTableData is not null)
        {
            foreach (var i in SelectedTableData)
            {
                var dbEntity = i as IDBEntity;
                tableDataStrings.Add(dbEntity?.DataToText()?.ToUpper() ?? string.Empty);
            }
        }
    }


    private void NaplnTableDataPodleSelectedTable()
    {
        if (SelectedTable != null)
        {
            switch (SelectedTable.TableName)
            {
                case "ADRESY":
                    SelectedTableData = new(AdresaService.GetAll());
                    break;

                case "BRIGADNICI":
                    var seznamBrigadnici = BrigadnikService.GetAll();
                    foreach (var i in seznamBrigadnici)
                    {
                        if (i.Vedouci is not null) i.Vedouci = PlnyUvazekService.Get(i.Vedouci) ?? new();
                        if (i.Supermarket is not null) i.Supermarket = SupermarketService.Get(i.Supermarket) ?? new();
                        if (i.Adresa is not null) i.Adresa = AdresaService.Get(i.Adresa) ?? new();
                        if (i.Role is not null) i.Role = RoleService.Get(i.Role) ?? new();
                        if (i.ObrazekZamestnance is not null) i.ObrazekZamestnance = ObrazekZamestnanceService.Get(i.ObrazekZamestnance) ?? new();
                    }
                    SelectedTableData = new(seznamBrigadnici);
                    break;

                case "CENY":
                    SelectedTableData = new(CenaService.GetAll());
                    break;

                case "DODAVATELE":

                    var seznamDodavatele = DodavatelService.GetAll();
                    foreach (var i in seznamDodavatele)
                    {
                        i.Adresa = AdresaService.Get(i.Adresa) ?? new();
                    }
                    SelectedTableData = new(seznamDodavatele);
                    break;

                case "INVENTARNI_POLOZKY":
                    var seznamInventarniPolozky = InventarniPolozkaService.GetAll();
                    foreach (var i in seznamInventarniPolozky)
                    {
                        i.Supermarket = SupermarketService.Get(i.Supermarket) ?? new();
                        i.Zbozi = ZboziService.Get(i.Zbozi) ?? new();
                    }
                    SelectedTableData = new(seznamInventarniPolozky);
                    break;

                case "KATEGORIE":
                    SelectedTableData = new(KategorieService.GetAll());
                    break;

                case "LOGY":
                    SelectedTableData = new(LogyService.GetAll());
                    break;

                case "OBJEDNANE_ZBOZI":
                    var seznamObjednaneZbozi = ObjednaneZboziService.GetAll();
                    foreach (var i in seznamObjednaneZbozi)
                    {
                        i.Objednavka = ObjednavkaService.Get(i.Objednavka) ?? new();
                        i.Zbozi = ZboziService.Get(i.Zbozi) ?? new();
                    }

                    SelectedTableData = new(seznamObjednaneZbozi);
                    break;

                case "OBJEDNAVKY":
                    var seznamObjednavka = ObjednavkaService.GetAll();
                    foreach (var i in seznamObjednavka)
                    {
                        i.Supermarket = SupermarketService.Get(i.Supermarket) ?? new();
                        i.Dodavatel = DodavatelService.Get(i.Dodavatel) ?? new();
                    }
                    SelectedTableData = new(seznamObjednavka);
                    break;

                case "OBRAZKYZAMESTNANCU":
                    SelectedTableData = new(ObrazekZamestnanceService.GetAll());
                    break;

                case "PLATBY":
                    var seznamPlatby = PlatbaService.GetAll();

                    foreach (var i in seznamPlatby)
                    {
                        if (i.Vydavatel is not null) i.Vydavatel = VydavatelService.Get(i.Vydavatel) ?? new();
                        if (i.Vernostni_Karta is not null) i.Vernostni_Karta = VernostniKartaService.Get(i.Vernostni_Karta) ?? new();
                    }
                    SelectedTableData = new(seznamPlatby);
                    break;

                case "PLNE_UVAZKY":
                    var seznamPlneUvazky = PlnyUvazekService.GetAll();
                    foreach (var i in seznamPlneUvazky)
                    {
                        if (i.Vedouci is not null) i.Vedouci = PlnyUvazekService.Get(i.Vedouci) ?? new();
                        if (i.Supermarket is not null) i.Supermarket = SupermarketService.Get(i.Supermarket) ?? new();
                        if (i.Adresa is not null) i.Adresa = AdresaService.Get(i.Adresa) ?? new();
                        if (i.Role is not null) i.Role = RoleService.Get(i.Role) ?? new();
                        if (i.ObrazekZamestnance is not null) i.ObrazekZamestnance = ObrazekZamestnanceService.Get(i.ObrazekZamestnance) ?? new();
                    }
                    SelectedTableData = new(seznamPlneUvazky);
                    break;

                case "POKLADNY":
                    var seznamPokladny = PokladnaService.GetAll();
                    foreach (var i in seznamPokladny)
                    {
                        if (i.Supermarket is not null) i.Supermarket = SupermarketService.Get(i.Supermarket) ?? new();
                    }
                    SelectedTableData = new(seznamPokladny);
                    break;

                case "PRODANE_ZBOZI":
                    var seznamProdaneZbozi = ProdaneZboziService.GetAll();
                    foreach (var i in seznamProdaneZbozi)
                    {
                        if (i.Zbozi is not null) i.Zbozi = ZboziService.Get(i.Zbozi) ?? new();
                        if (i.Uctenka is not null) i.Uctenka = UctenkaService.Get(i.Uctenka) ?? new();
                    }
                    SelectedTableData = new(seznamProdaneZbozi);
                    break;

                case "ROLE":
                    SelectedTableData = new(RoleService.GetAll());
                    break;

                case "SUPERMARKETY":
                    var supermarkety = SupermarketService.GetAll();
                    foreach (var item in supermarkety)
                    {
                        item.Adresa = AdresaService.Get(item.Adresa) ?? new();
                    }
                    SelectedTableData = new(supermarkety);
                    break;

                case "UCTENKY":
                    var seznamUctenky = UctenkaService.GetAll();
                    foreach (var i in seznamUctenky)
                    {
                        if (i.Pokladna is not null) i.Pokladna = PokladnaService.Get(i.Pokladna) ?? new();
                        if (i.Pokladna?.Supermarket is not null) i.Pokladna.Supermarket = SupermarketService.Get(i.Pokladna.Supermarket) ?? new();
                        if (i.Platba is not null) i.Platba = PlatbaService.Get(i.Platba) ?? new();
                    }
                    SelectedTableData = new(seznamUctenky);
                    break;

                case "UZIVATELE":
                    SelectedTableData = new(UzivateleService.GetAll());
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
                    var seznamZamestnanci = ZamestnanecService.GetAll();
                    foreach (var i in seznamZamestnanci)
                    {
                        if (i.Supermarket is not null) i.Supermarket = SupermarketService.Get(i.Supermarket) ?? new();
                        if (i.Adresa is not null) i.Adresa = AdresaService.Get(i.Adresa) ?? new();
                        if (i.Role is not null) i.Role = RoleService.Get(i.Role) ?? new();
                        if (i.ObrazekZamestnance is not null) i.ObrazekZamestnance = ObrazekZamestnanceService.Get(i.ObrazekZamestnance) ?? new();
                    }
                    SelectedTableData = new(seznamZamestnanci);
                    break;

                case "ZBOZI":
                    var seznamZbozi = ZboziService.GetAll();
                    foreach (var i in seznamZbozi)
                    {
                        if (i.Kategorie is not null) i.Kategorie = KategorieService.Get(i.Kategorie) ?? new();
                        if (i.Vyrobce is not null) i.Vyrobce = VyrobceService.Get(i.Vyrobce) ?? new();
                    }
                    SelectedTableData = new(seznamZbozi);
                    break;
                default: break;
            }
        }
    }

    private void RefreshTablesRowCounts()
    {
        var freshDBTables = SystemCatalogService.GetAllTables();
        freshDBTables.Sort((x, y) => x.TableName.CompareTo(y.TableName));

        for (int i = 0; i < freshDBTables.Count; i++)
        {
            Tables[i].RowCount = freshDBTables[i].RowCount;
        }
    }

    [RelayCommand]
    private void FilterTableData()
    {
        if (TextToSearch is null)
        {
            CancelFilter();
            return;
        }

        List<int> indexySNalezem = new();
        //string pattern = $"@\"{TextToSearch}\"";
        string pattern = $@"\b\w*{TextToSearch?.ToUpper()}\w*\b";

        // Pro každý řetězec v listu
        for (int i = 0; i < tableDataStrings.Count; i++)
        {
            // Vyhledání shody s regulárním výrazem
            MatchCollection matches = Regex.Matches(tableDataStrings[i], pattern);

            // Pokud byla shoda nalezena, ukládám index
            if (matches.Count > 0)
            {
                indexySNalezem.Add(i);
            }
        }

        //vymazání SelectedTableData
        SelectedTableData = new();

        //naplnění SelectedTableData z backupSelectedTableData podle nalezených indexů
        if (backupSelectedTableData is not null)
        {
            for (int i = 0; i < backupSelectedTableData.Count; i++)
            {
                if(indexySNalezem.Contains(i))
                {
                    SelectedTableData.Add(backupSelectedTableData[i]);
                }
            }
        }

    }


    [RelayCommand]
    private void CancelFilter()
    {
        SelectedTableData = backupSelectedTableData;
    }

    [RelayCommand]
    private void DeleteRecord()
    {
        MessageBox.Show("zatím neimplementováno");
    }

    [RelayCommand]
    private void AcceptChanges()
    {
        MessageBox.Show("zatím neimplementováno");
    }

    internal void Refresh()
    {
        var dbTables = SystemCatalogService.GetAllTables();
        dbTables.Sort((x, y) => x.TableName.CompareTo(y.TableName));
        Tables = new(dbTables);
    }
}
