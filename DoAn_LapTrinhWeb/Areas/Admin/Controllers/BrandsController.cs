using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using DoAn_LapTrinhWeb.Common.Helpers;
using DoAn_LapTrinhWeb.Models;
using PagedList;

namespace DoAn_LapTrinhWeb.Areas.Admin.Controllers
{
    public class BrandsController : BaseController
    {
        private readonly DbContext _db = new DbContext();

        // GET: Areas/Brands
        public ActionResult Index(string search,int? size, int? page)
        {
            var pageSize = (size ?? 15);
            var pageNumber = (page ?? 1);
            ViewBag.search = search;
            var list = from a in _db.Brands
                orderby a.create_at descending
                select a;
            if (!string.IsNullOrEmpty(search))
            {
                list = from a in _db.Brands
                    where a.brand_name.Contains(search)
                    orderby a.create_at descending
                    select a;
            }
            return View(list.ToPagedList(pageNumber, pageSize));
        }


        [HttpPost]
        public JsonResult Create(string brandName, Brand brand)
        {
            string result = "false";
            try
            {
                Brand checkExist = _db.Brands.SingleOrDefault(m=>m.brand_name == brandName);
                if (checkExist != null)
                {
                    result = "exist";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                brand.brand_name = brandName;
                brand.create_by = User.Identity.GetEmail();
                brand.update_by = User.Identity.GetEmail();
                brand.create_at = DateTime.Now;
                brand.update_at = DateTime.Now;
                _db.Brands.Add(brand);
                _db.SaveChanges();
                result = "success";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Edit(int id, string brandName)
        {
            string result = "error";
            Brand brand = _db.Brands.FirstOrDefault(m=>m.brand_id==id);
            var checkExist = _db.Brands.SingleOrDefault(m => m.brand_name == brandName);
            try
            {
                if (checkExist != null)
                {
                    result = "exist";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                result = "success";
                brand.brand_name = brandName;
                brand.update_at = DateTime.Now;
                brand.update_by = User.Identity.GetEmail();
                _db.Entry(brand).State = EntityState.Modified;
                _db.SaveChanges();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Admin/ProductLogin/Delete/5
        public ActionResult Delete(int id)
        {
            string result = "error";
            Brand brand = _db.Brands.FirstOrDefault(m => m.brand_id == id);          
            try
            {
                result = "delete";
                _db.Brands.Remove(brand);
                _db.SaveChanges();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing) _db.Dispose();
            base.Dispose(disposing);
        }
    }
}