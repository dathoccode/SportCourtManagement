using System;
using System.Collections.Generic;

namespace SportCourtManagement.Models;

public partial class TSlot
{
    public string SlotId { get; set; } = null!;

    public string CourtId { get; set; } = null!;

    public string? SlotType { get; set; }

    public string? SlotName { get; set; }

    public virtual TCourt Court { get; set; } = null!;
}
