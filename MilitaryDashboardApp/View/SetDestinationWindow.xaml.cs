using MilitaryDashboardApp.Models;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace MilitaryDashboardApp.View
{
    /// <summary>
    /// Interaction logic for SetDestinationWindow.xaml
    /// </summary>
    public partial class SetDestinationWindow : Window
    {
        public Unit SelectedUnit { get; set; }

        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        public SetDestinationWindow(Unit unit)
        {
            InitializeComponent();
            SelectedUnit = unit;

            // Set the current values of the selected unit's position as default
            LatitudeTextBox.Text = SelectedUnit.Latitude.ToString();
            LongitudeTextBox.Text = SelectedUnit.Longitude.ToString();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(LatitudeTextBox.Text, out double latitude) &&
                double.TryParse(LongitudeTextBox.Text, out double longitude))
            {
                SelectedUnit.DestinationLat = latitude;
                SelectedUnit.DestinationLng = longitude;

                DialogResult = true; 
                Close();
            }
            else
            {
                MessageBox.Show("Please enter valid latitude and longitude values.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false; 
            Close();
        }
    }
}
