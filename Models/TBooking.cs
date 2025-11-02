using System;
using System.Collections.Generic;

namespace SportCourtManagement.Models;

public partial class TBooking
{
    public string BookingId { get; set; } = null!;

    public string AccountId { get; set; } = null!;

    public DateTime BookingDate { get; set; }

    public double? Sale { get; set; }

    public string StatusId { get; set; } = null!;

    public decimal Price { get; set; }

    public virtual TAccount Account { get; set; } = null!;

    public virtual TStatus Status { get; set; } = null!;

    public virtual ICollection<TBookingDetail> TBookingDetails { get; set; } = new List<TBookingDetail>();
}
