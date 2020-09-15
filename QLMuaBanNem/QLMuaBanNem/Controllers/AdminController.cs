using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLMuaBanNem.Models;

namespace QLMuaBanNem.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        QLBanNemDataContext db = new QLBanNemDataContext();
        public ActionResult TrangChuADMIN()
        {
            return View();
        }
        public ActionResult SanPhamADMIN()
        {
            List<SanPham> lstSP = db.SanPhams.ToList();
            return View(lstSP);
        }
        public ActionResult ThemSanPhamADMIN()
        {
            return View();
        }

        [HttpPost]
        public ActionResult themSanPham(FormCollection c)
        {
            SanPham sp = new SanPham();
            sp.MaSanPham = int.Parse(c["txtMaSanPham"]);
            sp.TenSanPham = c["txtTenSanPham"];
            db.SanPhams.InsertOnSubmit(sp);
            db.SubmitChanges();
            return RedirectToAction("SanPhamADMIN");
        }
        public ActionResult XoaSanPham(int x)
        {
            bool temp = xoaSanPham(x);
            return RedirectToAction("SanPhamADMIN");
        }

        [HttpDelete]
        public bool xoaSanPham(int id)
        {
            SanPham sp = db.SanPhams.FirstOrDefault(x => x.MaSanPham == id);
            db.SanPhams.DeleteOnSubmit(sp);
            db.SubmitChanges();
            return true;
        }
    }
}
