using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SportCourtManagement.Data.Models;

[Table("tCourt")]
public partial class TCourt
{
    [Key]
    [Column("CourtID")]
    [StringLength(10)]
    public string CourtId { get; set; } = null!;

    [StringLength(10)]
    public string? CourtName { get; set; }

    [StringLength(50)]
    public string? CourtAddress { get; set; }

    [StringLength(11)]
    public string? Contact { get; set; }

    public TimeOnly? OpenTime { get; set; }

    public TimeOnly? CloseTime { get; set; }

    [StringLength(50)]
    public string? Img { get; set; }

    public double? Rating { get; set; }

    [InverseProperty("Court")]
    public virtual ICollection<TBookingDetail> TBookingDetails { get; set; } = new List<TBookingDetail>();

    [InverseProperty("Court")]
    public virtual ICollection<TPrice> TPrices { get; set; } = new List<TPrice>();

    [InverseProperty("Court")]
    public virtual ICollection<TSlot> TSlots { get; set; } = new List<TSlot>();

    [ForeignKey("CourtId")]
    [InverseProperty("Courts")]
    public virtual ICollection<TAccount> Accounts { get; set; } = new List<TAccount>();
}
