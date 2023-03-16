using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockTripConsoleApp.Model
{
    public class MaxtripsPerHour
    {
        public DateTime Date { get; set; }
        public int PickupDay { get; set; }
        public int Hour { get; set; }
        public int Counts { get; set; }

        public int EquipmentVehicleTypeId { get; set; }

    }
}
