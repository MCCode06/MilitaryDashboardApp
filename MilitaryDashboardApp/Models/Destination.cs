using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryDashboardApp.Models
{
    public class Destination
    {
        public Unit Unit { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Destination(Unit unit, double latitude, double longitude)
        {
            Unit = unit;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
