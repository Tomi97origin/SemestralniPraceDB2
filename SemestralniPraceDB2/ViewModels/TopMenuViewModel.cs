using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Win32;
using Newtonsoft.Json;
using SemestralniPraceDB2.Models;
using SemestralniPraceDB2.Views.DialogWindows;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Xps;
using System.Xml;

namespace SemestralniPraceDB2.ViewModels;

[ObservableRecipient]
partial class TopMenuViewModel : BaseViewModel, IRecipient<UserLogin>, IRecipient<UserLogout>
{
    private readonly IMessenger messenger = WeakReferenceMessenger.Default;

    private bool isSomebodyLogged = false;

    [ObservableProperty]
    public string loginLogoutButtonText = "Přihlásit";

    public TopMenuViewModel()
    {
        Messenger = WeakReferenceMessenger.Default;
        Messenger.Register<UserLogin>(this);
        Messenger.Register<UserLogout>(this);
    }

    [RelayCommand]
    public void UpdateView(string parameter)
    {
        messenger.Send(new ViewChanged(parameter));
    }

    [RelayCommand]
    private void MenuItem(string parameter)
    {
        MessageBox.Show("Klikli jste na položku menu.");
    }

    [RelayCommand]
    private void ShowContextMenu(string parameter)
    {

        MessageBox.Show("Klikli jste na položku menu.");
    }

    [RelayCommand]
    private void LoginLogout() //pozn.: toto by mohla být dvě tlačítka, která by se zobrazovala/skrývala na základě toho zda je někdo přihlášený
    {
        if (!isSomebodyLogged)
        {
            //Změň View na přihlašovací okno
            messenger.Send(new ViewChanged("UserLogin"));
        }
        else if (isSomebodyLogged)
        {
            //Proveď odhlášení
            UzivateleService.LogOut();
            
            //Pošli zprávu o odhlášení:
            messenger.Send(new UserLogout());
        }


    }

    [RelayCommand]
    private void StopEmulation()
    {
        UzivateleService.StopEmulating();
    }

    [RelayCommand]
    private void OdlozeniSplatnosti()
    {
        new ChooseContractorWindow().ShowDialog();
    }

    [RelayCommand]
    private void ExportLogs()
    {
        var logs = LogyService.GetAll();
        if (logs.Count > 0)
        {
            // Vytvoření instance SaveFileDialog
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            // Nastavení výchozího názvu souboru (nepovinné)
            saveFileDialog.FileName = "DB_logs.txt";

            // Nastavení filtru souborů (nepovinné)
            saveFileDialog.Filter = "JSON strukturovaný soubor (*.json)|*.json";

            // Zobrazení dialogového okna a získání výsledku
            bool? result = saveFileDialog.ShowDialog();

            // Zpracování výsledku dialogu
            if (result == true)
            {
                // Získání vybrané cesty k uložení souboru
                string filePath = saveFileDialog.FileName;

                // Serializace seznamu do JSON
                string jsonText = JsonConvert.SerializeObject(logs, Newtonsoft.Json.Formatting.Indented);

                // Uložení JSON do souboru
                File.WriteAllText(filePath, jsonText);
            }
            else
            {
                return;
            }
        }
        else
        {
            MessageBox.Show("Nepodařilo se načíst žádné logy.");
        }
    }

    [RelayCommand]
    private void SetTheLeastSaledGoodsCheaper()
    {
        var Result = MessageBox.Show(
            "Opravdu chcete zlevnit málo prodávané zboží?",
            "Opravdu chcete zlevnit?",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (Result == MessageBoxResult.Yes)
        {
            ZboziService.ZlevniNejmeneProdavane();
            MessageBox.Show("Zboží zlevněno.", "Provedeno", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }
        else if (Result == MessageBoxResult.No)
        {
            return;
        }
    }

    [RelayCommand]
    private void DeleteDeactivated()
    {
        var Result = MessageBox.Show(
            "Opravdu chcete smazat uživatele, kteří již delší dobu nezískali aktivaci účtu?",
            "Opravdu chcete smazat?",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (Result == MessageBoxResult.Yes)
        {
            UzivateleService.DeleteAllOldDeactivated();
            MessageBox.Show("Smazáni všichni dlouho neaktivovaní užiavtelé.", "Provedeno", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }
        else if (Result == MessageBoxResult.No)
        {
            return;
        }


    }



    public void Receive(UserLogin message)
    {
        if (!isSomebodyLogged)
        {
            LoginLogoutButtonText = "Odhlásit";
            isSomebodyLogged = true;
        }
        else throw new ApplicationException("Unexpected Error: There cannot be logged 2 users at the time.");
    }

    public void Receive(UserLogout message)
    {
        if (isSomebodyLogged)
        {
            isSomebodyLogged = false;
            LoginLogoutButtonText = "Přihlásit";
        }
        else throw new ApplicationException("Unexpected Error: Can not logout when no user is logged in.");
    }

}
