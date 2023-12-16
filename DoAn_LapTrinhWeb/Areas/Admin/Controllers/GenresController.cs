using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using DoAn_LapTrinhWeb.Common.Helpers;
using DoAn_LapTrinhWeb.Models;
using DoAn_LapTrinhWeb.Model;
using PagedList;

namespace DoAn_LapTrinhWeb.Areas.Admin.Controllers
{
    public class GenresController : BaseController
    {
        private readonly DbContext db = new DbContext();

        // GET: Areas/Brands
        public ActionResult Index(string search,int? size, int? page)
        {
            var pageSize = (size ?? 15);
            var pageNumber = (page ?? 1);
            ViewBag.search = search;
            var list = from a in db.Genres
                orderby a.create_at descending
                select a;
            if (!string.IsNullOrEmpty(search))
            {
                list = from a in db.Genres
                       where a.genre_name.Contains(search)
                    orderby a.create_at descending
                    select a;
            }
            return View(list.ToPagedList(pageNumber, pageSize));
        }


        [HttpPost]
        public JsonResult Create(string genreName, Genre genre)
        {
            string result = "false";
            try
            {
                Genre checkExist = db.Genres.SingleOrDefault(m=>m.genre_name == genreName);
                if (checkExist != null)
                {
                    result = "exist";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                genre.genre_name = genreName;
                genre.create_by = User.Identity.GetEmail();
                genre.update_by = User.Identity.GetEmail();
                genre.create_at = DateTime.Now;
                genre.update_at = DateTime.Now;
                db.Genres.Add(genre);
                db.SaveChanges();
                result = "success";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Edit(int id, string genreName)
        {
            string result = "error";
            Genre genre = db.Genres.FirstOrDefault(m=>m.genre_id==id);
            var checkExist = db.Genres.SingleOrDefault(m => m.genre_name == genreName);
            try
            {
                if (checkExist != null)
                {
                    result = "exist";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                result = "success";
                genre.genre_name = genreName;
                genre.update_at = DateTime.Now;
                genre.update_by = User.Identity.GetEmail();
                db.Entry(genre).State = EntityState.Modified;
                db.SaveChanges();
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
            Genre genre = db.Genres.FirstOrDefault(m => m.genre_id == id);          
            try
            {
                result = "delete";
                db.Genres.Remove(genre);
                db.SaveChanges();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}