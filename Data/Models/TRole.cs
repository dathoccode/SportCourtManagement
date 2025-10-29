using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SportCourtManagement.Data.Models;

[Table("tRole")]
public partial class TRole
{
    [Key]
    [Column("RoleID")]
    public int RoleId { get; set; }

    [StringLength(20)]
    public string? RoleName { get; set; }

    [InverseProperty("Role")]
    public virtual ICollection<TAccount> TAccounts { get; set; } = new List<TAccount>();
}
