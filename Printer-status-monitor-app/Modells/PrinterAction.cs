using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Printer_status_monitor_app.Modells
{
    class PrinterAction
    {
        public ActionType Type { get; set; }
        public Printer PrinterSnapshot { get; set; }
        public int Index { get; set; }
    }
}
