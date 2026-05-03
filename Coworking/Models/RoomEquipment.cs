using System;
using System.Collections.Generic;
using System.Text;

namespace Coworking.Models
{
    public partial class RoomEquipment
    {
        public int RoomId { get; set; }
        public int EquipmentId { get; set; }   
        public virtual Room Room { get; set; }
        public virtual Equipment Equipment { get; set; }
    }

}
