# 🛒 Website Kinh Doanh Sản Phẩm Quần Áo (Fashion E-Commerce)

> **Báo cáo Đồ án 1** - Ngành Công nghệ Thông tin  
> **Trường Đại học Nam Cần Thơ** (Khóa K10 - 08/2025)

---

## 📌 Giới thiệu dự án
Dự án **Website kinh doanh sản phẩm quần áo** là hệ thống thương mại điện tử giúp tối ưu hóa trải nghiệm mua sắm thời trang trực tuyến cho khách hàng, đồng thời hỗ trợ quản trị viên trong việc quản lý sản phẩm, đơn hàng và người dùng một cách hiệu quả.

---

## 👨‍💻 Thông tin thành viên & Giảng viên
* **Sinh viên thực hiện:**
  * Võ Trung Hậu (MSSV: `225635`)
  * Châu Nguyễn Trọng Hiếu (MSSV: `224426`)
* **Giảng viên hướng dẫn:** Cô Trương Thanh Thảo
* **Giảng viên phản biện:** Cô Bùi Thị Diễm Trinh

---

## 🛠️ Công nghệ & Công cụ sử dụng
* **Back-end:** ASP.NET Core Web App (C#)
* **Front-end:** HTML5, CSS3, JavaScript, Bootstrap
* **Cơ sở dữ liệu:** Microsoft SQL Server
* **Công cụ phát triển:** Visual Studio / Visual Studio Code, SQL Server Management Studio (SSMS), Git & GitHub

---

## 🌟 Các chức năng chính

### 👤 1. Dành cho Khách hàng (User)
* **Tài khoản:** Đăng ký, đăng nhập, thay đổi thông tin cá nhân, đổi mật khẩu.
* **Sản phẩm:** Xem danh sách, tìm kiếm, lọc theo loại/danh mục, sắp xếp giá, xem chi tiết sản phẩm.
* **Mua hàng:** Thêm/sửa/xóa sản phẩm trong giỏ hàng, tiến hành đặt hàng.
* **Thanh toán:** Hỗ trợ thanh toán khi nhận hàng (COD).

### 🛠️ 2. Dành cho Quản trị viên (Admin)
* **Quản lý người dùng:** Xem danh sách, thêm, sửa, xóa, phân quyền tài khoản.
* **Quản lý sản phẩm:** Quản lý danh mục, loại sản phẩm, cập nhật số lượng tồn kho và giá bán (CRUD).
* **Quản lý đơn hàng:** Xử lý và cập nhật trạng thái đơn hàng.

---

## 🗄️ Cấu trúc Cơ sở dữ liệu
Hệ thống sử dụng SQL Server gồm 9 bảng chính:
1. `Users` - Quản lý thông tin người dùng / admin.
2. `Sản Phẩm` - Lưu trữ thông tin quần áo.
3. `Loại Sản Phẩm` & `Danh Mục Sản Phẩm` - Phân loại hàng hóa.
4. `Giỏ Hàng` & `Chi Tiết Giỏ Hàng` - Lưu trữ trạng thái mua sắm của người dùng.
5. `Đơn Hàng` & `Chi Tiết Đơn Hàng` - Lưu trữ thông tin đơn hàng đã đặt.
6. `Thanh Toán` - Ghi nhận hình thức và trạng thái thanh toán.

---
