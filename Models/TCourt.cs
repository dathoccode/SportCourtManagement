using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SportCourtManagement.Models;

public partial class TCourt
{
    public string CourtId { get; set; } = null!;

    public string CourtName { get; set; } = null!;

    public string CourtAddress { get; set; } = null!;
    [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
    [RegularExpression(@"(0[3|5|7|8|9])+([0-9]{8})\b", ErrorMessage = "Số điện thoại không hợp lệ (Phải có 10 số và bắt đầu bằng 03, 05, 07, 08 hoặc 09).")]
    [MaxLength(10)]
    public string? Contact { get; set; }

    public string SportId { get; set; } = null!;

    public TimeOnly OpenTime { get; set; }

    public TimeOnly CloseTime { get; set; }

    public double? Latitude { get; set; }

    public double? Longtitude { get; set; }

    public string? Img { get; set; }

    public double? Rating { get; set; }

    public virtual TSport Sport { get; set; } = null!;

    public virtual ICollection<TBookingDetail> TBookingDetails { get; set; } = new List<TBookingDetail>();

    public virtual ICollection<TPrice> TPrices { get; set; } = new List<TPrice>();

    public virtual ICollection<TSlot> TSlots { get; set; } = new List<TSlot>();

    public virtual ICollection<TAccount> Accounts { get; set; } = new List<TAccount>();
}
