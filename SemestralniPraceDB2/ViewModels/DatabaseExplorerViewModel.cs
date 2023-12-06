using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SemestralniPraceDB2.Models;
using SemestralniPraceDB2.Models.Entities;
using SemestralniPraceDB2.ViewModels.DialogViewModels;
using SemestralniPraceDB2.Views.DialogWindows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;

namespace SemestralniPraceDB2.ViewModels;

partial class DatabaseExplorerViewModel : BaseViewModel
{

    [ObservableProperty]
    public DBTable? selectedTable;

    [ObservableProperty]
    ObservableCollection<DBTable> tables = new();

    [ObservableProperty]
    private ObservableCollection<object>? selectedTableData;

    [ObservableProperty]
    private string? textToSearch;

    private List<string> tableDataStrings = new();

    private ObservableCollection<object>? backupSelectedTableData;

    [ObservableProperty]
    private IDBEntity? selectedRecord;

    private List<IDBEntity> deletedRecords = new();
    private List<IDBEntity> editedRecords = new();
    private List<IDBEntity> newRecords = new();


    public DatabaseExplorerViewModel()
    {
        Refresh();
    }


    partial void OnSelectedTableChanged(DBTable? value)
    {
        if (deletedRecords.Count > 0 || editedRecords.Count > 0)
        {
            var Result = MessageBox.Show(
                "Máte nepotvrzené změny v tabulce, chcete je zahodit?",
                "Opravdu chcete odejít?",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (Result == MessageBoxResult.No)
            {
                return;
            }
            if (Result == MessageBoxResult.Yes)
            {
                editedRecords = new();
                deletedRecords = new();
                newRecords = new();
            }
        }


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
                if (indexySNalezem.Contains(i))
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
        if (SelectedRecord is null)
        {
            MessageBox.Show("Nejdříve vyberte záznam.", "Pozor", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        else if (SelectedRecord is Log)
        {
            MessageBox.Show("Nelze smazat log.", "Pozor", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        else
        {
            var rec = SelectedRecord;

            deletedRecords.Add(rec);

            SelectedTableData?.Remove(rec);
            backupSelectedTableData?.Remove(rec);

            NaplnTableDataStrings();
        }

    }

    [RelayCommand]
    private void EditRecord()
    {
        if (SelectedRecord is null)
        {
            MessageBox.Show("Nejdříve vyberte záznam.", "Pozor", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        else if (SelectedRecord is Log)
        {
            MessageBox.Show("Nelze editovat log.", "Pozor", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        else
        {
            HandleEditUserRequset();
        }
    }


    [RelayCommand]
    private void AddRecord()
    {
        if (SelectedTable is null)
        {
            MessageBox.Show("Nejdříve vyberte tabulku.", "Pozor", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        else if (SelectedTable.TableName.ToUpper() == "LOGY")
        {
            MessageBox.Show("Nelze přidávat logy.", "Pozor", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        else
        {
            HandleAddUserRequset();
            NaplnTableDataStrings();
        }
    }

    [RelayCommand]
    private void AcceptChanges()
    {
        if (newRecords.Count > 0 || editedRecords.Count > 0 || deletedRecords.Count > 0)
        {
            try
            {
                SendEditedRecordsToDB();
                SendNewRecordsToDB();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Některé povinné pole nebylo vplněno. Některé záznamy nebyly uloženy.");
            }
            SendDeletedRecordsToDB();
            MessageBox.Show("Změny provedeny.");
        }
        else
        {
            MessageBox.Show("Nejsou žádné změny k uložení.");
        }

    }

    [RelayCommand]
    private void RefreshLoadedData()
    {
        OnSelectedTableChanged(SelectedTable);
    }

    [RelayCommand]
    private void CancelChanges()
    {
        if (newRecords.Count > 0 || editedRecords.Count > 0 || deletedRecords.Count > 0)
        {
            newRecords = new();
            editedRecords = new();
            deletedRecords = new();

            OnSelectedTableChanged(SelectedTable);

            MessageBox.Show("Změny zrušeny.");
        }
        else
        {

            MessageBox.Show("Nejsou žádné změny ke zrušení.");
        }

    }


    /* Funcke podle vybraného recordu zavolá příslušné modální okno. Po potvrzení okna
     * se upravená třída načte do pole změn, které se později můžou aplikovat.
     */
    private void HandleEditUserRequset()
    {
        switch (SelectedRecord)
        {
            case Adresa:
                var adresaDialog = new AdresaEditWindow();
                var adresaDialogVM = new AdresaEditWindowViewModel();
                adresaDialogVM.Adresa = SelectedRecord as Adresa;
                adresaDialog.DataContext = adresaDialogVM;
                adresaDialog.ShowDialog();
                if (adresaDialogVM.Adresa is not null)
                {
                    editedRecords.Add(adresaDialogVM.Adresa);
                }
                break;

            case Brigadnik:
                var brigadnikDialog = new BrigadnikEditWindow();
                var brigadnikDialogVM = new BrigadnikEditWindowViewModel(SelectedRecord as Brigadnik);
                brigadnikDialog.DataContext = brigadnikDialogVM;
                brigadnikDialog.ShowDialog();
                if (brigadnikDialogVM.Brigadnik is not null)
                {
                    editedRecords.Add(brigadnikDialogVM.Brigadnik);
                }
                break;

            /*
        case Cena:
            var cenaDialog = new CenaEditWindow();
            var cenaDialogVM = new CenaEditWindowViewModel();
            cenaDialogVM.Cena = SelectedRecord as Cena;
            cenaDialog.DataContext = cenaDialogVM;
            cenaDialog.ShowDialog();
            if (cenaDialogVM.Cena is not null)
            {
                editedRecords.Add(cenaDialogVM.Cena);
            }
            break;

        case Dodavatel:
            var dodavatelDialog = new DodavatelEditWindow();
            var dodavatelDialogVM = new DodavatelEditWindowViewModel();
            dodavatelDialogVM.Dodavatel = SelectedRecord as Dodavatel;
            dodavatelDialog.DataContext = dodavatelDialogVM;
            dodavatelDialog.ShowDialog();
            if (dodavatelDialogVM.Dodavatel is not null)
            {
                editedRecords.Add(dodavatelDialogVM.Dodavatel);
            }
            break;

        case InventarniPolozka:
            var inventarniPolozkaDialog = new InventarniPolozkaEditWindow();
            var inventarniPolozkaDialogVM = new InventarniPolozkaEditWindowViewModel();
            inventarniPolozkaDialogVM.InventarniPolozka = SelectedRecord as InventarniPolozka;
            inventarniPolozkaDialog.DataContext = inventarniPolozkaDialogVM;
            inventarniPolozkaDialog.ShowDialog();
            if (inventarniPolozkaDialogVM.InventarniPolozka is not null)
            {
                editedRecords.Add(inventarniPolozkaDialogVM.InventarniPolozka);
            }
            break;

        case Kategorie:
            var kategorieDialog = new KategorieEditWindow();
            var kategorieDialogVM = new KategorieEditWindowViewModel();
            kategorieDialogVM.Kategorie = SelectedRecord as Kategorie;
            kategorieDialog.DataContext = kategorieDialogVM;
            kategorieDialog.ShowDialog();
            if (kategorieDialogVM.Kategorie is not null)
            {
                editedRecords.Add(kategorieDialogVM.Kategorie);
            }
            break;

        case ObjednaneZbozi:
            var objednaneZboziDialog = new ObjednaneZboziEditWindow();
            var objednaneZboziDialogVM = new ObjednaneZboziEditWindowViewModel();
            objednaneZboziDialogVM.ObjednaneZbozi = SelectedRecord as ObjednaneZbozi;
            objednaneZboziDialog.DataContext = objednaneZboziDialogVM;
            objednaneZboziDialog.ShowDialog();
            if (objednaneZboziDialogVM.ObjednaneZbozi is not null)
            {
                editedRecords.Add(objednaneZboziDialogVM.ObjednaneZbozi);
            }
            break;

        case Objednavka:
            var objednavkaDialog = new ObjednavkaEditWindow();
            var objednavkaDialogVM = new ObjednavkaEditWindowViewModel();
            objednavkaDialogVM.Objednavka = SelectedRecord as Objednavka;
            objednavkaDialog.DataContext = objednavkaDialogVM;
            objednavkaDialog.ShowDialog();
            if (objednavkaDialogVM.Objednavka is not null)
            {
                editedRecords.Add(objednavkaDialogVM.Objednavka);
            }
            break;

        case ObrazekZamestnance:
            var obrazekZamestnanceDialog = new ObrazekZamestnanceEditWindow();
            var obrazekZamestnanceDialogVM = new ObrazekZamestnanceEditWindowViewModel();
            obrazekZamestnanceDialogVM.ObrazekZamestnance = SelectedRecord as ObrazekZamestnance;
            obrazekZamestnanceDialog.DataContext = obrazekZamestnanceDialogVM;
            obrazekZamestnanceDialog.ShowDialog();
            if (obrazekZamestnanceDialogVM.ObrazekZamestnance is not null)
            {
                editedRecords.Add(obrazekZamestnanceDialogVM.ObrazekZamestnance);
            }
            break;

        case Platba:
            var platbaDialog = new PlatbaEditWindow();
            var platbaDialogVM = new PlatbaEditWindowViewModel();
            platbaDialogVM.Platba = SelectedRecord as Platba;
            platbaDialog.DataContext = platbaDialogVM;
            platbaDialog.ShowDialog();
            if (platbaDialogVM.Platba is not null)
            {
                editedRecords.Add(platbaDialogVM.Platba);
            }
            break;

        case PlnyUvazek:
            var plnyUvazekDialog = new PlnyUvazekEditWindow();
            var plnyUvazekDialogVM = new PlnyUvazekEditWindowViewModel();
            plnyUvazekDialogVM.PlnyUvazek = SelectedRecord as PlnyUvazek;
            plnyUvazekDialog.DataContext = plnyUvazekDialogVM;
            plnyUvazekDialog.ShowDialog();
            if (plnyUvazekDialogVM.PlnyUvazek is not null)
            {
                editedRecords.Add(plnyUvazekDialogVM.PlnyUvazek);
            }
            break;

        case Pokladna:
            var pokladnaDialog = new PokladnaEditWindow();
            var pokladnaDialogVM = new PokladnaEditWindowViewModel();
            pokladnaDialogVM.Pokladna = SelectedRecord as Pokladna;
            pokladnaDialog.DataContext = pokladnaDialogVM;
            pokladnaDialog.ShowDialog();
            if (pokladnaDialogVM.Pokladna is not null)
            {
                editedRecords.Add(pokladnaDialogVM.Pokladna);
            }
            break;

        case ProdaneZbozi:
            var prodaneZboziDialog = new ProdaneZboziEditWindow();
            var prodaneZboziDialogVM = new ProdaneZboziEditWindowViewModel();
            prodaneZboziDialogVM.ProdaneZbozi = SelectedRecord as ProdaneZbozi;
            prodaneZboziDialog.DataContext = prodaneZboziDialogVM;
            prodaneZboziDialog.ShowDialog();
            if (prodaneZboziDialogVM.ProdaneZbozi is not null)
            {
                editedRecords.Add(prodaneZboziDialogVM.ProdaneZbozi);
            }
            break;

        case Role:
            var roleDialog = new RoleEditWindow();
            var roleDialogVM = new RoleEditWindowViewModel();
            roleDialogVM.Role = SelectedRecord as Role;
            roleDialog.DataContext = roleDialogVM;
            roleDialog.ShowDialog();
            if (roleDialogVM.Role is not null)
            {
                editedRecords.Add(roleDialogVM.Role);
            }
            break;

        case Supermarket:
            var supermarketDialog = new SupermarketEditWindow();
            var supermarketDialogVM = new SupermarketEditWindowViewModel();
            supermarketDialogVM.Supermarket = SelectedRecord as Supermarket;
            supermarketDialog.DataContext = supermarketDialogVM;
            supermarketDialog.ShowDialog();
            if (supermarketDialogVM.Supermarket is not null)
            {
                editedRecords.Add(supermarketDialogVM.Supermarket);
            }
            break;

        case Uctenka:
            var uctenkaDialog = new UctenkaEditWindow();
            var uctenkaDialogVM = new UctenkaEditWindowViewModel();
            uctenkaDialogVM.Uctenka = SelectedRecord as Uctenka;
            uctenkaDialog.DataContext = uctenkaDialogVM;
            uctenkaDialog.ShowDialog();
            if (uctenkaDialogVM.Uctenka is not null)
            {
                editedRecords.Add(uctenkaDialogVM.Uctenka);
            }
            break;

        case Uzivatel:
            var uzivatelDialog = new UzivatelEditWindow();
            var uzivatelDialogVM = new UzivatelEditWindowViewModel();
            uzivatelDialogVM.Uzivatel = SelectedRecord as Uzivatel;
            uzivatelDialog.DataContext = uzivatelDialogVM;
            uzivatelDialog.ShowDialog();
            if (uzivatelDialogVM.Uzivatel is not null)
            {
                editedRecords.Add(uzivatelDialogVM.Uzivatel);
            }
            break;

        case Vernostni_karta:
            var vernostniKartaDialog = new VernostniKartaEditWindow();
            var vernostniKartaDialogVM = new VernostniKartaEditWindowViewModel();
            vernostniKartaDialogVM.VernostniKarta = SelectedRecord as Vernostni_karta;
            vernostniKartaDialog.DataContext = vernostniKartaDialogVM;
            vernostniKartaDialog.ShowDialog();
            if (vernostniKartaDialogVM.VernostniKarta is not null)
            {
                editedRecords.Add(vernostniKartaDialogVM.VernostniKarta);
            }
            break;

        case Vydavatel:
            var vydavatelDialog = new VydavatelEditWindow();
            var vydavatelDialogVM = new VydavatelEditWindowViewModel();
            vydavatelDialogVM.Vydavatel = SelectedRecord as Vydavatel;
            vydavatelDialog.DataContext = vydavatelDialogVM;
            vydavatelDialog.ShowDialog();
            if (vydavatelDialogVM.Vydavatel is not null)
            {
                editedRecords.Add(vydavatelDialogVM.Vydavatel);
            }
            break;

        case Vyrobce:
            var vyrobceDialog = new VyrobceEditWindow();
            var vyrobceDialogVM = new VyrobceEditWindowViewModel();
            vyrobceDialogVM.Vyrobce = SelectedRecord as Vyrobce;
            vyrobceDialog.DataContext = vyrobceDialogVM;
            vyrobceDialog.ShowDialog();
            if (vyrobceDialogVM.Vyrobce is not null)
            {
                editedRecords.Add(vyrobceDialogVM.Vyrobce);
            }
            break;

        case Zamestnanec:
            var zamestnanecDialog = new ZamestnanecEditWindow();
            var zamestnanecDialogVM = new ZamestnanecEditWindowViewModel();
            zamestnanecDialogVM.Zamestnanec = SelectedRecord as Zamestnanec;
            zamestnanecDialog.DataContext = zamestnanecDialogVM;
            zamestnanecDialog.ShowDialog();
            if (zamestnanecDialogVM.Zamestnanec is not null)
            {
                editedRecords.Add(zamestnanecDialogVM.Zamestnanec);
            }
            break;

        case Zbozi:
            var zboziDialog = new ZboziEditWindow();
            var zboziDialogVM = new ZboziEditWindowViewModel();
            zboziDialogVM.Zbozi = SelectedRecord as Zbozi;
            zboziDialog.DataContext = zboziDialogVM;
            zboziDialog.ShowDialog();
            if (zboziDialogVM.Zbozi is not null)
            {
                editedRecords.Add(zboziDialogVM.Zbozi);
            }
            break;
            */
            default:
                throw new ApplicationException("No edit method for this table.");
        }


    }


    /* Funcke podle vybraného recordu zavolá příslušné modální okno. Po potvrzení okna
     * se nová třída načte do pole změn, které se později můžou aplikovat.
     */
    private void HandleAddUserRequset()
    {
        switch (SelectedTable?.TableName.ToUpper())
        {
            case "ADRESY":
                var adresaDialog = new AdresaEditWindow();
                var adresaDialogVM = new AdresaEditWindowViewModel();
                adresaDialog.DataContext = adresaDialogVM;
                adresaDialog.ShowDialog();
                if (adresaDialogVM.Adresa is not null)
                {
                    editedRecords.Add(adresaDialogVM.Adresa);
                    SelectedTableData?.Add(adresaDialogVM.Adresa);
                }
                break;

            case "BRIGADNICI":
                var brigadnikDialog = new BrigadnikEditWindow();
                var brigadnikDialogVM = new BrigadnikEditWindowViewModel();
                brigadnikDialog.DataContext = brigadnikDialogVM;
                brigadnikDialog.ShowDialog();
                if (brigadnikDialogVM.Brigadnik is not null)
                {
                    editedRecords.Add(brigadnikDialogVM.Brigadnik);
                    SelectedTableData?.Add(brigadnikDialogVM.Brigadnik);
                }
                break;

            /*
        case "CENY":
            var cenaDialog = new CenaEditWindow();
            var cenaDialogVM = new CenaEditWindowViewModel();
            cenaDialog.DataContext = cenaDialogVM;
            cenaDialog.ShowDialog();
            if (cenaDialogVM.Cena is not null)
            {
                editedRecords.Add(cenaDialogVM.Cena);
                    SelectedTableData?.Add(cenaDialogVM.Cena);
            }
            break;

        case "DODAVATELE":
            var dodavatelDialog = new DodavatelEditWindow();
            var dodavatelDialogVM = new DodavatelEditWindowViewModel();
            dodavatelDialog.DataContext = dodavatelDialogVM;
            dodavatelDialog.ShowDialog();
            if (dodavatelDialogVM.Dodavatel is not null)
            {
                editedRecords.Add(dodavatelDialogVM.Dodavatel);
                    SelectedTableData?.Add(dodavatelDialogVM.Dodavatel);
            }
            break;

        case "INVENTARNI_POLOZKY":
            var inventarniPolozkaDialog = new InventarniPolozkaEditWindow();
            var inventarniPolozkaDialogVM = new InventarniPolozkaEditWindowViewModel();
            inventarniPolozkaDialog.DataContext = inventarniPolozkaDialogVM;
            inventarniPolozkaDialog.ShowDialog();
            if (inventarniPolozkaDialogVM.InventarniPolozka is not null)
            {
                editedRecords.Add(inventarniPolozkaDialogVM.InventarniPolozka);
                    SelectedTableData?.Add(inventarniPolozkaDialogVM.InventarniPolozka);
            }
            break;

        case "KATEGORIE":
            var kategorieDialog = new KategorieEditWindow();
            var kategorieDialogVM = new KategorieEditWindowViewModel();
            kategorieDialog.DataContext = kategorieDialogVM;
            kategorieDialog.ShowDialog();
            if (kategorieDialogVM.Kategorie is not null)
            {
                editedRecords.Add(kategorieDialogVM.Kategorie);
                    SelectedTableData?.Add(kategorieDialogVM.Kategorie);
            }
            break;

        case "OBJEDNANE_ZBOZO":
            var objednaneZboziDialog = new ObjednaneZboziEditWindow();
            var objednaneZboziDialogVM = new ObjednaneZboziEditWindowViewModel();
            objednaneZboziDialog.DataContext = objednaneZboziDialogVM;
            objednaneZboziDialog.ShowDialog();
            if (objednaneZboziDialogVM.ObjednaneZbozi is not null)
            {
                editedRecords.Add(objednaneZboziDialogVM.ObjednaneZbozi);
                    SelectedTableData?.Add(objednaneZboziDialogVM.ObjednaneZbozi);
            }
            break;

        case "OBJEDNAVKY":
            var objednavkaDialog = new ObjednavkaEditWindow();
            var objednavkaDialogVM = new ObjednavkaEditWindowViewModel();
            objednavkaDialog.DataContext = objednavkaDialogVM;
            objednavkaDialog.ShowDialog();
            if (objednavkaDialogVM.Objednavka is not null)
            {
                editedRecords.Add(objednavkaDialogVM.Objednavka);
                    SelectedTableData?.Add(objednavkaDialogVM.Objednavka);
            }
            break;

        case "OBRAZKYZAMESTNANCU":
            var obrazekZamestnanceDialog = new ObrazekZamestnanceEditWindow();
            var obrazekZamestnanceDialogVM = new ObrazekZamestnanceEditWindowViewModel();
            obrazekZamestnanceDialog.DataContext = obrazekZamestnanceDialogVM;
            obrazekZamestnanceDialog.ShowDialog();
            if (obrazekZamestnanceDialogVM.ObrazekZamestnance is not null)
            {
                editedRecords.Add(obrazekZamestnanceDialogVM.ObrazekZamestnance);
                    SelectedTableData?.Add(obrazekZamestnanceDialogVM.ObrazekZamestnance);
            }
            break;

        case "PLATBY":
            var platbaDialog = new PlatbaEditWindow();
            var platbaDialogVM = new PlatbaEditWindowViewModel();
            platbaDialog.DataContext = platbaDialogVM;
            platbaDialog.ShowDialog();
            if (platbaDialogVM.Platba is not null)
            {
                editedRecords.Add(platbaDialogVM.Platba);
                    SelectedTableData?.Add(platbaDialogVM.Platba);
            }
            break;

        case "PLNE_UVAZKY":
            var plnyUvazekDialog = new PlnyUvazekEditWindow();
            var plnyUvazekDialogVM = new PlnyUvazekEditWindowViewModel();
            plnyUvazekDialog.DataContext = plnyUvazekDialogVM;
            plnyUvazekDialog.ShowDialog();
            if (plnyUvazekDialogVM.PlnyUvazek is not null)
            {
                editedRecords.Add(plnyUvazekDialogVM.PlnyUvazek);
                    SelectedTableData?.Add(plnyUvazekDialogVM.PlnyUvazek);
            }
            break;

        case "POKLADNY":
            var pokladnaDialog = new PokladnaEditWindow();
            var pokladnaDialogVM = new PokladnaEditWindowViewModel();
            pokladnaDialog.DataContext = pokladnaDialogVM;
            pokladnaDialog.ShowDialog();
            if (pokladnaDialogVM.Pokladna is not null)
            {
                editedRecords.Add(pokladnaDialogVM.Pokladna);
                    SelectedTableData?.Add(pokladnaDialogVM.Pokladna);
            }
            break;

        case "PRODANE_ZBOZI":
            var prodaneZboziDialog = new ProdaneZboziEditWindow();
            var prodaneZboziDialogVM = new ProdaneZboziEditWindowViewModel();
            prodaneZboziDialog.DataContext = prodaneZboziDialogVM;
            prodaneZboziDialog.ShowDialog();
            if (prodaneZboziDialogVM.ProdaneZbozi is not null)
            {
                editedRecords.Add(prodaneZboziDialogVM.ProdaneZbozi);
                    SelectedTableData?.Add(prodaneZboziDialogVM.ProdaneZbozi);
            }
            break;

        case "ROLE":
            var roleDialog = new RoleEditWindow();
            var roleDialogVM = new RoleEditWindowViewModel();
            roleDialog.DataContext = roleDialogVM;
            roleDialog.ShowDialog();
            if (roleDialogVM.Role is not null)
            {
                editedRecords.Add(roleDialogVM.Role);
                    SelectedTableData?.Add(roleDialogVM.Role);
            }
            break;

        case "SUPERMARKETY":
            var supermarketDialog = new SupermarketEditWindow();
            var supermarketDialogVM = new SupermarketEditWindowViewModel();
            supermarketDialog.DataContext = supermarketDialogVM;
            supermarketDialog.ShowDialog();
            if (supermarketDialogVM.Supermarket is not null)
            {
                editedRecords.Add(supermarketDialogVM.Supermarket);
                    SelectedTableData?.Add(supermarketDialogVM.Supermarket);
            }
            break;

        case "UCTENKY":
            var uctenkaDialog = new UctenkaEditWindow();
            var uctenkaDialogVM = new UctenkaEditWindowViewModel();
            uctenkaDialog.DataContext = uctenkaDialogVM;
            uctenkaDialog.ShowDialog();
            if (uctenkaDialogVM.Uctenka is not null)
            {
                editedRecords.Add(uctenkaDialogVM.Uctenka);
                    SelectedTableData?.Add(uctenkaDialogVM.Uctenka);
            }
            break;

        case "UZIVATELE":
            var uzivatelDialog = new UzivatelEditWindow();
            var uzivatelDialogVM = new UzivatelEditWindowViewModel();
            uzivatelDialog.DataContext = uzivatelDialogVM;
            uzivatelDialog.ShowDialog();
            if (uzivatelDialogVM.Uzivatel is not null)
            {
                editedRecords.Add(uzivatelDialogVM.Uzivatel);
                    SelectedTableData?.Add(uzivatelDialogVM.Uzivatel);
            }
            break;

        case "VERNOSTNI_KARTY":
            var vernostniKartaDialog = new VernostniKartaEditWindow();
            var vernostniKartaDialogVM = new VernostniKartaEditWindowViewModel();
            vernostniKartaDialog.DataContext = vernostniKartaDialogVM;
            vernostniKartaDialog.ShowDialog();
            if (vernostniKartaDialogVM.VernostniKarta is not null)
            {
                editedRecords.Add(vernostniKartaDialogVM.VernostniKarta);
                    SelectedTableData?.Add(vernostniKartaDialogVM.VernostniKarta);
            }
            break;

        case "VYDAVATELE":
            var vydavatelDialog = new VydavatelEditWindow();
            var vydavatelDialogVM = new VydavatelEditWindowViewModel();
            vydavatelDialog.DataContext = vydavatelDialogVM;
            vydavatelDialog.ShowDialog();
            if (vydavatelDialogVM.Vydavatel is not null)
            {
                editedRecords.Add(vydavatelDialogVM.Vydavatel);
                    SelectedTableData?.Add(vydavatelDialogVM.Vydavatel);
            }
            break;

        case "VYROBCI":
            var vyrobceDialog = new VyrobceEditWindow();
            var vyrobceDialogVM = new VyrobceEditWindowViewModel();
            vyrobceDialog.DataContext = vyrobceDialogVM;
            vyrobceDialog.ShowDialog();
            if (vyrobceDialogVM.Vyrobce is not null)
            {
                editedRecords.Add(vyrobceDialogVM.Vyrobce);
                    SelectedTableData?.Add(vyrobceDialogVM.Vyrobce);
            }
            break;

        case "ZAMESTNANCI":
            var zamestnanecDialog = new ZamestnanecEditWindow();
            var zamestnanecDialogVM = new ZamestnanecEditWindowViewModel();
            zamestnanecDialog.DataContext = zamestnanecDialogVM;
            zamestnanecDialog.ShowDialog();
            if (zamestnanecDialogVM.Zamestnanec is not null)
            {
                editedRecords.Add(zamestnanecDialogVM.Zamestnanec);
                    SelectedTableData?.Add(zamestnanecDialogVM.Zamestnanec);
            }
            break;

        case "ZBOZI":
            var zboziDialog = new ZboziEditWindow();
            var zboziDialogVM = new ZboziEditWindowViewModel();
            zboziDialog.DataContext = zboziDialogVM;
            zboziDialog.ShowDialog();
            if (zboziDialogVM.Zbozi is not null)
            {
                editedRecords.Add(zboziDialogVM.Zbozi);
                    SelectedTableData?.Add(zboziDialogVM.Zbozi);
            }
            break;
            */
            default:
                throw new ApplicationException("No add method for this table.");
        }
    }

    private void SendNewRecordsToDB()
    {
        foreach (var record in newRecords)
        {
            if (record is not null)
            {

                switch (record)
                {
                    case Adresa recordAdresa:
                        AdresaService.Create(recordAdresa);
                        break;
                    case Brigadnik recordBrigadnik:
                        BrigadnikService.Create(recordBrigadnik);
                        break;
                    case Cena recordCena:
                        CenaService.Create(recordCena);
                        break;
                    case Dodavatel recordDodavatel:
                        DodavatelService.Create(recordDodavatel);
                        break;
                    case InventarniPolozka recordInventarniPolozka:
                        InventarniPolozkaService.Create(new List<InventarniPolozka>() { recordInventarniPolozka });
                        break;
                    case Kategorie recordKategorie:
                        KategorieService.Create(recordKategorie);
                        break;
                    case ObjednaneZbozi recordObjednaneZbozi:
                        ObjednaneZboziService.Create(recordObjednaneZbozi);
                        break;
                    case Objednavka recordObjednavka:
                        ObjednavkaService.Create(recordObjednavka, new List<ObjednaneZbozi>());
                        break;
                    case ObrazekZamestnance recordObrazekZamestnance:
                        ObrazekZamestnanceService.Create(recordObrazekZamestnance);
                        break;
                    case Platba recordPlatba:
                        PlatbaService.Create(recordPlatba);
                        break;
                    case PlnyUvazek recordPlnyUvazek:
                        PlnyUvazekService.Create(recordPlnyUvazek);
                        break;
                    case Pokladna recordPokladna:
                        PokladnaService.Create(recordPokladna);
                        break;
                    case ProdaneZbozi recordProdaneZbozi:
                        ProdaneZboziService.Create(recordProdaneZbozi);
                        break;
                    case Role recordRole:
                        RoleService.Create(recordRole);
                        break;
                    case Supermarket recordSupermarket:
                        SupermarketService.Create(recordSupermarket);
                        break;
                    case Uctenka recordUctenka:
                        UctenkaService.Create(recordUctenka);
                        break;
                    case Uzivatel recordUzivatel:
                        UzivateleService.Registration(recordUzivatel.Username, recordUzivatel.Password);
                        break;
                    case Vernostni_karta recordVernostni_karta:
                        VernostniKartaService.Create(recordVernostni_karta);
                        break;
                    case Vydavatel recordVydavatel:
                        VydavatelService.Create(recordVydavatel);
                        break;
                    case Vyrobce recordVyrobce:
                        VyrobceService.Create(recordVyrobce);
                        break;
                    case Zamestnanec recordZamestnanec:
                        ZamestnanecService.Create(recordZamestnanec);
                        break;
                    case Zbozi recordZbozi:
                        ZboziService.Create(recordZbozi);
                        break;
                    default:
                        throw new NotImplementedException($"Create operation not implemented for type {record.GetType()}");
                }

            }
        }
        newRecords = new();
    }

    private void SendDeletedRecordsToDB()
    {
        foreach (var record in deletedRecords)
        {
            if (record is not null)
            {

                switch (record)
                {
                    case Adresa recordAdresa:
                        AdresaService.Delete(recordAdresa);
                        break;
                    case Brigadnik recordBrigadnik:
                        ZamestnanecService.Delete(recordBrigadnik);
                        break;
                    case Cena recordCena:
                        CenaService.Delete(recordCena);
                        break;
                    case Dodavatel recordDodavatel:
                        DodavatelService.Delete(recordDodavatel);
                        break;
                    case InventarniPolozka recordInventarniPolozka:
                        InventarniPolozkaService.Delete(recordInventarniPolozka);
                        break;
                    case Kategorie recordKategorie:
                        KategorieService.Delete(recordKategorie);
                        break;
                    case ObjednaneZbozi recordObjednaneZbozi:
                        ObjednaneZboziService.Delete(recordObjednaneZbozi);
                        break;
                    case Objednavka recordObjednavka:
                        ObjednavkaService.Delete(recordObjednavka);
                        break;
                    case ObrazekZamestnance recordObrazekZamestnance:
                        ObrazekZamestnanceService.Delete(recordObrazekZamestnance);
                        break;
                    case Platba recordPlatba:
                        PlatbaService.Delete(recordPlatba);
                        break;
                    case PlnyUvazek recordPlnyUvazek:
                        ZamestnanecService.Delete(recordPlnyUvazek);
                        break;
                    case Pokladna recordPokladna:
                        PokladnaService.Delete(recordPokladna);
                        break;
                    case ProdaneZbozi recordProdaneZbozi:
                        ProdaneZboziService.Delete(recordProdaneZbozi);
                        break;
                    case Role recordRole:
                        RoleService.Delete(recordRole);
                        break;
                    case Supermarket recordSupermarket:
                        SupermarketService.Delete(recordSupermarket);
                        break;
                    case Uctenka recordUctenka:
                        UctenkaService.Delete(recordUctenka);
                        break;
                    case Uzivatel recordUzivatel:
                        UzivateleService.Delete(recordUzivatel);
                        break;
                    case Vernostni_karta recordVernostni_karta:
                        VernostniKartaService.Delete(recordVernostni_karta);
                        break;
                    case Vydavatel recordVydavatel:
                        VydavatelService.Delete(recordVydavatel);
                        break;
                    case Vyrobce recordVyrobce:
                        VyrobceService.Delete(recordVyrobce);
                        break;
                    case Zamestnanec recordZamestnanec:
                        ZamestnanecService.Delete(recordZamestnanec);
                        break;
                    case Zbozi recordZbozi:
                        ZboziService.Delete(recordZbozi);
                        break;
                    default:
                        throw new NotImplementedException($"Delete operation not implemented for type {record.GetType()}");
                }

            }
        }
        deletedRecords = new();
    }

    private void SendEditedRecordsToDB()
    {
        foreach (var record in editedRecords)
        {
            if (record != null)
            {
                switch (record)
                {
                    case Adresa recordAdresa:
                        AdresaService.Update(recordAdresa);
                        break;
                    case Brigadnik recordBrigadnik:
                        BrigadnikService.Update(recordBrigadnik);
                        break;
                    case Cena recordCena:
                        CenaService.Update(recordCena);
                        break;
                    case Dodavatel recordDodavatel:
                        DodavatelService.Update(recordDodavatel);
                        break;
                    case InventarniPolozka recordInventarniPolozka:
                        InventarniPolozkaService.Update(new List<InventarniPolozka>() { recordInventarniPolozka });
                        break;
                    case Kategorie recordKategorie:
                        KategorieService.Update(recordKategorie);
                        break;
                    case ObjednaneZbozi recordObjednaneZbozi:
                        ObjednaneZboziService.Update(recordObjednaneZbozi);
                        break;
                    case Objednavka recordObjednavka:
                        ObjednavkaService.Update(recordObjednavka);
                        break;
                    case ObrazekZamestnance recordObrazekZamestnance:
                        ObrazekZamestnanceService.Update(recordObrazekZamestnance);
                        break;
                    case Platba recordPlatba:
                        PlatbaService.Update(recordPlatba);
                        break;
                    case PlnyUvazek recordPlnyUvazek:
                        PlnyUvazekService.Update(recordPlnyUvazek);
                        break;
                    case Pokladna recordPokladna:
                        PokladnaService.Update(recordPokladna);
                        break;
                    case ProdaneZbozi recordProdaneZbozi:
                        ProdaneZboziService.Update(recordProdaneZbozi);
                        break;
                    case Role recordRole:
                        RoleService.Update(recordRole);
                        break;
                    case Supermarket recordSupermarket:
                        SupermarketService.Update(recordSupermarket);
                        break;
                    case Uctenka recordUctenka:
                        UctenkaService.Update(recordUctenka);
                        break;
                    case Uzivatel recordUzivatel:
                        UzivateleService.Update(recordUzivatel);
                        break;
                    case Vernostni_karta recordVernostni_karta:
                        VernostniKartaService.Update(recordVernostni_karta);
                        break;
                    case Vydavatel recordVydavatel:
                        VydavatelService.Update(recordVydavatel);
                        break;
                    case Vyrobce recordVyrobce:
                        VyrobceService.Update(recordVyrobce);
                        break;
                    case Zamestnanec recordZamestnanec:
                        ZamestnanecService.Update(recordZamestnanec);
                        break;
                    case Zbozi recordZbozi:
                        ZboziService.Update(recordZbozi);
                        break;
                    default:
                        throw new NotImplementedException($"Update operation not implemented for type {record.GetType()}");
                }
            }
        }
        editedRecords = new();
    }

    internal void Refresh()
    {
        var dbTables = SystemCatalogService.GetAllTables();
        dbTables.Sort((x, y) => x.TableName.CompareTo(y.TableName));
        Tables = new(dbTables);
    }
}
