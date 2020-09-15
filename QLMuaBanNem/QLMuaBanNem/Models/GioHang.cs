using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QLMuaBanNem.Models;

namespace QLMuaBanNem.Models
{
    public class GioHang
    {
        QLBanNemDataContext db = new QLBanNemDataContext();
        public int maSP { get; set; }
        public string tenSP { get; set; }
        public string hinh { get; set; }
        public int donGia { get; set; }
        public int soLuong { get; set; }
        public int thanhTien { get { return donGia * soLuong; } }

        public GioHang(string ms)
        {
            SanPham s = db.SanPhams.FirstOrDefault(t => t.MaSanPham == int.Parse(ms));

            maSP = int.Parse(ms);
            tenSP = s.TenSanPham;
            hinh = s.HinhAnh;
            donGia = (int)s.GiaBan;
            soLuong = 1;
        }
    }
}