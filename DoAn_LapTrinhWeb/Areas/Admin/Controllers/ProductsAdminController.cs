using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DoAn_LapTrinhWeb.Common.Helpers;
using DoAn_LapTrinhWeb.DTOs;
using DoAn_LapTrinhWeb.Model;
using DoAn_LapTrinhWeb.Models;
using PagedList;

namespace DoAn_LapTrinhWeb.Areas.Admin.Controllers
{
    public class ProductsAdminController : BaseController
    {
        private readonly DbContext db = new DbContext();

        // GET: Admin/ProductsAdmin
        public ActionResult Index(string search, int? size, int? page) // hiển thị tất cả sp online
        {
            var pageSize = size ?? 15;
            var pageNumber = page ?? 1;
            ViewBag.search = search;
            ViewBag.countTrash = db.Products.Where(a => a.status == "0").Count(); //  đếm tổng sp có trong thùng rác
            var list = from a in db.Products
                join c in db.Genres on a.genre_id equals c.genre_id
                join d in db.Brands on a.brand_id equals d.brand_id
                join e in db.Discounts on a.disscount_id equals e.disscount_id
                where a.status == "1"
                orderby a.create_at descending // giảm dần
                select new ProductDTOs
                {
                    discount_start = (DateTime)e.discount_star,
                    discount_end = (DateTime)e.discount_end,
                    discount_name = e.discount_name,
                    discount_price = e.discount_price,
                    product_name = a.product_name,
                    quantity = a.quantity,
                    price = a.price,
                    Image = a.image,
                    genre_name = c.genre_name,
                    view = a.view,
                    brand_name = d.brand_name,
                    product_id = a.product_id
                };
            if (!string.IsNullOrEmpty(search))
            {
                 list = list.Where(s => s.product_name.Contains(search) || s.product_id.ToString().Contains(search));
            }
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Trash(string search, int? size, int? page) // hiển thị tất cả sp online
        {
            var pageSize = size ?? 15;
            var pageNumber = page ?? 1;
            ViewBag.search = search;
            var list = from a in db.Products
                       join c in db.Genres on a.genre_id equals c.genre_id
                       join d in db.Brands on a.brand_id equals d.brand_id
                       join e in db.Discounts on a.disscount_id equals e.disscount_id
                       where a.status == "0"
                       orderby a.create_at descending // giảm dần
                       select new ProductDTOs
                       {
                           discount_start = (DateTime)e.discount_star,
                           discount_end = (DateTime)e.discount_end,
                           discount_name = e.discount_name,
                           discount_price = e.discount_price,
                           product_name = a.product_name,
                           quantity = a.quantity,
                           price = a.price,
                           Image = a.image,
                           genre_name = c.genre_name,
                           view = a.view,
                           brand_name = d.brand_name,
                           product_id = a.product_id
                       };
            if (!string.IsNullOrEmpty(search))
            {
                list = list.Where(s => s.product_name.Contains(search) || s.product_id.ToString().Contains(search));
            }
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        // GET: Areas/ProductsAdmin/Details/5

        public ActionResult Details(int? id)
        {
            Product product = db.Products.FirstOrDefault(m => m.product_id == id);
            if (product == null)
            {
                Notification.setNotification1_5s("Không tồn tại! (ID = " + id + ")", "warning");
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Areas/ProductsAdmin/Create
        public ActionResult Create() //Tạo sản phẩm
        {
            ViewBag.ListDiscount =
                new SelectList(db.Discounts.OrderBy(m=>m.discount_price), "disscount_id", "discount_name", 0);
            ViewBag.ListBrand = new SelectList(db.Brands, "brand_id", "brand_name", 0);
            ViewBag.ListGenre = new SelectList(db.Genres, "genre_id", "genre_name", 0);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Product product,ProductImages productImage)
        {
            ViewBag.ListDiscount =
                new SelectList(db.Discounts.OrderBy(m => m.discount_price), "disscount_id", "discount_name", 0);
            ViewBag.ListBrand = new SelectList(db.Brands, "brand_id", "brand_name", 0);
            ViewBag.ListGenre = new SelectList(db.Genres, "genre_id", "genre_name", 0);
            try
            {
                if (product.ImageUpload != null)
                {
                    var fileName = Path.GetFileNameWithoutExtension(product.ImageUpload.FileName);
                    var extension = Path.GetExtension(product.ImageUpload.FileName);
                    fileName = fileName + DateTime.Now.ToString("HH-mm-dd-MM-yyyy") + extension;
                    product.image = "/Content/Images/" + fileName;
                    product.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/Images/"), fileName));
                }
                else
                {
                    Notification.setNotification3s("Vui lòng thêm Ảnh Thumbnail!", "error");
                    return View(product);
                }
                product.status = "1";
                product.view = 0;
                product.buyturn = 0;
                product.type = product.type;
                product.specifications = product.specifications;
                product.description = product.description;
                product.create_at = DateTime.Now;
                product.create_by = User.Identity.GetUserId().ToString();
                product.update_at = DateTime.Now;
                product.update_by = User.Identity.GetUserId().ToString();
                db.Products.Add(product);
                db.SaveChanges();
                foreach (HttpPostedFileBase image_multi in product.ImageUploadMulti)
                {
                    if (image_multi != null)
                    {
                        var fileName = Path.GetFileNameWithoutExtension(image_multi.FileName);
                        var extension = Path.GetExtension(image_multi.FileName);
                        fileName = fileName + DateTime.Now.ToString("HH-mm-dd-MM-yyyy") + extension;
                        productImage.image = "/Content/Images/" + fileName;
                        image_multi.SaveAs(Path.Combine(Server.MapPath("~/Content/Images/"), fileName));
                        productImage.product_id = product.product_id;
                        productImage.disscount_id = product.disscount_id;
                        productImage.genre_id = product.genre_id;
                        db.ProductImages.Add(productImage);
                        db.SaveChanges();
                    }
                }
                Notification.setNotification1_5s("Thêm mới thành công!", "success");
                return RedirectToAction("Index");
            }
            catch
            {
                Notification.setNotification1_5s("Lỗi", "error");
                return View(product);
            }
        }

        // GET: Areas/ProductsAdmin/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.ListDiscount = new SelectList(db.Discounts.OrderBy(m => m.discount_price), "disscount_id", "discount_name", 0);
            ViewBag.ListBrand = new SelectList(db.Brands, "brand_id", "brand_name", 0);
            ViewBag.ListGenre = new SelectList(db.Genres, "genre_id", "genre_name", 0);
            var product = db.Products.FirstOrDefault(x => x.product_id == id);
            if (product == null || id == null)
            {
                Notification.setNotification1_5s("Không tồn tại! (ID = " + id + ")", "warning");
                return RedirectToAction("Index");
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Product model, ProductImages productImage)
        {
            ViewBag.ListDiscount = new SelectList(db.Discounts.OrderBy(m => m.discount_price), "disscount_id", "discount_name", 0);
            ViewBag.ListBrand = new SelectList(db.Brands, "brand_id", "brand_name", 0);
            ViewBag.ListGenre = new SelectList(db.Genres, "genre_id", "genre_name", 0);
            var product = db.Products.SingleOrDefault(x => x.product_id == model.product_id);
            try
            {
                if (model.ImageUpload != null)
                {
                    var fileName = Path.GetFileNameWithoutExtension(model.ImageUpload.FileName);
                    var extension = Path.GetExtension(model.ImageUpload.FileName);
                    fileName = fileName + extension;
                    product.image = "/Content/Images/" + fileName;
                    model.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/Images/"), fileName));
                }
                product.product_name = model.product_name;
                product.quantity = model.quantity;
                product.description = model.description;
                product.specifications = model.specifications;
                product.price = model.price;
                product.brand_id = model.brand_id;
                product.type = model.type;
                product.update_at = DateTime.Now;
                product.update_by = User.Identity.GetUsername();
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                Notification.setNotification1_5s("Đã cập nhật lại thông tin!", "success");
                return RedirectToAction("Index");
            }
            catch
            {
                Notification.setNotification1_5s("Lỗi", "error");
                return View(model);
            }
        }
        public JsonResult Disable(int id)
        {
            string result = "error";
            Product product = db.Products.FirstOrDefault(m=>m.product_id ==id);
            try
            {
                result = "disabled";
                product.status = "0";
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Undo(int id)
        {
            string result = "error";
            Product product = db.Products.FirstOrDefault(m => m.product_id == id);
            try
            {
                result = "activate";
                product.status = "1";
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Delete(int id)
        {
            string result = "error";
            Product product = db.Products.FirstOrDefault(m => m.product_id == id);
            try
            {
                List<ProductImages> listImage = db.ProductImages.Where(m => m.product_id == id).ToList();
                foreach(var item in listImage)
                {
                    db.ProductImages.Remove(item);
                    db.SaveChanges();
                }
                result = "delete";
                db.Products.Remove(product);
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