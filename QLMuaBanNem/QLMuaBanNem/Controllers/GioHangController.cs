using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLMuaBanNem.Models;

namespace QLMuaBanNem.Controllers
{
    public class GioHangController : Controller
    {
        //
        // GET: /GioHang/
        QLBanNemDataContext dbQLBanNem = new QLBanNemDataContext();
        public ActionResult XemGioHang()
        {
            List<GioHang> lst = layGioHang();
            ViewBag.TongSoLuong = togSL();
            Session["SoLuongSPTrongGio"] = togSL();
            ViewBag.TongTien = togTien();
            return View(lst);
        }
        public List<GioHang> layGioHang()
        {
            List<GioHang> lst = Session["GH"] as List<GioHang>;
            if (lst == null)
            {
                lst = new List<GioHang>();
                Session["GH"] = lst;
            }
            return lst;
        }
        public ActionResult themGioHang(string ms)
        {
            List<GioHang> gh = layGioHang();

            GioHang a = gh.FirstOrDefault(x => x.maSP == int.Parse(ms));
            if (a == null)
            {
                GioHang b = new GioHang(ms);
                gh.Add(b);
            }
            else
            {
                a.soLuong++;
            }
            return RedirectToAction("XemGioHang");
        }
        public ActionResult xoaGioHang(string ms)
        {
            List<GioHang> gh = layGioHang();

            GioHang a = gh.FirstOrDefault(x => x.maSP == int.Parse(ms));
            if (a != null)
            {
                gh.Remove(a);
            }
            return RedirectToAction("XemGioHang");
        }
        public ActionResult xoaALLGioHang()
        {
            Session.Remove("GH");
            Session["SoLuongSPTrongGio"] = 0;
            return RedirectToAction("TrangChu","Home");
        }
        public int togSL()
        {
            int tong = 0;
            List<GioHang> lst = Session["GH"] as List<GioHang>;
            if (lst != null)
                tong = lst.Sum(x => x.soLuong);
            return tong;
        }
        public int togTien()
        {
            int tong = 0;
            List<GioHang> lst = Session["GH"] as List<GioHang>;
            if (lst != null)
                tong = lst.Sum(x => x.thanhTien);
            return tong;
        }
        public ActionResult DatHang(string ms)
        {
            String tk = Session["TaiKhoanKH"] as String;
            if (Session["TaiKhoanKH"] == null)
            {
                TempData["LoiDatHang"] = "Ban Chua Dang Nhap !";
                return RedirectToAction("XemGioHang");
            }
            ViewBag.TongTien = togTien();
            TempData["DatHangThanhCong"] = "Cam on ban da dat hang!";
            KhachHang kh = dbQLBanNem.KhachHangs.FirstOrDefault(x => x.TaiKhoan == tk);
            return View(kh);
        }

    }
}
