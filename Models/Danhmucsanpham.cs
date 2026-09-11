using System;
using System.Collections.Generic;

namespace DA1CNTT.Models;

public partial class Danhmucsanpham
{
    public string IdDanhmuc { get; set; } = null!;

    public string? NameDanhmuc { get; set; }

    public virtual ICollection<Sanpham> Sanphams { get; set; } = new List<Sanpham>();
}
