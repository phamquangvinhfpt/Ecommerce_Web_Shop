using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAn_LapTrinhWeb.Areas.Admin.Controllers
{
    public class DashBoardsController : BaseController
    {
        private readonly DbContext db = new DbContext();
        // GET: Admin/DashBoards
        public ActionResult Index()
        {
            ViewBag.Order = db.Orders.ToList();
            ViewBag.OrderDetail = db.Oder_Detail.ToList();
            ViewBag.ListOrderDetail = db.Oder_Detail.OrderByDescending(m => m.create_at).Take(3).ToList();
            ViewBag.ListOrder = db.Orders.Take(7).ToList();
            return View();
        }
    }
}