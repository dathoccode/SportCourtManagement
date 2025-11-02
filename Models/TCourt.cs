using System;
using System.Collections.Generic;

namespace SportCourtManagement.Models;

public partial class TCourt
{
    public string CourtId { get; set; } = null!;

    public string? CourtName { get; set; }

    public string? CourtAddress { get; set; }

    public string? Contact { get; set; }

    public TimeOnly? OpenTime { get; set; }

    public TimeOnly? CloseTime { get; set; }

    public byte[]? Img { get; set; }

    public double? Rating { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public string? Kind { get; set; }

    public virtual ICollection<TBookingDetail> TBookingDetails { get; set; } = new List<TBookingDetail>();

    public virtual ICollection<TPrice> TPrices { get; set; } = new List<TPrice>();

    public virtual ICollection<TSlot> TSlots { get; set; } = new List<TSlot>();

    public virtual ICollection<TAccount> Accounts { get; set; } = new List<TAccount>();
}
