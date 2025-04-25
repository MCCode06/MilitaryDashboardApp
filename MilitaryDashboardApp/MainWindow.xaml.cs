using MilitaryDashboardApp.Services;
using MilitaryDashboardApp.ViewModels;
using MilitaryDashboardApp.View;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MilitaryDashboardApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly MessageLogService _messageLogService = new();
        public MapView MapViewRef => MapView;

        public MainWindow()
        {
            InitializeComponent();

            CommLogControl.DataContext = new CommLogViewModel(_messageLogService);
            MissionPlannerControl.DataContext = new MissionPlannerViewModel(_messageLogService);


        }
    }
}