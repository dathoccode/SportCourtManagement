using System;
using System.Collections.Generic;

namespace SportCourtManagement.Models;

public partial class TBookingDetail
{
    public string DetailId { get; set; } = null!;

    public string? BookingId { get; set; }

    public string? CourtId { get; set; }

    public string? SlotId { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }

    public virtual TBooking? Booking { get; set; }

    public virtual TCourt? Court { get; set; }
}
