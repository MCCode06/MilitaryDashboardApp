using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MilitaryDashboardApp.Models;
using MilitaryDashboardApp.ViewModels;


namespace MilitaryDashboardApp.View
{
    /// <summary>
    /// Interaction logic for UnitPanel.xaml
    /// </summary>
    /// 

    public partial class UnitPanel : UserControl
    {
        public UnitPanel()
        {
            InitializeComponent();
            DataContext = new UnitPanelViewModel();
        }

        private void SetDestination_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as UnitPanelViewModel;

            if (viewModel?.SelectedUnit == null)
            {
                MessageBox.Show("Please select a unit first.");
                return;
            }

            var destinationWindow = new SetDestinationWindow(viewModel.SelectedUnit);
            bool? result = destinationWindow.ShowDialog();

            if (result == true)
            {
                var mapView = FindMapView();
                if (mapView != null)
                {
                    var newDestination = new Destination(viewModel.SelectedUnit,
                                                         viewModel.SelectedUnit.DestinationLat,
                                                         viewModel.SelectedUnit.DestinationLng);

                    mapView.AddDestination(newDestination);
                }
            }
        }

        private void March_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as UnitPanelViewModel;

            if (viewModel?.SelectedUnit == null)
            {
                MessageBox.Show("Select a unit first.");
                return;
            }

            var unit = viewModel.SelectedUnit;

            if (double.IsNaN(unit.DestinationLat) || double.IsNaN(unit.DestinationLng))
            {
                MessageBox.Show("Set a destination first.");
                return;
            }

            unit.IsMarching = true;
            unit.Status = "Marching";

            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.MapViewRef?.RefreshMarkers();
            mainWindow?.MapViewRef?.StartMarching();
        }




        private MapView? FindMapView()
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            return mainWindow?.MapViewRef;
        }



    }

}
