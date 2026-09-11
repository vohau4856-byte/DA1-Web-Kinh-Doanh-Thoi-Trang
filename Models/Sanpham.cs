using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DA1CNTT.Models;

public partial class Sanpham
{
    public string IdSanpham { get; set; } = null!;

    public string? TenSanpham { get; set; }
   
    public decimal? Gia { get; set; }

    public int? SoLuongTonKho { get; set; }

    public string? IdDanhmuc { get; set; }

    public string? IdLoai { get; set; }

    public string? ImageUrl { get; set; }

    public string? Size { get; set; }

    public DateTime? CreateAt { get; set; }

    public string? Sex { get; set; }

    public virtual ICollection<Donhang> Donhangs { get; set; } = new List<Donhang>();

    public virtual Danhmucsanpham? IdDanhmucNavigation { get; set; }

    public virtual Loaisanpham? IdLoaiNavigation { get; set; }

    [ForeignKey(nameof(IdSanpham))]
    public virtual Sanpham? IdSanphamNavigation { get; set; }
}
