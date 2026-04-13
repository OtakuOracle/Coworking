using System;
using System.Collections.Generic;

namespace Coworking.Models;

public partial class User
{
    public int UserId { get; set; }

    public int RoleId { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Password { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Role? Role { get; set; }
}
