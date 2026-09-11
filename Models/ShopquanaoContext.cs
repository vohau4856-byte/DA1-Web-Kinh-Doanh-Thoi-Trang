using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DA1CNTT.Models;

public partial class ShopquanaoContext : DbContext
{
    public ShopquanaoContext()
    {
    }

    public ShopquanaoContext(DbContextOptions<ShopquanaoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Chitietdonhang> Chitietdonhangs { get; set; }

    public virtual DbSet<Chitietgiohang> Chitietgiohangs { get; set; }

    public virtual DbSet<Danhmucsanpham> Danhmucsanphams { get; set; }

    public virtual DbSet<Donhang> Donhangs { get; set; }

    public virtual DbSet<Giohang> Giohangs { get; set; }

    public virtual DbSet<Loaisanpham> Loaisanphams { get; set; }

    public virtual DbSet<Sanpham> Sanphams { get; set; }

    public virtual DbSet<Thanhtoan> Thanhtoans { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-TGVNST0\\SQL2022;Initial Catalog=SHOPQUANAO;Persist Security Info=True;User ID=sa;Password=123456;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chitietdonhang>(entity =>
        {
            entity.HasKey(e => e.IdOrder).HasName("PK__CHITIETD__D23A85651D359493");

            entity.ToTable("CHITIETDONHANG");

            entity.Property(e => e.IdOrder)
                .HasMaxLength(20)
                .HasColumnName("ID_ORDER");
            entity.Property(e => e.Giadat)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("GIADAT");
            entity.Property(e => e.IdDonhang)
                .HasMaxLength(20)
                .HasColumnName("ID_DONHANG");
            entity.Property(e => e.IdSanpham)
                .HasMaxLength(20)
                .HasColumnName("ID_SANPHAM");
            entity.Property(e => e.Soluong).HasColumnName("SOLUONG");

            entity.HasOne(d => d.IdDonhangNavigation).WithMany(p => p.Chitietdonhangs)
                .HasForeignKey(d => d.IdDonhang)
                .HasConstraintName("FK__CHITIETDO__ID_DO__571DF1D5");
        });

        modelBuilder.Entity<Chitietgiohang>(entity =>
        {
            entity.HasKey(e => e.IdChitietGiohang).HasName("PK__CHITIETG__B8A79BF71210C3D3");

            entity.ToTable("CHITIETGIOHANG");

            entity.Property(e => e.IdChitietGiohang)
                .HasMaxLength(20)
                .HasColumnName("ID_CHITIET_GIOHANG");
            entity.Property(e => e.IdGiohang)
                .HasMaxLength(20)
                .HasColumnName("ID_GIOHANG");
            entity.Property(e => e.IdSanpham)
                .HasMaxLength(20)
                .HasColumnName("ID_SANPHAM");
            entity.Property(e => e.IdUsers)
                .HasMaxLength(20)
                .HasColumnName("ID_USERS");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("NGAY_TAO");
            entity.Property(e => e.Soluong).HasColumnName("SOLUONG");

            entity.HasOne(d => d.IdGiohangNavigation).WithMany(p => p.Chitietgiohangs)
                .HasForeignKey(d => d.IdGiohang)
                .HasConstraintName("FK__CHITIETGI__ID_GI__4E88ABD4");

            entity.HasOne(d => d.IdUsersNavigation).WithMany(p => p.Chitietgiohangs)
                .HasForeignKey(d => d.IdUsers)
                .HasConstraintName("FK_CHITIETGIOHANG_USERS");
        });

        modelBuilder.Entity<Danhmucsanpham>(entity =>
        {
            entity.HasKey(e => e.IdDanhmuc).HasName("PK__DANHMUCS__D547059E93A37B86");

            entity.ToTable("DANHMUCSANPHAM");

            entity.Property(e => e.IdDanhmuc)
                .HasMaxLength(20)
                .HasColumnName("ID_DANHMUC");
            entity.Property(e => e.NameDanhmuc)
                .HasMaxLength(50)
                .HasColumnName("NAME_DANHMUC");
        });

        modelBuilder.Entity<Donhang>(entity =>
        {
            entity.HasKey(e => e.IdDonhang).HasName("PK__DONHANG__49C6BA93E99569C7");

            entity.ToTable("DONHANG");

            entity.Property(e => e.IdDonhang)
                .HasMaxLength(20)
                .HasColumnName("ID_DONHANG");
            entity.Property(e => e.IdSanpham)
                .HasMaxLength(20)
                .HasColumnName("ID_SANPHAM");
            entity.Property(e => e.IdUsers)
                .HasMaxLength(20)
                .HasColumnName("ID_USERS");
            entity.Property(e => e.Ngaydathang)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("NGAYDATHANG");
            entity.Property(e => e.Trangthai)
                .HasMaxLength(20)
                .HasColumnName("TRANGTHAI");

            entity.HasOne(d => d.IdSanphamNavigation).WithMany(p => p.Donhangs)
                .HasForeignKey(d => d.IdSanpham)
                .HasConstraintName("FK_DONHANG_SANPHAM");

            entity.HasOne(d => d.IdUsersNavigation).WithMany(p => p.Donhangs)
                .HasForeignKey(d => d.IdUsers)
                .HasConstraintName("FK__DONHANG__ID_USER__5441852A");
        });

        modelBuilder.Entity<Giohang>(entity =>
        {
            entity.HasKey(e => e.IdGiohang).HasName("PK__GIOHANG__4A345124065A68CC");

            entity.ToTable("GIOHANG");

            entity.Property(e => e.IdGiohang)
                .HasMaxLength(20)
                .HasColumnName("ID_GIOHANG");
            entity.Property(e => e.IdUsers)
                .HasMaxLength(20)
                .HasColumnName("ID_USERS");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("NGAY_TAO");
            entity.Property(e => e.Phuongthuc)
                .HasMaxLength(100)
                .HasColumnName("PHUONGTHUC");
            entity.Property(e => e.Trangthai)
                .HasMaxLength(100)
                .HasColumnName("TRANGTHAI");

            entity.HasOne(d => d.IdUsersNavigation).WithMany(p => p.Giohangs)
                .HasForeignKey(d => d.IdUsers)
                .HasConstraintName("FK__GIOHANG__NGAY_TA__4AB81AF0");
        });

        modelBuilder.Entity<Loaisanpham>(entity =>
        {
            entity.HasKey(e => e.IdLoai).HasName("PK__LOAISANP__994CB9EAFD98668F");

            entity.ToTable("LOAISANPHAM");

            entity.Property(e => e.IdLoai)
                .HasMaxLength(20)
                .HasColumnName("ID_LOAI");
            entity.Property(e => e.NameLoai)
                .HasMaxLength(50)
                .HasColumnName("NAME_LOAI");
        });

        modelBuilder.Entity<Sanpham>(entity =>
        {
            entity.HasKey(e => e.IdSanpham).HasName("PK__SANPHAM__216A0553300395C5");

            entity.ToTable("SANPHAM");

            entity.Property(e => e.IdSanpham)
                .HasMaxLength(20)
                .HasColumnName("ID_SANPHAM");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATE_AT");
            entity.Property(e => e.Gia)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("GIA");
            entity.Property(e => e.IdDanhmuc)
                .HasMaxLength(20)
                .HasColumnName("ID_DANHMUC");
            entity.Property(e => e.IdLoai)
                .HasMaxLength(20)
                .HasColumnName("ID_LOAI");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(50)
                .HasColumnName("IMAGE_URL");
            entity.Property(e => e.Sex)
                .HasMaxLength(10)
                .HasColumnName("SEX");
            entity.Property(e => e.Size)
                .HasMaxLength(10)
                .HasColumnName("SIZE");
            entity.Property(e => e.SoLuongTonKho).HasColumnName("SO_LUONG_TON_KHO");
            entity.Property(e => e.TenSanpham)
                .HasMaxLength(50)
                .HasColumnName("TEN_SANPHAM");

            entity.HasOne(d => d.IdDanhmucNavigation).WithMany(p => p.Sanphams)
                .HasForeignKey(d => d.IdDanhmuc)
                .HasConstraintName("FK_SANPHAM_DANHMUCSANPHAM");

            entity.HasOne(d => d.IdLoaiNavigation).WithMany(p => p.Sanphams)
                .HasForeignKey(d => d.IdLoai)
                .HasConstraintName("FK_SANPHAM_LOAISANPHAM");
        });

        modelBuilder.Entity<Thanhtoan>(entity =>
        {
            entity.HasKey(e => e.IdThanhtoan).HasName("PK__THANHTOA__B8F0855B7C6DF6ED");

            entity.ToTable("THANHTOAN");

            entity.Property(e => e.IdThanhtoan)
                .HasMaxLength(20)
                .HasColumnName("ID_THANHTOAN");
            entity.Property(e => e.IdGiohang)
                .HasMaxLength(20)
                .HasColumnName("ID_GIOHANG");
            entity.Property(e => e.Phuongthuc)
                .HasMaxLength(100)
                .HasColumnName("PHUONGTHUC");
            entity.Property(e => e.Tongtien)
                .HasMaxLength(20)
                .HasColumnName("TONGTIEN");
            entity.Property(e => e.Trangthai)
                .HasMaxLength(20)
                .HasColumnName("TRANGTHAI");

            entity.HasOne(d => d.IdGiohangNavigation).WithMany(p => p.Thanhtoans)
                .HasForeignKey(d => d.IdGiohang)
                .HasConstraintName("FK__THANHTOAN__ID_GI__5CD6CB2B");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUsers).HasName("PK__USERS__1DDB35C3D5587889");

            entity.ToTable("USERS");

            entity.Property(e => e.IdUsers)
                .HasMaxLength(20)
                .HasColumnName("ID_USERS");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATE_AT");
            entity.Property(e => e.Diachi)
                .HasMaxLength(200)
                .HasColumnName("DIACHI");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("EMAIL");
            entity.Property(e => e.ImgUrl)
                .HasMaxLength(50)
                .HasColumnName("IMG_URL");
            entity.Property(e => e.PasswordUsers)
                .HasMaxLength(50)
                .HasColumnName("PASSWORD_USERS");
            entity.Property(e => e.Phone)
                .HasMaxLength(30)
                .HasColumnName("PHONE");
            entity.Property(e => e.Roles)
                .HasMaxLength(20)
                .HasDefaultValue("NGUOIDUNG")
                .HasColumnName("ROLES");
            entity.Property(e => e.Tennguoidung)
                .HasMaxLength(100)
                .HasColumnName("TENNGUOIDUNG");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("USERNAME");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
