using System;
using System.Collections.Generic;

namespace SportCourtManagement.Models;

public partial class TSport
{
    public string SportId { get; set; } = null!;

    public string SportName { get; set; } = null!;

    public virtual ICollection<TCourt> TCourts { get; set; } = new List<TCourt>();
}
