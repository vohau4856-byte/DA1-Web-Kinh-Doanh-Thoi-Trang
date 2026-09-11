using System;
using System.Collections.Generic;

namespace DA1CNTT.Models;

public partial class Donhang
{
    public string IdDonhang { get; set; } = null!;

    public string? IdUsers { get; set; }

    public DateTime? Ngaydathang { get; set; }

    public string? Trangthai { get; set; }

    public string? IdSanpham { get; set; }

    public virtual ICollection<Chitietdonhang> Chitietdonhangs { get; set; } = new List<Chitietdonhang>();

    public virtual Sanpham? IdSanphamNavigation { get; set; }

    public virtual User? IdUsersNavigation { get; set; }

}
