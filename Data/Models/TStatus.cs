using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SportCourtManagement.Data.Models;

[Table("tStatus")]
public partial class TStatus
{
    [Key]
    [Column("StatusID")]
    public int StatusId { get; set; }

    [StringLength(30)]
    public string? StatusName { get; set; }

    [InverseProperty("Status")]
    public virtual ICollection<TBooking> TBookings { get; set; } = new List<TBooking>();
}
