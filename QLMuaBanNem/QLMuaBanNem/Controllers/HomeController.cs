using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PagedList;
using PagedList.Mvc;
using QLMuaBanNem.Models;

namespace QLMuaBanNem.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        QLBanNemDataContext dbQLBanNem = new QLBanNemDataContext();

        public ActionResult TrangChu()
        {
            return View();
        }
        public ActionResult GioiThieu()
        {
            return View();
        }
        public ActionResult LienHe()
        {
            return View();
        }
        public ActionResult HienThiThuongHieu()
        {
            List<ThuongHieu> lstThuongHieu = dbQLBanNem.ThuongHieus.ToList();
            return PartialView(lstThuongHieu);
        }
        public ActionResult HienThiLoai(string ms)
        {
            List<Loai> lstLoai = dbQLBanNem.Loais.ToList();
            List<Loai> lstLoaiSauKhiLoc = dbQLBanNem.Loais.Where(t => t.MaThuongHieu == int.Parse(ms)).ToList();
            return PartialView(lstLoaiSauKhiLoc);
        }
        public IEnumerable<SanPham> ListSanPham(int page, int pageSize)
        {
            return dbQLBanNem.SanPhams.ToPagedList(page, pageSize);
        }
        public ActionResult SanPham(int pageIndex = 1, int pageSize = 8)
        {
            var lstSanPham = ListSanPham(pageIndex, pageSize);
            return View(lstSanPham);
        }
        public ActionResult PhanLoaiSanPham(string maLoai)
        {
            List<SanPham> lstSanPhamSauKhiLoc = dbQLBanNem.SanPhams.Where(t => t.Loai == int.Parse(maLoai)).ToList();
            return View(lstSanPhamSauKhiLoc);
        }
        public ActionResult HienThiChiTiet(string ms)
        {
            SanPham sp = dbQLBanNem.SanPhams.FirstOrDefault(t => t.MaSanPham == int.Parse(ms));
            return View(sp);
        }
        public ActionResult TimKiem(string txtSearch)
        {
            List<SanPham> lstSanPham = dbQLBanNem.SanPhams.Where(x => x.TenSanPham.Contains(txtSearch)).ToList();
            if (lstSanPham == null)
                TempData["Error"] = "Khong tim thay san pham !";
            return View(lstSanPham);
        }
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult XLDangNhap(FormCollection c)
        {
            String username = c["txtTaiKhoan"].ToString().Trim();
            String password = c["txtPassword"].ToString().Trim();
            if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
            {
                TempData["LoiDangNhap"] = "Username and Password can not be blank.";
            }
            else
            {
                Account user = dbQLBanNem.Accounts.FirstOrDefault(m => m.TaiKhoan == username && m.MatKhau == password);
                if (user == null)
                {
                    TempData["LoiDangNhap"] = "Tai khoan hoac mat khau sai!";
                }
                else
                {
                    //if (user.TaiKhoan != username)
                    //{
                    //    TempData["LoiDangNhap"] = password;
                    //}
                    //else
                    //{
                    if (username.Equals("admin"))
                        return RedirectToAction("TrangChuADMIN", "Admin");
                    Session["TaiKhoanKH"] = username;
                    return RedirectToAction("TrangChu", "Home");
                    //}
                }
            }
            return RedirectToAction("DangNhap");
        }
        public ActionResult DangXuat()
        {
            Session["TaiKhoanKH"] = null;
            return Redirect("/");
        }
        public ActionResult QuenMatKhau()
        {
            return View();
        }

        [HttpPost]
        public ActionResult XLQuenMatKhau(FormCollection c)
        {
            var TaiKhoan = c["txtTaiKhoan"];
            var MatKhau = c["txtMatKhau"];
            var NhapLaiMatKhau = c["txtNhapLaiMatKhau"];
            var Email = c["txtEmail"];
            if (MatKhau != NhapLaiMatKhau)
                TempData["XacNhanMatKhau"] = "Nhap lai mat khau khong trung khop";
            else
            {
                KhachHang acc = dbQLBanNem.KhachHangs.FirstOrDefault(x => x.TaiKhoan == TaiKhoan);
                Account ac = dbQLBanNem.Accounts.FirstOrDefault(x => x.TaiKhoan == TaiKhoan);
                if (acc == null)
                    TempData["XacNhanMatKhau"] = "Tai khoan khong ton tai";
                else
                {
                    if (Email != acc.Email)
                        TempData["XacNhanMatKhau"] = "Email sai";
                    else
                    {
                        ac.MatKhau = MatKhau;
                        acc.MatKhau = MatKhau;
                        dbQLBanNem.SubmitChanges();
                        return RedirectToAction("DangNhap");
                    }
                }
            }
            return RedirectToAction("QuenMatKhau");
        }
        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult XLDangKy(FormCollection c)
        {
            String hoTen = c["name"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", c["birthday"]);
            var gioiTinh = c["sex"];
            var email = c["email"];
            var diaChi = c["Address"];
            var taiKhoan = c["username"];
            var matKhau = c["password"];
            var matKhauNhapLai = c["repassword"];

            KhachHang kh1 = dbQLBanNem.KhachHangs.FirstOrDefault(x => x.TaiKhoan == taiKhoan);
            KhachHang kh = new KhachHang();
            Account acc = new Account();
            if (kh1 != null)
                TempData["XLDangKy"] = "Tai khoan da ton tai";
            else
            {
                kh.HoTen = c["name"];
                kh.NgaySinh = DateTime.Parse(ngaysinh);
                kh.GioiTinh = gioiTinh;
                kh.Email = email;
                kh.DiaChi = diaChi;
                kh.TaiKhoan = taiKhoan;
                kh.MatKhau = matKhau;

                acc.TaiKhoan = taiKhoan;
                acc.MatKhau = matKhau;

                dbQLBanNem.KhachHangs.InsertOnSubmit(kh);
                dbQLBanNem.Accounts.InsertOnSubmit(acc);
                dbQLBanNem.SubmitChanges();

                TempData["XLDangKy"] = "Đăng ký thành công";
                return RedirectToAction("DangNhap");
            }
            return RedirectToAction("DangKy");
        }
    }
}
