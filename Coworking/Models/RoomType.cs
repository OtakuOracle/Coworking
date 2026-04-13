using System;
using System.Collections.Generic;

namespace Coworking.Models;

public partial class RoomType
{
    public int RoomTypeId { get; set; }

    public string? RoomTypeName { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
