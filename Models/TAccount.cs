using System;
using System.Collections.Generic;

namespace SportCourtManagement.Models;

public partial class TAccount
{
    public string AccountId { get; set; } = null!;

    public int? RoleId { get; set; }

    public string? AccName { get; set; }

    public string? AccPassword { get; set; }

    public byte[]? AccImg { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public virtual TRole? Role { get; set; }

    public virtual ICollection<TBooking> TBookings { get; set; } = new List<TBooking>();

    public virtual ICollection<TCourt> Courts { get; set; } = new List<TCourt>();
}
