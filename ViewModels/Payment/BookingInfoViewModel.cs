

namespace SportCourtManagement.Models;

public partial class BookingInfoViewModel
{
    public TCourt? court { get; set; }
    public TAccount? account { get; set; }
    public TBooking? booking { get; set; }
    public List<TBookingDetail>? detail { get; set; }
    public List<TSlot>? slot { get; set; }
}
