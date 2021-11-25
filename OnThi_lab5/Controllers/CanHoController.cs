using Microsoft.AspNetCore.Mvc;
using OnThi_lab5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnThi_lab5.Controllers
{
    public class CanHoController : Controller
    {
        public IActionResult ThemCanHo()
        {
            return View();
        }
        [HttpPost]
        public string InsertCanHo(CanHoModel canho)
        {
            int count;
            DataContext context = HttpContext.RequestServices.GetService(typeof(OnThi_lab5.Models.DataContext)) as DataContext;
            count = context.sqlInsertCanHo(canho);
            if (count == 1)
            {
                return "Them thanh cong!";
            }
            return "Them that bai!";
        }
    }
}
