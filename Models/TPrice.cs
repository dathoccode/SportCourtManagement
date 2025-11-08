using System;
using System.Collections.Generic;

namespace SportCourtManagement.Models;

public partial class TPrice
{
    public string CourtId { get; set; } = null!;

    public string SlotId { get; set; } = null!;

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public double UnitPrice { get; set; }

    public virtual TCourt Court { get; set; } = null!;
}
