using System;
using System.Collections.Generic;

namespace DA1CNTT.Models;

public partial class Loaisanpham
{
    public string IdLoai { get; set; } = null!;

    public string? NameLoai { get; set; }

    public virtual ICollection<Sanpham> Sanphams { get; set; } = new List<Sanpham>();
}
