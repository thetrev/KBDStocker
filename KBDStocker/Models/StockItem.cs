using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KBDStocker.Models
{
    public class StockItem
    {
        public Guid Id { get; set; }

        public string Url { get; set; }

        public string Name { get; set; }

        public StockCategory Category { get; set; }
    }

    public enum StockCategory
    {
        Plate,
        Keycaps,
        Pcb,
        Switch,
        Keyboard
    }
}
