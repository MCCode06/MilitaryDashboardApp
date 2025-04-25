using GMap.NET;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryDashboardApp.Models
{
    public class Unit
    {
        public string Name { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public double DestinationLat { get; set; }
        public double DestinationLng { get; set; }
        public ObservableCollection<PointLatLng> Trail { get; set; } = new ObservableCollection<PointLatLng>();

        public bool IsMarching { get; set; } = false;

        public string Status { get; set; } = "Idle";

     
    }

}
