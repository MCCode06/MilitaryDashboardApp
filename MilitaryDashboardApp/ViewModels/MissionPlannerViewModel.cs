using MilitaryDashboardApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;


namespace MilitaryDashboardApp.ViewModels
{
    public class MissionPlannerViewModel : BaseViewModel
    {
        private readonly MessageLogService _logService;

        private string _missionName = string.Empty;

        public string MissionName
        {
            get => _missionName;
            set
            {
                if (_missionName != value)
                {
                    _missionName = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand AssignMissionCommand { get; }

        public MissionPlannerViewModel(MessageLogService logService)
        {
            _logService = logService;
            AssignMissionCommand = new RelayCommand(AssignMission);
        }

        public event EventHandler? MissionAssigned;

        private void AssignMission()
        {
            _logService.AddMessage($"Mission '{MissionName}' assigned successfully.");
            MissionName = string.Empty;
            MissionAssigned?.Invoke(this, EventArgs.Empty);
        }

    }

}