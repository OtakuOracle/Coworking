using System;
using System.Collections.Generic;

namespace Coworking.Models;

public partial class Equipment
{
    public int EquipmentId { get; set; }

    public string? EquipmentName { get; set; }

    public virtual ICollection<RoomEquipment> RoomEquipments { get; set; } = new List<RoomEquipment>(); // Инициализация коллекции

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();

}
