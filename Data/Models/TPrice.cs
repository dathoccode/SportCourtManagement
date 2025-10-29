using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SportCourtManagement.Data.Models;

[PrimaryKey("CourtId", "SlotId")]
[Table("tPrice")]
public partial class TPrice
{
    [Key]
    [Column("CourtID")]
    [StringLength(10)]
    public string CourtId { get; set; } = null!;

    [Key]
    [Column("SlotID")]
    [StringLength(10)]
    public string SlotId { get; set; } = null!;

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }

    [Column(TypeName = "money")]
    public decimal? UnitPrice { get; set; }

    [ForeignKey("CourtId")]
    [InverseProperty("TPrices")]
    public virtual TCourt Court { get; set; } = null!;
}
