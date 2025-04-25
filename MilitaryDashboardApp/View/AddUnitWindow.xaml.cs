using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for AddUnitWindow.xaml
    /// </summary>
    public partial class AddUnitWindow : Window
    {
        public string UnitName { get; private set; }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        public AddUnitWindow()
        {
            InitializeComponent();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text) ||
                !double.TryParse(LatitudeTextBox.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out double lat) ||
                !double.TryParse(LongitudeTextBox.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out double lng))
            {
                MessageBox.Show("Please enter valid values for all fields.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            UnitName = NameTextBox.Text.Trim();
            Latitude = lat;
            Longitude = lng;

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
