using System;
using System.Collections.Generic;

namespace Coworking.Models;

public partial class BookingStatus
{
    public int BookingStatusId { get; set; }

    public string? BookingStatusName { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
