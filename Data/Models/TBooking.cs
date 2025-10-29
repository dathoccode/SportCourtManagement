using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SportCourtManagement.Data.Models;

[Table("tBooking")]
public partial class TBooking
{
    [Key]
    [Column("BookingID")]
    [StringLength(10)]
    public string BookingId { get; set; } = null!;

    [Column("AccountID")]
    [StringLength(10)]
    public string? AccountId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? BookingDate { get; set; }

    public double? Sale { get; set; }

    [Column("StatusID")]
    public int? StatusId { get; set; }

    [Column(TypeName = "money")]
    public decimal? Price { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("TBookings")]
    public virtual TAccount? Account { get; set; }

    [ForeignKey("StatusId")]
    [InverseProperty("TBookings")]
    public virtual TStatus? Status { get; set; }

    [InverseProperty("Booking")]
    public virtual ICollection<TBookingDetail> TBookingDetails { get; set; } = new List<TBookingDetail>();
}
