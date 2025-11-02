using System;
using System.Collections.Generic;

namespace SportCourtManagement.Models;

public partial class TBooking
{
    public string BookingId { get; set; } = null!;

    public string? AccountId { get; set; }

    public DateTime? BookingDate { get; set; }

    public double? Sale { get; set; }

    public int? StatusId { get; set; }

    public decimal? Price { get; set; }

    public virtual TAccount? Account { get; set; }

    public virtual TStatus? Status { get; set; }

    public virtual ICollection<TBookingDetail> TBookingDetails { get; set; } = new List<TBookingDetail>();
}
