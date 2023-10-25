using SemestralniPraceDB2.ViewModels;
using System.Windows;

namespace SemestralniPraceDB2;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        DataContext = new MainWindowViewModel();
        InitializeComponent();
    }
}
