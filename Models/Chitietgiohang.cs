using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DA1CNTT.Models;

public partial class Chitietgiohang
{
    public string IdChitietGiohang { get; set; } = null!;

    public string? IdGiohang { get; set; }

    public string? IdSanpham { get; set; }

    public int? Soluong { get; set; }

    public DateTime? NgayTao { get; set; }

    public string? IdUsers { get; set; }

    public virtual Giohang? IdGiohangNavigation { get; set; }

    public virtual User? IdUsersNavigation { get; set; }

    [ForeignKey(nameof(IdSanpham))]
    public virtual Sanpham? IdSanphamNavigation { get; set; }
}
