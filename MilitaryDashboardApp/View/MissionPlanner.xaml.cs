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
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Threading;
using MilitaryDashboardApp.ViewModels;

namespace MilitaryDashboardApp.View
{
    /// <summary>
    /// Interaction logic for MissionPlanner.xaml
    /// </summary>
    public partial class MissionPlanner : UserControl
    {
        public MissionPlanner()
        {
            InitializeComponent();

            var vm = new MissionPlannerViewModel(new Services.MessageLogService());
            vm.MissionAssigned += OnMissionAssigned;
            this.DataContext = vm;
        }

        private void OnMissionAssigned(object? sender, EventArgs e)
        {
            Toast.Visibility = Visibility.Visible;
            Toast.Opacity = 1;

            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2)
            };
            timer.Tick += (s, ev) =>
            {
                timer.Stop();
                Toast.Visibility = Visibility.Collapsed;
            };
            timer.Start();
        }
    }
}
