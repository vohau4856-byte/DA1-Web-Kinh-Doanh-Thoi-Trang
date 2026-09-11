using System;
using System.Collections.Generic;

namespace DA1CNTT.Models;

public partial class Giohang
{
    public string IdGiohang { get; set; } = null!;

    public string? IdUsers { get; set; }

    public string? Phuongthuc { get; set; }

    public string? Trangthai { get; set; }

    public DateOnly? NgayTao { get; set; }

    public virtual ICollection<Chitietgiohang> Chitietgiohangs { get; set; } = new List<Chitietgiohang>();

    public virtual User? IdUsersNavigation { get; set; }

    public virtual ICollection<Thanhtoan> Thanhtoans { get; set; } = new List<Thanhtoan>();
}
