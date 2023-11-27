﻿using CommunityToolkit.Mvvm.ComponentModel;
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
        var dbTables = SystemCatalogService.GetAllTables();
        dbTables.Sort((x, y) => x.TableName.CompareTo(y.TableName));
        tables = new(dbTables);
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
                    SelectedTableData = new(PlatbaService.GetAll());
                    break;

                case "PLNE_UVAZKY":
                    var seznamPlneUvazky = PlnyUvazekService.GetAll();
                    foreach(var i in  seznamPlneUvazky)
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
                    SelectedTableData = new(PokladnaService.GetAll());
                    break;

                case "PRODANE_ZBOZI":
                    SelectedTableData = new(ProdaneZboziService.GetAll());
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
                    var seznamUctenka = UctenkaService.GetAll();
                    foreach (var i in seznamUctenka)
                    {
                        i.Platba = PlatbaService.Get(i.Platba) ?? new();
                    }
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
                    var seznamZamestnanci = ZamestnanecService.GetAll();
                    foreach (var i in seznamZamestnanci)
                    {
                        if (i.Vedouci is not null) i.Vedouci = PlnyUvazekService.Get(i.Vedouci) ?? new();
                        if (i.Supermarket is not null) i.Supermarket = SupermarketService.Get(i.Supermarket) ?? new();
                        if (i.Adresa is not null) i.Adresa = AdresaService.Get(i.Adresa) ?? new();
                        if (i.Role is not null) i.Role = RoleService.Get(i.Role) ?? new();
                        if (i.ObrazekZamestnance is not null) i.ObrazekZamestnance = ObrazekZamestnanceService.Get(i.ObrazekZamestnance) ?? new();
                    }
                    SelectedTableData = new(seznamZamestnanci);
                    break;

                case "ZBOZI":
                    SelectedTableData = new(ZboziService.GetAll());
                    break;
                default: break;
            }
        }



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


}
