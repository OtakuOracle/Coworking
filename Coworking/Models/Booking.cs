using System;
using System.Collections.Generic;

namespace Coworking.Models;

public partial class Booking
{
    public int BookingId { get; set; }

    public int? UserId { get; set; }

    public int? RoomId { get; set; }

    public DateOnly? Date { get; set; }

    public TimeOnly? Time { get; set; }

    public int? BookingStatusId { get; set; }

    public int? ServiceId { get; set; }

    public DateOnly? DateCreate { get; set; }

    public virtual BookingStatus? BookingStatus { get; set; }

    public virtual Room? Room { get; set; }

    public virtual Service? Service { get; set; }

    public virtual User? User { get; set; }
}
