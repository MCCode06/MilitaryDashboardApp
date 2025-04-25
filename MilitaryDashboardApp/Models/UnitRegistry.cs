using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryDashboardApp.Models
{
    public class UnitRegistry
    {
        private static UnitRegistry _instance;
        public static UnitRegistry Instance => _instance ??= new UnitRegistry();

        public ObservableCollection<Unit> Units { get; } = new ObservableCollection<Unit>();
    }

}
