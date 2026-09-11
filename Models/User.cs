using System;
using System.Collections.Generic;

namespace DA1CNTT.Models;

public partial class User
{
    public string IdUsers { get; set; } = null!;

    public string? Tennguoidung { get; set; }

    public string? Username { get; set; }

    public string? Email { get; set; }

    public string? PasswordUsers { get; set; }

    public string? Phone { get; set; }

    public string? Diachi { get; set; }

    public string? ImgUrl { get; set; }

    public string? Roles { get; set; }

    public DateTime? CreateAt { get; set; }

    public virtual ICollection<Chitietgiohang> Chitietgiohangs { get; set; } = new List<Chitietgiohang>();

    public virtual ICollection<Donhang> Donhangs { get; set; } = new List<Donhang>();

    public virtual ICollection<Giohang> Giohangs { get; set; } = new List<Giohang>();
}
