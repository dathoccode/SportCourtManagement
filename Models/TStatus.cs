using System;
using System.Collections.Generic;

namespace SportCourtManagement.Models;

public partial class TStatus
{
    public string StatusId { get; set; } = null!;

    public string StatusName { get; set; } = null!;

    public virtual ICollection<TBooking> TBookings { get; set; } = new List<TBooking>();
}
