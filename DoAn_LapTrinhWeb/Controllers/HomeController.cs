using DoAn_LapTrinhWeb.Common;
using DoAn_LapTrinhWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
// using DoAn_LapTrinhWeb.Models;


namespace DoAn_LapTrinhWeb.Controllers
{
    public class HomeController : Controller
    {
        private DbContext db = new DbContext();
        //Trang chủ
        public ActionResult Index()
        {
            ViewBag.AvgFeedback = db.Feedbacks.ToList();
            ViewBag.HotProduct = db.Products.Where(item => item.status == "1" && item.quantity != "0").OrderByDescending(item => item.buyturn + item.view).Take(8).ToList();
            ViewBag.NewProduct = db.Products.Where(item => item.status == "1" && item.quantity != "0").OrderByDescending(item => item.create_at).Take(8).ToList();
            ViewBag.Laptop = db.Products.Where(item => item.status == "1" && item.type == 1 && item.quantity != "0").OrderByDescending(item => item.buyturn + item.view).Take(8).ToList();
            ViewBag.Accessory = db.Products.Where(item => item.status == "1" && item.type == 2 && item.quantity != "0").OrderByDescending(item => item.buyturn + item.view).Take(8).ToList();
            ViewBag.OrderDetail = db.Oder_Detail.ToList();
            return View();       
        }
        //PageNotFound
        public ActionResult PageNotFound()
        {
            return View();
        }
    }
}