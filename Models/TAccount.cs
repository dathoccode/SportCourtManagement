using System;
using System.Collections.Generic;

namespace SportCourtManagement.Models;

public partial class TAccount
{
    public string AccountId { get; set; } = null!;

    public string RoleId { get; set; } = null!;

    public string AccName { get; set; } = null!;

    public string AccPassword { get; set; } = null!;

    public byte[]? AccImg { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? Gender { get; set; }

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public virtual TRole Role { get; set; } = null!;

    public virtual ICollection<TBooking> TBookings { get; set; } = new List<TBooking>();

    public virtual ICollection<TCourt> Courts { get; set; } = new List<TCourt>();
}
