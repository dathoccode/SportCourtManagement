using System;
using System.Collections.Generic;

namespace SportCourtManagement.Models;

public partial class TRole
{
    public int RoleId { get; set; }

    public string? RoleName { get; set; }

    public virtual ICollection<TAccount> TAccounts { get; set; } = new List<TAccount>();
}
