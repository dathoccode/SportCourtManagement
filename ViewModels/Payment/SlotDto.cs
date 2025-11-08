using System;
using System.Collections.Generic;

namespace SportCourtManagement.ViewModels.Payment;

public partial class SlotDto
{
    public string? slotId { get; set; }
    public string? courtId { get; set; }
    public string? startTime { get; set; }
    public string? endTime { get; set; }
}
