using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Printer_status_monitor_app.Modells
{
    public partial class DigitalProperty : ObservableObject
    {
        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string location;

        [ObservableProperty]
        private string ipAddress;

        [ObservableProperty]
        private string modelType;
    }
}
