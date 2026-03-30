using System.Windows;
using EpsteinsMarket.ApplicationData;
using EpsteinsMarket.Pages;

namespace EpsteinsMarket
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AppFrame.frmMain = frmMain;
            AppFrame.frmMain.Navigate(new Auth());
        }
    }
}
