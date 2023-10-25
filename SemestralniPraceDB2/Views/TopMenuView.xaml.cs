using SemestralniPraceDB2.ViewModels;
using System.Windows.Controls;

namespace SemestralniPraceDB2.Views;

/// <summary>
/// Interakční logika pro TopMenuView.xaml
/// </summary>
public partial class TopMenuView : UserControl
{
    public TopMenuView()
    {
        DataContext = new TopMenuViewModel();
        InitializeComponent();
    }
}

