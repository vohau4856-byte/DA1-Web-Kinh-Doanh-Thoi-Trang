using System;
using System.Collections.Generic;

namespace DA1CNTT.Models;

public partial class Thanhtoan
{
    public string IdThanhtoan { get; set; } = null!;

    public string? IdGiohang { get; set; }

    public string? Tongtien { get; set; }

    public string? Phuongthuc { get; set; }

    public string? Trangthai { get; set; }

    public virtual Giohang? IdGiohangNavigation { get; set; }
}
