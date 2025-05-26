using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Printer_status_monitor_app.Modells
{
    public partial class Printer : ObservableObject
    {
        [ObservableProperty]
        private DigitalProperty digitalProperty = new();
    }
}
