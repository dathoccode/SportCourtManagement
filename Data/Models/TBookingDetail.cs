using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SportCourtManagement.Data.Models;

[Table("tBookingDetail")]
public partial class TBookingDetail
{
    [Key]
    [Column("DetailID")]
    [StringLength(10)]
    public string DetailId { get; set; } = null!;

    [Column("BookingID")]
    [StringLength(10)]
    public string? BookingId { get; set; }

    [Column("CourtID")]
    [StringLength(10)]
    public string? CourtId { get; set; }

    [Column("SlotID")]
    [StringLength(10)]
    public string? SlotId { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }

    [ForeignKey("BookingId")]
    [InverseProperty("TBookingDetails")]
    public virtual TBooking? Booking { get; set; }

    [ForeignKey("CourtId")]
    [InverseProperty("TBookingDetails")]
    public virtual TCourt? Court { get; set; }
}
