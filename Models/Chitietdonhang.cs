using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DA1CNTT.Models;

public partial class Chitietdonhang
{
    public string IdOrder { get; set; } = null!;

    public string? IdDonhang { get; set; }

    public string? IdSanpham { get; set; }

    public int? Soluong { get; set; }

    public decimal? Giadat { get; set; }

    public virtual Donhang? IdDonhangNavigation { get; set; }

    [ForeignKey(nameof(IdSanpham))]
    public virtual Sanpham? IdSanphamNavigation { get; set; }
}
