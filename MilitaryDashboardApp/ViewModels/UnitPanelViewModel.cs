using MilitaryDashboardApp.Models;
using MilitaryDashboardApp.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MilitaryDashboardApp.ViewModels
{
    public class UnitPanelViewModel : BaseViewModel
    {
        public ObservableCollection<Unit> Units { get; set; } = new();

        private Unit _selectedUnit;

        public Unit SelectedUnit
        {
            get { return _selectedUnit; }
            set
            {
                if (_selectedUnit != value)
                {
                    _selectedUnit = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand SetDestinationCommand { get; }
        public ICommand StartMarchingCommand { get; }
        public ICommand AddNewUnitCommand { get; }  

        public UnitPanelViewModel()
        {
            SetDestinationCommand = new RelayCommand(SetDestination, () => SelectedUnit != null);
            StartMarchingCommand = new RelayCommand(StartMarching, () => SelectedUnit != null);
            AddNewUnitCommand = new RelayCommand(AddNewUnit);

            Units = new ObservableCollection<Unit>
            {
                new Unit { Name = "Alpha", Latitude = 40.4093, Longitude = 49.8671 }, 
                new Unit { Name = "Bravo", Latitude = 40.7561, Longitude = 47.0590 }
            };

        }

        private void SetDestination()
        {
            SelectedUnit.DestinationLat = SelectedUnit.Latitude + 0.01;
            SelectedUnit.DestinationLng = SelectedUnit.Longitude + 0.01;
            OnPropertyChanged(nameof(SelectedUnit));
        }

        private void StartMarching()
        {
            SelectedUnit.IsMarching = true;
            OnPropertyChanged(nameof(SelectedUnit));
        }

        private void AddNewUnit()
        {
            var addUnitWindow = new AddUnitWindow();
            if (addUnitWindow.ShowDialog() == true)
            {
                var newUnit = new Unit
                {
                    Name = addUnitWindow.UnitName,
                    Latitude = addUnitWindow.Latitude,
                    Longitude = addUnitWindow.Longitude,
                    IsMarching = false
                };

                UnitRegistry.Instance.Units.Add(newUnit); 
                Units.Add(newUnit);

                if (Application.Current.MainWindow is MainWindow mainWindow)
                {
                    var mapView = mainWindow.FindName("MapView") as MapView;
                    mapView?.RefreshMarkers();
                }
            }
        }


    }


}

