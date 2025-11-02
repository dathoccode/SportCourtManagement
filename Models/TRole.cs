using System;
using System.Collections.Generic;

namespace SportCourtManagement.Models;

public partial class TRole
{
    public string RoleId { get; set; } = null!;

    public string RoleName { get; set; } = null!;

    public virtual ICollection<TAccount> TAccounts { get; set; } = new List<TAccount>();
}
