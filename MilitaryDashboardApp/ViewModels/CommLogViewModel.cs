using MilitaryDashboardApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MilitaryDashboardApp.ViewModels
{
    public class LogEntry
    {
        public string Message { get; set; } = string.Empty;
    }

    public class CommLogViewModel : BaseViewModel
    {
        public ObservableCollection<LogEntry> LogEntries { get; }

        public CommLogViewModel(MessageLogService logService)
        {
            LogEntries = logService.Messages;
        }
    }

}
