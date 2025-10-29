using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SportCourtManagement.Data.Models;

[PrimaryKey("SlotId", "CourtId")]
[Table("tSlot")]
public partial class TSlot
{
    [Key]
    [Column("SlotID")]
    [StringLength(10)]
    public string SlotId { get; set; } = null!;

    [Key]
    [Column("CourtID")]
    [StringLength(10)]
    public string CourtId { get; set; } = null!;

    [StringLength(100)]
    public string? SlotType { get; set; }

    [ForeignKey("CourtId")]
    [InverseProperty("TSlots")]
    public virtual TCourt Court { get; set; } = null!;
}
