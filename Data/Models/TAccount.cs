using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SportCourtManagement.Data.Models;

[Table("tAccount")]
public partial class TAccount
{
    [Key]
    [Column("AccountID")]
    [StringLength(10)]
    public string AccountId { get; set; } = null!;

    [Column("RoleID")]
    public int? RoleId { get; set; }

    [StringLength(30)]
    public string? AccName { get; set; }

    [StringLength(20)]
    public string? AccPassword { get; set; }

    [StringLength(50)]
    public string? AccImg { get; set; }

    [ForeignKey("RoleId")]
    [InverseProperty("TAccounts")]
    public virtual TRole? Role { get; set; }

    [InverseProperty("Account")]
    public virtual ICollection<TBooking> TBookings { get; set; } = new List<TBooking>();

    [ForeignKey("AccountId")]
    [InverseProperty("Accounts")]
    public virtual ICollection<TCourt> Courts { get; set; } = new List<TCourt>();
}
