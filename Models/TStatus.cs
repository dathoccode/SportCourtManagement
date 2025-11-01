using System;
using System.Collections.Generic;

namespace SportCourtManagement.Models;

public partial class TStatus
{
    public int StatusId { get; set; }

    public string? StatusName { get; set; }

    public virtual ICollection<TBooking> TBookings { get; set; } = new List<TBooking>();
}
