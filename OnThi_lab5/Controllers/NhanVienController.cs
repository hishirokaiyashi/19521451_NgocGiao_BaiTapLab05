using Microsoft.AspNetCore.Mvc;
using OnThi_lab5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnThi_lab5.Controllers
{
    public class NhanVienController : Controller
    {
        public IActionResult LietKeNhanVienTheoSoLan()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ListByTimeNV(int solan)
        {
            DataContext context = HttpContext.RequestServices.GetService(typeof(OnThi_lab5.Models.DataContext)) as DataContext;
            return View(context.sqlListByTimeNhanVien(solan));
        }
        public IActionResult ListNhanVien()
        {
            DataContext context = HttpContext.RequestServices.GetService(typeof(OnThi_lab5.Models.DataContext)) as DataContext;
            return View(context.sqlListNhanVien());
        }
        public IActionResult LietKeThietBiCuaNV(string manhanvien)
        {
            DataContext context = HttpContext.RequestServices.GetService(typeof(OnThi_lab5.Models.DataContext)) as DataContext;
            return View(context.sqlListThietBiByNhanVien(manhanvien));
        }
        public IActionResult XoaThietBiByNV(string mathietbi)
        {
            DataContext context = HttpContext.RequestServices.GetService(typeof(OnThi_lab5.Models.DataContext)) as DataContext;
            int[] count = context.sqlDeleteThietBi(mathietbi);
            //if (count > 0)
            //{
                ViewData["Thông Báo"] = count;
            //}
            //else
                //ViewData["Thông Báo"] = "Xóa Thất Bại";
            return View();
        }
        public IActionResult XemThietBiByNV(string mathietbi)
        {
            DataContext context = HttpContext.RequestServices.GetService(typeof(OnThi_lab5.Models.DataContext)) as DataContext;
            return View(context.sqlViewThietBiByNhanVien(mathietbi));
        }
    }
}
