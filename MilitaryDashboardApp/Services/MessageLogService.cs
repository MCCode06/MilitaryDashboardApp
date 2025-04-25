using MilitaryDashboardApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MilitaryDashboardApp.Services
{
    public class MessageLogService
    {
        public ObservableCollection<LogEntry> Messages { get; } = new();

        public void AddMessage(string message)
        {
            Messages.Add(new LogEntry { Message = $"[{DateTime.Now:HH:mm}] {message}" });
        }

    }
}