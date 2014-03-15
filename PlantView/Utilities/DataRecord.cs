using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities
{
    public class DataRecord
    {
        public DateTime Date { get; set;}
        public int PlantId { get; set; }
        public int ProductId { get; set; }
        public decimal Weight { get; set; }
        public decimal TonsPerHour { get; set; }

    }
}
